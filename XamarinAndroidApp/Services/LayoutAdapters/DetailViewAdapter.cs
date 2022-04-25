using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using XamarinAndroidApp.Models;
using XamarinAndroidApp.Utils;

namespace XamarinAndroidApp.Services
{
    public class DetailViewAdapter<T> : BaseAdapter where T : Processor
    {
        Context context;
        List<string> items;
        public int columnsCount = 8;

        public DetailViewAdapter(Context c, List<T> list)
        {
            context = c;
            items = new List<string>();
            items.Add("Name");
            items.Add("Description");
            items.Add("CodeName");
            items.Add("Socket");
            items.Add("Frequency");
            items.Add("IsMultiThreading");
            items.Add("CoresCount");
            items.Add("TDP");
            for (int i = 0; i < list.Count; i++)
            {
                items.Add(list[i].Name);
                items.Add(list[i].Description);
                items.Add(list[i].CodeName);
                items.Add(list[i].Socket);
                items.Add(list[i].Frequency);
                items.Add(list[i].IsMultiThreading.ToString());
                items.Add(list[i].CoresCount.ToString());
                items.Add(list[i].TDP.ToString());
            }
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
            TextView textView = new TextView(context);
            textView.Text = items[position];
            return textView;
        }

        internal void Update(List<T> list)
        {
            items = new List<string>();
            items.Add("Name");
            items.Add("Description");
            items.Add("CodeName");
            items.Add("Socket");
            items.Add("Frequency");
            items.Add("MultiThread");
            items.Add("CoresCount");
            items.Add("TDP");
            for (int i = 0; i < list.Count; i++)
            {
                items.Add(list[i].Name);
                items.Add(list[i].Description);
                items.Add(list[i].CodeName);
                items.Add(list[i].Socket);
                items.Add(list[i].Frequency);
                items.Add(list[i].IsMultiThreading.ToString());
                items.Add(list[i].CoresCount.ToString());
                items.Add(list[i].TDP.ToString());
            }
        }
    }
}