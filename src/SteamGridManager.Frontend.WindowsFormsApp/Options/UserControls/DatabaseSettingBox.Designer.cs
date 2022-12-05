
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls
{
	partial class DatabaseSettingBox
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
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.textBox = new System.Windows.Forms.TextBox();
			this.loadAtStartCheckBox = new System.Windows.Forms.CheckBox();
			this.loadButton = new System.Windows.Forms.Button();
			this.unloadButton = new System.Windows.Forms.Button();
			this.selectInExplorerButton = new System.Windows.Forms.Button();
			this.groupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox
			// 
			this.groupBox.Controls.Add(this.selectInExplorerButton);
			this.groupBox.Controls.Add(this.unloadButton);
			this.groupBox.Controls.Add(this.loadButton);
			this.groupBox.Controls.Add(this.loadAtStartCheckBox);
			this.groupBox.Controls.Add(this.textBox);
			this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox.Location = new System.Drawing.Point(0, 0);
			this.groupBox.Name = "groupBox";
			this.groupBox.Size = new System.Drawing.Size(512, 243);
			this.groupBox.TabIndex = 0;
			this.groupBox.TabStop = false;
			// 
			// textBox
			// 
			this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox.Location = new System.Drawing.Point(6, 33);
			this.textBox.Name = "textBox";
			this.textBox.ReadOnly = true;
			this.textBox.Size = new System.Drawing.Size(500, 29);
			this.textBox.TabIndex = 0;
			// 
			// loadAtStartCheckBox
			// 
			this.loadAtStartCheckBox.AutoSize = true;
			this.loadAtStartCheckBox.Location = new System.Drawing.Point(19, 80);
			this.loadAtStartCheckBox.Name = "loadAtStartCheckBox";
			this.loadAtStartCheckBox.Size = new System.Drawing.Size(145, 29);
			this.loadAtStartCheckBox.TabIndex = 1;
			this.loadAtStartCheckBox.Text = "&Load at start";
			this.loadAtStartCheckBox.UseVisualStyleBackColor = true;
			// 
			// loadButton
			// 
			this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.loadButton.Location = new System.Drawing.Point(6, 121);
			this.loadButton.Name = "loadButton";
			this.loadButton.Size = new System.Drawing.Size(500, 35);
			this.loadButton.TabIndex = 2;
			this.loadButton.Text = "&Load";
			this.loadButton.UseVisualStyleBackColor = true;
			this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
			// 
			// unloadButton
			// 
			this.unloadButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.unloadButton.Location = new System.Drawing.Point(6, 162);
			this.unloadButton.Name = "unloadButton";
			this.unloadButton.Size = new System.Drawing.Size(500, 35);
			this.unloadButton.TabIndex = 3;
			this.unloadButton.Text = "&Unload";
			this.unloadButton.UseVisualStyleBackColor = true;
			this.unloadButton.Click += new System.EventHandler(this.unloadButton_Click);
			// 
			// selectInExplorerButton
			// 
			this.selectInExplorerButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.selectInExplorerButton.Location = new System.Drawing.Point(6, 203);
			this.selectInExplorerButton.Name = "selectInExplorerButton";
			this.selectInExplorerButton.Size = new System.Drawing.Size(500, 35);
			this.selectInExplorerButton.TabIndex = 4;
			this.selectInExplorerButton.Text = "&Select in Explorer...";
			this.selectInExplorerButton.UseVisualStyleBackColor = true;
			this.selectInExplorerButton.Click += new System.EventHandler(this.selectInExplorerButton_Click);
			// 
			// DatabaseSettingBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox);
			this.Name = "DatabaseSettingBox";
			this.Size = new System.Drawing.Size(512, 243);
			this.groupBox.ResumeLayout(false);
			this.groupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox;
		private System.Windows.Forms.Button selectInExplorerButton;
		private System.Windows.Forms.Button unloadButton;
		private System.Windows.Forms.Button loadButton;
		private System.Windows.Forms.CheckBox loadAtStartCheckBox;
		private System.Windows.Forms.TextBox textBox;
	}
}
