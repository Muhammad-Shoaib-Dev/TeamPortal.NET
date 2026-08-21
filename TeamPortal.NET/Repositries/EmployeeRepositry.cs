using Microsoft.EntityFrameworkCore;
using TeamPortal.NET.Data;
using TeamPortal.NET.Models;
using TeamPortal.NET.Repositries.IRepositries;

namespace TeamPortal.NET.Repositries
{
    public class EmployeeRepositry : IEmployeeRepositries
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepositry(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Employee> GetAllEmployees()
        {
            return _context.Employees.Include(e => e.Department); 
        }
    }
}
