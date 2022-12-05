using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	using Helpers;

	public partial class InterfaceVdfDefinitionRelativeTimeOptionsPanel : UserControl, INotifyDataErrorInfo
	{
		private readonly bool designMode;

		public InterfaceVdfDefinitionRelativeTimeOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeDataSource();
				InitializeCultureComboBox();
				InitializeDataBindings();
			}
		}


		private readonly DateTime validationDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private void formatTextBox_Validating(object sender, CancelEventArgs e)
		{
			try
			{
				_ = validationDateTime.ToString(Properties.Settings.Default.VdfDefinition_RelativeTime_Format);
			}
			catch (Exception ex)
			{
				errors.Add(nameof(formatTextBox), ex);
				ErrorsChanged.Invoke(this, new DataErrorsChangedEventArgs(nameof(formatTextBox)));
			}
		}

		private void cultureComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			formatTextBox.CultureInfo = VdfUtils.GetCultureInfo((VdfDefinitionUIntCulture)cultureComboBox.SelectedValue);
		}

		#region INotifyDataErrorInfo

		private readonly Dictionary<string, Exception> errors = new Dictionary<string, Exception>();

		public bool HasErrors
			=> errors.Count > 0;

		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

		public IEnumerable GetErrors(string propertyName)
		{
			if (errors.TryGetValue(propertyName, out var error))
			{
				return new string[] { error.Message };
			}

			return null;
		}

		#endregion

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
			colorButton.DataMember = nameof(Properties.Settings.Default.VdfDefinition_RelativeTime_Color);

			formatTextBox.DataSource = Properties.Settings.Default;
			formatTextBox.DataMember = nameof(Properties.Settings.Default.VdfDefinition_RelativeTime_Format);

			pathsListBox.DataSource = Properties.Settings.Default;
			pathsListBox.DataMember = nameof(Properties.Settings.Default.VdfPropertyType_RelativeTime_KnownPaths);
		}

		private void InitializeDataBindings()
		{
			InitializeDataSource();
			cultureComboBox.DataBindings.Add(nameof(cultureComboBox.SelectedValue), Properties.Settings.Default, nameof(Properties.Settings.Default.VdfDefinition_UInt_Culture), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		public class ComboBoxItem
		{
			public string Name { get; set; }
			public VdfDefinitionUIntCulture Value { get; set; }
		}

	}
}
