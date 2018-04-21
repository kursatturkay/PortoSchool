using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace PortoSchool.Libs
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
            var CultureInfoName = LocalizationUtils.ResourceValueByKey("CultureInfoName");
            return CultureInfoName;
        }

        public static string ResourceValueByKey(string key)
        {
            var res = ResourceLoader.GetForCurrentView();

            //If you want to access a string such as DeleteBlock.Text you cannot put a period. Instead, put a /
            // like this var deleteText = res.GetString("DeleteBlock/Text"); instead of DeleteBlock.Text
            return res.GetString(key);
        }
    }
}
