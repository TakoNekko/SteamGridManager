using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class EnvironmentGeneralOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public EnvironmentGeneralOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeDataSources();
				InitializeDataBindings();
			}
		}

		private void InitializeDataSources()
		{
			InitializeLanguageDataSource();
			InitializeCultureDataSource();

			loadAppInfoDatabaseCheckBox.DataBindings.Add(nameof(loadAppInfoDatabaseCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.Database_AppInfo_LoadAtStart), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			loadShortcutDatabaseCheckBox.DataBindings.Add(nameof(loadShortcutDatabaseCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.Database_Shortcuts_LoadAtStart), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			restoreApplicationFilterCheckBox.DataBindings.Add(nameof(restoreApplicationFilterCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.List_Filter_RestoreAtStart), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			restoreApplicationSelectionCheckBox.DataBindings.Add(nameof(restoreApplicationSelectionCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_RestoreAppIDAtStart), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void InitializeDataBindings()
		{
			InitializeLanguageDataBindings();
			InitializeCultureDataBindings();
		}

		private void InitializeLanguageDataSource()
		{
			var values = new LanguageComboBoxItem[]
			{
				new LanguageComboBoxItem { Display = "English (Default)", Value = "" },
				//new LanguageComboBoxItem { Display = "English (Default)", Value = "en-us" },
			};

			// TODO: fetch the available localization folders then populate the list above.

			languageComboBox.DataSource = values;
			languageComboBox.DisplayMember = nameof(LanguageComboBoxItem.Display);
			languageComboBox.ValueMember = nameof(LanguageComboBoxItem.Value);
		}

		private void InitializeLanguageDataBindings()
		{
			languageComboBox.DataBindings.Add(nameof(languageComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.UI_Language), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void InitializeCultureDataSource()
		{
			var items = CultureInfo.GetCultures(CultureTypes.AllCultures)
				.Where(cultureInfo => cultureInfo != CultureInfo.InvariantCulture)
				.Select(cultureInfo =>
				new CultureComboBoxItem { Display = $"{cultureInfo.NativeName}", Value = cultureInfo.Name });

			cultureComboBox.DataSource = items.ToArray();
			cultureComboBox.DisplayMember = nameof(CultureComboBoxItem.Display);
			cultureComboBox.ValueMember = nameof(CultureComboBoxItem.Value);
		}

		private void InitializeCultureDataBindings()
		{
			cultureComboBox.DataBindings.Add(nameof(cultureComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.UI_Culture), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		public class LanguageComboBoxItem
		{
			public string Display { get; set; }
			public string Value { get; set; }
		}

		public class CultureComboBoxItem
		{
			public string Display { get; set; }
			public string Value { get; set; }
		}
	}
}
