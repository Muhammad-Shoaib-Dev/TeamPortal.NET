using TeamPortal.NET.Models;
using TeamPortal.NET.Models.ViewModel;

namespace TeamPortal.NET.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<PaginatedListVM<Employee>> GetEmployeesAsync(string Search, string Sort, string Department, string Designation, bool? isActive,
    Decimal? minSalary, Decimal? maxSalary, int pageIndex , int pageSize);
    }
}
