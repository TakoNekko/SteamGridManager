using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class InterfaceListFilterOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public InterfaceListFilterOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeDataBindings();
				UpdateTextAutoCommitOptionsState();
			}
		}
		private void textAutoCommitEnableCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateTextAutoCommitOptionsState();
		}

		private void InitializeDataBindings()
		{
			useRegularExpressionsCheckBox.DataBindings.Add(nameof(useRegularExpressionsCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.List_VdfFilter_UseRegularExpressions), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			matchCaseCheckBox.DataBindings.Add(nameof(matchCaseCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.List_VdfFilter_MatchCase), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			matchWholeWordCheckBox.DataBindings.Add(nameof(matchWholeWordCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.List_VdfFilter_MatchWholeWord), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			vdfFilterPrefixTextBox.DataBindings.Add(nameof(vdfFilterPrefixTextBox.Text), Properties.Settings.Default, nameof(Properties.Settings.Default.List_VdfFilter_Prefix), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			textAutoCommitEnableCheckBox.DataBindings.Add(nameof(textAutoCommitEnableCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.List_Filter_AutoCommit_Enable), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			textAutoCommitDelayNumericUpDown.DataBindings.Add(nameof(textAutoCommitDelayNumericUpDown.Value), Properties.Settings.Default, nameof(Properties.Settings.Default.List_Filter_AutoCommit_Delay), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void UpdateTextAutoCommitOptionsState()
		{
			textAutoCommitOptionsGroupBox.Enabled = textAutoCommitEnableCheckBox.Checked;
		}
	}
}
