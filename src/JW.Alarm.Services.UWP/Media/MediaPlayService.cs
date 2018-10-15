using JW.Alarm.Models;
using JW.Alarm.Models.Media;
using JW.Alarm.Services.Contracts;
using JW.Alarm.Services.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;

namespace JW.Alarm.Services.UWP
{

    public class UwpMediaPlayService : MediaPlayService
    {

        private Dictionary<int, MediaPlayer> alarmToMediaPlayersMap = new Dictionary<int, MediaPlayer>();
        private Dictionary<MediaPlayer, int> mediaPlayersToAlarmMap = new Dictionary<MediaPlayer, int>();

        public UwpMediaPlayService(IAlarmService scheduleService, IMediaService mediaService)
            : base(scheduleService, mediaService)
        {
        }

        public override async Task Play(int alarmId)
        {
            var nextPlayItem = await NextUrlToPlay(alarmId);

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
           
            SetNextItemToPlay(alarmId).Wait();

            var nextPlayItem = await NextUrlToPlay(alarmId);

            mediaPlayer.Source = MediaSource.CreateFromUri(new Uri(nextPlayItem.Url));
            mediaPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(nextPlayItem.Second);
            mediaPlayer.Play();
        }


        public override async Task Stop(AlarmSchedule alarmSchedule)
        {
            var mediaPlayer = alarmToMediaPlayersMap[alarmSchedule.Id];
            var second = mediaPlayer.PlaybackSession.Position.TotalSeconds;
            await UpdatePlayedSeconds(alarmSchedule, (int)second < 0 ? 0 : (int)second);
            mediaPlayer.Dispose();

            await base.Stop(alarmSchedule);
        }


    }
}
