
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	partial class EnvironmentPathsOptionsPanel
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
			this.steamFolderGroupBox = new System.Windows.Forms.GroupBox();
			this.steamFolderCreateButton = new System.Windows.Forms.Button();
			this.spacingLabel1 = new System.Windows.Forms.Label();
			this.spacingLabel2 = new System.Windows.Forms.Label();
			this.steamUserIDGroupBox = new System.Windows.Forms.GroupBox();
			this.steamUserIDComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.shortcutIconCacheFolderGroupBox = new System.Windows.Forms.GroupBox();
			this.spacingLabel3 = new System.Windows.Forms.Label();
			this.overlaysFolderGroupBox = new System.Windows.Forms.GroupBox();
			this.overlaysGroupBox = new System.Windows.Forms.GroupBox();
			this.overlaysFolderCreateButton = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.localizationGroupBox = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.localizationFolderPathBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathBox();
			this.overlaysFolderPathBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathBox();
			this.backupFolderPathBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathBox();
			this.nonSteamAppIconCacheFolderPathBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathBox();
			this.steamFolderPathBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathBox();
			this.steamFolderGroupBox.SuspendLayout();
			this.steamUserIDGroupBox.SuspendLayout();
			this.shortcutIconCacheFolderGroupBox.SuspendLayout();
			this.overlaysFolderGroupBox.SuspendLayout();
			this.overlaysGroupBox.SuspendLayout();
			this.localizationGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// steamFolderGroupBox
			// 
			this.steamFolderGroupBox.AutoSize = true;
			this.steamFolderGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.steamFolderGroupBox.Controls.Add(this.steamFolderCreateButton);
			this.steamFolderGroupBox.Controls.Add(this.spacingLabel1);
			this.steamFolderGroupBox.Controls.Add(this.steamFolderPathBox);
			this.steamFolderGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.steamFolderGroupBox.Location = new System.Drawing.Point(0, 105);
			this.steamFolderGroupBox.Name = "steamFolderGroupBox";
			this.steamFolderGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.steamFolderGroupBox.Size = new System.Drawing.Size(480, 177);
			this.steamFolderGroupBox.TabIndex = 3;
			this.steamFolderGroupBox.TabStop = false;
			this.steamFolderGroupBox.Text = "&Steam Folder";
			// 
			// steamFolderCreateButton
			// 
			this.steamFolderCreateButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.steamFolderCreateButton.Enabled = false;
			this.steamFolderCreateButton.Location = new System.Drawing.Point(12, 130);
			this.steamFolderCreateButton.Name = "steamFolderCreateButton";
			this.steamFolderCreateButton.Size = new System.Drawing.Size(456, 35);
			this.steamFolderCreateButton.TabIndex = 6;
			this.steamFolderCreateButton.Text = "&Create";
			this.steamFolderCreateButton.UseVisualStyleBackColor = true;
			this.steamFolderCreateButton.Click += new System.EventHandler(this.steamFolderCreateButton_Click);
			// 
			// spacingLabel1
			// 
			this.spacingLabel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacingLabel1.Location = new System.Drawing.Point(12, 105);
			this.spacingLabel1.Name = "spacingLabel1";
			this.spacingLabel1.Size = new System.Drawing.Size(456, 25);
			this.spacingLabel1.TabIndex = 5;
			this.spacingLabel1.Text = " ";
			// 
			// spacingLabel2
			// 
			this.spacingLabel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacingLabel2.Location = new System.Drawing.Point(0, 80);
			this.spacingLabel2.Name = "spacingLabel2";
			this.spacingLabel2.Size = new System.Drawing.Size(480, 25);
			this.spacingLabel2.TabIndex = 2;
			this.spacingLabel2.Text = " ";
			// 
			// steamUserIDGroupBox
			// 
			this.steamUserIDGroupBox.Controls.Add(this.steamUserIDComboBox);
			this.steamUserIDGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.steamUserIDGroupBox.Location = new System.Drawing.Point(0, 0);
			this.steamUserIDGroupBox.Name = "steamUserIDGroupBox";
			this.steamUserIDGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.steamUserIDGroupBox.Size = new System.Drawing.Size(480, 80);
			this.steamUserIDGroupBox.TabIndex = 0;
			this.steamUserIDGroupBox.TabStop = false;
			this.steamUserIDGroupBox.Text = "Steam &User ID";
			// 
			// steamUserIDComboBox
			// 
			this.steamUserIDComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.steamUserIDComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.steamUserIDComboBox.FormattingEnabled = true;
			this.steamUserIDComboBox.Location = new System.Drawing.Point(12, 34);
			this.steamUserIDComboBox.Name = "steamUserIDComboBox";
			this.steamUserIDComboBox.Size = new System.Drawing.Size(456, 32);
			this.steamUserIDComboBox.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 282);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(480, 25);
			this.label1.TabIndex = 7;
			this.label1.Text = " ";
			// 
			// shortcutIconCacheFolderGroupBox
			// 
			this.shortcutIconCacheFolderGroupBox.AutoSize = true;
			this.shortcutIconCacheFolderGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.shortcutIconCacheFolderGroupBox.Controls.Add(this.nonSteamAppIconCacheFolderPathBox);
			this.shortcutIconCacheFolderGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.shortcutIconCacheFolderGroupBox.Location = new System.Drawing.Point(0, 307);
			this.shortcutIconCacheFolderGroupBox.Name = "shortcutIconCacheFolderGroupBox";
			this.shortcutIconCacheFolderGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.shortcutIconCacheFolderGroupBox.Size = new System.Drawing.Size(480, 117);
			this.shortcutIconCacheFolderGroupBox.TabIndex = 8;
			this.shortcutIconCacheFolderGroupBox.TabStop = false;
			this.shortcutIconCacheFolderGroupBox.Text = "Shortcut Icon Cache Folder";
			// 
			// spacingLabel3
			// 
			this.spacingLabel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacingLabel3.Location = new System.Drawing.Point(0, 424);
			this.spacingLabel3.Name = "spacingLabel3";
			this.spacingLabel3.Size = new System.Drawing.Size(480, 25);
			this.spacingLabel3.TabIndex = 10;
			this.spacingLabel3.Text = " ";
			// 
			// overlaysFolderGroupBox
			// 
			this.overlaysFolderGroupBox.AutoSize = true;
			this.overlaysFolderGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.overlaysFolderGroupBox.Controls.Add(this.backupFolderPathBox);
			this.overlaysFolderGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.overlaysFolderGroupBox.Location = new System.Drawing.Point(0, 449);
			this.overlaysFolderGroupBox.Name = "overlaysFolderGroupBox";
			this.overlaysFolderGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.overlaysFolderGroupBox.Size = new System.Drawing.Size(480, 117);
			this.overlaysFolderGroupBox.TabIndex = 11;
			this.overlaysFolderGroupBox.TabStop = false;
			this.overlaysFolderGroupBox.Text = "&Backup Folder";
			// 
			// overlaysGroupBox
			// 
			this.overlaysGroupBox.AutoSize = true;
			this.overlaysGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.overlaysGroupBox.Controls.Add(this.overlaysFolderCreateButton);
			this.overlaysGroupBox.Controls.Add(this.label4);
			this.overlaysGroupBox.Controls.Add(this.overlaysFolderPathBox);
			this.overlaysGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.overlaysGroupBox.Location = new System.Drawing.Point(0, 591);
			this.overlaysGroupBox.Name = "overlaysGroupBox";
			this.overlaysGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.overlaysGroupBox.Size = new System.Drawing.Size(480, 177);
			this.overlaysGroupBox.TabIndex = 13;
			this.overlaysGroupBox.TabStop = false;
			this.overlaysGroupBox.Text = "&Overlays Folder";
			// 
			// overlaysFolderCreateButton
			// 
			this.overlaysFolderCreateButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.overlaysFolderCreateButton.Enabled = false;
			this.overlaysFolderCreateButton.Location = new System.Drawing.Point(12, 130);
			this.overlaysFolderCreateButton.Name = "overlaysFolderCreateButton";
			this.overlaysFolderCreateButton.Size = new System.Drawing.Size(456, 35);
			this.overlaysFolderCreateButton.TabIndex = 16;
			this.overlaysFolderCreateButton.Text = "&Create";
			this.overlaysFolderCreateButton.UseVisualStyleBackColor = true;
			this.overlaysFolderCreateButton.Click += new System.EventHandler(this.overlaysFolderCreateButton_Click);
			// 
			// label4
			// 
			this.label4.Dock = System.Windows.Forms.DockStyle.Top;
			this.label4.Location = new System.Drawing.Point(12, 105);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(456, 25);
			this.label4.TabIndex = 15;
			this.label4.Text = " ";
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 566);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(480, 25);
			this.label2.TabIndex = 12;
			this.label2.Text = " ";
			// 
			// localizationGroupBox
			// 
			this.localizationGroupBox.AutoSize = true;
			this.localizationGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.localizationGroupBox.Controls.Add(this.localizationFolderPathBox);
			this.localizationGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.localizationGroupBox.Location = new System.Drawing.Point(0, 793);
			this.localizationGroupBox.Name = "localizationGroupBox";
			this.localizationGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.localizationGroupBox.Size = new System.Drawing.Size(480, 117);
			this.localizationGroupBox.TabIndex = 18;
			this.localizationGroupBox.TabStop = false;
			this.localizationGroupBox.Text = "&Localization Folder";
			// 
			// label3
			// 
			this.label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.label3.Location = new System.Drawing.Point(0, 768);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(480, 25);
			this.label3.TabIndex = 17;
			this.label3.Text = " ";
			// 
			// localizationFolderPathBox
			// 
			this.localizationFolderPathBox.CheckDirectoryExists = false;
			this.localizationFolderPathBox.CheckFileExists = false;
			this.localizationFolderPathBox.DefaultPath = null;
			this.localizationFolderPathBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.localizationFolderPathBox.Filter = null;
			this.localizationFolderPathBox.FilterIndex = 0;
			this.localizationFolderPathBox.InitialDirectory = null;
			this.localizationFolderPathBox.Location = new System.Drawing.Point(12, 34);
			this.localizationFolderPathBox.MinimumSize = new System.Drawing.Size(0, 71);
			this.localizationFolderPathBox.MultiSelect = false;
			this.localizationFolderPathBox.Name = "localizationFolderPathBox";
			this.localizationFolderPathBox.Path = null;
			this.localizationFolderPathBox.PathOpenType = SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathOpenType.Open;
			this.localizationFolderPathBox.PathType = SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathType.Directory;
			this.localizationFolderPathBox.Size = new System.Drawing.Size(456, 71);
			this.localizationFolderPathBox.TabIndex = 19;
			this.localizationFolderPathBox.Title = "Localization Folder";
			// 
			// overlaysFolderPathBox
			// 
			this.overlaysFolderPathBox.CheckDirectoryExists = false;
			this.overlaysFolderPathBox.CheckFileExists = false;
			this.overlaysFolderPathBox.DefaultPath = null;
			this.overlaysFolderPathBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.overlaysFolderPathBox.Filter = null;
			this.overlaysFolderPathBox.FilterIndex = 0;
			this.overlaysFolderPathBox.InitialDirectory = null;
			this.overlaysFolderPathBox.Location = new System.Drawing.Point(12, 34);
			this.overlaysFolderPathBox.MinimumSize = new System.Drawing.Size(0, 71);
			this.overlaysFolderPathBox.MultiSelect = false;
			this.overlaysFolderPathBox.Name = "overlaysFolderPathBox";
			this.overlaysFolderPathBox.Path = null;
			this.overlaysFolderPathBox.PathOpenType = SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathOpenType.Open;
			this.overlaysFolderPathBox.PathType = SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathType.Directory;
			this.overlaysFolderPathBox.Size = new System.Drawing.Size(456, 71);
			this.overlaysFolderPathBox.TabIndex = 14;
			this.overlaysFolderPathBox.Title = "Overlays Folder";
			// 
			// backupFolderPathBox
			// 
			this.backupFolderPathBox.CheckDirectoryExists = false;
			this.backupFolderPathBox.CheckFileExists = false;
			this.backupFolderPathBox.DefaultPath = null;
			this.backupFolderPathBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.backupFolderPathBox.Filter = null;
			this.backupFolderPathBox.FilterIndex = 0;
			this.backupFolderPathBox.InitialDirectory = null;
			this.backupFolderPathBox.Location = new System.Drawing.Point(12, 34);
			this.backupFolderPathBox.MinimumSize = new System.Drawing.Size(0, 71);
			this.backupFolderPathBox.MultiSelect = false;
			this.backupFolderPathBox.Name = "backupFolderPathBox";
			this.backupFolderPathBox.Path = null;
			this.backupFolderPathBox.PathOpenType = SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathOpenType.Open;
			this.backupFolderPathBox.PathType = SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathType.Directory;
			this.backupFolderPathBox.Size = new System.Drawing.Size(456, 71);
			this.backupFolderPathBox.TabIndex = 11;
			this.backupFolderPathBox.Title = "Backup Folder";
			// 
			// nonSteamAppIconCacheFolderPathBox
			// 
			this.nonSteamAppIconCacheFolderPathBox.CheckDirectoryExists = false;
			this.nonSteamAppIconCacheFolderPathBox.CheckFileExists = false;
			this.nonSteamAppIconCacheFolderPathBox.DefaultPath = null;
			this.nonSteamAppIconCacheFolderPathBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.nonSteamAppIconCacheFolderPathBox.Filter = null;
			this.nonSteamAppIconCacheFolderPathBox.FilterIndex = 0;
			this.nonSteamAppIconCacheFolderPathBox.InitialDirectory = null;
			this.nonSteamAppIconCacheFolderPathBox.Location = new System.Drawing.Point(12, 34);
			this.nonSteamAppIconCacheFolderPathBox.MinimumSize = new System.Drawing.Size(0, 71);
			this.nonSteamAppIconCacheFolderPathBox.MultiSelect = false;
			this.nonSteamAppIconCacheFolderPathBox.Name = "nonSteamAppIconCacheFolderPathBox";
			this.nonSteamAppIconCacheFolderPathBox.Path = null;
			this.nonSteamAppIconCacheFolderPathBox.PathOpenType = SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathOpenType.Open;
			this.nonSteamAppIconCacheFolderPathBox.PathType = SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathType.Directory;
			this.nonSteamAppIconCacheFolderPathBox.Size = new System.Drawing.Size(456, 71);
			this.nonSteamAppIconCacheFolderPathBox.TabIndex = 9;
			this.nonSteamAppIconCacheFolderPathBox.Title = "Non-Steam App Icon Cache Folder";
			// 
			// steamFolderPathBox
			// 
			this.steamFolderPathBox.CheckDirectoryExists = false;
			this.steamFolderPathBox.CheckFileExists = false;
			this.steamFolderPathBox.DefaultPath = null;
			this.steamFolderPathBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.steamFolderPathBox.Filter = null;
			this.steamFolderPathBox.FilterIndex = 0;
			this.steamFolderPathBox.InitialDirectory = null;
			this.steamFolderPathBox.Location = new System.Drawing.Point(12, 34);
			this.steamFolderPathBox.MinimumSize = new System.Drawing.Size(0, 71);
			this.steamFolderPathBox.MultiSelect = false;
			this.steamFolderPathBox.Name = "steamFolderPathBox";
			this.steamFolderPathBox.Path = null;
			this.steamFolderPathBox.PathOpenType = SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathOpenType.Open;
			this.steamFolderPathBox.PathType = SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.PathType.Directory;
			this.steamFolderPathBox.Size = new System.Drawing.Size(456, 71);
			this.steamFolderPathBox.TabIndex = 4;
			this.steamFolderPathBox.Title = "Steam Folder";
			// 
			// EnvironmentPathsOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.localizationGroupBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.overlaysGroupBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.overlaysFolderGroupBox);
			this.Controls.Add(this.spacingLabel3);
			this.Controls.Add(this.shortcutIconCacheFolderGroupBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.steamFolderGroupBox);
			this.Controls.Add(this.spacingLabel2);
			this.Controls.Add(this.steamUserIDGroupBox);
			this.MinimumSize = new System.Drawing.Size(300, 320);
			this.Name = "EnvironmentPathsOptionsPanel";
			this.Size = new System.Drawing.Size(480, 914);
			this.steamFolderGroupBox.ResumeLayout(false);
			this.steamUserIDGroupBox.ResumeLayout(false);
			this.shortcutIconCacheFolderGroupBox.ResumeLayout(false);
			this.overlaysFolderGroupBox.ResumeLayout(false);
			this.overlaysGroupBox.ResumeLayout(false);
			this.localizationGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.GroupBox steamFolderGroupBox;
		private System.Windows.Forms.Button steamFolderCreateButton;
		private System.Windows.Forms.Label spacingLabel1;
		private UserControls.PathBox steamFolderPathBox;
		private System.Windows.Forms.Label spacingLabel2;
		private System.Windows.Forms.GroupBox steamUserIDGroupBox;
		private System.Windows.Forms.ComboBox steamUserIDComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox shortcutIconCacheFolderGroupBox;
		private UserControls.PathBox nonSteamAppIconCacheFolderPathBox;
		private System.Windows.Forms.Label spacingLabel3;
		private System.Windows.Forms.GroupBox overlaysFolderGroupBox;
		private UserControls.PathBox backupFolderPathBox;
		private System.Windows.Forms.GroupBox overlaysGroupBox;
		private UserControls.PathBox overlaysFolderPathBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox localizationGroupBox;
		private UserControls.PathBox localizationFolderPathBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button overlaysFolderCreateButton;
		private System.Windows.Forms.Label label4;
	}
}
