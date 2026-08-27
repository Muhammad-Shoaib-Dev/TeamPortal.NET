using TeamPortal.NET.Models;

namespace TeamPortal.NET.Repositries.IRepositries
{

public interface IAnnouncementRepositry
{
    IEnumerable<Announcement> GetActiveAnnouncements();
    IEnumerable<Announcement> GetAll();
    Announcement GetById(int id);
    void Add(Announcement a);
    void Update(Announcement a);
    void Delete(int id);
    void SaveChanges();
}
}