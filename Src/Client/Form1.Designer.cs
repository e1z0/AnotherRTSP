using System.Drawing;

namespace AnotherRTSP
{
    partial class PlayerForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerForm));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fullscreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Snop_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Recode_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OSDShow_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FullWindos_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.KeyFreamDecode_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RenderRect_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PlaySound_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullscreenToolStripMenuItem,
            this.Snop_MenuItem,
            this.Recode_MenuItem,
            this.OSDShow_MenuItem,
            this.FullWindos_MenuItem,
            this.KeyFreamDecode_MenuItem,
            this.RenderRect_MenuItem,
            this.PlaySound_MenuItem,
            this.reloadToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 202);
            this.contextMenuStrip1.Opened += new System.EventHandler(this.contextMenuStrip1_Opened);
            // 
            // fullscreenToolStripMenuItem
            // 
            this.fullscreenToolStripMenuItem.Name = "fullscreenToolStripMenuItem";
            this.fullscreenToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.fullscreenToolStripMenuItem.Text = "Fullscreen";
            this.fullscreenToolStripMenuItem.Click += new System.EventHandler(this.fullscreenToolStripMenuItem_Click);
            // 
            // Snop_MenuItem
            // 
            this.Snop_MenuItem.Name = "Snop_MenuItem";
            this.Snop_MenuItem.Size = new System.Drawing.Size(180, 22);
            this.Snop_MenuItem.Text = "Screenshot";
            this.Snop_MenuItem.Click += new System.EventHandler(this.Snop_MenuItem_Click);
            // 
            // Recode_MenuItem
            // 
            this.Recode_MenuItem.Name = "Recode_MenuItem";
            this.Recode_MenuItem.Size = new System.Drawing.Size(180, 22);
            this.Recode_MenuItem.Text = "Video recording";
            this.Recode_MenuItem.Click += new System.EventHandler(this.Recode_MenuItem_Click);
            // 
            // OSDShow_MenuItem
            // 
            this.OSDShow_MenuItem.Name = "OSDShow_MenuItem";
            this.OSDShow_MenuItem.Size = new System.Drawing.Size(180, 22);
            this.OSDShow_MenuItem.Text = "OSD display";
            this.OSDShow_MenuItem.Click += new System.EventHandler(this.OSDShow_MenuItem_Click);
            // 
            // FullWindos_MenuItem
            // 
            this.FullWindos_MenuItem.Name = "FullWindos_MenuItem";
            this.FullWindos_MenuItem.Size = new System.Drawing.Size(180, 22);
            this.FullWindos_MenuItem.Text = "Proportional display";
            this.FullWindos_MenuItem.Click += new System.EventHandler(this.FullWindos_MenuItem_Click);
            // 
            // KeyFreamDecode_MenuItem
            // 
            this.KeyFreamDecode_MenuItem.Name = "KeyFreamDecode_MenuItem";
            this.KeyFreamDecode_MenuItem.Size = new System.Drawing.Size(180, 22);
            this.KeyFreamDecode_MenuItem.Text = "Key frame decoding";
            this.KeyFreamDecode_MenuItem.Click += new System.EventHandler(this.KeyFreamDecode_MenuItem_Click);
            // 
            // RenderRect_MenuItem
            // 
            this.RenderRect_MenuItem.Name = "RenderRect_MenuItem";
            this.RenderRect_MenuItem.Size = new System.Drawing.Size(180, 22);
            this.RenderRect_MenuItem.Text = "Rendering area";
            // 
            // PlaySound_MenuItem
            // 
            this.PlaySound_MenuItem.Name = "PlaySound_MenuItem";
            this.PlaySound_MenuItem.Size = new System.Drawing.Size(180, 22);
            this.PlaySound_MenuItem.Text = "Play audio";
            this.PlaySound_MenuItem.Click += new System.EventHandler(this.PlaySound_MenuItem_Click);
            // 
            // reloadToolStripMenuItem
            // 
            this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            this.reloadToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reloadToolStripMenuItem.Text = "Reload";
            this.reloadToolStripMenuItem.Click += new System.EventHandler(this.reloadToolStripMenuItem_Click);
            // 
            // PlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 571);
            this.Font = new System.Drawing.Font("Courier New", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "PlayerForm";
            this.Text = "AnotherRTSP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlayerForm_FormClosing);
            this.Load += new System.EventHandler(this.PlayerForm_Load);
            this.Resize += new System.EventHandler(this.PlayerForm_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Snop_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem Recode_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem OSDShow_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem FullWindos_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem KeyFreamDecode_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem RenderRect_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem PlaySound_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem reloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fullscreenToolStripMenuItem;
    }
}

