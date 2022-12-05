
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	partial class EnvironmentDatabaseOptionsPanel
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
			this.appInfoDatabaseSettingBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.DatabaseSettingBox();
			this.shortcutsDatabaseSettingBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.DatabaseSettingBox();
			this.SuspendLayout();
			// 
			// appInfoDatabaseSettingBox
			// 
			this.appInfoDatabaseSettingBox.DatabasePath = null;
			this.appInfoDatabaseSettingBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.appInfoDatabaseSettingBox.Location = new System.Drawing.Point(0, 0);
			this.appInfoDatabaseSettingBox.Name = "appInfoDatabaseSettingBox";
			this.appInfoDatabaseSettingBox.Size = new System.Drawing.Size(532, 243);
			this.appInfoDatabaseSettingBox.TabIndex = 0;
			// 
			// shortcutsDatabaseSettingBox
			// 
			this.shortcutsDatabaseSettingBox.DatabasePath = null;
			this.shortcutsDatabaseSettingBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.shortcutsDatabaseSettingBox.Location = new System.Drawing.Point(0, 243);
			this.shortcutsDatabaseSettingBox.Name = "shortcutsDatabaseSettingBox";
			this.shortcutsDatabaseSettingBox.Size = new System.Drawing.Size(532, 243);
			this.shortcutsDatabaseSettingBox.TabIndex = 1;
			// 
			// EnvironmentDatabaseOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.shortcutsDatabaseSettingBox);
			this.Controls.Add(this.appInfoDatabaseSettingBox);
			this.Name = "EnvironmentDatabaseOptionsPanel";
			this.Size = new System.Drawing.Size(532, 495);
			this.ResumeLayout(false);

		}

		#endregion

		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.DatabaseSettingBox appInfoDatabaseSettingBox;
		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.DatabaseSettingBox shortcutsDatabaseSettingBox;
	}
}
