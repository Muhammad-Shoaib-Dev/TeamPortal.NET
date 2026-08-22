using TeamPortal.NET.Data;
using TeamPortal.NET.Models;
using TeamPortal.NET.Repositries.IRepositries;

namespace TeamPortal.NET.Repositries
{
    public class DepartmentRepositry : IDepartmentRepositry
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepositry(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Department> GetAll()
        {
            return _context.Departments;
        }
    }
}
