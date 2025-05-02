/*
 * Copyright (c) 2024 e1z0. All Rights Reserved.
 * Licensed under MIT license.
 */

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
        private Thread logThread;

        public LogForm()
        {
            InitializeComponent();
            this.filePath = YmlSettings.Data.LogPath;

        }

        public void StartService()
        {
            if (YmlSettings.Data.Logging && File.Exists(YmlSettings.Data.LogPath))
            {
                logThread = new Thread(ReadLogFile);
                logThread.Start();
                Logger.WriteLog("Log Window Thread is running...");
                if (logThread.IsAlive)
                {
                    YmlSettings.LogWindowRunning = true;
                    YmlSettings.Data.LogWindow = true;
                    TrayIconManager.logmenuItem.Checked = true;
                }
            }
        }

        public void StopService()
        {
            logThread.Abort();
            YmlSettings.LogWindowRunning = false;
            TrayIconManager.logmenuItem.Checked = false;
            Logger.WriteLog("Log Window Thread is done.");
        }

        public void WaitForCompletion()
        {
            logThread.Join();
        }



        private void ReadLogFile()
        {
            var initialFileSize = new FileInfo(filePath).Length;
            var lastReadLength = initialFileSize - 1024;
            if (lastReadLength < 0) lastReadLength = 0;
            while (logThread.ThreadState == ThreadState.Running)
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
            
            if (YmlSettings.Data.Logging && YmlSettings.LogWindowRunning)
            {
                Form frm = sender as Form;
                if (frm != null)
                {
                    YmlSettings.Data.LogWindowX = frm.Location.X;
                    YmlSettings.Data.LogWindowY = frm.Location.Y;
                    YmlSettings.Data.LogWindowHeight = frm.Height;
                    YmlSettings.Data.LogWindowWidth = frm.Width;

                    // only fire when user actually closes the form
                    if (e.CloseReason == CloseReason.UserClosing)
                    {
                        YmlSettings.Data.LogWindow = false;
                        //YmlSettings.LogWindowRunning = false;
                        StopService();
                    }
                }
            }
             
        }

        private void LogForm_Load(object sender, EventArgs e)
        {
            if (YmlSettings.Data.LogWindowHeight > 0 && YmlSettings.Data.LogWindowWidth > 0)
                this.Size = new Size(YmlSettings.Data.LogWindowWidth, YmlSettings.Data.LogWindowHeight);
            this.Location = new Point(YmlSettings.Data.LogWindowX, YmlSettings.Data.LogWindowY);
            StartService();
        }

        private void LogForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void LogBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void LogForm_Click(object sender, EventArgs e)
        {

        }

        private void LogForm_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void LogBox_MouseDown(object sender, MouseEventArgs e)
        {
            MouseEventArgs mouseEvent = e as MouseEventArgs;
            if (mouseEvent != null && mouseEvent.Button == MouseButtons.Left)
            {
                if (YmlSettings.Advanced.FocusAllWindowsOnClick)
                {
                    // Get all open forms and bring each one to the front
                    foreach (Form form in Application.OpenForms)
                    {
                        form.Focus();
                    }
                }
            }
        }
    }
}
