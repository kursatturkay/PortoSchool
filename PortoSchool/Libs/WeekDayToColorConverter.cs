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
    class WeekDayToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var DayOfWeek = (string)value;

            var color=new SolidColorBrush(new Color { A=255,R=255,G=255,B=255});
            //new SolidColorBrush(new Color { R = 0xe3, G = 0xe9, B = 0xef, A = byte.MaxValue });
            //=SolidColorBrush.ColorProperty( .FromArgb(12,12,12,12);

            switch (value)
            {
                case "PAZARTESİ": color=new SolidColorBrush(new Color { A=255, R=255, G=218, B=185 }) ; break;
                case "SALI": color = new SolidColorBrush(new Color { A = 255, R = 240, G = 255, B = 240 }); break;
                case "ÇARŞAMBA": color = new SolidColorBrush(new Color { A = 255, R = 230, G = 230, B = 250 }); break;
                case "PERŞEMBE": color = new SolidColorBrush(new Color { A = 255, R = 255, G = 250, B = 205 }); break;
                case "CUMA": color = new SolidColorBrush(new Color { A = 255, R = 240, G = 230, B = 140 }); break;
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
