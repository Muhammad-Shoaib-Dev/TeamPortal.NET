using Microsoft.EntityFrameworkCore;
using TeamPortal.NET.Data;
using TeamPortal.NET.Models;
using TeamPortal.NET.Models.ViewModel;
using TeamPortal.NET.Services.Interfaces;

namespace TeamPortal.NET.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<PaginatedListVM<Employee>> GetEmployeesAsync(string Search, string Sort, string Department, string Designation, bool? isActive, decimal? minSalary, decimal? maxSalary, int pageIndex, int pageSize)
        {
            var employee = _context.Employees.Include(e => e.Department).AsQueryable();

            // Search logic
            if (!string.IsNullOrEmpty(Search))
            {
                bool Isnumeric = int.TryParse(Search, out int searchid);
                employee = employee.Where(e =>
                    (Isnumeric && e.EmployeeId == searchid) ||
                    e.Designation.Contains(Search) ||
                    e.FirstName.Contains(Search) ||
                    e.LastName.Contains(Search) ||
                    e.Email.Contains(Search));
            }

            // Filtering logic
            if (!string.IsNullOrEmpty(Department))
            {
                employee = employee.Where(e => e.Department.DepartmentName == Department);
            }
            if (!string.IsNullOrEmpty(Designation))
            {
                employee = employee.Where(e => e.Designation == Designation);
            }
            if (isActive.HasValue)
            {
                employee = employee.Where(e => e.IsActive == isActive.Value);
            }
            if (minSalary.HasValue)
            {
                employee = employee.Where(e => e.Salary >= minSalary.Value);
            }
            if (maxSalary.HasValue)
            {
                employee = employee.Where(e => e.Salary <= maxSalary.Value);
            }

            // Sorting logic
            switch (Sort)
            {
                case "name_desc":
                    employee = employee.OrderByDescending(e => e.FirstName).ThenByDescending(e => e.LastName);
                    break;
                case "email_asc":
                    employee = employee.OrderBy(e => e.Email);
                    break;
                case "email_desc":
                    employee = employee.OrderByDescending(e => e.Email);
                    break;
                case "designation_asc":
                    employee = employee.OrderBy(e => e.Designation);
                    break;
                case "designation_desc":
                    employee = employee.OrderByDescending(e => e.Designation);
                    break;
                case "department_asc":
                    employee = employee.OrderBy(e => e.Department.DepartmentName);
                    break;
                case "department_desc":
                    employee = employee.OrderByDescending(e => e.Department.DepartmentName);
                    break;
                default:
                    employee = employee.OrderBy(e => e.FirstName).ThenBy(e => e.LastName);
                    break;
            }
            return PaginatedListVM<Employee>.CreateAsync(employee, pageIndex, pageSize);
        }
    }
}
