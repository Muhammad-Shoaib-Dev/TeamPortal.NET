using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
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
        public IActionResult Index(string Search)
        {
            ViewData["CurrentFilter"] = Search;
            var employee = _context.Employees.AsQueryable();
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
            return View(employee.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Employee emp, IFormFile profileImage)
        {
            if (!ModelState.IsValid)
            {
                return View(emp);  
            }

            if (profileImage != null && profileImage.Length > 0)
            {
                string FileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName);
                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Employee");

                if (!Directory.Exists(FilePath))
                    Directory.CreateDirectory(FilePath);

                string ImagePath = Path.Combine(FilePath, FileName);
                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }

                emp.ProfilePicture = "/images/Employee/" + FileName;
            }

            _context.Employees.Add(emp);
            _context.SaveChanges();
            return RedirectToAction("Index");
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
        public async Task<IActionResult> Edit(Employee emp, IFormFile profileImage)
        {
            ModelState.Remove("ProfilePicture");   

            if (ModelState.IsValid)
            {
                if (profileImage != null && profileImage.Length > 0)
                {
                    string FileName = Guid.NewGuid().ToString() + Path.GetExtension(profileImage.FileName);
                    string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Employee");

                    if (!Directory.Exists(FilePath))
                        Directory.CreateDirectory(FilePath);

                    var existingEmployee = _context.Employees.AsNoTracking()
                        .FirstOrDefault(e => e.EmployeeId == emp.EmployeeId);

                    string ImagePath = Path.Combine(FilePath, FileName);
                    using (var stream = new FileStream(ImagePath, FileMode.Create))
                    {
                        await profileImage.CopyToAsync(stream);
                    }

                    if (existingEmployee != null && !string.IsNullOrEmpty(existingEmployee.ProfilePicture))
                    {
                        string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                            existingEmployee.ProfilePicture.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                        if (System.IO.File.Exists(oldImagePath))
                            System.IO.File.Delete(oldImagePath);
                    }

                    emp.ProfilePicture = "/images/Employee/" + FileName;
                }
                else
                {
                    var existingEmployee = _context.Employees.AsNoTracking()
                        .FirstOrDefault(e => e.EmployeeId == emp.EmployeeId);
                    if (existingEmployee != null)
                    {
                        emp.ProfilePicture = existingEmployee.ProfilePicture;
                    }
                }

                _context.Employees.Update(emp);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emp);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Employee? emp = _context.Employees.Find(id);
            if (emp == null)
            {
                return NotFound();
            }
            return View(emp);

        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int id,IFormFile profileImage)
        {
            var existingEmployee = _context.Employees.AsNoTracking()
                        .FirstOrDefault(e => e.EmployeeId == id);
            if (existingEmployee != null && !string.IsNullOrEmpty(existingEmployee.ProfilePicture))
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                    existingEmployee.ProfilePicture.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }
            Employee? emp = _context.Employees.Find(id);
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
