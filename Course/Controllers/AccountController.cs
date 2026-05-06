using Application.Dto.Request;
using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Course.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            var res = await _accountService.Register(email,password);

            if (res)
            {
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
            var res = await _accountService.Login(email, password);

            if (res)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return StatusCode(403);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _accountService.GetAllUsers();

            return Json(users);
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            return View();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUsers([FromBody] string[] Ids)
        {
            await _accountService.DeleteUsers(Ids);

            return StatusCode(204);
        }
    }
}
