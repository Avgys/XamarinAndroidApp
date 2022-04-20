using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Firebase;
using Microsoft.Extensions.DependencyInjection;
using XamarinAndroidApp.Droid.Services;
using XamarinAndroidApp.Services;

namespace XamarinAndroidApp.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { Startup(); });
            startupWork.Start();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        async void Startup()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<IFirebaseDbService,FirebaseDbService>();
            services.AddSingleton<IFirebaseStorageService, FirebaseStorageService>();
            services.AddSingleton<IFirebaseAuthentication, FirebaseAuthentication>();

            var options = new FirebaseOptions.Builder().SetApplicationId("1:183356596791:android:af4bc5c2a5cbd7c52755b2").SetApiKey("AIzaSyCAbvarDLj9lYs1vQrf6orevUd7U6XMV_s").Build(); //.SetDatabaseUrl("Firebase -Database-Url") .Build();
            FirebaseApp.InitializeApp(this, options);
            MainActivity.ServicesProvider = services.BuildServiceProvider();

            Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
            await Task.Delay(500); // Simulate a bit of startup work.
            Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
            StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
        }
    }
}