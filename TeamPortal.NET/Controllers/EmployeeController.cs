using Microsoft.AspNetCore.Mvc;
using TeamPortal.NET.Data;
using TeamPortal.NET.Models;

namespace TeamPortal.NET.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext _context)
        {
          this._context = _context;
        }
        public IActionResult Index()
        {
            IEnumerable<Employee> employees = _context.Employees.ToList();
            return View(employees);
        }
        public IActionResult Details(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            if(ModelState.IsValid)
            {
                _context.Employees.Add(emp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emp);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {
                _context.Employees.Update(emp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emp);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Employee emp = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if (emp == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(emp);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
