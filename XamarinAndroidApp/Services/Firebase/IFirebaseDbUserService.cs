using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinAndroidApp.Models;

namespace XamarinAndroidApp.Services.Firebase
{
    public interface IFirebaseDbUserService
    {        
        Task AddUserInfo(User userDto);

        IEnumerable<User> GetAllUsers();

        User GetCurrentUser();

        Task BanUser(string email);
    }
}