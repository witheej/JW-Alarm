using Advanced.Algorithms.DataStructures.Foundation;

namespace JW.Alarm.Models
{
    public class BibleBook
    {
        public int BookNumber { get; set; }

        public string Name { get; set; }

        public int TotalChapters => Chapters.Count;

        public TreeDictionary<int, BibleChapter> Chapters { get; set; }

        public BibleBook()
        {
            Chapters = new TreeDictionary<int, BibleChapter>();
        }
    }


}
