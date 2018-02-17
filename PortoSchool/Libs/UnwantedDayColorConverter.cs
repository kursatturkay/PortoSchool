using PortoSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace PortoSchool.Libs
{
    class UnwantedDayColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var color = new SolidColorBrush(new Color { A = 255, R = 255, G = 255, B = 255 });

            //new SolidColorBrush(new Color { R = 0xe3, G = 0xe9, B = 0xef, A = byte.MaxValue });
            AssistantDirectorSentinelDay AssistantDirectorSentinelDay= (AssistantDirectorSentinelDay)value;

            switch (AssistantDirectorSentinelDay.IsUnwantedDay)
            {
                case false: color = new SolidColorBrush(ColorUtils.GetColorFromHex("#000000")); break;
                case true: color = new SolidColorBrush(ColorUtils.GetColorFromHex("#FF0000")); break;
            }

            //Availability ? new Brush(Brush..Green) : new SolidColorBrush(Colors.Red);
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
