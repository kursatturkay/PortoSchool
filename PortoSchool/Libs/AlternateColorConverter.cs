using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace PortoSchool.Libs
{
    class AlternateColorConverter : IValueConverter
    {
        private static int idx_ = -1;
        private static int odd_ = 0;

        /// <summary>
        /// Fixes a bug. resets alternate color on creatin of used page
        /// </summary>
        public static void reset()
        {
            idx_ = -1;
            odd_ = 0;
        }
        private static int idx(int offset)
        {
            int idx_before = idx_;
            idx_ = (idx_ + 1) % (offset);

            if (idx_ < idx_before) odd_ = (odd_ + 1) % 2;
            return odd_;
            //return idx_;
        }

        public static Color GetColorFromHex(string hexString)
        {
            Color x = (Color)XamlBindingHelper.ConvertValue(typeof(Color), hexString);
            return x;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int param = System.Convert.ToInt32(parameter);
            return new SolidColorBrush(ColorUtils.GetColorFromHex((idx(param) == 0) ? "#FBBA42" : "#FFFFFF"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
