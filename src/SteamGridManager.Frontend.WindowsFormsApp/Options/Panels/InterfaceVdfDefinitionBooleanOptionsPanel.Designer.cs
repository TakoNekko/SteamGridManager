
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	partial class InterfaceVdfDefinitionBooleanOptionsPanel
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pathsListBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.StringCollectionListBox();
			this.pathsLabel = new System.Windows.Forms.Label();
			this.colorLabel = new System.Windows.Forms.Label();
			this.trueStringLabel = new System.Windows.Forms.Label();
			this.trueStringTextBox = new System.Windows.Forms.TextBox();
			this.falseStringLabel = new System.Windows.Forms.Label();
			this.falseStringTextBox = new System.Windows.Forms.TextBox();
			this.formatCheckBox = new System.Windows.Forms.CheckBox();
			this.formattingGroupBox = new System.Windows.Forms.GroupBox();
			this.spacing3 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.colorButton = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox();
			this.formattingGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// pathsListBox
			// 
			this.pathsListBox.CanEdit = false;
			this.pathsListBox.DataMember = null;
			this.pathsListBox.DataSource = null;
			this.pathsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pathsListBox.Location = new System.Drawing.Point(0, 343);
			this.pathsListBox.Name = "pathsListBox";
			this.pathsListBox.Size = new System.Drawing.Size(600, 137);
			this.pathsListBox.TabIndex = 11;
			// 
			// pathsLabel
			// 
			this.pathsLabel.AutoSize = true;
			this.pathsLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.pathsLabel.Location = new System.Drawing.Point(0, 318);
			this.pathsLabel.Name = "pathsLabel";
			this.pathsLabel.Size = new System.Drawing.Size(68, 25);
			this.pathsLabel.TabIndex = 10;
			this.pathsLabel.Text = "Paths:";
			// 
			// colorLabel
			// 
			this.colorLabel.AutoSize = true;
			this.colorLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.colorLabel.Location = new System.Drawing.Point(0, 0);
			this.colorLabel.Name = "colorLabel";
			this.colorLabel.Size = new System.Drawing.Size(65, 25);
			this.colorLabel.TabIndex = 0;
			this.colorLabel.Text = "&Color:";
			// 
			// trueStringLabel
			// 
			this.trueStringLabel.AutoSize = true;
			this.trueStringLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.trueStringLabel.Location = new System.Drawing.Point(12, 113);
			this.trueStringLabel.Name = "trueStringLabel";
			this.trueStringLabel.Size = new System.Drawing.Size(59, 25);
			this.trueStringLabel.TabIndex = 7;
			this.trueStringLabel.Text = "True:";
			// 
			// trueStringTextBox
			// 
			this.trueStringTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.trueStringTextBox.Location = new System.Drawing.Point(12, 138);
			this.trueStringTextBox.Name = "trueStringTextBox";
			this.trueStringTextBox.Size = new System.Drawing.Size(576, 29);
			this.trueStringTextBox.TabIndex = 8;
			// 
			// falseStringLabel
			// 
			this.falseStringLabel.AutoSize = true;
			this.falseStringLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.falseStringLabel.Location = new System.Drawing.Point(12, 34);
			this.falseStringLabel.Name = "falseStringLabel";
			this.falseStringLabel.Size = new System.Drawing.Size(66, 25);
			this.falseStringLabel.TabIndex = 4;
			this.falseStringLabel.Text = "False:";
			// 
			// falseStringTextBox
			// 
			this.falseStringTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.falseStringTextBox.Location = new System.Drawing.Point(12, 59);
			this.falseStringTextBox.Name = "falseStringTextBox";
			this.falseStringTextBox.Size = new System.Drawing.Size(576, 29);
			this.falseStringTextBox.TabIndex = 5;
			// 
			// formatCheckBox
			// 
			this.formatCheckBox.AutoSize = true;
			this.formatCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.formatCheckBox.Location = new System.Drawing.Point(0, 93);
			this.formatCheckBox.Name = "formatCheckBox";
			this.formatCheckBox.Size = new System.Drawing.Size(600, 29);
			this.formatCheckBox.TabIndex = 2;
			this.formatCheckBox.Text = "&Format";
			this.formatCheckBox.UseVisualStyleBackColor = true;
			this.formatCheckBox.CheckedChanged += new System.EventHandler(this.formatCheckBox_CheckedChanged);
			// 
			// formattingGroupBox
			// 
			this.formattingGroupBox.Controls.Add(this.trueStringTextBox);
			this.formattingGroupBox.Controls.Add(this.trueStringLabel);
			this.formattingGroupBox.Controls.Add(this.spacing3);
			this.formattingGroupBox.Controls.Add(this.falseStringTextBox);
			this.formattingGroupBox.Controls.Add(this.falseStringLabel);
			this.formattingGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.formattingGroupBox.Location = new System.Drawing.Point(0, 122);
			this.formattingGroupBox.Name = "formattingGroupBox";
			this.formattingGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.formattingGroupBox.Size = new System.Drawing.Size(600, 173);
			this.formattingGroupBox.TabIndex = 3;
			this.formattingGroupBox.TabStop = false;
			this.formattingGroupBox.Text = "&Strings";
			// 
			// spacing3
			// 
			this.spacing3.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacing3.Location = new System.Drawing.Point(12, 88);
			this.spacing3.Name = "spacing3";
			this.spacing3.Size = new System.Drawing.Size(576, 25);
			this.spacing3.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.label3.Location = new System.Drawing.Point(0, 70);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(600, 23);
			this.label3.TabIndex = 13;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 295);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(600, 23);
			this.label1.TabIndex = 9;
			// 
			// colorButton
			// 
			this.colorButton.DataMember = null;
			this.colorButton.DataSource = null;
			this.colorButton.DefaultColor = System.Drawing.Color.Black;
			this.colorButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.colorButton.Location = new System.Drawing.Point(0, 25);
			this.colorButton.Name = "colorButton";
			this.colorButton.Size = new System.Drawing.Size(600, 45);
			this.colorButton.TabIndex = 1;
			// 
			// InterfaceVdfDefinitionBooleanOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pathsListBox);
			this.Controls.Add(this.pathsLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.formattingGroupBox);
			this.Controls.Add(this.formatCheckBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.colorButton);
			this.Controls.Add(this.colorLabel);
			this.MinimumSize = new System.Drawing.Size(300, 300);
			this.Name = "InterfaceVdfDefinitionBooleanOptionsPanel";
			this.Size = new System.Drawing.Size(600, 480);
			this.formattingGroupBox.ResumeLayout(false);
			this.formattingGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.StringCollectionListBox pathsListBox;
		private System.Windows.Forms.Label pathsLabel;
		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox colorButton;
		private System.Windows.Forms.Label colorLabel;
		private System.Windows.Forms.Label trueStringLabel;
		private System.Windows.Forms.TextBox trueStringTextBox;
		private System.Windows.Forms.Label falseStringLabel;
		private System.Windows.Forms.TextBox falseStringTextBox;
		private System.Windows.Forms.CheckBox formatCheckBox;
		private System.Windows.Forms.GroupBox formattingGroupBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label spacing3;
	}
}
