using System.ComponentModel.DataAnnotations;
namespace api6test.Models
{
    public class Student
    {
         [Key]  //标记主键列
        public int Id {get;set;}
        public string? Stname {get; set;}
        public string? Gender {get; set;}        
    }
}