using Android.App;
using Android.OS;
using Android.Widget;
using System;
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Services;
using XamarinAndroidApp.Services.Firebase;

namespace XamarinAndroidApp
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : Activity
    {
        EditText email;
        EditText password;
        EditText rePassword;

        private IFirebaseAuthentication _firebaseAuthentication;
        private IFirebaseDbUserService _firebaseDbUserService;

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
            _firebaseDbUserService = MainActivity.ServicesProvider.GetService(typeof(IFirebaseDbUserService)) as IFirebaseDbUserService;
        }

        public async void DoRegister(object sender, EventArgs e)
        {
            if (password.Text == rePassword.Text)
            {
                bool isRegistrationSuccessful = await _firebaseAuthentication.RegisterWithEmailAndPasswordAsync(email.Text, password.Text);

                if (isRegistrationSuccessful)
                {
                    var user = new User
                    {
                        Email = email.Text,
                        IsAdmin = false,
                        IsBlocked = false
                    };

                    await _firebaseDbUserService.AddUserInfo(user);

                    //if (isAuthSuccessful)
                    //{
                    //    //var currentUser = _firebaseDbService.GetCurrentUser();

                    //    //if (currentUser.IsBlocked)
                    //    //{
                    //    //    _firebaseAuthentication.SignOut();

                    //    //    await Application.Current.MainPage.DisplayAlert("Blocked",
                    //    //        "You are blocked!", AppContentText.OkButton);
                    //    //    Application.Current.MainPage = new LoginPage();
                    //    //}
                    //    //else
                    //    //{

                    //    //Application.Current.MainPage = new AppShell();
                    //    //}
                    //}
                    //else
                    //{
                    //    Toast.MakeText(this, "Wrong credentials found!", ToastLength.Long).Show();
                    //} //Toast.makeText(getActivity(), "Wrong credentials found!", Toast.LENGTH_LONG).show(); 
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
        }

        public void ToLogin(object sender, EventArgs e)
        {
            StartActivity(typeof(LoginActivity));
        }
    }
}