using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAndroidApp.Settings;

namespace XamarinAndroidApp.Services
{
    [ContentProperty("FontFamily")]
    public class FontFamilyService : IMarkupExtension
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return DefaultSettings.FontFamily;
        }
    }
}