using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamPortal.NET.Services;
using TeamPortal.NET.Services.Interfaces;

[Authorize]
public class HomeController : Controller
{
    private readonly IAnnouncementService _announcementService;

    public HomeController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    public IActionResult Index()
    {
        var announcements = _announcementService.GetActiveAnnouncements();
        return View(announcements);
    }
    public IActionResult Privacy()
    {
        return View();
    }
}