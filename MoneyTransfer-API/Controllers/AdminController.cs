using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTransfer_API.Models;
using MoneyTransfer_API.Services;
using System.Collections;
using System.Collections.Generic;

namespace MoneyTransfer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetAllUsers")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var userList = _adminService.GetAllUsers();
            return Ok(userList);
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("SortUsersByBalance")]
        public IActionResult SortUsersByBalance(double balance)
        {
            var sortedList = _adminService.SortUsersByBalance(balance);
            return Ok(sortedList);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("BlockUser")]
        public IActionResult BlockUser(int userId)
        {
            var existingUser = _adminService.BlockUser(userId);
            if(existingUser == null)
            {
                return BadRequest(new { Message = "There is no any User by this ID to block!" });
            }
            else
            {
                return Ok(new {Message = $"User: {existingUser.UserName} has blocked!"});
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("UnblockUser")]
        public IActionResult UnblockUser(int userId)
        {
            var existingUser = _adminService.UnblockUser(userId);
            if (existingUser == null)
            {
                return BadRequest(new { Message = "There is no any User by this ID to Unblock!" });
            }
            else
            {
                return Ok(new { Message = $"User: {existingUser.UserName} has unblocked!" });
            }
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetById")]
        public IActionResult GetById(int userId)
        {
            var existingUser = _adminService.GetById(userId);
            if( existingUser == null)
            {
                return NotFound("There is no any person by this ID!");
            }
            else
            {
                return Ok(existingUser);
            }
        }
    }
}
