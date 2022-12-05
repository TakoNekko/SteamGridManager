using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls
{
	public partial class ColorBox : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public Button ColorButton => colorButton;
		public Button ResetButton => resetButton;


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


		private Color defaultColor = Color.Transparent;

		public Color DefaultColor
		{
			get => defaultColor;
			set
			{
				if (defaultColor != value)
				{
					defaultColor = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DefaultColor)));
				}
			}
		}

		public ColorBox()
		{
			InitializeComponent();
		}

		private void InitializeDataBindings()
		{
			if (DataSource != null
				&& !string.IsNullOrEmpty(DataMember))
			{
				colorButton.DataBindings.Clear();
				colorButton.DataBindings.Add(nameof(colorButton.BackColor), DataSource, DataMember, formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
				colorButton.DataBindings.Add(nameof(colorButton.Text), colorButton, nameof(colorButton.BackColor), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

				colorButton.BackColorChanged += ColorButton_BackColorChanged;
			}
		}

		private void ColorButton_BackColorChanged(object sender, EventArgs e)
		{
			if (colorButton.BackColor.IsEmpty)
			{
				colorButton.ForeColor = Button.DefaultForeColor;
			}
			else
			{
				var nonLinearBrightness = (colorButton.BackColor.R * .2126f + colorButton.BackColor.G * .7152f + colorButton.BackColor.B * .0722f) / 256f;

				colorButton.ForeColor = nonLinearBrightness < .54f ? Color.White : Color.Black;
			}

			resetButton.Enabled = ColorButton.BackColor != DefaultBackColor;
		}

		private void resetButton_Click(object sender, EventArgs e)
		{
			Reset();
		}

		private void colorButton_Click(object sender, EventArgs e)
		{
			if (TryChooseColor(colorButton.BackColor, out var value))
			{
				colorButton.BackColor = value;
			}
		}

		private void Reset()
		{
			colorButton.BackColor = DefaultColor;
		}

		private bool TryChooseColor(Color inputColor, out Color outputColor)
		{
			if (ModifierKeys == Keys.Alt)
			{
				outputColor = DefaultColor;
				return true;
			}

			using (var dialog = new ColorDialog
			{
				Color = inputColor == Color.Transparent ? Color.White : inputColor,
				CustomColors = CustomColorsCollection.Instance.Get("General")?.Select(x => x.ToArgb()).ToArray(),
			})
			{
				if (dialog.ShowDialog(ParentForm) == DialogResult.OK)
				{
					if (dialog.CustomColors != null)
					{
						CustomColorsCollection.Instance.Set("General", dialog.CustomColors.Select(argb => Color.FromArgb(argb)).ToArray());
					}

					if (ModifierKeys == Keys.Alt)
					{
						outputColor = DefaultColor;
					}
					else
					{
						outputColor = dialog.Color;
					}

					return true;
				}
			}

			outputColor = DefaultColor;
			return false;
		}
	}
}
