using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class InterfaceVdfDefinitionEnumerationOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public InterfaceVdfDefinitionEnumerationOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeDataSource();
				InitializeDataBindings();
				PopulatePathsListBox();
			}
		}

		private void InitializeDataSource()
		{
			colorButton.DataSource = Properties.Settings.Default;
			colorButton.DataMember = nameof(Properties.Settings.Default.VdfDefinition_Enum_Color);
		}

		private void InitializeDataBindings()
		{

		}

		private void PopulatePathsListBox()
		{

		}

		private void addButton_Click(object sender, EventArgs e)
		{
			AddNewEnumBox();
		}

		private void AddNewEnumBox()
		{
			// TODO
		}
	}
}
