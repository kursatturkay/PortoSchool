using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace PortoSchool.Libs
{
    class DateFormatConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var resource = ResourceLoader.GetForCurrentView();
            var CultureInfoName = resource.GetString("CultureInfoName");
            CultureInfo trTR = new CultureInfo(CultureInfoName);

            var res = string.Format(trTR,"{0:dd/M/yyyy dddd}", (DateTime)value);
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}



