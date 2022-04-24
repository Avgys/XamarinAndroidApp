using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Xamarin.Essentials;
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Services;
using XamarinAndroidApp.Utils;

namespace XamarinAndroidApp
{
    [Activity(Label = "EditActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class EditActivity : Activity
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

        private ImageView _image;
        private Button _btnAddImage;
        private Button _btnAddVideo;
        private Button _btnSave;
        private CloudFileData Image { get; set; }
        private CloudFileData Video { get; set; }

        private IFirebaseDbService<Processor> _firebaseDbService;
        private IFirebaseStorageService _firebaseStorageService;

        bool isEditMode = false;

        private Processor currentEntity;
        private Button _btnShowVideo;
        private VideoView _videoView;
        private RelativeLayout _mediaLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.detail_entity);

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
            _image = FindViewById<ImageView>(Resource.Id.detailImageView);

            _btnAddImage = FindViewById<Button>(Resource.Id.btnAddImage);
            _btnAddImage.Click += OnAddImageButtonClicked;

            _btnAddVideo = FindViewById<Button>(Resource.Id.btnAddVideo);
            _btnAddVideo.Click += OnAddVideoButtonClicked;

            FindViewById<Button>(Resource.Id.btnEdit).Click += ToggleEditMode;
            _btnShowVideo = FindViewById<Button>(Resource.Id.btnShowVideo);
            _btnShowVideo.Click += ShowVideoPlayer;

            _videoView = FindViewById<VideoView>(Resource.Id.videoView);
            _mediaLayout = FindViewById<RelativeLayout>(Resource.Id.media_player);
            var mediaController = new MediaController(this);
            mediaController.SetAnchorView(_videoView);
            _videoView.SetMediaController(mediaController);
            _btnSave = FindViewById<Button>(Resource.Id.btnSave);
            _btnSave.Click += EnableInputs;

            _isMultiThreadingYes.Click += (o, e) => { _isMultiThreadingNo.Checked = false; };
            _isMultiThreadingNo.Click += (o, e) => { _isMultiThreadingYes.Checked = false; };

            var path = Intent.GetStringExtra("Path");
            var id = Intent.GetStringExtra("EntityId");
            try
            {
                currentEntity = _firebaseDbService.GetEntityByPath(path)?.Unwrap();
            }
            catch (Exception ex) { }
            if (currentEntity == null || currentEntity.Id != id)
            {
                currentEntity = _firebaseDbService.GetEntityById(id)?.Unwrap();
            }
            DisableInputs(null, null);

            if (currentEntity != null)
            {
                UpdateView(currentEntity);
            }
            else
            {
                Toast.MakeText(this, "Something wrong with getting entity by id", ToastLength.Long).Show();
                Finish();
            }
        }

        void ToggleEditMode(object sernder, EventArgs args)
        {
            if (!isEditMode)
            {
                EnableInputs(null, null);
                isEditMode = true;
            }
            else
            {
                DisableInputs(null, null);
                isEditMode = false;
            }
        }

        private void ShowVideoPlayer(object sernder, EventArgs args)
        {
            _mediaLayout.Visibility = ViewStates.Visible;
            _videoView.RequestFocus();
            _videoView.Start();
        }

        private void UpdateView(Processor currentEntity)
        {
            _name.Text = currentEntity.Name;
            _description.Text = currentEntity.Description;
            _socket.Text = currentEntity.Socket;
            if (currentEntity.IsMultiThreading)
            {
                _isMultiThreadingYes.Checked = true;
                _isMultiThreadingNo.Checked = false;
            }
            else
            {
                _isMultiThreadingYes.Checked = false;
                _isMultiThreadingNo.Checked = true;
            }
            _coresCount.Text = currentEntity.CoresCount.ToString();
            _frequency.Text = currentEntity.Frequency;
            _codeName.Text = currentEntity.CodeName;
            _tdp.Text = currentEntity.TDP.ToString();
            _image.SetImageBitmap(ImageLoader.GetBitmapFromUrl(this, currentEntity.ImageUri));
            _videoView.Visibility = ViewStates.Visible;
            var uri = Android.Net.Uri.Parse(currentEntity.Video.DownloadUrl);
            //var uri = Android.Net.Uri.Parse("https://firebasestorage.googleapis.com/v0/b/mobilki1-dd9f2.appspot.com/o/%D0%97%D0%90%D0%A1%D0%A2%D0%A0%D0%AF%D0%9B%20%D0%9A%D0%A3%D0%A1%D0%9E%D0%9A%20%D0%9C%D0%98%D0%9D%D0%98%D0%94%D0%96%D0%95%D0%9A%D0%90%20%D0%92%20%D0%97%D0%92%D0%A3%D0%9A%D0%9E%D0%92%D0%9E%D0%9C%20%D0%A0%D0%90%D0%97%D0%AA%D0%81%D0%9C%D0%95%20%D0%9F%D0%9A.mp4?alt=media&token=a7492fee-742d-4fab-a05a-f89a93ea7e62");
            if (uri != null)
            {
                _videoView.SetVideoURI(uri);
            }
        }

        private void EnableInputs(object sernder, EventArgs args)
        {
            _name.FocusableInTouchMode = true;
            _description.FocusableInTouchMode = true;
            _socket.FocusableInTouchMode = true;
            _isMultiThreadingYes.FocusableInTouchMode = true;
            _isMultiThreadingYes.Visibility = ViewStates.Visible;
            _isMultiThreadingYes.FocusableInTouchMode = true;
            _isMultiThreadingNo.Visibility = ViewStates.Visible;
            _coresCount.FocusableInTouchMode = true;
            _frequency.FocusableInTouchMode = true;
            _codeName.FocusableInTouchMode = true;
            _tdp.FocusableInTouchMode = true;

            _btnShowVideo.Visibility = ViewStates.Gone;
            _btnAddVideo.Visibility = ViewStates.Visible;
            _btnAddImage.Visibility = ViewStates.Visible;
            _btnSave.Visibility = ViewStates.Visible;
        }

        private void DisableInputs(object sernder, EventArgs args)
        {
            _name.Focusable = false;
            _description.Focusable = false;
            _socket.Focusable = false;
            _isMultiThreadingYes.Focusable = false;
            _isMultiThreadingYes.Visibility = ViewStates.Gone;
            _isMultiThreadingYes.Focusable = false;
            _isMultiThreadingNo.Visibility = ViewStates.Gone;
            _coresCount.Focusable = false;
            _frequency.Focusable = false;
            _codeName.Focusable = false;
            _tdp.Focusable = false;
            _btnShowVideo.Visibility = ViewStates.Visible;
            _btnAddVideo.Visibility = ViewStates.Gone;
            _btnAddImage.Visibility = ViewStates.Gone;
            _btnSave.Visibility = ViewStates.Gone;
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
