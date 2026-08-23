using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamPortal.NET.Models.ViewModel;

namespace TeamPortal.NET.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("index", "Employee");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(" ", error.Description);
            }
            return View(model);

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (!result.Succeeded)
            {
                if (result.IsNotAllowed) ModelState.AddModelError("", "Not allowed - email confirmation required.");
                else if (result.IsLockedOut) ModelState.AddModelError("", "Account locked out.");
                else if (result.RequiresTwoFactor) ModelState.AddModelError("", "Requires 2FA.");
                else ModelState.AddModelError("", "Invalid login attempt (wrong email/password).");
                return View(model);
            }
            if (result.Succeeded)
            {
                return RedirectToAction("index", "Employee");
            }
            ModelState.AddModelError("", "Invalid login attempt.");

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
