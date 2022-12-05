
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	partial class InterfaceListGeneralOptionsPanel
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
			this.missingAssetGroupBox = new System.Windows.Forms.GroupBox();
			this.missingAssetColorButton = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox();
			this.missingAssetColorLabel = new System.Windows.Forms.Label();
			this.missingAssetColor = new System.Windows.Forms.Label();
			this.missingAssetContentTextBox = new System.Windows.Forms.TextBox();
			this.missingAssetContentLabel = new System.Windows.Forms.Label();
			this.spacingLabel3 = new System.Windows.Forms.Label();
			this.existingAssetGroupBox = new System.Windows.Forms.GroupBox();
			this.existingAssetColorButton = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox();
			this.existingAssetColorLabel = new System.Windows.Forms.Label();
			this.existingAssetColor = new System.Windows.Forms.Label();
			this.existingAssetContentTextBox = new System.Windows.Forms.TextBox();
			this.existingAssetContentLabel = new System.Windows.Forms.Label();
			this.listItemDoubleClickActionGroupBox = new System.Windows.Forms.GroupBox();
			this.listItemDoubleClickActionComboBox = new System.Windows.Forms.ComboBox();
			this.listItemDoubleClickActionLabel = new System.Windows.Forms.Label();
			this.spacingLabel5 = new System.Windows.Forms.Label();
			this.missingAssetGroupBox.SuspendLayout();
			this.existingAssetGroupBox.SuspendLayout();
			this.listItemDoubleClickActionGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// missingAssetGroupBox
			// 
			this.missingAssetGroupBox.Controls.Add(this.missingAssetColorButton);
			this.missingAssetGroupBox.Controls.Add(this.missingAssetColorLabel);
			this.missingAssetGroupBox.Controls.Add(this.missingAssetColor);
			this.missingAssetGroupBox.Controls.Add(this.missingAssetContentTextBox);
			this.missingAssetGroupBox.Controls.Add(this.missingAssetContentLabel);
			this.missingAssetGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.missingAssetGroupBox.Location = new System.Drawing.Point(0, 344);
			this.missingAssetGroupBox.Name = "missingAssetGroupBox";
			this.missingAssetGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.missingAssetGroupBox.Size = new System.Drawing.Size(460, 195);
			this.missingAssetGroupBox.TabIndex = 11;
			this.missingAssetGroupBox.TabStop = false;
			this.missingAssetGroupBox.Text = "&Missing Asset";
			// 
			// missingAssetColorButton
			// 
			this.missingAssetColorButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.missingAssetColorButton.DataMember = null;
			this.missingAssetColorButton.DataSource = null;
			this.missingAssetColorButton.DefaultColor = System.Drawing.Color.Transparent;
			this.missingAssetColorButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.missingAssetColorButton.Location = new System.Drawing.Point(12, 138);
			this.missingAssetColorButton.Name = "missingAssetColorButton";
			this.missingAssetColorButton.Size = new System.Drawing.Size(436, 45);
			this.missingAssetColorButton.TabIndex = 16;
			// 
			// missingAssetColorLabel
			// 
			this.missingAssetColorLabel.AutoSize = true;
			this.missingAssetColorLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.missingAssetColorLabel.Location = new System.Drawing.Point(12, 113);
			this.missingAssetColorLabel.Name = "missingAssetColorLabel";
			this.missingAssetColorLabel.Size = new System.Drawing.Size(65, 25);
			this.missingAssetColorLabel.TabIndex = 15;
			this.missingAssetColorLabel.Text = "&Color:";
			// 
			// missingAssetColor
			// 
			this.missingAssetColor.Dock = System.Windows.Forms.DockStyle.Top;
			this.missingAssetColor.Location = new System.Drawing.Point(12, 88);
			this.missingAssetColor.Name = "missingAssetColor";
			this.missingAssetColor.Size = new System.Drawing.Size(436, 25);
			this.missingAssetColor.TabIndex = 14;
			this.missingAssetColor.Text = " ";
			// 
			// missingAssetContentTextBox
			// 
			this.missingAssetContentTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.missingAssetContentTextBox.Location = new System.Drawing.Point(12, 59);
			this.missingAssetContentTextBox.Name = "missingAssetContentTextBox";
			this.missingAssetContentTextBox.Size = new System.Drawing.Size(436, 29);
			this.missingAssetContentTextBox.TabIndex = 13;
			// 
			// missingAssetContentLabel
			// 
			this.missingAssetContentLabel.AutoSize = true;
			this.missingAssetContentLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.missingAssetContentLabel.Location = new System.Drawing.Point(12, 34);
			this.missingAssetContentLabel.Name = "missingAssetContentLabel";
			this.missingAssetContentLabel.Size = new System.Drawing.Size(87, 25);
			this.missingAssetContentLabel.TabIndex = 12;
			this.missingAssetContentLabel.Text = "&Content:";
			// 
			// spacingLabel3
			// 
			this.spacingLabel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacingLabel3.Location = new System.Drawing.Point(0, 321);
			this.spacingLabel3.Name = "spacingLabel3";
			this.spacingLabel3.Size = new System.Drawing.Size(460, 23);
			this.spacingLabel3.TabIndex = 10;
			// 
			// existingAssetGroupBox
			// 
			this.existingAssetGroupBox.Controls.Add(this.existingAssetColorButton);
			this.existingAssetGroupBox.Controls.Add(this.existingAssetColorLabel);
			this.existingAssetGroupBox.Controls.Add(this.existingAssetColor);
			this.existingAssetGroupBox.Controls.Add(this.existingAssetContentTextBox);
			this.existingAssetGroupBox.Controls.Add(this.existingAssetContentLabel);
			this.existingAssetGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.existingAssetGroupBox.Location = new System.Drawing.Point(0, 126);
			this.existingAssetGroupBox.Name = "existingAssetGroupBox";
			this.existingAssetGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.existingAssetGroupBox.Size = new System.Drawing.Size(460, 195);
			this.existingAssetGroupBox.TabIndex = 4;
			this.existingAssetGroupBox.TabStop = false;
			this.existingAssetGroupBox.Text = "&Existing Asset";
			// 
			// existingAssetColorButton
			// 
			this.existingAssetColorButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.existingAssetColorButton.DataMember = null;
			this.existingAssetColorButton.DataSource = null;
			this.existingAssetColorButton.DefaultColor = System.Drawing.Color.Transparent;
			this.existingAssetColorButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.existingAssetColorButton.Location = new System.Drawing.Point(12, 138);
			this.existingAssetColorButton.Name = "existingAssetColorButton";
			this.existingAssetColorButton.Size = new System.Drawing.Size(436, 45);
			this.existingAssetColorButton.TabIndex = 9;
			// 
			// existingAssetColorLabel
			// 
			this.existingAssetColorLabel.AutoSize = true;
			this.existingAssetColorLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.existingAssetColorLabel.Location = new System.Drawing.Point(12, 113);
			this.existingAssetColorLabel.Name = "existingAssetColorLabel";
			this.existingAssetColorLabel.Size = new System.Drawing.Size(65, 25);
			this.existingAssetColorLabel.TabIndex = 8;
			this.existingAssetColorLabel.Text = "&Color:";
			// 
			// existingAssetColor
			// 
			this.existingAssetColor.Dock = System.Windows.Forms.DockStyle.Top;
			this.existingAssetColor.Location = new System.Drawing.Point(12, 88);
			this.existingAssetColor.Name = "existingAssetColor";
			this.existingAssetColor.Size = new System.Drawing.Size(436, 25);
			this.existingAssetColor.TabIndex = 7;
			this.existingAssetColor.Text = " ";
			// 
			// existingAssetContentTextBox
			// 
			this.existingAssetContentTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.existingAssetContentTextBox.Location = new System.Drawing.Point(12, 59);
			this.existingAssetContentTextBox.Name = "existingAssetContentTextBox";
			this.existingAssetContentTextBox.Size = new System.Drawing.Size(436, 29);
			this.existingAssetContentTextBox.TabIndex = 6;
			// 
			// existingAssetContentLabel
			// 
			this.existingAssetContentLabel.AutoSize = true;
			this.existingAssetContentLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.existingAssetContentLabel.Location = new System.Drawing.Point(12, 34);
			this.existingAssetContentLabel.Name = "existingAssetContentLabel";
			this.existingAssetContentLabel.Size = new System.Drawing.Size(87, 25);
			this.existingAssetContentLabel.TabIndex = 5;
			this.existingAssetContentLabel.Text = "&Content:";
			// 
			// listItemDoubleClickActionGroupBox
			// 
			this.listItemDoubleClickActionGroupBox.Controls.Add(this.listItemDoubleClickActionComboBox);
			this.listItemDoubleClickActionGroupBox.Controls.Add(this.listItemDoubleClickActionLabel);
			this.listItemDoubleClickActionGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.listItemDoubleClickActionGroupBox.Location = new System.Drawing.Point(0, 0);
			this.listItemDoubleClickActionGroupBox.Name = "listItemDoubleClickActionGroupBox";
			this.listItemDoubleClickActionGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.listItemDoubleClickActionGroupBox.Size = new System.Drawing.Size(460, 103);
			this.listItemDoubleClickActionGroupBox.TabIndex = 0;
			this.listItemDoubleClickActionGroupBox.TabStop = false;
			this.listItemDoubleClickActionGroupBox.Text = "Actions";
			// 
			// listItemDoubleClickActionComboBox
			// 
			this.listItemDoubleClickActionComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.listItemDoubleClickActionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.listItemDoubleClickActionComboBox.FormattingEnabled = true;
			this.listItemDoubleClickActionComboBox.Location = new System.Drawing.Point(12, 57);
			this.listItemDoubleClickActionComboBox.Name = "listItemDoubleClickActionComboBox";
			this.listItemDoubleClickActionComboBox.Size = new System.Drawing.Size(436, 32);
			this.listItemDoubleClickActionComboBox.TabIndex = 2;
			// 
			// listItemDoubleClickActionLabel
			// 
			this.listItemDoubleClickActionLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.listItemDoubleClickActionLabel.Location = new System.Drawing.Point(12, 34);
			this.listItemDoubleClickActionLabel.Name = "listItemDoubleClickActionLabel";
			this.listItemDoubleClickActionLabel.Size = new System.Drawing.Size(436, 23);
			this.listItemDoubleClickActionLabel.TabIndex = 1;
			this.listItemDoubleClickActionLabel.Text = "Mouse Left Double-Click && Enter Key:";
			// 
			// spacingLabel5
			// 
			this.spacingLabel5.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacingLabel5.Location = new System.Drawing.Point(0, 103);
			this.spacingLabel5.Name = "spacingLabel5";
			this.spacingLabel5.Size = new System.Drawing.Size(460, 23);
			this.spacingLabel5.TabIndex = 3;
			// 
			// InterfaceListGeneralOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.missingAssetGroupBox);
			this.Controls.Add(this.spacingLabel3);
			this.Controls.Add(this.existingAssetGroupBox);
			this.Controls.Add(this.spacingLabel5);
			this.Controls.Add(this.listItemDoubleClickActionGroupBox);
			this.Name = "InterfaceListGeneralOptionsPanel";
			this.Size = new System.Drawing.Size(460, 526);
			this.missingAssetGroupBox.ResumeLayout(false);
			this.missingAssetGroupBox.PerformLayout();
			this.existingAssetGroupBox.ResumeLayout(false);
			this.existingAssetGroupBox.PerformLayout();
			this.listItemDoubleClickActionGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox missingAssetGroupBox;
		private UserControls.ColorBox missingAssetColorButton;
		private System.Windows.Forms.Label missingAssetColorLabel;
		private System.Windows.Forms.Label missingAssetColor;
		private System.Windows.Forms.TextBox missingAssetContentTextBox;
		private System.Windows.Forms.Label missingAssetContentLabel;
		private System.Windows.Forms.Label spacingLabel3;
		private System.Windows.Forms.GroupBox existingAssetGroupBox;
		private UserControls.ColorBox existingAssetColorButton;
		private System.Windows.Forms.Label existingAssetColorLabel;
		private System.Windows.Forms.Label existingAssetColor;
		private System.Windows.Forms.TextBox existingAssetContentTextBox;
		private System.Windows.Forms.Label existingAssetContentLabel;
		private System.Windows.Forms.GroupBox listItemDoubleClickActionGroupBox;
		private System.Windows.Forms.ComboBox listItemDoubleClickActionComboBox;
		private System.Windows.Forms.Label spacingLabel5;
		private System.Windows.Forms.Label listItemDoubleClickActionLabel;
	}
}
