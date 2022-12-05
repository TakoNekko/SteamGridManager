using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class InterfaceDialogPropertiesOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public InterfaceDialogPropertiesOptionsPanel()
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
			var defaultValues = new VdfPropertyActionComboBoxItem[]
			{
				new VdfPropertyActionComboBoxItem { Display = "None", Value = VdfPropertyAction.None },
				new VdfPropertyActionComboBoxItem { Display = "Edit", Value = VdfPropertyAction.Edit },
				new VdfPropertyActionComboBoxItem { Display = "Quick Edit", Value = VdfPropertyAction.InlineEdit },
			};

			defaultItemActivationComboBox.DataSource = defaultValues;
			defaultItemActivationComboBox.DisplayMember = nameof(VdfPropertyActionComboBoxItem.Display);
			defaultItemActivationComboBox.ValueMember = nameof(VdfPropertyActionComboBoxItem.Value);

			var objectValues = new VdfPropertyActionComboBoxItem[]
			{
				new VdfPropertyActionComboBoxItem { Display = "None", Value = VdfPropertyAction.None },
				new VdfPropertyActionComboBoxItem { Display = "Edit", Value = VdfPropertyAction.Edit },
				// NOTE: not currently supported, but might add support for it later (by parsing ACF)
				//new VdfPropertyActionComboBoxItem { Display = "Quick Edit", Value = VdfPropertyAction.InlineEdit },
				new VdfPropertyActionComboBoxItem { Display = "Open", Value = VdfPropertyAction.Open },
			};

			objectItemActivationComboBox.DataSource = objectValues;
			objectItemActivationComboBox.DisplayMember = nameof(VdfPropertyActionComboBoxItem.Display);
			objectItemActivationComboBox.ValueMember = nameof(VdfPropertyActionComboBoxItem.Value);
		}

		private void InitializeDataBindings()
		{
			defaultItemActivationComboBox.DataBindings.Add(nameof(defaultItemActivationComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.PropertiesDialog_DefaultAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			objectItemActivationComboBox.DataBindings.Add(nameof(objectItemActivationComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.PropertiesDialog_ObjectAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			showPropertiesMaximizedCheckBox.DataBindings.Add(nameof(showPropertiesMaximizedCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.PropertiesDialog_StartMaximized), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}
	}

	public class VdfPropertyActionComboBoxItem
	{
		public string Display { get; set; }
		public VdfPropertyAction Value { get; set; }
	}
}
