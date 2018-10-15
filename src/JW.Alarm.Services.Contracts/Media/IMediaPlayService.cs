using JW.Alarm.Models;
using JW.Alarm.Models.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JW.Alarm.Services.Contracts
{
    public interface IMediaPlayService
    {
        Task Play(int alarmId);
        Task Stop(AlarmSchedule alarmSchedule);
        Task SetNextItemToPlay(int alarmId);
        Task UpdatePlayedSeconds(AlarmSchedule alarm, int second);
        Task<CurrentlyPlaying> NextUrlToPlay(int alarmId);
    }
}
