using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using XamarinAndroidApp.Droid.Services;
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Services;
using Filter = XamarinAndroidApp.Utils.Filter;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace XamarinAndroidApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar")]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        public static IServiceProvider ServicesProvider;
        IFirebaseDbService<Processor> _firebaseDbService;
        RecyclerView _recyclerview;
        RecyclerView.LayoutManager _layoutManager;
        RecycleViewAdapter<Processor> _recycleAdapter;
        List<Processor> _entityList = new List<Processor>();
       
        LinearLayout _gridviewLayout;
        private LinearLayout _detailviewLayout;
        LinearLayout _recycleviewLayout;
        private GridView _detailview;
        private DetailViewAdapter<Processor> _detailviewAdapter;
        private GridView _gridview;
        private GridViewAdapter<Processor> _gridviewAdapter;        
        private Filter _filter;
        DisplayOrientation _orientation;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            //DeviceDisplay.MainDisplayInfoChanged += OnMainDisplayInfoChanged;
            _orientation = DeviceDisplay.MainDisplayInfo.Orientation;
            _firebaseDbService = ServicesProvider.GetService(typeof(IFirebaseDbService<Processor>)) as IFirebaseDbService<Processor>;
            _entityList = _firebaseDbService.GetAllEntities().Select(item => item.Unwrap()).ToList();

            _gridviewLayout = FindViewById<LinearLayout>(Resource.Id.gridviewLayout);
            _recycleviewLayout = FindViewById<LinearLayout>(Resource.Id.recycleviewLayout);
            _detailviewLayout = FindViewById<LinearLayout>(Resource.Id.detailviewLayout);
            if (_orientation == DisplayOrientation.Portrait)
            {
                _detailviewLayout.Visibility = ViewStates.Gone;
                _gridviewLayout.Visibility = ViewStates.Gone;
                //SetContentView(Resource.Layout.recycleview_main);
                _recyclerview = FindViewById<RecyclerView>(Resource.Id.recyclerView);

                // Plug in the linear layout manager:
                _layoutManager = new LinearLayoutManager(this);
                _recyclerview.SetLayoutManager(_layoutManager);

                // Plug in my adapter:
                _recycleAdapter = new RecycleViewAdapter<Processor>(this, _entityList);
                _recycleAdapter.ItemClick += delegate (object sender, Processor arg)
                {
                    var editIntent = new Intent(this, typeof(EditActivity));
                    editIntent.PutExtra("EntityId", arg.Id);
                    editIntent.PutExtra("Path", arg.Path);
                    StartActivity(editIntent);
                    UpdateAdapters();
                };
                _recyclerview.SetAdapter(_recycleAdapter);

                _gridview = FindViewById<GridView>(Resource.Id.gridview);
                _gridviewAdapter = new GridViewAdapter<Processor>(this, _entityList);
                _gridview.Adapter = _gridviewAdapter;
                _gridview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                {
                    var editIntent = new Intent(this, typeof(EditActivity));
                    editIntent.PutExtra("EntityId", _entityList[args.Position].Id);
                    editIntent.PutExtra("Path", _entityList[args.Position].Path);
                    StartActivity(editIntent);
                    UpdateAdapters();
                };                
            }
            else
            {
                _detailviewLayout.Visibility = ViewStates.Visible;
                _gridviewLayout.Visibility = ViewStates.Gone;
                _recycleviewLayout.Visibility = ViewStates.Gone;
                _detailview = FindViewById<GridView>(Resource.Id.detailview);
                _detailviewAdapter = new DetailViewAdapter<Processor>(this, _entityList);
                _detailview.Adapter = _detailviewAdapter;
                _detailview.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
                {
                    var editIntent = new Intent(this, typeof(EditActivity));
                    editIntent.PutExtra("EntityId", _entityList[(args.Position / _detailviewAdapter.columnsCount) + 1].Id);
                    editIntent.PutExtra("Path", _entityList[(args.Position / _detailviewAdapter.columnsCount) + 1].Path);
                    StartActivity(editIntent);
                    UpdateAdapters();
                };
            }

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
        }

        private void UpdateAdapters()
        {
            _entityList = _firebaseDbService.GetAllEntities().Select(item => item.Unwrap()).ToList();
            _recycleAdapter?.Update(_entityList);
            _recycleAdapter?.NotifyDataSetChanged();
            _gridviewAdapter?.Update(_entityList);
            _gridviewAdapter?.NotifyDataSetChanged();
            _gridview?.Invalidate();
            _detailviewAdapter?.Update(_entityList);
            _detailviewAdapter?.NotifyDataSetChanged();
            _detailview?.Invalidate();
        }

        private void UpdateAdapters(List<Processor> list)
        {
            _entityList = list;

            _recycleAdapter?.Update(_entityList);
            _recycleAdapter?.NotifyDataSetChanged();
            _gridviewAdapter?.Update(_entityList);
            _gridviewAdapter?.NotifyDataSetChanged();
            _gridview?.Invalidate();
            _detailviewAdapter?.Update(_entityList);
            _detailviewAdapter?.NotifyDataSetChanged();
            _detailview?.Invalidate();
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
            if (id == Resource.Id.filter_settings)
            {
                BtnshowPopup_Click();
            }
            else if (id == Resource.Id.switch_layout_view)
            {
                if (_orientation == DisplayOrientation.Portrait)
                {
                    _gridviewLayout.Visibility = _gridviewLayout.Visibility ^ ViewStates.Visible ^ ViewStates.Gone;
                    _recycleviewLayout.Visibility = _recycleviewLayout.Visibility ^ ViewStates.Gone ^ ViewStates.Visible;
                }
                else
                {
                    Toast.MakeText(this, "Change orientation to portrait first", ToastLength.Long).Show();
                }
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            StartActivity(typeof(AddActivity));
            UpdateAdapters();
        }

        private void BtnshowPopup_Click()
        {
            var popupDialog = new Dialog(this);
            popupDialog.SetContentView(Resource.Layout.filter_popup);
            popupDialog.Window.SetSoftInputMode(SoftInput.AdjustResize);
            popupDialog.Show();

            // Some Time Layout width not fit with windows size  
            // but Below lines are not necessery  
            popupDialog.Window.SetBackgroundDrawableResource(Android.Resource.Color.Transparent);

            // Access Popup layout fields like below  
            var btnPopupCancel = popupDialog.FindViewById<Button>(Resource.Id.btnCancel);
            var btnPopOk = popupDialog.FindViewById<Button>(Resource.Id.btnOk);

            var name = popupDialog.FindViewById<EditText>(Resource.Id.entityNameFilter);
            var description = popupDialog.FindViewById<EditText>(Resource.Id.entityDescriptionFilter);
            var socket = popupDialog.FindViewById<EditText>(Resource.Id.entitySocketFilter);
            
            var isMultiThreadingYes = popupDialog.FindViewById<RadioButton>(Resource.Id.entityIsMultiThreadingYesFilter);            
            var isMultiThreadingNo = popupDialog.FindViewById<RadioButton>(Resource.Id.entityIsMultiThreadingNoFilter);
            var isMultiThreadingAny = popupDialog.FindViewById<RadioButton>(Resource.Id.entityIsMultiThreadingAnyFilter);

            isMultiThreadingYes.Click += (o, e) => { isMultiThreadingNo.Checked = false; isMultiThreadingAny.Checked = false; };
            isMultiThreadingNo.Click += (o, e) => { isMultiThreadingYes.Checked = false; isMultiThreadingAny.Checked = false; };
            isMultiThreadingAny.Click += (o, e) => { isMultiThreadingNo.Checked = false; isMultiThreadingYes.Checked = false; };

            var coresCountLeft = popupDialog.FindViewById<EditText>(Resource.Id.entityCoresLeftFilter);
            var coresCountRight = popupDialog.FindViewById<EditText>(Resource.Id.entityCoresRightFilter);
            var frequency = popupDialog.FindViewById<EditText>(Resource.Id.entityFrequncyFilter);
            var codeName = popupDialog.FindViewById<EditText>(Resource.Id.entityCodeNameFilter);
            var tdpLeft = popupDialog.FindViewById<EditText>(Resource.Id.entityTDPLeftFilter);
            var tdpRight = popupDialog.FindViewById<EditText>(Resource.Id.entityTDPRightFilter);

            if (_filter != null)
            {
                name.Text = _filter.name;
                description.Text = _filter.description;
                socket.Text = _filter.socket;
                isMultiThreadingYes.Checked = _filter.isMultiThreadingYes;
                isMultiThreadingNo.Checked = _filter.isMultiThreadingNo;
                isMultiThreadingAny.Checked = _filter.isMultiThreadingAny;
                coresCountLeft.Text = _filter.coresCountLeft.ToString();
                coresCountRight.Text = _filter.coresCountRight.ToString();
                frequency.Text = _filter.frequency;
                codeName.Text = _filter.codeName;
                tdpLeft.Text = _filter.tdpLeft.ToString();
                tdpRight.Text = _filter.tdpRight.ToString();
            }

            // Events for that popup layout  
            btnPopupCancel.Click += (o, e) =>
            {
                var list = _firebaseDbService.GetAllEntities().Select(item => item.Unwrap()).ToList();
                UpdateAdapters(list);
                popupDialog.Dismiss();
                popupDialog.Hide();
            };

            btnPopOk.Click += (o, e) =>
            {
                _filter = new Filter(
                    name.Text,
                    description.Text,
                    socket.Text,
                    isMultiThreadingYes.Checked,
                    isMultiThreadingNo.Checked,
                    isMultiThreadingAny.Checked,
                    coresCountLeft.Text,
                    coresCountRight.Text,
                    frequency.Text,
                    codeName.Text,
                    tdpLeft.Text,
                    tdpRight.Text
                    );
                var list = _firebaseDbService.GetAllEntities().Select(item => item.Unwrap()).ToList();
                list = _filter.FilterList(list);
                UpdateAdapters(list);
                popupDialog.Dismiss();
                popupDialog.Hide();
            };
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_camera)
            {
                BtnshowPopup_Click();
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

