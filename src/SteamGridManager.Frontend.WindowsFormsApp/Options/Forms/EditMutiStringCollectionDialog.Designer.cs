
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Forms
{
	partial class EditMutiStringCollectionDialog
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
			this.stringCollectionListBox1 = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.StringCollectionListBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.spacingLabel1 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// stringCollectionListBox1
			// 
			this.stringCollectionListBox1.DataMember = null;
			this.stringCollectionListBox1.DataSource = null;
			this.stringCollectionListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.stringCollectionListBox1.Location = new System.Drawing.Point(12, 12);
			this.stringCollectionListBox1.Name = "stringCollectionListBox1";
			this.stringCollectionListBox1.Size = new System.Drawing.Size(776, 426);
			this.stringCollectionListBox1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.okButton);
			this.panel1.Controls.Add(this.spacingLabel1);
			this.panel1.Controls.Add(this.cancelButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(12, 372);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(12);
			this.panel1.Size = new System.Drawing.Size(776, 66);
			this.panel1.TabIndex = 1;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.cancelButton.Location = new System.Drawing.Point(644, 12);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(120, 42);
			this.cancelButton.TabIndex = 0;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.okButton.Location = new System.Drawing.Point(512, 12);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(120, 42);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// spacingLabel1
			// 
			this.spacingLabel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.spacingLabel1.Location = new System.Drawing.Point(632, 12);
			this.spacingLabel1.Name = "spacingLabel1";
			this.spacingLabel1.Size = new System.Drawing.Size(12, 42);
			this.spacingLabel1.TabIndex = 2;
			// 
			// EditMutiStringCollectionDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.stringCollectionListBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "EditMutiStringCollectionDialog";
			this.Padding = new System.Windows.Forms.Padding(12);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Strings Editor";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private UserControls.StringCollectionListBox stringCollectionListBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Label spacingLabel1;
		private System.Windows.Forms.Button cancelButton;
	}
}