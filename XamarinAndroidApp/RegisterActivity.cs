using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XamarinAndroidApp.Services;

namespace XamarinAndroidApp
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        EditText email;
        EditText password;
        EditText rePassword;

        IFirebaseAuthentication _firebaseAuthentication;
        IFirebaseDbService _firebaseDbService;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.register);
            //Get email & password values from edit text  
            email = FindViewById<EditText>(Resource.Id.txtEmail);
            password = FindViewById<EditText>(Resource.Id.txtPassword);
            rePassword = FindViewById<EditText>(Resource.Id.txtRePassword);
            //Trigger click event of Login Button  
            var regButton = FindViewById<Button>(Resource.Id.btnRegister);
            regButton.Click += DoRegister;

            var loginButton = FindViewById<Button>(Resource.Id.btnToLogin);
            loginButton.Click += ToLogin;

            _firebaseAuthentication = MainActivity.ServicesProvider.GetService(typeof(IFirebaseAuthentication)) as IFirebaseAuthentication;
            _firebaseDbService = MainActivity.ServicesProvider.GetService(typeof(IFirebaseDbService)) as IFirebaseDbService;
        }

        public async void DoRegister(object sender, EventArgs e)
        {

            bool isAuthSuccessful = true;
            //isAuthSuccessful = await _firebaseAuthentication.RegisterWithEmailAndPasswordAsync(email.Text, password.Text);
            if (password.Text == rePassword.Text)
            {
                bool isRegistrationSuccessful = await _firebaseAuthentication.RegisterWithEmailAndPasswordAsync(email.Text, password.Text);

                if (isRegistrationSuccessful)
                {
                    //var user = new User
                    //{
                    //    Email = Email,
                    //    IsAdmin = false,
                    //    IsBlocked = false
                    //};

                    //await _firebaseDbService.AddUserInfo(user);

                    Toast.MakeText(this, "Registration successfully done!", ToastLength.Long).Show();
                    StartActivity(typeof(MainActivity));
                }
                else
                {
                    Toast.MakeText(this, "Wrong credentials found!", ToastLength.Long).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Wrong passwords found!", ToastLength.Long).Show();
            }
            if (email.Text == "admin" && password.Text == "admin")
            {
                isAuthSuccessful = true;
            }

            if (isAuthSuccessful)
            {
                //var currentUser = _firebaseDbService.GetCurrentUser();

                //if (currentUser.IsBlocked)
                //{
                //    _firebaseAuthentication.SignOut();

                //    await Application.Current.MainPage.DisplayAlert("Blocked",
                //        "You are blocked!", AppContentText.OkButton);
                //    Application.Current.MainPage = new LoginPage();
                //}
                //else
                //{
                
                //Application.Current.MainPage = new AppShell();
                //}
            }
            else
            {

                Toast.MakeText(this, "Wrong credentials found!", ToastLength.Long).Show();
            } //Toast.makeText(getActivity(), "Wrong credentials found!", Toast.LENGTH_LONG).show(); 
        }

        public void ToLogin(object sender, EventArgs e)
        {
            StartActivity(typeof(LoginActivity));
        }
    }
}