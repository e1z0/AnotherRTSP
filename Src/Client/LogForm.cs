using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using AnotherRTSP.Classes;

namespace AnotherRTSP
{
    public partial class LogForm : Form
    {
        private string filePath;

        public LogForm()
        {
            InitializeComponent();
            this.filePath = Settings.LogPath;

        }



        private void ReadLogFile()
        {
            var initialFileSize = new FileInfo(filePath).Length;
            var lastReadLength = initialFileSize - 1024;
            if (lastReadLength < 0) lastReadLength = 0;
            while (Settings.LogWindowRunning)
            {
                try
                {
                    var fileSize = new FileInfo(filePath).Length;
                    if (fileSize > lastReadLength)
                    {
                        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            fs.Seek(lastReadLength, SeekOrigin.Begin);
                            var buffer = new byte[1024];

                            while (true)
                            {
                                var bytesRead = fs.Read(buffer, 0, buffer.Length);
                                lastReadLength += bytesRead;

                                if (bytesRead == 0)
                                    break;

                                var text = ASCIIEncoding.ASCII.GetString(buffer, 0, bytesRead);
                                UpdateTextBox(text);
                       
                            }
                        }
                    }
                }
                catch { }
                Thread.Sleep(1000);
            }
            Logger.WriteLog("Log Window Thread is done.");

        }

        private void UpdateTextBox(string text)
        {
            if (LogBox.InvokeRequired)
            {
                LogBox.Invoke(new MethodInvoker(delegate
                {
                    LogBox.AppendText(text);
                }));
            }
            else
            {
                LogBox.AppendText(text);
            }
        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Settings.Logging > 0)
            {
                Form frm = sender as Form;
                if (frm != null)
                {
                    Settings.LogWindowX = frm.Location.X;
                    Settings.LogWindowY = frm.Location.Y;
                    Settings.LogWindowHeight = frm.Height;
                    Settings.LogWindowWidth = frm.Width;

                    // only fire when user actually closes the form
                    if (e.CloseReason == CloseReason.UserClosing)
                    {
                        Settings.LogWindow = 0;
                    }
                    Settings.LogWindowRunning = false;
                    CustomUI.logmenuItem.Checked = false;

                }
            }
        }

        private void LogForm_Load(object sender, EventArgs e)
        {
            if (Settings.LogWindowHeight > 0 && Settings.LogWindowWidth > 0)
                       this.Size = new Size(Settings.LogWindowWidth, Settings.LogWindowHeight);
           // if (Settings.LogWindowX > 0 && Settings.LogWindowY > 0)
                        this.Location = new Point(Settings.LogWindowX, Settings.LogWindowY); 
            if (Settings.Logging > 0 && File.Exists(Settings.LogPath))
            {
                Thread logThread = new Thread(ReadLogFile);
                logThread.IsBackground = true;
                Settings.LogWindowRunning = true;
                logThread.Start();
                Settings.LogWindow = 1;
                CustomUI.logmenuItem.Checked = true;
            }
        }

        private void LogForm_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void LogBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
