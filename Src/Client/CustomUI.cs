/*
 * Copyright (c) 2024-2025 e1z0. All Rights Reserved.
 * Licensed under the Business Source License 1.1.
 */


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

namespace AnotherRTSP
{
    public class CustomUI
    {
        // if player SDK is initialized
        //private bool isInit = false;

        // window resize and move variables
        private static int movX, movY;
        private static bool isMoving;

        // tray icon
        private NotifyIcon trayIcon;
        // program context menus
        public static ContextMenuStrip MainContextMenu = new ContextMenuStrip();
        public static List<ToolStripMenuItem> CamerasMenu = new List<ToolStripMenuItem>();
        //public static ToolStripMenuItem logmenuItem = new ToolStripMenuItem();


        // custom UI init
        public void Init()
        {

            int LimitDayOut = PlayerSdk.EasyPlayer_Init();
            if (LimitDayOut >= 0)
                YmlSettings.IsInit = true;
            if (!YmlSettings.IsInit)
                Logger.WriteLog("Unable to initialize Player SDK!");
            // load tray icon
            TrayIconManager.Initialize(); // initialize the tray icon and shared context menu
            TrayIconManager.PopulateCameraList(); // populate menu with current config

            // load cameras
            foreach (CameraItem item in YmlSettings.Data.Cameras)
            {
                if (!item.Disabled)
                {
                    Camera cam = new Camera(item);
                }
            }

            if (YmlSettings.Data.MqttEnabled && YmlSettings.LedsCount > 0)
            {
                // Load icon from embedded resource
                Icon icon;
                using (Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AnotherRTSP.Images.leds_256.ico"))
                {
                    icon = new Icon(iconStream);
                }
                Form ledform = new Form();
                ledform.ShowInTaskbar = false;
                ledform.Name = "Leds";
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
                    frm.Location = new System.Drawing.Point(YmlSettings.Data.LedWindowX, YmlSettings.Data.LedWindowY);
                };
                ledform.FormClosing += (sender, e) =>
                {
                    Form frm = sender as Form;
                    YmlSettings.Data.LedWindowX = frm.Location.X;
                    YmlSettings.Data.LedWindowY = frm.Location.Y;
                };
                int desiredWidth = flowLayoutPanel.PreferredSize.Width + SystemInformation.VerticalScrollBarWidth;
                ledform.Size = new System.Drawing.Size(desiredWidth, ledform.Size.Height);
                if (YmlSettings.Data.AdvancedSettings.LedsWindowOnTop)
                    ledform.TopMost = true;
                ledform.Show();
            }

        }


        public static void FormsKeyDown(object sender, KeyEventArgs e)
        {
            Form frm = sender as Form;

            if (e.KeyCode == Keys.Up)
            {
                frm.Height -= YmlSettings.Data.AdvancedSettings.ResizeWindowBy;
            }
            else if (e.KeyCode == Keys.Down)
            {
                frm.Height += YmlSettings.Data.AdvancedSettings.ResizeWindowBy;
            }
            else if (e.KeyCode == Keys.Left)
            {
                frm.Width -= YmlSettings.Data.AdvancedSettings.ResizeWindowBy;
            }
            else if (e.KeyCode == Keys.Right)
            {
                frm.Width += YmlSettings.Data.AdvancedSettings.ResizeWindowBy;
            }
        }

        public static void FormsGotFocus(object sender, EventArgs e)
        {

        }

        public static void FormsMouseDown(object sender, MouseEventArgs e)
        {
            // Assign this method to mouse_Down event of Form or Panel,whatever you want
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (YmlSettings.Data.AdvancedSettings.FocusAllWindowsOnClick)
                {
                    // Get all open forms and bring each one to the front
                    foreach (Form form in Application.OpenForms)
                    {
                        form.Focus();
                    }
                }
                isMoving = true;
                movX = e.X;
                movY = e.Y;
            }
            if (Control.MouseButtons == MouseButtons.Right)
                isMoving = false;
        }

        public static void FormsMouseMove(object sender, MouseEventArgs e)
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

        public static void FormsMouseUp(object sender, MouseEventArgs e)
        {
            // Assign this method to Mouse_Up event of Form or Panel.
            if (e.Button == MouseButtons.Left)
            {
                isMoving = false;
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
            trayIcon.Click += (sender, e) =>
            {
                   var mouseArgs = e as MouseEventArgs;
                   // left mouse button click activates all windows
                   if (mouseArgs != null && mouseArgs.Button == MouseButtons.Left)
                   {

                       if (YmlSettings.Data.AdvancedSettings.FocusAllWindowsOnClick)
                       {
                           // Get all open forms and bring each one to the front
                           foreach (Form form in Application.OpenForms)
                           {
                               Win32Func.SetForegroundWindow(form.Handle);
                           }
                       }
                   }
            };
            // Make the tray icon visible
            trayIcon.Visible = true;
        }

        public static void UncheckMenuItems(string itemName)
        {

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


   

    }
}
