using EasyPlayerNetSDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using AnotherRTSP.Classes;

/*
 * 
 * TODO:
 * * Show application cpu/memory usage on the window
 * * Make stream stop/start context menu on each video
 * * Make double click full screen video
 * * Window resize, also resizes the video tiles
 * * Remove white borders from the full screen
 */

namespace AnotherRTSP
{
    public partial class PlayerForm : Form
    {

        private PlayerSdk.MediaSourceCallBack callBack = null;
        private bool isInit = false;
        private int channelID = -1;
        private bool isTCP = false;
        private bool isHardEncode = false;
        private PlayerSdk.RENDER_FORMAT RENDER_FORMAT = PlayerSdk.RENDER_FORMAT.DISPLAY_FORMAT_RGB24_GDI;
        private Panel[] panels = new Panel[10];
        private int[] Chans = new int[10];
        private int focusedVideo = -1;
        private bool videoFullScreen = false;
        private bool windowFullScreen = false;
        private int streamCache = 3;
        //private const int PanelSize = 24;
        //private const int PanelCount = 10;
        public PlayerForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.Sizable;
            Width = YmlSettings.Data.WindowWidth;
            Height = YmlSettings.Data.WindowHeight;
            if (YmlSettings.Data.WindowX > 0 && YmlSettings.Data.WindowY > 0)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(YmlSettings.Data.WindowX, YmlSettings.Data.WindowY);
            }
        }

        /// <summary>
        /// Screenshot
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Snop_MenuItem_Click(object sender, EventArgs e)
        {
            if (channelID <= 0)
                return;
            int ret = PlayerSdk.EasyPlayer_PicShot(channelID);
        }

        /// <summary>
        /// Recording
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Recode_MenuItem_Click(object sender, EventArgs e)
        {
            if (channelID <= 0)
                return;

            var checkState = (sender as ToolStripMenuItem).CheckState;
            if (checkState == CheckState.Unchecked)
            {
                int ret = PlayerSdk.EasyPlayer_StartManuRecording(channelID);
                (sender as ToolStripMenuItem).CheckState = CheckState.Checked;
            }
            if (checkState == CheckState.Checked)
            {
                int ret = PlayerSdk.EasyPlayer_StopManuRecording(channelID);
                (sender as ToolStripMenuItem).CheckState = CheckState.Unchecked;
            }
        }

        /// <summary>
        /// OSD Display
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OSDShow_MenuItem_Click(object sender, EventArgs e)
        {
            if (channelID <= 0)
                return;

            var checkState = (sender as ToolStripMenuItem).CheckState;
            if (checkState == CheckState.Unchecked)
            {
                int ret = PlayerSdk.EasyPlayer_ShowStatisticalInfo(channelID, 1);
                (sender as ToolStripMenuItem).CheckState = CheckState.Checked;
            }
            if (checkState == CheckState.Checked)
            {
                int ret = PlayerSdk.EasyPlayer_ShowStatisticalInfo(channelID, 0);
                (sender as ToolStripMenuItem).CheckState = CheckState.Unchecked;
            }
        }

        /// <summary>
        /// Data flow callback
        /// </summary>
        /// <param name="_channelId">The _channel identifier.</param>
        /// <param name="_channelPtr">The _channel PTR.</param>
        /// <param name="_frameType">Type of the _frame.</param>
        /// <param name="pBuf">The p buf.</param>
        /// <param name="_frameInfo">The _frame information.</param>
        /// <returns>System.Int32.</returns>
        private int MediaCallback(int _channelId, IntPtr _channelPtr, int _frameType, IntPtr pBuf, ref PlayerSdk.EASY_FRAME_INFO _frameInfo)
        {
            return 0;
        }

        /// <summary>
        /// Proportional display (soft decoding only).
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void FullWindos_MenuItem_Click(object sender, EventArgs e)
        {
            if (channelID > 0)
            {
                var checkState = (sender as ToolStripMenuItem).CheckState;
                if (checkState == CheckState.Unchecked)
                {
                    int ret = PlayerSdk.EasyPlayer_SetShownToScale(channelID, 1);
                    (sender as ToolStripMenuItem).CheckState = CheckState.Checked;
                }
                if (checkState == CheckState.Checked)
                {
                    int ret = PlayerSdk.EasyPlayer_SetShownToScale(channelID, 0);
                    (sender as ToolStripMenuItem).CheckState = CheckState.Unchecked;
                }
            }
        }

        /// <summary>
        /// Keyframe playback
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void KeyFreamDecode_MenuItem_Click(object sender, EventArgs e)
        {
            if (channelID <= 0)
                return;

            var checkState = (sender as ToolStripMenuItem).CheckState;
            if (checkState == CheckState.Unchecked)
            {
                int ret = PlayerSdk.EasyPlayer_SetDecodeType(channelID, 1);
                (sender as ToolStripMenuItem).CheckState = CheckState.Checked;
            }
            if (checkState == CheckState.Checked)
            {
                int ret = PlayerSdk.EasyPlayer_SetShownToScale(channelID, 0);
                (sender as ToolStripMenuItem).CheckState = CheckState.Unchecked;
            }

        }

        /// <summary>
        /// partial rendering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*
        private void RenderRect_MenuItem_Click(object sender, EventArgs e)
        {
            if (channelID <= 0)
                return;

            var panelSize = this.panel1.ClientSize;
            int ret = PlayerSdk.EasyPlayer_SetRenderRect(channelID, new Rect { X = 0, Y = 0, Width = panelSize.Width / 2, Height = panelSize.Height / 2 });
            (sender as ToolStripMenuItem).CheckState = CheckState.Checked;

        }
        */
        /// <summary>
        /// Play audio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaySound_MenuItem_Click(object sender, EventArgs e)
        {
            if (channelID <= 0)
                return;

            var checkState = (sender as ToolStripMenuItem).CheckState;
            if (checkState == CheckState.Unchecked)
            {
                int ret = PlayerSdk.EasyPlayer_PlaySound(channelID);
                (sender as ToolStripMenuItem).CheckState = CheckState.Checked;
            }
            if (checkState == CheckState.Checked)
            {
                int ret = PlayerSdk.EasyPlayer_StopSound();
                (sender as ToolStripMenuItem).CheckState = CheckState.Unchecked;
            }
        }

        private void PlayerForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            YmlSettings.Data.WindowHeight = this.Height;
            YmlSettings.Data.WindowWidth = this.Width;
            YmlSettings.Data.WindowX = this.Location.X;
            YmlSettings.Data.WindowY = this.Location.Y;

            if (isInit)
            {
                foreach (int chan in Chans)
                {
                    PlayerSdk.EasyPlayer_CloseStream(chan);
                }
                PlayerSdk.EasyPlayer_Release();
            }
        }

        

        private void PlayerForm_Load(object sender, System.EventArgs e)
        {
            int LimitDayOut = PlayerSdk.EasyPlayer_Init();
            if (LimitDayOut >= 0)
                isInit = true;
            callBack = new PlayerSdk.MediaSourceCallBack(MediaCallback);
            //this.DecodeType.SelectedItem = "GDI";
            isTCP = true;
            isHardEncode = false;
            this.RightToLeft = RightToLeft.Inherit;

            // Create the panels
            int tileWidth = 240;
            int tileHeight = 180;
            int split = 3;
            int StartPoint = 0;
            int SplitSide = 10;

            // counter
            int i = 0;
            if (Camera.AllCameras != null && Camera.AllCameras.Count > 0)
            {
                foreach (Camera cam in Camera.AllCameras)
                {
                    //MessageBox.Show("cam: "+cam.Key+" url: "+cam.Value, "cam");
                    panels[i] = new Panel();
                    panels[i].Size = new System.Drawing.Size(tileWidth, tileHeight);
                    panels[i].Location = new System.Drawing.Point(StartPoint + (i % split) * tileWidth, SplitSide + (i / split) * tileHeight);
                    panels[i].BackColor = Color.Black;
                    panels[i].BorderStyle = BorderStyle.FixedSingle;
                    panels[i].ContextMenuStrip = contextMenuStrip1;
                    panels[i].DoubleClick += new EventHandler(videoPanel_DoubleClick);
                    panels[i].Tag = i;
                    Chans[i] = PlayerSdk.EasyPlayer_OpenStream(cam.Config.Url, panels[i].Handle, RENDER_FORMAT, isTCP ? 1 : 0, "", "", callBack, IntPtr.Zero, isHardEncode);
                    if (Chans[i] > 0)
                    {
                        PlayerSdk.EasyPlayer_SetFrameCache(Chans[i], streamCache);
                        PlayerSdk.EasyPlayer_SetShownToScale(Chans[i], 1);
                    }
                    this.Controls.Add(panels[i]);
                    i++;
                }
            }
            
        }

        private void DecodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var text = (sender as ComboBox).Text;
            switch (text.ToUpper())
            {
                case "GDI":
                    RENDER_FORMAT = PlayerSdk.RENDER_FORMAT.DISPLAY_FORMAT_RGB24_GDI; break;
                case "RGB565":
                    RENDER_FORMAT = PlayerSdk.RENDER_FORMAT.DISPLAY_FORMAT_RGB565; break;
                case "YV12":
                    RENDER_FORMAT = PlayerSdk.RENDER_FORMAT.DISPLAY_FORMAT_YV12; break;
                case "YUY2":
                    RENDER_FORMAT = PlayerSdk.RENDER_FORMAT.DISPLAY_FORMAT_YUY2; break;
                default:
                    break;
            }
        }

        private void rtpoverType_CheckStateChanged(object sender, EventArgs e)
        {
            isTCP = (sender as CheckBox).CheckState == CheckState.Checked;
        }


        // on context menu open, assign the focusedVideo variable to the correct panel of the exact video that where triggered context menu, we will use component tag property for this
        private void contextMenuStrip1_Opened(object sender, EventArgs e)
        {
            ContextMenuStrip menu = sender as ContextMenuStrip;
            Control sourceControl = menu.SourceControl;
            int id = (int)sourceControl.Tag;
            if (id >= 0)
            {
                focusedVideo = id;
                channelID = Chans[id];
            }
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (focusedVideo >= 0)
            {
                PlayerSdk.EasyPlayer_CloseStream(channelID);
            }
        }

        // full screen on context menu click
        private void fullscreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (videoFullScreen)
            {
                int index = 0;
                foreach (int panid in Chans)
                {
                    if (index != focusedVideo)
                    {
                        if (panels[index] != null)
                            panels[index].Visible = true;
                    }
                    index++;
                }
                panels[focusedVideo].Dock = DockStyle.None;
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                videoFullScreen = false;
            }
            else
            {
                int index = 0;
                foreach (int panid in Chans)
                {
                    if (index != focusedVideo)
                    {
                        if (panels[index] != null)
                            panels[index].Visible = false;
                    }
                    index++;
                }
                panels[focusedVideo].Dock = DockStyle.Fill;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
                videoFullScreen = true;
            }
        }

        // full screen on window
        private void videoPanel_DoubleClick(object sender, EventArgs e)
        {
            Panel pnl = sender as Panel;
            int id = (int)pnl.Tag;
            if (id >= 0)
            {
                focusedVideo = id;
                channelID = Chans[id];



                if (windowFullScreen)
                {
                    int index = 0;
                    foreach (int panid in Chans)
                    {
                        if (index != id)
                        {
                            if (panels[index] != null)
                                panels[index].Visible = true;
                        }
                        index++;
                    }
                    panels[focusedVideo].Dock = DockStyle.None;
                    windowFullScreen = false;
                }
                else
                {
                    int index = 0;
                    foreach (int panid in Chans)
                    {
                        if (index != id)
                        {
                            if (panels[index] != null)
                                panels[index].Visible = false;
                        }
                        index++;
                    }
                    panels[focusedVideo].Dock = DockStyle.Fill;
                    windowFullScreen = true;
                }
            }
        }

        private void PlayerForm_Resize(object sender, EventArgs e)
        {
          
        }
    }
}
