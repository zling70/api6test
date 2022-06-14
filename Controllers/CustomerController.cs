using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api6test.Models;

namespace api6test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ApiContext _context;

        public CustomerController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
        {
            //严谨的逻辑是先执行空值的判断,zling,'!'操作符消除了空值检查
            // if(!object.ReferenceEquals(_context,null)&&!object.ReferenceEquals(_context.Customer,null)){
            //     return await _context!.Customer!.ToListAsync();
            // }            
            // return null;
            return await _context.Customer!.ToListAsync();
        }

        // GET: api/Customer/c01  //zling Customer的主键id为 customer=c01 这个字段
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(string id)
        {
            var customer = await _context.Customer!.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customer/c01
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // zling 如何调用本方法，参考最下方部分的说明和代码
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(string id, Customer customer)
        {
            if (id != customer.customer)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();//js客户端得到的是一个undefined值
        }

        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customer!.Add(customer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.customer))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCustomer", new { id = customer.customer }, customer);
        }

        // DELETE: api/Customer/c01
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var customer = await _context.Customer!.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(string id)
        {
            return _context.Customer!.Any(e => e.customer == id);
        }
    }
}

//zling 关于客户端如何访问本api中的put请求
//put请求方法中有两个参数客户id,及被修改的客户对象Customer实例
//客户端ajax的PUT请求案例：
// const getCustomer=function(){
//     let cust={}
//     const custid='c02'
//     const customer=JSON.stringify({
//         "linenum": 2,
//         "customer": "c02",
//         "custname": "关小羽",
//         "custaddress": "湘潭"
//         })
//     $.ajax({
//         url:'https://127.0.0.1:5001/api/Customer/'+custid,
//         type:'PUT',
//         datatype:'json',
//         contentType:'application/json;charset=UTF-8',
//         data:customer,
//         success:function(res){
//             console.log(res)  //输出undefined
//         }
//     })
// }