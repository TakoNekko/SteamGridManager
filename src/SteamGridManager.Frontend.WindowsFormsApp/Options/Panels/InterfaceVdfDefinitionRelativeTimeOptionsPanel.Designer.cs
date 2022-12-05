
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	partial class InterfaceVdfDefinitionRelativeTimeOptionsPanel
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
			this.formatLabel = new System.Windows.Forms.Label();
			this.pathsLabel = new System.Windows.Forms.Label();
			this.pathsListBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.StringCollectionListBox();
			this.colorButton = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox();
			this.colorLabel = new System.Windows.Forms.Label();
			this.formatTextBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.DateTimeFormatBox();
			this.spacingLabel1 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.sapcingLabel2 = new System.Windows.Forms.Label();
			this.cultureComboBox = new System.Windows.Forms.ComboBox();
			this.cultureLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// formatLabel
			// 
			this.formatLabel.AutoSize = true;
			this.formatLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.formatLabel.Location = new System.Drawing.Point(0, 173);
			this.formatLabel.Name = "formatLabel";
			this.formatLabel.Size = new System.Drawing.Size(79, 25);
			this.formatLabel.TabIndex = 6;
			this.formatLabel.Text = "&Format:";
			// 
			// pathsLabel
			// 
			this.pathsLabel.AutoSize = true;
			this.pathsLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.pathsLabel.Location = new System.Drawing.Point(0, 480);
			this.pathsLabel.Name = "pathsLabel";
			this.pathsLabel.Size = new System.Drawing.Size(68, 25);
			this.pathsLabel.TabIndex = 9;
			this.pathsLabel.Text = "Paths:";
			// 
			// pathsListBox
			// 
			this.pathsListBox.CanEdit = false;
			this.pathsListBox.DataMember = null;
			this.pathsListBox.DataSource = null;
			this.pathsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pathsListBox.Location = new System.Drawing.Point(0, 505);
			this.pathsListBox.Name = "pathsListBox";
			this.pathsListBox.Size = new System.Drawing.Size(520, 35);
			this.pathsListBox.TabIndex = 10;
			// 
			// colorButton
			// 
			this.colorButton.DataMember = null;
			this.colorButton.DataSource = null;
			this.colorButton.DefaultColor = System.Drawing.Color.Black;
			this.colorButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.colorButton.Location = new System.Drawing.Point(0, 25);
			this.colorButton.Name = "colorButton";
			this.colorButton.Size = new System.Drawing.Size(520, 45);
			this.colorButton.TabIndex = 1;
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
			// formatTextBox
			// 
			this.formatTextBox.CultureInfo = null;
			this.formatTextBox.DataMember = null;
			this.formatTextBox.DataSource = null;
			this.formatTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.formatTextBox.Location = new System.Drawing.Point(0, 198);
			this.formatTextBox.Name = "formatTextBox";
			this.formatTextBox.Padding = new System.Windows.Forms.Padding(12);
			this.formatTextBox.Size = new System.Drawing.Size(520, 259);
			this.formatTextBox.TabIndex = 7;
			this.formatTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.formatTextBox_Validating);
			// 
			// spacingLabel1
			// 
			this.spacingLabel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacingLabel1.Location = new System.Drawing.Point(0, 70);
			this.spacingLabel1.Name = "spacingLabel1";
			this.spacingLabel1.Size = new System.Drawing.Size(520, 23);
			this.spacingLabel1.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 457);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(520, 23);
			this.label1.TabIndex = 8;
			// 
			// sapcingLabel2
			// 
			this.sapcingLabel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.sapcingLabel2.Location = new System.Drawing.Point(0, 150);
			this.sapcingLabel2.Name = "sapcingLabel2";
			this.sapcingLabel2.Size = new System.Drawing.Size(520, 23);
			this.sapcingLabel2.TabIndex = 5;
			// 
			// cultureComboBox
			// 
			this.cultureComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.cultureComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cultureComboBox.FormattingEnabled = true;
			this.cultureComboBox.Location = new System.Drawing.Point(0, 118);
			this.cultureComboBox.Name = "cultureComboBox";
			this.cultureComboBox.Size = new System.Drawing.Size(520, 32);
			this.cultureComboBox.TabIndex = 4;
			this.cultureComboBox.SelectedIndexChanged += new System.EventHandler(this.cultureComboBox_SelectedIndexChanged);
			// 
			// cultureLabel
			// 
			this.cultureLabel.AutoSize = true;
			this.cultureLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.cultureLabel.Location = new System.Drawing.Point(0, 93);
			this.cultureLabel.Name = "cultureLabel";
			this.cultureLabel.Size = new System.Drawing.Size(81, 25);
			this.cultureLabel.TabIndex = 3;
			this.cultureLabel.Text = "&Culture:";
			// 
			// InterfaceVdfDefinitionRelativeTimeOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pathsListBox);
			this.Controls.Add(this.pathsLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.formatTextBox);
			this.Controls.Add(this.formatLabel);
			this.Controls.Add(this.sapcingLabel2);
			this.Controls.Add(this.cultureComboBox);
			this.Controls.Add(this.cultureLabel);
			this.Controls.Add(this.spacingLabel1);
			this.Controls.Add(this.colorButton);
			this.Controls.Add(this.colorLabel);
			this.MinimumSize = new System.Drawing.Size(300, 540);
			this.Name = "InterfaceVdfDefinitionRelativeTimeOptionsPanel";
			this.Size = new System.Drawing.Size(520, 540);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label formatLabel;
		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.DateTimeFormatBox formatTextBox;
		private System.Windows.Forms.Label pathsLabel;
		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.StringCollectionListBox pathsListBox;
		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox colorButton;
		private System.Windows.Forms.Label colorLabel;
		private System.Windows.Forms.Label spacingLabel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label sapcingLabel2;
		private System.Windows.Forms.ComboBox cultureComboBox;
		private System.Windows.Forms.Label cultureLabel;
	}
}
