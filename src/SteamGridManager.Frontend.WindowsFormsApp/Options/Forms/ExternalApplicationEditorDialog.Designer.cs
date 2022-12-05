
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Forms
{
	partial class ExternalApplicationEditorDialog
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
			this.argumentsInsertButton = new System.Windows.Forms.Button();
			this.locationBrowseButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.typeComboBox = new System.Windows.Forms.ComboBox();
			this.typeLabel = new System.Windows.Forms.Label();
			this.startingDirectoryTextBox = new System.Windows.Forms.TextBox();
			this.startingDirectoryLabel = new System.Windows.Forms.Label();
			this.argumentsTextBox = new System.Windows.Forms.TextBox();
			this.argumentsLabel = new System.Windows.Forms.Label();
			this.locationTextBox = new System.Windows.Forms.TextBox();
			this.locationLabel = new System.Windows.Forms.Label();
			this.titleTextBox = new System.Windows.Forms.TextBox();
			this.titleLabel = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.assetTypeNameListView = new System.Windows.Forms.ListView();
			this.assetTypeValueColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.assetTypeColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.assetTypeNameContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label17 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.panel3 = new System.Windows.Forms.Panel();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.panel4 = new System.Windows.Forms.Panel();
			this.argumentsInsertContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.assetTypeNameContextMenuStrip.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// argumentsInsertButton
			// 
			this.argumentsInsertButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.argumentsInsertButton.Location = new System.Drawing.Point(12, 195);
			this.argumentsInsertButton.Name = "argumentsInsertButton";
			this.argumentsInsertButton.Size = new System.Drawing.Size(176, 35);
			this.argumentsInsertButton.TabIndex = 7;
			this.argumentsInsertButton.Text = "&Insert...";
			this.argumentsInsertButton.UseVisualStyleBackColor = true;
			this.argumentsInsertButton.Click += new System.EventHandler(this.argumentsInsertButton_Click);
			// 
			// locationBrowseButton
			// 
			this.locationBrowseButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.locationBrowseButton.Location = new System.Drawing.Point(12, 110);
			this.locationBrowseButton.Name = "locationBrowseButton";
			this.locationBrowseButton.Size = new System.Drawing.Size(176, 35);
			this.locationBrowseButton.TabIndex = 4;
			this.locationBrowseButton.Text = "&Browse...";
			this.locationBrowseButton.UseVisualStyleBackColor = true;
			this.locationBrowseButton.Click += new System.EventHandler(this.locationBrowseButton_Click);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Enabled = false;
			this.okButton.Location = new System.Drawing.Point(430, 13);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(176, 35);
			this.okButton.TabIndex = 13;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(612, 13);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(176, 35);
			this.cancelButton.TabIndex = 12;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// typeComboBox
			// 
			this.typeComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.typeComboBox.FormattingEnabled = true;
			this.typeComboBox.Location = new System.Drawing.Point(12, 345);
			this.typeComboBox.Name = "typeComboBox";
			this.typeComboBox.Size = new System.Drawing.Size(576, 32);
			this.typeComboBox.TabIndex = 11;
			// 
			// typeLabel
			// 
			this.typeLabel.AutoSize = true;
			this.typeLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.typeLabel.Location = new System.Drawing.Point(12, 320);
			this.typeLabel.Name = "typeLabel";
			this.typeLabel.Size = new System.Drawing.Size(63, 25);
			this.typeLabel.TabIndex = 10;
			this.typeLabel.Text = "&Type:";
			// 
			// startingDirectoryTextBox
			// 
			this.startingDirectoryTextBox.AllowDrop = true;
			this.startingDirectoryTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.startingDirectoryTextBox.Location = new System.Drawing.Point(12, 268);
			this.startingDirectoryTextBox.Name = "startingDirectoryTextBox";
			this.startingDirectoryTextBox.Size = new System.Drawing.Size(576, 29);
			this.startingDirectoryTextBox.TabIndex = 9;
			this.startingDirectoryTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.startingDirectoryTextBox_DragDrop);
			this.startingDirectoryTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.startingDirectoryTextBox_DragEnter);
			// 
			// startingDirectoryLabel
			// 
			this.startingDirectoryLabel.AutoSize = true;
			this.startingDirectoryLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.startingDirectoryLabel.Location = new System.Drawing.Point(12, 243);
			this.startingDirectoryLabel.Name = "startingDirectoryLabel";
			this.startingDirectoryLabel.Size = new System.Drawing.Size(167, 25);
			this.startingDirectoryLabel.TabIndex = 8;
			this.startingDirectoryLabel.Text = "&Starting Directory:";
			// 
			// argumentsTextBox
			// 
			this.argumentsTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.argumentsTextBox.Location = new System.Drawing.Point(12, 191);
			this.argumentsTextBox.Name = "argumentsTextBox";
			this.argumentsTextBox.Size = new System.Drawing.Size(576, 29);
			this.argumentsTextBox.TabIndex = 6;
			// 
			// argumentsLabel
			// 
			this.argumentsLabel.AutoSize = true;
			this.argumentsLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.argumentsLabel.Location = new System.Drawing.Point(12, 166);
			this.argumentsLabel.Name = "argumentsLabel";
			this.argumentsLabel.Size = new System.Drawing.Size(113, 25);
			this.argumentsLabel.TabIndex = 5;
			this.argumentsLabel.Text = "&Arguments:";
			// 
			// locationTextBox
			// 
			this.locationTextBox.AllowDrop = true;
			this.locationTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.locationTextBox.Location = new System.Drawing.Point(12, 114);
			this.locationTextBox.Name = "locationTextBox";
			this.locationTextBox.Size = new System.Drawing.Size(576, 29);
			this.locationTextBox.TabIndex = 3;
			this.locationTextBox.TextChanged += new System.EventHandler(this.locationTextBox_TextChanged);
			this.locationTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.locationTextBox_DragDrop);
			this.locationTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.locationTextBox_DragEnter);
			// 
			// locationLabel
			// 
			this.locationLabel.AutoSize = true;
			this.locationLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.locationLabel.Location = new System.Drawing.Point(12, 89);
			this.locationLabel.Name = "locationLabel";
			this.locationLabel.Size = new System.Drawing.Size(92, 25);
			this.locationLabel.TabIndex = 2;
			this.locationLabel.Text = "&Location:";
			// 
			// titleTextBox
			// 
			this.titleTextBox.AllowDrop = true;
			this.titleTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.titleTextBox.Location = new System.Drawing.Point(12, 37);
			this.titleTextBox.Name = "titleTextBox";
			this.titleTextBox.Size = new System.Drawing.Size(576, 29);
			this.titleTextBox.TabIndex = 1;
			this.titleTextBox.TextChanged += new System.EventHandler(this.titleTextBox_TextChanged);
			this.titleTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.titleTextBox_DragDrop);
			this.titleTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.titleTextBox_DragEnter);
			// 
			// titleLabel
			// 
			this.titleLabel.AutoSize = true;
			this.titleLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.titleLabel.Location = new System.Drawing.Point(12, 12);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(55, 25);
			this.titleLabel.TabIndex = 0;
			this.titleLabel.Text = "&Title:";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.panel2);
			this.panel1.Controls.Add(this.panel3);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(800, 659);
			this.panel1.TabIndex = 34;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.assetTypeNameListView);
			this.panel2.Controls.Add(this.label17);
			this.panel2.Controls.Add(this.label18);
			this.panel2.Controls.Add(this.typeComboBox);
			this.panel2.Controls.Add(this.typeLabel);
			this.panel2.Controls.Add(this.label12);
			this.panel2.Controls.Add(this.startingDirectoryTextBox);
			this.panel2.Controls.Add(this.startingDirectoryLabel);
			this.panel2.Controls.Add(this.label11);
			this.panel2.Controls.Add(this.argumentsTextBox);
			this.panel2.Controls.Add(this.argumentsLabel);
			this.panel2.Controls.Add(this.label10);
			this.panel2.Controls.Add(this.locationTextBox);
			this.panel2.Controls.Add(this.locationLabel);
			this.panel2.Controls.Add(this.label9);
			this.panel2.Controls.Add(this.titleTextBox);
			this.panel2.Controls.Add(this.titleLabel);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 0);
			this.panel2.Name = "panel2";
			this.panel2.Padding = new System.Windows.Forms.Padding(12);
			this.panel2.Size = new System.Drawing.Size(600, 659);
			this.panel2.TabIndex = 0;
			// 
			// assetTypeNameListView
			// 
			this.assetTypeNameListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.assetTypeValueColumnHeader,
            this.assetTypeColumnHeader});
			this.assetTypeNameListView.ContextMenuStrip = this.assetTypeNameContextMenuStrip;
			this.assetTypeNameListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.assetTypeNameListView.FullRowSelect = true;
			this.assetTypeNameListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.assetTypeNameListView.HideSelection = false;
			this.assetTypeNameListView.LabelEdit = true;
			this.assetTypeNameListView.Location = new System.Drawing.Point(12, 425);
			this.assetTypeNameListView.MultiSelect = false;
			this.assetTypeNameListView.Name = "assetTypeNameListView";
			this.assetTypeNameListView.Size = new System.Drawing.Size(576, 222);
			this.assetTypeNameListView.TabIndex = 41;
			this.assetTypeNameListView.UseCompatibleStateImageBehavior = false;
			this.assetTypeNameListView.View = System.Windows.Forms.View.Details;
			this.assetTypeNameListView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.assetTypeNameListView_AfterLabelEdit);
			this.assetTypeNameListView.SelectedIndexChanged += new System.EventHandler(this.assetTypeNameListView_SelectedIndexChanged);
			// 
			// assetTypeValueColumnHeader
			// 
			this.assetTypeValueColumnHeader.DisplayIndex = 1;
			this.assetTypeValueColumnHeader.Text = "Value";
			this.assetTypeValueColumnHeader.Width = 512;
			// 
			// assetTypeColumnHeader
			// 
			this.assetTypeColumnHeader.DisplayIndex = 0;
			this.assetTypeColumnHeader.Text = "Name";
			this.assetTypeColumnHeader.Width = 140;
			// 
			// assetTypeNameContextMenuStrip
			// 
			this.assetTypeNameContextMenuStrip.ImageScalingSize = new System.Drawing.Size(28, 28);
			this.assetTypeNameContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameToolStripMenuItem});
			this.assetTypeNameContextMenuStrip.Name = "assetTypeNameContextMenuStrip";
			this.assetTypeNameContextMenuStrip.Size = new System.Drawing.Size(196, 40);
			// 
			// renameToolStripMenuItem
			// 
			this.renameToolStripMenuItem.Enabled = false;
			this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
			this.renameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
			this.renameToolStripMenuItem.Size = new System.Drawing.Size(195, 36);
			this.renameToolStripMenuItem.Text = "&Rename";
			this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Dock = System.Windows.Forms.DockStyle.Top;
			this.label17.Location = new System.Drawing.Point(12, 400);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(185, 25);
			this.label17.TabIndex = 39;
			this.label17.Text = "Asset &Type Names:";
			// 
			// label18
			// 
			this.label18.Dock = System.Windows.Forms.DockStyle.Top;
			this.label18.Location = new System.Drawing.Point(12, 377);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(576, 23);
			this.label18.TabIndex = 40;
			// 
			// label12
			// 
			this.label12.Dock = System.Windows.Forms.DockStyle.Top;
			this.label12.Location = new System.Drawing.Point(12, 297);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(576, 23);
			this.label12.TabIndex = 38;
			// 
			// label11
			// 
			this.label11.Dock = System.Windows.Forms.DockStyle.Top;
			this.label11.Location = new System.Drawing.Point(12, 220);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(576, 23);
			this.label11.TabIndex = 37;
			// 
			// label10
			// 
			this.label10.Dock = System.Windows.Forms.DockStyle.Top;
			this.label10.Location = new System.Drawing.Point(12, 143);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(576, 23);
			this.label10.TabIndex = 36;
			// 
			// label9
			// 
			this.label9.Dock = System.Windows.Forms.DockStyle.Top;
			this.label9.Location = new System.Drawing.Point(12, 66);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(576, 23);
			this.label9.TabIndex = 35;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.label16);
			this.panel3.Controls.Add(this.label15);
			this.panel3.Controls.Add(this.label14);
			this.panel3.Controls.Add(this.label13);
			this.panel3.Controls.Add(this.label7);
			this.panel3.Controls.Add(this.label6);
			this.panel3.Controls.Add(this.argumentsInsertButton);
			this.panel3.Controls.Add(this.label5);
			this.panel3.Controls.Add(this.label4);
			this.panel3.Controls.Add(this.locationBrowseButton);
			this.panel3.Controls.Add(this.label8);
			this.panel3.Controls.Add(this.label3);
			this.panel3.Controls.Add(this.label2);
			this.panel3.Controls.Add(this.label1);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel3.Location = new System.Drawing.Point(600, 0);
			this.panel3.Name = "panel3";
			this.panel3.Padding = new System.Windows.Forms.Padding(12);
			this.panel3.Size = new System.Drawing.Size(200, 659);
			this.panel3.TabIndex = 1;
			// 
			// label16
			// 
			this.label16.Dock = System.Windows.Forms.DockStyle.Top;
			this.label16.Location = new System.Drawing.Point(12, 355);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(176, 25);
			this.label16.TabIndex = 45;
			// 
			// label15
			// 
			this.label15.Dock = System.Windows.Forms.DockStyle.Top;
			this.label15.Location = new System.Drawing.Point(12, 330);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(176, 25);
			this.label15.TabIndex = 44;
			// 
			// label14
			// 
			this.label14.Dock = System.Windows.Forms.DockStyle.Top;
			this.label14.Location = new System.Drawing.Point(12, 305);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(176, 25);
			this.label14.TabIndex = 43;
			// 
			// label13
			// 
			this.label13.Dock = System.Windows.Forms.DockStyle.Top;
			this.label13.Location = new System.Drawing.Point(12, 280);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(176, 25);
			this.label13.TabIndex = 42;
			// 
			// label7
			// 
			this.label7.Dock = System.Windows.Forms.DockStyle.Top;
			this.label7.Location = new System.Drawing.Point(12, 255);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(176, 25);
			this.label7.TabIndex = 40;
			// 
			// label6
			// 
			this.label6.Dock = System.Windows.Forms.DockStyle.Top;
			this.label6.Location = new System.Drawing.Point(12, 230);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(176, 25);
			this.label6.TabIndex = 39;
			// 
			// label5
			// 
			this.label5.Dock = System.Windows.Forms.DockStyle.Top;
			this.label5.Location = new System.Drawing.Point(12, 170);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(176, 25);
			this.label5.TabIndex = 38;
			// 
			// label4
			// 
			this.label4.Dock = System.Windows.Forms.DockStyle.Top;
			this.label4.Location = new System.Drawing.Point(12, 145);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(176, 25);
			this.label4.TabIndex = 37;
			// 
			// label8
			// 
			this.label8.Dock = System.Windows.Forms.DockStyle.Top;
			this.label8.Location = new System.Drawing.Point(12, 85);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(176, 25);
			this.label8.TabIndex = 41;
			// 
			// label3
			// 
			this.label3.Dock = System.Windows.Forms.DockStyle.Top;
			this.label3.Location = new System.Drawing.Point(12, 60);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(176, 25);
			this.label3.TabIndex = 36;
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(12, 35);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(176, 25);
			this.label2.TabIndex = 35;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(12, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(176, 23);
			this.label1.TabIndex = 34;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.cancelButton);
			this.panel4.Controls.Add(this.okButton);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel4.Location = new System.Drawing.Point(0, 659);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(800, 66);
			this.panel4.TabIndex = 35;
			// 
			// argumentsInsertContextMenuStrip
			// 
			this.argumentsInsertContextMenuStrip.ImageScalingSize = new System.Drawing.Size(28, 28);
			this.argumentsInsertContextMenuStrip.Name = "argumentsInsertContextMenuStrip";
			this.argumentsInsertContextMenuStrip.Size = new System.Drawing.Size(61, 4);
			// 
			// ExternalApplicationEditorDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(800, 725);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel4);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "ExternalApplicationEditorDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "External Application Editor";
			this.panel1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.assetTypeNameContextMenuStrip.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button argumentsInsertButton;
		private System.Windows.Forms.Button locationBrowseButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ComboBox typeComboBox;
		private System.Windows.Forms.Label typeLabel;
		private System.Windows.Forms.TextBox startingDirectoryTextBox;
		private System.Windows.Forms.Label startingDirectoryLabel;
		private System.Windows.Forms.TextBox argumentsTextBox;
		private System.Windows.Forms.Label argumentsLabel;
		private System.Windows.Forms.TextBox locationTextBox;
		private System.Windows.Forms.Label locationLabel;
		private System.Windows.Forms.TextBox titleTextBox;
		private System.Windows.Forms.Label titleLabel;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.ContextMenuStrip argumentsInsertContextMenuStrip;
		private System.Windows.Forms.ListView assetTypeNameListView;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.ColumnHeader assetTypeColumnHeader;
		private System.Windows.Forms.ColumnHeader assetTypeValueColumnHeader;
		private System.Windows.Forms.ContextMenuStrip assetTypeNameContextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
	}
}