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
            this.button2 = new System.Windows.Forms.Button();
            this.resetPositionsBtn = new System.Windows.Forms.Button();
            this.backupSettingsbutton4 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.customUIcheckBox1 = new System.Windows.Forms.CheckBox();
            this.LoggingcheckBox1 = new System.Windows.Forms.CheckBox();
            this.camerasTab = new System.Windows.Forms.TabPage();
            this.selectedCameraLabel = new System.Windows.Forms.Label();
            this.camerasListView1 = new System.Windows.Forms.ListView();
            this.cameraNameHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cameraUrlHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.camerasListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.advancedTabPage = new System.Windows.Forms.TabPage();
            this.allCamerasOntopCheckbox = new System.Windows.Forms.CheckBox();
            this.disableCameraCaptionCheckbox = new System.Windows.Forms.CheckBox();
            this.staticCameraCaptioncheckBox1 = new System.Windows.Forms.CheckBox();
            this.allWindowsFocus = new System.Windows.Forms.CheckBox();
            this.checkBoxLedsAlertSounds = new System.Windows.Forms.CheckBox();
            this.checkBoxLedsOnTop = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.generalTab.SuspendLayout();
            this.camerasTab.SuspendLayout();
            this.camerasListContextMenu.SuspendLayout();
            this.mqttTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.advancedTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(136, 2);
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
            this.generalTab.Controls.Add(this.button2);
            this.generalTab.Controls.Add(this.resetPositionsBtn);
            this.generalTab.Controls.Add(this.backupSettingsbutton4);
            this.generalTab.Controls.Add(this.label9);
            this.generalTab.Controls.Add(this.label6);
            this.generalTab.Controls.Add(this.customUIcheckBox1);
            this.generalTab.Controls.Add(this.LoggingcheckBox1);
            this.generalTab.Controls.Add(this.button1);
            this.generalTab.Location = new System.Drawing.Point(4, 22);
            this.generalTab.Name = "generalTab";
            this.generalTab.Padding = new System.Windows.Forms.Padding(3);
            this.generalTab.Size = new System.Drawing.Size(352, 232);
            this.generalTab.TabIndex = 0;
            this.generalTab.Text = "General";
            this.generalTab.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 174);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "TEST";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // resetPositionsBtn
            // 
            this.resetPositionsBtn.Location = new System.Drawing.Point(8, 203);
            this.resetPositionsBtn.Name = "resetPositionsBtn";
            this.resetPositionsBtn.Size = new System.Drawing.Size(161, 23);
            this.resetPositionsBtn.TabIndex = 5;
            this.resetPositionsBtn.Text = "Reset windows positions";
            this.resetPositionsBtn.UseVisualStyleBackColor = true;
            this.resetPositionsBtn.Click += new System.EventHandler(this.resetPositionsBtn_Click);
            // 
            // backupSettingsbutton4
            // 
            this.backupSettingsbutton4.Location = new System.Drawing.Point(237, 203);
            this.backupSettingsbutton4.Name = "backupSettingsbutton4";
            this.backupSettingsbutton4.Size = new System.Drawing.Size(109, 23);
            this.backupSettingsbutton4.TabIndex = 4;
            this.backupSettingsbutton4.Text = "Backup settings";
            this.backupSettingsbutton4.UseVisualStyleBackColor = true;
            this.backupSettingsbutton4.Click += new System.EventHandler(this.backupSettingsbutton4_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(25, 62);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(202, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = " It works but have very basic functionality";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(273, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "*Note that single window mode is not in development yet";
            // 
            // customUIcheckBox1
            // 
            this.customUIcheckBox1.AutoSize = true;
            this.customUIcheckBox1.Enabled = false;
            this.customUIcheckBox1.Location = new System.Drawing.Point(6, 29);
            this.customUIcheckBox1.Name = "customUIcheckBox1";
            this.customUIcheckBox1.Size = new System.Drawing.Size(243, 17);
            this.customUIcheckBox1.TabIndex = 1;
            this.customUIcheckBox1.Text = "Use custom UI instead of single window mode";
            this.customUIcheckBox1.UseVisualStyleBackColor = true;
            // 
            // LoggingcheckBox1
            // 
            this.LoggingcheckBox1.AutoSize = true;
            this.LoggingcheckBox1.Location = new System.Drawing.Point(6, 6);
            this.LoggingcheckBox1.Name = "LoggingcheckBox1";
            this.LoggingcheckBox1.Size = new System.Drawing.Size(124, 17);
            this.LoggingcheckBox1.TabIndex = 0;
            this.LoggingcheckBox1.Text = "Enable logging to file";
            this.LoggingcheckBox1.UseVisualStyleBackColor = true;
            // 
            // camerasTab
            // 
            this.camerasTab.Controls.Add(this.selectedCameraLabel);
            this.camerasTab.Controls.Add(this.camerasListView1);
            this.camerasTab.Location = new System.Drawing.Point(4, 22);
            this.camerasTab.Name = "camerasTab";
            this.camerasTab.Padding = new System.Windows.Forms.Padding(3);
            this.camerasTab.Size = new System.Drawing.Size(352, 232);
            this.camerasTab.TabIndex = 1;
            this.camerasTab.Text = "Cameras";
            this.camerasTab.UseVisualStyleBackColor = true;
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
            this.camerasListView1.Size = new System.Drawing.Size(352, 232);
            this.camerasListView1.TabIndex = 0;
            this.camerasListView1.UseCompatibleStateImageBehavior = false;
            this.camerasListView1.View = System.Windows.Forms.View.Details;
            this.camerasListView1.SelectedIndexChanged += new System.EventHandler(this.camerasListView1_SelectedIndexChanged);
            this.camerasListView1.DoubleClick += new System.EventHandler(this.camerasListView1_DoubleClick);
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
            // camerasListContextMenu
            // 
            this.camerasListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.camerasListContextMenu.Name = "camerasListContextMenu";
            this.camerasListContextMenu.Size = new System.Drawing.Size(108, 70);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
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
            this.mqttalertsbtn.Visible = false;
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
            // advancedTabPage
            // 
            this.advancedTabPage.Controls.Add(this.allCamerasOntopCheckbox);
            this.advancedTabPage.Controls.Add(this.disableCameraCaptionCheckbox);
            this.advancedTabPage.Controls.Add(this.staticCameraCaptioncheckBox1);
            this.advancedTabPage.Controls.Add(this.allWindowsFocus);
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
            // allCamerasOntopCheckbox
            // 
            this.allCamerasOntopCheckbox.AutoSize = true;
            this.allCamerasOntopCheckbox.Location = new System.Drawing.Point(8, 121);
            this.allCamerasOntopCheckbox.Name = "allCamerasOntopCheckbox";
            this.allCamerasOntopCheckbox.Size = new System.Drawing.Size(211, 17);
            this.allCamerasOntopCheckbox.TabIndex = 5;
            this.allCamerasOntopCheckbox.Text = "All Cameras windows are always on top";
            this.allCamerasOntopCheckbox.UseVisualStyleBackColor = true;
            // 
            // disableCameraCaptionCheckbox
            // 
            this.disableCameraCaptionCheckbox.AutoSize = true;
            this.disableCameraCaptionCheckbox.Location = new System.Drawing.Point(8, 98);
            this.disableCameraCaptionCheckbox.Name = "disableCameraCaptionCheckbox";
            this.disableCameraCaptionCheckbox.Size = new System.Drawing.Size(191, 17);
            this.disableCameraCaptionCheckbox.TabIndex = 4;
            this.disableCameraCaptionCheckbox.Text = "Disable Camera caption completely";
            this.disableCameraCaptionCheckbox.UseVisualStyleBackColor = true;
            // 
            // staticCameraCaptioncheckBox1
            // 
            this.staticCameraCaptioncheckBox1.AutoSize = true;
            this.staticCameraCaptioncheckBox1.Location = new System.Drawing.Point(8, 75);
            this.staticCameraCaptioncheckBox1.Name = "staticCameraCaptioncheckBox1";
            this.staticCameraCaptioncheckBox1.Size = new System.Drawing.Size(129, 17);
            this.staticCameraCaptioncheckBox1.TabIndex = 3;
            this.staticCameraCaptioncheckBox1.Text = "Static camera caption";
            this.staticCameraCaptioncheckBox1.UseVisualStyleBackColor = true;
            // 
            // allWindowsFocus
            // 
            this.allWindowsFocus.AutoSize = true;
            this.allWindowsFocus.Location = new System.Drawing.Point(8, 52);
            this.allWindowsFocus.Name = "allWindowsFocus";
            this.allWindowsFocus.Size = new System.Drawing.Size(212, 17);
            this.allWindowsFocus.TabIndex = 2;
            this.allWindowsFocus.Text = "Focus all windows on one window click";
            this.allWindowsFocus.UseVisualStyleBackColor = true;
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
            // checkBoxLedsOnTop
            // 
            this.checkBoxLedsOnTop.AutoSize = true;
            this.checkBoxLedsOnTop.Location = new System.Drawing.Point(8, 6);
            this.checkBoxLedsOnTop.Name = "checkBoxLedsOnTop";
            this.checkBoxLedsOnTop.Size = new System.Drawing.Size(177, 17);
            this.checkBoxLedsOnTop.TabIndex = 0;
            this.checkBoxLedsOnTop.Text = "Alert light leds are always on top";
            this.checkBoxLedsOnTop.UseVisualStyleBackColor = true;
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsForm_FormClosed);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.Leave += new System.EventHandler(this.SettingsForm_Leave);
            this.tabControl1.ResumeLayout(false);
            this.generalTab.ResumeLayout(false);
            this.generalTab.PerformLayout();
            this.camerasTab.ResumeLayout(false);
            this.camerasTab.PerformLayout();
            this.camerasListContextMenu.ResumeLayout(false);
            this.mqttTab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.ListView camerasListView1;
        private System.Windows.Forms.ColumnHeader cameraNameHeader1;
        private System.Windows.Forms.ColumnHeader cameraUrlHeader1;
        private System.Windows.Forms.Label selectedCameraLabel;
        private System.Windows.Forms.ContextMenuStrip camerasListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.TabPage advancedTabPage;
        private System.Windows.Forms.CheckBox checkBoxLedsAlertSounds;
        private System.Windows.Forms.CheckBox checkBoxLedsOnTop;
        private System.Windows.Forms.CheckBox allWindowsFocus;
        private System.Windows.Forms.CheckBox staticCameraCaptioncheckBox1;
        private System.Windows.Forms.CheckBox disableCameraCaptionCheckbox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox customUIcheckBox1;
        private System.Windows.Forms.CheckBox LoggingcheckBox1;
        private System.Windows.Forms.Button backupSettingsbutton4;
        private System.Windows.Forms.CheckBox allCamerasOntopCheckbox;
        private System.Windows.Forms.Button resetPositionsBtn;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.Button button2;
    }
}