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
            var res = await _accountService.RegisterAsync(email,password);

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
            var res = await _accountService.LoginAsync(email, password);

            if (res)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();

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
            var users = await _accountService.GetAllUsersAsync();

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
            await _accountService.DeleteUsersAsync(Ids);

            return StatusCode(204);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUsers([FromBody] string[] Ids)
        {
            await _accountService.BlockUserAsync(Ids);

            return StatusCode(204);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnblockUsers([FromBody] string[] Ids)
        {
            await _accountService.UnblockUserAsync(Ids);

            return StatusCode(204);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SetStatus([FromBody] SetStatusRequest request)
        {
            await _accountService.SetUserStatusAsync(request.Ids, request.Status);

            return StatusCode(204);
        }
    }
}
