using Autofac;
using JW.Alarm.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Notifications;

namespace JW.Alarm.Services.Uwp.Tasks
{
    public class AlarmTask
    {
        private IScheduleService alarmService;
        private MediaLookUpService mediaLookUpService;

        public AlarmTask(IScheduleService alarmService, MediaLookUpService mediaLookUpService)
        {
            this.alarmService = alarmService;
            this.mediaLookUpService = mediaLookUpService;
        }

        public async void Handle(IBackgroundTaskInstance backgroundTask)
        {
            var deferral = backgroundTask.GetDeferral();

            var details = backgroundTask.TriggerDetails as ToastNotificationHistoryChangedTriggerDetail;

            if (details.ChangeType == ToastHistoryChangedType.Added)
            {
                await mediaLookUpService.Verify();

                var history = ToastNotificationManager.History.GetHistory();

                foreach (var item in history)
                {

                }
           }

            deferral.Complete();
        }
    }
}
