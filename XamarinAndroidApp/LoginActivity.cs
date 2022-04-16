using Android.App;
using Android.OS;
using Android.Widget;
using System;
using XamarinAndroidApp.Services;
using DependencyService = Xamarin.Forms.DependencyService;

namespace XamarinAndroidApp
{
    [Activity(Theme = "@style/MyTheme.Login", Icon = "@mipmap/ic_launcher")]
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
            var button = FindViewById<Button>(Resource.Id.btnLogin);
            button.Click += DoLogin;

            _firebaseAuthentication = DependencyService.Get<IFirebaseAuthentication>();
            _firebaseDbService = DependencyService.Get<IFirebaseDbService>();
        }
        public void DoLogin(object sender, EventArgs e)
        {
            bool isAuthSuccessful = true;
            //bool isAuthSuccessful = await _firebaseAuthentication.LoginWithEmailAndPasswordAsync(Email, Password);
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
    }
}