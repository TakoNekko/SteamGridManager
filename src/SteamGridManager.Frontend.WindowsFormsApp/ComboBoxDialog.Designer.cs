
namespace SteamGridManager.Frontend.WindowsFormsApp
{
	partial class ComboBoxDialog
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
			this.outerPanel = new System.Windows.Forms.Panel();
			this.mainComboBox = new System.Windows.Forms.ComboBox();
			this.label = new System.Windows.Forms.Label();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.outerPanel.SuspendLayout();
			this.bottomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// outerPanel
			// 
			this.outerPanel.Controls.Add(this.mainComboBox);
			this.outerPanel.Controls.Add(this.label);
			this.outerPanel.Controls.Add(this.bottomPanel);
			this.outerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outerPanel.Location = new System.Drawing.Point(0, 0);
			this.outerPanel.Name = "outerPanel";
			this.outerPanel.Padding = new System.Windows.Forms.Padding(12);
			this.outerPanel.Size = new System.Drawing.Size(746, 185);
			this.outerPanel.TabIndex = 1;
			// 
			// mainComboBox
			// 
			this.mainComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.mainComboBox.FormattingEnabled = true;
			this.mainComboBox.Location = new System.Drawing.Point(12, 37);
			this.mainComboBox.Name = "mainComboBox";
			this.mainComboBox.Size = new System.Drawing.Size(722, 32);
			this.mainComboBox.TabIndex = 0;
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.Dock = System.Windows.Forms.DockStyle.Top;
			this.label.Location = new System.Drawing.Point(12, 12);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(0, 25);
			this.label.TabIndex = 2;
			// 
			// bottomPanel
			// 
			this.bottomPanel.Controls.Add(this.okButton);
			this.bottomPanel.Controls.Add(this.cancelButton);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point(12, 124);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size(722, 49);
			this.bottomPanel.TabIndex = 1;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(394, 3);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(161, 42);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(561, 4);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(161, 42);
			this.cancelButton.TabIndex = 0;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// ComboBoxDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(746, 185);
			this.Controls.Add(this.outerPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimumSize = new System.Drawing.Size(600, 200);
			this.Name = "ComboBoxDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Prompt";
			this.outerPanel.ResumeLayout(false);
			this.outerPanel.PerformLayout();
			this.bottomPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel outerPanel;
		private System.Windows.Forms.Panel bottomPanel;
		private System.Windows.Forms.Label label;
		private System.Windows.Forms.ComboBox mainComboBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
	}
}