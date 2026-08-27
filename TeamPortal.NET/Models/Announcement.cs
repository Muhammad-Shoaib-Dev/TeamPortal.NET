
using System.ComponentModel.DataAnnotations;

namespace TeamPortal.NET.Models
{

    public class Announcement
    {
        public int AnnouncementId { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        public DateTime? ExpiryDate { get; set; }
    }
}