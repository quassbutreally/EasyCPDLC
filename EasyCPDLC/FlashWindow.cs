/*  EASYCPDLC: CPDLC Client for the VATSIM Network
    Copyright (C) 2021 Joshua Seagrave joshseagrave@googlemail.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/
using System;
using System.Runtime.InteropServices;

namespace EasyCPDLC;

public static class FlashWindow
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FlashWindowEx(ref Flashwinfo pwfi);

    [StructLayout(LayoutKind.Sequential)]
    private struct Flashwinfo
    {
        public uint cbSize;
        public IntPtr hwnd;
        public uint dwFlags;
        public uint uCount;
        public uint dwTimeout;
    }
    public const uint FlashwStop = 0;
    public const uint FlashwCaption = 1;
    public const uint FlashwTray = 2;
    public const uint FlashwAll = 3;
    public const uint FlashwTimer = 4;
    public const uint FlashwTimernofg = 12;

    public static bool Flash(System.Windows.Forms.Form form)
    {
        // Make sure we're running under Windows 2000 or later
        if (!Win2000OrLater) return false;
        var fi = Create_FLASHWINFO(form.Handle, FlashwAll | FlashwTimernofg, uint.MaxValue, 0);
        return FlashWindowEx(ref fi);
    }

    private static Flashwinfo Create_FLASHWINFO(IntPtr handle, uint flags, uint count, uint timeout)
    {
        Flashwinfo fi = new();
        fi.cbSize = Convert.ToUInt32(Marshal.SizeOf(fi));
        fi.hwnd = handle;
        fi.dwFlags = flags;
        fi.uCount = count;
        fi.dwTimeout = timeout;
        return fi;
    }
    public static bool Flash(System.Windows.Forms.Form form, uint count)
    {
        if (!Win2000OrLater) return false;
        var fi = Create_FLASHWINFO(form.Handle, FlashwAll, count, 0);
        return FlashWindowEx(ref fi);
    }

    public static bool Start(System.Windows.Forms.Form form)
    {
        if (!Win2000OrLater) return false;
        var fi = Create_FLASHWINFO(form.Handle, FlashwAll, uint.MaxValue, 0);
        return FlashWindowEx(ref fi);
    }
    public static bool Stop(System.Windows.Forms.Form form)
    {
        if (!Win2000OrLater) return false;
        var fi = Create_FLASHWINFO(form.Handle, FlashwStop, uint.MaxValue, 0);
        return FlashWindowEx(ref fi);
    }
    private static bool Win2000OrLater => Environment.OSVersion.Version.Major >= 5;
}