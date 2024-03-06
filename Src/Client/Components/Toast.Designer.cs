namespace AnotherRTSP.Components
{
    partial class Toast
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Message = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Message
            // 
            this.Message.BackColor = System.Drawing.Color.White;
            this.Message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Message.Location = new System.Drawing.Point(0, 0);
            this.Message.Name = "Message";
            this.Message.Size = new System.Drawing.Size(405, 25);
            this.Message.TabIndex = 0;
            this.Message.Text = "label1";
            this.Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Toast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(405, 25);
            this.Controls.Add(this.Message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Toast";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Toast";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Message;
    }
}