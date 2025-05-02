/*
 * Copyright (c) 2024 e1z0. All Rights Reserved.
 * Licensed under MIT license.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using AnotherRTSP.Classes;

namespace AnotherRTSP.Classes
{
    public static class Logger
    {
        // Define a public static method to write a log message.
        public static void WriteLog(string message, params object[] args)
        {
            if (YmlSettings.Data.Logging)
            {
                // Open the log file for appending.
                try
                {
                    using (StreamWriter writer = new StreamWriter(YmlSettings.Data.LogPath, true))
                    {
                        string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        // Write the log message to the file.
                        string formattedString = String.Format(message, args);
                        writer.WriteLine(TimeStamp + " -> " + formattedString);
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        public static void WriteDebug(string message, params object[] args)
        {
#if DEBUG
            WriteLog("DEBUG -> " + message, args);
#endif
        }
    }
}
