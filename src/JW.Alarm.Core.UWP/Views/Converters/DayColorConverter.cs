using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace JW.Alarm.Core.UWP.Views.Converters
{
    public class DayColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var isEnabled = (value as DayOfWeek[]).Contains((DayOfWeek)parameter);

            return isEnabled ? Color.Black : Color.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
