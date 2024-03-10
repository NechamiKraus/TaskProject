using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tasks.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using Tasks.Services;


namespace Tasks.Controllers
{
    using Tasks.Models;
    [ApiController]
    [Route("[controller]")]
    public class TaskManagerController: ControllerBase
    {
        private List<User> users;
                IUserService userService;

        public TaskManagerController(IUserService userService)
        {
             this.userService = userService;
            this.users = userService.GetAll();
        }
        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] User User)
        {
            System.Console.WriteLine(User.Username);
            var dt = DateTime.Now;

            var user = users.FirstOrDefault(u =>
                u.Username == User.Username 
                && u.Password == User.Password
            );        

            if (user == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim("UserType", user.TaskManager ? "TaskManager" : "Agent"),
                new Claim("userId", user.UserId.ToString()),

            };
            if(user.TaskManager)
                claims.Add(new Claim("UserType","Agent"));



            var token = TaskTokenService.GetToken(claims);

            return new OkObjectResult(TaskTokenService.WriteToken(token));
        }
    }
}