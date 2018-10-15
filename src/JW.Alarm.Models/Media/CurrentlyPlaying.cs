using System;
using System.Collections.Generic;
using System.Text;

namespace JW.Alarm.Models.Media
{
    public class CurrentlyPlaying
    {
        public string Url { get; set; }
        public int Second { get; set; }

        public CurrentlyPlaying(string url, int second)
        {
            Url = url;
            Second = second;
        }
    }
}
