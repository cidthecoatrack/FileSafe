namespace FileSafe
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.BrowseDirectoryButton = new System.Windows.Forms.Button();
            this.BackUpButton = new System.Windows.Forms.Button();
            this.DirChecklist = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BackUpDirectoryText = new System.Windows.Forms.TextBox();
            this.NewDirButton = new System.Windows.Forms.Button();
            this.DelDirButton = new System.Windows.Forms.Button();
            this.RestoreButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ExceptChecklist = new System.Windows.Forms.CheckedListBox();
            this.ExceptDeleteButton = new System.Windows.Forms.Button();
            this.ExceptAddButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.TaskLabel = new System.Windows.Forms.Label();
            this.OperationsGroup = new System.Windows.Forms.GroupBox();
            this.ElapsedTimeLabel = new System.Windows.Forms.Label();
            this.AbortButton = new System.Windows.Forms.Button();
            this.SettingsGroup = new System.Windows.Forms.GroupBox();
            this.OptionsGroup = new System.Windows.Forms.GroupBox();
            this.AllExcpLoadCheckBox = new System.Windows.Forms.CheckBox();
            this.AllDirLoadCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.OperationsGroup.SuspendLayout();
            this.SettingsGroup.SuspendLayout();
            this.OptionsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // BrowseDirectoryButton
            // 
            this.BrowseDirectoryButton.Location = new System.Drawing.Point(453, 143);
            this.BrowseDirectoryButton.Name = "BrowseDirectoryButton";
            this.BrowseDirectoryButton.Size = new System.Drawing.Size(57, 26);
            this.BrowseDirectoryButton.TabIndex = 2;
            this.BrowseDirectoryButton.Text = "Browse";
            this.BrowseDirectoryButton.UseVisualStyleBackColor = true;
            this.BrowseDirectoryButton.Click += new System.EventHandler(this.BrowseDirectoryButton_Click);
            // 
            // BackUpButton
            // 
            this.BackUpButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackUpButton.Location = new System.Drawing.Point(550, 86);
            this.BackUpButton.Name = "BackUpButton";
            this.BackUpButton.Size = new System.Drawing.Size(209, 119);
            this.BackUpButton.TabIndex = 5;
            this.BackUpButton.Text = "Start Back-Up";
            this.BackUpButton.UseVisualStyleBackColor = true;
            this.BackUpButton.Click += new System.EventHandler(this.BackUpButton_Click);
            // 
            // DirChecklist
            // 
            this.DirChecklist.CheckOnClick = true;
            this.DirChecklist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DirChecklist.FormattingEnabled = true;
            this.DirChecklist.HorizontalScrollbar = true;
            this.DirChecklist.Location = new System.Drawing.Point(69, 18);
            this.DirChecklist.Name = "DirChecklist";
            this.DirChecklist.Size = new System.Drawing.Size(426, 94);
            this.DirChecklist.Sorted = true;
            this.DirChecklist.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Back-Up Directory:";
            // 
            // BackUpDirectoryText
            // 
            this.BackUpDirectoryText.Location = new System.Drawing.Point(109, 147);
            this.BackUpDirectoryText.Name = "BackUpDirectoryText";
            this.BackUpDirectoryText.ReadOnly = true;
            this.BackUpDirectoryText.Size = new System.Drawing.Size(338, 20);
            this.BackUpDirectoryText.TabIndex = 9;
            // 
            // NewDirButton
            // 
            this.NewDirButton.Location = new System.Drawing.Point(6, 19);
            this.NewDirButton.Name = "NewDirButton";
            this.NewDirButton.Size = new System.Drawing.Size(57, 40);
            this.NewDirButton.TabIndex = 10;
            this.NewDirButton.Text = "Add";
            this.NewDirButton.UseVisualStyleBackColor = true;
            this.NewDirButton.Click += new System.EventHandler(this.NewDirButton_Click);
            // 
            // DelDirButton
            // 
            this.DelDirButton.Location = new System.Drawing.Point(6, 65);
            this.DelDirButton.Name = "DelDirButton";
            this.DelDirButton.Size = new System.Drawing.Size(57, 47);
            this.DelDirButton.TabIndex = 11;
            this.DelDirButton.Text = "Delete";
            this.DelDirButton.UseVisualStyleBackColor = true;
            this.DelDirButton.Click += new System.EventHandler(this.DelDirButton_Click);
            // 
            // RestoreButton
            // 
            this.RestoreButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RestoreButton.Location = new System.Drawing.Point(550, 211);
            this.RestoreButton.Name = "RestoreButton";
            this.RestoreButton.Size = new System.Drawing.Size(209, 98);
            this.RestoreButton.TabIndex = 12;
            this.RestoreButton.Text = "Restore Files";
            this.RestoreButton.UseVisualStyleBackColor = true;
            this.RestoreButton.Click += new System.EventHandler(this.RestoreButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.NewDirButton);
            this.groupBox1.Controls.Add(this.DelDirButton);
            this.groupBox1.Controls.Add(this.DirChecklist);
            this.groupBox1.Location = new System.Drawing.Point(9, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(501, 118);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Directories:";
            // 
            // ExceptChecklist
            // 
            this.ExceptChecklist.CheckOnClick = true;
            this.ExceptChecklist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExceptChecklist.FormattingEnabled = true;
            this.ExceptChecklist.HorizontalScrollbar = true;
            this.ExceptChecklist.Location = new System.Drawing.Point(69, 19);
            this.ExceptChecklist.Name = "ExceptChecklist";
            this.ExceptChecklist.Size = new System.Drawing.Size(426, 94);
            this.ExceptChecklist.Sorted = true;
            this.ExceptChecklist.TabIndex = 12;
            // 
            // ExceptDeleteButton
            // 
            this.ExceptDeleteButton.Location = new System.Drawing.Point(6, 72);
            this.ExceptDeleteButton.Name = "ExceptDeleteButton";
            this.ExceptDeleteButton.Size = new System.Drawing.Size(57, 41);
            this.ExceptDeleteButton.TabIndex = 15;
            this.ExceptDeleteButton.Text = "Delete";
            this.ExceptDeleteButton.UseVisualStyleBackColor = true;
            this.ExceptDeleteButton.Click += new System.EventHandler(this.ExceptDeleteButton_Click);
            // 
            // ExceptAddButton
            // 
            this.ExceptAddButton.Location = new System.Drawing.Point(6, 19);
            this.ExceptAddButton.Name = "ExceptAddButton";
            this.ExceptAddButton.Size = new System.Drawing.Size(57, 47);
            this.ExceptAddButton.TabIndex = 12;
            this.ExceptAddButton.Text = "Add";
            this.ExceptAddButton.UseVisualStyleBackColor = true;
            this.ExceptAddButton.Click += new System.EventHandler(this.ExceptAddButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ExceptChecklist);
            this.groupBox2.Controls.Add(this.ExceptAddButton);
            this.groupBox2.Controls.Add(this.ExceptDeleteButton);
            this.groupBox2.Location = new System.Drawing.Point(9, 166);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(501, 125);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Exceptions";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(6, 45);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(658, 32);
            this.progressBar.TabIndex = 17;
            // 
            // TaskLabel
            // 
            this.TaskLabel.AutoSize = true;
            this.TaskLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaskLabel.Location = new System.Drawing.Point(6, 29);
            this.TaskLabel.Name = "TaskLabel";
            this.TaskLabel.Size = new System.Drawing.Size(80, 13);
            this.TaskLabel.TabIndex = 18;
            this.TaskLabel.Tag = "";
            this.TaskLabel.Text = "(Current Action)";
            // 
            // OperationsGroup
            // 
            this.OperationsGroup.Controls.Add(this.ElapsedTimeLabel);
            this.OperationsGroup.Controls.Add(this.AbortButton);
            this.OperationsGroup.Controls.Add(this.progressBar);
            this.OperationsGroup.Controls.Add(this.TaskLabel);
            this.OperationsGroup.Enabled = false;
            this.OperationsGroup.Location = new System.Drawing.Point(12, 315);
            this.OperationsGroup.Name = "OperationsGroup";
            this.OperationsGroup.Size = new System.Drawing.Size(747, 86);
            this.OperationsGroup.TabIndex = 19;
            this.OperationsGroup.TabStop = false;
            this.OperationsGroup.Text = "Operations";
            // 
            // ElapsedTimeLabel
            // 
            this.ElapsedTimeLabel.AutoSize = true;
            this.ElapsedTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ElapsedTimeLabel.Location = new System.Drawing.Point(6, 16);
            this.ElapsedTimeLabel.Name = "ElapsedTimeLabel";
            this.ElapsedTimeLabel.Size = new System.Drawing.Size(77, 13);
            this.ElapsedTimeLabel.TabIndex = 19;
            this.ElapsedTimeLabel.Tag = "";
            this.ElapsedTimeLabel.Text = "(Elapsed Time)";
            // 
            // AbortButton
            // 
            this.AbortButton.Location = new System.Drawing.Point(670, 45);
            this.AbortButton.Name = "AbortButton";
            this.AbortButton.Size = new System.Drawing.Size(72, 32);
            this.AbortButton.TabIndex = 16;
            this.AbortButton.Text = "Cancel";
            this.AbortButton.UseVisualStyleBackColor = true;
            this.AbortButton.Click += new System.EventHandler(this.AbortButton_Click);
            // 
            // SettingsGroup
            // 
            this.SettingsGroup.Controls.Add(this.BrowseDirectoryButton);
            this.SettingsGroup.Controls.Add(this.groupBox1);
            this.SettingsGroup.Controls.Add(this.groupBox2);
            this.SettingsGroup.Controls.Add(this.BackUpDirectoryText);
            this.SettingsGroup.Controls.Add(this.label2);
            this.SettingsGroup.Location = new System.Drawing.Point(12, 12);
            this.SettingsGroup.Name = "SettingsGroup";
            this.SettingsGroup.Size = new System.Drawing.Size(522, 297);
            this.SettingsGroup.TabIndex = 20;
            this.SettingsGroup.TabStop = false;
            this.SettingsGroup.Text = "Settings";
            // 
            // OptionsGroup
            // 
            this.OptionsGroup.Controls.Add(this.AllExcpLoadCheckBox);
            this.OptionsGroup.Controls.Add(this.AllDirLoadCheckBox);
            this.OptionsGroup.Location = new System.Drawing.Point(550, 12);
            this.OptionsGroup.Name = "OptionsGroup";
            this.OptionsGroup.Size = new System.Drawing.Size(209, 68);
            this.OptionsGroup.TabIndex = 20;
            this.OptionsGroup.TabStop = false;
            this.OptionsGroup.Text = "Options";
            // 
            // AllExcpLoadCheckBox
            // 
            this.AllExcpLoadCheckBox.AutoSize = true;
            this.AllExcpLoadCheckBox.Location = new System.Drawing.Point(6, 42);
            this.AllExcpLoadCheckBox.Name = "AllExcpLoadCheckBox";
            this.AllExcpLoadCheckBox.Size = new System.Drawing.Size(162, 17);
            this.AllExcpLoadCheckBox.TabIndex = 23;
            this.AllExcpLoadCheckBox.Text = "Check all exceptions on load";
            this.AllExcpLoadCheckBox.UseVisualStyleBackColor = true;
            this.AllExcpLoadCheckBox.CheckedChanged += new System.EventHandler(this.AllExcpLoadCheckBox_CheckedChanged);
            // 
            // AllDirLoadCheckBox
            // 
            this.AllDirLoadCheckBox.AutoSize = true;
            this.AllDirLoadCheckBox.Location = new System.Drawing.Point(6, 19);
            this.AllDirLoadCheckBox.Name = "AllDirLoadCheckBox";
            this.AllDirLoadCheckBox.Size = new System.Drawing.Size(198, 17);
            this.AllDirLoadCheckBox.TabIndex = 1;
            this.AllDirLoadCheckBox.Text = "Check all backup directories on load";
            this.AllDirLoadCheckBox.UseVisualStyleBackColor = true;
            this.AllDirLoadCheckBox.CheckedChanged += new System.EventHandler(this.AllDirLoadCheckBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 420);
            this.Controls.Add(this.OptionsGroup);
            this.Controls.Add(this.BackUpButton);
            this.Controls.Add(this.SettingsGroup);
            this.Controls.Add(this.OperationsGroup);
            this.Controls.Add(this.RestoreButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "FileSafe 3.1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.OperationsGroup.ResumeLayout(false);
            this.OperationsGroup.PerformLayout();
            this.SettingsGroup.ResumeLayout(false);
            this.SettingsGroup.PerformLayout();
            this.OptionsGroup.ResumeLayout(false);
            this.OptionsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BrowseDirectoryButton;
        private System.Windows.Forms.Button BackUpButton;
        private System.Windows.Forms.CheckedListBox DirChecklist;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox BackUpDirectoryText;
        private System.Windows.Forms.Button NewDirButton;
        private System.Windows.Forms.Button DelDirButton;
        private System.Windows.Forms.Button RestoreButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox ExceptChecklist;
        private System.Windows.Forms.Button ExceptDeleteButton;
        private System.Windows.Forms.Button ExceptAddButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label TaskLabel;
        private System.Windows.Forms.GroupBox OperationsGroup;
        private System.Windows.Forms.GroupBox SettingsGroup;
        private System.Windows.Forms.GroupBox OptionsGroup;
        private System.Windows.Forms.CheckBox AllDirLoadCheckBox;
        private System.Windows.Forms.CheckBox AllExcpLoadCheckBox;
        private System.Windows.Forms.Label ElapsedTimeLabel;
        private System.Windows.Forms.Button AbortButton;
    }
}

