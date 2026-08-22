using TeamPortal.NET.Models;

namespace TeamPortal.NET.Repositries.IRepositries
{
    public interface IEmployeeRepositries
    {
        IQueryable<Employee> GetAllEmployees();
        Task AddAsync(Employee employee);
        void UpdateEmployee(Employee employee); 
        void DeleteEmployee(Employee employee);
        Task SaveChangesAsync();
        Task<Employee?> GetByIdAsync(int id);
    }
}
