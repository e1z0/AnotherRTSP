namespace AnotherRTSP.Forms
{
    partial class ScriptEditorForm
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
            this.txtCode = new System.Windows.Forms.TextBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.descrBox = new System.Windows.Forms.TextBox();
            this.RunOnStartupBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("Lucida Console", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Location = new System.Drawing.Point(-2, -2);
            this.txtCode.Multiline = true;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(748, 474);
            this.txtCode.TabIndex = 0;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(658, 478);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 1;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 483);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Description:";
            // 
            // descrBox
            // 
            this.descrBox.Location = new System.Drawing.Point(72, 480);
            this.descrBox.Name = "descrBox";
            this.descrBox.Size = new System.Drawing.Size(282, 20);
            this.descrBox.TabIndex = 3;
            // 
            // RunOnStartupBox
            // 
            this.RunOnStartupBox.AutoSize = true;
            this.RunOnStartupBox.Location = new System.Drawing.Point(360, 483);
            this.RunOnStartupBox.Name = "RunOnStartupBox";
            this.RunOnStartupBox.Size = new System.Drawing.Size(100, 17);
            this.RunOnStartupBox.TabIndex = 5;
            this.RunOnStartupBox.Text = "Run On Startup";
            this.RunOnStartupBox.UseVisualStyleBackColor = true;
            // 
            // ScriptEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 510);
            this.Controls.Add(this.RunOnStartupBox);
            this.Controls.Add(this.descrBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.txtCode);
            this.Name = "ScriptEditorForm";
            this.Text = "Script Editor";
            this.Load += new System.EventHandler(this.ScriptEditorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox descrBox;
        private System.Windows.Forms.CheckBox RunOnStartupBox;
    }
}