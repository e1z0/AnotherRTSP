/*
 * Copyright (c) 2024-2025 e1z0. All Rights Reserved.
 * Licensed under the Business Source License 1.1.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyPlayerNetSDK;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.IO;

namespace AnotherRTSP.Classes
{
    public static class TrayIconManager
    {
        private static NotifyIcon _trayIcon;
        private static ContextMenuStrip _trayMenu;
        private static ToolStripMenuItem _cameraRootItem;
        private static Dictionary<string, ToolStripMenuItem> _cameraItems = new Dictionary<string, ToolStripMenuItem>();
        public static ToolStripMenuItem logmenuItem;


        public static void Initialize()
        {
            _trayMenu = new ContextMenuStrip();
            _cameraRootItem = new ToolStripMenuItem("Cameras");
            _trayMenu.Items.Add(_cameraRootItem);

            _trayMenu.Items.Add("About", null, (s, e) => Utils.About());
            _trayMenu.Items.Add("Settings", null, (s, e) => { Utils.ShowOrActivateForm<SettingsForm>(); });
            _trayMenu.Items.Add("Scripting", null, (s, e) => { Utils.ShowOrActivateForm<Forms.ScriptManagerForm>(); });
            // Add log menu item to the context menu.
            logmenuItem = new ToolStripMenuItem("Log")
            {
                Checked = YmlSettings.LogWindowRunning,
                CheckOnClick = true
            };
            logmenuItem.CheckedChanged += (s, e) =>
            {
                //MessageBox.Show("logas");
                if (logmenuItem.Checked)
                    Utils.ShowOrActivateForm<LogForm>();
                else
                    Utils.DeactivateForm<LogForm>();
                //Utils.DeactivateForm<LogForm>();
            };
            _trayMenu.Items.Add(logmenuItem);
            _trayMenu.Items.Add("Exit", null, (s, e) => Utils.AppExit());

            // Load icon from embedded resource
            Icon icon;
            using (Stream iconStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AnotherRTSP.Images.icon_32.ico"))
            {
                icon = new Icon(iconStream);
            }
            _trayIcon = new NotifyIcon();
            // Set icon image
            _trayIcon.Icon = icon;
            // Set tooltip text
            _trayIcon.Text = "AnotherRTSP";
            // Add a context menu
            _trayIcon.Click += (sender, e) =>
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
            _trayIcon.Visible = true;
            _trayIcon.ContextMenuStrip = _trayMenu;
        }

        public static ContextMenuStrip GetSharedMenu()
        {
            return _trayMenu; // or return _trayIcon.ContextMenuStrip;
        }

        public static void ShowBalloon(string title, string text, string iconTypeString, int delay)
        {
            ToolTipIcon iconType = ToolTipIcon.Info;
            if (!string.IsNullOrEmpty(iconTypeString))
            {
                Enum.TryParse(iconTypeString, true, out iconType); // case-insensitive parse
            }
            _trayIcon.BalloonTipText = text;
            _trayIcon.BalloonTipTitle = title;
            _trayIcon.BalloonTipIcon = iconType;
            _trayIcon.ShowBalloonTip(delay);
        }

        public static void PopulateCameraList()
        {
            _cameraRootItem.DropDownItems.Clear();
            _cameraItems.Clear();

            foreach (CameraItem localItem in YmlSettings.Data.Cameras)
            {
                CameraItem item = localItem;
                var camMenu = new ToolStripMenuItem(item.Name)
                {
                    Checked = !item.Disabled,
                    CheckOnClick = true
                };

                var resetCam = new ToolStripMenuItem("Reset") { Enabled = !item.Disabled };
                resetCam.Click += (s, e) => Camera.ResetCamera(item);

                var resetItem = new ToolStripMenuItem("Reset position") { Enabled = !item.Disabled };
                resetItem.Click += (s, e) => Camera.ResetPosition(item);

                var recordItem = new ToolStripMenuItem("Record") { Enabled = !item.Disabled, CheckOnClick = true };
                recordItem.Checked = false;
                recordItem.Click += (s, e) => Camera.ToggleRecording(item, recordItem);

                var soundItem = new ToolStripMenuItem("Sound") { Enabled = !item.Disabled, CheckOnClick = true };
                soundItem.Checked = false;
                soundItem.Click += (s, e) => Camera.ToggleSound(item, soundItem);

                camMenu.DropDownItems.Add(resetCam);
                camMenu.DropDownItems.Add(resetItem);
                camMenu.DropDownItems.Add(recordItem);
                camMenu.DropDownItems.Add(soundItem);

                camMenu.CheckedChanged += (s, e) =>
                {
                    if (camMenu.Checked)
                        Camera.EnableCamera(item);
                    else
                        Camera.DisableCamera(item);

                    resetItem.Enabled = camMenu.Checked;
                    recordItem.Enabled = camMenu.Checked;
                    soundItem.Enabled = camMenu.Checked;
                };

                _cameraRootItem.DropDownItems.Add(camMenu);
                _cameraItems[item.Name] = camMenu;
            }
        }
    }
}
