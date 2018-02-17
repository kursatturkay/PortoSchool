using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace PortoSchool.Libs
{
    static class ColorUtils
    {
        public static Color GetColorFromHex(string hexString)
        {
            Color x =(Color)XamlBindingHelper.ConvertValue(typeof(Color), hexString);
            return x;
        }
    }
}
