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
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Utils;

namespace XamarinAndroidApp.Services
{
    public class GridViewAdapter<T> : BaseAdapter where T : IItem
    {
        Context context;
        List<T> items;


        public GridViewAdapter(Context c, List<T> list)
        {
            context = c;
            items = list;
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return 0;
        }

        // create a new ImageView for each item referenced by the Adapter
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ImageView imageView;

            if (convertView == null)
            {  // if it's not recycled, initialize some attributes
                imageView = new ImageView(context);
                imageView.LayoutParameters = new GridView.LayoutParams(85, 85);
                imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
                imageView.SetPadding(8, 8, 8, 8);
            }
            else
            {
                imageView = (ImageView)convertView;
            }


            var bitmap = ImageLoader.GetBitmapFromUrl(context, items[position].ImageUri);

            imageView.SetImageBitmap(bitmap);
            return imageView;
        }
    }
}