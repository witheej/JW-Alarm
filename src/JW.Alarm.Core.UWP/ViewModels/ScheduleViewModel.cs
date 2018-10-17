using JW.Alarm.Models;
using JW.Alarm.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace JW.Alarm.Core.UWP.ViewModels
{

    public class ScheduleViewModel : BindableBase
    {
        IScheduleService scheduleService;
        public ScheduleViewModel(IScheduleService scheduleService, AlarmSchedule model = null)
        {
            this.scheduleService = scheduleService;
            Model = model ?? new AlarmSchedule();
        }

        private AlarmSchedule model;

        public AlarmSchedule Model
        {
            get => model;
            set
            {
                if (model != value)
                {
                    model = value;
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        public string Description
        {
            get => Model.Description;
            set
            {
                if (value != Model.Description)
                {
                    Model.Description = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEnabled
        {
            get => Model.IsEnabled;
            set
            {
                if (value != Model.IsEnabled)
                {
                    Model.IsEnabled = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }


        public DayOfWeek[] DaysOfWeek
        {
            get => Model.DaysOfWeek;
            set
            {
                if (value != Model.DaysOfWeek)
                {
                    Model.DaysOfWeek = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        public int Hour
        {
            get => Model.Hour;
            set
            {
                if (value != Model.Hour)
                {
                    Model.Hour = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        public int Minute
        {
            get => Model.Minute;
            set
            {
                if (value != Model.Minute)
                {
                    Model.Minute = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

  
        public AlarmMusic Music
        {
            get => Model.Music;
            set
            {
                if (value != Model.Music)
                {
                    Model.Music = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

        public AlarmAudio Audio
        {
            get => Model.Audio;
            set
            {
                if (value != Model.Audio)
                {
                    Model.Audio = value;
                    IsModified = true;
                    OnPropertyChanged();
                }
            }
        }

  
        public bool IsModified { get; set; }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set => Set(ref isLoading, value);
        }

        private bool isNewSchedule;
        public bool IsNewSchedule
        {
            get => isNewSchedule;
            set => Set(ref isNewSchedule, value);
        }

        public async Task SaveAsync()
        {
           
            IsModified = false;
            if (IsNewSchedule)
            {
                IsNewSchedule = false;
                await scheduleService.Create(Model);
            }

            await scheduleService.Update(Model);
        }


        public async Task CancelAsync()
        {
            if (!IsNewSchedule)
            {
                await RevertChangesAsync();
            }

        }

        
        public async Task RevertChangesAsync()
        {
            if (IsModified)
            {
                await RefreshScheduleAsync();
                IsModified = false;
            }
        }

        public async Task RefreshScheduleAsync()
        {
            Model = (await scheduleService.Schedules)[Model.Id];
        }

    }
}
