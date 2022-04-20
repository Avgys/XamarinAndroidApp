using Android.App;
using Android.OS;
using Android.Widget;
using System;
using XamarinAndroidApp.Services;

namespace XamarinAndroidApp
{
    [Activity(Theme = "@style/MyTheme.Login", Icon = "@mipmap/ic_launcher", NoHistory = true)]
    public class LoginActivity : Activity
    {
        EditText email;
        EditText password;

        IFirebaseAuthentication _firebaseAuthentication;
        IFirebaseDbService _firebaseDbService;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            //Get email & password values from edit text  
            email = FindViewById<EditText>(Resource.Id.txtEmail);
            password = FindViewById<EditText>(Resource.Id.txtPassword);
            //Trigger click event of Login Button  
            var loginButton = FindViewById<Button>(Resource.Id.btnLogin);
            loginButton.Click += DoLogin;

            var regButton = FindViewById<Button>(Resource.Id.btnToRegister);
            regButton.Click += ToRegister;

            _firebaseAuthentication = MainActivity.ServicesProvider.GetService(typeof(IFirebaseAuthentication)) as IFirebaseAuthentication;            
            _firebaseDbService = MainActivity.ServicesProvider.GetService(typeof(IFirebaseDbService)) as IFirebaseDbService;
        }

        public async void DoLogin(object sender, EventArgs e)
        {
            bool isAuthSuccessful = true;
            isAuthSuccessful = await _firebaseAuthentication.LoginWithEmailAndPasswordAsync("2@gmail.com", "123456");
            
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
                Toast.MakeText(this, "Login successfully done!", ToastLength.Long).Show();
                StartActivity(typeof(MainActivity));
                //Application.Current.MainPage = new AppShell();
                //}
            }
            else
            {
                Toast.MakeText(this, "Wrong credentials found!", ToastLength.Long).Show();
            } 
        }

        public void ToRegister(object sender, EventArgs e)
        {
            StartActivity(typeof(RegisterActivity));
        }
    }
}
