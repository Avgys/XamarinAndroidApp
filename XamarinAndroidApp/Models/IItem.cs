using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinAndroidApp.Models
{
    public interface IItem
    { 
        string ImageUri { get; }
        string Caption { get; }
        public string Id { get; set; }
        public string Path { get; set; }
    }
}
