using Advanced.Algorithms.DataStructures.Foundation;


namespace JW.Alarm.Models
{
    public class MusicDisc
    {
        public string Title { get; set; }
        public TreeDictionary<int, MusicTrack> Tracks { get; set; }

        public MusicDisc()
        {
            Tracks = new TreeDictionary<int, MusicTrack>();
        }
    }

}
