using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Services;

namespace XamarinAndroidApp.Droid.Services
{
    public class FirebaseDbService<T> : IFirebaseDbService<T> where T : class, IItem
    {
        private readonly FirebaseClient _databaseClient =
            new FirebaseClient("https://mobilki1-dd9f2-default-rtdb.europe-west1.firebasedatabase.app/");

        private string entityDir = "Processors";
        

        public async Task AddEntity(T enityDto)
        {
            await _databaseClient
                .Child(entityDir)
                .PostAsync(enityDto, false);
        }

        public IEnumerable<FirebaseObject<T>> GetAllEntities()
        {
            var taskGetAllProcessors = _databaseClient
                .Child(entityDir)
                .OnceAsync<T>();

            taskGetAllProcessors.Wait();

            if (taskGetAllProcessors.Exception != null)
            {
                Console.WriteLine(taskGetAllProcessors.Exception.Message);
                return null;
            }

            IEnumerable<FirebaseObject<T>> resultProcessors = taskGetAllProcessors.Result;
            return resultProcessors;
        }

        public FirebaseObject<T> GetEntityById(string id)
        {
            var taskGetAllProcessors = _databaseClient
                .Child(entityDir)
                .OnceAsync<T>();

            taskGetAllProcessors.Wait();

            if (taskGetAllProcessors.Exception != null)
            {
                Console.WriteLine(taskGetAllProcessors.Exception.Message);
                return null;
            }

            IEnumerable<FirebaseObject<T>> resultProcessors = taskGetAllProcessors.Result;

            return resultProcessors.Where(c => c.Object.Id == id).FirstOrDefault();
        }

        public FirebaseObject<T> GetEntityByPath(string path)
        {
            var taskGetProcessor = _databaseClient
                .Child(entityDir)
                .Child(path)
                .OnceAsync<T>();

            taskGetProcessor.Wait();

            if (taskGetProcessor.Exception != null)
            {
                Console.WriteLine(taskGetProcessor.Exception.Message);
                return null;
            }

            var resultProcessors = taskGetProcessor.Result.First();

            return resultProcessors;
        }

        public async Task UpdateEntity(string id, T entity)
        {
            var toUpdateProcessor = GetEntityById(id);

            await _databaseClient
                .Child(entityDir)
                .Child(toUpdateProcessor?.Key)
                .PutAsync(entity);
        }
    }
}