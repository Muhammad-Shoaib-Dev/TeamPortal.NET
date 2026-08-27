using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamPortal.NET.Models;
using TeamPortal.NET.Services.Interfaces;

namespace TeamPortal.NET.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AnnouncementController : Controller
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        public IActionResult Index()
        {
            var announcements = _announcementService.GetAll();
            return View(announcements);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Announcement announcement)
        {
            if (!ModelState.IsValid)
                return View(announcement);

            _announcementService.Add(announcement);
            TempData["Success"] = "Announcement created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var announcement = _announcementService.GetById(id);
            if (announcement == null) return NotFound();
            return View(announcement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Announcement announcement)
        {
            if (!ModelState.IsValid)
                return View(announcement);

            _announcementService.Update(announcement);
            TempData["Success"] = "Announcement updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var announcement = _announcementService.GetById(id);
            if (announcement == null) return NotFound();
            return View(announcement);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _announcementService.Delete(id);
            TempData["Success"] = "Announcement deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}