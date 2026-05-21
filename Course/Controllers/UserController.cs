using Application.Dto.Request;
using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Course.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService accountService)
        {
            _userService = accountService;
        }

        public IActionResult Register()
        { 
            return View();
        }
        public IActionResult Login() { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            var res = await _userService.RegisterAsync(name, email,password);

            if (res)
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var res = await _userService.LoginAsync(email, password);

            if (res)
            {
                return RedirectToAction("PersonalPage", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();

            return RedirectToAction("Login", "User");
        }

        [HttpGet]
        public async Task<IActionResult> AdminPanel()
        {
            return View();
        }
    }
}
