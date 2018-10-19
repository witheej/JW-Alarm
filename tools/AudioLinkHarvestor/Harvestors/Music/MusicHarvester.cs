using AudioLinkHarvester.Utility;
using JW.Alarm.Models;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace AudioLinkHarvester.Audio
{
    internal class MusicHarverster
    {
        internal static async Task<bool> HarvestMusicLinks(string songBookCode, string musicDiscCode, string languageCode = null)
        {
            var dir = languageCode == null ? $"../../media/Music/Melodies/{songBookCode}/{musicDiscCode}" :
                                             $"../../media/Music/Vocals/{songBookCode}/{languageCode}/{musicDiscCode}";
            var file = $"{dir}/index.json";

            if(File.Exists(file))
            {
                return true;
            }

            var harvestLink = $"https://www.jw.org/apps/TRGCHlZRQVNYVrXF?output=json&pub={musicDiscCode}&fileformat=MP3%2CAAC&alllangs=0{(languageCode == null ? $"&langwritten=E" : $"&langwritten={languageCode}")}&txtCMSLang=E";

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

            var musicDisc = new MusicDisc();
            musicDisc.Title = model.parentPubName;

            var musicFiles = model.files[languageCode ?? "E"].MP3;
            foreach (var musicFile in musicFiles)
            {
                if (musicFile.track == 0)
                    continue;

                musicDisc.Tracks.Add(int.Parse(musicFile.track as string), new MusicTrack()
                {
                    Title = musicFile.title,
                    Url = musicFile.file.url
                });
            }

            if (musicDisc.Tracks.Count > 0)
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(file, JsonConvert.SerializeObject(musicDisc));
                return true;
            }

            return false;
        }

    }
}
