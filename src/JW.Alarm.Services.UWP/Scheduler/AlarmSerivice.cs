using JW.Alarm.Models;
using JW.Alarm.Services.Contracts;
using JW.Alarm.Services.Scheduler;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace JW.Alarm.Services.Uwp
{
    public class UwpAlarmService : AlarmService
    {
        public UwpAlarmService(IStorageService storageService)
            : base(storageService)
        {
        }

        public override async Task Create(AlarmSchedule alarmSchedule)
        {
            createNotification(alarmSchedule);
            await base.Create(alarmSchedule);
        }

        public override async Task Delete(int alarmId)
        {
            removeNotification(alarmId.ToString());
            await base.Delete(alarmId);
        }

        public override async Task Update(AlarmSchedule alarmSchedule)
        {
            removeNotification(alarmSchedule.Id.ToString());
            createNotification(alarmSchedule);

            await base.Update(alarmSchedule);
        }

        private void createNotification(AlarmSchedule alarmSchedule)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();

            var scheduledNotification = generateAlarmNotification(alarmSchedule, alarmSchedule.NextFireDate());
            notifier.AddToSchedule(scheduledNotification);
        }

        private bool removeNotification(string tagName)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();

            var notifications = notifier.GetScheduledToastNotifications();

            var notification = notifications.FirstOrDefault(x => x.Tag == tagName);

            if (notification != null)
            {
                notifier.RemoveFromSchedule(notification);
                return true;
            }

            return false;
        }

        private static ScheduledToastNotification generateAlarmNotification(AlarmSchedule alarm, DateTimeOffset alarmTime)
        {
            var content = new ToastContent()
            {
                Scenario = ToastScenario.Reminder,
                DisplayTimestamp = alarmTime,
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "Alarm",
                                HintMaxLines = 1
                            },
                            new AdaptiveText()
                            {
                                Text = alarm.Description
                            }
                        }
                    }
                },

                Actions = new ToastActionsSnoozeAndDismiss()
            };

            // We can easily enable Universal Dismiss by generating a RemoteId for the alarm that will be
            // the same on both devices. We'll just use the alarm delivery time. If an alarm on one device
            // has the same delivery time as an alarm on another device, it'll be dismissed when one of the
            // alarms is dismissed.
            string remoteId = (alarmTime.Ticks / 10000000 / 60).ToString(); // Minutes

            return new ScheduledToastNotification(content.GetXml(), alarmTime)
            {
                Group = "sample",
                Tag = alarm.Id.ToString(),
                RemoteId = remoteId
            };
        }

    }
}
