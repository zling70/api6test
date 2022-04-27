using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace api6test.Models
{
    [Table("Orderbill")]
    public class Orderbill
    {
        [Key]
        public string billno {get;set;}        //单据编号
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]//似乎没有起到效果2021-08-27
        public DateTime billdate {get;set;}     //单据日期
        public string? customer {get;set;}       //客户名称
        public string? customeraddress {get;set;}      //客户地址
        //在简单案例后，扩展为一个复杂订单页面的crud，增加新的属性
        public string? billstatus {get;set;}//varchar(10),--单据审核状况
        public string? remark {get;set;}//varchar(200),--备注
        public string?     currid {get;set;}//varchar(50), --币别
        public string?     billstylename {get;set;}//varchar(50),--单据类型
        public string?     maker {get;set;}//varchar(20),--制单人员
        public string?     permitter {get;set;}//varchar(20),--审核人员
        public string?     priceofoftax {get;set;}//varchar(20),--单价是否含税
        public string?     salesid {get;set;}//varchar(20),--业务员
        public string?     departid {get;set;}//varchar(20) --部门(编号)
        public ICollection<OrderItems>? details {get;set;} 
    }
}