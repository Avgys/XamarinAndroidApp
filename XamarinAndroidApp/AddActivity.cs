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

namespace XamarinAndroidApp
{
    [Activity(Label = "AddActivity")]
    public class AddActivity : Activity
    {
        public string _Id { get; set; }
        public string _Name { get; set; }
        public string _Description { get; set; }
        public string _Socket { get; set; }
        public bool _IsMultiThreading { get; set; }
        public int _CoresCount { get; set; }
        public int _Frequency { get; set; }
        public string _CodeName { get; set; }
        public int _TDP { get; set; }

        //public CloudFileData Image { get; set; }
        //public CloudFileData Video { get; set; }
        //public string Caption { get => Name; }
        //public string ImageUri { get => Image.DownloadUrl; }
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.add_entity);

            //email = FindViewById<EditText>(Resource.Id.entityName);
            //email = FindViewById<EditText>(Resource.Id.entityDescription);
            //email = FindViewById<EditText>(Resource.Id.entityDescription);
            //email = FindViewById<EditText>(Resource.Id.entityDescription);
            //email = FindViewById<EditText>(Resource.Id.entityDescription);
            //email = FindViewById<EditText>(Resource.Id.entityDescription);
            //email = FindViewById<EditText>(Resource.Id.entityDescription);
            //email = FindViewById<EditText>(Resource.Id.entityDescription);
            //email = FindViewById<EditText>(Resource.Id.entityDescription);
            //email = FindViewById<EditText>(Resource.Id.entityDescription);
            //email = FindViewById<EditText>(Resource.Id.entityDescription);
            //email = FindViewById<EditText>(Resource.Id.entityDescription);
            //password = FindViewById<EditText>(Resource.Id.txtPassword);
            //Trigger click event of Login Button
            //var loginButton = FindViewById<Button>(Resource.Id.btnLogin);
            //loginButton.Click += DoLogin;

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

        //private async void OnSaveButtonClicked()
        //{
        //    if (IsCorrectFields())
        //    {
        //        var computer = new Computer
        //        {
        //            Id = Guid.NewGuid().ToString(),
        //            Name = Name,
        //            Description = Description,
        //            Type = Type,
        //            ProcessorModel = ProcessorModel,
        //            RamSize = int.Parse(RamSize),
        //            SsdSize = int.Parse(SsdSize),
        //            Price = decimal.Parse(Price),
        //            MapPoint = new MapPoint
        //            {
        //                Latitude = double.Parse(Latitude),
        //                Longitude = double.Parse(Longitude)
        //            },
        //            Image = new CloudFileData
        //            {
        //                FileName = ImageName,
        //                DownloadUrl = ImageUrl
        //            },
        //            Video = new CloudFileData
        //            {
        //                FileName = VideoName,
        //                DownloadUrl = VideoUrl
        //            }
        //        };

        //        await _firebaseDbService.AddComputer(computer);
        //        await Shell.Current.GoToAsync(nameof(HomePage));
        //    }
        //    else
        //    {
        //        await Application.Current.MainPage.DisplayAlert(AppContentText.IncorrectFieldsTitle,
        //            AppContentText.IncorrectFieldsMessage, AppContentText.OkButton);
        //    }
        //}

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