using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp
{
	public partial class ComboBoxDialog : Form, INotifyPropertyChanged
	{
		#region Events

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		#endregion

		#region Getters

		public Label Label => label;

		public ComboBox ComboBox => mainComboBox;

		#endregion

		#region Properties

		private IReadOnlyList<object> values;

		public IReadOnlyList<object> Values
		{
			get => values;
			set
			{
				if (values != value)
				{
					values = value;
					OnValuesChanged();
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Values)));
				}
			}
		}

		private object selectedValue;

		public object SelectedValue
		{
			get => selectedValue;
			set
			{
				if (selectedValue != value)
				{
					selectedValue = value;
					OnSelectedValueChanged();
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedValue)));
				}
			}
		}

		#endregion

		#region Property Changes

		private void OnValuesChanged()
		{
			mainComboBox.DataSource = Values;
		}

		private void OnSelectedValueChanged()
		{
		}

		#endregion

		#region Constructors

		public ComboBoxDialog()
		{
			InitializeComponent();

			if (!DesignMode)
			{
				mainComboBox.DataBindings.Add(nameof(mainComboBox.Text), this, nameof(SelectedValue), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			}
		}

		#endregion

		#region Methods
		
		public static DialogResult Show(IWin32Window owner, string value, string[] values, string caption)
		{
			using (var dialog = new ComboBoxDialog
			{
			})
			{
				if (!string.IsNullOrEmpty(caption))
				{
					dialog.Text = caption;
				}

				if (!string.IsNullOrEmpty(value))
				{
					dialog.SelectedValue = value;
				}

				if (values != null)
				{
					dialog.Values = values;
				}

				return dialog.ShowDialog(owner);
			}
		}

		#endregion
	}
}
