using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	using Helpers;

	// TODO: perhaps disable temporarily mainform & userpicturebox event listeners while editing paths.
	// FIXME: [BUG] editing the steam path can make the mainform's list to throw exceptions
	// FIXME: [BUG] resetting the steam path doesn't correctly raise a change event (needed to update the steam user id list)

	public partial class EnvironmentPathsOptionsPanel : UserControl
	{
		private readonly bool designMode;

		public EnvironmentPathsOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeDataSource();
				EnableEventListeners();
				InitializeDataBindings();
				SetDefaultPaths();
				UpdateOverlaysFolderCreateControlState();
				UpdateSteamFolderCreateControlState();

				//Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;
			}
		}

		private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
					case nameof(Properties.Settings.Default.SteamAppPath):
						UpdateSteamFolderCreateControlState();

						if (Directory.Exists(steamFolderPathBox.Path))
						{
							return;
						}
						//Directory.CreateDirectory(nonSteamAppIconCacheFolderPathBox.Path);

						InitializeSteamUserIDsDataSource();
						break;

					case nameof(Properties.Settings.Default.NonSteamAppIconCachePath):
						if (Directory.Exists(nonSteamAppIconCacheFolderPathBox.Path))
						{
							return;
						}
						//Directory.CreateDirectory(nonSteamAppIconCacheFolderPathBox.Path);
						break;

					case nameof(Properties.Settings.Default.BackupPath):
						if (Directory.Exists(backupFolderPathBox.Path))
						{
							return;
						}
						//Directory.CreateDirectory(backupFolderPathBox.Path);
						break;

					case nameof(Properties.Settings.Default.Overlays_Path):
						UpdateOverlaysFolderCreateControlState();
						break;

					case nameof(Properties.Settings.Default.Localization_Path):
						break;
			}
		}

		private void InitializeDataSource()
		{
			InitializeSteamUserIDsDataSource();
			InitializePathsDataSources();
		}

		private void InitializeSteamUserIDsDataSource()
		{
			var values = SteamUtils.EnumerateUserIDs();
			var empty = values.Count() == 0;

			steamUserIDComboBox.DataSource = values.ToArray();
			steamUserIDComboBox.SelectedIndex = empty ? -1 : 0;
			steamUserIDComboBox.Enabled = !empty;

			if (empty)
			{
				steamUserIDComboBox.DataBindings.Clear();
				Properties.Settings.Default.SteamUserID = 0;
			}
			else
			{
				steamUserIDComboBox.DataBindings.Add(nameof(steamUserIDComboBox.SelectedItem), Properties.Settings.Default, nameof(Properties.Settings.Default.SteamUserID), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			}
		}

		private void InitializePathsDataSources()
		{
			/*
			steamFolderPathBox.DataSource = Properties.Settings.Default;
			steamFolderPathBox.DataMember = nameof(Properties.Settings.Default.SteamAppPath);

			nonSteamAppIconCacheFolderPathBox.DataSource = Properties.Settings.Default;
			nonSteamAppIconCacheFolderPathBox.DataMember = nameof(Properties.Settings.Default.NonSteamAppIconCachePath);

			backupFolderPathBox.DataSource = Properties.Settings.Default;
			backupFolderPathBox.DataMember = nameof(Properties.Settings.Default.BackupPath);

			overlaysFolderPathBox.DataSource = Properties.Settings.Default;
			overlaysFolderPathBox.DataMember = nameof(Properties.Settings.Default.Overlays_Path);

			localizationFolderPathBox.DataSource = Properties.Settings.Default;
			localizationFolderPathBox.DataMember = nameof(Properties.Settings.Default.Localization_Path);
			*/
		}

		private void InitializeDataBindings()
		{
			steamFolderPathBox.DataBindings.Add(nameof(steamFolderPathBox.Path), Properties.Settings.Default, nameof(Properties.Settings.Default.SteamAppPath), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			nonSteamAppIconCacheFolderPathBox.DataBindings.Add(nameof(nonSteamAppIconCacheFolderPathBox.Path), Properties.Settings.Default, nameof(Properties.Settings.Default.NonSteamAppIconCachePath), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			backupFolderPathBox.DataBindings.Add(nameof(backupFolderPathBox.Path), Properties.Settings.Default, nameof(Properties.Settings.Default.BackupPath), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			overlaysFolderPathBox.DataBindings.Add(nameof(overlaysFolderPathBox.Path), Properties.Settings.Default, nameof(Properties.Settings.Default.Overlays_Path), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
			localizationFolderPathBox.DataBindings.Add(nameof(localizationFolderPathBox.Path), Properties.Settings.Default, nameof(Properties.Settings.Default.Localization_Path), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void EnableEventListeners()
		{
			steamFolderPathBox.PropertyChanged += SteamFolderPathBox_PropertyChanged;
			nonSteamAppIconCacheFolderPathBox.PropertyChanged += NonSteamAppIconCacheFolderPathBox_PropertyChanged;
			backupFolderPathBox.PropertyChanged += BackupFolderPathBox_PropertyChanged;
			overlaysFolderPathBox.PropertyChanged += OverlaysFolderPathBox_PropertyChanged;
			localizationFolderPathBox.PropertyChanged += LocalizationFolderPathBox_PropertyChanged;
		}

		private void SetDefaultPaths()
		{
			steamFolderPathBox.DefaultPath = SteamUtils.FindSteamPathFromProgramFiles() ?? SteamUtils.FindSteamPathFromRegistry();
			nonSteamAppIconCacheFolderPathBox.DefaultPath = SteamUtils.GetDefaultNonSteamAppIconCacheFolder();
			backupFolderPathBox.DefaultPath = SteamUtils.GetDefaultBackupFolder();
			overlaysFolderPathBox.DefaultPath = AssetOverlayUtils.GetDefaultOverlaysRootFolder();
			// TODO: add a LocalizationUtils class
			//localizationFolderPathBox.DefaultPath = LocalizationUtils.GetDefaultLocalizationFolder();
		}

		private void SteamFolderPathBox_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(steamFolderPathBox.Path):
					UpdateSteamFolderCreateControlState();
					InitializeSteamUserIDsDataSource();
					break;
			}
		}

		private void NonSteamAppIconCacheFolderPathBox_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(nonSteamAppIconCacheFolderPathBox.Path):
					break;
			}
		}

		private void BackupFolderPathBox_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(backupFolderPathBox.Path):
					break;
			}
		}

		private void OverlaysFolderPathBox_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(overlaysFolderPathBox.Path):
					UpdateOverlaysFolderCreateControlState();
					break;
			}
		}

		private void LocalizationFolderPathBox_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(localizationFolderPathBox.Path):
					break;
			}
		}

		private void UpdateSteamFolderCreateControlState()
		{
			steamFolderCreateButton.Enabled = IsDirectoryEmpty(steamFolderPathBox.Path);
		}

		private void UpdateOverlaysFolderCreateControlState()
		{
			overlaysFolderCreateButton.Enabled = IsDirectoryEmpty(overlaysFolderPathBox.Path);
		}

		private void UpdateSteamFolderSelectControlState()
		{
			steamFolderPathBox.SelectButton.Enabled = Directory.Exists(steamFolderPathBox.Path);
		}

		private void UpdateOverlaysFolderSelectControlState()
		{
			overlaysFolderPathBox.SelectButton.Enabled = Directory.Exists(overlaysFolderPathBox.Path);
		}

		private static bool IsDirectoryEmpty(string path)
		{
			return !Directory.Exists(path)
				|| !Directory.EnumerateFileSystemEntries(path).Any();
		}

		private void steamFolderCreateButton_Click(object sender, EventArgs e)
		{
			var paths = new string[]
			{
				Path.Combine(steamFolderPathBox.Path, "appcache", "librarycache"),
				Path.Combine(steamFolderPathBox.Path, "userdata", "1", "config", "grid"),
			};

			foreach (var path in paths)
			{
				Directory.CreateDirectory(path);
			}

			UpdateSteamFolderSelectControlState();
			InitializeSteamUserIDsDataSource();
		}

		private void overlaysFolderCreateButton_Click(object sender, EventArgs e)
		{
			foreach (var assetTypeName in Enum.GetNames(typeof(AssetType)))
			{
				if (assetTypeName.Equals("None"))
				{
					continue;
				}

				var fullPath = Path.Combine(overlaysFolderPathBox.Path, assetTypeName);

				Directory.CreateDirectory(fullPath);
			}

			UpdateOverlaysFolderSelectControlState();
		}
	}
}
