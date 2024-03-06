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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // read the embedded version information
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream("AnotherRTSP.version.info");
            byte[] data = new byte[stream.Length];
            stream.Position = 0; // Set the stream position to the beginning
            stream.Read(data, 0, data.Length); // Read the entire stream
            Settings.VERSION = System.Text.Encoding.UTF8.GetString(data); 
            // continue loading
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            //AppDomain.CurrentDomain.ProcessExit += new EventHandler(Application_ApplicationExit);
            Settings.Load();
            if (!Settings.FirstRun)
            {
                if (Settings.CustomLayout > 0)
                {
                    MqttService mqttsvc = new MqttService();
                    Thread thread = new Thread(mqttsvc.ServiceStart);
                    thread.IsBackground = true;
                    CustomUI ui = new CustomUI();
                    ui.Init();
                    
                    // start mqtt service if enabled
                    if (Settings.MqttEnabled > 0)
                    {
                        thread.Start();
                    }
                    Application.Run();
                    thread.Join();
                }
                else
                {
                    Application.Run(new PlayerForm());
                }
            }
            else
            {
                Application.Run(new FirstRun());
            }
            
        }
        // bind application exit event and stop all threads on event
        static void Application_ApplicationExit(object sender, EventArgs e)
        {
                Settings.MqttServiceRunning = false;
                Settings.LogWindowRunning = false;
                Settings.Save();
                Thread.Sleep(5000);
                Logger.WriteLog("Program gracefully closed!");
                Environment.Exit(Environment.ExitCode);

        }
    }
}
