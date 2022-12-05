using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class InterfaceVdfDefinitionBooleanOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public InterfaceVdfDefinitionBooleanOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeDataBindings();
				InitializeDataSource();
				UpdateFormattingControlStates();
			}
		}

		private void formatCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateFormattingControlStates();
		}

		private void InitializeDataSource()
		{
			colorButton.DataSource = Properties.Settings.Default;
			colorButton.DataMember = nameof(Properties.Settings.Default.VdfDefinition_Boolean_Color);

			pathsListBox.DataSource = Properties.Settings.Default;
			pathsListBox.DataMember = nameof(Properties.Settings.Default.VdfPropertyType_Boolean_KnownPaths);
		}

		private void InitializeDataBindings()
		{
			formatCheckBox.DataBindings.Add(nameof(formatCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.VdfDefinition_Boolean_FormatAsString), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			falseStringTextBox.DataBindings.Add(nameof(falseStringTextBox.Text), Properties.Settings.Default, nameof(Properties.Settings.Default.VdfDefinition_Boolean_FalseString), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			trueStringTextBox.DataBindings.Add(nameof(trueStringTextBox.Text), Properties.Settings.Default, nameof(Properties.Settings.Default.VdfDefinition_Boolean_TrueString), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void UpdateFormattingControlStates()
		{
			formattingGroupBox.Enabled = formatCheckBox.Checked;
		}
	}
}
