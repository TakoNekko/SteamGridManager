
namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	partial class InterfaceDialogPromptsOptionsPanel
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
			this.promptsCheckedListBox = new System.Windows.Forms.CheckedListBox();
			this.SuspendLayout();
			// 
			// promptsCheckedListBox
			// 
			this.promptsCheckedListBox.CheckOnClick = true;
			this.promptsCheckedListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.promptsCheckedListBox.FormattingEnabled = true;
			this.promptsCheckedListBox.IntegralHeight = false;
			this.promptsCheckedListBox.Location = new System.Drawing.Point(0, 0);
			this.promptsCheckedListBox.Name = "promptsCheckedListBox";
			this.promptsCheckedListBox.Size = new System.Drawing.Size(389, 447);
			this.promptsCheckedListBox.TabIndex = 0;
			this.promptsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.promptsCheckedListBox_ItemCheck);
			// 
			// EnvironmentPromptsOptionsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.promptsCheckedListBox);
			this.Name = "EnvironmentPromptsOptionsPanel";
			this.Size = new System.Drawing.Size(389, 447);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.CheckedListBox promptsCheckedListBox;
	}
}
