using System.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using api6test.Models;

namespace api6test.BLL
{
    public class OrderbillService
    {
        private readonly ApiContext _context;

        public OrderbillService(ApiContext context)
        { //构造方法被系统调用时，执行注入
            _context = context;
        }

        //简单单据的查询操作，对应前端页面Orderbill.html
        public async Task<ActionResult<IEnumerable<Orderbill>>> getAll()
        {
            List<OrderItems> tms = await _context.OrderItems!.ToListAsync();
            //测试中一直不能查询到导航属性中的子数据，无意中写了上面一行代码，在测试中返回值就可以得到导航数据了，这是什么原因？
            //应该有更优雅的实现方式
            List<Orderbill> ods = await _context.Orderbill!.ToListAsync();
            return ods;
        }

        //根据单据编号删除数据，没有判断是否已经审核
        public async Task<int> DelOrder(string billno)
        {
            var status = 0;
            Orderbill? od = await _context.Orderbill!.FindAsync(billno);
            _context.Entry(od!).Collection(b => b.details!).Load(); //加载明细
            //删除明细，代码删除比配置级联删除，个人认为有优势
            foreach (OrderItems itm in od!.details!)
            {
                _context.OrderItems!.Remove(itm);
            }
            _context.Remove(od);
            status = await _context.SaveChangesAsync();
            return status;
        }

        //简单单据的新增、修改操作，对应前端页面Orderbill.html(简易版)，或bill.html（完整版）
        public async Task<int> PostOrderbill(Orderbill orderbill)
        {
            var status = 0;
            //所传过来的参数orderbill对象，与数据库的关系比较复杂：订单编号在实体表中不存在，将执行新增操作，如果存在，订单主表执行新增操作，明细表
            //中的操作包含：1）删除了某一个明细行，2）新增了若干行，3）修改了某个订单明细的字段，所以，总结就是：删除所有明细，将新的明细重新添加一遍。
            Orderbill? rsbill = await _context.Orderbill!.FindAsync(orderbill.billno); //经测试，不会查出关联明细数据
            var customer = await _context.Customer!.FindAsync(orderbill.cust!.customer);//找到关联的客户对象
            if (rsbill != null)
            {
                //执行更新操作
                Console.WriteLine("执行更新");
                //先执行明细的删除操作，
                _context.Entry(rsbill).Collection(b => b.details!).Load(); //加载明细

                foreach (OrderItems oit in rsbill.details!)
                {
                    _context.Remove(oit); //删除明细
                }
                EntityState s = _context.Entry(rsbill).State; //此时检测到rsbill是被跟踪到的
                EntityState s2 = _context.Entry(orderbill).State; //而orderbill是Detached的，表示独立的孤独的，或称为游离态的
                rsbill.details = orderbill.details;
                //采用一种笨办法，对主单其它字段进行修改，关联的客户涉及到外表，后面专门处理
                rsbill.billdate = orderbill.billdate;
                rsbill.customeraddress = orderbill.customeraddress;
                rsbill.billstatus = orderbill.billstatus;
                rsbill.remark = orderbill.remark;
                rsbill.currid = orderbill.currid;
                rsbill.billstylename = orderbill.billstylename;
                rsbill.maker = orderbill.maker;
                rsbill.permitter = orderbill.permitter;
                rsbill.priceofoftax = orderbill.priceofoftax;
                rsbill.salesid = orderbill.salesid;
                rsbill.departid = orderbill.departid;
                //var tm = _context.Orderbill.Attach(orderbill);//此时执行这行代码将报错，原因如下：
                //The instance of entity type 'Orderbill' cannot be tracked because another instance with the same key value for {'billno'} is already being tracked.
                //_context.Update(rsbill);
                //因为前端单据可能更新了客户，传过来新的客户编号，根据编号找到客户，设置到主单关联的客户对象
                
                if (customer == null)
                {
                    //抛出异常，没有找到关联的客户，程序返回
                }
                rsbill.cust = customer; //更新客户
            }
            else
            {
                Console.WriteLine("执行插入操作");
                orderbill.cust=customer;//设置与客户的关系
                _context.Orderbill.Add(orderbill);
            }

            //以下的方法能实现的逻辑是，能对主详表进行同时的更新，但是，如果出现订单明细的增删操作，和字段的修改操作，就不能达成要求
            //订单明细界面新增了一行，同时删除了头两行，那么真完成不了这个订单更新操作
            // EntityState s=_context.Entry(orderbill).State;//鉴别新传递过来的实体数据是否一个与持久化层有关联的对象
            // _context.Update(orderbill);                   //更新数据
            // EntityState st=_context.Entry(orderbill).State;//再次鉴别对象状态
            status = await _context.SaveChangesAsync(); //最后修改/保存对象到数据库。
            return status;
            //EF Core提供了SaveChange方法，可以把数据操作好之后再统一调用SaveChange方法，这样就实现了简单的事务功能
        }

        //完整订单操作
        //查找最后一条数据，但，是通过查找所有以后取最后一条进行返回，意图是测试熟悉
        public async Task<Orderbill> GetEnd()
        {
            string odnum = await _context.Orderbill!.MaxAsync(o => o.billno!);
            var od = _context.Orderbill!.FindAsync(odnum);
            _context!.Entry(od.Result!).Reference("cust").Load(); //查询主单时，加载关联的外键表数据
            _context!.Entry(od.Result!).Collection(b => b.details!).Load(); //查询主单时，加载关联的外键表集合
            return od.Result!;
        }

        public async Task<Orderbill> GetOrder(string billno, int i = 1) //目的是想找到某一个单号的前一条/或后一条最近的记录，
        {
            //select max(billno) from orderbill where billno<'20210818006'
            //select min(billno) from orderbill where billno>'20210818001'
            //select max(billno) from orderbill where billno<billno  //if i<=0
            //select min(billno) from orderbill where billno>billno  //if i>0
            //var sql = "select min(billno) from orderbill where billno>{0}";
            //var res = await _context.Database.ExecuteSqlRawAsync(sql,new[]{billno});
            //var res = _context.Orderbill.FromSqlRaw(sql,new[]{billno});
            string bno = "";
            if (i >= 1)
            {
                bno = await _context.Orderbill!
                    .FromSqlInterpolated($"SELECT * FROM dbo.orderbill where billno<{billno}")
                    .AsNoTracking()
                    .MaxAsync(p => p.billno!);
                //var bills = _context.Orderbill.FromSqlInterpolated($"SELECT * FROM dbo.orderbill");
                //zling 在EFCore的体系下执行原生SQL
                //1.执行查询原生sql（FromSqlInterpolated）(关键词搜索可以找到用法)
                //2.执行非查询sql（ExecuteSqlInterpolatedAsync）
                bno =
                    null == bno || bno.Equals("")
                        ? await _context.Orderbill!.MaxAsync(p => p.billno!)
                        : bno; //如果找到了最后，已经没有单据了，就又从头开始
            }
            else
            {
                bno = await _context.Orderbill!
                    .FromSqlInterpolated($"SELECT * FROM dbo.orderbill where billno>{billno}")
                    .AsNoTracking()
                    .MinAsync(p => p.billno!);
                bno =
                    null == bno || bno.Equals("")
                        ? await _context.Orderbill!.MinAsync(p => p.billno!)
                        : bno;
            }
            Orderbill? odbill = await _context.Orderbill!.FindAsync(bno);
            _context.Entry(odbill!).Reference("cust").Load(); //查询主单时，加载关联的外键表数据
            _context.Entry(odbill!).Collection(b => b.details!).Load();
            //var od = _context.Orderbill.FindAsync(odnum);
            //_context.Entry(od.Result).Collection(b => b.details).Load();
            return odbill!; //od.Result;
        }
        public async Task<Orderbill> GetOrder_efc(string billno, int i = 1) //另一种实现方式，使用Linq而非sql
        {
            Orderbill? odbill = null;
            
            if (i >= 1)
            {
                odbill = await _context.Orderbill!.OrderBy(b=>b.billno).FirstOrDefaultAsync(b=>b.billno!.CompareTo(billno)>0);
                if(null == odbill) {
                    //zling 直接查询单号最小的单据，所报错误是 'DbSet<Orderbill>() .OrderBy(o => o.billno) .Min()' could not be translated
                    //也就是不能翻译成sql语句，同学们可以自己另外实现这个功能（语法熟悉）
                    //odbill = await _context.Orderbill!.OrderBy(b=>b.billno).MinAsync();//运行时报错的语句
                    //否则，这里先执行查找最小单据号码，根据最小单据号码再执行单据查询，需要两次访问数据库，不是最优方案
                    string? odb = await _context.Orderbill!.MinAsync(b=>b.billno);
                    odbill =await _context.Orderbill!.OrderBy(b=>b.billno).FirstOrDefaultAsync(b=>b.billno!.Equals(odb));
                }
                   
            }else{
                odbill = await _context.Orderbill!.OrderByDescending(b=>b.billno).FirstOrDefaultAsync(b=>b.billno!.CompareTo(billno)<0);
                if(null == odbill) {
                    string? odb = await _context.Orderbill!.MaxAsync(b=>b.billno);
                    odbill =await _context.Orderbill!.OrderBy(b=>b.billno).FirstOrDefaultAsync(b=>b.billno!.Equals(odb));
                }                
                   

            }
            //Orderbill? odbill = await _context.Orderbill!.FindAsync(bno);
            _context.Entry(odbill!).Reference("cust").Load(); //查询主单时，加载关联的外键表数据
            _context.Entry(odbill!).Collection(b => b.details!).Load();
            //var od = _context.Orderbill.FindAsync(odnum);
            //_context.Entry(od.Result).Collection(b => b.details).Load();
            return odbill!; //od.Result;
        }
    }
}
