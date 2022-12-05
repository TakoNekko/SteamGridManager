using Steam.Vdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.UserControls
{
	using Extensions.Control;

	public partial class UserPictureBoxGroup : UserControl, INotifyPropertyChanged
	{
		#region Events

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		#endregion

		#region Fields

		private readonly bool designMode;

		#endregion

		#region Properties

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
					if (!designMode)
					{
						OnShortcutsChanged();
					}
				}
			}
		}

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
					if (!designMode)
					{
						OnAppInfosChanged();
					}
				}
			}
		}

		private CacheLocation cacheLocation;

		public CacheLocation CacheLocation
		{
			get => cacheLocation;
			set
			{
				if (cacheLocation != value)
				{
					cacheLocation = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CacheLocation)));
					if (!designMode)
					{
						OnCacheLocationChanged();
					}
				}
			}
		}

		private AssetOverlayCollection assetOverlays;

		public AssetOverlayCollection AssetOverlays
		{
			get => assetOverlays;
			set
			{
				if (assetOverlays != value)
				{
					assetOverlays = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(AssetOverlays)));
					if (!designMode)
					{
						OnAssetOverlaysChanged();
					}
				}
			}
		}


		private bool asleep;

		public bool Asleep
		{
			get => asleep;
			set
			{
				if (asleep != value)
				{
					asleep = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Asleep)));
					if (!designMode)
					{
						OnAsleepChanged();
					}
				}
			}
		}

		private AssetFileWatcherSystem assetFileWatcherSystem;

		public AssetFileWatcherSystem AssetFileWatcherSystem
		{
			get => assetFileWatcherSystem;
			set
			{
				if (assetFileWatcherSystem != value)
				{
					assetFileWatcherSystem = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(AssetFileWatcherSystem)));
					OnAssetFileWatcherSystemChanged();
				}
			}
		}

		private readonly UserPictureBox[] userPictureBoxes;

		public IEnumerable<UserPictureBox> UserPictureBoxes => userPictureBoxes;

		#endregion

		#region Getters

		public TabControl TabControl => tabControl;

		public TabPage TabPage => tabPage;

		public Panel Panel => innerPanel;

		public UserPictureBox LibraryCapsule => libraryCapsuleUserPictureBox;

		public UserPictureBox HeroGraphic => heroGraphicUserPictureBox;

		public UserPictureBox LogoUserPictureBox => logoUserPictureBox;

		public UserPictureBox HeaderUserPictureBox => headerUserPictureBox;

		public UserPictureBox IconUserPictureBox => iconUserPictureBox;

		#endregion

		#region Constructors

		public UserPictureBoxGroup()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				this.SetDoubleBuffered(true);

				userPictureBoxes = new UserPictureBox[]
					{
						libraryCapsuleUserPictureBox,
						heroGraphicUserPictureBox,
						logoUserPictureBox,
						headerUserPictureBox,
						iconUserPictureBox,
					};

				libraryCapsuleUserPictureBox.Visible = Properties.Settings.Default.View_Show_LibraryCapsule;
				heroGraphicUserPictureBox.Visible = Properties.Settings.Default.View_Show_HeroGraphic;
				logoUserPictureBox.Visible = Properties.Settings.Default.View_Show_Logo;
				headerUserPictureBox.Visible = Properties.Settings.Default.View_Show_Header;
				iconUserPictureBox.Visible = Properties.Settings.Default.View_Show_Icon;

				Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;

				tabPage.BackColor = Properties.Settings.Default.Details_BackColor;
			}
		}

		#endregion

		#region Property Changes

		protected virtual void OnShortcutsChanged()
		{
			foreach (var userPictureBox in userPictureBoxes)
			{
				userPictureBox.Shortcuts = Shortcuts;
			}
		}

		protected virtual void OnAppInfosChanged()
		{
			foreach (var userPictureBox in userPictureBoxes)
			{
				userPictureBox.AppInfos = AppInfos;
			}
		}

		protected virtual void OnCacheLocationChanged()
		{
			foreach (var userPictureBox in userPictureBoxes)
			{
				userPictureBox.CacheLocation = CacheLocation;
			}
		}

		protected virtual void OnAssetOverlaysChanged()
		{
			foreach (var userPictureBox in userPictureBoxes)
			{
				userPictureBox.AssetOverlays = AssetOverlays;
			}
		}

		protected virtual void OnAsleepChanged()
		{
			foreach (var userPictureBox in userPictureBoxes)
			{
				userPictureBox.Asleep = Asleep;
			}
		}

		protected virtual void OnAssetFileWatcherSystemChanged()
		{
			foreach (var userPictureBox in userPictureBoxes)
			{
				userPictureBox.AssetFileWatcherSystem = AssetFileWatcherSystem;
			}
		}

		#endregion

		#region Class Overrides

		protected override void OnHandleDestroyed(EventArgs e)
		{
			if (!designMode)
			{
				Properties.Settings.Default.PropertyChanged -= Default_PropertyChanged;
			}

			base.OnHandleDestroyed(e);
		}

		#endregion

		#region Event Handlers

		#region Settings

		private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (!Properties.Settings.Default.IsReady)
			{
				return;
			}

			switch (e.PropertyName)
			{
				case nameof(Properties.Settings.Default.Details_BackColor):
					tabPage.BackColor = Properties.Settings.Default.Details_BackColor;
					break;
				case nameof(Properties.Settings.Default.View_Show_LibraryCapsule):
					libraryCapsuleUserPictureBox.Visible = Properties.Settings.Default.View_Show_LibraryCapsule;
					break;
				case nameof(Properties.Settings.Default.View_Show_HeroGraphic):
					heroGraphicUserPictureBox.Visible = Properties.Settings.Default.View_Show_HeroGraphic;
					break;
				case nameof(Properties.Settings.Default.View_Show_Logo):
					logoUserPictureBox.Visible = Properties.Settings.Default.View_Show_Logo;
					break;
				case nameof(Properties.Settings.Default.View_Show_Header):
					headerUserPictureBox.Visible = Properties.Settings.Default.View_Show_Header;
					break;
				case nameof(Properties.Settings.Default.View_Show_Icon):
					iconUserPictureBox.Visible = Properties.Settings.Default.View_Show_Icon;
					break;
			}
		}

		#endregion

		#endregion

		public void Clear()
		{
			foreach (var box in userPictureBoxes)
			{
				box.Clear();
			}
		}
	}
}
