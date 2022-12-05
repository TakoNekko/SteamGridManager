
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls
{
	partial class ColorBox
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
			this.colorButton = new System.Windows.Forms.Button();
			this.resetButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// colorButton
			// 
			this.colorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.colorButton.Location = new System.Drawing.Point(3, 3);
			this.colorButton.Name = "colorButton";
			this.colorButton.Size = new System.Drawing.Size(379, 39);
			this.colorButton.TabIndex = 0;
			this.colorButton.UseVisualStyleBackColor = true;
			this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
			// 
			// resetButton
			// 
			this.resetButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.resetButton.Enabled = false;
			this.resetButton.Location = new System.Drawing.Point(388, 3);
			this.resetButton.Name = "resetButton";
			this.resetButton.Size = new System.Drawing.Size(129, 39);
			this.resetButton.TabIndex = 1;
			this.resetButton.Text = "&Reset";
			this.resetButton.UseVisualStyleBackColor = true;
			this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
			// 
			// ColorBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.resetButton);
			this.Controls.Add(this.colorButton);
			this.Name = "ColorBox";
			this.Size = new System.Drawing.Size(520, 45);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button colorButton;
		private System.Windows.Forms.Button resetButton;
	}
}
