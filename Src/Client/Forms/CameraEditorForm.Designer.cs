namespace AnotherRTSP.Forms
{
    partial class CameraEditorForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.camName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.camURL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.isDisabled = new System.Windows.Forms.CheckBox();
            this.isHardDecode = new System.Windows.Forms.CheckBox();
            this.isTCP = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.camWidth = new System.Windows.Forms.NumericUpDown();
            this.camHeight = new System.Windows.Forms.NumericUpDown();
            this.camX = new System.Windows.Forms.NumericUpDown();
            this.camY = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.camWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camY)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // camName
            // 
            this.camName.Location = new System.Drawing.Point(15, 25);
            this.camName.Name = "camName";
            this.camName.Size = new System.Drawing.Size(413, 20);
            this.camName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Url:";
            // 
            // camURL
            // 
            this.camURL.Location = new System.Drawing.Point(15, 64);
            this.camURL.Name = "camURL";
            this.camURL.Size = new System.Drawing.Size(413, 20);
            this.camURL.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Width:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(118, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Height:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "X:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(118, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Y:";
            // 
            // isDisabled
            // 
            this.isDisabled.AutoSize = true;
            this.isDisabled.Location = new System.Drawing.Point(15, 168);
            this.isDisabled.Name = "isDisabled";
            this.isDisabled.Size = new System.Drawing.Size(67, 17);
            this.isDisabled.TabIndex = 12;
            this.isDisabled.Text = "Disabled";
            this.isDisabled.UseVisualStyleBackColor = true;
            // 
            // isHardDecode
            // 
            this.isHardDecode.AutoSize = true;
            this.isHardDecode.Location = new System.Drawing.Point(15, 191);
            this.isHardDecode.Name = "isHardDecode";
            this.isHardDecode.Size = new System.Drawing.Size(90, 17);
            this.isHardDecode.TabIndex = 13;
            this.isHardDecode.Text = "Hard Decode";
            this.isHardDecode.UseVisualStyleBackColor = true;
            // 
            // isTCP
            // 
            this.isTCP.AutoSize = true;
            this.isTCP.Location = new System.Drawing.Point(15, 214);
            this.isTCP.Name = "isTCP";
            this.isTCP.Size = new System.Drawing.Size(147, 17);
            this.isTCP.TabIndex = 14;
            this.isTCP.Text = "TCP (Checked) else UDP";
            this.isTCP.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 237);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(353, 237);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 16;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // camWidth
            // 
            this.camWidth.Location = new System.Drawing.Point(15, 103);
            this.camWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.camWidth.Name = "camWidth";
            this.camWidth.Size = new System.Drawing.Size(100, 20);
            this.camWidth.TabIndex = 17;
            // 
            // camHeight
            // 
            this.camHeight.Location = new System.Drawing.Point(121, 103);
            this.camHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.camHeight.Name = "camHeight";
            this.camHeight.Size = new System.Drawing.Size(100, 20);
            this.camHeight.TabIndex = 18;
            // 
            // camX
            // 
            this.camX.Location = new System.Drawing.Point(15, 142);
            this.camX.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.camX.Name = "camX";
            this.camX.Size = new System.Drawing.Size(100, 20);
            this.camX.TabIndex = 19;
            // 
            // camY
            // 
            this.camY.Location = new System.Drawing.Point(121, 142);
            this.camY.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.camY.Name = "camY";
            this.camY.Size = new System.Drawing.Size(100, 20);
            this.camY.TabIndex = 20;
            // 
            // CameraEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 270);
            this.Controls.Add(this.camY);
            this.Controls.Add(this.camX);
            this.Controls.Add(this.camHeight);
            this.Controls.Add(this.camWidth);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.isTCP);
            this.Controls.Add(this.isHardDecode);
            this.Controls.Add(this.isDisabled);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.camURL);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.camName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "CameraEditorForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Camera Editor";
            ((System.ComponentModel.ISupportInitialize)(this.camWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camY)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox camName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox camURL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox isDisabled;
        private System.Windows.Forms.CheckBox isHardDecode;
        private System.Windows.Forms.CheckBox isTCP;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.NumericUpDown camWidth;
        private System.Windows.Forms.NumericUpDown camHeight;
        private System.Windows.Forms.NumericUpDown camX;
        private System.Windows.Forms.NumericUpDown camY;
    }
}