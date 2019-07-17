using System;
using System.Collections.Generic;
using System.Linq;

namespace WebsiteChecker
{
    interface IUrlDataComparator<T>
    {
        List<string> CompareData(T lastData, T newData, out bool change);
    }

    class ArrayOfStringComparator : IUrlDataComparator<string[]>
    {
        public List<string> CompareData(string[] lastData, string[] newData, out bool change)
        {
            var messages = new List<string>();
            change = false;
            if ((lastData == null || lastData.Length == 0) && (newData == null || newData.Length == 0)) return messages;
            change = true;
            if(lastData == null || lastData.Length == 0)
            {
                messages.Add("New data showed up!");
                messages.Add($"There are {newData.Length} new entries");
                return messages;
            }
            if(newData == null || newData.Length == 0)
            {
                messages.Add("New data is empty!");
                return messages;
            }
            if (lastData.Length == newData.Length && lastData.Zip(newData, (s1, s2) => s1 == s2).All(b => b))
            {
                change = false;
                return messages;
            }
            if(lastData[0] != newData[0])
            {
                var idx = Array.FindIndex(newData, (x) => x == lastData[0]);
                messages.Add("New data showed up!");
                if(idx > 0 ) messages.Add($"There are probably {idx} new entries");
                return messages;
            }
            return messages;
            
        }
    }

}