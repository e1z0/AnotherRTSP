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

namespace AnotherRTSP.Forms
{
    public partial class CameraEditorForm : Form
    {
        public CameraItem CameraItem { get; private set; }

        public CameraEditorForm(CameraItem existing = null)
        {
            InitializeComponent();
            if (existing != null)
            {
                camName.Text = existing.Name;
                camURL.Text = existing.Url;
                camWidth.Value = existing.WWidth;
                camHeight.Value = existing.WHeight;
                camX.Value = existing.WX;
                camY.Value = existing.WY;
                isDisabled.Checked = existing.Disabled;
                isTCP.Checked = existing.isTCP;
                isHardDecode.Checked = existing.HardDecode;
                CameraItem = existing;
            }
            else
            {
                CameraItem = new CameraItem();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CameraItem.Name = camName.Text.Trim();
            CameraItem.Url = camURL.Text.Trim();
            CameraItem.WWidth = (int)camWidth.Value;
            CameraItem.WHeight = (int)camHeight.Value;
            CameraItem.WX = (int)camX.Value;

            CameraItem.WY = (int)camY.Value;

            CameraItem.Disabled = isDisabled.Checked;
            CameraItem.isTCP = isTCP.Checked;
            CameraItem.HardDecode = isHardDecode.Checked;

            if (string.IsNullOrEmpty(CameraItem.Name) || string.IsNullOrEmpty(CameraItem.Url))
            {
                MessageBox.Show("Please fill in Name and URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
