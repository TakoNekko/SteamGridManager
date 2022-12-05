
namespace SteamGridManager.Frontend.WindowsFormsApp
{
	partial class OptionsDialog
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
			System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("General");
			System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Paths");
			System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Database");
			System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Environment", new System.Windows.Forms.TreeNode[] {
            treeNode26,
            treeNode27,
            treeNode28});
			System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("General");
			System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Filter");
			System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("Columns");
			System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("List", new System.Windows.Forms.TreeNode[] {
            treeNode30,
            treeNode31,
            treeNode32});
			System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("General");
			System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("Preview");
			System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Details", new System.Windows.Forms.TreeNode[] {
            treeNode34,
            treeNode35});
			System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("Prompts");
			System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("Properties");
			System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("Property");
			System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("Dialog", new System.Windows.Forms.TreeNode[] {
            treeNode37,
            treeNode38,
            treeNode39});
			System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("Object");
			System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("String");
			System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("Unsigned Integer");
			System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("Boolean");
			System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("Relative Time");
			System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("Enumeration");
			System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("VDF Definition", new System.Windows.Forms.TreeNode[] {
            treeNode41,
            treeNode42,
            treeNode43,
            treeNode44,
            treeNode45,
            treeNode46});
			System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("Interface", new System.Windows.Forms.TreeNode[] {
            treeNode33,
            treeNode36,
            treeNode40,
            treeNode47});
			System.Windows.Forms.TreeNode treeNode49 = new System.Windows.Forms.TreeNode("External Applications");
			System.Windows.Forms.TreeNode treeNode50 = new System.Windows.Forms.TreeNode("Integration", new System.Windows.Forms.TreeNode[] {
            treeNode49});
			this.mainPanel = new System.Windows.Forms.Panel();
			this.categoryTreeView = new System.Windows.Forms.TreeView();
			this.outerPanel = new System.Windows.Forms.Panel();
			this.spacingLabel = new System.Windows.Forms.Label();
			this.buttonPanel = new System.Windows.Forms.Panel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.outerPanel.SuspendLayout();
			this.buttonPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainPanel
			// 
			this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainPanel.Location = new System.Drawing.Point(289, 12);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.Size = new System.Drawing.Size(875, 737);
			this.mainPanel.TabIndex = 3;
			// 
			// categoryTreeView
			// 
			this.categoryTreeView.Dock = System.Windows.Forms.DockStyle.Left;
			this.categoryTreeView.FullRowSelect = true;
			this.categoryTreeView.HideSelection = false;
			this.categoryTreeView.Location = new System.Drawing.Point(12, 12);
			this.categoryTreeView.Name = "categoryTreeView";
			treeNode26.Name = "Environment/General";
			treeNode26.Tag = "Environment/General";
			treeNode26.Text = "General";
			treeNode27.Name = "Environment/Paths";
			treeNode27.Tag = "Environment/Paths";
			treeNode27.Text = "Paths";
			treeNode28.Name = "Environment/Database";
			treeNode28.Tag = "Environment/Database";
			treeNode28.Text = "Database";
			treeNode29.Name = "Environment";
			treeNode29.Text = "Environment";
			treeNode30.Name = "Interface/List/General";
			treeNode30.Tag = "Interface/List/General";
			treeNode30.Text = "General";
			treeNode31.Name = "Interface/List/Filter";
			treeNode31.Tag = "Interface/List/Filter";
			treeNode31.Text = "Filter";
			treeNode32.Name = "Interface/List/Columns";
			treeNode32.Tag = "Interface/List/Columns";
			treeNode32.Text = "Columns";
			treeNode33.Name = "Interface/List";
			treeNode33.Tag = "";
			treeNode33.Text = "List";
			treeNode34.Name = "Interface/Details/General";
			treeNode34.Tag = "Interface/Details/General";
			treeNode34.Text = "General";
			treeNode35.Name = "Interface/Details/Preview";
			treeNode35.Tag = "Interface/Details/Preview";
			treeNode35.Text = "Preview";
			treeNode36.Name = "Interface/Details";
			treeNode36.Tag = "";
			treeNode36.Text = "Details";
			treeNode37.Name = "Interface/Dialog/Prompts";
			treeNode37.Tag = "Interface/Dialog/Prompts";
			treeNode37.Text = "Prompts";
			treeNode38.Name = "Interface/Dialog/Properties";
			treeNode38.Tag = "Interface/Dialog/Properties";
			treeNode38.Text = "Properties";
			treeNode39.Name = "Interface/Dialog/Property";
			treeNode39.Tag = "Interface/Dialog/Property";
			treeNode39.Text = "Property";
			treeNode40.Name = "Interface/Dialog";
			treeNode40.Text = "Dialog";
			treeNode41.Name = "Interface/VdfDefinition/Object";
			treeNode41.Tag = "Interface/VdfDefinition/Object";
			treeNode41.Text = "Object";
			treeNode42.Name = "Interface/VdfDefinition/String";
			treeNode42.Tag = "Interface/VdfDefinition/String";
			treeNode42.Text = "String";
			treeNode43.Name = "Interface/VdfDefinition/UInt";
			treeNode43.Tag = "Interface/VdfDefinition/UInt";
			treeNode43.Text = "Unsigned Integer";
			treeNode44.Name = "Interface/VdfDefinition/Boolean";
			treeNode44.Tag = "Interface/VdfDefinition/Boolean";
			treeNode44.Text = "Boolean";
			treeNode45.Name = "Interface/VdfDefinition/RelativeTime";
			treeNode45.Tag = "Interface/VdfDefinition/RelativeTime";
			treeNode45.Text = "Relative Time";
			treeNode46.Name = "Interface/VdfDefinition/Enum";
			treeNode46.Tag = "Interface/VdfDefinition/Enum";
			treeNode46.Text = "Enumeration";
			treeNode47.Name = "Interface/VdfDefinition";
			treeNode47.Text = "VDF Definition";
			treeNode48.Name = "Interface";
			treeNode48.Tag = "";
			treeNode48.Text = "Interface";
			treeNode49.Name = "Integration/ExternalApplications";
			treeNode49.Tag = "Integration/ExternalApplications";
			treeNode49.Text = "External Applications";
			treeNode50.Name = "Integration";
			treeNode50.Tag = "";
			treeNode50.Text = "Integration";
			this.categoryTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode29,
            treeNode48,
            treeNode50});
			this.categoryTreeView.PathSeparator = "/";
			this.categoryTreeView.ShowLines = false;
			this.categoryTreeView.ShowNodeToolTips = true;
			this.categoryTreeView.ShowPlusMinus = false;
			this.categoryTreeView.Size = new System.Drawing.Size(260, 737);
			this.categoryTreeView.TabIndex = 1;
			this.categoryTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.categoryTreeView_BeforeCollapse);
			this.categoryTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.categoryTreeView_BeforeSelect);
			this.categoryTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.categoryTreeView_AfterSelect);
			// 
			// outerPanel
			// 
			this.outerPanel.Controls.Add(this.mainPanel);
			this.outerPanel.Controls.Add(this.spacingLabel);
			this.outerPanel.Controls.Add(this.categoryTreeView);
			this.outerPanel.Controls.Add(this.buttonPanel);
			this.outerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outerPanel.Location = new System.Drawing.Point(0, 0);
			this.outerPanel.Name = "outerPanel";
			this.outerPanel.Padding = new System.Windows.Forms.Padding(12);
			this.outerPanel.Size = new System.Drawing.Size(1176, 836);
			this.outerPanel.TabIndex = 0;
			// 
			// spacingLabel
			// 
			this.spacingLabel.AutoSize = true;
			this.spacingLabel.Dock = System.Windows.Forms.DockStyle.Left;
			this.spacingLabel.Location = new System.Drawing.Point(272, 12);
			this.spacingLabel.Name = "spacingLabel";
			this.spacingLabel.Size = new System.Drawing.Size(17, 25);
			this.spacingLabel.TabIndex = 2;
			this.spacingLabel.Text = " ";
			// 
			// buttonPanel
			// 
			this.buttonPanel.AutoSize = true;
			this.buttonPanel.Controls.Add(this.cancelButton);
			this.buttonPanel.Controls.Add(this.okButton);
			this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonPanel.Location = new System.Drawing.Point(12, 749);
			this.buttonPanel.MinimumSize = new System.Drawing.Size(0, 75);
			this.buttonPanel.Name = "buttonPanel";
			this.buttonPanel.Padding = new System.Windows.Forms.Padding(12);
			this.buttonPanel.Size = new System.Drawing.Size(1152, 75);
			this.buttonPanel.TabIndex = 4;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(470, 0);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(0, 0);
			this.cancelButton.TabIndex = 6;
			this.cancelButton.TabStop = false;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// okButton
			// 
			this.okButton.AutoSize = true;
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.okButton.Location = new System.Drawing.Point(990, 12);
			this.okButton.MinimumSize = new System.Drawing.Size(150, 35);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(150, 51);
			this.okButton.TabIndex = 5;
			this.okButton.Text = "&OK";
			this.okButton.UseVisualStyleBackColor = true;
			// 
			// OptionsDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(1176, 836);
			this.Controls.Add(this.outerPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimumSize = new System.Drawing.Size(640, 480);
			this.Name = "OptionsDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.Load += new System.EventHandler(this.OptionsDialog_Load);
			this.outerPanel.ResumeLayout(false);
			this.outerPanel.PerformLayout();
			this.buttonPanel.ResumeLayout(false);
			this.buttonPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel mainPanel;
		private System.Windows.Forms.TreeView categoryTreeView;
		private System.Windows.Forms.Panel outerPanel;
		private System.Windows.Forms.Panel buttonPanel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label spacingLabel;
	}
}