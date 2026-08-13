using Microsoft.AspNetCore.Mvc;
using TeamPortal.NET.Data;
using TeamPortal.NET.Models;

namespace TeamPortal.NET.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DepartmentController(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public IActionResult Index()
        {
            IEnumerable<Department> departments = _context.Departments.ToList();
            return View(departments);
        }
    }
}
