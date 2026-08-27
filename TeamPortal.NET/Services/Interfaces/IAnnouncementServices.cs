
using TeamPortal.NET.Models;
using TeamPortal.NET.Models.ViewModel;

namespace TeamPortal.NET.Services.Interfaces
{
    public interface IAnnouncementService
    {
        IEnumerable<Announcement> GetAll();
        IEnumerable<Announcement> GetActiveAnnouncements();
        Announcement GetById(int id);
        void Add(Announcement announcement);
        void Update(Announcement announcement);
        void Delete(int id);
    }
}