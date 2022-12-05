
namespace SteamGridManager.Frontend.WindowsFormsApp
{
	partial class VdfPropertyEditorDialog
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
			this.propertyNameLabel = new System.Windows.Forms.Label();
			this.propertyNameTextBox = new System.Windows.Forms.TextBox();
			this.propertyValueLabel = new System.Windows.Forms.Label();
			this.propertyValueTabControl = new System.Windows.Forms.TabControl();
			this.vdfObjectValueTabPage = new System.Windows.Forms.TabPage();
			this.vdfObjectPropertyListView = new SteamGridManager.Frontend.WindowsFormsApp.UserControls.VdfObjectPropertyListView();
			this.stringValueTabPage = new System.Windows.Forms.TabPage();
			this.stringValueTextBox = new System.Windows.Forms.TextBox();
			this.allowTabsCheckBox = new System.Windows.Forms.CheckBox();
			this.allowNewLinesCheckBox = new System.Windows.Forms.CheckBox();
			this.uintValueTabPage = new System.Windows.Forms.TabPage();
			this.uintValueNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.booleanValueTabPage = new System.Windows.Forms.TabPage();
			this.booleanValueCheckBox = new System.Windows.Forms.CheckBox();
			this.enumValueTabPage = new System.Windows.Forms.TabPage();
			this.label2 = new System.Windows.Forms.Label();
			this.enumValueComboBox = new System.Windows.Forms.ComboBox();
			this.relativeTimeValueTabPage = new System.Windows.Forms.TabPage();
			this.relativeTimeValueDateTimePicker = new System.Windows.Forms.DateTimePicker();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.spacingLabel3 = new System.Windows.Forms.Label();
			this.propertyValueTabControl.SuspendLayout();
			this.vdfObjectValueTabPage.SuspendLayout();
			this.stringValueTabPage.SuspendLayout();
			this.uintValueTabPage.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.uintValueNumericUpDown)).BeginInit();
			this.booleanValueTabPage.SuspendLayout();
			this.enumValueTabPage.SuspendLayout();
			this.relativeTimeValueTabPage.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// propertyNameLabel
			// 
			this.propertyNameLabel.AutoSize = true;
			this.propertyNameLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.propertyNameLabel.Location = new System.Drawing.Point(12, 12);
			this.propertyNameLabel.Name = "propertyNameLabel";
			this.propertyNameLabel.Size = new System.Drawing.Size(70, 25);
			this.propertyNameLabel.TabIndex = 0;
			this.propertyNameLabel.Text = "&Name:";
			// 
			// propertyNameTextBox
			// 
			this.propertyNameTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.propertyNameTextBox.Location = new System.Drawing.Point(12, 37);
			this.propertyNameTextBox.Name = "propertyNameTextBox";
			this.propertyNameTextBox.Size = new System.Drawing.Size(852, 29);
			this.propertyNameTextBox.TabIndex = 1;
			// 
			// propertyValueLabel
			// 
			this.propertyValueLabel.AutoSize = true;
			this.propertyValueLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.propertyValueLabel.Location = new System.Drawing.Point(12, 66);
			this.propertyValueLabel.Name = "propertyValueLabel";
			this.propertyValueLabel.Size = new System.Drawing.Size(69, 25);
			this.propertyValueLabel.TabIndex = 5;
			this.propertyValueLabel.Text = "&Value:";
			// 
			// propertyValueTabControl
			// 
			this.propertyValueTabControl.Controls.Add(this.vdfObjectValueTabPage);
			this.propertyValueTabControl.Controls.Add(this.stringValueTabPage);
			this.propertyValueTabControl.Controls.Add(this.uintValueTabPage);
			this.propertyValueTabControl.Controls.Add(this.booleanValueTabPage);
			this.propertyValueTabControl.Controls.Add(this.enumValueTabPage);
			this.propertyValueTabControl.Controls.Add(this.relativeTimeValueTabPage);
			this.propertyValueTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyValueTabControl.Location = new System.Drawing.Point(12, 91);
			this.propertyValueTabControl.Name = "propertyValueTabControl";
			this.propertyValueTabControl.SelectedIndex = 0;
			this.propertyValueTabControl.Size = new System.Drawing.Size(852, 344);
			this.propertyValueTabControl.TabIndex = 2;
			// 
			// vdfObjectValueTabPage
			// 
			this.vdfObjectValueTabPage.Controls.Add(this.vdfObjectPropertyListView);
			this.vdfObjectValueTabPage.Location = new System.Drawing.Point(4, 33);
			this.vdfObjectValueTabPage.Name = "vdfObjectValueTabPage";
			this.vdfObjectValueTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.vdfObjectValueTabPage.Size = new System.Drawing.Size(844, 307);
			this.vdfObjectValueTabPage.TabIndex = 0;
			this.vdfObjectValueTabPage.Tag = "object";
			this.vdfObjectValueTabPage.Text = "Object";
			this.vdfObjectValueTabPage.UseVisualStyleBackColor = true;
			// 
			// vdfObjectPropertyListView
			// 
			this.vdfObjectPropertyListView.AppID = ((ulong)(0ul));
			this.vdfObjectPropertyListView.AppInfos = null;
			this.vdfObjectPropertyListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vdfObjectPropertyListView.Location = new System.Drawing.Point(3, 3);
			this.vdfObjectPropertyListView.MinimumSize = new System.Drawing.Size(300, 200);
			this.vdfObjectPropertyListView.Name = "vdfObjectPropertyListView";
			this.vdfObjectPropertyListView.Path = null;
			this.vdfObjectPropertyListView.Shortcuts = null;
			this.vdfObjectPropertyListView.Size = new System.Drawing.Size(838, 301);
			this.vdfObjectPropertyListView.TabIndex = 0;
			this.vdfObjectPropertyListView.VdfObject = null;
			// 
			// stringValueTabPage
			// 
			this.stringValueTabPage.Controls.Add(this.stringValueTextBox);
			this.stringValueTabPage.Controls.Add(this.allowTabsCheckBox);
			this.stringValueTabPage.Controls.Add(this.allowNewLinesCheckBox);
			this.stringValueTabPage.Location = new System.Drawing.Point(4, 33);
			this.stringValueTabPage.Name = "stringValueTabPage";
			this.stringValueTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.stringValueTabPage.Size = new System.Drawing.Size(844, 307);
			this.stringValueTabPage.TabIndex = 1;
			this.stringValueTabPage.Tag = "string";
			this.stringValueTabPage.Text = "Text";
			this.stringValueTabPage.UseVisualStyleBackColor = true;
			// 
			// stringValueTextBox
			// 
			this.stringValueTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stringValueTextBox.Location = new System.Drawing.Point(3, 3);
			this.stringValueTextBox.Multiline = true;
			this.stringValueTextBox.Name = "stringValueTextBox";
			this.stringValueTextBox.Size = new System.Drawing.Size(838, 243);
			this.stringValueTextBox.TabIndex = 1;
			// 
			// allowTabsCheckBox
			// 
			this.allowTabsCheckBox.AutoSize = true;
			this.allowTabsCheckBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.allowTabsCheckBox.Location = new System.Drawing.Point(3, 246);
			this.allowTabsCheckBox.Name = "allowTabsCheckBox";
			this.allowTabsCheckBox.Size = new System.Drawing.Size(838, 29);
			this.allowTabsCheckBox.TabIndex = 2;
			this.allowTabsCheckBox.Text = "Allow Tabs";
			this.allowTabsCheckBox.UseVisualStyleBackColor = true;
			this.allowTabsCheckBox.CheckedChanged += new System.EventHandler(this.allowTabsCheckBox_CheckedChanged);
			// 
			// allowNewLinesCheckBox
			// 
			this.allowNewLinesCheckBox.AutoSize = true;
			this.allowNewLinesCheckBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.allowNewLinesCheckBox.Location = new System.Drawing.Point(3, 275);
			this.allowNewLinesCheckBox.Name = "allowNewLinesCheckBox";
			this.allowNewLinesCheckBox.Size = new System.Drawing.Size(838, 29);
			this.allowNewLinesCheckBox.TabIndex = 3;
			this.allowNewLinesCheckBox.Text = "Allow New Lines";
			this.allowNewLinesCheckBox.UseVisualStyleBackColor = true;
			this.allowNewLinesCheckBox.CheckedChanged += new System.EventHandler(this.allowNewLinesCheckBox_CheckedChanged);
			// 
			// uintValueTabPage
			// 
			this.uintValueTabPage.Controls.Add(this.uintValueNumericUpDown);
			this.uintValueTabPage.Location = new System.Drawing.Point(4, 33);
			this.uintValueTabPage.Name = "uintValueTabPage";
			this.uintValueTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.uintValueTabPage.Size = new System.Drawing.Size(844, 307);
			this.uintValueTabPage.TabIndex = 2;
			this.uintValueTabPage.Tag = "uint";
			this.uintValueTabPage.Text = "Number";
			this.uintValueTabPage.UseVisualStyleBackColor = true;
			// 
			// uintValueNumericUpDown
			// 
			this.uintValueNumericUpDown.Dock = System.Windows.Forms.DockStyle.Fill;
			this.uintValueNumericUpDown.Location = new System.Drawing.Point(3, 3);
			this.uintValueNumericUpDown.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
			this.uintValueNumericUpDown.Name = "uintValueNumericUpDown";
			this.uintValueNumericUpDown.Size = new System.Drawing.Size(838, 29);
			this.uintValueNumericUpDown.TabIndex = 0;
			// 
			// booleanValueTabPage
			// 
			this.booleanValueTabPage.Controls.Add(this.booleanValueCheckBox);
			this.booleanValueTabPage.Location = new System.Drawing.Point(4, 33);
			this.booleanValueTabPage.Name = "booleanValueTabPage";
			this.booleanValueTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.booleanValueTabPage.Size = new System.Drawing.Size(844, 307);
			this.booleanValueTabPage.TabIndex = 3;
			this.booleanValueTabPage.Tag = "bool";
			this.booleanValueTabPage.Text = "Boolean";
			this.booleanValueTabPage.UseVisualStyleBackColor = true;
			// 
			// booleanValueCheckBox
			// 
			this.booleanValueCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.booleanValueCheckBox.Location = new System.Drawing.Point(3, 3);
			this.booleanValueCheckBox.Name = "booleanValueCheckBox";
			this.booleanValueCheckBox.Size = new System.Drawing.Size(838, 24);
			this.booleanValueCheckBox.TabIndex = 0;
			this.booleanValueCheckBox.Text = "&On";
			this.booleanValueCheckBox.UseVisualStyleBackColor = true;
			// 
			// enumValueTabPage
			// 
			this.enumValueTabPage.Controls.Add(this.label2);
			this.enumValueTabPage.Controls.Add(this.enumValueComboBox);
			this.enumValueTabPage.Location = new System.Drawing.Point(4, 33);
			this.enumValueTabPage.Name = "enumValueTabPage";
			this.enumValueTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.enumValueTabPage.Size = new System.Drawing.Size(844, 307);
			this.enumValueTabPage.TabIndex = 4;
			this.enumValueTabPage.Tag = "enum";
			this.enumValueTabPage.Text = "Enumeration";
			this.enumValueTabPage.UseVisualStyleBackColor = true;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.ForeColor = System.Drawing.SystemColors.MenuHighlight;
			this.label2.Location = new System.Drawing.Point(3, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(838, 269);
			this.label2.TabIndex = 1;
			this.label2.Text = "TODO: Not implemented yet.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// enumValueComboBox
			// 
			this.enumValueComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.enumValueComboBox.FormattingEnabled = true;
			this.enumValueComboBox.Location = new System.Drawing.Point(3, 3);
			this.enumValueComboBox.Name = "enumValueComboBox";
			this.enumValueComboBox.Size = new System.Drawing.Size(838, 32);
			this.enumValueComboBox.TabIndex = 0;
			// 
			// relativeTimeValueTabPage
			// 
			this.relativeTimeValueTabPage.Controls.Add(this.relativeTimeValueDateTimePicker);
			this.relativeTimeValueTabPage.Location = new System.Drawing.Point(4, 33);
			this.relativeTimeValueTabPage.Name = "relativeTimeValueTabPage";
			this.relativeTimeValueTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.relativeTimeValueTabPage.Size = new System.Drawing.Size(844, 307);
			this.relativeTimeValueTabPage.TabIndex = 5;
			this.relativeTimeValueTabPage.Tag = "rtime";
			this.relativeTimeValueTabPage.Text = "Relative Time";
			this.relativeTimeValueTabPage.UseVisualStyleBackColor = true;
			// 
			// relativeTimeValueDateTimePicker
			// 
			this.relativeTimeValueDateTimePicker.Dock = System.Windows.Forms.DockStyle.Top;
			this.relativeTimeValueDateTimePicker.Location = new System.Drawing.Point(3, 3);
			this.relativeTimeValueDateTimePicker.MaxDate = new System.DateTime(2106, 2, 7, 0, 0, 0, 0);
			this.relativeTimeValueDateTimePicker.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
			this.relativeTimeValueDateTimePicker.Name = "relativeTimeValueDateTimePicker";
			this.relativeTimeValueDateTimePicker.Size = new System.Drawing.Size(838, 29);
			this.relativeTimeValueDateTimePicker.TabIndex = 0;
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.okButton.Enabled = false;
			this.okButton.Location = new System.Drawing.Point(502, 12);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(150, 40);
			this.okButton.TabIndex = 3;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.cancelButton.Location = new System.Drawing.Point(664, 12);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(176, 40);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.okButton);
			this.panel1.Controls.Add(this.spacingLabel3);
			this.panel1.Controls.Add(this.cancelButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(12, 435);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(12);
			this.panel1.Size = new System.Drawing.Size(852, 64);
			this.panel1.TabIndex = 6;
			// 
			// spacingLabel3
			// 
			this.spacingLabel3.Dock = System.Windows.Forms.DockStyle.Right;
			this.spacingLabel3.Location = new System.Drawing.Point(652, 12);
			this.spacingLabel3.Name = "spacingLabel3";
			this.spacingLabel3.Size = new System.Drawing.Size(12, 40);
			this.spacingLabel3.TabIndex = 5;
			this.spacingLabel3.Text = "label3";
			// 
			// VdfPropertyEditorDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(876, 511);
			this.Controls.Add(this.propertyValueTabControl);
			this.Controls.Add(this.propertyValueLabel);
			this.Controls.Add(this.propertyNameTextBox);
			this.Controls.Add(this.propertyNameLabel);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimumSize = new System.Drawing.Size(450, 450);
			this.Name = "VdfPropertyEditorDialog";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Object Property Editor";
			this.propertyValueTabControl.ResumeLayout(false);
			this.vdfObjectValueTabPage.ResumeLayout(false);
			this.stringValueTabPage.ResumeLayout(false);
			this.stringValueTabPage.PerformLayout();
			this.uintValueTabPage.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.uintValueNumericUpDown)).EndInit();
			this.booleanValueTabPage.ResumeLayout(false);
			this.enumValueTabPage.ResumeLayout(false);
			this.relativeTimeValueTabPage.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label propertyNameLabel;
		private System.Windows.Forms.TextBox propertyNameTextBox;
		private System.Windows.Forms.Label propertyValueLabel;
		private System.Windows.Forms.TabControl propertyValueTabControl;
		private System.Windows.Forms.TabPage vdfObjectValueTabPage;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TabPage stringValueTabPage;
		private System.Windows.Forms.TabPage uintValueTabPage;
		private System.Windows.Forms.TextBox stringValueTextBox;
		private System.Windows.Forms.NumericUpDown uintValueNumericUpDown;
		private System.Windows.Forms.CheckBox allowNewLinesCheckBox;
		private System.Windows.Forms.CheckBox allowTabsCheckBox;
		private System.Windows.Forms.TabPage booleanValueTabPage;
		private System.Windows.Forms.TabPage enumValueTabPage;
		private System.Windows.Forms.TabPage relativeTimeValueTabPage;
		private System.Windows.Forms.CheckBox booleanValueCheckBox;
		private System.Windows.Forms.ComboBox enumValueComboBox;
		private System.Windows.Forms.DateTimePicker relativeTimeValueDateTimePicker;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label spacingLabel3;
		private UserControls.VdfObjectPropertyListView vdfObjectPropertyListView;
	}
}