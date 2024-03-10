using Tasks.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Tasks.Controllers;


namespace Tasks.Services
{
 
 using Tasks.Models;

    public class TaskService:ITaskService
     {      
        List<Task> myTasks {get;}
        private IWebHostEnvironment  webHost;
        private string filePath;

        public TaskService(IWebHostEnvironment webHost)
        {
          
            this.webHost = webHost;

            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Task.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                myTasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            
       }
       
        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(myTasks));
        }

      public List<Task> GetAll(long UserId) => myTasks.Where(t => t.agentId == UserId).ToList();
       

       public Task GetTask(long UserId,int id) => myTasks.FirstOrDefault (t => t.id == id && t.agentId == UserId);
              
                      

      public bool Put(long UserId,int id, Task newTask)
    {
        var oldTask = myTasks.FirstOrDefault(t => t.id == id && t.agentId == UserId);
        var index = myTasks.IndexOf(oldTask);
        if (index == -1)
            return false;
        newTask.agentId=UserId;
        myTasks[index] = newTask;
        saveToFile();

        return true;

    } 

    public void Post(long UserId,Task newTask)
    {
        System.Console.WriteLine("dddd");
        System.Console.WriteLine(newTask.descreption,newTask.status,newTask);
        newTask.agentId=UserId;
        newTask.id =myTasks.Count();//myTasks.Count()+1;
        myTasks.Add(newTask);
        saveToFile();
    }

    public void Delete(long UserId,int id)
    {
        var temptask = GetTask( UserId,id);
        if (temptask is null)
            return;
        var oldTask = myTasks.FirstOrDefault(t => t.id == id);
        var index = myTasks.IndexOf(oldTask);
        myTasks.RemoveAt(index);
        myTasks.Remove(temptask);
        saveToFile();
    } 

    


    
}
}