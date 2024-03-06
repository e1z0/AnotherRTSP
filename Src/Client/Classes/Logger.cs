using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace AnotherRTSP.Classes
{
    public static class Logger
    {
        // Define a public static method to write a log message.
        public static void WriteLog(string message, params object[] args)
        {
            if (Settings.Logging > 0)
            {
                // Open the log file for appending.
                using (StreamWriter writer = new StreamWriter(Settings.LogPath, true))
                {
                    string TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    // Write the log message to the file.
                    string formattedString = String.Format(message, args);
                    writer.WriteLine(TimeStamp+" -> "+formattedString);
                }
            }
        }
    }
}
