using Steam.Vdf;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	public partial class EnvironmentDatabaseOptionsPanel : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		private AppInfos appInfos;
		public AppInfos AppInfos
		{
			get => appInfos;
			set
			{
				if (appInfos != value)
				{
					appInfos = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(AppInfos)));
					OnAppInfosChanged();
				}
			}
		}


		private Shortcuts shortcuts;

		public Shortcuts Shortcuts
		{
			get => shortcuts;
			set
			{
				if (shortcuts != value)
				{
					shortcuts = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Shortcuts)));
					OnShortcutsChanged();
				}
			}
		}

		public EnvironmentDatabaseOptionsPanel()
		{
			InitializeComponent();

			appInfoDatabaseSettingBox.Text = "&App Info";
			appInfoDatabaseSettingBox.LoadButton.Click += AppInfoDatabase_LoadButton_Click;
			appInfoDatabaseSettingBox.UnloadButton.Click += AppInfoDatabase_UnloadButton_Click;

			shortcutsDatabaseSettingBox.Text = "&Shortcuts";
			shortcutsDatabaseSettingBox.LoadButton.Click += Shortcuts_LoadButton_Click;
			shortcutsDatabaseSettingBox.UnloadButton.Click += Shortcuts_UnloadButton_Click;
		}

		private void OnAppInfosChanged()
		{

		}

		private void OnShortcutsChanged()
		{

		}

		private void AppInfoDatabase_UnloadButton_Click(object sender, EventArgs e)
		{

		}

		private void AppInfoDatabase_LoadButton_Click(object sender, EventArgs e)
		{
		}

		private void Shortcuts_UnloadButton_Click(object sender, EventArgs e)
		{
		}

		private void Shortcuts_LoadButton_Click(object sender, EventArgs e)
		{
		}
	}
}
