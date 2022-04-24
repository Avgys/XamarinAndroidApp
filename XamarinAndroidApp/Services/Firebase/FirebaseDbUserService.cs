using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Services.Firebase;

namespace XamarinAndroidApp.Services
{
    public class FirebaseDbUserService : IFirebaseDbUserService
    {
        private readonly FirebaseClient _databaseClient =
            new FirebaseClient("https://mobilki1-dd9f2-default-rtdb.europe-west1.firebasedatabase.app/");

        private string usersDir = "Users";

        public async Task AddUserInfo(User userDto)
        {
            await _databaseClient
                .Child(usersDir)                
                .PostAsync(userDto);
        }

        public IEnumerable<User> GetAllUsers()
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
    }
}