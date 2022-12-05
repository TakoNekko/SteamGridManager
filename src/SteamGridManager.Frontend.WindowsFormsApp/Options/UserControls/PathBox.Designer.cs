
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls
{
	partial class PathBox
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
			this.pathTextBox = new System.Windows.Forms.TextBox();
			this.browseButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.resetButton = new System.Windows.Forms.Button();
			this.selectButton = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pathTextBox
			// 
			this.pathTextBox.AllowDrop = true;
			this.tableLayoutPanel1.SetColumnSpan(this.pathTextBox, 3);
			this.pathTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pathTextBox.Location = new System.Drawing.Point(3, 3);
			this.pathTextBox.Name = "pathTextBox";
			this.pathTextBox.Size = new System.Drawing.Size(462, 29);
			this.pathTextBox.TabIndex = 0;
			this.pathTextBox.TextChanged += new System.EventHandler(this.pathTextBox_TextChanged);
			this.pathTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.pathTextBox_DragDrop);
			this.pathTextBox.DragEnter += new System.Windows.Forms.DragEventHandler(this.pathTextBox_DragEnter);
			// 
			// browseButton
			// 
			this.browseButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.browseButton.Location = new System.Drawing.Point(3, 38);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(150, 30);
			this.browseButton.TabIndex = 1;
			this.browseButton.Text = "&Browse...";
			this.browseButton.UseVisualStyleBackColor = true;
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel1.Controls.Add(this.pathTextBox, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.browseButton, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.resetButton, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.selectButton, 1, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(468, 71);
			this.tableLayoutPanel1.TabIndex = 4;
			// 
			// resetButton
			// 
			this.resetButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resetButton.Enabled = false;
			this.resetButton.Location = new System.Drawing.Point(315, 38);
			this.resetButton.Name = "resetButton";
			this.resetButton.Size = new System.Drawing.Size(150, 30);
			this.resetButton.TabIndex = 2;
			this.resetButton.Text = "&Reset";
			this.resetButton.UseVisualStyleBackColor = true;
			this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
			// 
			// selectButton
			// 
			this.selectButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.selectButton.Enabled = false;
			this.selectButton.Location = new System.Drawing.Point(159, 38);
			this.selectButton.Name = "selectButton";
			this.selectButton.Size = new System.Drawing.Size(150, 30);
			this.selectButton.TabIndex = 1;
			this.selectButton.Text = "&Select...";
			this.selectButton.UseVisualStyleBackColor = true;
			this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
			// 
			// PathBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.MinimumSize = new System.Drawing.Size(0, 71);
			this.Name = "PathBox";
			this.Size = new System.Drawing.Size(468, 71);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox pathTextBox;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button resetButton;
		private System.Windows.Forms.Button selectButton;
	}
}
