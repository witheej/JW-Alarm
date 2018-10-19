
using System.IO;

namespace JW.Alarm.Models
{
    public abstract class AlarmMusic
    {
        public MusicType MusicType { get; protected set; }
        public string VersionCode { get; set; }

        //Always play current track or let the alarm move to next track when alarm is fired next time.
        public bool IsFixed { get; set; }
    }

    public class VocalMusic : AlarmMusic
    {
        public string LanguageCode { get; set; }
        public string DiscCode { get; set; }
        public int TrackNumber { get; set; }

        public VocalMusic()
        {
            MusicType = MusicType.Vocals;
        }
    }

    public class MelodyMusic : AlarmMusic
    {
        public string DiscCode { get; set; }
        public int TrackNumber { get; set; }
      
        public int Second { get; set; }

        public MelodyMusic()
        {
            MusicType = MusicType.Melodies;
        }
    }
}
