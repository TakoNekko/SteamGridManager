using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class InterfaceListGeneralOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public InterfaceListGeneralOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeDataSource();
				InitializeDataBindings();
			}
		}

		private void InitializeDataSource()
		{
			var appActions = new AppActionComboBoxItem[]
			{
				new AppActionComboBoxItem { Display = "None", Value = AppAction.None },
				new AppActionComboBoxItem { Display = "Properties", Value = AppAction.Properties },
			};

			listItemDoubleClickActionComboBox.DataSource = appActions;
			listItemDoubleClickActionComboBox.DisplayMember = nameof(AppActionComboBoxItem.Display);
			listItemDoubleClickActionComboBox.ValueMember = nameof(AppActionComboBoxItem.Value);
		}

		private void InitializeDataBindings()
		{
			listItemDoubleClickActionComboBox.DataBindings.Add(nameof(listItemDoubleClickActionComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.List_AppAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

			existingAssetContentTextBox.DataBindings.Add(nameof(existingAssetContentTextBox.Text), Properties.Settings.Default, nameof(Properties.Settings.Default.List_Assets_Exist_String), formattingEnabled: true, DataSourceUpdateMode.OnValidation);
			existingAssetColorButton.DataSource = Properties.Settings.Default;
			existingAssetColorButton.DataMember = nameof(Properties.Settings.Default.List_Assets_Exist_Color);

			missingAssetContentTextBox.DataBindings.Add(nameof(missingAssetContentTextBox.Text), Properties.Settings.Default, nameof(Properties.Settings.Default.List_Assets_Missing_String), formattingEnabled: true, DataSourceUpdateMode.OnValidation);
			missingAssetColorButton.DataSource = Properties.Settings.Default;
			missingAssetColorButton.DataMember = nameof(Properties.Settings.Default.List_Assets_Missing_Color);
		}
	}

	public class AppActionComboBoxItem
	{
		public string Display { get; set; }
		public AppAction Value { get; set; }
	}
}
