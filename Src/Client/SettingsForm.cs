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
using AnotherRTSP.Classes;
using AnotherRTSP.Forms;
using uPLibrary.Networking.M2Mqtt;
using System.IO;
using System.Reflection;

namespace AnotherRTSP
{
    public partial class SettingsForm : Form
    {

        public SettingsForm()
        {
            InitializeComponent();

        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            loadSettings();
        }

        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Logger.WriteLog("testing...");
        }

        private void setSettings()
        {
            // new settings interface
            YmlSettings.Data.MqttEnabled = mqttsupport_checkbox.Checked;
            YmlSettings.Data.MQTT.Server = servertextbox.Text;
            YmlSettings.Data.MQTT.Port = int.Parse(porttextbox.Text);
            YmlSettings.Data.MQTT.Username = usernametextbox.Text;
            YmlSettings.Data.MQTT.Password = passwordtextbox.Text;
            YmlSettings.Data.MQTT.ClientID = clientidtextbox.Text;

            // general settings
            YmlSettings.Data.Logging = LoggingcheckBox1.Checked;
            YmlSettings.Data.CustomLayout = LoggingcheckBox1.Checked;

            // mqtt section
            YmlSettings.Data.MqttEnabled = mqttsupport_checkbox.Checked;
            if (servertextbox.Text != "")
                YmlSettings.Data.MQTT.Server = servertextbox.Text;
            if (porttextbox.Text != "")
                YmlSettings.Data.MQTT.Port = int.Parse(porttextbox.Text);
            if (usernametextbox.Text != "")
                YmlSettings.Data.MQTT.Username = usernametextbox.Text;
            if (passwordtextbox.Text != "")
                YmlSettings.Data.MQTT.Password = passwordtextbox.Text;
            if (clientidtextbox.Text != "")
                YmlSettings.Data.MQTT.ClientID = clientidtextbox.Text;

            // advanced tab
            YmlSettings.Advanced.LedsWindowOnTop = checkBoxLedsOnTop.Checked;
            YmlSettings.Advanced.LedsSoundAlert = checkBoxLedsAlertSounds.Checked;
            YmlSettings.Advanced.FocusAllWindowsOnClick = allWindowsFocus.Checked;
            YmlSettings.Advanced.StaticCameraCaption = staticCameraCaptioncheckBox1.Checked;
            YmlSettings.Advanced.DisableCameraCaptions = disableCameraCaptionCheckbox.Checked;
            YmlSettings.Advanced.AllCamerasWindowsOnTop = allCamerasOntopCheckbox.Checked;
        }

        private void loadSettings()
        {
            // general settings
            LoggingcheckBox1.Checked = YmlSettings.Data.Logging;
            customUIcheckBox1.Checked = YmlSettings.Data.CustomLayout;

            // mqtt section
            mqttsupport_checkbox.Checked = YmlSettings.Data.MqttEnabled;
            servertextbox.Text = YmlSettings.Data.MQTT.Server;
            porttextbox.Text = YmlSettings.Data.MQTT.Port.ToString();
            usernametextbox.Text = YmlSettings.Data.MQTT.Username;
            passwordtextbox.Text = YmlSettings.Data.MQTT.Password;
            clientidtextbox.Text = YmlSettings.Data.MQTT.ClientID;


            // load cameras
            foreach (CameraItem cam in YmlSettings.Data.Cameras)
            {
                ListViewItem item = new ListViewItem(new[] { cam.Name, cam.Url });
                camerasListView1.Items.Add(item);
            }
            // advanced tab
            checkBoxLedsOnTop.Checked = YmlSettings.Data.AdvancedSettings.LedsWindowOnTop;
            checkBoxLedsAlertSounds.Checked = YmlSettings.Data.AdvancedSettings.LedsSoundAlert;
            allWindowsFocus.Checked = YmlSettings.Data.AdvancedSettings.FocusAllWindowsOnClick;
            staticCameraCaptioncheckBox1.Checked = YmlSettings.Data.AdvancedSettings.StaticCameraCaption;
            disableCameraCaptionCheckbox.Checked = YmlSettings.Data.AdvancedSettings.DisableCameraCaptions;
            allCamerasOntopCheckbox.Checked = YmlSettings.Data.AdvancedSettings.AllCamerasWindowsOnTop;
        }

        private void okbtn_Click(object sender, EventArgs e)
        {
            setSettings();
            this.Close();
        }

        private void applybtn_Click(object sender, EventArgs e)
        {
            setSettings();
        }

        private void cancelbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mqtttest_Click(object sender, EventArgs e)
        {
            // test mqtt connectivity
            var clientid = clientidtextbox.Text ?? "AnotherRTSP";
            MqttClient client = new MqttClient(servertextbox.Text);
            try
            {
                var status = client.Connect(clientid);
                if (status == 0)
                {
                    MessageBox.Show("Connected!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect! Err:" + ex.ToString());
            }


        }

        private void mqttalertsbtn_Click(object sender, EventArgs e)
        {
            MqttRulesForm mqttrls = new MqttRulesForm();
            mqttrls.Show();
        }

        private void camerasListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }





        private void backupSettingsbutton4_Click(object sender, EventArgs e)
        {
            try
            {
                // Get application directory
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string backupsDirectory = Path.Combine(appDirectory, "backups");

                // Ensure backups folder exists
                if (!Directory.Exists(backupsDirectory))
                {
                    Directory.CreateDirectory(backupsDirectory);
                }

                // Create filename
                string timestamp = DateTime.Now.ToString("yyyy.MM.dd-HH-mm-ss");
                string backupFilename = string.Format("settings-{0}.yml", timestamp);
                string backupFilePath = Path.Combine(backupsDirectory, backupFilename);

                // Get path to current settings file
                string currentSettingsPath = Path.Combine(appDirectory, "config.yml"); // Adjust if your main config has different path

                // Copy file
                if (File.Exists(currentSettingsPath))
                {
                    File.Copy(currentSettingsPath, backupFilePath, true);

                    // Inform user
                    MessageBox.Show("Settings backed up successfully!\nLocation:\n" + backupFilePath, "Backup Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Current settings file not found!\nExpected at:\n" + currentSettingsPath, "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during backup:\n" + ex.Message, "Backup Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            /*using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "ini files (*.ini)|*.ini|All files (*.*)|*.*";
                dialog.FileName = "settings.ini";
                dialog.FilterIndex = 2;
                dialog.RestoreDirectory = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string IniPath = new FileInfo(Assembly.GetExecutingAssembly().GetName().Name + ".ini").FullName;
                    // Save data
                    var currentfile = File.ReadAllText(IniPath);
                    System.IO.StreamWriter file = new System.IO.StreamWriter(dialog.FileName.ToString());
                    file.Write(currentfile);
                    file.Close();

                }
            }*/
        }

        private void resetPositionsBtn_Click(object sender, EventArgs e)
        {
            foreach (CameraItem localItem in YmlSettings.Data.Cameras)
            {
                CameraItem item = localItem;
                Camera.ResetCamera(item);
            }
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "Leds")
                {
                    Win32Func.MoveWindow(form.Handle, 0, 0, form.Width, form.Height, true);
                }
                else if (form.Name == "LogForm")
                {
                    Win32Func.MoveWindow(form.Handle, 0, 0, 616, 354, true);
                }

            }
            MessageBox.Show("Resize done!");

        }

        private void SettingsForm_Leave(object sender, EventArgs e)
        {

        }

        // delete
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in camerasListView1.SelectedItems)
            {
                string selectedName = item.Text;
                CameraItem selectedCameraItem = YmlSettings.Data.Cameras.FirstOrDefault(c => c.Name == selectedName);
                if (selectedCameraItem != null)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to remove camera: " + selectedCameraItem.Name + "?",
    "Confirm Removal",
    MessageBoxButtons.YesNo,
    MessageBoxIcon.Warning
);
                    if (result == DialogResult.Yes)
                    {
                        Camera.RemoveCamera(selectedCameraItem);
                        camerasListView1.Items.Remove(item);
                    }
                }

            }
        }

        private void EditCamera()
        {
            if (camerasListView1.SelectedItems.Count > 0)
            {
                var selectedCameraItemName = camerasListView1.SelectedItems[0].Text;
                CameraItem selectedCameraItem = YmlSettings.Data.Cameras.FirstOrDefault(c => c.Name == selectedCameraItemName);
                CameraEditorForm editor = new CameraEditorForm(selectedCameraItem);
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    CameraItem editCamera = editor.CameraItem;
                    Camera.EditCamera(selectedCameraItem, editCamera);
                    camerasListView1.Items.Remove(camerasListView1.SelectedItems[0]);
                    ListViewItem listitem = new ListViewItem(new[] { editCamera.Name, editCamera.Url });
                    camerasListView1.Items.Add(listitem);
                }
            }
            else
            {
                MessageBox.Show("No item selected", "Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // edit
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditCamera();
        }

        // new
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraEditorForm editor = new CameraEditorForm();
            if (editor.ShowDialog() == DialogResult.OK)
            {
                CameraItem newCamera = editor.CameraItem;
                Camera.AddCamera(newCamera);
                ListViewItem listitem = new ListViewItem(new[] { newCamera.Name, newCamera.Url });
                camerasListView1.Items.Add(listitem);
            }
        }

        private void camerasListView1_DoubleClick(object sender, EventArgs e)
        {
            EditCamera();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Classes.TrayIconManager.BalloonTip("title", "text", ToolTipIcon.Info, 3000);
            //NotificationForm.ShowNotification("Motion Detected", "Camera 1 detected movement!", 5000);
            SoundManager.PlaySound("battle1_");

        }
    }
}
