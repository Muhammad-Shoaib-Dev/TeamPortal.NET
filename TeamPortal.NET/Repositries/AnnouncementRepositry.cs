using TeamPortal.NET.Data;
using TeamPortal.NET.Models;
using TeamPortal.NET.Repositries.IRepositries;


namespace TeamPortal.NET.Repositries
{
    public class AnnouncementRepositry : IAnnouncementRepositry
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementRepositry(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Announcement> GetAll()
        {
            return _context.Announcements
                .OrderByDescending(a => a.CreatedDate)
                .ToList();
        }

        public IEnumerable<Announcement> GetActiveAnnouncements()
        {
            return _context.Announcements
                .Where(a => a.IsActive && (a.ExpiryDate == null || a.ExpiryDate >= DateTime.Now))
                .OrderByDescending(a => a.CreatedDate)
                .ToList();
        }

        public Announcement GetById(int id)
        {
            return _context.Announcements.FirstOrDefault(a => a.AnnouncementId == id);
        }

        public void Add(Announcement announcement)
        {
            _context.Announcements.Add(announcement);
        }

        public void Update(Announcement announcement)
        {
            _context.Announcements.Update(announcement);
        }

        public void Delete(int id)
        {
            var announcement = GetById(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}