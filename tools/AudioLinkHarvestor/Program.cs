using AudioLinkHarvester.Audio;
using AudioLinkHarvester.Bible;
using AudioLinkHarvester.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace AudioLinkHarvester
{
    class Program
    {
        private static Dictionary<string, string> bibleCodes = new Dictionary<string, string>(new KeyValuePair<string, string>[]{
            new KeyValuePair<string, string>("nwt","NWT 1984"),
            new KeyValuePair<string, string>("bi12","NWT 2013")
        });
        /// <summary>
        /// harvest URL links from Mobile.ORG to get the mp3 files liks for Bible & Music 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var tasks = new List<Task>();

            //Bible
            tasks.Add(Harvest_Bible_Links());

            // Music
            tasks.Add(Harvest_SingToJehovah_Vocal_Links());
            tasks.Add(Harvest_KingdomMelody_Links());

            Task.WaitAll(tasks.ToArray());

            var index = new
            {
                ReleaseDate = DateTime.Now.Ticks
            };

            var indexFile = "../../index.json";
            if (File.Exists(indexFile))
            {
                File.Delete(indexFile);
            }

            File.WriteAllText(indexFile, JsonConvert.SerializeObject(index));

            ZipFiles(index.ReleaseDate);
        }

        private static void ZipFiles(long ticks)
        {
            var zipIndex = $"../../index.zip";
            if (File.Exists(zipIndex))
            {
                File.Delete(zipIndex);
            }

            ZipFile.CreateFromDirectory("../../media", zipIndex);
            // Directory.Delete("../../media", true);
        }

        private async static Task Harvest_Bible_Links()
        {
            var tasks = new List<Task>();

            var languageEditionsMapping = new Dictionary<string, List<string>>();
            var languageCodes = new Dictionary<string, string>();

            foreach (var edition in bibleCodes)
            {
                var editionCode = edition.Key;

                var harvestLink = $"https://www.jw.org/apps/TRGCHlZRQVNYVrXF?booknum=0&output=json&pub={editionCode}&fileformat=MP3%2CAAC&alllangs=1&langwritten=E&txtCMSLang=E";

                var jsonString = await DownloadUtility.GetAsync(harvestLink);
                var model = JsonConvert.DeserializeObject<dynamic>(jsonString);

                foreach (var item in model.languages)
                {
                    var languageCode = item.Name;
                    var language = model.languages[item.Name]["name"].Value;

                    bool result = false;
                    languageCodes[languageCode] = language;

                    try
                    {
                        result = await BibleHarvester.HarvestBibleLinks(languageCode, editionCode);
                    }
                    catch { }

                    if (result)
                    {
                        if (languageEditionsMapping.ContainsKey(languageCode))
                        {
                            languageEditionsMapping[languageCode].Add(edition.Key);
                        }
                        else
                        {
                            languageEditionsMapping[languageCode] = new List<string>(new[] { edition.Key });
                        }
                    }
                }
            }


            if (!Directory.Exists($"../../media/Audio/Bible"))
            {
                Directory.CreateDirectory($"../../media/Audio/Bible");
            }

            File.WriteAllText($"../../media/Audio/Bible/languages.json", JsonConvert.SerializeObject(
                languageEditionsMapping.Select(x => new
                {
                    Key = x.Key,
                    Value = languageCodes[x.Key]
                }).ToList()));

            foreach (var languageEditionsMap in languageEditionsMapping)
            {
                if (!Directory.Exists($"../../media/Audio/Bible/{languageEditionsMap.Key}"))
                {
                    Directory.CreateDirectory($"../media/Audio/Bible/{languageEditionsMap.Key}");
                }

                File.WriteAllText($"../../media/Audio/Bible/{languageEditionsMap.Key}/editions.json", JsonConvert.SerializeObject(
                languageEditionsMap.Value.Select(x => new
                {
                    Key = x,
                    Value = bibleCodes[x]
                })));
            }

        }

        private async static Task Harvest_SingToJehovah_Vocal_Links()
        {
            var tasks = new List<Task>();

            var languageDiscs = new Dictionary<string, List<string>>();
            var languageCodes = new Dictionary<string, string>();
            var discCodes = new Dictionary<string, string>();

            for (int i = 1; i <= 6; i++)
            {
                var currentDisc = string.Format("Disc {0}", i);
                var currentDiscCode = $"iasnv{i}";
                discCodes.Add(currentDiscCode, currentDisc);

                var harvestLink = $"https://www.jw.org/apps/TRGCHlZRQVNYVrXF?booknum=0&output=json&pub={currentDiscCode}&fileformat=MP3%2CAAC&alllangs=1&langwritten=E&txtCMSLang=E";

                var jsonString = await DownloadUtility.GetAsync(harvestLink);
                var model = JsonConvert.DeserializeObject<dynamic>(jsonString);

                foreach (var item in model.languages)
                {
                    var languageCode = item.Name;
                    var language = model.languages[item.Name]["name"].Value;

                    languageCodes[languageCode] = language;

                    await MusicHarverster.HarvestMusicLinks("iasnv", currentDiscCode, languageCode);

                    if (languageDiscs.ContainsKey(languageCode))
                    {
                        languageDiscs[languageCode].Add(currentDiscCode);
                    }
                    else
                    {
                        languageDiscs[languageCode] = new List<string>(new[] { currentDiscCode });
                    }

                }
            }

            if (!Directory.Exists($"../../media/Music/Vocals/iasnv"))
            {
                Directory.CreateDirectory($"../../media/music/Vocals/iasnv");
            }

            File.WriteAllText($"../../media/Music/Vocals/index.json", JsonConvert.SerializeObject(
             new [] { new { Key = "iasnv", Value = "Sing to Jehovah (2009)" } }));


            File.WriteAllText($"../../media/Music/Vocals/iasnv/index.json", JsonConvert.SerializeObject(
            languageDiscs.Select(x => new
            {
                Key = x.Key,
                Value = languageCodes[x.Key]
            }).ToList()));

            foreach (var discs in languageDiscs)
            {
                if (!Directory.Exists($"../../media/Music/Vocals/iasnv/{discs.Key}"))
                {
                    Directory.CreateDirectory($"../../media/Music/Vocals/iasnv/{discs.Key}");
                }

                File.WriteAllText($"../../media/Music/Vocals/iasnv/{discs.Key}/index.json", JsonConvert.SerializeObject(
                 discs.Value.Select(x => new
                 {
                     Key = x,
                     Value = discCodes[x]
                 })));
            }

        }

        private async static Task Harvest_KingdomMelody_Links()
        {
            var discs = new List<string>();
            var discCodes = new Dictionary<string, string>();

            for (int i = 1; i <= 9; i++)
            {
                if (i == 7 || i == 8) continue;

                var currentDisc = $"Kingdom Melodies - Disk {i}";
                var currentDiscCode = $"iam-{i}";
                discCodes.Add(currentDiscCode, currentDisc);

                discs.Add(currentDiscCode);

                await MusicHarverster.HarvestMusicLinks("iam", currentDiscCode);
            }

            if (!Directory.Exists($"../../media/Music/Melodies/iam"))
            {
                Directory.CreateDirectory($"../../media/Music/Melodies/iam");
            }

            File.WriteAllText($"../../media/Music/Melodies/index.json", JsonConvert.SerializeObject(
            new[] { new { Key = "iam", Value = "Sing Praises to Jehovah (1984)" } }));

            File.WriteAllText($"../../media/Music/Melodies/iam/index.json", JsonConvert.SerializeObject(
            discs.Select(x => new
            {
                Key = x,
                Value = discCodes[x]
            })));

        }

    }
}
