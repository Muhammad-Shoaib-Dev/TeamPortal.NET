using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TeamPortal.NET.Data;
using TeamPortal.NET.Models;
using TeamPortal.NET.Repositries.IRepositries;
using TeamPortal.NET.Services.Interfaces;

namespace TeamPortal.NET.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IDepartmentRepositry _departmentRepositry;
        private readonly IEmployeeRepositries _employeeRepositries;
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IDepartmentRepositry _departmentRepositry, IEmployeeRepositries _employeeRepositries, IEmployeeService _employeeService)
        {
          this._departmentRepositry = _departmentRepositry;
          this._employeeRepositries = _employeeRepositries;
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

            ViewData["Departments"] = new SelectList(_departmentRepositry.GetAll(), "DepartmentId", "DepartmentName", Department);
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
            
            ViewData["Departments"] = new SelectList(_departmentRepositry.GetAll(), "DepartmentId", "DepartmentName");
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
                ViewData["Departments"] = new SelectList(_departmentRepositry.GetAll(), "DepartmentId", "DepartmentName");
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
            
            await _employeeRepositries.AddAsync(emp);
            await _employeeRepositries.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewData["Departments"] = new SelectList(_departmentRepositry.GetAll(), "DepartmentId", "DepartmentName");
            ViewData["Designation"] = new SelectList(new List<string> { "Manager", "HR", "Developer", "Designer" });
            var employee = _employeeRepositries.GetByIdAsync(id).Result;
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

                    var existingEmployee = _employeeRepositries.GetAllEmployees().AsNoTracking()
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
                    var existingEmployee = _employeeRepositries.GetAllEmployees().AsNoTracking()
                        .FirstOrDefault(e => e.EmployeeId == emp.EmployeeId);
                    if (existingEmployee != null)
                    {
                        emp.ProfilePicture = existingEmployee.ProfilePicture;
                    }
                }

                _employeeRepositries.UpdateEmployee(emp);
                await _employeeRepositries.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewData["Departments"] = new SelectList(_departmentRepositry.GetAll(), "DepartmentId", "DepartmentName", emp.DepartmentId);
            ViewData["Designation"] = new SelectList(new List<string> { "Manager", "HR", "Developer", "Designer" }, emp.Designation);
            return View(emp);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Employee? emp = _employeeRepositries.GetByIdAsync(id).Result;
            if (emp == null)
            {
                return NotFound();
            }
            return View(emp);

        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id,IFormFile profileImage)
        {
            var existingEmployee = _employeeRepositries.GetAllEmployees().AsNoTracking()
                        .FirstOrDefault(e => e.EmployeeId == id);
            if (existingEmployee != null && !string.IsNullOrEmpty(existingEmployee.ProfilePicture))
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                    existingEmployee.ProfilePicture.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }
            Employee? emp = _employeeRepositries.GetByIdAsync(id).Result;
            if (emp == null)
            {
                return NotFound();
            }
            _employeeRepositries.DeleteEmployee(emp);
            await _employeeRepositries.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}