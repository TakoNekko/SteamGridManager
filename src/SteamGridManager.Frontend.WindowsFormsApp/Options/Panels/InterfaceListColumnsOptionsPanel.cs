using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	using Forms;

	public partial class InterfaceListColumnsOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public InterfaceListColumnsOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeDataBindings();
				InitializeDataSource();
				InitializeListBox();
			}
		}

		private void InitializeListBox()
		{
			listColumnsListBox.CanEdit = true;
			listColumnsListBox.EditRequested += ListColumnsListBox_EditRequested;
			listColumnsListBox.ListBox.SelectedIndexChanged += ListBox_SelectedIndexChanged;
			listColumnsListBox.CanEdit = false;
		}

		private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			var hasSingleSelection = listColumnsListBox.ListBox.SelectedIndices.Count == 1;

			listColumnsListBox.CanEdit = hasSingleSelection;
		}

		private void ListColumnsListBox_EditRequested(object sender, EventArgs e)
		{
			if (listColumnsListBox.ListBox.SelectedIndices.Count != 1)
			{
				return;
			}

			const string separator = "||";
			var selectedIndex = listColumnsListBox.ListBox.SelectedIndices[0];
			var selectedItem = listColumnsListBox.ListBox.Items[selectedIndex];
			var lines = selectedItem.Text.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
			var stringCollection = new StringCollection();

			stringCollection.AddRange(lines);

			using (var form = new EditMutiStringCollectionDialog
			{
				Values = stringCollection,
			})
			{
				if (form.ShowDialog(ParentForm) == DialogResult.OK)
				{
					var result = new string[form.Values.Count];

					for (var i = 0; i < form.Values.Count; ++i)
					{
						result[i] = form.Values[i];
					}

					listColumnsListBox.SetValue(selectedIndex, string.Join(separator, result));
				}
			}
		}

		private void InitializeDataBindings()
		{
			useAlternateColumnHeadersCheckBox.DataBindings.Add(nameof(useAlternateColumnHeadersCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.View_UseAlternateColumnHeaders), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			allowPathSelectionInMenuCheckBox.DataBindings.Add(nameof(allowPathSelectionInMenuCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.List_Menu_VdfColumn_AllowSelectPath), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			showAllFiltersInMenuCheckBox.DataBindings.Add(nameof(showAllFiltersInMenuCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.List_Menu_VdfColumn_ShowAllFilters), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			maxTitleColumnWidthNumericUpDown.DataBindings.Add(nameof(maxTitleColumnWidthNumericUpDown.Value), Properties.Settings.Default, nameof(Properties.Settings.Default.List_Column_Title_MaxAutoWidth), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void InitializeDataSource()
		{
			listColumnsListBox.DataSource = Properties.Settings.Default;
			listColumnsListBox.DataMember = nameof(Properties.Settings.Default.List_Column_Types);
		}
	}
}
