using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinAndroidApp.Resources;
using XamarinAndroidApp.Settings;

namespace XamarinAndroidApp.Services
{
    [ContentProperty("Text")]
    public class LanguageService : IMarkupExtension
    {
        private const string LanguageResource = "XamarinAndroidApp.Resources.AppContentText";
        private readonly CultureInfo _cultureInfo;

        public string Text { get; set; }

        public LanguageService()
        {
            _cultureInfo = new CultureInfo(DefaultSettings.Language);
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return "";
            }

            var resourceManager = new ResourceManager(LanguageResource, typeof(LanguageService).GetTypeInfo().Assembly);

            string translation = resourceManager.GetString(Text, _cultureInfo);

            if (translation == null)
            {
                translation = Text;
            }

            return translation;
        }
    }
}