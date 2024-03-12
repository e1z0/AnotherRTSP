using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnotherRTSP.Services;
using System.Threading;
using AnotherRTSP.Classes;
using System.Reflection;
using System.IO;


namespace AnotherRTSP
{
    static class Program
    {
        private static MqttService mqttService;
        private static LogForm logService;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Read the embedded version information
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream("AnotherRTSP.version.info"))
            using (StreamReader reader = new StreamReader(stream))
            {
                Settings.VERSION = reader.ReadToEnd();
            }
            // Enable visual styles and set text rendering default
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += Application_ApplicationExit;
            // Load settings
            Settings.Load();

            // Check for first run
            if (Settings.FirstRun)
            {
                Application.Run(new FirstRun());
                return;
            }


            // Initialize MQTT service and UI
            if (Settings.CustomLayout > 0)
            {
                CustomUI ui = new CustomUI();
                ui.Init();

                // Start Log service if enabled
                if (Settings.Logging > 0 && Settings.LogWindow > 0)
                {
                    logService = new LogForm();
                    logService.Show();
                }

                // Start MQTT service if enabled
                if (Settings.MqttEnabled > 0)
                {
                    mqttService = new MqttService();
                    mqttService.StartService();
                }
                Application.Run();
            }
            else
            {
                Application.Run(new PlayerForm());
            }
        }


        // application exit event, we should stop all threads on this event
        static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Settings.Save();
            if (mqttService != null)
            {
                mqttService.StopService(); // Stop MQTT service
                mqttService.WaitForCompletion(); // Wait for MQTT service to stop
            }
            if (logService != null)
            {
                logService.StopService();
                logService.WaitForCompletion();
            }
            // clean exit
            Logger.WriteLog("Program gracefully closed!");
            Application.Exit();
        }

        // for some reason i left it, maybe it be useful someday
        static void TerminateAllThreads()
        {
            // Get the current process
            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();

            // Get all threads except the main thread
            foreach (System.Diagnostics.ProcessThread thread in currentProcess.Threads)
            {
                if (thread.Id != Thread.CurrentThread.ManagedThreadId)
                {
                    // Terminate the thread
                    try
                    {
                        thread.Dispose();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

    }
}
