using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.WPF.Common.Utility
{
    internal static class ButtonStrings
    {
        internal static string GetStringOK()
        {
            return Marshal.PtrToStringAuto(SafeNativeMethods.MB_GetString(0));
        }

        internal static string GetStringCancel()
        {
            return Marshal.PtrToStringAuto(SafeNativeMethods.MB_GetString(1));
        }

        internal static string GetStringAbort()
        {
            return Marshal.PtrToStringAuto(SafeNativeMethods.MB_GetString(2))[1..];
        }

        internal static string GetStringRetry()
        {
            return Marshal.PtrToStringAuto(SafeNativeMethods.MB_GetString(3))[1..];
        }

        internal static string GetStringIgnore()
        {
            return Marshal.PtrToStringAuto(SafeNativeMethods.MB_GetString(4))[1..];
        }

        internal static string GetStringYes()
        {
            return Marshal.PtrToStringAuto(SafeNativeMethods.MB_GetString(5))[1..];
        }

        internal static string GetStringNo()
        {
            return Marshal.PtrToStringAuto(SafeNativeMethods.MB_GetString(6))[1..];
        }

        internal static string GetStringClose()
        {
            return Marshal.PtrToStringAuto(SafeNativeMethods.MB_GetString(7))[1..];
        }

        internal static string GetStringHelp()
        {
            return Marshal.PtrToStringAuto(SafeNativeMethods.MB_GetString(8));
        }

        internal static string GetStringTryAgain()
        {
            return Marshal.PtrToStringAuto(SafeNativeMethods.MB_GetString(9))[1..];
        }

        internal static string GetStringContinue()
        {
            return Marshal.PtrToStringAuto(SafeNativeMethods.MB_GetString(10))[1..];
        }
    }
}
