
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	partial class InterfaceVdfDefinitionObjectOptionsPanel
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
			this.colorButton = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox();
			this.colorLabel = new System.Windows.Forms.Label();
			this.pathsListBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.StringCollectionListBox();
			this.pathsLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
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
			this.colorButton.TabIndex = 8;
			// 
			// colorLabel
			// 
			this.colorLabel.AutoSize = true;
			this.colorLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.colorLabel.Location = new System.Drawing.Point(0, 0);
			this.colorLabel.Name = "colorLabel";
			this.colorLabel.Size = new System.Drawing.Size(65, 25);
			this.colorLabel.TabIndex = 7;
			this.colorLabel.Text = "&Color:";
			// 
			// pathsListBox
			// 
			this.pathsListBox.CanEdit = false;
			this.pathsListBox.DataMember = null;
			this.pathsListBox.DataSource = null;
			this.pathsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pathsListBox.Location = new System.Drawing.Point(0, 118);
			this.pathsListBox.Name = "pathsListBox";
			this.pathsListBox.Size = new System.Drawing.Size(600, 362);
			this.pathsListBox.TabIndex = 10;
			// 
			// pathsLabel
			// 
			this.pathsLabel.AutoSize = true;
			this.pathsLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.pathsLabel.Location = new System.Drawing.Point(0, 93);
			this.pathsLabel.Name = "pathsLabel";
			this.pathsLabel.Size = new System.Drawing.Size(68, 25);
			this.pathsLabel.TabIndex = 9;
			this.pathsLabel.Text = "Paths:";
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 70);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(600, 23);
			this.label1.TabIndex = 11;
			// 
			// EnvironmentVdfDefinitionObjectOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pathsListBox);
			this.Controls.Add(this.pathsLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.colorButton);
			this.Controls.Add(this.colorLabel);
			this.MinimumSize = new System.Drawing.Size(300, 300);
			this.Name = "EnvironmentVdfDefinitionObjectOptionsPanel";
			this.Size = new System.Drawing.Size(600, 480);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox colorButton;
		private System.Windows.Forms.Label colorLabel;
		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.StringCollectionListBox pathsListBox;
		private System.Windows.Forms.Label pathsLabel;
		private System.Windows.Forms.Label label1;
	}
}
