using System;

namespace XamarinAndroidApp.Models
{
    public class Item : IItem
    {
        public string ImageUri => "";

        public string Caption => "";

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}