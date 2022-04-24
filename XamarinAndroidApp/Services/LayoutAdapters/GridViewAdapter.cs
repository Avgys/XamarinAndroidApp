using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
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

        public override void NotifyDataSetChanged()
        {
            base.NotifyDataSetChanged();
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
                imageView.LayoutParameters = new AbsListView.LayoutParams(180, 180);
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

        internal void Update(List<T> entityList)
        {
            items = entityList;
        }
    }
}