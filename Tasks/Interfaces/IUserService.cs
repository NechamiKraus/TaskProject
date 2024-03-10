
namespace Tasks.Interfaces
{
    using Tasks.Models;
    using System.Collections.Generic;
    public interface IUserService
    {
        List<User> GetAll();
        User Get(long UserId);
        void Post(User t);
        void Delete(long Id);
    }
}
