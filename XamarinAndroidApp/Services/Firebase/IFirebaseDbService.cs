using Firebase.Database;
using System.Collections.Generic;
using System.Threading.Tasks;
using XamarinAndroidApp.Models;

namespace XamarinAndroidApp.Services
{
    public interface IFirebaseDbService<T>
    {
        IEnumerable<FirebaseObject<T>> GetAllEntities();

        FirebaseObject<T> GetEntityById(string id);

        FirebaseObject<T> GetEntityByPath(string path);

        Task AddEntity(T ProcessorDto);

        Task UpdateEntity(string id, T entityUpdate);
    }
}