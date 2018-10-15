using JW.Alarm.Services.Contracts;
using JW.Alarm.Services.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace JW.Alarm.Services.Uwp.Tasks
{
    public class SchedulerTask 
    {
        private IAlarmService alarmService;
        public SchedulerTask(IAlarmService alarmService)
        {
            this.alarmService = alarmService;
        }

        public void Handle(IBackgroundTaskInstance backgroundTask)
        {

        }
    }
}
