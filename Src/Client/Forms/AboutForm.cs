using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Media;
using System.Diagnostics;

namespace AnotherRTSP.Forms
{
   public partial class AboutForm : Form
    {
        private Random random = new Random();
        private Timer animationTimer;
        private int[] yPositions;
        private float opacityIncrement = 0.05f;
        private const int FontSize = 18;
        private const int FadeSpeedMs = 30;
        private string appName = "AnotherRTSP";
        private string version = "v"+AnotherRTSP.Classes.YmlSettings.VERSION;
        private string author = "by Justinas K (e1z0)";
        private string link = "https://github.com/e1z0/AnotherRTSP";
        private RectangleF linkArea;


        private string[] emojiSymbols = new string[]
{
    "😎", "🚀", "🔥", "🤖", "🎯", "🌌", "✨", "🛸", "👾", "⚡", "💻", "🎶", "🧠", "📡", "🧬"
};


        private SoundPlayer ambientPlayer;

        // For rounded corners (optional)
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        public AboutForm()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;
            this.Size = new Size(640, 420);
            this.TopMost = true;
            this.Opacity = 0.0;

            // Rounded corners
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, 20, 20));

            int columns = this.Width / FontSize;
            yPositions = new int[columns];

            animationTimer = new Timer();
            animationTimer.Interval = 100;
            animationTimer.Tick += AnimationTimer_Tick;
            animationTimer.Start();

            this.Load += AboutForm_Load;
            //this.Click += (s, e) => BeginFadeOut(); // Click to fade close
            this.Click += AboutForm_Click;
        }

        private void AboutForm_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Point clickPoint = me.Location;

            if (linkArea.Contains(clickPoint))
            {
                try
                {
                    Process.Start(link);
                }
                catch (Exception ex)
                {
                    AnotherRTSP.Classes.Logger.WriteLog("[About] Failed to open link: " + ex.Message);
                }
            }
            else
            {
                BeginFadeOut(); // normal fade out if clicked elsewhere
            }
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            // Start ambient music (if exists)
            string ambientPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sounds", "ambient.wav");
            if (System.IO.File.Exists(ambientPath))
            {
                ambientPlayer = new SoundPlayer(ambientPath);
                ambientPlayer.PlayLooping();
            }

            // Start fade-in
            var fadeTimer = new Timer();
            fadeTimer.Interval = FadeSpeedMs;
            fadeTimer.Tick += (s2, e2) =>
            {
                if (this.Opacity < 1.0)
                    this.Opacity += opacityIncrement;
                else
                    (s2 as Timer).Stop();
            };
            fadeTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            // Background neon pulse
            Rectangle rect = this.ClientRectangle;
            using (LinearGradientBrush gradient = new LinearGradientBrush(rect, Color.DarkBlue, Color.DarkMagenta, 45f))
            {
                g.FillRectangle(gradient, rect);
            }

            Brush greenBrush = new SolidBrush(Color.LimeGreen);
            Brush neonBrush = new SolidBrush(Color.Cyan);

            //Font matrixFont = new Font("Consolas", FontSize, FontStyle.Bold);
            Font matrixFont = new Font("Segoe UI Emoji", FontSize, FontStyle.Bold);


            int columns = this.Width / FontSize;

            for (int i = 0; i < columns; i++)
            {
                //char c = (char)(0x30A0 + random.Next(0, 96)); // Random Katakana symbols
                //string letter = c.ToString();
                string letter = emojiSymbols[random.Next(emojiSymbols.Length)];


                float x = i * FontSize;
                float y = yPositions[i] * FontSize;

                g.DrawString(letter, matrixFont, greenBrush, x, y);

                if (y > this.Height && random.NextDouble() > 0.975)
                {
                    yPositions[i] = 0;
                }
                else
                {
                    yPositions[i]++;
                }
            }

            // Draw neon title
            Font titleFont = new Font("Segoe UI", 26, FontStyle.Bold);
            Font infoFont = new Font("Segoe UI", 14, FontStyle.Regular);

            SizeF appSize = g.MeasureString(appName, titleFont);
            SizeF verSize = g.MeasureString(version, infoFont);
            SizeF authSize = g.MeasureString(author, infoFont);
            SizeF linkSize = g.MeasureString(link, infoFont);

            g.DrawString(appName, titleFont, neonBrush, (this.Width - appSize.Width) / 2, 30);
            g.DrawString(version, infoFont, Brushes.WhiteSmoke, (this.Width - verSize.Width) / 2, 80);
            g.DrawString(author, infoFont, Brushes.WhiteSmoke, (this.Width - authSize.Width) / 2, 110);
            g.DrawString(link, infoFont, Brushes.WhiteSmoke, (this.Width - linkSize.Width) / 2, 140);
            float linkX = (this.Width - linkSize.Width) / 2;
            float linkY = 140;
            linkArea = new RectangleF(linkX, linkY, linkSize.Width, linkSize.Height);


            base.OnPaint(e);
        }

        private void BeginFadeOut()
        {
            if (ambientPlayer != null)
            {
                ambientPlayer.Stop();
            }

            var fadeOutTimer = new Timer();
            fadeOutTimer.Interval = FadeSpeedMs;
            fadeOutTimer.Tick += (s, e) =>
            {
                if (this.Opacity > 0.0)
                    this.Opacity -= opacityIncrement;
                else
                {
                    (s as Timer).Stop();
                    this.Close();
                }
            };
            fadeOutTimer.Start();
        }
    }
}
