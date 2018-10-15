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
            return isEnabled ? (object)"ms-appdata:///Mobile.Clock.UI.UWP/Assets/clock_active.png"
                : (object)"ms-appdata:///Mobile.Clock.UI.UWP/Assets/in_active.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
