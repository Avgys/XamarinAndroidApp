using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinAndroidApp.Models;

namespace XamarinAndroidApp.Services
{
    public interface IFirebaseDbService<T>
    {
        Task AddUserInfo(User userDto);

        List<User> GetAllUsers();

        User GetCurrentUser();

        Task BanUser(string email);

        List<T> GetAllEntities();

        T GetEntityById(string id);

        Task AddEntity(T ProcessorDto);

        Task UpdateEntity(string id, T entityUpdate);
    }
}