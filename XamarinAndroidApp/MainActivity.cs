using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Firebase.Components;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using Google.Android.Material.Snackbar;
using Microsoft.Extensions.DependencyInjection;
using XamarinAndroidApp.Droid.Services;
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Services;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace XamarinAndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        public static IServiceProvider ServicesProvider;

        IFirebaseDbService _firebaseDbService;
        RecyclerView _recyclerview;
        RecyclerView.LayoutManager _layoutManager;
        RecycleViewAdapter<Processor> _recycleAdapter;
        List<Processor> _entityList = new List<Processor>();

        LinearLayout _gridviewLayout;
        LinearLayout _recycleviewLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            _firebaseDbService = ServicesProvider.GetService(typeof(IFirebaseDbService)) as IFirebaseDbService;
            _entityList = _firebaseDbService.GetAllProcessors();

            //SetContentView(Resource.Layout.recycleview_main);
            _recyclerview = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            // Plug in the linear layout manager:
            _layoutManager = new LinearLayoutManager(this);
            _recyclerview.SetLayoutManager(_layoutManager);

            // Plug in my adapter:
            _recycleAdapter = new RecycleViewAdapter<Processor>(this, _entityList);
            _recycleAdapter.ItemClick += OnItemClick;
            _recyclerview.SetAdapter(_recycleAdapter);

            var gridview = FindViewById<GridView>(Resource.Id.gridview);
            gridview.Adapter = new GridViewAdapter<Processor>(this, _entityList);

            gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                Toast.MakeText(this, args.Position.ToString(), ToastLength.Short).Show();
            };

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            _gridviewLayout = FindViewById<LinearLayout>(Resource.Id.gridviewLayout);
            _recycleviewLayout = FindViewById<LinearLayout>(Resource.Id.recycleviewLayout);

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }
            else if (id == Resource.Id.switch_layout_view)
            {
                _gridviewLayout.Visibility = _gridviewLayout.Visibility ^ ViewStates.Visible ^ ViewStates.Gone;
                _recycleviewLayout.Visibility = _recycleviewLayout.Visibility ^ ViewStates.Gone ^ ViewStates.Visible;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            //Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
            //    .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
            StartActivity(typeof(AddActivity));            
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                // Handle the camera action
            }
            else if (id == Resource.Id.nav_gallery)
            {

            }
            else if (id == Resource.Id.nav_slideshow)
            {

            }
            else if (id == Resource.Id.nav_manage)
            {

            }
            else if (id == Resource.Id.nav_share)
            {

            }
            else if (id == Resource.Id.nav_send)
            {

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        void OnItemClick(object sender, int position)
        {
            int photoNum = position + 1;
            Toast.MakeText(this, "This is photo number " + photoNum, ToastLength.Short).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

