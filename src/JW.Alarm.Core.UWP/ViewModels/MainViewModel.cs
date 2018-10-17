using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JW.Alarm.Services.Contracts;
using Microsoft.Toolkit.Uwp.Helpers;

namespace JW.Alarm.Core.UWP.ViewModels
{

    public class MainViewModel : BindableBase
    {
        private IScheduleService scheduleService;
     
        public MainViewModel(IScheduleService scheduleService)
        {
            this.scheduleService = scheduleService;
            Task.Run(GetScheduleListAsync);
        }

        public ObservableCollection<ScheduleViewModel> Schedules { get; }
            = new ObservableCollection<ScheduleViewModel>();

        private ScheduleViewModel selectedSchedule;

        public ScheduleViewModel SelectedSchedule
        {
            get => selectedSchedule;
            set => Set(ref selectedSchedule, value);
        }

        private bool isLoading = false;

        public bool IsLoading
        {
            get => isLoading;
            set => Set(ref isLoading, value);
        }

      
        public async Task GetScheduleListAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);

            var schedules = await scheduleService.Schedules;
            if (schedules == null)
            {
                return;
            }

            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Schedules.Clear();
                foreach (var schedule in schedules)
                {
                    Schedules.Add(new ScheduleViewModel(scheduleService, schedule.Value));
                }
                IsLoading = false;
            });
        }

    }
}
