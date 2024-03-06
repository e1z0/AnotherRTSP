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
 * TODO:
 * Make MQTT integration, separate tile support (red lights or something)
 * Make MQTT integration for notifications (frigate support)
 * Make settings dialog with all cameras, enable to add/remove/disable some of the cameras, also show settings on the first startup where are no cameras at all
 * Global logging to a custom window and log file (if enabled)
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
        private bool _isResizing = false;
        private System.Drawing.Point _startLocation;
        private System.Drawing.Size _startSize;


        private int movX, movY;
        private bool isMoving;


        private void SpawnCamera()
        {

        }

        // application exit event
        private void AppExit() {
            Application.Exit();
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
                    MessageBox.Show(string.Format("Copyright (c) 2024 e1z0. {0}",Settings.VERSION));
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

            // counter
            int i = 0;
            foreach (KeyValuePair<string, Camera> cam in Settings.Cameras)
            {
                cam.Value.Id = i;
                forms[i] = new Form();
                var mnu = InitializeContextMenu();
                forms[i].ContextMenu = mnu;
                forms[i].ShowInTaskbar = false;
                forms[i].BackColor = Color.Black;
                forms[i].FormBorderStyle = FormBorderStyle.None;

                forms[i].FormClosing += (sender, e) =>
                {
                    Form frm = sender as Form;
                    Settings.SetFormDetails(frm.Text, frm.Width, frm.Height, frm.Location.X, frm.Location.Y);
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
                Form ledform = new Form();
                ledform.ShowInTaskbar = false;
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
                                Logger.WriteLog("turn off led for {0}", control.Tag);
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
