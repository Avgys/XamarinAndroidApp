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
using System.Threading.Tasks;
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Services;

namespace XamarinAndroidApp
{
    [Activity(Label = "AddActivity")]
    public class AddActivity : Activity
    {
        private EditText _id { get; set; }
        private EditText _name { get; set; }
        private EditText _description { get; set; }
        private EditText _socket { get; set; }
        private EditText _isMultiThreading { get; set; }
        private EditText _coresCount { get; set; }
        private EditText _frequency { get; set; }
        private EditText _codeName { get; set; }
        private EditText _tdp { get; set; }

        //private CloudFileData Image { get; set; }
        //private CloudFileData Video { get; set; }
        //private string Caption { get => Name; }
        //private string ImageUri { get => Image.DownloadUrl; }

        private IFirebaseDbService _firebaseDbService;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.add_entity);

            _firebaseDbService = MainActivity.ServicesProvider.GetService(typeof(IFirebaseDbService)) as IFirebaseDbService;

            //_Id = FindViewById<EditText>(Resource.Id.entityName);
            _name = FindViewById<EditText>(Resource.Id.entityName);
            _description = FindViewById<EditText>(Resource.Id.entityDescription);
            _socket = FindViewById<EditText>(Resource.Id.entitySocket);
            _isMultiThreading = FindViewById<EditText>(Resource.Id.entityIsMultiThreading);
            _coresCount = FindViewById<EditText>(Resource.Id.entityCores);
            _frequency = FindViewById<EditText>(Resource.Id.entityFrequncy);
            _codeName = FindViewById<EditText>(Resource.Id.entityCodeName);
            _tdp = FindViewById<EditText>(Resource.Id.entityTDP);
            //password = FindViewById<EditText>(Resource.Id.txtPassword);
            //Trigger click event of Login Button
            var saveAdd = FindViewById<Button>(Resource.Id.btnSaveAdd);
            saveAdd.Click += OnSaveButtonClicked;

            //await Task.Delay(500);
            //await Task.Run(() => { Finish(); });            
        }

        //private async void OnAddImageButtonClicked()
        //{
        //    var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Select image" });

        //    if (photo is null)
        //    {
        //        return;
        //    }

        //    string extension = photo.FileName.Split('.')[1];
        //    var stream = await photo.OpenReadAsync();
        //    ImageName = Guid.NewGuid().ToString();
        //    ImageUrl = await _firebaseStorageService.LoadImage(stream, ImageName, extension);
        //}

        //private async void OnAddVideoButtonClicked()
        //{
        //    var video = await MediaPicker.PickVideoAsync(new MediaPickerOptions { Title = "Select video" });

        //    if (video is null)
        //    {
        //        return;
        //    }

        //    string extension = video.FileName.Split('.')[1];
        //    var stream = await video.OpenReadAsync();
        //    VideoName = Guid.NewGuid().ToString();
        //    VideoUrl = await _firebaseStorageService.LoadVideo(stream, VideoName, extension);
        //}

        private async void OnSaveButtonClicked(object sender, EventArgs eventArgs)
        {
            if (/*IsCorrectFields()*/true)
            {
                var entity = new Processor
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = _name.Text,
                    Description = _description.Text,
                    Type = Type,
                    ProcessorModel = ProcessorModel,
                    RamSize = int.Parse(RamSize),
                    SsdSize = int.Parse(SsdSize),
                    Price = decimal.Parse(Price),
                    MapPoint = new MapPoint
                    {
                        Latitude = double.Parse(Latitude),
                        Longitude = double.Parse(Longitude)
                    },
                    Image = new CloudFileData
                    {
                        FileName = ImageName,
                        DownloadUrl = ImageUrl
                    },
                    Video = new CloudFileData
                    {
                        FileName = VideoName,
                        DownloadUrl = VideoUrl
                    }
                };

                await _firebaseDbService.AddEntity(entity);
                //await Shell.Current.GoToAsync(nameof(HomePage));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AppContentText.IncorrectFieldsTitle,
                    AppContentText.IncorrectFieldsMessage, AppContentText.OkButton);
            }
        }

        //private bool IsCorrectFields()
        //{
        //    if (!string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(Description) ||
        //        !string.IsNullOrWhiteSpace(Type) || !string.IsNullOrWhiteSpace(ProcessorModel))
        //    {
        //        try
        //        {
        //            _ = int.Parse(RamSize);
        //            _ = int.Parse(SsdSize);
        //            _ = decimal.Parse(Price);
        //            _ = double.Parse(Latitude);
        //            _ = double.Parse(Longitude);

        //            return true;
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }

        //    return false;
        //}
    }
}