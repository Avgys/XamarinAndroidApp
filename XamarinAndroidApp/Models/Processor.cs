using System;

namespace XamarinAndroidApp.Models
{
    public class Processor : IItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Socket { get; set; }
        public bool IsMultiThreading { get; set; }
        public int CoresCount { get; set; }
        public string Frequency { get; set; }
        public string CodeName { get; set; }
        public int TDP { get; set; }

        public CloudFileData Image { get; set; }
        public CloudFileData Video { get; set; }
        public string Caption { get => Name; }

        public string ImageUri { get => Image?.DownloadUrl; }
        public string Path { get; set; }
    }
}