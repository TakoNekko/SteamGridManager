using Steam.Vdf;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp
{
	public partial class AppPropertiesDialog : Form, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		private InfoBlock info;

		public InfoBlock Info
		{
			get => info;
			set
			{
				if (info != value)
				{
					info = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Info)));
					OnInfoChanged();
				}
			}
		}

		private void OnInfoChanged()
		{
			if (info is null)
			{
				return;
			}

			vdfObjectPropertyListView.Path = info.Path;

			if (info.AppID != 0)
			{
				vdfObjectPropertyListView.AppInfos = info.AppInfos;
				vdfObjectPropertyListView.Shortcuts = info.Shortcuts;
				vdfObjectPropertyListView.AppID = info.AppID;
			}

			if (info.VdfObject != null)
			{
				vdfObjectPropertyListView.VdfObject = info.VdfObject;
			}
		}

		public AppPropertiesDialog()
		{
			InitializeComponent();
		}

		public class InfoBlock
		{
			public string Path { get; set; }
			public AppInfos AppInfos { get; set; }
			public Shortcuts Shortcuts { get; set; }
			public ulong AppID { get; set; }
			public VdfObject VdfObject { get; set; }
		}
	}
}
