/*
 * Copyright (c) 2024 e1z0. All Rights Reserved.
 * Licensed under MIT license.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace AnotherRTSP.Classes
{
    public static class Win32Func
    {
        // Win32 api to move/resize window
        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        // Win32 api to make focus on the target window
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


    }
}
