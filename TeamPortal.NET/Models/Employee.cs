using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamPortal.NET.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public long? PhoneNumber { get; set; }
        public string? Designation { get; set; }
        public int? Salary { get; set; }
        public DateTime? DateOfJoining { get; set; }
        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }
        public bool? IsActive { get; set; }

    }
}
