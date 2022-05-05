using System.Data;
using System.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace api6test.Models
{
    [Table("customer")]
    public class Customer
    {
        public int  linenum {get;set;}
        [Key]  //标记主键列b       
        public string customer {get; set;}
        public string? custname {get; set;}
        public string? custaddress {get; set;}         
    }
}