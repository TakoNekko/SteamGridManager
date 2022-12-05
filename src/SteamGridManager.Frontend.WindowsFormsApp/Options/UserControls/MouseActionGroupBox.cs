using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls
{
	public partial class MouseActionGroupBox : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		private readonly bool designMode;

		private readonly Item mouseLeftClickAction;
		private readonly Item mouseLeftDoubleClickAction;

		private readonly Item mouseRightClickAction;
		private readonly Item mouseRightDoubleClickAction;

		private readonly Item mouseMiddleClickAction;
		private readonly Item mouseMiddleDoubleClickAction;

		public Item[] Actions
			=> new Item[]
			{
				mouseLeftClickAction, mouseLeftDoubleClickAction,
				mouseRightClickAction, mouseRightDoubleClickAction,
				mouseMiddleClickAction, mouseMiddleDoubleClickAction,
			};

		public Item MouseLeftClickAction => mouseLeftClickAction;
		public Item MouseLeftDoubleClickAction => mouseLeftDoubleClickAction;

		public Item MouseRightClickAction => mouseRightClickAction;
		public Item MouseRightDoubleClickAction => mouseRightDoubleClickAction;

		public Item MouseMiddleClickAction => mouseMiddleClickAction;
		public Item MouseMiddleDoubleClickAction => mouseMiddleDoubleClickAction;

		private bool mouseLeftClickActionVisible = true;

		public bool MouseLeftClickActionVisible
		{
			get => mouseLeftClickActionVisible;
			set
			{
				if (mouseLeftClickActionVisible != value)
				{
					mouseLeftClickActionVisible = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(MouseLeftClickActionVisible)));
					mouseLeftClickActionLabel.Visible = value;
					mouseLeftClickActionComboBox.Visible = value;
					spacing1.Visible = value;
				}
			}
		}

		private bool mouseLeftDoubleClickActionVisible = true;

		public bool MouseLeftDoubleClickActionVisible
		{
			get => mouseLeftDoubleClickActionVisible;
			set
			{
				if (mouseLeftDoubleClickActionVisible != value)
				{
					mouseLeftDoubleClickActionVisible = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(MouseLeftDoubleClickActionVisible)));
					mouseLeftDoubleClickActionLabel.Visible = value;
					mouseLeftDoubleClickActionComboBox.Visible = value;
					spacing2.Visible = value;
				}
			}
		}

		private bool mouseRightClickActionVisible = true;

		public bool MouseRightClickActionVisible
		{
			get => mouseRightClickActionVisible;
			set
			{
				if (mouseRightClickActionVisible != value)
				{
					mouseRightClickActionVisible = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(MouseRightClickActionVisible)));
					mouseRightClickActionLabel.Visible = value;
					mouseRightClickActionComboBox.Visible = value;
					spacing3.Visible = value;
				}
			}
		}

		private bool mouseRightDoubleClickActionVisible = true;

		public bool MouseRightDoubleClickActionVisible
		{
			get => mouseRightDoubleClickActionVisible;
			set
			{
				if (mouseRightDoubleClickActionVisible != value)
				{
					mouseRightDoubleClickActionVisible = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(MouseRightDoubleClickActionVisible)));
					mouseRightDoubleClickActionLabel.Visible = value;
					mouseRightDoubleClickActionComboBox.Visible = value;
					spacing4.Visible = value;
				}
			}
		}

		private bool mouseMiddleClickActionVisible = true;

		public bool MouseMiddleClickActionVisible
		{
			get => mouseMiddleClickActionVisible;
			set
			{
				if (mouseMiddleClickActionVisible != value)
				{
					mouseMiddleClickActionVisible = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(MouseMiddleClickActionVisible)));
					mouseMiddleClickActionLabel.Visible = value;
					mouseMiddleClickActionComboBox.Visible = value;
					spacing5.Visible = value;
				}
			}
		}

		private bool mouseMiddleDoubleClickActionVisible = true;

		public bool MouseMiddleDoubleClickActionVisible
		{
			get => mouseMiddleDoubleClickActionVisible;
			set
			{
				if (mouseMiddleDoubleClickActionVisible != value)
				{
					mouseMiddleDoubleClickActionVisible = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(MouseMiddleDoubleClickActionVisible)));
					mouseMiddleDoubleClickActionLabel.Visible = value;
					mouseMiddleDoubleClickActionComboBox.Visible = value;
				}
			}
		}

		public MouseActionGroupBox()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				mouseLeftClickAction = new Item { Label = mouseLeftClickActionLabel, ComboBox = mouseLeftClickActionComboBox };
				mouseLeftDoubleClickAction = new Item { Label = mouseLeftDoubleClickActionLabel, ComboBox = mouseLeftDoubleClickActionComboBox };

				mouseRightClickAction = new Item { Label = mouseRightClickActionLabel, ComboBox = mouseRightClickActionComboBox };
				mouseRightDoubleClickAction = new Item { Label = mouseRightDoubleClickActionLabel, ComboBox = mouseRightDoubleClickActionComboBox };

				mouseMiddleClickAction = new Item { Label = mouseMiddleClickActionLabel, ComboBox = mouseMiddleClickActionComboBox };
				mouseMiddleDoubleClickAction = new Item { Label = mouseMiddleDoubleClickActionLabel, ComboBox = mouseMiddleDoubleClickActionComboBox };
			}
		}

		public class Item
		{
			public Label Label { get; set; }
			public ComboBox ComboBox { get; set; }
		}

	}
}
