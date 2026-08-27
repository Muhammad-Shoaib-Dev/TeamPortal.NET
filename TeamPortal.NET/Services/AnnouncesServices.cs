using TeamPortal.NET.Models;
using TeamPortal.NET.Repositries.IRepositries;
using TeamPortal.NET.Services.Interfaces;

namespace TeamPortal.NET.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepositry _repository;

        public AnnouncementService(IAnnouncementRepositry repository)
        {
            _repository = repository;
        }

        public IEnumerable<Announcement> GetAll() => _repository.GetAll();

        public IEnumerable<Announcement> GetActiveAnnouncements() => _repository.GetActiveAnnouncements();

        public Announcement GetById(int id) => _repository.GetById(id);

        public void Add(Announcement announcement)
        {
            _repository.Add(announcement);
            _repository.SaveChanges();
        }

        public void Update(Announcement announcement)
        {
            _repository.Update(announcement);
            _repository.SaveChanges();
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
            _repository.SaveChanges();
        }
    }
}