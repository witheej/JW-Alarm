using JW.Alarm.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JW.Alarm.Models;
using System.IO;
using Newtonsoft.Json;

namespace JW.Alarm.Services.Scheduler
{
    public abstract class AlarmService : IAlarmService
    {
        private Random random = new Random();
        private IStorageService storageService;
        private Lazy<string> scheduleFilePath;
        private Dictionary<int, AlarmSchedule> schedules;

        public Task<Dictionary<int, AlarmSchedule>> Schedules => getSchedules();

        public AlarmService(IStorageService storageService)
        {
            this.storageService = storageService;
            scheduleFilePath = new Lazy<string>(() => Path.Combine(this.storageService.StorageRoot, "schedules.json"));
        }

        public virtual async Task Create(AlarmSchedule alarm)
        {
            await getSchedules();

            alarm.Id = getNextId();
            schedules.Add(alarm.Id, alarm);

            await saveChanges();
        }

        public virtual async Task Delete(int alarmId)
        {
            await getSchedules();

            schedules.Remove(alarmId);

            await saveChanges();
        }

        public virtual async Task Update(AlarmSchedule schedule)
        {
            await getSchedules();

            schedules[schedule.Id] = schedule;

            await saveChanges();
        }

        private async Task<Dictionary<int, AlarmSchedule>> getSchedules()
        {
            if (schedules == null)
            {
                if(await storageService.FileExists(scheduleFilePath.Value))
                {
                    schedules = JsonConvert.DeserializeObject<Dictionary<int, AlarmSchedule>>(await storageService.ReadFile(scheduleFilePath.Value));
                }
                else
                {
                    schedules = new Dictionary<int, AlarmSchedule>();
                }  
            }

            return schedules;
        }

        private async Task saveChanges()
        {
            await storageService.SaveFile(storageService.StorageRoot, "schedules.json", JsonConvert.SerializeObject(schedules));
        }

        private int getNextId()
        {
            var candidate = random.Next();

            while(schedules.ContainsKey(candidate))
            {
                candidate = random.Next();
            }

            return candidate;
        }
    }
}
