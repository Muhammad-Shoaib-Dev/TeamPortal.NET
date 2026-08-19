using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public IActionResult Index(string Search , string Sort , string Department , string Designation ,bool? isActive ,
           Decimal? minSalary, Decimal? maxSalary)
        {
            ViewData["CurrentFilter"] = Search;

            ViewData["NameSortparam"] = string.IsNullOrEmpty(Sort) ? "name_desc" : "";
            ViewData["EmailSortparam"] = Sort == "email_asc" ? "email_desc" : "email_asc";
            ViewData["DesignationSortparam"] = Sort == "designation_asc" ? "designation_desc" : "designation_asc";
            ViewData["DepartmentIDSortparam"] = Sort == "department_asc" ? "department_desc" : "department_asc";

            var employee = _context.Employees.Include(e => e.Department).AsQueryable();
            // Search logic based on the provided search term
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
            //Filtering logic based on the provided parameters
            if (!string.IsNullOrEmpty(Department))
            {
               employee = employee.Where(e=>e.Department.DepartmentName == Department);
            }
            if (!string.IsNullOrEmpty(Designation))
            {
                employee = employee.Where(e => e.Designation == Designation);
            }
            if (isActive.HasValue)
            {
                employee= employee.Where(e => e.IsActive == isActive.Value);
            }
            if (minSalary.HasValue)
            {
                employee = employee.Where(e => e.Salary >= minSalary.Value);
            }
            if (maxSalary.HasValue)
            {
                employee = employee.Where(e => e.Salary <= maxSalary.Value);
            }
            //Sorting logic based on the Sort parameter
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
                    employee = employee.OrderBy(e => e.DepartmentId);
                    break;
                case "department_desc":
                    employee = employee.OrderByDescending(e => e.DepartmentId);
                    break;
                default:
                    employee = employee.OrderBy(e => e.FirstName).ThenBy(e => e.LastName);
                    break;
            }

            return View(employee.ToList());
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

            // Yeh block missing tha — dropdowns dobara set karein
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
