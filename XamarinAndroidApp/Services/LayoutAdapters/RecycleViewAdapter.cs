using Android.Content;
using Android.Graphics;
using Android.Net;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using System.Net;
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Utils;

namespace XamarinAndroidApp.Droid.Services
{
    public class RecyclerViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }
        public TextView Caption { get; private set; }

        public RecyclerViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            // Locate and cache view references:
            Image = itemView.FindViewById<ImageView>(Resource.Id.rc_imageView);
            Caption = itemView.FindViewById<TextView>(Resource.Id.rc_textView);

            itemView.Click += (sender, e) => listener(base.LayoutPosition);
        }
    }

    public class RecycleViewAdapter<T> : RecyclerView.Adapter where T : IItem
    {

        public event EventHandler<T> ItemClick;
        private Context context;
        private List<T> list = new List<T>();
        public override int ItemCount => list.Count;

        public RecycleViewAdapter(Context c, List<T> l)
        {
            context = c;
            list = l;
        }

        void OnClick(int position)
        {
            
            if (ItemClick != null)
                ItemClick(this, this.list[position]);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecyclerViewHolder viewHolder = holder as RecyclerViewHolder;
            var bitmap = ImageLoader.GetBitmapFromUrl(context, list[position].ImageUri);
            
            viewHolder.Image.SetImageBitmap(bitmap);
            viewHolder.Caption.Text = list[position].Caption;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.recycleview_Item, parent, false);
            return new RecyclerViewHolder(itemView, OnClick);
        }

        internal void Update(List<T> entityList)
        {
            list = entityList;
        }
    }
}