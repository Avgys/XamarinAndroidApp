using Android.Content;
using Android.Graphics;
using System.Net;

namespace XamarinAndroidApp.Utils
{
    public static class ImageLoader
    {
        public static Bitmap GetBitmapFromUrl(Context c, string url)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    byte[] bytes = webClient.DownloadData(url);
                    if (bytes != null && bytes.Length > 0)
                    {
                        return BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
                    }
                }
            }
            catch
            {
                return BitmapFactory.DecodeResource(c.Resources, Resource.Drawable.missing);
            }

            return null;
        }
    }
}