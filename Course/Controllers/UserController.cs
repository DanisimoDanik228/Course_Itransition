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

        [HttpGet]
        public IActionResult Register()
        { 
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
        public IActionResult AccessDenied()
        {
            return StatusCode(403);
        }

        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Json(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetEditorInventory(long idInventory)
        {
            var editors = await _userService.GetEditorInventoryAsync(idInventory);

            return Json(editors);
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
        public async Task<IActionResult> MakeAdmin([FromBody] string[] Ids)
        {
            await _userService.MakeAdminRoleAsync(Ids);

            return StatusCode(204);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveAdmin([FromBody] string[] Ids)
        {
            await _userService.RemoveAdminRoleAsync(Ids);

            return StatusCode(204);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> MakeEditor(long idInventory, [FromBody] string[] userIds)
        {
            await _userService.MakeEditorRoleAsync(userIds, idInventory);

            return StatusCode(204);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> RemoveEditor(long idInventory, [FromBody] string[] userIds)
        {
            await _userService.RemoveEditorRoleAsync(userIds, idInventory);

            return StatusCode(204);
        }
    }
}
