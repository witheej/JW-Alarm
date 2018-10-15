using JW.Alarm.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JW.Alarm.Services.Contracts
{
    public interface IMediaService
    {
        Task<Dictionary<string, string>> GetBibleLanguages();
        Task<Dictionary<string, string>> GetBibleVersions(string languageCode);

        Task<List<BibleBook>> GetBibleBooks(string languageCode, string versionCode);
        Task<List<BibleChapter>> GetBibleChapters(string languageCode, string versionCode, int bookNumber);

        Task<List<MusicDisc>> GetMelodyMusicDiscs();
        Task<List<MusicTrack>> GetMelodyMusicTracks(string discCode);

        Task<Dictionary<string, string>> GetVocalMusicLanguages();
        Task<List<MusicDisc>> GetVocalMusicDiscs(string languageCode);
        Task<List<MusicTrack>> GetVocalMusicTracks(string languageCode, string discCode);
    }
}
