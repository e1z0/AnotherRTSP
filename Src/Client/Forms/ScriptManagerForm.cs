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

namespace AnotherRTSP.Forms
{
    public partial class ScriptManagerForm : Form
    {
        public ScriptManagerForm()
        {
            InitializeComponent();
        }

        private void RefreshList()
        {
            listScripts.Items.Clear();
            foreach (var script in Scripting.LuaManager.Scripts)
            {
                string qamark = script.RunOnStartup ? "Yes" : "No";
                ListViewItem listitem = new ListViewItem(new[] { script.Name, script.Description, qamark });
                listScripts.Items.Add(listitem);
            }
        }


        private void EditSelected()
        {
            if (listScripts.SelectedItems != null)
            {
                var name = listScripts.SelectedItems[0].Text;
                var editor = new ScriptEditorForm(name);
                editor.ShowDialog();
                RefreshList();
            }
        }

        // add new
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editor = new ScriptEditorForm();
            editor.ShowDialog();
            RefreshList();
        }

        // refresh
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        // edit
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSelected();
        }

        // list double click
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            EditSelected();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listScripts.SelectedItems != null)
            {
                var name = listScripts.SelectedItems[0].Text;
                Scripting.LuaManager.RemoveScript(name);
                RefreshList();
            }
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listScripts.SelectedItems != null)
            {
                var name = listScripts.SelectedItems[0].Text;
                Scripting.LuaManager.RunScript(name);
            }
        }

        private void ScriptManagerForm_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void terminateScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scripting.LuaManager.Shutdown();
        }
    }
}
