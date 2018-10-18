using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace JW.Alarm.Core.UWP.Views.Converters
{
    public class ClockIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            bool isEnabled = (bool)value;
            return isEnabled ? (object)"/Assets/Icons/clock_active.png"
                : (object)"/Assets/Icons/clock_inactive.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
