using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class InterfaceDialogPromptsOptionsPanel : UserControl
	{
		#region Fields

		private readonly bool designMode;

		#endregion

		#region Constructors

		public InterfaceDialogPromptsOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeDataSource();
				InitializePromptsListView();
			}
		}

		#endregion

		#region Event Handlers

		#region Prompts Checked List Box

		private void promptsCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			if (e.Index < 0)
			{
				return;
			}

			if (promptsCheckedListBox.Items[e.Index] is PromptListBoxItem item)
			{
				ToggleCollection.Instance.Remove(item.Key);
				ToggleCollection.Instance.Add(item.Key, e.NewValue == CheckState.Checked);
			}
		}

		#endregion

		#endregion

		#region Implementation

		private void InitializeDataSource()
		{
			promptsCheckedListBox.DataSource = ToggleCollection.Instance.Items.OrderBy(x => x.Key).Select(kvp => new PromptListBoxItem { Title = kvp.Key, Key = kvp.Key, Value = kvp.Value }).ToArray();
			promptsCheckedListBox.DisplayMember = nameof(PromptListBoxItem.Title);
			promptsCheckedListBox.ValueMember = nameof(PromptListBoxItem.Value);
		}

		private void InitializePromptsListView()
		{
			var count = promptsCheckedListBox.Items.Count;

			for (var i = 0; i < count; ++i)
			{
				if (promptsCheckedListBox.Items[i] is PromptListBoxItem item)
				{
					promptsCheckedListBox.SetItemChecked(i, ToggleCollection.Instance.Items[item.Key]);
				}
			}
		}

		#endregion

		#region Types

		public class PromptListBoxItem
		{
			// NOTE: should be localized.
			public string Title { get; set; }

			public string Key { get; set; }
			public bool Value { get; set; }
		}

		#endregion
	}
}
