using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class InterfaceVdfDefinitionUIntOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public InterfaceVdfDefinitionUIntOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeCultureComboBox();
				InitializeDataSource();
				InitializeDataBindings();
			}
		}

		private void InitializeCultureComboBox()
		{
			var values = new ComboBoxItem[]
			{
				new ComboBoxItem { Name = "Invariant",  Value = VdfDefinitionUIntCulture.Invariant },
				new ComboBoxItem { Name = "Auto",  Value = VdfDefinitionUIntCulture.Auto },
				new ComboBoxItem { Name = "Current",  Value = VdfDefinitionUIntCulture.Current },
				new ComboBoxItem { Name = "Current UI",  Value = VdfDefinitionUIntCulture.CurrentUI },
				new ComboBoxItem { Name = "Default Thread Current",  Value = VdfDefinitionUIntCulture.DefaultThreadCurrent },
				new ComboBoxItem { Name = "Default Thread Current UI",  Value = VdfDefinitionUIntCulture.DefaultThreadCurrentUI },
				new ComboBoxItem { Name = "Installed UI",  Value = VdfDefinitionUIntCulture.InstalledUI },
			};

			cultureComboBox.DataSource = values;
			cultureComboBox.DisplayMember = nameof(ComboBoxItem.Name);
			cultureComboBox.ValueMember = nameof(ComboBoxItem.Value);
		}

		private void InitializeDataSource()
		{
			colorButton.DataSource = Properties.Settings.Default;
			colorButton.DataMember = nameof(Properties.Settings.Default.VdfDefinition_UInt_Color);

			pathsListBox.DataSource = Properties.Settings.Default;
			pathsListBox.DataMember = nameof(Properties.Settings.Default.VdfPropertyType_UInt_KnownPaths);
		}

		private void InitializeDataBindings()
		{
			cultureComboBox.DataBindings.Add(nameof(cultureComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.VdfDefinition_UInt_Culture), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}
	}

	public class ComboBoxItem
	{
		public string Name { get; set; }
		public VdfDefinitionUIntCulture Value { get; set; }
	}
}
