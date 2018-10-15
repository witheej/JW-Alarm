using JW.Alarm.Models;
using JW.Alarm.Services.Contracts;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace JW.Alarm.Services.Media
{
    public class MediaService : IMediaService
    {
        private MediaLookUpService mediaLookUpService;
        private IStorageService storageService;

        public MediaService(MediaLookUpService mediaLookUpService, IStorageService storageService)
        {
            this.mediaLookUpService = mediaLookUpService;
            this.storageService = storageService;
        }

        public async Task<Dictionary<string, string>> GetBibleLanguages()
        {
            var root = mediaLookUpService.IndexRoot;
            var languageIndex = Path.Combine(root, "Audio", "Bible", "index.json");
            var languages = await storageService.ReadFile(languageIndex);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(languages);
        }

        public async Task<Dictionary<string, string>> GetBibleVersions(string languageCode)
        {
            var root = mediaLookUpService.IndexRoot;
            var bibleIndex = Path.Combine(root, "Audio", "Bible", languageCode, "index.json");
            var bibleVersions = await storageService.ReadFile(bibleIndex);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(bibleVersions);
        }

        public async Task<List<BibleBook>> GetBibleBooks(string languageCode, string versionCode)
        {
            var root = mediaLookUpService.IndexRoot;
            var booksIndex = Path.Combine(root, "Audio", "Bible", languageCode, versionCode, "index.json");
            var bibleBooks = await storageService.ReadFile(booksIndex);
            return JsonConvert.DeserializeObject<List<BibleBook>>(bibleBooks);
        }

        public async Task<List<BibleChapter>> GetBibleChapters(string languageCode, string versionCode, int bookNumber)
        {
            var root = mediaLookUpService.IndexRoot;
            var chaptersIndex = Path.Combine(root, "Audio", "Bible", languageCode, versionCode, bookNumber.ToString(), "index.json");
            var bibleChapters = await storageService.ReadFile(chaptersIndex);
            return JsonConvert.DeserializeObject<List<BibleChapter>>(bibleChapters);
        }


        public async Task<List<MusicDisc>> GetMelodyMusicDiscs()
        {
            var root = mediaLookUpService.IndexRoot;
            var discIndex = Path.Combine(root, "Music", "Melodies", "index.json");
            var melodyDiscs = await storageService.ReadFile(discIndex);
            return JsonConvert.DeserializeObject<List<MusicDisc>>(melodyDiscs);
        }


        public async Task<List<MusicTrack>> GetMelodyMusicTracks(string discCode)
        {
            var root = mediaLookUpService.IndexRoot;
            var trackIndex = Path.Combine(root, "Music", "Melodies", discCode, "index.json");
            var melodyDiscs = await storageService.ReadFile(trackIndex);
            return JsonConvert.DeserializeObject<List<MusicTrack>>(melodyDiscs);
        }


        public async Task<Dictionary<string, string>> GetVocalMusicLanguages()
        {
            var root = mediaLookUpService.IndexRoot;
            var trackIndex = Path.Combine(root, "Music", "Vocals", "index.json");
            var melodyDiscs = await storageService.ReadFile(trackIndex);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(melodyDiscs);
        }


        public async Task<List<MusicDisc>> GetVocalMusicDiscs(string languageCode)
        {
            var root = mediaLookUpService.IndexRoot;
            var discIndex = Path.Combine(root, "Music", "Vocals", languageCode, "index.json");
            var vocalDiscs = await storageService.ReadFile(discIndex);
            return JsonConvert.DeserializeObject<List<MusicDisc>>(vocalDiscs);
        }

        public async Task<List<MusicTrack>> GetVocalMusicTracks(string languageCode, string discCode)
        {
            var root = mediaLookUpService.IndexRoot;
            var trackIndex = Path.Combine(root, "Music", "Vocals", languageCode, discCode, "index.json");
            var melodyDiscs = await storageService.ReadFile(trackIndex);
            return JsonConvert.DeserializeObject<List<MusicTrack>>(melodyDiscs);
        }

    }
}
