﻿using MVVM.Template.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace System.Windows
{
    static class WindowExtensions
    {
        private const string user32 = "user32.dll";

        [DllImport(user32, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport(user32, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);


        public static IntPtr SendMessage(this Window window, WM Msg, SC wParam, IntPtr lParam = default)
        {
            return SendMessage(window.GetWindowHandle(), (uint)Msg, (IntPtr)wParam, lParam == default ? (IntPtr)' ' : lParam);
        }

        public static IntPtr SendMessage(this Window window, WM Msg, IntPtr wParam, IntPtr lParam)
        {
            return SendMessage(window.GetWindowHandle(), (uint)Msg, (IntPtr)wParam, lParam);
        }


        public static IntPtr GetWindowHandle(this Window window)
        {
            var helper = new WindowInteropHelper(window);
            return helper.Handle;
        }
    }
}
