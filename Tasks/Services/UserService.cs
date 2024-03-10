 using System.Collections.Generic;
    using Microsoft.AspNetCore.Hosting;
    using System.Text.Json;
    using Tasks.Services;
    using Tasks.Interfaces;
    using System.Linq;
    using System.IO;
    using System;
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Tasks.Controllers;

    
namespace User.Services
{
        using Tasks.Models;

   
    public class UserService : IUserService
    {
        List<User> users { get; }
        private IWebHostEnvironment webHost;
        private string filePath;
        public UserService(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Users.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }
        private void saveToFile()
        {
            
            File.WriteAllText(filePath, JsonSerializer.Serialize(users));
        }

        public List<User> GetAll() => users;

        public User Get(long UserId) => users?.FirstOrDefault( t => UserId==t.UserId);

        public void Post(User u)
        {
            u.UserId = users[users.Count()-1].UserId+1;
            u.TaskManager = false;
            users.Add(u);
            saveToFile();
        }

        public void Delete(long id)
        {
            var user = Get(id);
            if (user is null)
                return;
            users.Remove(user);
            saveToFile();
        }
    }

}
