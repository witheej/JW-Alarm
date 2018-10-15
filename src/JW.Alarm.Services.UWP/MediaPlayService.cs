using JW.Alarm.Models;
using JW.Alarm.Models.Media;
using JW.Alarm.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace JW.Alarm.Services.UWP
{
    public abstract class MediaPlayService
    {
        private IAlarmService scheduleService;
        private IMediaService mediaService;

        public MediaPlayService(IAlarmService scheduleService, IMediaService mediaService)
        {
            this.scheduleService = scheduleService;
            this.mediaService = mediaService;
        }

        public async Task moveToNextItemToPlay(int alarmId)
        {
            var alarm = (await scheduleService.Schedules)[alarmId];

            switch (alarm.CurrentPlayItem)
            {
                case PlayItem.Music:
                    await moveToNextBibleChapter(alarm, true);
                    break;
                case PlayItem.Bible:
                    await moveToNextBibleChapter(alarm);
                    break;
            }

        }

        private async Task moveToNextBibleChapter(AlarmSchedule alarm, bool switchingPublication = false)
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

        public async Task updateSecondsPlayed(AlarmSchedule alarm, int second)
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

        public async Task<CurrentlyPlaying> nextUrlToPlay(int alarmId)
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

    public class UwpMediaPlayService : MediaPlayService
    {

        private Dictionary<int, MediaPlayer> alarmToMediaPlayersMap = new Dictionary<int, MediaPlayer>();
        private Dictionary<MediaPlayer, int> mediaPlayersToAlarmMap = new Dictionary<MediaPlayer, int>();

        public UwpMediaPlayService(IAlarmService scheduleService, IMediaService mediaService)
            : base(scheduleService, mediaService)
        {
        }

        public async Task Play(int alarmId)
        {
            var nextPlayItem = await nextUrlToPlay(alarmId);

            var mediaPlayer = new MediaPlayer();
            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(nextPlayItem.Url));
            mediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(nextPlayItem.Second);
            mediaPlayer.PlaybackSession.BufferingEnded += onTrackEnd;
            mediaPlayer.Play();

            alarmToMediaPlayersMap.Add(alarmId, mediaPlayer);
            mediaPlayersToAlarmMap.Add(mediaPlayer, alarmId);
        }

        //move to next track on track end
        private async void onTrackEnd(MediaPlaybackSession sender, object args)
        {
            var mediaPlayer = sender.MediaPlayer;

            var alarmId = mediaPlayersToAlarmMap[mediaPlayer];
           
            moveToNextItemToPlay(alarmId).Wait();

            var nextPlayItem = await nextUrlToPlay(alarmId);

            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(nextPlayItem.Url));
            mediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(nextPlayItem.Second);
            mediaPlayer.Play();
        }


        public override async Task Stop(AlarmSchedule alarmSchedule)
        {
            var mediaPlayer = alarmToMediaPlayersMap[alarmSchedule.Id];
            var second = mediaPlayer.PlaybackSession.Position.TotalSeconds;
            await updateSecondsPlayed(alarmSchedule, (int)second < 0 ? 0 : (int)second);
            mediaPlayer.Dispose();

            await base.Stop(alarmSchedule);
        }


    }
}
