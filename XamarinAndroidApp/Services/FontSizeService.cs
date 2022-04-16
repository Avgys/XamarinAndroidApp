using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAndroidApp.Settings;

namespace XamarinAndroidApp.Services
{
    [ContentProperty("FontSize")]
    public class FontSizeService : IMarkupExtension
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return DefaultSettings.FontSize;
        }
    }
}