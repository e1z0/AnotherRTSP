/*
 * Copyright (c) 2024-2025 e1z0. All Rights Reserved.
 * Licensed under the Business Source License 1.1.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AnotherRTSP.Forms
{
    public partial class NotificationForm : Form
    {
        private Timer closeTimer;
        private float opacityIncrement = 0.07f;
        private float opacityDecrement = 0.07f;
        private bool fadingOut = false;
        private int displayDuration = 4000; // default 4s if not specified

        // For rounded corners
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        public NotificationForm(string title, string message, int timeoutMs)
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            this.Size = new Size(320, 120);
            this.Opacity = 0.0;
            this.Click += NotificationForm_Click;

            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));

            this.Paint += NotificationForm_Paint;

            int x = Screen.PrimaryScreen.WorkingArea.Width - this.Width - 20;
            int y = Screen.PrimaryScreen.WorkingArea.Height - this.Height - 20;
            this.Location = new Point(x, y);

            Label titleLabel = new Label();
            titleLabel.Text = title;
            titleLabel.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            titleLabel.ForeColor = Color.Black;
            titleLabel.AutoSize = false;
            titleLabel.TextAlign = ContentAlignment.MiddleLeft;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Padding = new Padding(20, 10, 20, 0);
            titleLabel.Height = 40;

            Label messageLabel = new Label();
            messageLabel.Text = message;
            messageLabel.Font = new Font("Segoe UI", 11);
            messageLabel.ForeColor = Color.Gray;
            messageLabel.Dock = DockStyle.Fill;
            messageLabel.Padding = new Padding(20, 0, 20, 10);
            messageLabel.TextAlign = ContentAlignment.TopLeft;

            this.Controls.Add(messageLabel);
            this.Controls.Add(titleLabel);


            // Also register click on all labels so whole area is clickable
            foreach (Control c in this.Controls)
            {
                c.Click += NotificationForm_Click;
            }

            // Fade in
            var fadeTimer = new Timer();
            fadeTimer.Interval = 20;
            fadeTimer.Tick += (s, e) =>
            {
                if (this.Opacity < 1.0)
                    this.Opacity += opacityIncrement;
                else
                    fadeTimer.Stop();
            };
            fadeTimer.Start();

            // Save timeout value
            displayDuration = timeoutMs;

            // Auto close
            closeTimer = new Timer();
            closeTimer.Interval = displayDuration;
            closeTimer.Tick += (s, e) =>
            {
                closeTimer.Stop();
                BeginFadeOut();
            };
            closeTimer.Start();
        }

        private void NotificationForm_Click(object sender, EventArgs e)
        {
            if (!fadingOut)
            {
                closeTimer.Stop();
                BeginFadeOut();
            }
        }

        private void NotificationForm_Paint(object sender, PaintEventArgs e)
        {
            using (Pen p = new Pen(Color.LightGray, 2))
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawRectangle(p, new Rectangle(1, 1, this.Width - 2, this.Height - 2));
            }
        }

        private void BeginFadeOut()
        {
            fadingOut = true;
            var fadeOutTimer = new Timer();
            fadeOutTimer.Interval = 20;
            fadeOutTimer.Tick += (s, e) =>
            {
                if (this.Opacity > 0.0)
                    this.Opacity -= opacityDecrement;
                else
                {
                    fadeOutTimer.Stop();
                    this.Close();
                }
            };
            fadeOutTimer.Start();
        }

        // STATIC EASY CALL METHOD
        public static void ShowNotification(string title, string message, int timeoutMs = 4000)
        {
            if (Application.MessageLoop)
            {
                // We are in the UI thread
                var notif = new NotificationForm(title, message, timeoutMs);
                notif.Show();
            }
            else
            {
                // We are NOT in the UI thread, so marshal the call
                Application.OpenForms[0].BeginInvoke(new Action(() =>
                {
                    var notif = new NotificationForm(title, message, timeoutMs);
                    notif.Show();
                }));
            }
        }
    }
}
