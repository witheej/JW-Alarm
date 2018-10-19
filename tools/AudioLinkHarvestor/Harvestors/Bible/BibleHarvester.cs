using AudioLinkHarvester.Utility;
using JW.Alarm.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AudioLinkHarvester.Bible
{
    internal class BibleHarvester
    {
        internal static async Task<bool> HarvestBibleLinks( string languageCode, string editionCode)
        {
            var dir = $"../../media/Audio/Bible/{languageCode}/{editionCode}";
            var file = $"{dir}/index.json";

            if (File.Exists(file))
            {
                return true;
            }

            var harvestLink = $"https://www.jw.org/apps/TRGCHlZRQVNYVrXF?output=json&pub={editionCode}&fileformat=MP3%2CAAC&alllangs=0&langwritten={languageCode}&txtCMSLang=E";

            var jsonString = await DownloadUtility.GetAsync(harvestLink);

            dynamic model;
            try
            {
                model = JsonConvert.DeserializeObject<dynamic>(jsonString);
            }
            catch
            {
                return false;
            }

            var bibleBooks = new Dictionary<int, BibleBook>();
            var bookFiles = model.files[languageCode].MP3;
            foreach (var bookFile in bookFiles)
            {
                string url = bookFile.file.url;

                if (bookFile.track == 0
                || url.EndsWith(".zip")) continue;

                int bookNumber = bookFile.booknum;

                if (!bibleBooks.ContainsKey(bookNumber))
                {
                    bibleBooks[bookNumber] = new BibleBook()
                    {
                        BookNumber = bookFile.booknum,
                        Name = bookFile.title.ToString().Split('-')[0].Trim()
                    };
                }

                bibleBooks[bookNumber].Chapters.Add(new BibleChapter()
                {
                    Chapter = $"Chapter {bookFile.track}",
                    Url = bookFile.file.url
                });
            }

           

            if (bibleBooks.Count > 0)
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(file, JsonConvert.SerializeObject(bibleBooks.Select(x => x.Value)));

                return true;
            }

            return false;
        }

    }
}
