
using System;
using System.IO;

namespace JW.Alarm.Models
{
    public abstract class AlarmAudio
    {
        public string LanguageCode { get; set; }
        public Publication Publication { get; set; }
    }

    public class BibleAudio : AlarmAudio
    {
        public string VersionCode { get; set; }
        public int BookNumber { get; set; }
        public int ChapterNumber { get; set; }
        public int Second { get; set; }

    }
}
