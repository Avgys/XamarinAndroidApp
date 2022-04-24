using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XamarinAndroidApp.Models;

namespace XamarinAndroidApp.Services
{
    public static class FirebaseObjectMapper
    {
        public static Processor Unwrap(this FirebaseObject<Processor> item)
        {
            return new Processor()
            {
                Id = item.Object.Id,
                Name = item.Object.Name,
                Description = item.Object.Description,
                Socket = item.Object.Socket,
                IsMultiThreading = item.Object.IsMultiThreading,
                CoresCount = item.Object.CoresCount,
                Frequency = item.Object.Frequency,
                CodeName = item.Object.CodeName,
                TDP = item.Object.TDP,
                Image = new CloudFileData
                {
                    FileName = item.Object.Image?.FileName ?? "",
                    DownloadUrl = item.Object.Image?.DownloadUrl ??
                                  "https://www.generationsforpeace.org/wp-content/uploads/2018/03/empty.jpg"
                },
                Video = new CloudFileData
                {
                    FileName = item.Object.Video?.FileName ?? "",
                    DownloadUrl = item.Object.Video?.DownloadUrl ?? ""
                }, 
                Path = item.Key
            };
        }
    }
}