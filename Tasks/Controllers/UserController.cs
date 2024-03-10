using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tasks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using Tasks.Services;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace Tasks.Controllers
{
    using Tasks.Models;

    [ApiController]
    [Route("[controller]")]
    public class userController : ControllerBase
    {

        private readonly long userId;

        IUserService userService;
        public userController(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            this.userService = userService;
            this.userId = long.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value);

        }
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Policy = "TaskManager")]
        public ActionResult<List<User>> GetAll() => userService.GetAll();

        [HttpGet]
        [Route("GetMyUser")]
        [Authorize(Policy = "Agent")]
        public ActionResult<User> GetMyUser()
        {
            var user = userService.Get(userId);
            if (user == null)
                return NotFound();
            return user;
        }

        // [HttpPost("{user}")]
        [HttpPost]
        [Authorize(Policy = "TaskManager")]
        public ActionResult Post([FromBody] User user)
        {
            userService.Post(user);
            return CreatedAtAction(nameof(Post), new { Id = user.UserId}, user);
        }

        [HttpDelete]
        [Authorize(Policy = "TaskManager")]
        public ActionResult Delete(int id)
        {
            var user = userService.Get(id);
            if (user == null)
                return NotFound();
            userService.Delete(id);
            return NoContent();
        }

    }

}