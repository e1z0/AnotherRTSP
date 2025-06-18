/*
 * Copyright (c) 2024 e1z0. All Rights Reserved.
 * Licensed under the Business Source License 1.1.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using AnotherRTSP.Classes;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using EasyPlayerNetSDK;
using System.Linq;
using System.IO;
using System.Reflection;

namespace AnotherRTSP
{
    public class Camera : Form
    {
        public CameraItem Config { get; private set; }
        public static List<Camera> AllCameras = new List<Camera>();
        public static List<Camera> ActiveCameras = new List<Camera>();


        public int ChannelID;
        private PlayerSdk.MediaSourceCallBack callback;
        private PlayerSdk.RENDER_FORMAT RENDER_FORMAT = PlayerSdk.RENDER_FORMAT.DISPLAY_FORMAT_RGB24_GDI;
        //private bool isTCP = true;
        //private bool isHardEncode = false;
        private int streamCache = 3;
        private bool FullScreen = false;

        private Label camLabel; // camera label on top of left corner

        // resizable window
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 1;
        private const int HTCAPTION = 2;
        private const int WM_GETMINMAXINFO = 0x24;
        private const int WM_LBUTTONDBLCLK = 0x0203;


        public Camera(CameraItem config)
        {
            this.Config = config;
            this.Text = config.Name;
            // Load icon from embedded resource
            Icon icon;
            using (Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AnotherRTSP.Images.camera_64.ico"))
            {
                icon = new Icon(iconStream);
            }
            this.Icon = icon;
            this.Width = config.WWidth;
            this.Height = config.WHeight;
            this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point(config.WX, config.WY);
            // new implementation checks if it's primary screen, also checks the resolution, if the resolution is lower than saved one, realigns the windows to be in the correct place
            this.Location = ClampToVisibleScreen(new Point(config.WX, config.WY), new Size(config.WWidth, config.WHeight));

            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.ShowInTaskbar = false;
            this.TopMost = YmlSettings.Data.AdvancedSettings.AllCamerasWindowsOnTop;
            this.ContextMenuStrip = TrayIconManager.GetSharedMenu(); // reuse the tray menu


            this.KeyDown += CameraKeyDown;
            this.MouseDown += CameraMouseDown;
            this.MouseMove += CameraMouseMove;
            this.MouseUp += CameraMouseUp;
            this.MouseDoubleClick += CameraDoubleClick;
            this.FormClosing += CameraClosing;
            this.MouseEnter += CameraMouseEnter;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;

            // Transparent background camera label
            camLabel = new Label();
            camLabel.Text = Config.Name;
            camLabel.Font = new Font("Arial", 12, FontStyle.Bold);
            camLabel.ForeColor = Color.White;
            camLabel.AutoSize = true;
            camLabel.Location = new Point(5, 5);
            camLabel.Visible = false;
            camLabel.BackColor = Color.Transparent;

            this.Controls.Add(camLabel);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
            camLabel.BringToFront();

            AllCameras.Add(this);

            callback = new PlayerSdk.MediaSourceCallBack(MediaCallback);
            ChannelID = PlayerSdk.EasyPlayer_OpenStream(
                Config.Url,
                this.Handle,
                RENDER_FORMAT,
                config.isTCP ? 1 : 0,
                "", "", callback,
                IntPtr.Zero,
                config.HardDecode
            );

            if (ChannelID > 0)
            {
                PlayerSdk.EasyPlayer_SetFrameCache(ChannelID, streamCache);
                PlayerSdk.EasyPlayer_SetShownToScale(ChannelID, 1);
            }
            this.Show();
        }

        private Point ClampToVisibleScreen(Point desiredLocation, Size windowSize)
        {
            foreach (var screen in Screen.AllScreens)
            {
                Rectangle bounds = screen.WorkingArea;
                Rectangle windowRect = new Rectangle(desiredLocation, windowSize);

                if (bounds.Contains(windowRect))
                    return desiredLocation;
            }

            // If not fully contained in any screen, adjust to primary screen
            var primary = Screen.PrimaryScreen.WorkingArea;
            int x = Math.Max(primary.Left, Math.Min(desiredLocation.X, primary.Right - windowSize.Width));
            int y = Math.Max(primary.Top, Math.Min(desiredLocation.Y, primary.Bottom - windowSize.Height));

            return new Point(x, y);
        }


        public static void EnableCamera(CameraItem item)
        {
            if (!AllCameras.Any(c => c.Config == item))
            {
                Camera cam = new Camera(item);
                item.Disabled = false;
            }
        }

        public static void DisableCamera(CameraItem item)
        {
            var cam = AllCameras.FirstOrDefault(c => c.Config == item);
            if (cam != null)
            {
                cam.Close();
                item.Disabled = true;
            }
        }

        public static void ResetCamera(CameraItem item)
        {
            DisableCamera(item);
            System.Threading.Thread.Sleep(500);
            EnableCamera(item);
        }

        public static void ResetPosition(CameraItem item)
        {
            var cam = AllCameras.FirstOrDefault(c => c.Config == item);
            if (cam != null)
            {
                cam.Location = new Point(0, 0);
                cam.Size = new Size(300, 200);
                cam.Config.WX = 0;
                cam.Config.WY = 0;
                cam.Config.WWidth = 300;
                cam.Config.WHeight = 200;
            }
        }

        public static void ToggleRecording(CameraItem item, ToolStripMenuItem menu)
        {
            var cam = AllCameras.FirstOrDefault(c => c.Config == item);
            if (cam != null)
            {
                if (menu.Checked)
                    PlayerSdk.EasyPlayer_StartManuRecording(cam.ChannelID);
                else
                    PlayerSdk.EasyPlayer_StopManuRecording(cam.ChannelID);
            }
        }

        public static void Stop(CameraItem item)
        {
            var cam = AllCameras.FirstOrDefault(c => c.Config == item);
            if (cam != null)
            {
                PlayerSdk.EasyPlayer_CloseStream(cam.ChannelID);
            }
        }

        public static void ToggleSound(CameraItem item, ToolStripMenuItem menu)
        {
            var cam = AllCameras.FirstOrDefault(c => c.Config == item);
            if (cam != null)
            {
                if (menu.Checked)
                    PlayerSdk.EasyPlayer_PlaySound(cam.ChannelID);
                else
                    PlayerSdk.EasyPlayer_StopSound();
            }
        }


        private int MediaCallback(int _channelId, IntPtr _channelPtr, int _frameType, IntPtr pBuf, ref PlayerSdk.EASY_FRAME_INFO _frameInfo)
        {
            try
            {
                if (_channelId > 0 && pBuf == IntPtr.Zero || _frameInfo.length == 0)
                {
                    Logger.WriteLog("[RTSP] Skipping empty or null frame from channel {channelId}");
                    return 0;
                }

                if (_frameInfo.length > 1920 * 1080 * 4)
                {
                    Logger.WriteLog("[RTSP] Suspicious frame size: {frameInfo.length} bytes. Skipped.");
                    return 0;
                }

                //byte[] buffer = new byte[frameInfo.length];
                //Marshal.Copy(pBuf, buffer, 0, (int)frameInfo.length);

                // Your frame processing here...

                return 0;
            }
            catch (AccessViolationException ex)
            {
                Logger.WriteLog("[RTSP] AccessViolation in channel {channelId}: {ex.Message}");
                // Do not rethrow!
                return -1;
            }
            catch (Exception ex)
            {
                Logger.WriteLog("[RTSP] Exception in callback: {ex}");
                return -1;
            }
        }

        private void CameraMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (YmlSettings.Data.AdvancedSettings.FocusAllWindowsOnClick && !FullScreen)
                {
                    // Get all open forms and bring each one to the front
                    foreach (Form form in Application.OpenForms)
                    {
                        form.Focus();
                    }
                }
                ReleaseCapture();
                SendMessage(this.Handle, 0xA1, HTCAPTION, 0);
            }

        }

        private void CameraMouseMove(object sender, MouseEventArgs e)
        {
        }

        private void CameraMouseUp(object sender, MouseEventArgs e)
        {
        }

        private void CameraKeyDown(object sender, KeyEventArgs e)
        {
            int delta = YmlSettings.Data.AdvancedSettings.ResizeWindowBy;
            if (e.KeyCode == Keys.Up) this.Height -= delta;
            if (e.KeyCode == Keys.Down) this.Height += delta;
            if (e.KeyCode == Keys.Left) this.Width -= delta;
            if (e.KeyCode == Keys.Right) this.Width += delta;
        }

        private void CameraDoubleClick(object sender, EventArgs e)
        {

            if (!FullScreen)
            {
                UpdateConfigFromForm();
                Screen screen = Screen.FromControl(this);
                Rectangle workingArea = screen.WorkingArea;
                this.Size = workingArea.Size;
                this.Location = new Point(
                    workingArea.Left + (workingArea.Width - this.Width) / 2,
                    workingArea.Top + (workingArea.Height - this.Height) / 2
                );
                FullScreen = true;
                if (Utils.DetectInvalidResolution())
                    PlayerSdk.EasyPlayer_SetShownToScale(ChannelID, 0);
            }
            else
            {
                this.Size = new Size(Config.WWidth, Config.WHeight);
                this.Location = new Point(Config.WX, Config.WY);
                FullScreen = false;
                if (Utils.DetectInvalidResolution())
                    PlayerSdk.EasyPlayer_SetShownToScale(ChannelID, 1);
            }

            /*
                if (this.WindowState == FormWindowState.Normal)
                {
                    UpdateConfigFromForm();
                    this.WindowState = FormWindowState.Maximized;
                    if (Utils.DetectInvalidResolution())
                        PlayerSdk.EasyPlayer_SetShownToScale(ChannelID, 0);
                    
                    new System.Threading.Thread(() =>
                    {
                        System.Threading.Thread.Sleep(500);
                        this.Invoke((MethodInvoker)delegate
                        {
                            this.Dock = DockStyle.Fill;
                            this.Invalidate();
                        });
                    }).Start();
                     
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                    this.Size = new Size(Config.WWidth, Config.WHeight);
                    this.Location = new Point(Config.WX, Config.WY);
                    if (Utils.DetectInvalidResolution())
                        PlayerSdk.EasyPlayer_SetShownToScale(ChannelID, 1);
                }
            */
        }

        private void CameraMouseEnter(object sender, EventArgs e)
        {

            if (!YmlSettings.Data.AdvancedSettings.StaticCameraCaption && !YmlSettings.Data.AdvancedSettings.DisableCameraCaptions)
            {
                camLabel.Visible = true;
                Timer fadeTimer = new Timer();
                fadeTimer.Interval = 3000;
                fadeTimer.Tick += (s, args) =>
                {
                    camLabel.Visible = false;
                    fadeTimer.Stop();
                };
                fadeTimer.Start();
            }

        }

        public void UpdateConfigFromForm()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                Config.WWidth = this.Width;
                Config.WHeight = this.Height;
                Config.WX = this.Location.X;
                Config.WY = this.Location.Y;
            }
        }

        private void CameraClosing(object sender, FormClosingEventArgs e)
        {
            PlayerSdk.EasyPlayer_CloseStream(ChannelID);
            YmlSettings.UpdateCamera(this);
            AllCameras.Remove(this);
        }


        // this function used in settings to dinamically remove the camera
        public static void RemoveCamera(CameraItem item)
        {
            // 1. Close live window if open
            var cam = Camera.AllCameras.FirstOrDefault(c => ReferenceEquals(c.Config, item));
            if (cam != null)
            {
                cam.Close();
                Camera.AllCameras.Remove(cam);
            }

            // 2. Remove from settings
            YmlSettings.Data.Cameras.Remove(item);

            // 3. Save settings
            YmlSettings.Save();

            // 4. Refresh tray menu
            TrayIconManager.PopulateCameraList();
        }

        // this function used in settings to dinamically add the new camera
        public static void AddCamera(CameraItem item)
        {
            if (item != null)
            {
                YmlSettings.AddCamera(item);
                if (!item.Disabled)
                    Camera.EnableCamera(item);
                TrayIconManager.PopulateCameraList();
            }
        }

        // this function used in settings to dinamically edit the current camera
        public static void EditCamera(CameraItem olditem, CameraItem newitem)
        {
            RemoveCamera(olditem);
            AddCamera(newitem);
        }

        // win32 api call to resize and move windows
        protected override void WndProc(ref Message m)
        {

            // handle left mouse button double click
            if (m.Msg == WM_LBUTTONDBLCLK)
            {
                CameraDoubleClick(this, EventArgs.Empty);
                return;
            }

            // handle resize and move
            if (m.Msg == WM_GETMINMAXINFO)
            {
                MINMAXINFO mmi = (MINMAXINFO)System.Runtime.InteropServices.Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));
                mmi.ptMinTrackSize.x = 45; // minimum size
                mmi.ptMinTrackSize.y = 45;
                System.Runtime.InteropServices.Marshal.StructureToPtr(mmi, m.LParam, true);
                return;
            }

            const int RESIZE_HANDLE_SIZE = 6;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);
                Point screenPoint = new Point(m.LParam.ToInt32());
                Point clientPoint = this.PointToClient(screenPoint);

                if (clientPoint.Y < RESIZE_HANDLE_SIZE)
                {
                    if (clientPoint.X < RESIZE_HANDLE_SIZE)
                        m.Result = (IntPtr)HTTOPLEFT;
                    else if (clientPoint.X > this.ClientSize.Width - RESIZE_HANDLE_SIZE)
                        m.Result = (IntPtr)HTTOPRIGHT;
                    else
                        m.Result = (IntPtr)HTTOP;
                }
                else if (clientPoint.Y > this.ClientSize.Height - RESIZE_HANDLE_SIZE)
                {
                    if (clientPoint.X < RESIZE_HANDLE_SIZE)
                        m.Result = (IntPtr)HTBOTTOMLEFT;
                    else if (clientPoint.X > this.ClientSize.Width - RESIZE_HANDLE_SIZE)
                        m.Result = (IntPtr)HTBOTTOMRIGHT;
                    else
                        m.Result = (IntPtr)HTBOTTOM;
                }
                else if (clientPoint.X < RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTLEFT;
                }
                else if (clientPoint.X > this.ClientSize.Width - RESIZE_HANDLE_SIZE)
                {
                    m.Result = (IntPtr)HTRIGHT;
                }
                return;
            }

            base.WndProc(ref m);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }
        // end of resizable window
    }
}
