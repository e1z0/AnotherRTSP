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
            // general settings
            Settings.Logging = LoggingcheckBox1.Checked ? 1 : 0;
            Settings.CustomLayout = customUIcheckBox1.Checked ? 1 : 0;

            // mqtt section
            Settings.MqttEnabled = mqttsupport_checkbox.Checked ? 1 : 0;
            if (servertextbox.Text != "")
                Settings.MqttSettings.Server = servertextbox.Text;
            if (porttextbox.Text != "")
                Settings.MqttSettings.Port = int.Parse(porttextbox.Text);
            if (usernametextbox.Text != "")
                Settings.MqttSettings.Username = usernametextbox.Text;
            if (passwordtextbox.Text != "")
                Settings.MqttSettings.Password = passwordtextbox.Text;
            if (clientidtextbox.Text != "")
                Settings.MqttSettings.ClientID = clientidtextbox.Text;

            // cameras section
            foreach (ListViewItem listItem in camerasListView1.Items)
            {
                string name = listItem.Text;
                int width = 240;
                int height = 180;
                int posx = 0;
                int posy = 0;
                string url = listItem.SubItems[1].Text;
                bool disabled = false;
                foreach (Camera cam in Settings.Cameras)
                {
                    if (cam.Name == name)
                    {
                        cam.UpdateWindowSpecs();
                        width = cam.WWidth;
                        height = cam.WHeight;
                        posx = cam.WX;
                        posy = cam.WY;
                        disabled = cam.Disabled;
                    }
                }
                Camera newCamera = new Camera(name,width,height,posx,posy,url,disabled);
                Settings.Cameras.Add(newCamera);
            }
            Settings.OverrideCamsList();
            // advanced tab
            Settings.Advanced.LedsWindowOnTop = checkBoxLedsOnTop.Checked;
            Settings.Advanced.LedsSoundAlert = checkBoxLedsAlertSounds.Checked;
            Settings.Advanced.FocusAllWindowsOnClick = allWindowsFocus.Checked;
            Settings.Advanced.StaticCameraCaption = staticCameraCaptioncheckBox1.Checked;
            Settings.Advanced.DisableCameraCaptions = disableCameraCaptionCheckbox.Checked;
            Settings.Advanced.AllCamerasWindowsOnTop = allCamerasOntopCheckbox.Checked;
        }

        private void loadSettings()
        {
            // general settings
            LoggingcheckBox1.Checked = Settings.Logging == 1;
            customUIcheckBox1.Checked = Settings.CustomLayout == 1;

            // mqtt section
            mqttsupport_checkbox.Checked = Settings.MqttEnabled == 1;
            servertextbox.Text = Settings.MqttSettings.Server;
            porttextbox.Text = Settings.MqttSettings.Port.ToString();
            usernametextbox.Text = Settings.MqttSettings.Username;
            passwordtextbox.Text = Settings.MqttSettings.Password;
            clientidtextbox.Text = Settings.MqttSettings.ClientID;


            // load cameras
            foreach (Camera cam in Settings.Cameras)
            {
                ListViewItem item = new ListViewItem(new[] { cam.Name, cam.Url });
                camerasListView1.Items.Add(item);
            }
            // advanced tab
            checkBoxLedsOnTop.Checked = Settings.Advanced.LedsWindowOnTop;
            checkBoxLedsAlertSounds.Checked = Settings.Advanced.LedsSoundAlert;
            allWindowsFocus.Checked = Settings.Advanced.FocusAllWindowsOnClick;
            staticCameraCaptioncheckBox1.Checked = Settings.Advanced.StaticCameraCaption;
            disableCameraCaptionCheckbox.Checked = Settings.Advanced.DisableCameraCaptions;
            allCamerasOntopCheckbox.Checked = Settings.Advanced.AllCamerasWindowsOnTop;
        }
        /*
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Form logfrm = new LogForm();
            if (checkBox1.Checked)
            {
                // Show the form if the CheckBox is checked
                Settings.ShowOrActivateForm<LogForm>();
            }
            else
            {
                // Hide the form if the CheckBox is not checked
                Settings.DeactivateForm<LogForm>();
            }
        }
         */

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
            if (camerasListView1.SelectedItems.Count > 0)
            {
                var item = camerasListView1.SelectedItems[0];
                cameraNameEditBox.Text = item.SubItems[0].Text;
                cameraSourceEditBox.Text = item.SubItems[1].Text;
                selectedCameraLabel.Text = item.Index.ToString();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            selectedCameraLabel.Text = "-1";
            cameraSourceEditBox.Text = "";
            cameraNameEditBox.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int index = int.Parse(selectedCameraLabel.Text);

            if (index >= 0)
            {
                // update
                camerasListView1.Items[index].SubItems[0].Text = cameraNameEditBox.Text;
                camerasListView1.Items[index].SubItems[1].Text = cameraSourceEditBox.Text;
            }
            if (index == -1)
            {
                // insert new
                if (cameraSourceEditBox.Text != "" && cameraNameEditBox.Text != "")
                {
                    ListViewItem item = new ListViewItem(new[] { cameraNameEditBox.Text, cameraSourceEditBox.Text });
                    camerasListView1.Items.Add(item);
                }
                else
                {
                    MessageBox.Show("Not all fields are filled!");
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in camerasListView1.SelectedItems)
            {
                camerasListView1.Items.Remove(item);
            }
        }

        private void backupSettingsbutton4_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
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
            }
        }

        private void resetPositionsBtn_Click(object sender, EventArgs e)
        {
            Settings.LedWindowX = 0;
            Settings.LedWindowY = 0;
            Settings.LogWindowX = 0;
            Settings.LogWindowY = 0;
            foreach (Camera cam in Settings.Cameras)
            {
                cam.SetFormSize(240, 180);
                cam.SetFormLocation(0, 0);
            }
            foreach (Form form in Application.OpenForms)
            {
                if (form.Name == "Leds")
                {
                    Win32Func.MoveWindow(form.Handle, 0, 0, form.Width, form.Height, true);
                }
                else if (form.Name == "LogForm")
                {
                    Win32Func.MoveWindow(form.Handle,0,0,616,354,true);
                }
                
            }
            MessageBox.Show("Resize done!");
        }
    }
}
