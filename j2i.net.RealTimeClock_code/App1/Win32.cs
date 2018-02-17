using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace App1
{

    public static class Win32
    {
        [DllImport("kernelbase.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SystemTime time);
    }

}
