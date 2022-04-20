using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinAndroidApp.Models;

namespace XamarinAndroidApp.Services
{
    public interface IFirebaseDbService
    {
        Task AddUserInfo(User userDto);

        List<User> GetAllUsers();

        User GetCurrentUser();

        Task BanUser(string email);

        List<Processor> GetAllProcessors();

        Processor GetProcessorById(string id);

        Task AddProcessor(Processor ProcessorDto);

        Task UpdateProcessor(string id, Processor ProcessorDto);
    }
}