using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tasks.Interfaces;
using Tasks.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;


namespace Tasks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly long UserId;
        private ITaskService TaskService;
        public TaskController(ITaskService taskService, IHttpContextAccessor httpContextAccessor)
        {
            this.TaskService=taskService;
            this.UserId = long.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value);

        }

       [HttpGet]
       [Authorize(Policy = "Agent")]
      public IEnumerable<Task> Get()
        {
        return this.TaskService.GetAll(UserId);
        }

       [HttpGet ("{id}")]
        [Authorize(Policy = "Agent")]
       public Task Get(int id) 
       {   
           return this.TaskService.GetTask(UserId,id);
    
       }   

       [HttpPut ("{id}")]
      public bool Put(int id,Task newTask)
    {
        return this.TaskService.Put(UserId,id,newTask);
      
    } 
    [HttpPost]
     [Authorize(Policy = "TaskManager")]
    public ActionResult Post([FromBody]Task newTask)
    {
        System.Console.WriteLine(newTask.descreption);
        this.TaskService.Post(UserId,newTask);
        return CreatedAtAction(nameof(Post), new { Id = newTask.id}, newTask);
        
    }

    [HttpDelete("{id}")]
     [Authorize(Policy = "TaskManager")]
    public void Delete(int id)
    {
        this.TaskService.Delete(UserId,id);
    
    }       
     }

}
