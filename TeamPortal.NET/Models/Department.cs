using System.ComponentModel.DataAnnotations;

namespace TeamPortal.NET.Models
{
    public class Department
    {
        [Key]
        public string? DepartmentId{get;set;}
        public string? DepartmentName { get; set; }
    }
}
