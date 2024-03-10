using System.Collections.Generic;
using Tasks.Models;

namespace Tasks.Interfaces{
    public interface ITaskService{
         List<Task> GetAll(long userId);
        Task GetTask(long userId,int id);
        void Post(long userId,Task t);
        void Delete(long userId,int id);
        bool Put(long userId,int id,Task t);
      //  int Count { get; }
    }
}