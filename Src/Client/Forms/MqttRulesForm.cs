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
    public partial class MqttRulesForm : Form
    {
        public MqttRulesForm()
        {
            InitializeComponent();
        }

        // cancel button
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        // apply button
        private void button2_Click(object sender, EventArgs e)
        {
            Apply();
        }

        // ok button
        private void button1_Click(object sender, EventArgs e)
        {
            Apply();
            this.Close();
        }

        // new rule
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // delete rule
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in RulesListView.SelectedItems)
            {
                RulesListView.Items.Remove(item);
            }
        }

        // edit rule
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            int index = int.Parse(indexLabel.Text);

            if (index >= 0)
            {
                // update
                var idx = int.Parse(indexLabel.Text);
                RulesListView.Items[idx].SubItems[1].Text = nameBox.Text;
                RulesListView.Items[idx].SubItems[2].Text = topicTextBox.Text;
                RulesListView.Items[idx].SubItems[3].Text = typeTextBox.SelectedIndex.ToString();
                RulesListView.Items[idx].SubItems[4].Text = valueTextBox.Text;
                RulesListView.Items[idx].SubItems[5].Text = value2TextBox.Text;
                RulesListView.Items[idx].SubItems[6].Text = actionBox.SelectedIndex.ToString();

            }
            if (index == -1)
            {
                // insert new
                var itemsCount = RulesListView.Items.Count + 1;
                if (topicTextBox.Text != "" && typeTextBox.Text != "" && Value1Column.Text != "" && Value2Column.Text != "")
                {
                    ListViewItem item = new ListViewItem(new[] { itemsCount.ToString(), nameBox.Text ,topicTextBox.Text, typeTextBox.SelectedIndex.ToString(), valueTextBox.Text, value2TextBox.Text,actionBox.SelectedIndex.ToString() });
                    RulesListView.Items.Add(item);
                }
                else
                {
                    MessageBox.Show("Not all fields are filled!");
                }
            }
        }

        private void Apply()
        {
            var Rules = YmlSettings.Data.MQTTRules;
            YmlSettings.Data.MQTTRules.Clear();
            //Array.Clear(YmlSettings.Data.MQTTRules, 0, YmlSettings.Data.MQTTRules.Count);
            var cnt = 0;
            foreach (ListViewItem listItem in RulesListView.Items)
            {
                var rule = Mqtt.NewMqttRule(listItem.Text, listItem.SubItems[1].Text, int.Parse(listItem.SubItems[2].Text), listItem.SubItems[3].Text, listItem.SubItems[5].Text, int.Parse(listItem.SubItems[5].Text));
                Rules[cnt] = rule;
                cnt++;
            }
            YmlSettings.Data.MQTTRules = Rules;
        }

        private void MqttRules_Load(object sender, EventArgs e)
        {
            indexLabel.Text = "-1";
            foreach (MqttRulesDefinition rule in YmlSettings.Data.MQTTRules)
            {
                if (rule != null)
                {
                    var testlog = String.Format("Loading MQTT rule record: ID: {0} ", rule.Topic);
                    Logger.WriteLog(testlog);
                    ListViewItem item = new ListViewItem(new[] { rule.Name, rule.Topic, rule.Type.ToString(), rule.Value1, rule.Value2, rule.Action.ToString() });
                    RulesListView.Items.Add(item);
                }
            }
        }

        private void RulesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RulesListView.SelectedItems.Count > 0)
            {
                var item = RulesListView.SelectedItems[0];
                indexLabel.Text = item.Index.ToString();
                nameBox.Text = item.SubItems[1].Text;
                topicTextBox.Text = item.SubItems[2].Text;
                typeTextBox.SelectedIndex = int.Parse(item.SubItems[3].Text);
                valueTextBox.Text = item.SubItems[4].Text;
                value2TextBox.Text = item.SubItems[5].Text;
                actionBox.SelectedIndex = int.Parse(item.SubItems[6].Text);
            }
        }

        private void newbtn_Click(object sender, EventArgs e)
        {
            indexLabel.Text = "-1";
            nameBox.Text = "";
            topicTextBox.Text = "";
            typeTextBox.SelectedIndex = 0;
            valueTextBox.Text = "";
            value2TextBox.Text = "";
            actionBox.SelectedIndex = 0;
        }
    }
}
