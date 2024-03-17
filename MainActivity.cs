using Android.Content.PM;
using Android.Runtime;
using Android.Util;
using AndroidX.AppCompat.App;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace AndroidApp.Setup
{
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@style/AppTheme")]
    public class MainActivity : AppCompatActivity
    {
        private const string TAG = nameof(MainActivity);

        private Button? button;
        private ImageView? imageView;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Log.WriteLine(LogPriority.Info, TAG, "onCreate");

            button = FindViewById<Button>(Resource.Id.button1);
            imageView = FindViewById<ImageView>(Resource.Id.imageView);

            if (button != null) button.Click += Button_Click;
        }

        private async void Button_Click(object? sender, EventArgs e)
        {
            Log.WriteLine(LogPriority.Debug, TAG, "Button Clicked");

            FileResult? result = await MediaPicker.CapturePhotoAsync();

            Log.WriteLine(LogPriority.Debug, TAG, "The photo is capchured");

            if (result != null)
            {
                string localPath = Path.Combine(FileSystem.CacheDirectory, result.FileName);
                using (Stream sourceStream = await result.OpenReadAsync())
                using (FileStream localFileStream = File.OpenWrite(localPath))
                {
                    await sourceStream.CopyToAsync(localFileStream);
                    Android.Net.Uri? uri = Android.Net.Uri.FromFile(new Java.IO.File(localPath));
                    imageView?.SetImageURI(uri);
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}