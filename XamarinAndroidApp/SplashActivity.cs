using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using Firebase;
using Google.Android.Material.Animation;
using Java.Lang;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using XamarinAndroidApp.Droid.Services;
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Services;
using XamarinAndroidApp.Services.Firebase;
using Thread = System.Threading.Thread;

namespace XamarinAndroidApp.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;
        private Task logoMove;
        private bool move = true;
        static string madeBy = "made by Ilia ";

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            Log.Debug(TAG, "SplashActivity.OnCreate");
            SetContentView(Resource.Layout.splash_screen);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            SetContentView(Resource.Layout.splash_screen);
            //RequestWindowFeature(Android.Views.WindowFeatures.NoTitle);
            //var imageView = (ImageView)FindViewById(Resource.Id.imageView);
            //var textview = (TextView)FindViewById(Resource.Id.textView1);
            //var view_animation = AnimationUtils.z(this, Resource.Animation.view_animation);
            //var textview.SetTextColor(Color.Red);
            //imageView.StartAnimation(view_animation);
            //view_animation.AnimationEnd += Rotate_AnimationEnd;

            Task.Run(() => Startup());
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        void Startup()
        {
            Task.Delay(1000).Wait();
            if (MainActivity.ServicesProvider == null)
            {
                ServiceCollection services = new ServiceCollection();
                services.AddSingleton<IFirebaseDbService<Processor>, FirebaseDbService<Processor>>();
                services.AddSingleton<IFirebaseStorageService, FirebaseStorageService>();
                services.AddSingleton<IFirebaseAuthentication, FirebaseAuthentication>();
                services.AddSingleton<IFirebaseDbUserService, FirebaseDbUserService>();

                var options = new FirebaseOptions.Builder().SetApplicationId("1:183356596791:android:af4bc5c2a5cbd7c52755b2").SetApiKey("AIzaSyCAbvarDLj9lYs1vQrf6orevUd7U6XMV_s").Build(); //.SetDatabaseUrl("Firebase -Database-Url") .Build();
                FirebaseApp.InitializeApp(this, options);
                MainActivity.ServicesProvider = services.BuildServiceProvider();
            }

            Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
            Log.Debug(TAG, "Startup work is finished - starting MainActivity.");

            var textView = FindViewById<TextView>(Resource.Id.textView1);
            int i = 1;
            var logo = FindViewById<ImageView>(Resource.Id.logo);
            var task = Task.Run(() =>
            {
                while (move && i < madeBy.Length)
                {
                    //logo.SetPadding(0, i * 2, 0, 0);

                    textView.Post(new Runnable(() => textView.SetText((madeBy[..i]).ToCharArray(), 0, textView.Text.Length + 1)));

                    //textView.SetText((madeBy[..i]).ToCharArray(), 0, textView.Text.Length + 1);
                    i++;
                    Task.Delay(400).Wait();
                }
            });

            task.Wait();
            StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
        }
    }
}