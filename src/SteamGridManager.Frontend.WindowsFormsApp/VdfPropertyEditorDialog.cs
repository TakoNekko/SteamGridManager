using Steam.Vdf;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp
{
	using Extensions.Control;
	using Helpers;

	public partial class VdfPropertyEditorDialog : Form, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };


		private string path;

		public string Path
		{
			get => path;
			set
			{
				if (path != value)
				{
					path = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Path)));
				}
			}
		}

		private string propertyName;

		public string PropertyName
		{
			get => propertyName;
			set
			{
				if (propertyName != value)
				{
					propertyName = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(PropertyName)));
					OnPropertyNameChanged();
				}
			}
		}

		private VdfObject vdfObjectValue;

		public VdfObject VdfObjectValue
		{
			get
			{
				if (vdfObjectValue is null)
				{
					vdfObjectValue = new VdfObject();
				}

				return vdfObjectValue;
			}
			set
			{
				if (vdfObjectValue != value)
				{
					vdfObjectValue = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(VdfObjectValue)));
					OnVdfObjectValueChanged();
				}
			}
		}

		private string stringValue;

		public string StringValue
		{
			//get => stringValue ?? "";
			get
			{
				if (stringValue is null)
				{
					stringValue = "";
				}

				return stringValue;
			}
			set
			{
				if (stringValue != value)
				{
					stringValue = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(StringValue)));
					OnStringValueChanged();
				}
			}
		}

		private uint? uintValue;
		
		public uint? UIntValue
		{
			get => uintValue;
			set
			{
				if (uintValue != value)
				{
					uintValue = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(UIntValue)));
					OnUIntValueChanged();
				}
			}
		}

		public DateTime? RelativeTimeValue
		{
			get => uintValue.HasValue ? new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(uintValue.Value) : (DateTime?)null;
			set
			{
				var dto1 = uintValue.HasValue ? new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(uintValue.Value)) : (DateTimeOffset?)null;
				var dto2 = value.HasValue ? new DateTimeOffset(value.Value) : (DateTimeOffset?)null;

				if (dto1 is null
					|| dto1?.ToUnixTimeSeconds() != dto2?.ToUnixTimeSeconds())
				{
					uintValue = (uint)dto2?.ToUnixTimeSeconds();
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(RelativeTimeValue)));
					OnRelativeTimeValueChanged(value);
				}
			}
		}

		public object Value
		{
			get
			{
				switch (propertyValueTabControl.SelectedTab.Tag)
				{
					case "object":
						return vdfObjectValue;
					case "string":
						return stringValue;
					case "uint":
					case "bool":
					case "enum":
					case "rtime":
						return uintValue;
					default:
						return null;
				}
			}
			set
			{
				if (value is VdfObject vdfObjectValue)
				{
					VdfObjectValue = vdfObjectValue;
				}
				else if (value is string stringValue)
				{
					StringValue = stringValue;
				}
				else if (value is uint uintValue)
				{
					var propertyType = VdfUtils.GetVdfPropertyTypeByPath(System.IO.Path.Combine(Path ?? "", PropertyName).Replace("\\", "/"));

					switch (propertyType)
					{
						case VdfPropertyType.UInt:
							UIntValue = uintValue;
							break;
						case VdfPropertyType.Boolean:
							// TODO
							UIntValue = uintValue;
							break;
						case VdfPropertyType.RelativeTime:
							RelativeTimeValue = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(uintValue);
							break;
						case VdfPropertyType.Enum:
							// TODO
							UIntValue = uintValue;
							break;
						default:
							UIntValue = uintValue;
							break;
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException(nameof(value), value, $"VDF object value must be either of type object, string, or uint.");
				}

				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
			}
		}


		private bool typeIsReadOnly;

		public bool TypeIsReadOnly
		{
			get => typeIsReadOnly;
			set
			{
				if (typeIsReadOnly != value)
				{
					typeIsReadOnly = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(TypeIsReadOnly)));
				}
			}
		}

		private void OnPropertyNameChanged()
		{
			Text = $"{(string.IsNullOrEmpty(Path) ? "" : $"{Path}/")}{PropertyName} - {"Property Editor"}";

			UpdateControlStates();
		}

		private void OnVdfObjectValueChanged()
		{
			if (VdfObjectValue != null)
			{
				propertyValueTabControl.SelectedTab = vdfObjectValueTabPage;
				RemoveOtherTabs();
			}
		}

		private void OnStringValueChanged()
		{
			if (StringValue != null)
			{
				propertyValueTabControl.SelectedTab = stringValueTabPage;
				RemoveOtherTabs();
			}
		}

		private void OnUIntValueChanged()
		{
			if (UIntValue != null)
			{
				var propertyType = VdfUtils.GetVdfPropertyTypeByPath(System.IO.Path.Combine(Path ?? "", PropertyName).Replace("\\", "/"));

				switch (propertyType)
				{
					case VdfPropertyType.UInt:
						propertyValueTabControl.SelectedTab = uintValueTabPage;
						break;
					case VdfPropertyType.Boolean:
						propertyValueTabControl.SelectedTab = booleanValueTabPage;
						break;
					case VdfPropertyType.RelativeTime:
						propertyValueTabControl.SelectedTab = relativeTimeValueTabPage;
						break;
					case VdfPropertyType.Enum:
						propertyValueTabControl.SelectedTab = enumValueTabPage;
						break;
					default:
						propertyValueTabControl.SelectedTab = uintValueTabPage;
						break;
				}

				RemoveOtherTabs();
			}
		}

		private void OnRelativeTimeValueChanged(DateTime? value)
		{
			if (value != null)
			{
				propertyValueTabControl.SelectedTab = relativeTimeValueTabPage;
				RemoveOtherTabs();
			}
		}

		public VdfPropertyEditorDialog()
		{
			InitializeComponent();

			if (!DesignMode)
			{
				this.ApplyWindowTheme();

				InitializeRelativeTimeDateTimePicker();
				InitializeDataBindings();
				RestoreStringValueSettings();
				UpdateControlStates();

				// NOTE: disabled for now, because I'd have to deep clone the in/out object.
				//cancelButton.TabStop = false;
				//cancelButton.Size = new System.Drawing.Size(0, 0);
			}
		}

		private void allowTabsCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.PropertyEditor_AllowTabs = allowTabsCheckBox.Checked;
		}

		private void allowNewLinesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.PropertyEditor_AllowNewLines = allowNewLinesCheckBox.Checked;
		}

		private void InitializeRelativeTimeDateTimePicker()
		{
			if (Properties.Settings.Default.DateTimePicker_UseVdfDefinitionFormat)
			{
				relativeTimeValueDateTimePicker.CustomFormat = Properties.Settings.Default.VdfDefinition_RelativeTime_Format;
			}
			else
			{
				relativeTimeValueDateTimePicker.CustomFormat = Properties.Settings.Default.DateTimePicker_CustomFormat;
			}

			relativeTimeValueDateTimePicker.Format = Properties.Settings.Default.DateTimePicker_Format;
		}

		private void InitializeDataBindings()
		{
			propertyNameTextBox.DataBindings.Add(nameof(propertyNameTextBox.Text), this, nameof(PropertyName), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

			vdfObjectPropertyListView.DataBindings.Add(nameof(vdfObjectPropertyListView.VdfObject), this, nameof(VdfObjectValue), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			stringValueTextBox.DataBindings.Add(nameof(stringValueTextBox.Text), this, nameof(StringValue), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			uintValueNumericUpDown.DataBindings.Add(nameof(uintValueNumericUpDown.Value), this, nameof(UIntValue), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			booleanValueCheckBox.DataBindings.Add(nameof(booleanValueCheckBox.Checked), this, nameof(UIntValue), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			enumValueComboBox.DataBindings.Add(nameof(enumValueComboBox.SelectedValue), this, nameof(UIntValue), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

			relativeTimeValueDateTimePicker.DataBindings.Add(nameof(relativeTimeValueDateTimePicker.Value), this, nameof(RelativeTimeValue), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

			allowTabsCheckBox.DataBindings.Add(nameof(allowTabsCheckBox.Checked), stringValueTextBox, nameof(stringValueTextBox.AcceptsTab), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			allowNewLinesCheckBox.DataBindings.Add(nameof(allowNewLinesCheckBox.Checked), stringValueTextBox, nameof(stringValueTextBox.AcceptsReturn), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void RestoreStringValueSettings()
		{
			if (Properties.Settings.Default.PropertyEditor_PersistTextSettings)
			{
				allowTabsCheckBox.Checked = Properties.Settings.Default.PropertyEditor_AllowTabs;
				allowNewLinesCheckBox.Checked = Properties.Settings.Default.PropertyEditor_AllowNewLines;
			}
		}

		private void RemoveOtherTabs()
		{
			if (!TypeIsReadOnly)
			{
				return;
			}

			for (var i = propertyValueTabControl.TabCount; i > 0; --i)
			{
				if (propertyValueTabControl.TabPages[i - 1] == propertyValueTabControl.SelectedTab)
				{
					continue;
				}

				propertyValueTabControl.TabPages.RemoveAt(i - 1);
			}
		}

		private void UpdateControlStates()
		{
			okButton.Enabled = CanConfirm();
		}

		private bool CanConfirm()
			=> PropertyName?.Length > 0;
	}
}
