using JW.Alarm.Models;
using JW.Alarm.Models.Media;
using JW.Alarm.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JW.Alarm.Services.Media
{
    public abstract class MediaPlayService : IMediaPlayService
    {
        private IAlarmService scheduleService;
        private IMediaService mediaService;

        public MediaPlayService(IAlarmService scheduleService, IMediaService mediaService)
        {
            this.scheduleService = scheduleService;
            this.mediaService = mediaService;
        }

        public abstract Task Play(int alarmId);

        public async Task SetNextItemToPlay(int alarmId)
        {
            var alarm = (await scheduleService.Schedules)[alarmId];

            switch (alarm.CurrentPlayItem)
            {
                case PlayItem.Music:
                    await setNextBibleChapter(alarm, true);
                    break;
                case PlayItem.Bible:
                    await setNextBibleChapter(alarm);
                    break;
            }

        }

        private async Task setNextBibleChapter(AlarmSchedule alarm, bool switchingPublication = false)
        {
            if (switchingPublication)
            {
                alarm.CurrentPlayItem = PlayItem.Bible;
                await scheduleService.Update(alarm);
                return;
            }

            var bible = alarm.Audio as BibleAudio;
            var chapters = await mediaService.GetBibleChapters(bible.LanguageCode, bible.VersionCode, bible.BookNumber);
            if (bible.ChapterNumber < chapters.Count)
            {
                bible.ChapterNumber++;
                await scheduleService.Update(alarm);
                return;
            }

            var books = await mediaService.GetBibleBooks(bible.LanguageCode, bible.VersionCode);

            var currentBook = books.First(x => x.BookNumber == bible.BookNumber);
            var nextBook = books.Skip((books.IndexOf(currentBook) + 1) % books.Count).First();

            bible.BookNumber = nextBook.BookNumber;
            await scheduleService.Update(alarm);
            return;


        }

        public async Task UpdatePlayedSeconds(AlarmSchedule alarm, int second)
        {
            switch (alarm.CurrentPlayItem)
            {
                case PlayItem.Music:
                    break;
                case PlayItem.Bible:
                    (alarm.Audio as BibleAudio).Second = second;
                    await scheduleService.Update(alarm);
                    break;
            }
        }

        public async Task<CurrentlyPlaying> NextUrlToPlay(int alarmId)
        {
            var schedule = (await scheduleService.Schedules)[alarmId];

            if (schedule.CurrentPlayItem == PlayItem.Music)
            {
                return await nextMusicUrlToPlay(schedule);
            }
            else
            {
                return await nextBibleUrlToPlay(schedule);
            }
        }

        private async Task<CurrentlyPlaying> nextMusicUrlToPlay(AlarmSchedule schedule)
        {
            switch (schedule.Music.MusicType)
            {
                case MusicType.Melodies:
                    var melodyMusic = schedule.Music as MelodyMusic;
                    var melodyTracks = await mediaService.GetMelodyMusicTracks(melodyMusic.DiskCode);
                    var melodyTrack = melodyTracks[melodyMusic.TrackNumber];
                    return new CurrentlyPlaying(melodyTrack.Url, 0);

                case MusicType.Vocals:
                    var vocalMusic = schedule.Music as VocalMusic;
                    var vocalTracks = await mediaService.GetVocalMusicTracks(vocalMusic.LanguageCode, vocalMusic.DiskCode);
                    var vocalTrack = vocalTracks[vocalMusic.TrackNumber];
                    return new CurrentlyPlaying(vocalTrack.Url, 0);
            }

            return null;
        }

        private async Task<CurrentlyPlaying> nextBibleUrlToPlay(AlarmSchedule schedule)
        {
            switch (schedule.Audio.Publication)
            {
                case Publication.Bible:
                    var bibleAudio = schedule.Audio as BibleAudio;
                    var bibleTracks = await mediaService.GetBibleChapters(bibleAudio.LanguageCode, bibleAudio.VersionCode, bibleAudio.BookNumber);
                    var bibleTrack = bibleTracks[bibleAudio.ChapterNumber];
                    return new CurrentlyPlaying(bibleTrack.Url, bibleAudio.Second);
            }

            return null;
        }

        public async virtual Task Stop(AlarmSchedule alarmSchedule)
        {
            await scheduleService.Update(alarmSchedule);
        }
    }
}
