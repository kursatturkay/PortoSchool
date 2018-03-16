using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Portoschool.Libs
{
    static class LocalizationUtils
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns string en-US, tr-TR..</returns>
        /// <remarks>you can customize CultureInfoName in Strings\resw files</remarks>
        public static string GetDefaultCultureInforName()
        {
            var resource = ResourceLoader.GetForCurrentView();
            var CultureInfoName = resource.GetString("CultureInfoName");
            return CultureInfoName;
        }
    }
}
