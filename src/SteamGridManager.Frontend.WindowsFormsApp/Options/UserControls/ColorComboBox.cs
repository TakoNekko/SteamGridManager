using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls
{
	public partial class ColorComboBox : ComboBox, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		private readonly Lazy<Pen> blackPen = new Lazy<Pen>(() => new Pen(Brushes.Black));

		private int colorBoxWidth;

		public int ColorBoxWidth
		{
			get => colorBoxWidth;
			set
			{
				if (colorBoxWidth != value)
				{
					colorBoxWidth = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ColorBoxWidth)));
				}
			}
		}

		private Padding colorBoxMargin;

		public Padding ColorBoxMargin
		{
			get => colorBoxMargin;
			set
			{
				if (colorBoxMargin != value)
				{
					colorBoxMargin = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ColorBoxMargin)));
				}
			}
		}

		private bool hasError;

		public ColorComboBox()
		{
			InitializeComponent();

			//SystemColors.
			//var items = new ColorComboBoxItem();

			//DataSource = new ColorComboBoxItem
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (hasError)
			{
				base.OnDrawItem(e);
				return;
			}

			if (DataSource is null
				|| string.IsNullOrEmpty(DisplayMember)
				|| string.IsNullOrEmpty(ValueMember))
			{
				base.OnDrawItem(e);
				return;
			}

			if (!(DataSource is IList items))
			{
				hasError = true;
				throw new Exception($"DataSource does not point to a valid IList object.");
			}

			if (e.Index < 0
				|| e.Index > items.Count - 1)
			{
				hasError = true;
				throw new ArgumentOutOfRangeException(nameof(e.Index), e.Index, $"Index value must be within {0} and {items.Count - 1}.");
			}

			if (!(items[e.Index] is object item))
			{
				hasError = true;
				throw new Exception($"Item could not be queried.");
			}
			/*
			if (e.Index < 0
				|| !(Items[e.Index] is object item))
			{
				hasError = true;
				throw new Exception($"Invalid item at {e.Index}.");
			}
			*/
			var itemType = item.GetType();

			if (!(itemType.GetProperty(DisplayMember, BindingFlags.Public | BindingFlags.Instance).GetValue(item) is string name))
			{
				hasError = true;
				throw new Exception($"{DisplayMember} is not a valid display member.");
			}

			if (!(itemType.GetProperty(ValueMember, BindingFlags.Public | BindingFlags.Instance).GetValue(item) is Color value))
			{
				hasError = true;
				throw new Exception($"{ValueMember} is not a valid value member.");
			}

			e.DrawBackground();

			using (var graphics = e.Graphics)
			{
				var colorBoxHeight = e.Bounds.Height - ColorBoxMargin.Vertical;
				var colorBoxWidth = ColorBoxWidth == 0
					? colorBoxHeight - ColorBoxMargin.Horizontal
					: ColorBoxWidth;
				var colorRect = new Rectangle(
					e.Bounds.X + ColorBoxMargin.Left,
					e.Bounds.Y + ColorBoxMargin.Top,
					colorBoxWidth,
					colorBoxHeight);

				graphics.DrawString(name, Font, new SolidBrush(e.ForeColor), e.Bounds.X + colorBoxWidth + ColorBoxMargin.Horizontal, e.Bounds.Top);
				graphics.FillRectangle(new SolidBrush(value), colorRect);
				graphics.DrawRectangle(blackPen.Value, colorRect);
			}

			e.DrawFocusRectangle();
		}

		public class ColorComboBoxItem
		{
			public string Name { get; set; }
			public Color Value { get; set; }
		}
	}
}
