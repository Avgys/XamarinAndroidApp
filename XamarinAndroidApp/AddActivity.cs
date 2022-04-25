using Android.App;
using Android.OS;
using Android.Widget;
using System;
using Xamarin.Essentials;
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Services;

namespace XamarinAndroidApp
{
    [Activity(Label = "AddActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class AddActivity : Activity
    {
        private EditText _id { get; set; }
        private EditText _name { get; set; }
        private EditText _description { get; set; }
        private EditText _socket { get; set; }
        private RadioButton _isMultiThreadingYes { get; set; }
        private RadioButton _isMultiThreadingNo { get; set; }
        private EditText _coresCount { get; set; }
        private EditText _frequency { get; set; }
        private EditText _codeName { get; set; }
        private EditText _tdp { get; set; }

        private CloudFileData Image { get; set; }
        private CloudFileData Video { get; set; }

        private IFirebaseDbService<Processor> _firebaseDbService;
        private IFirebaseStorageService _firebaseStorageService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.add_entity);

            _firebaseDbService = MainActivity.ServicesProvider.GetService(typeof(IFirebaseDbService<Processor>)) as IFirebaseDbService<Processor>;
            _firebaseStorageService = MainActivity.ServicesProvider.GetService(typeof(IFirebaseStorageService)) as IFirebaseStorageService;            
            _name = FindViewById<EditText>(Resource.Id.entityName);
            _description = FindViewById<EditText>(Resource.Id.entityDescription);
            _socket = FindViewById<EditText>(Resource.Id.entitySocket);
            _isMultiThreadingYes = FindViewById<RadioButton>(Resource.Id.entityIsMultiThreadingYes);
            _isMultiThreadingNo = FindViewById<RadioButton>(Resource.Id.entityIsMultiThreadingNo);
            _coresCount = FindViewById<EditText>(Resource.Id.entityCores);
            _frequency = FindViewById<EditText>(Resource.Id.entityFrequncy);
            _codeName = FindViewById<EditText>(Resource.Id.entityCodeName);
            _tdp = FindViewById<EditText>(Resource.Id.entityTDP);

            FindViewById<Button>(Resource.Id.btnAddImage).Click += OnAddImageButtonClicked;
            FindViewById<Button>(Resource.Id.btnAddVideo).Click += OnAddVideoButtonClicked;
            FindViewById<Button>(Resource.Id.btnSaveAdd).Click += OnSaveButtonClicked;


            _isMultiThreadingYes.Click += (o, e) => { _isMultiThreadingNo.Checked = false; };
            _isMultiThreadingNo.Click += (o, e) => { _isMultiThreadingYes.Checked = false; };

        }

        private async void OnAddImageButtonClicked(object sender, EventArgs e)
        {
            var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Select image" });

            if (photo is null)
            {
                return;
            }

            string extension = photo.FileName.Split('.')[1];
            var stream = await photo.OpenReadAsync();
            Image = new CloudFileData();
            Image.FileName = Guid.NewGuid().ToString();
            Image.DownloadUrl = await _firebaseStorageService.LoadImage(stream, Image.FileName, extension);
        }

        private async void OnAddVideoButtonClicked(object sender, EventArgs e)
        {
            var video = await MediaPicker.PickVideoAsync(new MediaPickerOptions { Title = "Select video" });

            if (video is null)
            {
                return;
            }

            string extension = video.FileName.Split('.')[1];
            var stream = await video.OpenReadAsync();
            Video = new CloudFileData();
            Video.FileName = Guid.NewGuid().ToString();
            Video.DownloadUrl = await _firebaseStorageService.LoadVideo(stream, Video.FileName, extension);
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("Selected country is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs eventArgs)
        {
            try
            {
                var entity = new Processor
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = _name.Text,
                    Description = _description.Text,
                    Socket = _socket.Text,
                    IsMultiThreading = _isMultiThreadingYes.Checked,
                    CoresCount = int.Parse(_coresCount.Text),
                    Frequency = _frequency.Text,
                    CodeName = _codeName.Text,
                    TDP = int.Parse(_tdp.Text),
                    Image = this.Image,
                    Video = this.Video
                };

                await _firebaseDbService.AddEntity(entity);
                Finish();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, $"{ex.Source}:{ex.Message}", ToastLength.Long).Show();
            }
        }
    }
}