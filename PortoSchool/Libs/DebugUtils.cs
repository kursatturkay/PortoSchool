using PortoSchool.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortoSchool.Libs
{
    static class DebugUtils
    {
        public static void WriteLine(string str)
        {
#if PortoSchool_DEBUG
            //Debug.WriteLine(str);
            //DashboardPage.Current.FillListBoxLog
            App.ErrorLog.Add(str);
#endif
        }
    }
}
