﻿using JW.Alarm.Models;
using JW.Alarm.Services.Contracts;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JW.Alarm.ViewModels
{

    public class ScheduleViewModel : BindableBase
    {
        IScheduleService scheduleService;
        public ScheduleViewModel(IScheduleService scheduleService, AlarmSchedule model = null)
        {
            this.scheduleService = scheduleService;
            Model = model ?? new AlarmSchedule();

            EnableCommand = new RelayCommandAsync<object>(async (x) =>
            {
                IsEnabled = true;
                await SaveAsync();
            });

            DisableCommand = new RelayCommandAsync<object>(async (x) =>
            {
                IsEnabled = false;
                await SaveAsync();
            });
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

        public string Name
        {
            get => Model.Name;
            set
            {
                if (value != Model.Name)
                {
                    Model.Name = value;
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

        public Meridien Meridien
        {
            get => Model.Meridien;
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

        public BibleAudio Bible
        {
            get => Model.Bible;
            set
            {
                if (value != Model.Bible)
                {
                    Model.Bible = value;
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

        public ICommand DisableCommand { get; private set; }
        public ICommand EnableCommand { get; private set; }

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
