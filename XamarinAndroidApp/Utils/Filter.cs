using System;
using System.Collections.Generic;
using System.Linq;
using XamarinAndroidApp.Models;

namespace XamarinAndroidApp.Utils
{
    public class Filter
    {
        public string name;
        public string description;
        public string socket;
        public bool isMultiThreadingYes;
        public bool isMultiThreadingNo;
        public bool isMultiThreadingAny;
        public int coresCountLeft;
        public int coresCountRight;
        public string frequency;
        public string codeName;
        public int tdpLeft;
        public int tdpRight;

        public Filter(string name, string description, string socket, bool isMultiThreadingYes, 
            bool isMultiThreadingNo, bool isMultiThreadingAny, string coresCountLeft, string coresCountRight, string frequency, string codeName, string tdpLeft, string tdpRight)
        {
            this.name = name;
            this.description = description;
            this.socket = socket;
            this.isMultiThreadingYes = isMultiThreadingYes;
            this.isMultiThreadingNo = isMultiThreadingNo;
            this.isMultiThreadingAny = isMultiThreadingAny;
            ;
            if (!int.TryParse(coresCountLeft, out this.coresCountLeft)) this.coresCountLeft = 0;
            ;
            if (!int.TryParse(coresCountRight, out this.coresCountRight)) this.coresCountRight = int.MaxValue;
            if (this.coresCountLeft > this.coresCountRight) this.coresCountRight = this.coresCountLeft;

            this.frequency = frequency;
            this.codeName = codeName;

            if (!int.TryParse(tdpLeft, out this.tdpLeft)) this.tdpLeft = 0;
            if (!int.TryParse(tdpRight, out this.tdpRight)) this.tdpRight = int.MaxValue;
            if (this.tdpLeft > this.tdpRight) this.tdpRight = this.tdpLeft;
        }

        internal List<Processor> FilterList(List<Processor> entities)
        {
            return entities
                .Where(x => string.IsNullOrWhiteSpace(this.name) ? true : x.Name.Contains(this.name))
                .Where(x => string.IsNullOrWhiteSpace(this.description) ? true : x.Description.Contains(this.description))
                .Where(x => isMultiThreadingAny ? true : (this.isMultiThreadingYes ? x.IsMultiThreading : !x.IsMultiThreading))
                .Where(x => x.CoresCount >= coresCountLeft && x.CoresCount <= coresCountRight)
                .Where(x => string.IsNullOrWhiteSpace(this.frequency) ? true : x.Frequency.Contains(this.frequency))
                .Where(x => string.IsNullOrWhiteSpace(this.codeName) ? true : x.CodeName.Contains(this.codeName))
                .Where(x => x.TDP >= tdpLeft && x.TDP <= tdpRight)
                .ToList();
        }
    }
}