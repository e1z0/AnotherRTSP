using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnotherRTSP.Classes;
using System.Windows.Forms;
using System.Drawing;

namespace AnotherRTSP.Forms
{
    public partial class CameraEditorForm : Form
    {
        public CameraItem CameraItem { get; private set; }

        private TextBox textBoxName;
        private TextBox textBoxUrl;
        private NumericUpDown numericUpDownWidth;
        private NumericUpDown numericUpDownHeight;
        private NumericUpDown numericUpDownX;
        private NumericUpDown numericUpDownY;
        private CheckBox checkBoxDisabled;
        private Button buttonOK;
        private Button buttonCancel;

        private void InitializeComponent()
        {
            this.textBoxName = new TextBox();
            this.textBoxUrl = new TextBox();
            this.numericUpDownWidth = new NumericUpDown();
            this.numericUpDownHeight = new NumericUpDown();
            this.numericUpDownX = new NumericUpDown();
            this.numericUpDownY = new NumericUpDown();
            this.checkBoxDisabled = new CheckBox();
            this.buttonOK = new Button();
            this.buttonCancel = new Button();
            this.SuspendLayout();
            //
            // textBoxName
            //
            this.textBoxName.Location = new Point(12, 12);
            this.textBoxName.Size = new Size(260, 20);
            //this.textBoxName.PlaceholderText = "Camera Name";
            //
            // textBoxUrl
            //
            this.textBoxUrl.Location = new Point(12, 38);
            this.textBoxUrl.Size = new Size(260, 20);
            //this.textBoxUrl.PlaceholderText = "Camera URL";
            //
            // numericUpDownWidth
            //
            this.numericUpDownWidth.Location = new Point(12, 64);
            this.numericUpDownWidth.Maximum = 10000;
            this.numericUpDownWidth.Minimum = 1;
            this.numericUpDownWidth.Value = 300;
            //
            // numericUpDownHeight
            //
            this.numericUpDownHeight.Location = new Point(140, 64);
            this.numericUpDownHeight.Maximum = 10000;
            this.numericUpDownHeight.Minimum = 1;
            this.numericUpDownHeight.Value = 200;
            //
            // numericUpDownX
            //
            this.numericUpDownX.Location = new Point(12, 90);
            this.numericUpDownX.Maximum = 10000;
            this.numericUpDownX.Minimum = 0;
            //
            // numericUpDownY
            //
            this.numericUpDownY.Location = new Point(140, 90);
            this.numericUpDownY.Maximum = 10000;
            this.numericUpDownY.Minimum = 0;
            //
            // checkBoxDisabled
            //
            this.checkBoxDisabled.Location = new Point(12, 120);
            this.checkBoxDisabled.Size = new Size(200, 20);
            this.checkBoxDisabled.Text = "Disabled";
            //
            // buttonOK
            //
            this.buttonOK.Location = new Point(50, 150);
            this.buttonOK.Size = new Size(75, 23);
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
            //
            // buttonCancel
            //
            this.buttonCancel.Location = new Point(150, 150);
            this.buttonCancel.Size = new Size(75, 23);
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
            //
            // CameraEditorForm
            //
            this.ClientSize = new Size(284, 190);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.textBoxUrl);
            this.Controls.Add(this.numericUpDownWidth);
            this.Controls.Add(this.numericUpDownHeight);
            this.Controls.Add(this.numericUpDownX);
            this.Controls.Add(this.numericUpDownY);
            this.Controls.Add(this.checkBoxDisabled);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Camera Editor";
            this.ResumeLayout(false);
            this.PerformLayout();
        }


        public CameraEditorForm(CameraItem existing = null)
        {
            InitializeComponent();

            if (existing != null)
            {
                textBoxName.Text = existing.Name;
                textBoxUrl.Text = existing.Url;
                numericUpDownWidth.Value = existing.WWidth;
                numericUpDownHeight.Value = existing.WHeight;
                numericUpDownX.Value = existing.WX;
                numericUpDownY.Value = existing.WY;
                checkBoxDisabled.Checked = existing.Disabled;
                CameraItem = existing;
            }
            else
            {
                CameraItem = new CameraItem();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            CameraItem.Name = textBoxName.Text.Trim();
            CameraItem.Url = textBoxUrl.Text.Trim();
            CameraItem.WWidth = (int)numericUpDownWidth.Value;
            CameraItem.WHeight = (int)numericUpDownHeight.Value;
            CameraItem.WX = (int)numericUpDownX.Value;
            CameraItem.WY = (int)numericUpDownY.Value;
            CameraItem.Disabled = checkBoxDisabled.Checked;

            if (string.IsNullOrEmpty(CameraItem.Name) || string.IsNullOrEmpty(CameraItem.Url))
            {
                MessageBox.Show("Please fill in Name and URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
