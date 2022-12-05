using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class InterfaceDetailsPreviewOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public InterfaceDetailsPreviewOptionsPanel()
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
			var values = new PreviewActionComboBoxItem[]
			{
				new PreviewActionComboBoxItem { Display = "None", Value = PreviewAction.None },
				new PreviewActionComboBoxItem { Display = "Close", Value = PreviewAction.Close },
				new PreviewActionComboBoxItem { Display = "Choose Background Color", Value = PreviewAction.ChooseBackgroundColor },
				new PreviewActionComboBoxItem { Display = "Toggle Image Size Mode", Value = PreviewAction.ToggleImageSizeMode },
			};

			foreach (var action in mouseActionGroupBox.Actions)
			{
				action.ComboBox.DataSource = values.ToArray();
				action.ComboBox.DisplayMember = nameof(PreviewActionComboBoxItem.Display);
				action.ComboBox.ValueMember = nameof(PreviewActionComboBoxItem.Value);
			}
		}

		private void InitializeDataBindings()
		{
			dialogBackgroundColorColorBox.DataSource = Properties.Settings.Default;
			dialogBackgroundColorColorBox.DataMember = nameof(Properties.Settings.Default.Details_Asset_Preview_BackColor);

			mouseActionGroupBox.MouseLeftClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseLeftClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_Preview_MouseLeftClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			mouseActionGroupBox.MouseLeftDoubleClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseLeftDoubleClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_Preview_MouseLeftDoubleClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

			mouseActionGroupBox.MouseRightClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseRightClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_Preview_MouseRightClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			mouseActionGroupBox.MouseRightDoubleClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseRightDoubleClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_Preview_MouseRightDoubleClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

			mouseActionGroupBox.MouseMiddleClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseMiddleClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_Preview_MouseMiddleClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			mouseActionGroupBox.MouseMiddleDoubleClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseMiddleDoubleClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_Preview_MouseMiddleDoubleClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		public class PreviewActionComboBoxItem
		{
			public string Display { get; set; }
			public PreviewAction Value { get; set; }
		}
	}
}
