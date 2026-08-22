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

        public async Task AddAsync(Employee employee)
        {
            await _context.AddAsync(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            _context.Update(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            _context.Remove(employee);
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _context.FindAsync<Employee>(id);
        }
    }
}