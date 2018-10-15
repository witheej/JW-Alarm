using System.Collections.Generic;
using System.Threading.Tasks;
using JW.Alarm.Models;

namespace JW.Alarm.Services.Contracts
{
    public interface IAlarmService
    {
        Task<Dictionary<int, AlarmSchedule>> Schedules { get; }
        
        Task Create(AlarmSchedule alarm);
        Task Update(AlarmSchedule schedule);
        Task Delete(int alarmId);
        
    }
}
