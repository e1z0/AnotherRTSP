using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EasyPlayerNetSDK;
using AnotherRTSP.Classes;
using System.Media;
using AnotherRTSP.Services;
using System.Reflection;
using System.Drawing;
using System.IO;
using AnotherRTSP.Components;
using System.Diagnostics;
using System.Timers;
using System.Threading;

namespace AnotherRTSP.Classes
{
    public class Camera
    {
        // object definitions
        public int Id { get; set; }
        public int ChannelID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int WWidth { get; set; }
        public int WHeight { get; set; }
        public int WX { get; set; }
        public int WY { get; set; }
        public bool Disabled { get; set; }
        public bool SoundEnabled { get; set; }
        public bool FullScreen { get; set; }
        public bool Recording { get; set; }
        public ContextMenuStrip contextMenu { get; set; }

        // variables
        private static int NextId = 1;
        // for show tooltips on camera windows
        private int mouseEnterCount = 0;
        // libEasyPlayer SDK settings FIXME should be moved to configuration
        private bool isTCP = true;
        private bool isHardEncode = false;
        private int streamCache = 3;
        private PlayerSdk.MediaSourceCallBack callBack = null;
        private PlayerSdk.RENDER_FORMAT RENDER_FORMAT = PlayerSdk.RENDER_FORMAT.DISPLAY_FORMAT_RGB24_GDI;
        // camera menus and items
        public ToolStripMenuItem camMenuItem = new ToolStripMenuItem();
        public ToolStripMenuItem recordMenuItem = new ToolStripMenuItem();
        public ToolStripMenuItem enableSoundMenuItem = new ToolStripMenuItem();

        private Form cameraForm;

        // new object
        public Camera(string name, int width, int height, int winx, int winy, string url, bool disabled)
        {
            Name = name;
            Id = NextId++;
            WWidth = width;
            WHeight = height;
            WX = winx;
            WY = winy;
            Url = url;
            Disabled = disabled;
        }

        public void DisableCamera()
        {
            Disabled = true;
            CloseCamera();
        }

        public void EnableCamera()
        {
            Disabled = false;
            SpawnCameraWindow();
        }

        public void CloseCamera()
        {
            try
            {
                if (SoundEnabled)
                    PlayerSdk.EasyPlayer_StopSound();
                PlayerSdk.EasyPlayer_CloseStream(ChannelID);
                if (cameraForm != null)
                {
                    cameraForm.Close();
                    cameraForm.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog("Error at Camera object CloseCamera() func: {0}", ex.StackTrace.ToString());
            }
        }

        public void SetFormSize(int width, int height)
        {
            if (cameraForm != null)
            {
                cameraForm.Size = new System.Drawing.Size(width, height);
            }
        }

        public void SetFormLocation(int x, int y)
        {
            if (cameraForm != null)
            {
                cameraForm.SetDesktopLocation(x, y);
            }
        }

        private void StopSounds()
        {
            SoundEnabled = false;
            PlayerSdk.EasyPlayer_StopSound();
            CustomUI.UncheckMenuItems("Sound");
        }

        public ToolStripMenuItem ReturnMenuItem()
        {
            camMenuItem.Text = Name;
            camMenuItem.Checked = !Disabled;

            camMenuItem.Click += (sender, e) =>
            {
                ToolStripMenuItem obj = sender as ToolStripMenuItem;
                if (obj != null)
                {
                    if (obj.Checked)
                    {
                        // disable
                        DisableCamera();
                        obj.Checked = false;
                        recordMenuItem.Enabled = false;
                        enableSoundMenuItem.Enabled = false;

                    }
                    else
                    {
                        // enable
                        EnableCamera();
                        obj.Checked = true;
                        recordMenuItem.Enabled = true;
                        enableSoundMenuItem.Enabled = true;
                    }
                }
            };
            // add subitems
            // camera record menu item 
            recordMenuItem.Text = "Record";
            recordMenuItem.Checked = Recording;
            recordMenuItem.Enabled = !Disabled;
            recordMenuItem.Click += (sender, e) =>
            {
                ToolStripMenuItem obj = sender as ToolStripMenuItem;
                if (obj != null)
                {
                    if (obj.Checked)
                    {
                        obj.Checked = false;
                        PlayerSdk.EasyPlayer_StopManuRecording(ChannelID);
                        
                    }
                    else
                    {
                        obj.Checked = true;
                        PlayerSdk.EasyPlayer_StartManuRecording(ChannelID);
                    }

                }
            };
            camMenuItem.DropDownItems.Add(recordMenuItem);
            // camera sound enable menu item
            enableSoundMenuItem.Text = "Sound";
            enableSoundMenuItem.Checked = SoundEnabled;
            enableSoundMenuItem.Enabled = !Disabled;
            enableSoundMenuItem.Click += (sender, e) =>
            {
                ToolStripMenuItem obj = sender as ToolStripMenuItem;
                if (obj != null)
                {
                    if (obj.Checked)
                    {
                        obj.Checked = false;
                        StopSounds();
                    }
                    else
                    {
                        obj.Checked = true;
                        SoundEnabled = true;
                        PlayerSdk.EasyPlayer_PlaySound(ChannelID);
                    }
                }
            };
            camMenuItem.DropDownItems.Add(enableSoundMenuItem);
            return camMenuItem;
        }

        public void SpawnCameraWindow()
        {
            if (!Disabled)
            {
                cameraForm = new Form();                
                // Load icon from embedded resource
                Icon icon;
                using (Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AnotherRTSP.Images.camera_64.ico"))
                {
                    icon = new Icon(iconStream);
                }
                cameraForm.Icon = icon;
                cameraForm.ContextMenuStrip = contextMenu;
                cameraForm.ShowInTaskbar = false;
                cameraForm.BackColor = Color.Black;
                cameraForm.FormBorderStyle = FormBorderStyle.None;
                if (Settings.Advanced.AllCamerasWindowsOnTop)
                    cameraForm.TopMost = true;


                cameraForm.Click += (sender, e) =>
                {
                    MouseEventArgs mouseEvent = e as MouseEventArgs;
                    if (mouseEvent != null && mouseEvent.Button == MouseButtons.Left)
                    {
                        if (Settings.Advanced.FocusAllWindowsOnClick && cameraForm.WindowState != FormWindowState.Maximized)
                        {
                            // Get all open forms and bring each one to the front
                            foreach (Form form in Application.OpenForms)
                            {
                                form.Focus();
                            }
                        }
                    }
                };

                cameraForm.FormClosing += (sender, e) =>
                {
                    UpdateWindowSpecs();
                };


                // camera label on the window
                Label camLabel = new Label();
                camLabel.Text = Name;
                camLabel.Font = new Font(camLabel.Font.FontFamily, 16);
                
                camLabel.AutoSize = true;
                camLabel.BorderStyle = BorderStyle.None;
                camLabel.ForeColor = Color.White;
                camLabel.BackColor = System.Drawing.Color.Transparent; //Initial color with full transparency, why not full? lets examine it later
                camLabel.Tag = "";
                camLabel.Visible = false;
                if (!Settings.Advanced.DisableCameraCaptions)
                {
                    if (Settings.Advanced.StaticCameraCaption)
                        camLabel.Visible = true;
                    else
                        camLabel.Visible = false;
                }
                cameraForm.Controls.Add(camLabel);


                cameraForm.MouseEnter += (sender, e) =>
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
                                camLbl.BringToFront();
                                // reset colors
                                camLbl.ForeColor = Color.White;
                                camLbl.BackColor = Color.Transparent;
                                camLbl.Visible = true;
                                LabelFadeOut(camLbl, frm);
                            }
                        }

                    }
                };

                cameraForm.MouseLeave += (sender, e) =>
                {
                    mouseEnterCount = 0;

                };

                // bind keydown events
                cameraForm.KeyDown += CustomUI.FormsKeyDown;
                // detect which window is focused
                //cameraForm.GotFocus += (sender, e) => {
                //};
                // Enable moving windows on mouse click
                cameraForm.MouseDown += CustomUI.FormsMouseDown;
                // mouse move event
                cameraForm.MouseMove += CustomUI.FormsMouseMove;
                // mouse button up event
                cameraForm.MouseUp += CustomUI.FormsMouseUp;
                // end of enabling windows movement on mouse click

                cameraForm.DoubleClick += new EventHandler(video_DoubleClick);
                cameraForm.Tag = Id;
                cameraForm.Text = Name;
                callBack = new PlayerSdk.MediaSourceCallBack(MediaCallback);
                ChannelID = PlayerSdk.EasyPlayer_OpenStream(Url, cameraForm.Handle, RENDER_FORMAT, isTCP ? 1 : 0, "", "", callBack, IntPtr.Zero, isHardEncode);
                if (ChannelID > 0)
                {
                    PlayerSdk.EasyPlayer_SetFrameCache(ChannelID, streamCache);
                    PlayerSdk.EasyPlayer_SetShownToScale(ChannelID, 1);
                }
                cameraForm.Show();
                Win32Func.MoveWindow(cameraForm.Handle, WX, WY, WWidth, WHeight, true);
            }
        }

        // full screen on window
        private void video_DoubleClick(object sender, EventArgs e)
        {
            Form frm = sender as Form;
            if (frm != null)
            {
                if (FullScreen)
                {
                    frm.WindowState = FormWindowState.Normal;
                    FullScreen = false;
                }
                else
                {
                    frm.WindowState = FormWindowState.Maximized;
                    FullScreen = true;
                }
            }
        }




        public void LabelFadeOut(Label label, Form parent, int timeout = 2000)
        {

            if (label.Visible)
            {

                System.Windows.Forms.Timer fadeTimer = new System.Windows.Forms.Timer();
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

        public void UpdateWindowSpecs()
        {
            if (cameraForm != null)
            {
                WX = cameraForm.Location.X;
                WY = cameraForm.Location.Y;
                WHeight = cameraForm.Height;
                WWidth = cameraForm.Width;
            }
        }

        private int MediaCallback(int _channelId, IntPtr _channelPtr, int _frameType, IntPtr pBuf, ref PlayerSdk.EASY_FRAME_INFO _frameInfo)
        {
            //Logger.WriteLog("libEasyPlayer DEBUG: {0}, {1}", _channelId, _frameType); 
            return 0;
        }

 
    }
}
