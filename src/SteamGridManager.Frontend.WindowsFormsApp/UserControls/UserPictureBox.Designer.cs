
namespace SteamGridManager.Frontend.WindowsFormsApp.UserControls
{
	partial class UserPictureBox
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
			this.components = new System.ComponentModel.Container();
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.innerPanel = new System.Windows.Forms.Panel();
			this.pictureBoxContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.previewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openWithToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.editWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editWithToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.useWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.defaultProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.viewInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.fromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fromURLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setFromClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.setBackgroundColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.setAsCustomIconToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.overlayPictureBox = new System.Windows.Forms.PictureBox();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.errorLabel = new System.Windows.Forms.Label();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.placeholderLabel = new System.Windows.Forms.Label();
			this.fileSystemTimer = new System.Windows.Forms.Timer(this.components);
			this.fileSystemWatcher = new System.IO.FileSystemWatcher();
			this.groupBox.SuspendLayout();
			this.innerPanel.SuspendLayout();
			this.pictureBoxContextMenuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.overlayPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox
			// 
			this.groupBox.Controls.Add(this.innerPanel);
			this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox.Location = new System.Drawing.Point(0, 0);
			this.groupBox.Name = "groupBox";
			this.groupBox.Size = new System.Drawing.Size(381, 201);
			this.groupBox.TabIndex = 0;
			this.groupBox.TabStop = false;
			// 
			// innerPanel
			// 
			this.innerPanel.AllowDrop = true;
			this.innerPanel.ContextMenuStrip = this.pictureBoxContextMenuStrip;
			this.innerPanel.Controls.Add(this.overlayPictureBox);
			this.innerPanel.Controls.Add(this.pictureBox);
			this.innerPanel.Controls.Add(this.errorLabel);
			this.innerPanel.Controls.Add(this.progressBar);
			this.innerPanel.Controls.Add(this.placeholderLabel);
			this.innerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.innerPanel.Location = new System.Drawing.Point(3, 25);
			this.innerPanel.Name = "innerPanel";
			this.innerPanel.Size = new System.Drawing.Size(375, 173);
			this.innerPanel.TabIndex = 4;
			this.innerPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.innerPanel_DragDrop);
			this.innerPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.innerPanel_DragEnter);
			this.innerPanel.DragOver += new System.Windows.Forms.DragEventHandler(this.innerPanel_DragOver);
			this.innerPanel.DragLeave += new System.EventHandler(this.innerPanel_DragLeave);
			this.innerPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.innerPanel_MouseClick);
			this.innerPanel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.innerPanel_MouseDoubleClick);
			// 
			// pictureBoxContextMenuStrip
			// 
			this.pictureBoxContextMenuStrip.ImageScalingSize = new System.Drawing.Size(28, 28);
			this.pictureBoxContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.previewToolStripMenuItem,
            this.openWithToolStripMenuItem,
            this.editWithToolStripMenuItem,
            this.useWithToolStripMenuItem,
            this.viewInExplorerToolStripMenuItem,
            this.toolStripSeparator1,
            this.fromFileToolStripMenuItem,
            this.fromURLToolStripMenuItem,
            this.setFromClipboardToolStripMenuItem,
            this.toolStripSeparator2,
            this.copyToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.toolStripSeparator4,
            this.setBackgroundColorToolStripMenuItem,
            this.toolStripSeparator3,
            this.setAsCustomIconToolStripMenuItem});
			this.pictureBoxContextMenuStrip.Name = "pictureBoxContextMenuStrip";
			this.pictureBoxContextMenuStrip.Size = new System.Drawing.Size(302, 460);
			// 
			// previewToolStripMenuItem
			// 
			this.previewToolStripMenuItem.Enabled = false;
			this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
			this.previewToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.previewToolStripMenuItem.Text = "&Preview...";
			this.previewToolStripMenuItem.Click += new System.EventHandler(this.previewToolStripMenuItem_Click);
			// 
			// openWithToolStripMenuItem
			// 
			this.openWithToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openWithToolStripSeparator});
			this.openWithToolStripMenuItem.Enabled = false;
			this.openWithToolStripMenuItem.Name = "openWithToolStripMenuItem";
			this.openWithToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.openWithToolStripMenuItem.Text = "&Open with";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(284, 40);
			this.openToolStripMenuItem.Text = "&Default Program";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// openWithToolStripSeparator
			// 
			this.openWithToolStripSeparator.Name = "openWithToolStripSeparator";
			this.openWithToolStripSeparator.Size = new System.Drawing.Size(281, 6);
			this.openWithToolStripSeparator.Visible = false;
			// 
			// editWithToolStripMenuItem
			// 
			this.editWithToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.editWithToolStripSeparator});
			this.editWithToolStripMenuItem.Enabled = false;
			this.editWithToolStripMenuItem.Name = "editWithToolStripMenuItem";
			this.editWithToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.editWithToolStripMenuItem.Text = "&Edit with";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(284, 40);
			this.editToolStripMenuItem.Text = "&Default Program";
			this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
			// 
			// editWithToolStripSeparator
			// 
			this.editWithToolStripSeparator.Name = "editWithToolStripSeparator";
			this.editWithToolStripSeparator.Size = new System.Drawing.Size(281, 6);
			this.editWithToolStripSeparator.Visible = false;
			// 
			// useWithToolStripMenuItem
			// 
			this.useWithToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defaultProgramToolStripMenuItem,
            this.toolStripSeparator5});
			this.useWithToolStripMenuItem.Name = "useWithToolStripMenuItem";
			this.useWithToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.useWithToolStripMenuItem.Text = "&Use with";
			// 
			// defaultProgramToolStripMenuItem
			// 
			this.defaultProgramToolStripMenuItem.Name = "defaultProgramToolStripMenuItem";
			this.defaultProgramToolStripMenuItem.Size = new System.Drawing.Size(284, 40);
			this.defaultProgramToolStripMenuItem.Text = "&Default Program";
			this.defaultProgramToolStripMenuItem.Click += new System.EventHandler(this.useToolStripMenuItem_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(281, 6);
			// 
			// viewInExplorerToolStripMenuItem
			// 
			this.viewInExplorerToolStripMenuItem.Enabled = false;
			this.viewInExplorerToolStripMenuItem.Name = "viewInExplorerToolStripMenuItem";
			this.viewInExplorerToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.viewInExplorerToolStripMenuItem.Text = "View in &Explorer...";
			this.viewInExplorerToolStripMenuItem.Click += new System.EventHandler(this.viewInExplorerToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(298, 6);
			// 
			// fromFileToolStripMenuItem
			// 
			this.fromFileToolStripMenuItem.Name = "fromFileToolStripMenuItem";
			this.fromFileToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.fromFileToolStripMenuItem.Text = "Set from &File...";
			this.fromFileToolStripMenuItem.Click += new System.EventHandler(this.fromFileToolStripMenuItem_Click);
			// 
			// fromURLToolStripMenuItem
			// 
			this.fromURLToolStripMenuItem.Name = "fromURLToolStripMenuItem";
			this.fromURLToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.fromURLToolStripMenuItem.Text = "Set from &URL...";
			this.fromURLToolStripMenuItem.Click += new System.EventHandler(this.fromURLToolStripMenuItem_Click);
			// 
			// setFromClipboardToolStripMenuItem
			// 
			this.setFromClipboardToolStripMenuItem.Name = "setFromClipboardToolStripMenuItem";
			this.setFromClipboardToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.setFromClipboardToolStripMenuItem.Text = "Set from &Clipboard";
			this.setFromClipboardToolStripMenuItem.Click += new System.EventHandler(this.setFromClipboardToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(298, 6);
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Enabled = false;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.copyToolStripMenuItem.Text = "&Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Enabled = false;
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.removeToolStripMenuItem.Text = "&Delete";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(298, 6);
			// 
			// setBackgroundColorToolStripMenuItem
			// 
			this.setBackgroundColorToolStripMenuItem.Name = "setBackgroundColorToolStripMenuItem";
			this.setBackgroundColorToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.setBackgroundColorToolStripMenuItem.Text = "Set Background Color...";
			this.setBackgroundColorToolStripMenuItem.Click += new System.EventHandler(this.setBackgroundColorToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(298, 6);
			this.toolStripSeparator3.Visible = false;
			// 
			// setAsCustomIconToolStripMenuItem
			// 
			this.setAsCustomIconToolStripMenuItem.Name = "setAsCustomIconToolStripMenuItem";
			this.setAsCustomIconToolStripMenuItem.Size = new System.Drawing.Size(301, 36);
			this.setAsCustomIconToolStripMenuItem.Text = "Set as Custom &Icon";
			this.setAsCustomIconToolStripMenuItem.Visible = false;
			this.setAsCustomIconToolStripMenuItem.Click += new System.EventHandler(this.setAsCustomIconToolStripMenuItem_Click);
			// 
			// overlayPictureBox
			// 
			this.overlayPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.overlayPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.overlayPictureBox.Location = new System.Drawing.Point(0, 0);
			this.overlayPictureBox.Name = "overlayPictureBox";
			this.overlayPictureBox.Size = new System.Drawing.Size(375, 163);
			this.overlayPictureBox.TabIndex = 0;
			this.overlayPictureBox.TabStop = false;
			this.overlayPictureBox.Visible = false;
			// 
			// pictureBox
			// 
			this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox.Location = new System.Drawing.Point(0, 0);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(375, 163);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			this.pictureBox.Visible = false;
			this.pictureBox.BackgroundImageChanged += new System.EventHandler(this.pictureBox_BackgroundImageChanged);
			this.pictureBox.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.pictureBox_QueryContinueDrag);
			this.pictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseClick);
			this.pictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDoubleClick);
			this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
			// 
			// errorLabel
			// 
			this.errorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.errorLabel.ForeColor = System.Drawing.Color.Red;
			this.errorLabel.Location = new System.Drawing.Point(0, 0);
			this.errorLabel.Name = "errorLabel";
			this.errorLabel.Size = new System.Drawing.Size(375, 163);
			this.errorLabel.TabIndex = 3;
			this.errorLabel.Text = "Error";
			this.errorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.errorLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.errorLabel_MouseClick);
			this.errorLabel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.errorLabel_MouseDoubleClick);
			// 
			// progressBar
			// 
			this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.progressBar.Location = new System.Drawing.Point(0, 163);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(375, 10);
			this.progressBar.TabIndex = 1;
			this.progressBar.Visible = false;
			// 
			// placeholderLabel
			// 
			this.placeholderLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.placeholderLabel.Location = new System.Drawing.Point(0, 0);
			this.placeholderLabel.Name = "placeholderLabel";
			this.placeholderLabel.Size = new System.Drawing.Size(375, 173);
			this.placeholderLabel.TabIndex = 2;
			this.placeholderLabel.Text = "Double-click or drop an image file here.";
			this.placeholderLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.placeholderLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.placeholderLabel_MouseClick);
			this.placeholderLabel.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.placeholderLabel_MouseDoubleClick);
			// 
			// fileSystemTimer
			// 
			this.fileSystemTimer.Interval = 1000;
			this.fileSystemTimer.Tick += new System.EventHandler(this.fileSystemTimer_Tick);
			// 
			// fileSystemWatcher
			// 
			this.fileSystemWatcher.SynchronizingObject = this;
			this.fileSystemWatcher.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher_Changed);
			// 
			// UserPictureBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox);
			this.Name = "UserPictureBox";
			this.Size = new System.Drawing.Size(381, 201);
			this.groupBox.ResumeLayout(false);
			this.innerPanel.ResumeLayout(false);
			this.pictureBoxContextMenuStrip.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.overlayPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.PictureBox overlayPictureBox;
		private System.Windows.Forms.ContextMenuStrip pictureBoxContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem fromFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fromURLToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.ToolStripMenuItem viewInExplorerToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.Timer fileSystemTimer;
		private System.Windows.Forms.Label placeholderLabel;
		private System.Windows.Forms.Label errorLabel;
		private System.Windows.Forms.Panel innerPanel;
		private System.Windows.Forms.ToolStripMenuItem previewToolStripMenuItem;
		private System.IO.FileSystemWatcher fileSystemWatcher;
		private System.Windows.Forms.ToolStripMenuItem openWithToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editWithToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator openWithToolStripSeparator;
		private System.Windows.Forms.ToolStripSeparator editWithToolStripSeparator;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem setAsCustomIconToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setFromClipboardToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setBackgroundColorToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem useWithToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem defaultProgramToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
	}
}
