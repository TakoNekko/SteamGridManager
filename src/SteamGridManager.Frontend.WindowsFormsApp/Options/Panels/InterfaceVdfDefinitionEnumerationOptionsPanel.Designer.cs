
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	partial class InterfaceVdfDefinitionEnumerationOptionsPanel
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
			this.addButton = new System.Windows.Forms.Button();
			this.colorButton = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox();
			this.colorLabel = new System.Windows.Forms.Label();
			this.pathsLabel = new System.Windows.Forms.Label();
			this.spacingLabel1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// addButton
			// 
			this.addButton.Dock = System.Windows.Forms.DockStyle.Top;
			this.addButton.Location = new System.Drawing.Point(0, 118);
			this.addButton.Name = "addButton";
			this.addButton.Size = new System.Drawing.Size(600, 35);
			this.addButton.TabIndex = 0;
			this.addButton.Text = "Add...";
			this.addButton.UseVisualStyleBackColor = true;
			this.addButton.Click += new System.EventHandler(this.addButton_Click);
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
			this.colorButton.TabIndex = 6;
			// 
			// colorLabel
			// 
			this.colorLabel.AutoSize = true;
			this.colorLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.colorLabel.Location = new System.Drawing.Point(0, 0);
			this.colorLabel.Name = "colorLabel";
			this.colorLabel.Size = new System.Drawing.Size(65, 25);
			this.colorLabel.TabIndex = 5;
			this.colorLabel.Text = "&Color:";
			// 
			// pathsLabel
			// 
			this.pathsLabel.AutoSize = true;
			this.pathsLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.pathsLabel.Location = new System.Drawing.Point(0, 93);
			this.pathsLabel.Name = "pathsLabel";
			this.pathsLabel.Size = new System.Drawing.Size(68, 25);
			this.pathsLabel.TabIndex = 7;
			this.pathsLabel.Text = "Paths:";
			// 
			// spacingLabel1
			// 
			this.spacingLabel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacingLabel1.Location = new System.Drawing.Point(0, 70);
			this.spacingLabel1.Name = "spacingLabel1";
			this.spacingLabel1.Size = new System.Drawing.Size(600, 23);
			this.spacingLabel1.TabIndex = 12;
			// 
			// InterfaceVdfDefinitionEnumerationOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.addButton);
			this.Controls.Add(this.pathsLabel);
			this.Controls.Add(this.spacingLabel1);
			this.Controls.Add(this.colorButton);
			this.Controls.Add(this.colorLabel);
			this.MinimumSize = new System.Drawing.Size(300, 300);
			this.Name = "InterfaceVdfDefinitionEnumerationOptionsPanel";
			this.Size = new System.Drawing.Size(600, 480);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button addButton;
		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.ColorBox colorButton;
		private System.Windows.Forms.Label colorLabel;
		private System.Windows.Forms.Label pathsLabel;
		private System.Windows.Forms.Label spacingLabel1;
	}
}
