using Application.Dto.Request;
using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Course.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService accountService)
        {
            _userService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            var res = await _userService.RegisterAsync(email,password);

            if (res)
            {
                return RedirectToAction("Login", "User");
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
            var res = await _userService.LoginAsync(email, password);

            if (res)
            {
                return RedirectToAction("Index", "Home");
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
        public IActionResult AccessDenied()
        {
            return StatusCode(403);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

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
            await _userService.DeleteUsersAsync(Ids);

            return StatusCode(204);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUsers([FromBody] string[] Ids)
        {
            await _userService.BlockUserAsync(Ids);

            return StatusCode(204);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnblockUsers([FromBody] string[] Ids)
        {
            await _userService.UnblockUserAsync(Ids);

            return StatusCode(204);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeAdmin([FromBody] AdminRoleRequest request)
        {
            await _userService.MakeAdminRoleAsync(request.Ids);

            return StatusCode(204);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveAdmin([FromBody] AdminRoleRequest request)
        {
            await _userService.RemoveAdminRoleAsync(request.Ids);

            return StatusCode(204);
        }
    }
}
