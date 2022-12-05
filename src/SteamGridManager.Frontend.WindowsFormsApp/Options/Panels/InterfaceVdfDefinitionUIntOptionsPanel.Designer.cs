
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	partial class InterfaceVdfDefinitionUIntOptionsPanel
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
			this.cultureLabel = new System.Windows.Forms.Label();
			this.cultureComboBox = new System.Windows.Forms.ComboBox();
			this.pathsLabel = new System.Windows.Forms.Label();
			this.pathsListBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.StringCollectionListBox();
			this.colorButton = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox();
			this.colorLabel = new System.Windows.Forms.Label();
			this.spacingLabel1 = new System.Windows.Forms.Label();
			this.sapcingLabel2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cultureLabel
			// 
			this.cultureLabel.AutoSize = true;
			this.cultureLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.cultureLabel.Location = new System.Drawing.Point(0, 93);
			this.cultureLabel.Name = "cultureLabel";
			this.cultureLabel.Size = new System.Drawing.Size(81, 25);
			this.cultureLabel.TabIndex = 2;
			this.cultureLabel.Text = "&Culture:";
			// 
			// cultureComboBox
			// 
			this.cultureComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.cultureComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cultureComboBox.FormattingEnabled = true;
			this.cultureComboBox.Location = new System.Drawing.Point(0, 118);
			this.cultureComboBox.Name = "cultureComboBox";
			this.cultureComboBox.Size = new System.Drawing.Size(600, 32);
			this.cultureComboBox.TabIndex = 3;
			// 
			// pathsLabel
			// 
			this.pathsLabel.AutoSize = true;
			this.pathsLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.pathsLabel.Location = new System.Drawing.Point(0, 173);
			this.pathsLabel.Name = "pathsLabel";
			this.pathsLabel.Size = new System.Drawing.Size(68, 25);
			this.pathsLabel.TabIndex = 4;
			this.pathsLabel.Text = "Paths:";
			// 
			// pathsListBox
			// 
			this.pathsListBox.CanEdit = false;
			this.pathsListBox.DataMember = null;
			this.pathsListBox.DataSource = null;
			this.pathsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pathsListBox.Location = new System.Drawing.Point(0, 198);
			this.pathsListBox.Name = "pathsListBox";
			this.pathsListBox.Size = new System.Drawing.Size(600, 282);
			this.pathsListBox.TabIndex = 5;
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
			// spacingLabel1
			// 
			this.spacingLabel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacingLabel1.Location = new System.Drawing.Point(0, 70);
			this.spacingLabel1.Name = "spacingLabel1";
			this.spacingLabel1.Size = new System.Drawing.Size(600, 23);
			this.spacingLabel1.TabIndex = 6;
			// 
			// sapcingLabel2
			// 
			this.sapcingLabel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.sapcingLabel2.Location = new System.Drawing.Point(0, 150);
			this.sapcingLabel2.Name = "sapcingLabel2";
			this.sapcingLabel2.Size = new System.Drawing.Size(600, 23);
			this.sapcingLabel2.TabIndex = 7;
			// 
			// InterfaceVdfDefinitionUIntOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pathsListBox);
			this.Controls.Add(this.pathsLabel);
			this.Controls.Add(this.sapcingLabel2);
			this.Controls.Add(this.cultureComboBox);
			this.Controls.Add(this.cultureLabel);
			this.Controls.Add(this.spacingLabel1);
			this.Controls.Add(this.colorButton);
			this.Controls.Add(this.colorLabel);
			this.MinimumSize = new System.Drawing.Size(300, 300);
			this.Name = "InterfaceVdfDefinitionUIntOptionsPanel";
			this.Size = new System.Drawing.Size(600, 480);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label cultureLabel;
		private System.Windows.Forms.ComboBox cultureComboBox;
		private System.Windows.Forms.Label pathsLabel;
		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.StringCollectionListBox pathsListBox;
		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox colorButton;
		private System.Windows.Forms.Label colorLabel;
		private System.Windows.Forms.Label spacingLabel1;
		private System.Windows.Forms.Label sapcingLabel2;
	}
}
