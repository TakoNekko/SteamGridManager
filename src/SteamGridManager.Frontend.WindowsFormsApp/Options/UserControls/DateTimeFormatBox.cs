using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls
{
	using Extensions.TextBox;

	public partial class DateTimeFormatBox : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		private object dataSource;

		public object DataSource
		{
			get => dataSource;
			set
			{
				if (dataSource != value)
				{
					dataSource = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DataSource)));

					InitializeDataBindings();
				}
			}
		}

		private string dataMember;

		public string DataMember
		{
			get => dataMember;
			set
			{
				if (dataMember != value)
				{
					dataMember = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DataMember)));

					InitializeDataBindings();
				}
			}
		}

		private CultureInfo cultureInfo;

		public CultureInfo CultureInfo
		{
			get => cultureInfo;
			set
			{
				if (cultureInfo != value)
				{
					cultureInfo = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
					OnCultureInfoChanged();
				}
			}
		}

		private void OnCultureInfoChanged()
		{
			UpdateExample();
		}

		private void OnFormatOptionChanged()
		{
			//standardFormatPanel.Enabled = standardFormatRadioButton.Checked;
			//customFormatPanel.Enabled = customFormatRadioButton.Checked;
		}

		private readonly bool designMode;

		public DateTime ExampleDateTime => DateTime.Now;

		public DateTimeFormatBox()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeStandardFormatComboBox();
				InitializeDataBindings();

				PopulateCustomFormatSpecifierContextMenuRecursive(customFormatSpecifiersContextMenuStrip.Items);
			}
		}

		private void ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var toolStripMenuItem = (ToolStripMenuItem)sender;

			customFormatTextBox.InsertText(toolStripMenuItem.Tag as string);
		}

		private void formatSpecifierButton_Click(object sender, EventArgs e)
		{
			if (customFormatSpecifierButton.ContextMenuStrip is null)
			{
				return;
			}

			customFormatSpecifierButton.ContextMenuStrip.Show(Cursor.Position);
		}

		private void defaultFormatRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (defaultFormatRadioButton.Checked)
			{
				OnFormatOptionChanged();

				customFormatTextBox.Text = "";
			}
		}

		private void standardFormatRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (standardFormatRadioButton.Checked)
			{
				if (standardFormatSpecifierComboBox.SelectedIndex == -1)
				{
					standardFormatSpecifierComboBox.SelectedIndex = 0;
				}

				OnFormatOptionChanged();
			}
		}

		private void customFormatRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (customFormatRadioButton.Checked)
			{
				OnFormatOptionChanged();
			}
		}

		private void customFormatTextBox_TextChanged(object sender, EventArgs e)
		{
			UpdateExample();

			if (customFormatTextBox.Focused
				&& customFormatRadioButton.Checked)
			{
				return;
			}

			CheckRadioGroup();
		}

		private void PopulateCustomFormatSpecifierContextMenuRecursive(ToolStripItemCollection items)
		{
			foreach (ToolStripItem toolStripItem in items)
			{
				if (toolStripItem is ToolStripMenuItem toolStripMenuItem)
				{
					if (toolStripMenuItem.Tag is string formatSpecifier)
					{
						toolStripMenuItem.Click += ToolStripMenuItem_Click;
					}

					if (toolStripMenuItem.HasDropDownItems)
					{
						PopulateCustomFormatSpecifierContextMenuRecursive(toolStripMenuItem.DropDownItems);
					}
				}
			}
		}

		private void CheckRadioGroup()
		{
			defaultFormatRadioButton.Checked = string.IsNullOrEmpty(customFormatTextBox.Text);
			standardFormatRadioButton.Checked = standardFormatSpecifierComboBox.Tag is IEnumerable<string> values && values.Contains(customFormatTextBox.Text);
			customFormatRadioButton.Checked = !defaultFormatRadioButton.Checked && !standardFormatRadioButton.Checked;
		}

		private void InitializeStandardFormatComboBox()
		{
			var items = new StandardFormatSpecifierComboBoxItem[]
				{
					new StandardFormatSpecifierComboBoxItem { Name = "Short date", Value = "d" },
					new StandardFormatSpecifierComboBoxItem { Name = "Long date", Value = "D" },
					new StandardFormatSpecifierComboBoxItem { Name = "Full date / short time", Value = "f" },
					new StandardFormatSpecifierComboBoxItem { Name = "Full date / long time", Value = "F" },
					new StandardFormatSpecifierComboBoxItem { Name = "General date / short time", Value = "g" },
					new StandardFormatSpecifierComboBoxItem { Name = "General date / long time", Value = "G" },
					new StandardFormatSpecifierComboBoxItem { Name = "Month / day", Value = "m" },
					new StandardFormatSpecifierComboBoxItem { Name = "Round-trip date / time", Value = "o" },
					new StandardFormatSpecifierComboBoxItem { Name = "RFC1123", Value = "r" },
					new StandardFormatSpecifierComboBoxItem { Name = "Sortable date / time", Value = "s" },
					new StandardFormatSpecifierComboBoxItem { Name = "Short time", Value = "t" },
					new StandardFormatSpecifierComboBoxItem { Name = "Long time", Value = "T" },
					new StandardFormatSpecifierComboBoxItem { Name = "Universal sortable date / time", Value = "u" },
					new StandardFormatSpecifierComboBoxItem { Name = "Universal full date / time", Value = "U" },
					new StandardFormatSpecifierComboBoxItem { Name = "Year month", Value = "y" },
				};

			standardFormatSpecifierComboBox.Tag = items.Select(x => x.Value);
			standardFormatSpecifierComboBox.DataSource = items;
			standardFormatSpecifierComboBox.DisplayMember = nameof(StandardFormatSpecifierComboBoxItem.Name);
			standardFormatSpecifierComboBox.ValueMember = nameof(StandardFormatSpecifierComboBoxItem.Value);
		}

		private void InitializeDataBindings()
		{
			if (DataSource != null
				&& !string.IsNullOrEmpty(DataMember))
			{
				customFormatTextBox.DataBindings.Clear();
				customFormatTextBox.DataBindings.Add(nameof(customFormatTextBox.Text), DataSource, DataMember, formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

				standardFormatSpecifierComboBox.DataBindings.Add(nameof(standardFormatSpecifierComboBox.SelectedValue), DataSource, DataMember, formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

				UpdateExample();
				CheckRadioGroup();
			}
		}

		private void UpdateExample()
		{
			try
			{
				exampleLabel.ForeColor = Color.FromKnownColor(KnownColor.GrayText);
				exampleLabel.Text = $"Example: {DateTime.Now.ToString(customFormatTextBox.Text, CultureInfo)}";
			}
			catch (Exception ex)
			{
				exampleLabel.ForeColor = Color.Red;
				exampleLabel.Text = ex.Message;
			}
		}
	}

	public class StandardFormatSpecifierComboBoxItem
	{
		public string Name { get; set; }
		public string Value { get; set; }
	}

	public enum DateTimeFormatSpecifierType
	{
		None,
		Standard,
		Custom,
	}
}
