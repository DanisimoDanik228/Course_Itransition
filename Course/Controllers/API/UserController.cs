using Application.Service;
using Application.Service.Salesforce;
using Domain.Models.Salesforce;
using Elastic.Clients.Elasticsearch;
using Infrastructure.Service.Service.Salesforce;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Course.Controllers.API
{
    [ApiController]
    [Route("api/user/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ISalesforceService _salesforceService;

        public UserController(IUserService accountService,
            ISalesforceService salesforceService)
        {
            _userService = accountService;
            _salesforceService = salesforceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> UserByEmail(string email)
        {
            var user = await _salesforceService.GetContactByEmailAsync(email);

            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetEditorInventory(long idInventory, string? userName, string searchField)
        {
            var editors = await _userService.FindEditorInventoryAsync(idInventory, userName, searchField);

            return Ok(editors);
        }

        [HttpPatch]
        [Authorize(Roles = "Admin,Registered")]
        public async Task<IActionResult> UpdateContact(string id, string lastName, string description)
        {
            await _salesforceService.UpdateContactAsync(id, lastName, description);

            return StatusCode(204);
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
