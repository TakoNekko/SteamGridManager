using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class InterfaceVdfDefinitionStringOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public InterfaceVdfDefinitionStringOptionsPanel()
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
			colorButton.DataSource = Properties.Settings.Default;
			colorButton.DataMember = nameof(Properties.Settings.Default.VdfDefinition_String_Color);

			pathsListBox.DataSource = Properties.Settings.Default;
			pathsListBox.DataMember = nameof(Properties.Settings.Default.VdfPropertyType_String_KnownPaths);
		}

		private void InitializeDataBindings()
		{
		}
	}
}
