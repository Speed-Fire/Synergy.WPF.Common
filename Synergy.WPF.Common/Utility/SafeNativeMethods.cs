using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.WPF.Common.Utility
{
    internal static class SafeNativeMethods
    {


        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr MB_GetString(int wBtn);
    }
}
