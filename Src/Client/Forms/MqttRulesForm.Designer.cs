namespace AnotherRTSP.Forms
{
    partial class MqttRulesForm
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
            this.okbtn = new System.Windows.Forms.Button();
            this.applybtn = new System.Windows.Forms.Button();
            this.cancelbtn = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RulesListView = new System.Windows.Forms.ListView();
            this.IdColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TopicColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TypeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Value1Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Value2Column = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.actionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.actionBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.newbtn = new System.Windows.Forms.Button();
            this.indexLabel = new System.Windows.Forms.Label();
            this.savebtn = new System.Windows.Forms.Button();
            this.typeTextBox = new System.Windows.Forms.ComboBox();
            this.value2TextBox = new System.Windows.Forms.TextBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.topicTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // okbtn
            // 
            this.okbtn.Location = new System.Drawing.Point(1, 370);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(75, 23);
            this.okbtn.TabIndex = 1;
            this.okbtn.Text = "OK";
            this.okbtn.UseVisualStyleBackColor = true;
            this.okbtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // applybtn
            // 
            this.applybtn.Location = new System.Drawing.Point(423, 370);
            this.applybtn.Name = "applybtn";
            this.applybtn.Size = new System.Drawing.Size(75, 23);
            this.applybtn.TabIndex = 2;
            this.applybtn.Text = "Apply";
            this.applybtn.UseVisualStyleBackColor = true;
            this.applybtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // cancelbtn
            // 
            this.cancelbtn.Location = new System.Drawing.Point(217, 370);
            this.cancelbtn.Name = "cancelbtn";
            this.cancelbtn.Size = new System.Drawing.Size(75, 23);
            this.cancelbtn.TabIndex = 3;
            this.cancelbtn.Text = "Cancel";
            this.cancelbtn.UseVisualStyleBackColor = true;
            this.cancelbtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(108, 26);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // RulesListView
            // 
            this.RulesListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.IdColumn,
            this.NameColumn,
            this.TopicColumn,
            this.TypeColumn,
            this.Value1Column,
            this.Value2Column,
            this.actionColumn});
            this.RulesListView.ContextMenuStrip = this.contextMenuStrip1;
            this.RulesListView.FullRowSelect = true;
            this.RulesListView.GridLines = true;
            this.RulesListView.Location = new System.Drawing.Point(1, 2);
            this.RulesListView.MultiSelect = false;
            this.RulesListView.Name = "RulesListView";
            this.RulesListView.Size = new System.Drawing.Size(497, 198);
            this.RulesListView.TabIndex = 5;
            this.RulesListView.UseCompatibleStateImageBehavior = false;
            this.RulesListView.View = System.Windows.Forms.View.Details;
            this.RulesListView.SelectedIndexChanged += new System.EventHandler(this.RulesListView_SelectedIndexChanged);
            // 
            // IdColumn
            // 
            this.IdColumn.Text = "#";
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "Name";
            // 
            // TopicColumn
            // 
            this.TopicColumn.Text = "Topic";
            this.TopicColumn.Width = 171;
            // 
            // TypeColumn
            // 
            this.TypeColumn.Text = "Type";
            this.TypeColumn.Width = 46;
            // 
            // Value1Column
            // 
            this.Value1Column.Text = "Value1";
            this.Value1Column.Width = 70;
            // 
            // Value2Column
            // 
            this.Value2Column.Text = "Value2";
            this.Value2Column.Width = 103;
            // 
            // actionColumn
            // 
            this.actionColumn.Text = "Action";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nameBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.actionBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.newbtn);
            this.groupBox1.Controls.Add(this.indexLabel);
            this.groupBox1.Controls.Add(this.savebtn);
            this.groupBox1.Controls.Add(this.typeTextBox);
            this.groupBox1.Controls.Add(this.value2TextBox);
            this.groupBox1.Controls.Add(this.valueTextBox);
            this.groupBox1.Controls.Add(this.topicTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(1, 206);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(497, 158);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rules";
            // 
            // nameBox
            // 
            this.nameBox.Location = new System.Drawing.Point(49, 13);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(100, 20);
            this.nameBox.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Name:";
            // 
            // actionBox
            // 
            this.actionBox.FormattingEnabled = true;
            this.actionBox.Items.AddRange(new object[] {
            "Log message",
            "Toast message",
            "Led behaviour"});
            this.actionBox.Location = new System.Drawing.Point(49, 126);
            this.actionBox.Name = "actionBox";
            this.actionBox.Size = new System.Drawing.Size(143, 21);
            this.actionBox.TabIndex = 12;
            this.actionBox.Text = "Log message";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Action:";
            // 
            // newbtn
            // 
            this.newbtn.Location = new System.Drawing.Point(263, 129);
            this.newbtn.Name = "newbtn";
            this.newbtn.Size = new System.Drawing.Size(75, 23);
            this.newbtn.TabIndex = 10;
            this.newbtn.Text = "New";
            this.newbtn.UseVisualStyleBackColor = true;
            this.newbtn.Click += new System.EventHandler(this.newbtn_Click);
            // 
            // indexLabel
            // 
            this.indexLabel.AutoSize = true;
            this.indexLabel.Location = new System.Drawing.Point(198, 129);
            this.indexLabel.Name = "indexLabel";
            this.indexLabel.Size = new System.Drawing.Size(59, 13);
            this.indexLabel.TabIndex = 9;
            this.indexLabel.Text = "IndexLabel";
            this.indexLabel.Visible = false;
            // 
            // savebtn
            // 
            this.savebtn.Location = new System.Drawing.Point(344, 129);
            this.savebtn.Name = "savebtn";
            this.savebtn.Size = new System.Drawing.Size(75, 23);
            this.savebtn.TabIndex = 8;
            this.savebtn.Text = "Add/Update";
            this.savebtn.UseVisualStyleBackColor = true;
            this.savebtn.Click += new System.EventHandler(this.savebtn_Click);
            // 
            // typeTextBox
            // 
            this.typeTextBox.FormattingEnabled = true;
            this.typeTextBox.Items.AddRange(new object[] {
            "Raw",
            "Json"});
            this.typeTextBox.Location = new System.Drawing.Point(49, 57);
            this.typeTextBox.Name = "typeTextBox";
            this.typeTextBox.Size = new System.Drawing.Size(185, 21);
            this.typeTextBox.TabIndex = 7;
            this.typeTextBox.Text = "Raw";
            // 
            // value2TextBox
            // 
            this.value2TextBox.Location = new System.Drawing.Point(49, 104);
            this.value2TextBox.Name = "value2TextBox";
            this.value2TextBox.Size = new System.Drawing.Size(370, 20);
            this.value2TextBox.TabIndex = 6;
            // 
            // valueTextBox
            // 
            this.valueTextBox.Location = new System.Drawing.Point(49, 81);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(370, 20);
            this.valueTextBox.TabIndex = 5;
            // 
            // topicTextBox
            // 
            this.topicTextBox.Location = new System.Drawing.Point(49, 36);
            this.topicTextBox.Name = "topicTextBox";
            this.topicTextBox.Size = new System.Drawing.Size(370, 20);
            this.topicTextBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Value2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Value:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Type:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Topic:";
            // 
            // MqttRulesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 398);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.RulesListView);
            this.Controls.Add(this.cancelbtn);
            this.Controls.Add(this.applybtn);
            this.Controls.Add(this.okbtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MqttRulesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MqttRules";
            this.Load += new System.EventHandler(this.MqttRules_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button okbtn;
        private System.Windows.Forms.Button applybtn;
        private System.Windows.Forms.Button cancelbtn;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ListView RulesListView;
        private System.Windows.Forms.ColumnHeader TopicColumn;
        private System.Windows.Forms.ColumnHeader TypeColumn;
        private System.Windows.Forms.ColumnHeader Value1Column;
        private System.Windows.Forms.ColumnHeader Value2Column;
        private System.Windows.Forms.ColumnHeader IdColumn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button savebtn;
        private System.Windows.Forms.ComboBox typeTextBox;
        private System.Windows.Forms.TextBox value2TextBox;
        private System.Windows.Forms.TextBox valueTextBox;
        private System.Windows.Forms.TextBox topicTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label indexLabel;
        private System.Windows.Forms.Button newbtn;
        private System.Windows.Forms.ComboBox actionBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader actionColumn;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label label6;
    }
}