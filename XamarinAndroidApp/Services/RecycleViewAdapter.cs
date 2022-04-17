using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System.Collections.Generic;
using XamarinAndroidApp.Models;

namespace XamarinAndroidApp.Droid.Services
{
    public class RecyclerViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }
        public TextView Caption { get; private set; }

        public RecyclerViewHolder(View itemView) : base(itemView)
        {
            // Locate and cache view references:
            Image = itemView.FindViewById<ImageView>(Resource.Id.rc_imageView);
            Caption = itemView.FindViewById<TextView>(Resource.Id.rc_textView);
        }
    }

    public class RecycleViewAdapter<T> : RecyclerView.Adapter where T : IItem
    {

        private List<T> list = new List<T>();

        public RecycleViewAdapter(List<T> list)
        {
            this.list = list;
        }
        public override int ItemCount => list.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecyclerViewHolder viewHolder = holder as RecyclerViewHolder;
            viewHolder.Image.SetImageResource(list[position].ImageId);
            viewHolder.Caption.Text = list[position].Caption;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.recycleview_Item, parent, false);
            return new RecyclerViewHolder(itemView);
        }
    }

}