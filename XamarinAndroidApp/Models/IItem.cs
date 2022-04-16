using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinAndroidApp.Models
{
    public interface IItem
    { 
        int ImageId { get; }
        string Caption { get; }
    }
}
