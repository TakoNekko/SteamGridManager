using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class InterfaceDetailsGeneralOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public InterfaceDetailsGeneralOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeDataSource();
				InitializeHeaders();
				InitializeDataBindings();
				UpdateTextAutoCommitOptionsState();

				// NOTE: feature currently disabled.
				borderPanel.Visible = false;
			}
		}

		private void textAutoCommitEnableCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateTextAutoCommitOptionsState();
		}

		private void InitializeDataSource()
		{
			InitializeActionGroupBoxDataSource();
			InitializeBorderDataSource();
			InitializePanelsDataSource();
			InitializeUserDataPanelDataSource();
			InitializeLibraryCachePanelDataSource();
		}

		private void InitializeActionGroupBoxDataSource()
		{
			var assetActions = new AssetActionComboBoxItem[]
			{
				new AssetActionComboBoxItem { Display = "None", Value = AssetAction.None },
				new AssetActionComboBoxItem { Display = "Preview", Value = AssetAction.Preview },
				new AssetActionComboBoxItem { Display = "Open with Default Program", Value = AssetAction.Open },
				new AssetActionComboBoxItem { Display = "Edit with Default Program", Value = AssetAction.Edit },
				new AssetActionComboBoxItem { Display = "Select in Explorer", Value = AssetAction.Select },
				new AssetActionComboBoxItem { Display = "Set From File", Value = AssetAction.OpenFile },
				new AssetActionComboBoxItem { Display = "Set From Url", Value = AssetAction.OpenUrl },
			};

			foreach (var action in mouseActionGroupBox.Actions)
			{
				action.ComboBox.DataSource = assetActions.ToArray();
				action.ComboBox.DisplayMember = nameof(AssetActionComboBoxItem.Display);
				action.ComboBox.ValueMember = nameof(AssetActionComboBoxItem.Value);
			}
		}

		private void InitializeDataBindings()
		{
			InitializeBorderDataBindings();
			InitializeActionDataBindings();
			InitializeTextAutoCommitDataBindings();
			InitializeProgressDisplayDataBindings();
		}

		private void InitializeProgressDisplayDataBindings()
		{
			minFileSizeNumericUpDown.DataBindings.Add(nameof(minFileSizeNumericUpDown.Value), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Progress_MinFileSizeThreshold), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void InitializeTextAutoCommitDataBindings()
		{
			textAutoCommitEnableCheckBox.DataBindings.Add(nameof(textAutoCommitEnableCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Filter_AutoCommit_Enable), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			textAutoCommitDelayNumericUpDown.DataBindings.Add(nameof(textAutoCommitDelayNumericUpDown.Value), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Filter_AutoCommit_Delay), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void InitializeActionDataBindings()
		{
			mouseActionGroupBox.MouseLeftClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseLeftClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_MouseLeftClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			mouseActionGroupBox.MouseLeftDoubleClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseLeftDoubleClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_MouseLeftDoubleClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

			mouseActionGroupBox.MouseRightClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseRightClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_MouseRightClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			mouseActionGroupBox.MouseRightDoubleClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseRightDoubleClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_MouseRightDoubleClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

			mouseActionGroupBox.MouseMiddleClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseMiddleClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_MouseMiddleClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			mouseActionGroupBox.MouseMiddleDoubleClickAction.ComboBox.DataBindings.Add(nameof(mouseActionGroupBox.MouseMiddleDoubleClickAction.ComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_MouseMiddleDoubleClickAction), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void InitializeBorderDataSource()
		{
			var values = new AssetBorderComboBoxItem[]
			{
				new AssetBorderComboBoxItem { Display = "None", Value = AssetBorder.None },
				new AssetBorderComboBoxItem { Display = "Always", Value = AssetBorder.Always },
				new AssetBorderComboBoxItem { Display = "Mouse Over Only", Value = AssetBorder.MouseOverOnly },
			};

			borderComboBox.DataSource = values;
			borderComboBox.DisplayMember = nameof(AssetBorderComboBoxItem.Display);
			borderComboBox.ValueMember = nameof(AssetBorderComboBoxItem.Value);
		}

		private void InitializeBorderDataBindings()
		{
			borderComboBox.DataBindings.Add(nameof(borderComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.Details_Asset_BorderActivation), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void InitializePanelsDataSource()
		{
			tabPageBackgroundColorColorBox.DataSource = Properties.Settings.Default;
			tabPageBackgroundColorColorBox.DataMember = nameof(Properties.Settings.Default.Details_BackColor);
		}

		private void InitializeUserDataPanelDataSource()
		{
			userDataDetailsPanelOptionGroupBox.LibraryCapsule.ColorBox.DataSource = Properties.Settings.Default;
			userDataDetailsPanelOptionGroupBox.LibraryCapsule.ColorBox.DataMember = nameof(Properties.Settings.Default.Details_UserData_LibraryCapsule_BackColor);

			userDataDetailsPanelOptionGroupBox.HeroGraphic.ColorBox.DataSource = Properties.Settings.Default;
			userDataDetailsPanelOptionGroupBox.HeroGraphic.ColorBox.DataMember = nameof(Properties.Settings.Default.Details_UserData_HeroGraphic_BackColor);

			userDataDetailsPanelOptionGroupBox.Logo.ColorBox.DataSource = Properties.Settings.Default;
			userDataDetailsPanelOptionGroupBox.Logo.ColorBox.DataMember = nameof(Properties.Settings.Default.Details_UserData_Logo_BackColor);

			userDataDetailsPanelOptionGroupBox.Header.ColorBox.DataSource = Properties.Settings.Default;
			userDataDetailsPanelOptionGroupBox.Header.ColorBox.DataMember = nameof(Properties.Settings.Default.Details_UserData_Header_BackColor);

			userDataDetailsPanelOptionGroupBox.Icon.ColorBox.DataSource = Properties.Settings.Default;
			userDataDetailsPanelOptionGroupBox.Icon.ColorBox.DataMember = nameof(Properties.Settings.Default.Details_UserData_Icon_BackColor);
		}

		private void InitializeLibraryCachePanelDataSource()
		{
			libraryCacheDetailsPanelOptionGroupBox.LibraryCapsule.ColorBox.DataSource = Properties.Settings.Default;
			libraryCacheDetailsPanelOptionGroupBox.LibraryCapsule.ColorBox.DataMember = nameof(Properties.Settings.Default.Details_LibraryCache_LibraryCapsule_BackColor);

			libraryCacheDetailsPanelOptionGroupBox.HeroGraphic.ColorBox.DataSource = Properties.Settings.Default;
			libraryCacheDetailsPanelOptionGroupBox.HeroGraphic.ColorBox.DataMember = nameof(Properties.Settings.Default.Details_LibraryCache_HeroGraphic_BackColor);

			libraryCacheDetailsPanelOptionGroupBox.Logo.ColorBox.DataSource = Properties.Settings.Default;
			libraryCacheDetailsPanelOptionGroupBox.Logo.ColorBox.DataMember = nameof(Properties.Settings.Default.Details_LibraryCache_Logo_BackColor);

			libraryCacheDetailsPanelOptionGroupBox.Header.ColorBox.DataSource = Properties.Settings.Default;
			libraryCacheDetailsPanelOptionGroupBox.Header.ColorBox.DataMember = nameof(Properties.Settings.Default.Details_LibraryCache_Header_BackColor);

			libraryCacheDetailsPanelOptionGroupBox.Icon.ColorBox.DataSource = Properties.Settings.Default;
			libraryCacheDetailsPanelOptionGroupBox.Icon.ColorBox.DataMember = nameof(Properties.Settings.Default.Details_LibraryCache_Icon_BackColor);
		}

		private void InitializeHeaders()
		{
			userDataDetailsPanelOptionGroupBox.GroupBox.Text = "&User Data Panel";

			libraryCacheDetailsPanelOptionGroupBox.GroupBox.Text = "&Library Cache Panel";
		}

		private void UpdateTextAutoCommitOptionsState()
		{
			textAutoCommitOptionsGroupBox.Enabled = textAutoCommitEnableCheckBox.Checked;
		}

		public class AssetActionComboBoxItem
		{
			public string Display { get; set; }
			public AssetAction Value { get; set; }
		}

		public class AssetBorderComboBoxItem
		{
			public string Display { get; set; }
			public AssetBorder Value { get; set; }
		}
	}
}
