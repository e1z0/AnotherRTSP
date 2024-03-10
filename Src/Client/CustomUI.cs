using EasyPlayerNetSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using AnotherRTSP.Classes;
using System.Media;
using AnotherRTSP.Services;

/*
 * Copyright (c) 2024 e1z0. All Rights Reserved.
 * Licensed under MIT license.
 */

namespace AnotherRTSP
{
    public class CustomUI
    {
        // Win32 api to move/resize window
        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);



        private PlayerSdk.MediaSourceCallBack callBack = null;
        private bool isInit = false;
        private int channelID = -1;
        private bool isTCP = false;
        private bool isHardEncode = false;
        private PlayerSdk.RENDER_FORMAT RENDER_FORMAT = PlayerSdk.RENDER_FORMAT.DISPLAY_FORMAT_RGB24_GDI;
        private Form[] forms = new Form[10];
        private int[] Chans = new int[10];
        private int focusedVideo = -1;
        private bool videoFullScreen = false;
        private int streamCache = 3;

        // window resize and move variables
        private int movX, movY;
        private bool isMoving;

        // for show tooltips on camera windows
        private int mouseEnterCount = 0;

        // tray icon
        private NotifyIcon trayIcon;

        private void SpawnCamera()
        {

        }

        // application exit event
        private void AppExit()
        {
            if (isInit)
            {
                // close all open streams
                foreach (int chan in Chans) {
                PlayerSdk.EasyPlayer_CloseStream(chan);
                }
                
            }
            if (trayIcon != null)
                trayIcon.Dispose();
            Application.Exit();
        }

        private void About()
        {
            MessageBox.Show(string.Format("AnotherRTSP v{0}. Copyright (c) 2024 e1z0. All Rights Reserved\nLicensed under MIT License.", Settings.VERSION), "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            // Get the selected menu item.
            MenuItem menuItem = (MenuItem)sender;

            // Perform the appropriate action based on the selected menu item.
            switch (menuItem.Text)
            {
                case "Log":
                    Settings.ShowOrActivateForm<LogForm>();
                    break;
                case "Settings":
                    Settings.ShowOrActivateForm<SettingsForm>();
                    break;
                case "About":
                    // Copy the selected text to the clipboard.
                    About();
                    break;
                case "Exit":
                    AppExit();
                    break;

            }
        }

        private ContextMenu InitializeContextMenu()
        {
            // Create a new context menu.
            ContextMenu cnt = new ContextMenu();

            // Add a new menu item to the context menu.
            MenuItem menuItem = new MenuItem();
            menuItem.Text = "About";
            menuItem.Click += new EventHandler(MenuItem_Click);
            cnt.MenuItems.Add(menuItem);


            // Add another menu item to the context menu.
            menuItem = new MenuItem();
            menuItem.Text = "Settings";
            menuItem.Click += new EventHandler(MenuItem_Click);
            cnt.MenuItems.Add(menuItem);

            // Add another menu item to the context menu.
            menuItem = new MenuItem();
            menuItem.Text = "Log";
            menuItem.Click += new EventHandler(MenuItem_Click);
            cnt.MenuItems.Add(menuItem);

            // Add another menu item to the context menu.
            menuItem = new MenuItem();
            menuItem.Text = "Exit";
            menuItem.Click += new EventHandler(MenuItem_Click);
            cnt.MenuItems.Add(menuItem);
            return cnt;
        }

        public void FormsKeyDown(object sender, KeyEventArgs e)
        {
            Form frm = sender as Form;

            if (e.KeyCode == Keys.Up)
            {
                frm.Height -= 1;
            }
            else if (e.KeyCode == Keys.Down)
            {
                frm.Height += 1;
            }
            else if (e.KeyCode == Keys.Left)
            {
                frm.Width -= 1;
            }
            else if (e.KeyCode == Keys.Right)
            {
                frm.Width += 1;
            }
        }

        private void FormsGotFocus(object sender, EventArgs e)
        {
            Form frm = sender as Form;
            int id = (int)frm.Tag;
            if (id >= 0)
            {
                focusedVideo = id;
                channelID = Chans[id];
            }
        }

        private void FormsMouseDown(object sender, MouseEventArgs e)
        {
            // Assign this method to mouse_Down event of Form or Panel,whatever you want
            if (Control.MouseButtons == MouseButtons.Left)
            {
                isMoving = true;
                movX = e.X;
                movY = e.Y;
            }
            if (Control.MouseButtons == MouseButtons.Right)
                isMoving = false;
        }

        private void FormsMouseMove(object sender, MouseEventArgs e)
        {
            if (isMoving)
            {
                if (sender is Form)
                {
                    Form frm = (Form)sender;
                    if (frm != null)
                        frm.SetDesktopLocation(Control.MousePosition.X - movX, Control.MousePosition.Y - movY);
                }
                else
                {
                    Control control = sender as Control;
                    if (control != null)
                    {
                        Form parentForm = control.FindForm();
                        parentForm.SetDesktopLocation(Control.MousePosition.X - movX, Control.MousePosition.Y - movY);
                    }

                }
            }
        }

        private void FormsMouseUp(object sender, MouseEventArgs e)
        {
            // Assign this method to Mouse_Up event of Form or Panel.
            if (e.Button == MouseButtons.Left)
            {
                isMoving = false;
            }
        }


        public void LabelFadeOut(Label label, Form parent, int timeout = 2000)
        {
            
            if (label.Visible)
            {

                Timer fadeTimer = new Timer();
                fadeTimer.Interval = 1; // Adjust as needed for the desired fade speed
                int fade = 0;
                int r = 255, g = 255, b = 255;
                fadeTimer.Tick += (sender, e) =>
                {
                    fade++;
                    if (fade >= 200) // arbitrary duration set prior to initiating fade
                    {
                        if (r > 0) r--; // increase r value with each tick
                        if (g > 0) g--; // decrease g value with each tick
                        if (b > 0) b--; // decrease b value with each tick
                        label.ForeColor = Color.FromArgb(0, r, g, b);
                        if (r == 0 && g == 0 && b == 0) // arrived at target values
                        {
                            fade = 0;
                            label.Visible = false;
                            label.Tag = "";
                            fadeTimer.Stop();
                        }
                    }
                };
                fadeTimer.Start();
            }
        }

        private void InitializeTrayIcon()
        {
            // Load icon from embedded resource
            Icon icon;
            using (Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AnotherRTSP.Images.icon_32.ico"))
            {
                icon = new Icon(iconStream);
            }
            // Create a new NotifyIcon object
            trayIcon = new NotifyIcon();
            // Set icon image
            trayIcon.Icon = icon;
            // Set tooltip text
            trayIcon.Text = "AnotherRTSP";
            // Add a context menu
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            /*
            contextMenu.Items.Add("Test").Click += (sender, e) =>
            {
                MessageBox.Show("test");
            };
             */ 
            contextMenu.Items.Add("Settings").Click += (sender, e) => Settings.ShowOrActivateForm<SettingsForm>();
            contextMenu.Items.Add("Log Window").Click += (sender, e) => Settings.ShowOrActivateForm<LogForm>();
            contextMenu.Items.Add("About").Click += (sender, e) => About();
            contextMenu.Items.Add("Exit").Click += (sender, e) => AppExit();
            trayIcon.ContextMenuStrip = contextMenu;
            // Make the tray icon visible
            trayIcon.Visible = true;
            //trayIcon.ShowBalloonTip(3000, "Welcome", "AnotherRTSP Launched!", ToolTipIcon.Info);
        }

        public void Init()
        {

            int LimitDayOut = PlayerSdk.EasyPlayer_Init();
            if (LimitDayOut >= 0)
                isInit = true;
            if (!isInit)
                Logger.WriteLog("Unable to initialize Player SDK!");
            callBack = new PlayerSdk.MediaSourceCallBack(MediaCallback);
            isTCP = true;
            isHardEncode = false;

            InitializeTrayIcon();

            // counter
            int i = 0;
            foreach (KeyValuePair<string, Camera> cam in Settings.Cameras)
            {
                cam.Value.Id = i;
                forms[i] = new Form();
                var mnu = InitializeContextMenu();
                // Load icon from embedded resource
                Icon icon;
                using (Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AnotherRTSP.Images.camera_64.ico"))
                {
                    icon = new Icon(iconStream);
                }
                forms[i].Icon = icon;
                forms[i].ContextMenu = mnu;
                forms[i].ShowInTaskbar = false;
                forms[i].BackColor = Color.Black;
                forms[i].FormBorderStyle = FormBorderStyle.None;
                if (Settings.Advanced.AllCamerasWindowsOnTop)
                    forms[i].TopMost = true;
                

                forms[i].Click += (sender, e) =>
                {
                    if (Settings.Advanced.FocusAllWindowsOnClick)
                    {
                        // Get all open forms and bring each one to the front
                        foreach (Form form in Application.OpenForms)
                        {
                            form.Focus();
                        }
                    }
                };

                forms[i].FormClosing += (sender, e) =>
                {
                    Form frm = sender as Form;
                    Settings.SetFormDetails(frm.Text, frm.Width, frm.Height, frm.Location.X, frm.Location.Y);
                };


                // camera label on the window
                Label camLabel = new Label();
                camLabel.Text = cam.Key;
                camLabel.Font = new Font(camLabel.Font.FontFamily, 16);
                camLabel.AutoSize = true;
                // position later can be implemented in the config file
                //camLabel.Location = new System.Drawing.Point(50, 50);
                camLabel.ForeColor = Color.White; 
                camLabel.BackColor = Color.Empty; //Initial color with full transparency, why not full? lets examine it later
                camLabel.Tag = "";
                camLabel.Visible = false;
                if (!Settings.Advanced.DisableCameraCaptions)
                {
                    if (Settings.Advanced.StaticCameraCaption)
                        camLabel.Visible = true;
                    else
                        camLabel.Visible = false;
                }
                forms[i].Controls.Add(camLabel);

                
                forms[i].MouseEnter += (sender, e) =>
                {
                    mouseEnterCount++;
                    Form frm = sender as Form;
                    if (frm != null)
                    {
                        Label camLbl = frm.Controls[0] as Label;
                        if (camLbl != null)
                        {
                            if (mouseEnterCount == 1 && camLbl.Tag.ToString() == "" && !Settings.Advanced.StaticCameraCaption && !Settings.Advanced.DisableCameraCaptions)
                            {
                                camLbl.Tag = "lock";
                                // reset colors
                                camLbl.ForeColor = Color.White;
                                camLbl.BackColor = Color.Empty;
                                camLbl.Visible = true;
                                LabelFadeOut(camLbl,frm);
                            }
                        }

                    }
                };

                forms[i].MouseLeave += (sender, e) =>
                {
                    mouseEnterCount = 0;
                    
                };
                
    

                // bind keydown events
                forms[i].KeyDown += FormsKeyDown;
                // detect which window is focused
                forms[i].GotFocus += FormsGotFocus;
                // Enable moving windows on mouse click
                forms[i].MouseDown += FormsMouseDown;
                // mouse move event
                forms[i].MouseMove += FormsMouseMove;
                // mouse button up event
                forms[i].MouseUp += FormsMouseUp;
                // end of enabling windows movement on mouse click

                forms[i].DoubleClick += new EventHandler(video_DoubleClick);
                forms[i].Tag = i;

                forms[i].Text = cam.Key;
                Chans[i] = PlayerSdk.EasyPlayer_OpenStream(cam.Value.Url, forms[i].Handle, RENDER_FORMAT, isTCP ? 1 : 0, "", "", callBack, IntPtr.Zero, isHardEncode);
                cam.Value.ChannelID = Chans[i];
                if (Chans[i] > 0)
                {
                    PlayerSdk.EasyPlayer_SetFrameCache(Chans[i], streamCache);
                    PlayerSdk.EasyPlayer_SetShownToScale(Chans[i], 1);
                }
                forms[i].Show();
                MoveWindow(forms[i].Handle, cam.Value.WX, cam.Value.WY, cam.Value.WWidth, cam.Value.WHeight, true);
                i++;
            }

            if (Settings.Logging > 0 && Settings.LogWindow > 0)
            {
                Settings.ShowOrActivateForm<LogForm>();
            }



            if (Settings.MqttEnabled > 0 && Settings.LedsCount > 0)
            {
                // Load icon from embedded resource
                Icon icon;
                using (Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AnotherRTSP.Images.leds_256.ico"))
                {
                    icon = new Icon(iconStream);
                }
                Form ledform = new Form();
                ledform.ShowInTaskbar = false;
                ledform.Text = "Leds";
                ledform.Icon = icon;
                ledform.BackColor = Color.Black;
                ledform.AllowTransparency = true;
                ledform.TransparencyKey = Color.Black;
                ledform.FormBorderStyle = FormBorderStyle.None;
                ledform.AutoSize = true;
                ledform.AutoSizeMode = AutoSizeMode.GrowOnly;
                ledform.Size = new System.Drawing.Size(20, 45); // Adjust the form size as needed

                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                flowLayoutPanel.FlowDirection = FlowDirection.LeftToRight;
                flowLayoutPanel.Dock = DockStyle.Fill;

                // Add the FlowLayoutPanel to the form
                ledform.Controls.Add(flowLayoutPanel);

                // handle led events
                LedStateManager.LedStateChanged += (sender, e) =>
                {
                    foreach (Control control in flowLayoutPanel.Controls)
                    {

                        if (control is PictureBox)
                        {
                            PictureBox picbox = control as PictureBox;
                            int state = LedStateManager.ledStates[control.Tag.ToString()];
                            // Update PictureBox image based on state (just for demonstration)
                            if (state == 0)
                            {
                                //Logger.WriteLog("turn off led for {0}", control.Tag);
                                if (picbox.InvokeRequired)
                                {
                                    picbox.Invoke((MethodInvoker)delegate
                                    {
                                        // Update the control
                                        picbox.Image = LoadImageFromEmbeddedResource("AnotherRTSP.Images.led-lamp-red-off_32.png");
                                    });
                                }
                            }

                            else if (state == 1)
                            {
                                Logger.WriteLog("turn on led for {0}", control.Tag);
                                if (picbox.InvokeRequired)
                                {
                                    picbox.Invoke((MethodInvoker)delegate
                                    {
                                        // Update the control
                                        picbox.Image = LoadImageFromEmbeddedResource("AnotherRTSP.Images.led-lamp-red-on_32.png");
                                    });
                                }
                            }
                        }
                    }
                };

                // deploy leds
                foreach (KeyValuePair<string, int> led in LedStateManager.ledStates)
                {
                    PictureBox pictureBox = new PictureBox();
                    ToolTip toolTip = new ToolTip();
                    toolTip.SetToolTip(pictureBox, led.Key);
                    // Set PictureBox properties
                    pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
                    pictureBox.Tag = led.Key;
                    pictureBox.Margin = new Padding(5);
                    string resourceName = "AnotherRTSP.Images.led-lamp-red-off_32.png"; // Adjust the namespace and image names
                    pictureBox.Image = LoadImageFromEmbeddedResource(resourceName);

                    // Enable moving windows on mouse click
                    pictureBox.MouseDown += FormsMouseDown;
                    // mouse move event
                    pictureBox.MouseMove += FormsMouseMove;
                    // mouse button up event
                    pictureBox.MouseUp += FormsMouseUp;

                    flowLayoutPanel.Controls.Add(pictureBox);
                }
                ledform.Load += (sender, e) =>
                {
                    Form frm = sender as Form;
                    frm.Location = new System.Drawing.Point(Settings.LedWindowX, Settings.LedWindowY);
                };
                ledform.FormClosing += (sender, e) =>
                {
                    Form frm = sender as Form;
                    Settings.LedWindowX = frm.Location.X;
                    Settings.LedWindowY = frm.Location.Y;
                };
                int desiredWidth = flowLayoutPanel.PreferredSize.Width + SystemInformation.VerticalScrollBarWidth;
                ledform.Size = new System.Drawing.Size(desiredWidth, ledform.Size.Height);
                if (Settings.Advanced.LedsWindowOnTop)
                    ledform.TopMost = true;
                ledform.Show();
            }

        }

        private Image LoadImageFromEmbeddedResource(string resourceName)
        {
            // Get the assembly containing the embedded resources
            Assembly assembly = Assembly.GetExecutingAssembly();

            // Get the image stream from the embedded resource
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                // Return the image from the stream
                return Image.FromStream(stream);
            }
        }

        // full screen on window
        private void video_DoubleClick(object sender, EventArgs e)
        {
            Form frm = sender as Form;
            int id = (int)frm.Tag;
            if (id >= 0)
            {
                focusedVideo = id;
                channelID = Chans[id];

                if (videoFullScreen)
                {
                    forms[focusedVideo].WindowState = FormWindowState.Normal;
                    videoFullScreen = false;
                }
                else
                {
                    forms[focusedVideo].WindowState = FormWindowState.Maximized;
                    videoFullScreen = true;
                }
            }
        }

        private int MediaCallback(int _channelId, IntPtr _channelPtr, int _frameType, IntPtr pBuf, ref PlayerSdk.EASY_FRAME_INFO _frameInfo)
        {
            return 0;
        }

    }
}
