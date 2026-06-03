using Application.Dto.Request;
using Application.Service;
using Application.Service.Salesforce;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Course.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly ISalesforceService _salesforceService;

        public UserController(IUserService accountService,
            IAuthenticationService authenticationService,
            ISalesforceService salesforceService)
        {
            _userService = accountService;
            _authenticationService = authenticationService;
            _salesforceService = salesforceService;
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

            return View("Register","Invalid login or password");
        }


        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var res = await _userService.LoginAsync(email, password);

            if (res)
            {
                return RedirectToAction("PersonalPage", "Home");
            }

            return View("Login","Invalid login or password");
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
            var myId = _authenticationService.MyId();
            if (!await _authenticationService.IsAdminAsync(myId))
            {
                return RedirectToAction("MainPage", "Home");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            return View(await _salesforceService.GetContactByIdAsync(id));
        }
    }
}
