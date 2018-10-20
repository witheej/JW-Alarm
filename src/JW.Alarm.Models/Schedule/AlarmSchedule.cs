

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

        public HashSet<DayOfWeek> DaysOfWeek { get; set; } = new HashSet<DayOfWeek>(new DayOfWeek[] {
            DayOfWeek.Sunday,
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday,
            DayOfWeek.Friday,
            DayOfWeek.Saturday });

        //24 hour based
        public int Hour { get; set; }
        public int MeridienHour => Meridien == Meridien.AM ? Hour : Hour % 12;
        public int Minute { get; set; }

        public Meridien Meridien => Hour < 12 ? Meridien.AM : Meridien.PM;

        public bool MusicEnabled { get; set; } = true;

        public AlarmMusic Music { get; set; }

        public bool BibleEnabled { get; set; } = true;

        public BibleAudio Bible { get; set; }

        public string Key { get; set; }

        public string TimeText => $"{MeridienHour.ToString("D2")}:{Minute.ToString("D2")} {Meridien}";

        public string CronExpression => getCronExpression();

        public PlayType CurrentPlayItem { get; set; }

        public AlarmSchedule()
        {
            //Song number 15 from old song book (Sing Praises to Jehovah! - 1984) as default hymn.
            //"Life without end at last!" is the song theme.
            Music = new MelodyMusic()
            {
                DiscCode = "iam-6",
                TrackNumber = 12
            };

            //Isaiah chapter 1 of English NWT 2013 as default chapter of Bible.
            Bible = new BibleAudio()
            {
                LanguageCode = "E",
                VersionCode = "NWT",
                BookNumber = 23,
                ChapterNumber = 1
            };

        }

        public DateTimeOffset NextFireDate()
        {
            validateTime();

            var expression = new CronExpression(CronExpression);

            validateNextFire(expression);

            return expression.GetNextValidTimeAfter(DateTimeOffset.Now).Value;
        }

        private string getCronExpression()
        {
            string cronExpression = $"0 {Minute} {Hour} ? * ";

            if (DaysOfWeek.Contains(DayOfWeek.Sunday))
            {
                cronExpression += "1";
            }
            for (int i = 1; i < 7; i++)
            {
                if (DaysOfWeek.Contains((DayOfWeek)i))
                {
                    cronExpression += "," + (i + 1).ToString();
                }
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
                throw new Exception("Invalid hour.");
            }

            if (DaysOfWeek == null || DaysOfWeek.Count == 0)
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