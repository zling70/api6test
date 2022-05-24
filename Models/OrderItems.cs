using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace api6test.Models
{
    [Table("OrderItems")]
    public class OrderItems
    {
        [Key]
        [Column(Order=1)]
        public int linenum { get; set; } //int,--行号
        [Key]
        [Column(Order=2)]
        [ForeignKey("Orderbill.billno")]
        public string? billno { get; set; } //varchar(50),--单据编号
        public string? code { get; set; } //varchar(50),--物料编号
        public string? name { get; set; } //varchar(100),--物料名称
        public string? unitof { get; set; } //varchar(20),--计量单位
        //[Column]
        //[DecimalPrecision(14,4)]
        public decimal? unitprice  { get; set; } //decimal(14,4),--单价
        public decimal? quantities { get; set; } //decimal(14,4),--数量
        public decimal? amounts { get; set; } //decimal(14,4),--总金额
        public string? specs { get; set; } //varchar(50),--规格型号

        //public Orderbill orderbill {get;set;}
    
    }
}