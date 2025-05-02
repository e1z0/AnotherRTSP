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
    



    public partial class ScriptEditorForm : Form
    {
        private string editingName;

        public ScriptEditorForm(string name = null)
        {
            InitializeComponent();
            editingName = name;
            if (editingName != null)
            {
                var script = Scripting.LuaManager.Scripts.Find(s => s.Name == editingName);
                if (script != null)
                {
                    txtCode.Text = Utils.NormalizeLineEndings(script.Source);
                    descrBox.Text = script.Description;
                    RunOnStartupBox.Checked = script.RunOnStartup;
                }

            }
        }

        private void ScriptEditorForm_Load(object sender, EventArgs e)
        {

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (editingName == null)
            {
                string name = Prompt.ShowDialog("Enter script name:", "New Script");
                if (!string.IsNullOrWhiteSpace(name))
                    Scripting.LuaManager.AddOrUpdateScript(name, txtCode.Text,descrBox.Text,RunOnStartupBox.Checked);
            }
            else
            {
                Scripting.LuaManager.AddOrUpdateScript(editingName, txtCode.Text,descrBox.Text,RunOnStartupBox.Checked);
            }
            this.Close();
        }
    }
    public static class Prompt
    {


        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 10, Top = 20, Text = text, Width = 360 };
            TextBox inputBox = new TextBox() { Left = 10, Top = 50, Width = 360 };
            Button confirmation = new Button() { Text = "OK", Left = 300, Width = 70, Top = 80, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : "";
        }
    }
}
