
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	partial class InterfaceDetailsPreviewOptionsPanel
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
			this.optionsGroupBox = new System.Windows.Forms.GroupBox();
			this.tabPageBackgroundColorPanel = new System.Windows.Forms.Panel();
			this.dialogBackgroundColorColorBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox();
			this.dialogBackgroundColorLabel = new System.Windows.Forms.Label();
			this.actionsGroupBox = new System.Windows.Forms.GroupBox();
			this.mouseActionGroupBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.MouseActionGroupBox();
			this.spacingLabel1 = new System.Windows.Forms.Label();
			this.optionsGroupBox.SuspendLayout();
			this.tabPageBackgroundColorPanel.SuspendLayout();
			this.actionsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// optionsGroupBox
			// 
			this.optionsGroupBox.Controls.Add(this.tabPageBackgroundColorPanel);
			this.optionsGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.optionsGroupBox.Location = new System.Drawing.Point(0, 0);
			this.optionsGroupBox.Name = "optionsGroupBox";
			this.optionsGroupBox.Size = new System.Drawing.Size(559, 125);
			this.optionsGroupBox.TabIndex = 5;
			this.optionsGroupBox.TabStop = false;
			this.optionsGroupBox.Text = "&Options";
			// 
			// tabPageBackgroundColorPanel
			// 
			this.tabPageBackgroundColorPanel.AutoSize = true;
			this.tabPageBackgroundColorPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tabPageBackgroundColorPanel.Controls.Add(this.dialogBackgroundColorColorBox);
			this.tabPageBackgroundColorPanel.Controls.Add(this.dialogBackgroundColorLabel);
			this.tabPageBackgroundColorPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabPageBackgroundColorPanel.Location = new System.Drawing.Point(3, 25);
			this.tabPageBackgroundColorPanel.Name = "tabPageBackgroundColorPanel";
			this.tabPageBackgroundColorPanel.Padding = new System.Windows.Forms.Padding(12);
			this.tabPageBackgroundColorPanel.Size = new System.Drawing.Size(553, 94);
			this.tabPageBackgroundColorPanel.TabIndex = 1;
			// 
			// dialogBackgroundColorColorBox
			// 
			this.dialogBackgroundColorColorBox.DataMember = null;
			this.dialogBackgroundColorColorBox.DataSource = null;
			this.dialogBackgroundColorColorBox.DefaultColor = System.Drawing.SystemColors.Control;
			this.dialogBackgroundColorColorBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.dialogBackgroundColorColorBox.Location = new System.Drawing.Point(12, 37);
			this.dialogBackgroundColorColorBox.Name = "dialogBackgroundColorColorBox";
			this.dialogBackgroundColorColorBox.Size = new System.Drawing.Size(529, 45);
			this.dialogBackgroundColorColorBox.TabIndex = 1;
			// 
			// dialogBackgroundColorLabel
			// 
			this.dialogBackgroundColorLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.dialogBackgroundColorLabel.Location = new System.Drawing.Point(12, 12);
			this.dialogBackgroundColorLabel.Name = "dialogBackgroundColorLabel";
			this.dialogBackgroundColorLabel.Size = new System.Drawing.Size(529, 25);
			this.dialogBackgroundColorLabel.TabIndex = 0;
			this.dialogBackgroundColorLabel.Text = "&Background Color:";
			// 
			// actionsGroupBox
			// 
			this.actionsGroupBox.AutoSize = true;
			this.actionsGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.actionsGroupBox.Controls.Add(this.mouseActionGroupBox);
			this.actionsGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.actionsGroupBox.Location = new System.Drawing.Point(0, 148);
			this.actionsGroupBox.Name = "actionsGroupBox";
			this.actionsGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.actionsGroupBox.Size = new System.Drawing.Size(559, 513);
			this.actionsGroupBox.TabIndex = 6;
			this.actionsGroupBox.TabStop = false;
			this.actionsGroupBox.Text = "&Actions";
			// 
			// mouseActionGroupBox
			// 
			this.mouseActionGroupBox.AutoSize = true;
			this.mouseActionGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.mouseActionGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.mouseActionGroupBox.Location = new System.Drawing.Point(12, 34);
			this.mouseActionGroupBox.MouseLeftClickActionVisible = true;
			this.mouseActionGroupBox.MouseLeftDoubleClickActionVisible = true;
			this.mouseActionGroupBox.MouseMiddleClickActionVisible = true;
			this.mouseActionGroupBox.MouseMiddleDoubleClickActionVisible = true;
			this.mouseActionGroupBox.MouseRightClickActionVisible = true;
			this.mouseActionGroupBox.MouseRightDoubleClickActionVisible = true;
			this.mouseActionGroupBox.Name = "mouseActionGroupBox";
			this.mouseActionGroupBox.Size = new System.Drawing.Size(535, 467);
			this.mouseActionGroupBox.TabIndex = 1;
			// 
			// spacingLabel1
			// 
			this.spacingLabel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacingLabel1.Location = new System.Drawing.Point(0, 125);
			this.spacingLabel1.Name = "spacingLabel1";
			this.spacingLabel1.Size = new System.Drawing.Size(559, 23);
			this.spacingLabel1.TabIndex = 8;
			// 
			// EnvironmentDetailsPreviewOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.actionsGroupBox);
			this.Controls.Add(this.spacingLabel1);
			this.Controls.Add(this.optionsGroupBox);
			this.Name = "EnvironmentDetailsPreviewOptionsPanel";
			this.Size = new System.Drawing.Size(559, 660);
			this.optionsGroupBox.ResumeLayout(false);
			this.optionsGroupBox.PerformLayout();
			this.tabPageBackgroundColorPanel.ResumeLayout(false);
			this.actionsGroupBox.ResumeLayout(false);
			this.actionsGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox optionsGroupBox;
		private System.Windows.Forms.Panel tabPageBackgroundColorPanel;
		private UserControls.ColorBox dialogBackgroundColorColorBox;
		private System.Windows.Forms.Label dialogBackgroundColorLabel;
		private System.Windows.Forms.GroupBox actionsGroupBox;
		private System.Windows.Forms.Label spacingLabel1;
		private UserControls.MouseActionGroupBox mouseActionGroupBox;
	}
}
