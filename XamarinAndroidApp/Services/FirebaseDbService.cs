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
    public class FirebaseDbService<T> : IFirebaseDbService<T>
    {
        private readonly FirebaseClient _databaseClient =
            new FirebaseClient("https://mobilki1-dd9f2-default-rtdb.europe-west1.firebasedatabase.app/");

        private string processorDir = "Processors";
        private string usersDir = "Users";

        public async Task AddUserInfo(User userDto)
        {
            await _databaseClient
                .Child(usersDir)
                .PostAsync(userDto);
        }

        public List<User> GetAllUsers()
        {
            var taskGetAllUsers = _databaseClient
                .Child(usersDir)
                .OnceAsync<User>();

            taskGetAllUsers.Wait();

            if (taskGetAllUsers.Exception != null)
            {
                Console.WriteLine(taskGetAllUsers.Exception.Message);
                return null;
            }

            IEnumerable<FirebaseObject<User>> resultUsers = taskGetAllUsers.Result;

            return resultUsers.Select(item => new User
            {
                Email = item.Object.Email,
                IsAdmin = item.Object.IsAdmin,
                IsBlocked = item.Object.IsBlocked
            }).ToList();
        }

        public User GetCurrentUser()
        {
            string currentUserEmail = FirebaseAuth.Instance.CurrentUser.Email;

            var taskGetAllUsers = _databaseClient
                .Child(usersDir)
                .OnceAsync<User>();

            taskGetAllUsers.Wait();

            if (taskGetAllUsers.Exception != null)
            {
                Console.WriteLine(taskGetAllUsers.Exception.Message);
                return null;
            }

            IEnumerable<FirebaseObject<User>> resultUsers = taskGetAllUsers.Result;

            return resultUsers.Select(item => new User
            {
                Email = item.Object.Email,
                IsAdmin = item.Object.IsAdmin,
                IsBlocked = item.Object.IsBlocked
            }).First(u => u.Email == currentUserEmail);
        }

        public async Task BanUser(string email)
        {
            var userToBan = (await _databaseClient
                .Child(usersDir)
                .OnceAsync<User>()).FirstOrDefault(u => u.Object.Email == email);

            var newUser = new User
            {
                Email = email,
                IsAdmin = false,
                IsBlocked = true
            };

            await _databaseClient
                .Child(usersDir)
                .Child(userToBan?.Key)
                .PutAsync(newUser);
        }

        public async Task AddEntity(T enityDto)
        {
            await _databaseClient
                .Child(processorDir)
                .PostAsync(enityDto);
        }

        public List<T> GetAllEntities()
        {
            var taskGetAllProcessors = _databaseClient
                .Child(processorDir)
                .OnceAsync<T>();

            taskGetAllProcessors.Wait();

            if (taskGetAllProcessors.Exception != null)
            {
                Console.WriteLine(taskGetAllProcessors.Exception.Message);
                return null;
            }

            IEnumerable<FirebaseObject<T>> resultProcessors = taskGetAllProcessors.Result;
            return resultProcessors.;//.Select(item => new T
            //{
            //    Id = item.Object.Id,
            //    Name = item.Object.Name,
            //    Description = item.Object.Description,
            //    Socket = item.Object.Socket,
            //    IsMultiThreading = item.Object.IsMultiThreading,
            //    CoresCount = item.Object.CoresCount,
            //    Frequency = item.Object.Frequency,
            //    CodeName = item.Object.CodeName,
            //    TDP = item.Object.TDP,
            //    Image = new CloudFileData
            //    {
            //        FileName = item.Object.Image?.FileName ?? "",
            //        DownloadUrl = item.Object.Image?.DownloadUrl ??
            //                      "https://www.generationsforpeace.org/wp-content/uploads/2018/03/empty.jpg"
            //    },
            //    Video = new CloudFileData
            //    {
            //        FileName = item.Object.Video?.FileName ?? "",
            //        DownloadUrl = item.Object.Video?.DownloadUrl ?? ""
            //    }
            //}).ToList();
        }

        public Processor GetEntityByGuid(string id)
        {
            var taskGetAllProcessors = _databaseClient
                .Child(processorDir)
                .OnceAsync<Processor>();

            taskGetAllProcessors.Wait();

            if (taskGetAllProcessors.Exception != null)
            {
                Console.WriteLine(taskGetAllProcessors.Exception.Message);
                return null;
            }

            IEnumerable<FirebaseObject<Processor>> resultProcessors = taskGetAllProcessors.Result;

            return resultProcessors.Where(c => c.Object.Id == id).Select(item => new Processor
            {
                Id = item.Object.Id,
                Name = item.Object.Name,
                Description = item.Object.Description,
                Socket = item.Object.Socket,
                IsMultiThreading = item.Object.IsMultiThreading,
                CoresCount = item.Object.CoresCount,
                Frequency = item.Object.Frequency,
                CodeName = item.Object.CodeName,
                TDP = item.Object.TDP,
                //MapPoint = new MapPoint
                //{
                //    Latitude = item.Object.MapPoint.Latitude,
                //    Longitude = item.Object.MapPoint.Longitude
                //},
                Image = new CloudFileData
                {
                    FileName = item.Object.Image?.FileName ?? "",
                    DownloadUrl = item.Object.Image?.DownloadUrl ??
                                  "https://www.generationsforpeace.org/wp-content/uploads/2018/03/empty.jpg"
                },
                Video = new CloudFileData
                {
                    FileName = item.Object.Video?.FileName ?? "",
                    DownloadUrl = item.Object.Video?.DownloadUrl ?? ""
                }
            }).FirstOrDefault();
        }

        public async Task UpdateProcessor(string id, Processor ProcessorDto)
        {
            var toUpdateProcessor = (await _databaseClient
                .Child(processorDir)
                .OnceAsync<Processor>()).FirstOrDefault(c => c.Object.Id == id);

            await _databaseClient
                .Child(processorDir)
                .Child(toUpdateProcessor?.Key)
                .PutAsync(ProcessorDto);
        }
    }
}