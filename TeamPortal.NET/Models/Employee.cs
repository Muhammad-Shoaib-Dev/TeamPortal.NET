using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamPortal.NET.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        public string? Email { get; set; }
        [Required]
        public long? PhoneNumber { get; set; }
        [Required]
        public string? Designation { get; set; }
        public int? Salary { get; set; }
        public DateTime? DateOfJoining { get; set; }
        [ForeignKey("DepartmentId")]
        public int DepartmentId { get; set; }
        public bool? IsActive { get; set; }
        public string? ProfilePicture { get; set; }

    }
}
