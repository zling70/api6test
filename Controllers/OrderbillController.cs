using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api6test.BLL;
using api6test.Models;

namespace api6test.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderbillController:ControllerBase
    {
        private readonly OrderbillService _oser;

        public OrderbillController(OrderbillService oser)//构造方法注入
        {
            _oser = oser;
        }
        // GET: api/Orderbill
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orderbill>>> GetOrderbills()
        {
            return await _oser.getAll();
        }
         [HttpGet]
        public async Task<Orderbill> GetEnd()
        {
            return await _oser.GetEnd();
        }
        // POST: api/Orderbill
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<int> PostOrder(Orderbill orderbill)
        {
            Task<int> rs=_oser.PostOrderbill(orderbill);  
            return await rs;
        }
        [HttpDelete]
        public async Task<int> delOrder(Orderbill bill){
            var rs=await _oser.DelOrder(bill!.billno!);
            return  rs; 
        }

        // [HttpDelete]
        // public  int delOrder(string billno){
        //     Console.WriteLine("删除的请求："+billno);
        //     //var rs=await _oser.DelOrder(billno.billno);
        //     //return  rs;
        //     return  0 ; 
        // }

         [HttpGet]
        public async Task<Orderbill> Nextbill(string billno)
        {
            //这里想实现单条分页的查询操作，算法是：bill对象带一个参数，-1向前找，1向后找
            //string billno=bill.billno;
            Console.WriteLine("获得的请求参数是："+billno);
            //var res = await _oser.GetOrder(billno,1);
            var res = await _oser.GetOrder_efc(billno,1);
            return  res;
        }

         [HttpGet]
        public async Task<Orderbill> Prebill(string billno)
        {
            //var res = await _oser.GetOrder(billno,-1);
            var res = await _oser.GetOrder_efc(billno,-1);
            return  res;
        }
    }

    //************下面是整体被注释的，是完整的OrderbillController的class定义，被上面代码替换，多引入了业务逻辑层
    //************可以将下面的代码整体放开，注释掉所有上面的代码，也重样可以运行。
    // [Route("api/[controller]")]
    // [ApiController]
    // public class OrderbillController : ControllerBase
    // {
    //     private readonly ApiContext _context;

    //     public OrderbillController(ApiContext context)
    //     {
    //         _context = context;
    //     }

    //     // GET: api/Orderbill
    //     [HttpGet]
    //     public async Task<ActionResult<IEnumerable<Orderbill>>> GetOrderbill()
    //     {

    //         var tms = await _context.OrderItems.ToListAsync();
    //         //测试中一直不能查询到导航属性中的子数据，无意中写了上面一行代码，在测试中返回值就可以得到导航数据了，这是什么原因？
    //         //应该有更优雅的实现方式
    //         return await _context.Orderbill.ToListAsync();
    //     }

    //     // GET: api/Orderbill/5
    //     [HttpGet("{id}")]
    //     public async Task<ActionResult<Orderbill>> GetOrderbill(string id)
    //     {
    //         var orderbill = await _context.Orderbill.FindAsync(id);

    //         if (orderbill == null)
    //         {
    //             return NotFound();
    //         }
    //         _context.Entry(orderbill).Collection(b => b.details).Load();
    //         return orderbill;
    //     }

    //     // PUT: api/Orderbill/5
    //     // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //     [HttpPut("{id}")]
    //     public async Task<IActionResult> PutOrderbill(string id, Orderbill orderbill)
    //     {
    //         if (id != orderbill.billno)
    //         {
    //             return BadRequest();
    //         }

    //         _context.Entry(orderbill).State = EntityState.Modified;

    //         try
    //         {
    //             await _context.SaveChangesAsync();
    //         }
    //         catch (DbUpdateConcurrencyException)
    //         {
    //             if (!OrderbillExists(id))
    //             {
    //                 return NotFound();
    //             }
    //             else
    //             {
    //                 throw;
    //             }
    //         }

    //         return NoContent();
    //     }

    //     // POST: api/Orderbill
    //     // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    //     [HttpPost]
    //     public async Task<ActionResult<Orderbill>> PostOrderbill(Orderbill orderbill)
    //     {
    //         _context.Orderbill.Add(orderbill);
    //         try
    //         {
    //             await _context.SaveChangesAsync();
    //         }
    //         catch (DbUpdateException)
    //         {
    //             if (OrderbillExists(orderbill.billno))
    //             {
    //                 return Conflict();
    //             }
    //             else
    //             {
    //                 throw;
    //             }
    //         }

    //         return CreatedAtAction("GetOrderbill", new { id = orderbill.billno }, orderbill);
    //     }

    //     // DELETE: api/Orderbill/5
    //     [HttpDelete("{id}")]
    //     public async Task<IActionResult> DeleteOrderbill(string id)
    //     {
    //         var orderbill = await _context.Orderbill.FindAsync(id);
    //         if (orderbill == null)
    //         {
    //             return NotFound();
    //         }

    //         _context.Orderbill.Remove(orderbill);
    //         await _context.SaveChangesAsync();

    //         return NoContent();
    //     }

    //     private bool OrderbillExists(string id)
    //     {
    //         return _context.Orderbill.Any(e => e.billno == id);
    //     }
    // }
}
