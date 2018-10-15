using System.Collections.Generic;

namespace JW.Alarm.Models
{
    public class MusicDisc
    {
        public string Title { get; set; }
        public List<MusicTrack> Tracks { get; set; }

        public MusicDisc()
        {
            Tracks = new List<MusicTrack>();
        }
    }

}
