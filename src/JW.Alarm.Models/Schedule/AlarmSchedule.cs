

using Quartz;
using System;
using System.Collections.Generic;

namespace JW.Alarm.Models
{
    public class AlarmSchedule
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; } = true;

        public DayOfWeek[] DaysOfWeek { get; set; }

        //24 hour based
        public int Hour { get; set; }
        public int MeridienHour => Meridien == Meridien.AM ? Hour : Hour % 12;
        public int Minute { get; set; }

        public Meridien Meridien => Hour < 12 ? Meridien.AM : Meridien.PM;

        public AlarmMusic Music { get; set; }
        public BibleAudio Bible { get; set; }

        public string Key { get; set; }

        public string TimeText => $"{MeridienHour.ToString("D2")}:{Minute.ToString("D2")} {Meridien}";

        public string CronExpression => getCronExpression();

        public PlayType CurrentPlayItem { get; set; }

        public DateTimeOffset NextFireDate()
        {
            validateTime();

            var expression = new CronExpression(CronExpression);

            validateNextFire(expression);

            return expression.GetNextValidTimeAfter(DateTimeOffset.Now).Value;
        }

        private string getCronExpression()
        {
            string cronExpression = $"0 {Minute} {Hour} ? * {(int)DaysOfWeek[0] + 1}";

            for (int i = 1; i < DaysOfWeek.Length; i++)
            {
                cronExpression = cronExpression + "," + ((int)DaysOfWeek[i] + 1);
            }

            var expression = new CronExpression(cronExpression);
            return expression.CronExpressionString;
        }

        private void validateTime()
        {

            if (Minute < 0 || Minute >= 60)
            {
                throw new Exception("Invalid minute.");
            }

            if (Hour < 0 || Hour >= 24)
            {
                throw new Exception("Invalid minute.");
            }

            if (DaysOfWeek == null || DaysOfWeek.Length == 0)
            {
                throw new Exception("DaysOfWeek is empty.");
            }
        }

        private void validateNextFire(CronExpression expression)
        {
            var nextFire = expression.GetNextValidTimeAfter(DateTimeOffset.Now);

            if (nextFire == null)
            {
                throw new Exception("Invalid alarm time.");
            }
        }
    }

}