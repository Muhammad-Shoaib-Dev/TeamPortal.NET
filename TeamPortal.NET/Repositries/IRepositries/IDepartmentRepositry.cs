using TeamPortal.NET.Models;

namespace TeamPortal.NET.Repositries.IRepositries
{
    public interface IDepartmentRepositry
    {
        IQueryable<Department> GetAll();
    }
}
