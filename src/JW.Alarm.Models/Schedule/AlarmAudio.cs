
using System;
using System.IO;

namespace JW.Alarm.Models
{

    public class BibleAudio 
    {
        public string LanguageCode { get; set; }
        public string VersionCode { get; set; }
        public int BookNumber { get; set; }
        public int ChapterNumber { get; set; }
        public int Second { get; set; }

    }
}
