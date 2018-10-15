using System.Collections.Generic;

namespace Models.AudioLinks
{
    public class BibleBook
    {
        public int BookNumber { get; set; }

        public string Name { get; set; }

        public int TotalChapters => Chapters.Count;

        public List<BibleChapter> Chapters { get; set; }

        public BibleBook()
        {
            Chapters = new List<BibleChapter>();
        }
    }

    
}
