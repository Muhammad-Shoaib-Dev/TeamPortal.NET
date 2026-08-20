using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamPortal.NET.Data;
using TeamPortal.NET.Models;
using TeamPortal.NET.Services.Interfaces;

namespace TeamPortal.NET.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext _context , IEmployeeService _employeeService)
        {
          this._context = _context;
          this._employeeService = _employeeService;
        }
        public async Task<IActionResult> Index(string Search, string Sort, string Department, string Designation, bool? isActive,
        Decimal? minSalary, Decimal? maxSalary , int? pageIndex)
        {
            ViewData["CurrentFilter"] = Search;

            ViewData["NameSortparam"] = string.IsNullOrEmpty(Sort) ? "name_desc" : "";
            ViewData["EmailSortparam"] = Sort == "email_asc" ? "email_desc" : "email_asc";
            ViewData["DesignationSortparam"] = Sort == "designation_asc" ? "designation_desc" : "designation_asc";
            ViewData["DepartmentIDSortparam"] = Sort == "department_asc" ? "department_desc" : "department_asc";

            ViewData["Departments"] = new SelectList(_context.Departments, "DepartmentName", "DepartmentName", Department);
            ViewData["Designations"] = new SelectList(new List<string> { "Team Manager", "HR", "Developer", "Designer", "Tester", "Intern" }, Designation);
            ViewData["isActive"] = isActive;
            ViewData["minSalary"] = minSalary;
            ViewData["maxSalary"] = maxSalary;

            int pageSize = 5;
            var result = await _employeeService.GetEmployeesAsync(
                Search, Sort, Department, Designation, isActive, minSalary, maxSalary, pageIndex ?? 1, pageSize);

            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            
            ViewData["Departments"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["Designations"] = new SelectList(new List<string> { "Team Manager","HR", "Developer", "Designer","Tester","Intern"});
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Employee emp, IFormFile profileImage)
        {
            ModelState.Remove("Department");
            if(profileImage != null && profileImage.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(profileImage.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("profileImage", "Invalid file type. Please upload a JPG, JPEG, PNG, or GIF file.");
                }
                if (profileImage.Length > 2 * 1024 * 1024) 
                {
                    ModelState.AddModelError("profileImage", "File size exceeds the 2MB limit.");
                }
            }
            if (!ModelState.IsValid)
            {
                ViewData["Departments"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
                ViewData["Designations"] = new SelectList(new List<string> { "Team Manager", "HR", "Developer", "Designer", "Tester", "Intern" });
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
            ViewData["Departments"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["Designation"] = new SelectList(new List<string> { "Manager", "HR", "Developer", "Designer" });
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
            ModelState.Remove("Department");
            if (profileImage != null && profileImage.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(profileImage.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("profileImage", "Invalid file type. Please upload a JPG, JPEG, PNG, or GIF file.");
                }
                if (profileImage.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("profileImage", "File size exceeds the 2MB limit.");
                }
            }

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

            ViewData["Departments"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", emp.DepartmentId);
            ViewData["Designation"] = new SelectList(new List<string> { "Manager", "HR", "Developer", "Designer" }, emp.Designation);
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
