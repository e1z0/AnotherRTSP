namespace AnotherRTSP
{
    partial class SettingsForm
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.applybtn = new System.Windows.Forms.Button();
            this.okbtn = new System.Windows.Forms.Button();
            this.cancelbtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.generalTab = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.camerasTab = new System.Windows.Forms.TabPage();
            this.mqttTab = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mqttalertsbtn = new System.Windows.Forms.Button();
            this.clientidtextbox = new System.Windows.Forms.TextBox();
            this.passwordtextbox = new System.Windows.Forms.TextBox();
            this.usernametextbox = new System.Windows.Forms.TextBox();
            this.porttextbox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.servertextbox = new System.Windows.Forms.TextBox();
            this.mqtttest = new System.Windows.Forms.Button();
            this.mqttsupport_checkbox = new System.Windows.Forms.CheckBox();
            this.camerasListView1 = new System.Windows.Forms.ListView();
            this.button2 = new System.Windows.Forms.Button();
            this.cameraNameHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cameraUrlHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button3 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cameraNameEditBox = new System.Windows.Forms.TextBox();
            this.cameraSourceEditBox = new System.Windows.Forms.TextBox();
            this.selectedCameraLabel = new System.Windows.Forms.Label();
            this.camerasListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.advancedTabPage = new System.Windows.Forms.TabPage();
            this.checkBoxLedsOnTop = new System.Windows.Forms.CheckBox();
            this.checkBoxLedsAlertSounds = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.generalTab.SuspendLayout();
            this.camerasTab.SuspendLayout();
            this.mqttTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.camerasListContextMenu.SuspendLayout();
            this.advancedTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(81, 267);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "TestLog";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // applybtn
            // 
            this.applybtn.Location = new System.Drawing.Point(281, 267);
            this.applybtn.Name = "applybtn";
            this.applybtn.Size = new System.Drawing.Size(75, 23);
            this.applybtn.TabIndex = 3;
            this.applybtn.Text = "Apply";
            this.applybtn.UseVisualStyleBackColor = true;
            this.applybtn.Click += new System.EventHandler(this.applybtn_Click);
            // 
            // okbtn
            // 
            this.okbtn.Location = new System.Drawing.Point(0, 267);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(75, 23);
            this.okbtn.TabIndex = 4;
            this.okbtn.Text = "OK";
            this.okbtn.UseVisualStyleBackColor = true;
            this.okbtn.Click += new System.EventHandler(this.okbtn_Click);
            // 
            // cancelbtn
            // 
            this.cancelbtn.Location = new System.Drawing.Point(200, 267);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(75, 23);
            this.cancelbtn.TabIndex = 5;
            this.cancelbtn.Text = "Cancel";
            this.cancelbtn.UseVisualStyleBackColor = true;
            this.cancelbtn.Click += new System.EventHandler(this.cancelbtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.generalTab);
            this.tabControl1.Controls.Add(this.camerasTab);
            this.tabControl1.Controls.Add(this.mqttTab);
            this.tabControl1.Controls.Add(this.advancedTabPage);
            this.tabControl1.Location = new System.Drawing.Point(0, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(360, 258);
            this.tabControl1.TabIndex = 6;
            // 
            // generalTab
            // 
            this.generalTab.Controls.Add(this.label6);
            this.generalTab.Location = new System.Drawing.Point(4, 22);
            this.generalTab.Name = "generalTab";
            this.generalTab.Padding = new System.Windows.Forms.Padding(3);
            this.generalTab.Size = new System.Drawing.Size(352, 232);
            this.generalTab.TabIndex = 0;
            this.generalTab.Text = "General";
            this.generalTab.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Under heavy construction";
            // 
            // camerasTab
            // 
            this.camerasTab.Controls.Add(this.selectedCameraLabel);
            this.camerasTab.Controls.Add(this.cameraSourceEditBox);
            this.camerasTab.Controls.Add(this.cameraNameEditBox);
            this.camerasTab.Controls.Add(this.label8);
            this.camerasTab.Controls.Add(this.label7);
            this.camerasTab.Controls.Add(this.button3);
            this.camerasTab.Controls.Add(this.button2);
            this.camerasTab.Controls.Add(this.camerasListView1);
            this.camerasTab.Location = new System.Drawing.Point(4, 22);
            this.camerasTab.Name = "camerasTab";
            this.camerasTab.Padding = new System.Windows.Forms.Padding(3);
            this.camerasTab.Size = new System.Drawing.Size(352, 232);
            this.camerasTab.TabIndex = 1;
            this.camerasTab.Text = "Cameras";
            this.camerasTab.UseVisualStyleBackColor = true;
            // 
            // mqttTab
            // 
            this.mqttTab.Controls.Add(this.groupBox1);
            this.mqttTab.Location = new System.Drawing.Point(4, 22);
            this.mqttTab.Name = "mqttTab";
            this.mqttTab.Size = new System.Drawing.Size(352, 232);
            this.mqttTab.TabIndex = 2;
            this.mqttTab.Text = "MQTT";
            this.mqttTab.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mqttalertsbtn);
            this.groupBox1.Controls.Add(this.clientidtextbox);
            this.groupBox1.Controls.Add(this.passwordtextbox);
            this.groupBox1.Controls.Add(this.usernametextbox);
            this.groupBox1.Controls.Add(this.porttextbox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.servertextbox);
            this.groupBox1.Controls.Add(this.mqtttest);
            this.groupBox1.Controls.Add(this.mqttsupport_checkbox);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(346, 202);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MQTT Support";
            // 
            // mqttalertsbtn
            // 
            this.mqttalertsbtn.Location = new System.Drawing.Point(211, 19);
            this.mqttalertsbtn.Name = "mqttalertsbtn";
            this.mqttalertsbtn.Size = new System.Drawing.Size(75, 23);
            this.mqttalertsbtn.TabIndex = 12;
            this.mqttalertsbtn.Text = "Edit Alerts";
            this.mqttalertsbtn.UseVisualStyleBackColor = true;
            this.mqttalertsbtn.Click += new System.EventHandler(this.mqttalertsbtn_Click);
            // 
            // clientidtextbox
            // 
            this.clientidtextbox.Location = new System.Drawing.Point(74, 146);
            this.clientidtextbox.Name = "clientidtextbox";
            this.clientidtextbox.Size = new System.Drawing.Size(100, 20);
            this.clientidtextbox.TabIndex = 11;
            // 
            // passwordtextbox
            // 
            this.passwordtextbox.Location = new System.Drawing.Point(74, 121);
            this.passwordtextbox.Name = "passwordtextbox";
            this.passwordtextbox.Size = new System.Drawing.Size(100, 20);
            this.passwordtextbox.TabIndex = 10;
            // 
            // usernametextbox
            // 
            this.usernametextbox.Location = new System.Drawing.Point(74, 94);
            this.usernametextbox.Name = "usernametextbox";
            this.usernametextbox.Size = new System.Drawing.Size(100, 20);
            this.usernametextbox.TabIndex = 9;
            // 
            // porttextbox
            // 
            this.porttextbox.Location = new System.Drawing.Point(74, 68);
            this.porttextbox.Name = "porttextbox";
            this.porttextbox.Size = new System.Drawing.Size(100, 20);
            this.porttextbox.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "ClientID:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Server:";
            // 
            // servertextbox
            // 
            this.servertextbox.Location = new System.Drawing.Point(74, 42);
            this.servertextbox.Name = "servertextbox";
            this.servertextbox.Size = new System.Drawing.Size(100, 20);
            this.servertextbox.TabIndex = 2;
            // 
            // mqtttest
            // 
            this.mqtttest.Location = new System.Drawing.Point(9, 172);
            this.mqtttest.Name = "mqtttest";
            this.mqtttest.Size = new System.Drawing.Size(75, 23);
            this.mqtttest.TabIndex = 1;
            this.mqtttest.Text = "test";
            this.mqtttest.UseVisualStyleBackColor = true;
            this.mqtttest.Click += new System.EventHandler(this.mqtttest_Click);
            // 
            // mqttsupport_checkbox
            // 
            this.mqttsupport_checkbox.AutoSize = true;
            this.mqttsupport_checkbox.Location = new System.Drawing.Point(6, 19);
            this.mqttsupport_checkbox.Name = "mqttsupport_checkbox";
            this.mqttsupport_checkbox.Size = new System.Drawing.Size(139, 17);
            this.mqttsupport_checkbox.TabIndex = 0;
            this.mqttsupport_checkbox.Text = "MQTT Support Enabled";
            this.mqttsupport_checkbox.UseVisualStyleBackColor = true;
            // 
            // camerasListView1
            // 
            this.camerasListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cameraNameHeader1,
            this.cameraUrlHeader1});
            this.camerasListView1.ContextMenuStrip = this.camerasListContextMenu;
            this.camerasListView1.FullRowSelect = true;
            this.camerasListView1.GridLines = true;
            this.camerasListView1.Location = new System.Drawing.Point(0, 0);
            this.camerasListView1.Name = "camerasListView1";
            this.camerasListView1.Size = new System.Drawing.Size(352, 172);
            this.camerasListView1.TabIndex = 0;
            this.camerasListView1.UseCompatibleStateImageBehavior = false;
            this.camerasListView1.View = System.Windows.Forms.View.Details;
            this.camerasListView1.SelectedIndexChanged += new System.EventHandler(this.camerasListView1_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(263, 175);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(83, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Add/Update";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cameraNameHeader1
            // 
            this.cameraNameHeader1.Text = "Name";
            this.cameraNameHeader1.Width = 93;
            // 
            // cameraUrlHeader1
            // 
            this.cameraUrlHeader1.Text = "Source";
            this.cameraUrlHeader1.Width = 253;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(182, 175);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "New";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 181);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Name:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 203);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(44, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Source:";
            // 
            // cameraNameEditBox
            // 
            this.cameraNameEditBox.Location = new System.Drawing.Point(47, 178);
            this.cameraNameEditBox.Name = "cameraNameEditBox";
            this.cameraNameEditBox.Size = new System.Drawing.Size(100, 20);
            this.cameraNameEditBox.TabIndex = 5;
            // 
            // cameraSourceEditBox
            // 
            this.cameraSourceEditBox.Location = new System.Drawing.Point(47, 200);
            this.cameraSourceEditBox.Name = "cameraSourceEditBox";
            this.cameraSourceEditBox.Size = new System.Drawing.Size(299, 20);
            this.cameraSourceEditBox.TabIndex = 6;
            // 
            // selectedCameraLabel
            // 
            this.selectedCameraLabel.AutoSize = true;
            this.selectedCameraLabel.Location = new System.Drawing.Point(296, 73);
            this.selectedCameraLabel.Name = "selectedCameraLabel";
            this.selectedCameraLabel.Size = new System.Drawing.Size(16, 13);
            this.selectedCameraLabel.TabIndex = 7;
            this.selectedCameraLabel.Text = "-1";
            this.selectedCameraLabel.Visible = false;
            // 
            // camerasListContextMenu
            // 
            this.camerasListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.camerasListContextMenu.Name = "camerasListContextMenu";
            this.camerasListContextMenu.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // advancedTabPage
            // 
            this.advancedTabPage.Controls.Add(this.checkBoxLedsAlertSounds);
            this.advancedTabPage.Controls.Add(this.checkBoxLedsOnTop);
            this.advancedTabPage.Location = new System.Drawing.Point(4, 22);
            this.advancedTabPage.Name = "advancedTabPage";
            this.advancedTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.advancedTabPage.Size = new System.Drawing.Size(352, 232);
            this.advancedTabPage.TabIndex = 3;
            this.advancedTabPage.Text = "Advanced";
            this.advancedTabPage.UseVisualStyleBackColor = true;
            // 
            // checkBoxLedsOnTop
            // 
            this.checkBoxLedsOnTop.AutoSize = true;
            this.checkBoxLedsOnTop.Location = new System.Drawing.Point(8, 6);
            this.checkBoxLedsOnTop.Name = "checkBoxLedsOnTop";
            this.checkBoxLedsOnTop.Size = new System.Drawing.Size(165, 17);
            this.checkBoxLedsOnTop.TabIndex = 0;
            this.checkBoxLedsOnTop.Text = "MQTT leds are always on top";
            this.checkBoxLedsOnTop.UseVisualStyleBackColor = true;
            // 
            // checkBoxLedsAlertSounds
            // 
            this.checkBoxLedsAlertSounds.AutoSize = true;
            this.checkBoxLedsAlertSounds.Location = new System.Drawing.Point(8, 29);
            this.checkBoxLedsAlertSounds.Name = "checkBoxLedsAlertSounds";
            this.checkBoxLedsAlertSounds.Size = new System.Drawing.Size(144, 17);
            this.checkBoxLedsAlertSounds.TabIndex = 1;
            this.checkBoxLedsAlertSounds.Text = "Play Sound on Led alerts";
            this.checkBoxLedsAlertSounds.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 292);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cancelbtn);
            this.Controls.Add(this.okbtn);
            this.Controls.Add(this.applybtn);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.generalTab.ResumeLayout(false);
            this.generalTab.PerformLayout();
            this.camerasTab.ResumeLayout(false);
            this.camerasTab.PerformLayout();
            this.mqttTab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.camerasListContextMenu.ResumeLayout(false);
            this.advancedTabPage.ResumeLayout(false);
            this.advancedTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button applybtn;
        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.Button cancelbtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage generalTab;
        private System.Windows.Forms.TabPage camerasTab;
        private System.Windows.Forms.TabPage mqttTab;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button mqttalertsbtn;
        private System.Windows.Forms.TextBox clientidtextbox;
        private System.Windows.Forms.TextBox passwordtextbox;
        private System.Windows.Forms.TextBox usernametextbox;
        private System.Windows.Forms.TextBox porttextbox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox servertextbox;
        private System.Windows.Forms.Button mqtttest;
        private System.Windows.Forms.CheckBox mqttsupport_checkbox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListView camerasListView1;
        private System.Windows.Forms.ColumnHeader cameraNameHeader1;
        private System.Windows.Forms.ColumnHeader cameraUrlHeader1;
        private System.Windows.Forms.TextBox cameraSourceEditBox;
        private System.Windows.Forms.TextBox cameraNameEditBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label selectedCameraLabel;
        private System.Windows.Forms.ContextMenuStrip camerasListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.TabPage advancedTabPage;
        private System.Windows.Forms.CheckBox checkBoxLedsAlertSounds;
        private System.Windows.Forms.CheckBox checkBoxLedsOnTop;
    }
}