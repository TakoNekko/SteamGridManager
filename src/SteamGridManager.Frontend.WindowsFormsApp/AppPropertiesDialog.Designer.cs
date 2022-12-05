
namespace SteamGridManager.Frontend.WindowsFormsApp
{
	partial class AppPropertiesDialog
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
			this.cancelButton = new System.Windows.Forms.Button();
			this.vdfObjectPropertyListView = new SteamGridManager.Frontend.WindowsFormsApp.UserControls.VdfObjectPropertyListView();
			this.SuspendLayout();
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(0, 0);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(0, 0);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.TabStop = false;
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// vdfObjectPropertyListView
			// 
			this.vdfObjectPropertyListView.AppID = ((ulong)(0ul));
			this.vdfObjectPropertyListView.AppInfos = null;
			this.vdfObjectPropertyListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.vdfObjectPropertyListView.Location = new System.Drawing.Point(0, 0);
			this.vdfObjectPropertyListView.MinimumSize = new System.Drawing.Size(300, 200);
			this.vdfObjectPropertyListView.Name = "vdfObjectPropertyListView";
			this.vdfObjectPropertyListView.Path = null;
			this.vdfObjectPropertyListView.Shortcuts = null;
			this.vdfObjectPropertyListView.Size = new System.Drawing.Size(1000, 704);
			this.vdfObjectPropertyListView.TabIndex = 2;
			this.vdfObjectPropertyListView.VdfObject = null;
			// 
			// AppPropertiesDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(1000, 704);
			this.Controls.Add(this.vdfObjectPropertyListView);
			this.Controls.Add(this.cancelButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimumSize = new System.Drawing.Size(300, 200);
			this.Name = "AppPropertiesDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Properties";
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button cancelButton;
		private UserControls.VdfObjectPropertyListView vdfObjectPropertyListView;
	}
}