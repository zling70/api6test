using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api6test.Models;

    public class ApiContext : DbContext
    {
        public ApiContext (DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<api6test.Models.Student>? Student { get; set; } //承担模型注册
        //日志工厂创建，控制台的输出
        public static readonly ILoggerFactory loggerFactory= LoggerFactory.Create(builder =>{builder.AddConsole();});// new LoggerFactory(new [] {new ConsoleLoggerProvider((_, __) => true,true)});
        //注册实体
        public DbSet<api6test.Models.Orderbill>? Orderbill { get; set; }
        public DbSet<api6test.Models.OrderItems>? OrderItems { get; set; }
        public DbSet<api6test.Models.Customer>? Customer { get; set; }
       
        protected override void OnModelCreating (ModelBuilder builder){
            base.OnModelCreating(builder);//调用父类构造方法，
            builder.Entity<OrderItems>().HasKey(o => new { o.linenum, o.billno });//配置复合主键
            //builder.Entity<Orderbill>().HasMany(m=>m.details).WithOne(x => x.orderbill).HasForeignKey(x => x.billno).OnDelete(DeleteBehavior.Cascade);  //注释掉了，当时目的是配置导航属性，后来发现，只需要在实体中配置就可以
            builder.Entity<Orderbill>().HasMany(m=>m.details);
            builder.Entity<OrderItems>().Property(p=>p.unitprice).HasColumnType("decimal(14,4)");//配置数据字段的精度，个人认为此处不是配置的最佳位置，在实体模型中标注更有好处，但是没有找到正确的方式
            builder.Entity<OrderItems>().Property(p=>p.quantities).HasColumnType("decimal(14,4)");
            builder.Entity<OrderItems>().Property(p=>p.amounts).HasColumnType("decimal(14,4)");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(loggerFactory);//配置日志记录输出到控制台
        }
    }
