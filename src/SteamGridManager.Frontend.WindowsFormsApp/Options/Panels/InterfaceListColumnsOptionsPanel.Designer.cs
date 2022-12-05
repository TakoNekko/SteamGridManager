
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	partial class InterfaceListColumnsOptionsPanel
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
			this.customColumnsGroupBox = new System.Windows.Forms.GroupBox();
			this.maxTitleColumnWidthNumericUpDown = new System.Windows.Forms.NumericUpDown();
			this.useAlternateColumnHeadersCheckBox = new System.Windows.Forms.CheckBox();
			this.spacingLabel2 = new System.Windows.Forms.Label();
			this.optionsGroupBox = new System.Windows.Forms.GroupBox();
			this.maximumTitleColumnWidthLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.showAllFiltersInMenuCheckBox = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.allowPathSelectionInMenuCheckBox = new System.Windows.Forms.CheckBox();
			this.spacingLabel1 = new System.Windows.Forms.Label();
			this.listColumnsListBox = new SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.StringCollectionListBox();
			this.customColumnsGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.maxTitleColumnWidthNumericUpDown)).BeginInit();
			this.optionsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// customColumnsGroupBox
			// 
			this.customColumnsGroupBox.Controls.Add(this.listColumnsListBox);
			this.customColumnsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.customColumnsGroupBox.Location = new System.Drawing.Point(0, 279);
			this.customColumnsGroupBox.Name = "customColumnsGroupBox";
			this.customColumnsGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.customColumnsGroupBox.Size = new System.Drawing.Size(600, 491);
			this.customColumnsGroupBox.TabIndex = 6;
			this.customColumnsGroupBox.TabStop = false;
			this.customColumnsGroupBox.Text = "Custom VDF Columns";
			// 
			// maxTitleColumnWidthNumericUpDown
			// 
			this.maxTitleColumnWidthNumericUpDown.Dock = System.Windows.Forms.DockStyle.Top;
			this.maxTitleColumnWidthNumericUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
			this.maxTitleColumnWidthNumericUpDown.Location = new System.Drawing.Point(12, 215);
			this.maxTitleColumnWidthNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.maxTitleColumnWidthNumericUpDown.Name = "maxTitleColumnWidthNumericUpDown";
			this.maxTitleColumnWidthNumericUpDown.Size = new System.Drawing.Size(576, 29);
			this.maxTitleColumnWidthNumericUpDown.TabIndex = 5;
			// 
			// useAlternateColumnHeadersCheckBox
			// 
			this.useAlternateColumnHeadersCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.useAlternateColumnHeadersCheckBox.Location = new System.Drawing.Point(12, 34);
			this.useAlternateColumnHeadersCheckBox.Name = "useAlternateColumnHeadersCheckBox";
			this.useAlternateColumnHeadersCheckBox.Size = new System.Drawing.Size(576, 29);
			this.useAlternateColumnHeadersCheckBox.TabIndex = 1;
			this.useAlternateColumnHeadersCheckBox.Text = "&Shorten column headers";
			this.useAlternateColumnHeadersCheckBox.UseVisualStyleBackColor = true;
			// 
			// spacingLabel2
			// 
			this.spacingLabel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacingLabel2.Location = new System.Drawing.Point(0, 256);
			this.spacingLabel2.Name = "spacingLabel2";
			this.spacingLabel2.Size = new System.Drawing.Size(600, 23);
			this.spacingLabel2.TabIndex = 5;
			// 
			// optionsGroupBox
			// 
			this.optionsGroupBox.AutoSize = true;
			this.optionsGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.optionsGroupBox.Controls.Add(this.maxTitleColumnWidthNumericUpDown);
			this.optionsGroupBox.Controls.Add(this.maximumTitleColumnWidthLabel);
			this.optionsGroupBox.Controls.Add(this.label2);
			this.optionsGroupBox.Controls.Add(this.showAllFiltersInMenuCheckBox);
			this.optionsGroupBox.Controls.Add(this.label1);
			this.optionsGroupBox.Controls.Add(this.allowPathSelectionInMenuCheckBox);
			this.optionsGroupBox.Controls.Add(this.spacingLabel1);
			this.optionsGroupBox.Controls.Add(this.useAlternateColumnHeadersCheckBox);
			this.optionsGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.optionsGroupBox.Location = new System.Drawing.Point(0, 0);
			this.optionsGroupBox.Name = "optionsGroupBox";
			this.optionsGroupBox.Padding = new System.Windows.Forms.Padding(12);
			this.optionsGroupBox.Size = new System.Drawing.Size(600, 256);
			this.optionsGroupBox.TabIndex = 0;
			this.optionsGroupBox.TabStop = false;
			this.optionsGroupBox.Text = "&Options";
			// 
			// maximumTitleColumnWidthLabel
			// 
			this.maximumTitleColumnWidthLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.maximumTitleColumnWidthLabel.Location = new System.Drawing.Point(12, 190);
			this.maximumTitleColumnWidthLabel.Name = "maximumTitleColumnWidthLabel";
			this.maximumTitleColumnWidthLabel.Size = new System.Drawing.Size(576, 25);
			this.maximumTitleColumnWidthLabel.TabIndex = 4;
			this.maximumTitleColumnWidthLabel.Text = "&Maximum Title Column Width:";
			// 
			// label2
			// 
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(12, 167);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(576, 23);
			this.label2.TabIndex = 7;
			// 
			// showAllFiltersInMenuCheckBox
			// 
			this.showAllFiltersInMenuCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.showAllFiltersInMenuCheckBox.Location = new System.Drawing.Point(12, 138);
			this.showAllFiltersInMenuCheckBox.Name = "showAllFiltersInMenuCheckBox";
			this.showAllFiltersInMenuCheckBox.Size = new System.Drawing.Size(576, 29);
			this.showAllFiltersInMenuCheckBox.TabIndex = 3;
			this.showAllFiltersInMenuCheckBox.Text = "&Show all paths for multi-path filters in sorting menu";
			this.showAllFiltersInMenuCheckBox.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(12, 115);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(576, 23);
			this.label1.TabIndex = 6;
			this.label1.Visible = false;
			// 
			// allowPathSelectionInMenuCheckBox
			// 
			this.allowPathSelectionInMenuCheckBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.allowPathSelectionInMenuCheckBox.Location = new System.Drawing.Point(12, 86);
			this.allowPathSelectionInMenuCheckBox.Name = "allowPathSelectionInMenuCheckBox";
			this.allowPathSelectionInMenuCheckBox.Size = new System.Drawing.Size(576, 29);
			this.allowPathSelectionInMenuCheckBox.TabIndex = 2;
			this.allowPathSelectionInMenuCheckBox.Text = "&Allow path selection in sorting menu";
			this.allowPathSelectionInMenuCheckBox.UseVisualStyleBackColor = true;
			// 
			// spacingLabel1
			// 
			this.spacingLabel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.spacingLabel1.Location = new System.Drawing.Point(12, 63);
			this.spacingLabel1.Name = "spacingLabel1";
			this.spacingLabel1.Size = new System.Drawing.Size(576, 23);
			this.spacingLabel1.TabIndex = 2;
			this.spacingLabel1.Visible = false;
			// 
			// listColumnsListBox
			// 
			this.listColumnsListBox.CanEdit = false;
			this.listColumnsListBox.DataMember = null;
			this.listColumnsListBox.DataSource = null;
			this.listColumnsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listColumnsListBox.Location = new System.Drawing.Point(12, 34);
			this.listColumnsListBox.Name = "listColumnsListBox";
			this.listColumnsListBox.Size = new System.Drawing.Size(576, 445);
			this.listColumnsListBox.TabIndex = 7;
			// 
			// InterfaceListColumnsOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.customColumnsGroupBox);
			this.Controls.Add(this.spacingLabel2);
			this.Controls.Add(this.optionsGroupBox);
			this.MinimumSize = new System.Drawing.Size(300, 770);
			this.Name = "InterfaceListColumnsOptionsPanel";
			this.Size = new System.Drawing.Size(600, 770);
			this.customColumnsGroupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.maxTitleColumnWidthNumericUpDown)).EndInit();
			this.optionsGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.GroupBox customColumnsGroupBox;
		private SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls.StringCollectionListBox listColumnsListBox;
		private System.Windows.Forms.NumericUpDown maxTitleColumnWidthNumericUpDown;
		private System.Windows.Forms.CheckBox useAlternateColumnHeadersCheckBox;
		private System.Windows.Forms.Label spacingLabel2;
		private System.Windows.Forms.GroupBox optionsGroupBox;
		private System.Windows.Forms.Label maximumTitleColumnWidthLabel;
		private System.Windows.Forms.Label spacingLabel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox allowPathSelectionInMenuCheckBox;
		private System.Windows.Forms.CheckBox showAllFiltersInMenuCheckBox;
		private System.Windows.Forms.Label label2;
	}
}
