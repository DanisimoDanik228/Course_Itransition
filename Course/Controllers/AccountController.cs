using Course.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Course.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Registered");

                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login() { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return StatusCode(403);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllUsers()
        {
            var users = _userManager.Users.ToList(); 
            var userRolesViewModel = new List<UserRequest>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var responseUser = new UserRequest{ Email = user.Email, Role = roles };

                userRolesViewModel.Add(responseUser);
            }

            return View(userRolesViewModel);
        }
    }
}
