using System.ComponentModel.DataAnnotations;

namespace TeamPortal.NET.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId{get;set;}
        public string? DepartmentName { get; set; }
    }
}
