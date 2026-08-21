using TeamPortal.NET.Models;

namespace TeamPortal.NET.Repositries.IRepositries
{
    public interface IEmployeeRepositries
    {
        IQueryable<Employee> GetAllEmployees();
    }
}
