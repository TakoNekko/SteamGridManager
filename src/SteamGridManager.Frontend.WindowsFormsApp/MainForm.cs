using Steam.Vdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp
{
	using Extensions.Control;
	using Extensions.ExternalApplication;
	using Extensions.ListView;
	using Helpers;

	public partial class MainForm : Form
	{
		#region Types

		public class VdfColumn
		{
			public string PathFilter { get; set; }
			public string ValueFilter { get; set; }
			public VdfPropertyType PropertyType { get; set; }
		}

		public class ListViewItemSorter : Comparer<ListViewItem>
		{
			public ListView Owner { get; }
			public ColumnHeaderType SortColumn { get; set; }
			public string SortFilter { get; set; }

			public ListViewItemSorter(ListView owner) { Owner = owner; }

			public override int Compare(ListViewItem x, ListViewItem y)
			{
				if (Owner.Sorting == SortOrder.None)
				{
					return 0;
				}

				var a = Owner.Sorting == SortOrder.Ascending ? x : y;
				var b = Owner.Sorting == SortOrder.Ascending ? y : x;

				var columnIndex = GetColumnIndex(SortFilter);

				if (columnIndex != -1)
				{
					if (Owner.Columns[columnIndex].Tag is VdfColumn vdfColumn)
					{
						if (a.SubItems[columnIndex].Tag is null
							&& b.SubItems[columnIndex].Tag is null)
							return 0;
						else if (a.SubItems[columnIndex].Tag is null)
							return -1;
						else if (b.SubItems[columnIndex].Tag is null)
							return 1;

						switch (vdfColumn.PropertyType)
						{
							case VdfPropertyType.Object:
								if (a.SubItems[columnIndex].Tag is null)
									return -1;
								// not currently supported.
								return 0;
							//return ((VdfObject)a.SubItems[columnIndex].Tag).Count - ((VdfObject)b.SubItems[columnIndex].Tag).Count;
							case VdfPropertyType.String:
								if (a.SubItems[columnIndex].Tag is null)
									return -1;
								return ((string)a.SubItems[columnIndex].Tag).CompareTo((string)b.SubItems[columnIndex].Tag);
							case VdfPropertyType.Boolean:
							case VdfPropertyType.UInt:
								return ((uint)a.SubItems[columnIndex].Tag).CompareTo((uint)b.SubItems[columnIndex].Tag);
							case VdfPropertyType.RelativeTime:
								if ((uint)a.SubItems[columnIndex].Tag == 0
									&& (uint)b.SubItems[columnIndex].Tag == 0)
									return 0;
								else if ((uint)a.SubItems[columnIndex].Tag == 0)
									return -1;
								else if ((uint)b.SubItems[columnIndex].Tag == 0)
									return 1;
								return ((uint)a.SubItems[columnIndex].Tag).ToRelativeTime().CompareTo(((uint)b.SubItems[columnIndex].Tag).ToRelativeTime());
							default:
								return $"{a.SubItems[columnIndex].Tag}".CompareTo($"{b.SubItems[columnIndex].Tag}");
						}
					}
				}


				columnIndex = GetColumnIndex(SortColumn);

				if (columnIndex != -1)
				{
					switch (SortColumn)
					{
						case ColumnHeaderType.AppID:
							return ((ulong)a.SubItems[columnIndex].Tag).CompareTo((ulong)b.SubItems[columnIndex].Tag);
						case ColumnHeaderType.Title:
							if (a.SubItems[columnIndex].Tag is null)
								return 0;
							return ((string)a.SubItems[columnIndex].Tag).CompareTo((string)b.SubItems[columnIndex].Tag);
						case ColumnHeaderType.UserDataLibraryCapsule:
						case ColumnHeaderType.UserDataHeroGraphic:
						case ColumnHeaderType.UserDataLogo:
						case ColumnHeaderType.UserDataHeader:
						case ColumnHeaderType.UserDataIcon:
						case ColumnHeaderType.LibraryCacheLibraryCapsule:
						case ColumnHeaderType.LibraryCacheHeroGraphic:
						case ColumnHeaderType.LibraryCacheLogo:
						case ColumnHeaderType.LibraryCacheHeader:
						case ColumnHeaderType.LibraryCacheIcon:
							return ((bool)a.SubItems[columnIndex].Tag).CompareTo((bool)b.SubItems[columnIndex].Tag);
						default:
							throw new ArgumentOutOfRangeException(nameof(SortColumn));
					}
				}

				throw new ArgumentOutOfRangeException(nameof(SortColumn), $"Cannot sort by {SortColumn}. The column might be currently filtered.");
			}

			private int GetColumnIndex(ColumnHeaderType type)
				=> GetColumnIndex(Owner, type);

			private int GetColumnIndex(string filter)
				=> GetColumnIndex(Owner, filter);

			public static int GetColumnIndex(ListView listView, ColumnHeaderType type, string filter)
			{
				var index = GetColumnIndex(listView, filter);

				if (index == -1)
				{
					index = GetColumnIndex(listView, type);
				}

				return index;
			}

			public static int GetColumnIndex(ListView listView, ColumnHeaderType type)
			{
				foreach (ColumnHeader column in listView.Columns)
				{
					if (column.Tag is ColumnHeaderType columnType)
					{
						if (columnType == type)
						{
							return column.Index;
						}
					}
				}

				return -1;
			}

			public static int GetColumnIndex(ListView listView, string filter)
			{
				if (string.IsNullOrEmpty(filter))
				{
					return -1;
				}

				foreach (ColumnHeader column in listView.Columns)
				{
					if (column.Tag is VdfColumn vdfColumn)
					{
						// TODO: use regex?

						if (filter.Equals(vdfColumn.PathFilter, StringComparison.InvariantCultureIgnoreCase))
						{
							return column.Index;
						}
					}
				}

				return -1;
			}
		}

		public class LibraryAssetsGroup
		{
			public LibraryAssets UserData { get; set; } = new LibraryAssets();
			public LibraryAssets LibraryCache { get; set; } = new LibraryAssets();
		}

		public class LibraryAssets
		{
			public bool LibraryCapsule { get; set; }
			public bool HeroGraphic { get; set; }
			public bool Logo { get; set; }
			public bool Header { get; set; }
			public bool Icon { get; set; }
		}

		public class IconCacheUpdateContext
		{
			public ulong AppID { get; set; }
			public string Path { get; set; }
			public string Url { get; set; }
			public string IconType { get; set; }
			public string IconExtension { get; set; }
		}

		public class NonSteamAppIconCacheUpdateContext
		{
			public ulong AppID { get; set; }
			public string UserIconFullName { get; set; }
			public string OriginalAssetFullName { get; set; }
		}

		#endregion

		#region Constants

		private static readonly string[] validUserIconAssetFileExtensions = new string[]
		{
			".png",
			".tga",
			".exe",
		};

		#endregion

		#region Fields

		private readonly WebClient webClient = new WebClient();
		private readonly AssetOverlayCollection assetOverlays = new AssetOverlayCollection();
		private readonly Dictionary<ulong, LibraryAssetsGroup> assets = new Dictionary<ulong, LibraryAssetsGroup>();
		private readonly List<ListViewItem> cachedListViewItems = new List<ListViewItem>();
		private readonly List<ListViewItem> filteredListViewItems = new List<ListViewItem>();
		private readonly List<ListViewItem> sortedListViewItems = new List<ListViewItem>();
		private readonly AppInfos appInfos = new AppInfos();
		private readonly Shortcuts shortcuts = new Shortcuts();
		private readonly string startupFilter;
		private readonly ulong startupAppID;
		private readonly CacheLocation startupCacheLocation;
		private readonly CacheLocation startupDetailsView;
		private bool isLoaded;
		private ulong? focusedAppID;
		private int initializationStage;

		#endregion

		#region Properties

		public AssetFileWatcherSystem AssetFileWatcherSystem => AssetFileWatcherSystem.Instance;

		#endregion

		#region Constructors

		public MainForm()
		{
			InitializeComponent();
			this.ApplyWindowTheme();

			if (!DesignMode)
			{
				isLoaded = false;
				startupFilter = Properties.Settings.Default.View_List_Filter;
				startupAppID = Properties.Settings.Default.AppID;
				startupCacheLocation = Properties.Settings.Default.List_CacheLocation;
				startupDetailsView = Properties.Settings.Default.Details_CacheLocation;

				Initialization("reloading configuration file", () =>
				{
					Properties.Settings.Default.IsReady = false;

					try
					{
						if (ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal) is var configuration
							&& !File.Exists(configuration.FilePath))
						{
							if (ToggleCollection.Instance.IsOn("Log/Verbose"))
							{
								Console.WriteLine("\tupgrading settings");
							}

							Properties.Settings.Default.Upgrade();
						}

						Properties.Settings.Default.Reload();
					}
					catch (Exception ex)
					{
						Program.LogError(ex);
					}

					EnsureSettingArraysAreInstantiated();
					EnsureTogglesAreRegistered();

					Properties.Settings.Default.IsReady = true;
					Properties.Settings.Default.AppID = 0;
					Properties.Settings.Default.List_CacheLocation = CacheLocation.None;
					Properties.Settings.Default.Details_CacheLocation = CacheLocation.None;
					Properties.Settings.Default.View_List_Filter = "";
				});

				Initialization("updating controls", () =>
					{
						Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;

						Application.ApplicationExit += Application_ApplicationExit;

						ExternalApplications.Items.ListChanged += Items_ListChanged;
						ExternalApplications.Items.AddingNew += Items_AddingNew;
						PopulateExternalApplicationsSubMenu();

						appIDListView.SetDoubleBuffered(true);

						splitContainer1.Panel2.Visible = false;

						if (Properties.Settings.Default.List_Filter_AutoCommit_Delay > 0
							&& Properties.Settings.Default.List_Filter_AutoCommit_Delay <= int.MaxValue)
						{
							filterTextBoxTimer.Interval = Properties.Settings.Default.List_Filter_AutoCommit_Delay;
						}

						if (Properties.Settings.Default.Details_Filter_AutoCommit_Delay > 0
							&& Properties.Settings.Default.Details_Filter_AutoCommit_Delay <= int.MaxValue)
						{
							appIDTextBoxTimer.Interval = Properties.Settings.Default.Details_Filter_AutoCommit_Delay;
						}

						overlaysLibraryCapsuleToolStripMenuItem.Tag = AssetType.LibraryCapsule;
						overlaysHeroGraphicToolStripMenuItem.Tag = AssetType.HeroGraphic;
						overlaysLogoToolStripMenuItem.Tag = AssetType.Logo;
						overlaysHeaderToolStripMenuItem.Tag = AssetType.Header;
						overlaysIconToolStripMenuItem.Tag = AssetType.Icon;

						libraryCapsuleToolStripMenuItem.Checked = Properties.Settings.Default.View_Show_LibraryCapsule;
						heroGraphicToolStripMenuItem.Checked = Properties.Settings.Default.View_Show_HeroGraphic;
						logoToolStripMenuItem.Checked = Properties.Settings.Default.View_Show_Logo;
						headerToolStripMenuItem.Checked = Properties.Settings.Default.View_Show_Header;
						iconToolStripMenuItem.Checked = Properties.Settings.Default.View_Show_Icon;

						viewListAlternateHeadersToolStripMenuItem.Checked = Properties.Settings.Default.View_UseAlternateColumnHeaders;

						viewListShowAllToolStripMenuItem.Checked = startupCacheLocation == (CacheLocation.UserData | CacheLocation.LibraryCache);
						viewListUserDataToolStripMenuItem.Checked = startupCacheLocation == CacheLocation.UserData;
						viewListLibraryCacheToolStripMenuItem.Checked = startupCacheLocation == CacheLocation.LibraryCache;
						viewListShowAllToolStripMenuItem.Enabled = startupCacheLocation != (CacheLocation.UserData | CacheLocation.LibraryCache);
						viewListUserDataToolStripMenuItem.Enabled = startupCacheLocation != CacheLocation.UserData;
						viewListLibraryCacheToolStripMenuItem.Enabled = startupCacheLocation != CacheLocation.LibraryCache;

						viewDetailsShowAllToolStripMenuItem.Checked = startupDetailsView == (CacheLocation.UserData | CacheLocation.LibraryCache);
						viewDetailsUserDataToolStripMenuItem.Checked = startupDetailsView == CacheLocation.UserData;
						viewDetailsLibraryCacheToolStripMenuItem.Checked = startupDetailsView == CacheLocation.LibraryCache;
						viewDetailsShowAllToolStripMenuItem.Enabled = startupDetailsView != (CacheLocation.UserData | CacheLocation.LibraryCache);
						viewDetailsUserDataToolStripMenuItem.Enabled = startupDetailsView != CacheLocation.UserData;
						viewDetailsLibraryCacheToolStripMenuItem.Enabled = startupDetailsView != CacheLocation.LibraryCache;

						listViewAllToolStripMenuItem.Checked = startupCacheLocation == (CacheLocation.UserData | CacheLocation.LibraryCache);
						listViewUserDataOnlyToolStripMenuItem.Checked = startupCacheLocation == CacheLocation.UserData;
						listViewLibraryCacheOnlyToolStripMenuItem.Checked = startupCacheLocation == CacheLocation.LibraryCache;
						listViewAllToolStripMenuItem.Enabled = startupCacheLocation != (CacheLocation.UserData | CacheLocation.LibraryCache);
						listViewUserDataOnlyToolStripMenuItem.Enabled = startupCacheLocation != CacheLocation.UserData;
						listViewLibraryCacheOnlyToolStripMenuItem.Enabled = startupCacheLocation != CacheLocation.LibraryCache;

						totalCountToolStripStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
						unknownAppSubCountToolStripStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
						appSubCountToolStripStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
						shortcutSubCountToolStripStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
						selectionCountToolStripStatusLabel.TextAlign = ContentAlignment.MiddleLeft;

						try
						{
							CloneMenu(sortByToolStripMenuItem, sortByToolStripDropDownButton);
							CloneMenu(sortOrderToolStripMenuItem, sortOrderToolStripDropDownButton);
							//sortByToolStripDropDownButton.DropDownItems.AddRange(sortByToolStripMenuItem.DropDownItems);
							//sortOrderToolStripDropDownButton.DropDownItems.AddRange(sortOrderToolStripMenuItem.DropDownItems);
						}
						catch (Exception ex)
						{
							Program.LogError(ex);
							Console.WriteLine("Press any key to continue.");
							Console.ReadKey();
							throw ex;
						}

						listStatusStrip.Visible = Properties.Settings.Default.View_StatusBar_Show;

						viewToolStripDropDownButton.Text = "View";
						sortByToolStripDropDownButton.Text = "Sort By";
						sortOrderToolStripDropDownButton.Text = "Sort Order";
					});


				Initialization("initializing list sorter", () =>
				{
					var sorter = new ListViewItemSorter(appIDListView) { SortColumn = Properties.Settings.Default.View_List_SortBy };

					appIDListView.Sorting = Properties.Settings.Default.View_List_SortOrder;
					appIDListView.SetSortIcon(ListViewItemSorter.GetColumnIndex(appIDListView, Properties.Settings.Default.View_List_SortBy, Properties.Settings.Default.View_List_CustomSortBy), appIDListView.Sorting);
					//appIDListView.ListViewItemSorter = sorter;
					appIDListView.Tag = sorter;

					if (Properties.Settings.Default.List_Filter_RestoreAtStart
						&& !string.IsNullOrEmpty(startupFilter))
					{
						Properties.Settings.Default.View_List_Filter = startupFilter;
						filterTextBox.Text = startupFilter;
						filterTextBoxTimer.Stop();
					}

					if (Properties.Settings.Default.Details_RestoreAppIDAtStart)
					{
						Properties.Settings.Default.AppID = startupAppID;
					}

					UpdateSortCriteriaDependentControls();
					UpdateSortOrderDependentControls();
				});

				Initialization("initializing webclient", () =>
				{
					webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36");
				});

				Initialization("initializing asset overlays", () =>
				{
					assetOverlays.FetchAvailableOverlayFiles();
				});

				Initialization("assigning asset overlays (userdata)", () =>
				{
					userDataUserPictureBoxGroup.AssetOverlays = assetOverlays;
					userDataUserPictureBoxGroup.TabPage.Text = "User Data";
				});

				Initialization("assigning asset overlays (librarycache)", () =>
				{
					libraryCacheUserPictureBoxGroup.AssetOverlays = assetOverlays;
					libraryCacheUserPictureBoxGroup.TabPage.Text = "Library Cache";
				});

				Initialization("creating asset overlays menu", () =>
				{
					PopulateOverlaysMenu();
				});

				Initialization("initializing statusbar", () =>
				{
					InitializeListStatusControls();
				});

				Initialization("showing form", () => { });
			}
		}

		private void InitializeListStatusControls()
		{
			totalCountToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_TotalCount;
			unknownAppSubCountToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_UnknownAppSubCount;
			appSubCountToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_AppSubCount;
			shortcutSubCountToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_ShortcutSubCount;
			selectionCountToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_SelectionCount;
			statusToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_Status;
			viewToolStripDropDownButton.Visible = Properties.Settings.Default.List_Status_Part_DataProvider;
			sortByToolStripDropDownButton.Visible = Properties.Settings.Default.List_Status_Part_SortBy;
			sortOrderToolStripDropDownButton.Visible = Properties.Settings.Default.List_Status_Part_SortOrder;
		}

		private void EnsureTogglesAreRegistered()
		{
			ToggleCollection.Instance.BeginUpdate();
			RegisterToggleIfNecessary("Confirmation/AddAsset", false);
			RegisterToggleIfNecessary("Confirmation/PasteAsset", false);
			RegisterToggleIfNecessary("Confirmation/RemoveAsset", true);
			RegisterToggleIfNecessary("Confirmation/ReplaceAsset", true);
			RegisterToggleIfNecessary("Confirmation/DownloadAsset", true);
			RegisterToggleIfNecessary("Confirmation/DownloadAssets", true); // currently unused
			RegisterToggleIfNecessary("Confirmation/DeleteProperty", true);
			RegisterToggleIfNecessary("Confirmation/InlineEditProperty", true);
			RegisterToggleIfNecessary("Confirmation/CopyAsset", true);  // currently unused
			RegisterToggleIfNecessary("Confirmation/CopyAssets", true); // currently unused
			RegisterToggleIfNecessary("Confirmation/DeleteString", true);
			RegisterToggleIfNecessary("Notification/CopyError", true);
			RegisterToggleIfNecessary("Notification/DownloadError", true);
			RegisterToggleIfNecessary("Notification/EmptyObjectSelection", true);
			RegisterToggleIfNecessary("Log/Initialization", true);
			RegisterToggleIfNecessary("Log/Information", true);
			RegisterToggleIfNecessary("Log/Verbose", false);
			RegisterToggleIfNecessary("Log/GetAssetPath", false);
			RegisterToggleIfNecessary("Log/AssetFileNotFound", false);
			RegisterToggleIfNecessary("Log/FileSystemWatch", false);
			ToggleCollection.Instance.EndUpdate();
		}

		private void RegisterToggleIfNecessary(string key, bool defaultValue)
		{
			if (!ToggleCollection.Instance.Items.ContainsKey(key))
			{
				ToggleCollection.Instance.Add(key, defaultValue);
			}
		}
		private static void EnsureSettingArraysAreInstantiated()
		{
			if (Properties.Settings.Default.List_Column_Order is null)
			{
				Properties.Settings.Default.List_Column_Order = new System.Collections.Specialized.StringCollection();
			}

			if (Properties.Settings.Default.List_Column_Types is null)
			{
				Properties.Settings.Default.List_Column_Types = new System.Collections.Specialized.StringCollection();
			}


			if (Properties.Settings.Default.CustomColors is null)
			{
				Properties.Settings.Default.CustomColors = new System.Collections.Specialized.StringCollection();
			}

			if (Properties.Settings.Default.Toggles is null)
			{
				Properties.Settings.Default.Toggles = new System.Collections.Specialized.StringCollection();
			}

			if (Properties.Settings.Default.Details_Asset_Preview_BackColor_CustomColors is null)
			{
				Properties.Settings.Default.Details_Asset_Preview_BackColor_CustomColors = new System.Collections.Specialized.StringCollection();
			}

			if (Properties.Settings.Default.ExternalApplications is null)
			{
				Properties.Settings.Default.ExternalApplications = new System.Collections.Specialized.StringCollection();
			}

			if (Properties.Settings.Default.VdfPropertyType_Object_KnownPaths is null)
			{
				Properties.Settings.Default.VdfPropertyType_Object_KnownPaths = new System.Collections.Specialized.StringCollection();
			}

			if (Properties.Settings.Default.VdfPropertyType_String_KnownPaths is null)
			{
				Properties.Settings.Default.VdfPropertyType_String_KnownPaths = new System.Collections.Specialized.StringCollection();
			}

			if (Properties.Settings.Default.VdfPropertyType_UInt_KnownPaths is null)
			{
				Properties.Settings.Default.VdfPropertyType_UInt_KnownPaths = new System.Collections.Specialized.StringCollection();
			}

			if (Properties.Settings.Default.VdfPropertyType_Boolean_KnownPaths is null)
			{
				Properties.Settings.Default.VdfPropertyType_Boolean_KnownPaths = new System.Collections.Specialized.StringCollection();
			}

			if (Properties.Settings.Default.VdfPropertyType_RelativeTime_KnownPaths is null)
			{
				Properties.Settings.Default.VdfPropertyType_RelativeTime_KnownPaths = new System.Collections.Specialized.StringCollection();
			}

			if (Properties.Settings.Default.VdfPropertyType_Enum_KnownPaths is null)
			{
				Properties.Settings.Default.VdfPropertyType_Enum_KnownPaths = new System.Collections.Specialized.StringCollection();
			}

			if (Properties.Settings.Default.List_Status_Part_Order is null)
			{
				Properties.Settings.Default.List_Status_Part_Order = new System.Collections.Specialized.StringCollection();
			}
		}

		// NOTE: non-recursive.
		private void CloneMenu(ToolStripMenuItem source, ToolStripDropDownButton destination)
		{
			var clones = new ToolStripItem[source.DropDownItems.Count];
			var i = 0;

			foreach (var item in source.DropDownItems)
			{
				var clone = (ToolStripItem)null;

				if (item is ToolStripMenuItem menuItem)
				{
					clone = new ToolStripMenuItem(menuItem.Text, menuItem.Image, (sender, e) => menuItem.PerformClick())
					{
						Name = $"status_{menuItem.Name}",
						Alignment = menuItem.Alignment,
						Enabled = menuItem.Enabled,
						Checked = menuItem.Checked,
						Visible = menuItem.Visible,
					};
				}
				else if (item is ToolStripSeparator separator)
				{
					clone = new ToolStripSeparator
					{
						Name = $"status_{separator.Name}",
						Enabled = separator.Enabled,
						Visible = separator.Visible,
					};
				}
				else
				{
					Program.LogError($"Item is of unknown type {item?.GetType().Name}");
				}

				clones[i++] = clone;
			}

			destination.DropDownItems.AddRange(clones);
		}

		#endregion

		#region Event Handlers

		#region MainForm

		private void MainForm_Load(object sender, EventArgs e)
		{
			var abort = false;

			Initialization("auto finding steam path", () =>
			{
				if (string.IsNullOrEmpty(Properties.Settings.Default.SteamAppPath)
					|| !Directory.Exists(Properties.Settings.Default.SteamAppPath))
				{
					if (!AutoFindSteamPath())
					{
						MessageBox.Show(this, "Can't find the path to your Steam folder, please set it yourself.", "Steam Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
						Application.Exit();
						abort = true;
					}
				}
			});

			if (abort)
			{
				return;
			}

			Initialization("auto finding steam user", () =>
			{
				if (Properties.Settings.Default.SteamUserID == 0)
				{
					if (!AutoFindUser())
					{
						MessageBox.Show(this, "Can't find any steam user, please set it yourself.", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
						Application.Exit();
						abort = true;
					}
				}
			});

			if (abort)
			{
				return;
			}

			Initialization("auto setting non-steam app icon cache folder", () =>
			{
				if (string.IsNullOrEmpty(Properties.Settings.Default.NonSteamAppIconCachePath))
				{
					AutoInitNonSteamAppIconCacheDirectory();
				}
			});

			Initialization("auto setting backup directory", () =>
			{
				if (string.IsNullOrEmpty(Properties.Settings.Default.BackupPath))
				{
					AutoInitBackupDirectory();
				}
			});

			Initialization("initializing overlays path", () =>
			{
				if (string.IsNullOrEmpty(Properties.Settings.Default.Overlays_Path)
					|| !Directory.Exists(Properties.Settings.Default.Overlays_Path))
				{
					Properties.Settings.Default.Overlays_Path = AssetOverlayUtils.GetDefaultOverlaysRootFolder();
				}
			});

			Initialization("initializing localization path", () =>
			{
				if (string.IsNullOrEmpty(Properties.Settings.Default.Localization_Path)
					|| !Directory.Exists(Properties.Settings.Default.Localization_Path))
				{
					// TODO
					//Properties.Settings.Default.Localization_Path = LocalizationSystem.Instance.GetDefaultRootFolder();
				}
			});

			Initialization("loading appinfo database", () =>
			{
				if (Properties.Settings.Default.Database_AppInfo_LoadAtStart)
				{
					LoadAppInfo();
				}
			});

			Initialization("loading shortcuts database", () =>
			{
				if (Properties.Settings.Default.Database_Shortcuts_LoadAtStart)
				{
					LoadShortcuts();
				}
			});

			Initialization("building vdf filter menu", () =>
			{
				BuildVdfColumnsMenu();
			});

			Initialization("building vdf sort menu", () =>
			{
				BuildVdfColumnsSortingMenu();
			});

			Initialization("assigning appinfos and shortcuts (userdata)", () =>
			{
				userDataUserPictureBoxGroup.AppInfos = appInfos;
				userDataUserPictureBoxGroup.Shortcuts = shortcuts;
			});

			Initialization("assigning appinfos and shortcuts (librarycache)", () =>
			{
				libraryCacheUserPictureBoxGroup.AppInfos = appInfos;
				libraryCacheUserPictureBoxGroup.Shortcuts = shortcuts;
			});

			Initialization("applying cache location", () =>
			{
				if (Enum.TryParse<CacheLocation>(Properties.Settings.Default.StartupCacheLocation, out var cacheLocation))
				{
					Properties.Settings.Default.List_CacheLocation = cacheLocation;
				}
				else
				{
					Properties.Settings.Default.List_CacheLocation = startupCacheLocation;
				}

				Properties.Settings.Default.Details_CacheLocation = startupDetailsView;
			});

			Initialization("updating control states", () =>
			{
				UpdateListViewItemSelectionDependentItems();
				UpdateSteamPathDependentControls();
				UpdateSteamUserIDDependentControls();
				UpdateAppInfoDependentControls();
				UpdateShortcutsDependentControls();
			});

			Initialization("restoring last user selection", () =>
			{
				if (Properties.Settings.Default.Details_RestoreAppIDAtStart)
				{
					SelectAppIDs(startupAppID);
					FocusOnSelectedItem();
				}
			});

			Initialization("initializing asset file watcher system", () =>
			{
				if (Properties.Settings.Default.Details_WatchFileChanges
					|| Properties.Settings.Default.List_WatchFileChanges)
				{
					AssetFileWatcherSystem.Initialize();
				}

				if (Properties.Settings.Default.Details_WatchFileChanges)
				{
					userDataUserPictureBoxGroup.AssetFileWatcherSystem = AssetFileWatcherSystem;
					libraryCacheUserPictureBoxGroup.AssetFileWatcherSystem = AssetFileWatcherSystem;
				}

				if (Properties.Settings.Default.Details_WatchFileChanges
					|| Properties.Settings.Default.List_WatchFileChanges)
				{
					AssetFileWatcherSystem.Enable();
				}
			});

			Initialization("ready", () =>
			{
				isLoaded = true;
			});
		}

		private void Initialization(string description, Action action)
		{
			var stopWatch = (Stopwatch)null;
			var isLogOn = ToggleCollection.Instance.IsOn("Log/Initialization");
			var isVerboseOn = ToggleCollection.Instance.IsOn("Log/Verbose");

			if (isLogOn)
			{
				if (isVerboseOn)
				{
					stopWatch = Stopwatch.StartNew();
				}

				Console.WriteLine($"Initialization Phase {++initializationStage} - {description}");
			}

			action?.Invoke();

			if (isLogOn
				&& isVerboseOn)
			{
				stopWatch.Stop();
				Console.WriteLine($"\t{stopWatch.Elapsed}");
			}
		}

		private void BuildVdfColumnsMenu()
		{
			var allPaths = new List<string>();

			foreach (var path in Properties.Settings.Default.VdfPropertyType_Object_KnownPaths)
			{
				allPaths.Add(path);
			}
			foreach (var path in Properties.Settings.Default.VdfPropertyType_String_KnownPaths)
			{
				allPaths.Add(path);
			}
			foreach (var path in Properties.Settings.Default.VdfPropertyType_UInt_KnownPaths)
			{
				allPaths.Add(path);
			}
			foreach (var path in Properties.Settings.Default.VdfPropertyType_Boolean_KnownPaths)
			{
				allPaths.Add(path);
			}
			foreach (var path in Properties.Settings.Default.VdfPropertyType_Enum_KnownPaths)
			{
				allPaths.Add(path);
			}
			foreach (var path in Properties.Settings.Default.VdfPropertyType_RelativeTime_KnownPaths)
			{
				allPaths.Add(path);
			}

			allPaths.Sort();

			columnsToolStripMenuItem.DropDownItems.Clear();

			VdfUtils.PopulateMenuWithVdfFilter(allPaths, columnsToolStripMenuItem, onClick, check);

			void onClick(string fullPath)
			{
				// TODO: pass the part of this item so that we can check whether it's a directory or an end node. (because we should ignore directories)

				if (Properties.Settings.Default.List_Column_Types.Contains(fullPath))
				{
					Properties.Settings.Default.List_Column_Types.Remove(fullPath);
				}
				else
				{
					Properties.Settings.Default.List_Column_Types.Add(fullPath);
				}


				PopulateListView();
				FilterAndSortListView();
				BuildVdfColumnsSortingMenu();
			}

			bool check(string fullPath)
				=> Properties.Settings.Default.List_Column_Types.Contains(fullPath);
		}

		private void BuildVdfColumnsSortingMenu()
		{
			var allPaths = new List<string>();

			foreach (var path in Properties.Settings.Default.List_Column_Types)
			{
				var paths = path.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries);

				foreach (var subPath in paths)
				{
					allPaths.Add(subPath);
				}
			}

			advancedToolStripMenuItem.DropDownItems.Clear();
			listSortByAdvancedToolStripMenuItem.DropDownItems.Clear();

			// TODO: build a simple flat menu instead?

			VdfUtils.PopulateMenuWithVdfFilter(allPaths, advancedToolStripMenuItem, onClick, check);
			VdfUtils.PopulateMenuWithVdfFilter(allPaths, listSortByAdvancedToolStripMenuItem, onClick, check);

			void onClick(string fullPath)
			{
				SortListView(fullPath, appIDListView.Sorting);
			}

			bool check(string fullPath)
				=> appIDListView.Tag is ListViewItemSorter listViewItemSorter
					&& !string.IsNullOrEmpty(listViewItemSorter.SortFilter)
					&& listViewItemSorter.SortFilter.Equals(fullPath);

			// TODO
			//bool enable(string fullPath)
			//	=> Properties.Settings.Default.List_Column_Types.Contains(fullPath);
		}

		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (DataObjectHelpers.TryGetAppId(e.Data, out var appID))
			{
				Properties.Settings.Default.AppID = appID;
				e.Effect = e.AllowedEffect;
				return;
			}

			e.Effect = DragDropEffects.None;
		}

		private void MainForm_DragOver(object sender, DragEventArgs e)
		{
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
		}

		#endregion

		#region Settings

		private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(Properties.Settings.Default.UI_Language):
					// TODO
					//Localize();
					break;
				case nameof(Properties.Settings.Default.UI_Culture):
					try
					{
						var cultureInfo = CultureInfo.GetCultureInfo(Properties.Settings.Default.UI_Culture);

						CultureInfo.CurrentCulture = cultureInfo;
						CultureInfo.CurrentUICulture = cultureInfo;
					}
					catch (Exception ex)
					{
						Program.LogError(ex);
					}
					break;
				case nameof(Properties.Settings.Default.SteamAppPath):
					try
					{
						CollectAppIDsAndPopulateListView();
						UpdateSteamPathDependentControls();
						UpdateSteamUserIDDependentControls();
						UpdateAppInfoDependentControls();
						UpdateShortcutsDependentControls();
					}
					catch (Exception ex)
					{
						Program.LogError(ex);
					}
					break;
				case nameof(Properties.Settings.Default.SteamUserID):
					try
					{
						CollectAppIDsAndPopulateListView();
						UpdateSteamUserIDDependentControls();
						UpdateAppInfoDependentControls();
						UpdateShortcutsDependentControls();
					}
					catch (Exception ex)
					{
						Program.LogError(ex);
					}
					break;

				case nameof(Properties.Settings.Default.AppID):
					appIDTextBox.Value = Properties.Settings.Default.AppID;
					splitContainer1.Panel2.Visible = Properties.Settings.Default.AppID != 0;
					Text = $"{Properties.Settings.Default.AppID} - {"Steam Grid Manager"}";
					break;

				case nameof(Properties.Settings.Default.View_Show_LibraryCapsule):
					libraryCapsuleToolStripMenuItem.Checked = Properties.Settings.Default.View_Show_LibraryCapsule;
					CollectAppIDsAndPopulateListView();
					UpdateSortCriteriaDependentControls();
					break;

				case nameof(Properties.Settings.Default.View_Show_HeroGraphic):
					heroGraphicToolStripMenuItem.Checked = Properties.Settings.Default.View_Show_HeroGraphic;
					CollectAppIDsAndPopulateListView();
					UpdateSortCriteriaDependentControls();
					break;

				case nameof(Properties.Settings.Default.View_Show_Logo):
					logoToolStripMenuItem.Checked = Properties.Settings.Default.View_Show_Logo;
					CollectAppIDsAndPopulateListView();
					UpdateSortCriteriaDependentControls();
					break;

				case nameof(Properties.Settings.Default.View_Show_Header):
					headerToolStripMenuItem.Checked = Properties.Settings.Default.View_Show_Header;
					CollectAppIDsAndPopulateListView();
					UpdateSortCriteriaDependentControls();
					break;

				case nameof(Properties.Settings.Default.View_Show_Icon):
					iconToolStripMenuItem.Checked = Properties.Settings.Default.View_Show_Icon;
					CollectAppIDsAndPopulateListView();
					UpdateSortCriteriaDependentControls();
					break;

				case nameof(Properties.Settings.Default.View_UseAlternateColumnHeaders):
					viewListAlternateHeadersToolStripMenuItem.Checked = Properties.Settings.Default.View_UseAlternateColumnHeaders;
					InitializeColumnHeaders();
					AutoResizeColumns();
					break;

				case nameof(Properties.Settings.Default.List_Assets_Exist_String):
					OnListLibraryAssetsStyleChanged();
					break;

				case nameof(Properties.Settings.Default.List_Assets_Exist_Color):
					OnListLibraryAssetsStyleChanged();
					break;

				case nameof(Properties.Settings.Default.List_Assets_Missing_String):
					OnListLibraryAssetsStyleChanged();
					break;

				case nameof(Properties.Settings.Default.List_Assets_Missing_Color):
					OnListLibraryAssetsStyleChanged();
					break;

				case nameof(Properties.Settings.Default.Details_CacheLocation):
					/*
					viewDetailsLibraryCacheToolStripMenuItem.Checked = Properties.Settings.Default.Details_CacheLocation.HasFlag(CacheLocation.LibraryCache);
					viewDetailsUserDataToolStripMenuItem.Checked = Properties.Settings.Default.Details_CacheLocation.HasFlag(CacheLocation.UserData);
					viewDetailsLibraryCacheToolStripMenuItem.Enabled = Properties.Settings.Default.Details_CacheLocation.HasFlag(CacheLocation.UserData) || Properties.Settings.Default.List_CacheLocation == CacheLocation.None;
					viewDetailsUserDataToolStripMenuItem.Enabled = Properties.Settings.Default.Details_CacheLocation.HasFlag(CacheLocation.LibraryCache) || Properties.Settings.Default.List_CacheLocation == CacheLocation.None;
					*/
					viewDetailsShowAllToolStripMenuItem.Checked = Properties.Settings.Default.Details_CacheLocation == (CacheLocation.UserData | CacheLocation.LibraryCache);
					viewDetailsUserDataToolStripMenuItem.Checked = Properties.Settings.Default.Details_CacheLocation == CacheLocation.UserData;
					viewDetailsLibraryCacheToolStripMenuItem.Checked = Properties.Settings.Default.Details_CacheLocation == CacheLocation.LibraryCache;
					viewDetailsShowAllToolStripMenuItem.Enabled = Properties.Settings.Default.Details_CacheLocation != (CacheLocation.UserData | CacheLocation.LibraryCache);
					viewDetailsUserDataToolStripMenuItem.Enabled = Properties.Settings.Default.Details_CacheLocation != CacheLocation.UserData;
					viewDetailsLibraryCacheToolStripMenuItem.Enabled = Properties.Settings.Default.Details_CacheLocation != CacheLocation.LibraryCache;
					OnDetailsViewChanged();
					break;

				case nameof(Properties.Settings.Default.List_CacheLocation):
					/*
					viewListLibraryCacheToolStripMenuItem.Checked = Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);
					viewListUserDataToolStripMenuItem.Checked = Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData);
					viewListLibraryCacheToolStripMenuItem.Enabled = Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData) || Properties.Settings.Default.List_CacheLocation == CacheLocation.None;
					viewListUserDataToolStripMenuItem.Enabled = Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache) || Properties.Settings.Default.List_CacheLocation == CacheLocation.None;
					*/
					viewListShowAllToolStripMenuItem.Checked = Properties.Settings.Default.List_CacheLocation == (CacheLocation.UserData | CacheLocation.LibraryCache);
					viewListUserDataToolStripMenuItem.Checked = Properties.Settings.Default.List_CacheLocation == CacheLocation.UserData;
					viewListLibraryCacheToolStripMenuItem.Checked = Properties.Settings.Default.List_CacheLocation == CacheLocation.LibraryCache;
					viewListShowAllToolStripMenuItem.Enabled = Properties.Settings.Default.List_CacheLocation != (CacheLocation.UserData | CacheLocation.LibraryCache);
					viewListUserDataToolStripMenuItem.Enabled = Properties.Settings.Default.List_CacheLocation != CacheLocation.UserData;
					viewListLibraryCacheToolStripMenuItem.Enabled = Properties.Settings.Default.List_CacheLocation != CacheLocation.LibraryCache;

					listViewAllToolStripMenuItem.Checked = viewListShowAllToolStripMenuItem.Checked;
					listViewUserDataOnlyToolStripMenuItem.Checked = viewListUserDataToolStripMenuItem.Checked;
					listViewLibraryCacheOnlyToolStripMenuItem.Checked = viewListLibraryCacheToolStripMenuItem.Checked;
					listViewAllToolStripMenuItem.Enabled = viewListShowAllToolStripMenuItem.Enabled;
					listViewUserDataOnlyToolStripMenuItem.Enabled = viewListUserDataToolStripMenuItem.Enabled;
					listViewLibraryCacheOnlyToolStripMenuItem.Enabled = viewListLibraryCacheToolStripMenuItem.Enabled;
					viewToolStripDropDownButton.Text = $"{Properties.Settings.Default.List_CacheLocation}";
					//viewToolStripDropDownButton.Text = startupCacheLocation.ToString();

					appIDTextBox.ReadOnly = Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);

					CollectAppIDsAndPopulateListView();
					if (isLoaded)
					{
						SelectFirstAppIDFromListView();
					}
					UpdateSortCriteriaDependentControls();
					break;

				case nameof(Properties.Settings.Default.View_Overlay_LibraryCapsule):
					CheckOverlayMenuItemGroup(AssetType.LibraryCapsule, Properties.Settings.Default.View_Overlay_LibraryCapsule);
					break;
				case nameof(Properties.Settings.Default.View_Overlay_HeroGraphic):
					CheckOverlayMenuItemGroup(AssetType.HeroGraphic, Properties.Settings.Default.View_Overlay_HeroGraphic);
					break;
				case nameof(Properties.Settings.Default.View_Overlay_Logo):
					CheckOverlayMenuItemGroup(AssetType.Logo, Properties.Settings.Default.View_Overlay_Logo);
					break;
				case nameof(Properties.Settings.Default.View_Overlay_Header):
					CheckOverlayMenuItemGroup(AssetType.Header, Properties.Settings.Default.View_Overlay_Header);
					break;
				case nameof(Properties.Settings.Default.View_Overlay_Icon):
					CheckOverlayMenuItemGroup(AssetType.Icon, Properties.Settings.Default.View_Overlay_Icon);
					break;

				case nameof(Properties.Settings.Default.View_List_Filter):
					//FilterAndSortListView();
					break;

				case nameof(Properties.Settings.Default.List_Column_Types):
					BuildVdfColumnsSortingMenu();

					if (!Properties.Settings.Default.List_Column_Types.Contains(Properties.Settings.Default.View_List_CustomSortBy))
					{
						SortListView(ColumnHeaderType.Title, Properties.Settings.Default.View_List_SortOrder);
						/*
						Properties.Settings.Default.View_List_CustomSortBy = "";
						Properties.Settings.Default.View_List_SortBy = ColumnHeaderType.Title;
						UpdateSortCriteriaDependentControls();
						FilterAndSortListView();
						*/
					}
					break;

				case nameof(Properties.Settings.Default.List_Status_Part_TotalCount):
					totalCountToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_TotalCount;
					UpdateTotalCount();
					break;
				case nameof(Properties.Settings.Default.List_Status_Part_UnknownAppSubCount):
					unknownAppSubCountToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_UnknownAppSubCount;
					UpdateUnknownAppSubCount();
					break;
				case nameof(Properties.Settings.Default.List_Status_Part_AppSubCount):
					appSubCountToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_AppSubCount;
					UpdateAppSubCount();
					break;
				case nameof(Properties.Settings.Default.List_Status_Part_ShortcutSubCount):
					shortcutSubCountToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_ShortcutSubCount;
					UpdateShortcutSubCount();
					break;
				case nameof(Properties.Settings.Default.List_Status_Part_SelectionCount):
					selectionCountToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_SelectionCount;
					UpdateTotalCount();
					break;
				case nameof(Properties.Settings.Default.List_Status_Part_Status):
					statusToolStripStatusLabel.Visible = Properties.Settings.Default.List_Status_Part_Status;
					break;
				case nameof(Properties.Settings.Default.List_Status_Part_DataProvider):
					viewToolStripDropDownButton.Visible = Properties.Settings.Default.List_Status_Part_DataProvider;
					break;
				case nameof(Properties.Settings.Default.List_Status_Part_SortBy):
					sortByToolStripDropDownButton.Visible = Properties.Settings.Default.List_Status_Part_SortBy;
					break;
				case nameof(Properties.Settings.Default.List_Status_Part_SortOrder):
					sortOrderToolStripDropDownButton.Visible = Properties.Settings.Default.List_Status_Part_SortOrder;
					break;

				case nameof(Properties.Settings.Default.List_Filter_AutoCommit_Delay):
					try
					{
						filterTextBoxTimer.Interval = Properties.Settings.Default.List_Filter_AutoCommit_Delay;
					}
					catch (Exception ex)
					{
						Program.LogError(ex);
					}
					break;
				case nameof(Properties.Settings.Default.Details_Filter_AutoCommit_Delay):
					try
					{
						appIDTextBoxTimer.Interval = Properties.Settings.Default.Details_Filter_AutoCommit_Delay;
					}
					catch (Exception ex)
					{
						Program.LogError(ex);
					}
					break;
			}
		}

		private void UpdateSteamPathDependentControls()
		{
			var steamGamesVdfPath = Path.Combine(Properties.Settings.Default.SteamAppPath, "steam", "games");
			var steamGamesVdfPathExists = Directory.Exists(steamGamesVdfPath);

			updateIconCacheToolStripMenuItem.Enabled = steamGamesVdfPathExists;
		}

		private void UpdateSteamUserIDDependentControls()
		{
			var appInfosVdfPath = Path.Combine(Properties.Settings.Default.SteamAppPath, "appcache", "appinfo.vdf");
			var shortcutsVdfPath = Path.Combine(Properties.Settings.Default.SteamAppPath, "userdata", Properties.Settings.Default.SteamUserID.ToString(CultureInfo.InvariantCulture), "config", "shortcuts.vdf");
			var appInfoExists = File.Exists(appInfosVdfPath);
			var shortcutsExists = File.Exists(shortcutsVdfPath);

			toolsDatabaseLoadAppInfoToolStripMenuItem.Enabled = appInfoExists;

			toolsDatabaseLoadShortcutsToolStripMenuItem.Enabled = shortcutsExists;
		}

		private void UpdateAppInfoDependentControls()
		{
			var hasAppInfo = appInfos != null && appInfos.Items.Count > 0;

			saveAppInfoToolStripMenuItem.Enabled = hasAppInfo && Properties.Settings.Default.Database_AppInfo_AllowSaving;
			exportAppInfoAsToolStripMenuItem.Enabled = hasAppInfo;
		}

		private void UpdateShortcutsDependentControls()
		{
			var hasShortcuts = shortcuts != null && shortcuts.Items.Count > 0;

			toolsDatabaseSaveShortcutsToolStripMenuItem.Enabled = hasShortcuts;
			exportShortcutsAsToolStripMenuItem.Enabled = hasShortcuts;

			updateFromExecutablesAndCustomIconsToolStripMenuItem.Enabled = hasShortcuts;
			updateFromExecutablesOnlyToolStripMenuItem.Enabled = hasShortcuts;
			updateFromCustomIconsOnlyToolStripMenuItem.Enabled = hasShortcuts;
		}

		#endregion

		#region Application

		private void Application_ApplicationExit(object sender, EventArgs e)
		{
			try
			{
				Properties.Settings.Default.Save();
			}
			catch (Exception ex)
			{
				Program.LogError(ex);
			}
		}

		#endregion

		#region Filter Text Box + Timer

		private void filterTextBox_TextChanged(object sender, EventArgs e)
		{
			if (!Properties.Settings.Default.List_Filter_AutoCommit_Enable)
			{
				return;
			}
			else if (Properties.Settings.Default.List_Filter_AutoCommit_Delay == 0)
			{
				filterTextBoxTimer_Tick(filterTextBoxTimer, EventArgs.Empty);
			}
			else if (Properties.Settings.Default.List_Filter_AutoCommit_Delay > 0)
			{
				filterTextBoxTimer.Stop();
				filterTextBoxTimer.Start();
			}
		}

		private void filterTextBoxTimer_Tick(object sender, EventArgs e)
		{
			filterTextBoxTimer.Stop();

			Properties.Settings.Default.View_List_Filter = filterTextBox.Text;

			FilterAndSortListView();
		}

		private void FilterAndSortListView()
		{
			var filter = Properties.Settings.Default.View_List_Filter;

			if (cachedListViewItems is null
				|| cachedListViewItems.Count == 0)
			{
				filteredListViewItems.Clear();
				sortedListViewItems.Clear();
				//appIDListView.VirtualListSize = 0;

				return;
			}

			var selectedAppIDs = GetSelectedAppIDs();

			filteredListViewItems.Clear();

			if (string.IsNullOrEmpty(filter))
			{
				filteredListViewItems.AddRange(cachedListViewItems);
			}
			else
			{
				var prefix = Properties.Settings.Default.List_VdfFilter_Prefix;

				if (string.IsNullOrEmpty(prefix))
				{
					FilterListViewByVdfPath(filter);
					FilterListViewByColumnsContent(filter);
				}
				else if (filter.StartsWith(prefix))
				{
					FilterListViewByVdfPath(filter.Substring(prefix.Length));
				}
				else
				{
					FilterListViewByColumnsContent(filter);
				}
			}

			sortedListViewItems.Clear();
			sortedListViewItems.AddRange(filteredListViewItems);

			if (appIDListView.Sorting != SortOrder.None)
			{
				if (appIDListView.Tag is ListViewItemSorter listViewItemSorter)
				{
					try
					{
						sortedListViewItems.Sort(listViewItemSorter);
					}
					catch (Exception ex)
					{
						Program.LogError(ex);
					}
				}
			}

			appIDListView.BeginUpdate();
			appIDListView.VirtualListSize = sortedListViewItems.Count;
			AutoResizeColumns();
			appIDListView.EndUpdate();

			appIDListView.SelectedIndices.Clear();
			SelectAppIDs(selectedAppIDs);
			OnSelectionChanged();

			appIDListView_VirtualListSizeChanged(appIDListView, EventArgs.Empty);
		}

		private ulong[] GetSelectedAppIDs()
		{
			var result = new List<ulong>(appIDListView.SelectedIndices.Count);

			for (var i = 0; i < appIDListView.SelectedIndices.Count; ++i)
			{
				var selectedIndex = appIDListView.SelectedIndices[i];

				if (selectedIndex < 0
					|| selectedIndex > sortedListViewItems.Count - 1)
				{
					continue;
				}

				var selectedItem = sortedListViewItems[selectedIndex];

				if (GetListViewItemAppID(selectedItem) is var appID)
				{
					result.Add(appID.Value);
				}
				/*
				if (selectedItem.Tag is AppInfo appInfo)
				{
					result.Add(appInfo.Data.AppID);
				}
				else if (selectedItem.Tag is Steam.Vdf.Shortcut shortcut)
				{
					result.Add(shortcut.Node.GetUInt("appid") ?? 0);
				}
				else if (selectedItem.Tag is ulong appID)
				{
					result.Add(appID);
				}
				*/
			}

			return result.ToArray();
		}

		private void SelectAppIDs(params ulong[] appIDs)
		{
			var selectedIndices = new List<int>();

			foreach (var selectedAppID in appIDs)
			{
				var index = sortedListViewItems.FindIndex(x => GetListViewItemAppID(x) == selectedAppID);

				if (index == -1)
				{
					continue;
				}

				selectedIndices.Add(index);
			}

			if (selectedIndices.Count == 0)
			{
				return;
			}

			appIDListView.SelectRange(selectedIndices);
		}

		private void FilterListViewByColumnsContent(string filter)
		{
			foreach (var item in cachedListViewItems)
			{
				var found = false;

				foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
				{
					// FIXME? auto-detect culture to use? because current culture might be wrong. (e.g., items could be encoded in utf-8, shift_jis, etc.)

					if (subItem.Text.IndexOf(filter, StringComparison.CurrentCultureIgnoreCase) != -1)
					{
						found = true;
						break;
					}
				}

				if (found)
				{
					filteredListViewItems.Add(item);
				}
			}
		}

		private void FilterListViewByVdfPath(string filter)
		{
			if (appInfos.Items.Count == 0
				&& shortcuts.Items.Count == 0)
			{
				return;
			}

			var value = (string)null;
			var valueRegex = BuildFilterRegex(ref filter, ref value);

			foreach (var item in cachedListViewItems)
			{
				var found = false;
				var node = (VdfObject)null;

				if (item.Tag is AppInfo appInfo)
				{
					// TODO? allow searching through its metadata as well?

					node = appInfo.Data.Node;
				}
				else if (item.Tag is Steam.Vdf.Shortcut shortcut)
				{
					node = shortcut.Node;
				}

				if (node is null)
				{
					continue;
				}

				var result = node.FindValueByPath(filter);

				if (result != null)
				{
					var source = (string)null;

					if (result is VdfObject vdfObjectValue)
					{
						// TODO: compare ACF?

						source = "";
					}
					else if (result is string stringValue)
					{
						source = stringValue;
					}
					else if (result is uint uintValue)
					{
						source = uintValue.ToString(CultureInfo.InvariantCulture);
					}

					if (valueRegex != null)
					{
						found = valueRegex.IsMatch(source);
					}
					else if (value != null)
					{
						found = value.Contains(source);
						/*
						if (result is VdfObject vdfObjectValue)
						{
							// TODO: compare ACF.
						}
						else if (result is string stringValue)
						{
							// TODO: using the current culture might be wrong.

							found = value.Equals(stringValue, Properties.Settings.Default.List_VdfFilter_MatchCase ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);
						}
						else if (result is uint uintValue)
						{
							if (uint.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var valueAsUInt))
							{
								found = valueAsUInt.Equals(uintValue);
							}
						}*/
					}
					else
					{
						// no value specified, return everything matching the property name.
						found = true;
					}
				}

				if (found)
				{
					filteredListViewItems.Add(item);
				}
			}
		}

		private static Regex BuildFilterRegex(ref string filter, ref string value)
		{
			var valueRegex = (Regex)null;
			var useRegex = Properties.Settings.Default.List_VdfFilter_UseRegularExpressions;
			var matchCase = Properties.Settings.Default.List_VdfFilter_MatchCase;
			var matchWholeWord = Properties.Settings.Default.List_VdfFilter_MatchWholeWord;
			var simpleSearch = !useRegex && matchCase && !matchWholeWord;

			if (filter.Contains("="))
			{
				var tokens = filter.Split(new char[] { '=' }, 2);

				filter = tokens[0];
				value = tokens[1];

				if (!simpleSearch)
				{
					var regexOptions = RegexOptions.None;

					if (!useRegex)
					{
						value = Regex.Escape(value);
					}

					if (!matchCase)
					{
						regexOptions |= RegexOptions.IgnoreCase;
					}

					if (matchWholeWord)
					{
						value = $"\\b{value}\\b";
					}

					try
					{
						valueRegex = new Regex(value, regexOptions);
					}
					// user is probably still typing an incomplete pattern.
					catch { valueRegex = new Regex(""); }
				}
			}

			return valueRegex;
		}

		#endregion

		#region List View

		private void appIDListView_ColumnReordered(object sender, ColumnReorderedEventArgs e)
		{
			var columnType = (ColumnHeaderType)e.Header.Tag;
			var newList = new List<ColumnHeaderType>();
			var orderedColumns = new List<ColumnHeader>();

			foreach (ColumnHeader column in appIDListView.Columns)
			{
				orderedColumns.Add(column);
			}

			foreach (ColumnHeader column in orderedColumns.OrderBy(x => x.DisplayIndex))
			{
				newList.Add((ColumnHeaderType)column.Tag);
			}

			newList.RemoveAt(e.OldDisplayIndex);
			newList.Insert(e.NewDisplayIndex, columnType);

			Properties.Settings.Default.List_Column_Order.Clear();
			Properties.Settings.Default.List_Column_Order.AddRange(newList.Select(x => $"{x}").ToArray());
		}

		private void appIDListView_VirtualListSizeChanged(object sender, EventArgs e)
		{
			UpdateTotalCount();
			UpdateUnknownAppSubCount();
			UpdateAppSubCount();
			UpdateShortcutSubCount();
			UpdateEditMenuItems();
		}

		private void UpdateEditMenuItems()
		{
			var isListEmpty = sortedListViewItems.Count == 0;

			findToolStripMenuItem.Enabled = !isListEmpty;
			goToToolStripMenuItem.Enabled = !isListEmpty;
		}

		private void UpdateTotalCount()
		{
			if (Properties.Settings.Default.List_Status_Part_TotalCount)
			{
				var count = sortedListViewItems.Count;

				totalCountToolStripStatusLabel.Text = count == 1 ? $"{count} item" : $"{count} items";
			}
		}

		private void UpdateUnknownAppSubCount()
		{
			if (Properties.Settings.Default.List_Status_Part_UnknownAppSubCount)
			{
				var count = sortedListViewItems.Where(lvi => lvi.Tag is null).Count();

				unknownAppSubCountToolStripStatusLabel.Text = count == 1 ? $"{count} unknown app" : $"{count} unknown apps";
			}
		}

		private void UpdateAppSubCount()
		{
			if (Properties.Settings.Default.List_Status_Part_AppSubCount)
			{
				var count = sortedListViewItems.Where(lvi => lvi.Tag is AppInfo).Count();

				appSubCountToolStripStatusLabel.Text = count == 1 ? $"{count} app" : $"{count} apps";
			}
		}

		private void UpdateShortcutSubCount()
		{
			if (Properties.Settings.Default.List_Status_Part_ShortcutSubCount)
			{
				var count = sortedListViewItems.Where(lvi => lvi.Tag is Steam.Vdf.Shortcut).Count();

				shortcutSubCountToolStripStatusLabel.Text = count == 1 ? $"{count} shortcut" : $"{count} shortcuts";
			}
		}

		private void appIDListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			e.Item = sortedListViewItems[e.ItemIndex];
		}

		private void appIDListView_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs e)
		{
			// TODO: will properly implement virtual items later...
		}

		private void appIDListView_VirtualItemsSelectionRangeChanged(object sender, ListViewVirtualItemsSelectionRangeChangedEventArgs e)
		{
			// NOTE: this event is completely unreliable.
		}

		private void appIDListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			// NOTE: this event is unreliable with virtual lists.

			UpdateListViewItemSelectionDependentItems();
			OnSelectionChanged();

			var count = appIDListView.SelectedIndices.Count;
			var hasSingleSelection = count == 1;

			if (!hasSingleSelection)
			{
				return;
			}

			var appID = GetListViewItemAppID(sortedListViewItems[appIDListView.SelectedIndices[0]]);

			if (appID != null
				&& Properties.Settings.Default.AppID != appID)
			{
				Properties.Settings.Default.AppID = appID.Value;
			}

			focusedAppID = appID;
		}

		private void OnSelectionChanged()
		{
			UpdateSelectionCount();

			appIDPropertiesToolStripMenuItem.Enabled = CanShowProperties();
		}

		private void UpdateSelectionCount()
		{
			if (Properties.Settings.Default.List_Status_Part_SelectionCount)
			{
				var count = appIDListView.SelectedIndices.Count;

				selectionCountToolStripStatusLabel.Text = count == 1 ? $"{count} selected item" : $"{count} selected items";
			}
		}

		private void appIDListView_KeyUp(object sender, KeyEventArgs e)
		{
			// workaround for the completely broken virtual mode selection events.
			switch (e.KeyCode)
			{
				case Keys.Left:
				case Keys.Up:
				case Keys.Right:
				case Keys.Down:
				case Keys.Home:
				case Keys.End:
				case Keys.PageDown:
				case Keys.PageUp:
				case Keys.Space:
					OnSelectionChanged();
					break;
			}
		}

		private void appIDListView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			// workaround for the completely broken virtual mode selection events.
			switch (e.KeyCode)
			{
				case Keys.Left:
				case Keys.Up:
				case Keys.Right:
				case Keys.Down:
				case Keys.Home:
				case Keys.End:
				case Keys.PageDown:
				case Keys.PageUp:
				case Keys.Space:
					OnSelectionChanged();
					break;
			}
		}

		private void appIDListView_KeyDown(object sender, KeyEventArgs e)
		{
		}

		private void appIDListView_ItemActivate(object sender, EventArgs e)
		{
			switch (Properties.Settings.Default.List_AppAction)
			{
				case AppAction.Properties:
					appIDPropertiesToolStripMenuItem.PerformClick();
					break;
			}
		}

		private void appIDListView_MouseClick(object sender, MouseEventArgs e)
		{
			// workaround for the completely broken virtual mode selection events.
			OnSelectionChanged();
		}

		private void appIDListView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (e.Column == -1)
			{
				return;
			}

			var listViewItemSorter = appIDListView.Tag as ListViewItemSorter;
			var clickedColumnHeader = appIDListView.Columns[e.Column];

			if (clickedColumnHeader.Tag is ColumnHeaderType columnHeaderType)
			{
				var sortColumnHeaderType = (ColumnHeaderType?)null;
				var sortOrder = (SortOrder?)null;

				if (listViewItemSorter.SortColumn == columnHeaderType)
				{
					switch (appIDListView.Sorting)
					{
						case SortOrder.None:
							sortOrder = SortOrder.Ascending;
							break;
						case SortOrder.Ascending:
							sortOrder = SortOrder.Descending;
							break;
						case SortOrder.Descending:
							sortOrder = SortOrder.None;
							break;
					}
				}
				else
				{
					sortColumnHeaderType = columnHeaderType;

					if (appIDListView.Sorting == SortOrder.None)
					{
						sortOrder = SortOrder.Ascending;
					}
				}

				SortListView(sortColumnHeaderType, sortOrder);
			}
			else if (clickedColumnHeader.Tag is VdfColumn filter)
			{
				var sortFilter = (string)null;
				var sortOrder = (SortOrder?)null;

				if (!string.IsNullOrEmpty(listViewItemSorter.SortFilter)
					&& listViewItemSorter.SortFilter.Equals(filter.PathFilter, StringComparison.InvariantCulture))
				{
					switch (appIDListView.Sorting)
					{
						case SortOrder.None:
							sortOrder = SortOrder.Ascending;
							break;
						case SortOrder.Ascending:
							sortOrder = SortOrder.Descending;
							break;
						case SortOrder.Descending:
							sortOrder = SortOrder.None;
							break;
					}
				}
				else
				{
					sortFilter = filter.PathFilter;

					if (appIDListView.Sorting == SortOrder.None)
					{
						sortOrder = SortOrder.Ascending;
					}
				}

				SortListView(sortFilter, sortOrder);
			}
		}

		private void SortListView(ColumnHeaderType? columnHeaderType, string customFilter, SortOrder? sortOrder)
		{
			if (!string.IsNullOrEmpty(customFilter))
			{
				SortListView(customFilter, sortOrder);
			}
			else
			{
				SortListView(columnHeaderType, sortOrder);
			}
		}

		private void SortListView(ColumnHeaderType? columnHeaderType, SortOrder? sortOrder)
		{
			var listViewItemSorter = appIDListView.Tag as ListViewItemSorter;

			if (columnHeaderType.HasValue)
			{
				if (listViewItemSorter != null)
				{
					listViewItemSorter.SortFilter = "";
					listViewItemSorter.SortColumn = columnHeaderType.Value;

					Properties.Settings.Default.View_List_SortBy = listViewItemSorter.SortColumn;
				}
			}

			if (sortOrder.HasValue)
			{
				appIDListView.Sorting = sortOrder.Value;
				appIDListView.SetSortIcon(ListViewItemSorter.GetColumnIndex(appIDListView, listViewItemSorter.SortColumn), appIDListView.Sorting);

				Properties.Settings.Default.View_List_SortOrder = appIDListView.Sorting;
			}

			UpdateListViewAfterSorting(listViewItemSorter);
		}

		private void SortListView(string filter, SortOrder? sortOrder)
		{
			var listViewItemSorter = appIDListView.Tag as ListViewItemSorter;

			if (!string.IsNullOrEmpty(filter))
			{
				if (listViewItemSorter != null)
				{
					listViewItemSorter.SortColumn = ColumnHeaderType.None;
					listViewItemSorter.SortFilter = filter;

					Properties.Settings.Default.View_List_CustomSortBy = listViewItemSorter.SortFilter;
				}
			}

			if (sortOrder.HasValue)
			{
				appIDListView.Sorting = sortOrder.Value;
				appIDListView.SetSortIcon(ListViewItemSorter.GetColumnIndex(appIDListView, listViewItemSorter.SortFilter), appIDListView.Sorting);

				Properties.Settings.Default.View_List_SortOrder = appIDListView.Sorting;
			}

			UpdateListViewAfterSorting(listViewItemSorter);
		}

		private void UpdateListViewAfterSorting(ListViewItemSorter listViewItemSorter)
		{
			// TODO: move this to a callback when Sorting/SortingCriteria changes.
			UpdateSortCriteriaDependentControls();
			UpdateSortOrderDependentControls();

			var selectedAppIDs = GetSelectedAppIDs();

			appIDListView.BeginUpdate();
			if (appIDListView.Sorting == SortOrder.None)
			{
				sortedListViewItems.Clear();
				sortedListViewItems.AddRange(filteredListViewItems);
			}
			else
			{
				try
				{
					sortedListViewItems.Sort(listViewItemSorter);
				}
				catch (Exception ex)
				{
					Program.LogError(ex);
				}
			}
			appIDListView.EndUpdate();

			appIDListView.SelectedIndices.Clear();
			SelectAppIDs(selectedAppIDs);
			OnSelectionChanged();
		}

		private void UpdateSortOrderDependentControls()
		{
			noneToolStripMenuItem.Checked = appIDListView.Sorting == SortOrder.None;
			ascendingToolStripMenuItem.Checked = appIDListView.Sorting == SortOrder.Ascending;
			descendingToolStripMenuItem.Checked = appIDListView.Sorting == SortOrder.Descending;

			noneToolStripMenuItem.Enabled = !noneToolStripMenuItem.Checked;
			ascendingToolStripMenuItem.Enabled = !ascendingToolStripMenuItem.Checked;
			descendingToolStripMenuItem.Enabled = !descendingToolStripMenuItem.Checked;

			//context menu
			listSortOrderNoneToolStripMenuItem.Checked = appIDListView.Sorting == SortOrder.None;
			listSortOrderAscendingToolStripMenuItem.Checked = appIDListView.Sorting == SortOrder.Ascending;
			listSortOrderDescendingToolStripMenuItem.Checked = appIDListView.Sorting == SortOrder.Descending;

			//context menu
			listSortOrderNoneToolStripMenuItem.Enabled = !noneToolStripMenuItem.Checked;
			listSortOrderAscendingToolStripMenuItem.Enabled = !ascendingToolStripMenuItem.Checked;
			listSortOrderDescendingToolStripMenuItem.Enabled = !descendingToolStripMenuItem.Checked;

			sortOrderToolStripDropDownButton.Text = appIDListView != null ? appIDListView.Sorting.ToString() : "Sort Order";
		}

		private void UpdateSortCriteriaDependentControls()
		{
			var listViewItemSorter = appIDListView.Tag as ListViewItemSorter;

			appIDToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.AppID;
			titleToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.Title;

			userDataLibraryCapsuleToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.UserDataLibraryCapsule && Properties.Settings.Default.View_Show_LibraryCapsule && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData);
			userDataHeroGraphicToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.UserDataHeroGraphic && Properties.Settings.Default.View_Show_HeroGraphic && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData);
			userDataLogoToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.UserDataLogo && Properties.Settings.Default.View_Show_Logo && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData);
			userDataHeaderToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.UserDataHeader && Properties.Settings.Default.View_Show_Header && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData);
			userDataIconToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.UserDataIcon && Properties.Settings.Default.View_Show_Icon && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData);

			libraryCacheLibraryCapsuleToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.LibraryCacheLibraryCapsule && Properties.Settings.Default.View_Show_LibraryCapsule && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);
			libraryCacheHeroGraphicToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.LibraryCacheHeroGraphic && Properties.Settings.Default.View_Show_HeroGraphic && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);
			libraryCacheLogoToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.LibraryCacheLogo && Properties.Settings.Default.View_Show_Logo && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);
			libraryCacheHeaderToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.LibraryCacheHeader && Properties.Settings.Default.View_Show_Header && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);
			libraryCacheIconToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.LibraryCacheIcon && Properties.Settings.Default.View_Show_Icon && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);

			//context menu
			listSortByAppIDToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.AppID;
			listSortByTitleToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.Title;

			//context menu
			listSortByUserDataLibraryCapsuleToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.UserDataLibraryCapsule && Properties.Settings.Default.View_Show_LibraryCapsule && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData);
			listSortByUserDataHeroGraphicToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.UserDataHeroGraphic && Properties.Settings.Default.View_Show_HeroGraphic && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData);
			listSortByUserDataLogoToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.UserDataLogo && Properties.Settings.Default.View_Show_Logo && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData);
			listSortByUserDataHeaderToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.UserDataHeader && Properties.Settings.Default.View_Show_Header && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData);
			listSortByUserDataIconToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.UserDataIcon && Properties.Settings.Default.View_Show_Icon && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData);

			//context menu
			listSortByLibraryCacheLibraryCapsuleToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.LibraryCacheLibraryCapsule && Properties.Settings.Default.View_Show_LibraryCapsule && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);
			listSortByLibraryCacheHeroGraphicToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.LibraryCacheHeroGraphic && Properties.Settings.Default.View_Show_HeroGraphic && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);
			listSortByLibraryCacheLogoToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.LibraryCacheLogo && Properties.Settings.Default.View_Show_Logo && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);
			listSortByLibraryCacheHeaderToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.LibraryCacheHeader && Properties.Settings.Default.View_Show_Header && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);
			listSortByLibraryCacheIconToolStripMenuItem.Checked = listViewItemSorter != null && listViewItemSorter.SortColumn == ColumnHeaderType.LibraryCacheIcon && Properties.Settings.Default.View_Show_Icon && Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache);

			appIDToolStripMenuItem.Enabled = !appIDToolStripMenuItem.Checked;
			titleToolStripMenuItem.Enabled = !titleToolStripMenuItem.Checked;

			userDataLibraryCapsuleToolStripMenuItem.Enabled = !userDataLibraryCapsuleToolStripMenuItem.Checked;
			userDataHeroGraphicToolStripMenuItem.Enabled = !userDataHeroGraphicToolStripMenuItem.Checked;
			userDataLogoToolStripMenuItem.Enabled = !userDataLogoToolStripMenuItem.Checked;
			userDataHeaderToolStripMenuItem.Enabled = !userDataHeaderToolStripMenuItem.Checked;
			userDataIconToolStripMenuItem.Enabled = !userDataIconToolStripMenuItem.Checked;

			libraryCacheLibraryCapsuleToolStripMenuItem.Enabled = !libraryCacheLibraryCapsuleToolStripMenuItem.Checked;
			libraryCacheHeroGraphicToolStripMenuItem.Enabled = !libraryCacheHeroGraphicToolStripMenuItem.Checked;
			libraryCacheLogoToolStripMenuItem.Enabled = !libraryCacheLogoToolStripMenuItem.Checked;
			libraryCacheHeaderToolStripMenuItem.Enabled = !libraryCacheHeaderToolStripMenuItem.Checked;
			libraryCacheIconToolStripMenuItem.Enabled = !libraryCacheIconToolStripMenuItem.Checked;

			//context menu
			listSortByAppIDToolStripMenuItem.Enabled = !listSortByAppIDToolStripMenuItem.Checked;
			listSortByTitleToolStripMenuItem.Enabled = !listSortByTitleToolStripMenuItem.Checked;

			//context menu
			listSortByUserDataLibraryCapsuleToolStripMenuItem.Enabled = !listSortByUserDataLibraryCapsuleToolStripMenuItem.Checked;
			listSortByUserDataHeroGraphicToolStripMenuItem.Enabled = !listSortByUserDataHeroGraphicToolStripMenuItem.Checked;
			listSortByUserDataLogoToolStripMenuItem.Enabled = !listSortByUserDataLogoToolStripMenuItem.Checked;
			listSortByUserDataHeaderToolStripMenuItem.Enabled = !listSortByUserDataHeaderToolStripMenuItem.Checked;
			listSortByUserDataIconToolStripMenuItem.Enabled = !listSortByUserDataIconToolStripMenuItem.Checked;

			//context menu
			listSortByLibraryCacheLibraryCapsuleToolStripMenuItem.Enabled = !listSortByLibraryCacheLibraryCapsuleToolStripMenuItem.Checked;
			listSortByLibraryCacheHeroGraphicToolStripMenuItem.Enabled = !listSortByLibraryCacheHeroGraphicToolStripMenuItem.Checked;
			listSortByLibraryCacheLogoToolStripMenuItem.Enabled = !listSortByLibraryCacheLogoToolStripMenuItem.Checked;
			listSortByLibraryCacheHeaderToolStripMenuItem.Enabled = !listSortByLibraryCacheHeaderToolStripMenuItem.Checked;
			listSortByLibraryCacheIconToolStripMenuItem.Enabled = !listSortByLibraryCacheIconToolStripMenuItem.Checked;

			sortByToolStripDropDownButton.Text = listViewItemSorter != null
				? (!string.IsNullOrEmpty(listViewItemSorter.SortFilter)
				? GetFilterLastPart(listViewItemSorter.SortFilter)
				: listViewItemSorter.SortColumn.ToString())
				: "Sort By";

			string GetFilterLastPart(string filter)
			{
				if (string.IsNullOrEmpty(filter))
				{
					return "";
				}

				var tokens = filter.Split(new char[] { '/' });

				return tokens[tokens.Length - 1];
			}
		}

		#endregion

		#region AppID Text Box + Timer

		private void appIDTextBox_TextChanged(object sender, EventArgs e)
		{
			if (!Properties.Settings.Default.Details_Filter_AutoCommit_Enable)
			{
				return;
			}
			else if (Properties.Settings.Default.Details_Filter_AutoCommit_Delay == 0)
			{
				appIDTextBoxTimer_Tick(appIDTextBoxTimer, EventArgs.Empty);
			}
			else if (Properties.Settings.Default.Details_Filter_AutoCommit_Delay > 0)
			{
				appIDTextBoxTimer.Stop();
				appIDTextBoxTimer.Start();
			}
		}

		private void appIDTextBox_ValueChanged(object sender, EventArgs e)
		{
			appIDTextBoxTimer_Tick(appIDTextBoxTimer, EventArgs.Empty);
		}

		private void appIDTextBoxTimer_Tick(object sender, EventArgs e)
		{
			appIDTextBoxTimer.Stop();

			Properties.Settings.Default.AppID = (ulong)appIDTextBox.Value;
		}

		#endregion

		#region Menu

		#region File

		private void fileExitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FileExit();
		}

		#endregion

		#region View

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CollectAppIDsAndPopulateListView();
			SelectFirstAppIDFromListView();
		}

		private void viewLibraryCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToggleLibraryCapsuleAssetViewFilter();
		}

		private void viewHeroGraphicToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToggleHeroGraphicAssetViewFilter();
		}

		private void viewLogoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToggleLogoAssetViewFilter();
		}

		private void viewHeaderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToggleHeaderAssetViewFilter();
		}

		private void viewIconToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToggleIconAssetViewFilter();
		}

		#region List

		private void viewListShowAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ListShowAllViewFilter();
		}

		private void viewListUserDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ListShowUserDataViewFilter();
		}

		private void viewListLibraryCacheToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ListShowLibraryCacheViewFilter();
		}

		private void viewListAlternateHeadersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ToggleAlternateListColumnHeaders();
		}

		private void appIDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.AppID, appIDListView.Sorting);
		}

		private void titleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.Title, appIDListView.Sorting);
		}

		private void userDataLibraryCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.UserDataLibraryCapsule, appIDListView.Sorting);
		}

		private void userDataHeroGraphicToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.UserDataHeroGraphic, appIDListView.Sorting);
		}

		private void userDataLogoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.UserDataLogo, appIDListView.Sorting);
		}

		private void userDataHeaderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.UserDataHeader, appIDListView.Sorting);
		}

		private void userDataIconToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.UserDataIcon, appIDListView.Sorting);
		}

		private void libraryCacheLibraryCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.LibraryCacheLibraryCapsule, appIDListView.Sorting);
		}

		private void libraryCacheHeroGraphicToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.LibraryCacheHeroGraphic, appIDListView.Sorting);
		}

		private void libraryCacheLogoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.LibraryCacheLogo, appIDListView.Sorting);
		}

		private void libraryCacheHeaderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.LibraryCacheHeader, appIDListView.Sorting);
		}

		private void libraryCacheIconToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView(ColumnHeaderType.LibraryCacheIcon, appIDListView.Sorting);
		}

		private void noneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView((appIDListView.Tag as ListViewItemSorter)?.SortColumn, SortOrder.None);
		}

		private void ascendingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView((appIDListView.Tag as ListViewItemSorter)?.SortColumn, SortOrder.Ascending);
		}

		private void descendingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SortListView((appIDListView.Tag as ListViewItemSorter)?.SortColumn, SortOrder.Descending);
		}

		#endregion

		#region Details

		private void viewDetailsShowAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DetailsShowAllViewFilter();
		}

		private void viewDetailsUserDataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DetailsShowUserDataViewFilter();
		}

		private void viewDetailsLibraryCacheToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DetailsShowLibraryCacheViewFilter();
		}

		#endregion

		#endregion

		#region Tools

		private void toolsChangeSteamPathToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ModifierKeys == Keys.Alt)
			{
				AutoFindSteamPath();
			}
			else
			{
				var path = PromptSteamPath();

				if (path is null)
				{
					return;
				}

				Properties.Settings.Default.SteamAppPath = path;
			}
		}

		private void toolsChangeUserToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ModifierKeys == Keys.Alt)
			{
				AutoFindUser();
			}
			else
			{
				var userID = PromptUser(SteamUtils.EnumerateUserIDs());

				if (userID is null)
				{
					return;
				}

				Properties.Settings.Default.SteamUserID = userID.Value;
			}
		}

		private void toolsChangeNonSteamAppIconCachePathToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ModifierKeys == Keys.Alt)
			{
				AutoInitNonSteamAppIconCacheDirectory();
			}
			else
			{
				var path = PromptNonSteamAppIconCachePath();

				if (path is null)
				{
					return;
				}

				Properties.Settings.Default.NonSteamAppIconCachePath = path;
			}
		}

		private void toolsUpdateIconCacheToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UpdateIconCache();
		}

		private void updateFromExecutablesAndCustomIconsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UpdateNonSteamAppIconCache();
		}

		private void updateFromExecutablesOnlyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UpdateNonSteamAppIconCacheFromExecutablesOnly();
		}

		private void updateFromCustomIconsOnlyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UpdateNonSteamAppIconCacheFromCustomIconsOnly();
		}

		private void toolSaveShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UpdateShortcutsVdf();
		}

		private void toolsOptionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowOptions();
		}

		#endregion

		#region Help

		private void helpAboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowAbout();
		}

		#endregion

		#endregion

		#endregion

		#region Commands

		private static void FileExit()
		{
			Application.Exit();
		}

		private static void ToggleLibraryCapsuleAssetViewFilter()
		{
			Properties.Settings.Default.View_Show_LibraryCapsule = !Properties.Settings.Default.View_Show_LibraryCapsule;
		}

		private static void ToggleHeroGraphicAssetViewFilter()
		{
			Properties.Settings.Default.View_Show_HeroGraphic = !Properties.Settings.Default.View_Show_HeroGraphic;
		}

		private static void ToggleLogoAssetViewFilter()
		{
			Properties.Settings.Default.View_Show_Logo = !Properties.Settings.Default.View_Show_Logo;
		}

		private static void ToggleHeaderAssetViewFilter()
		{
			Properties.Settings.Default.View_Show_Header = !Properties.Settings.Default.View_Show_Header;
		}

		private static void ToggleIconAssetViewFilter()
		{
			Properties.Settings.Default.View_Show_Icon = !Properties.Settings.Default.View_Show_Icon;
		}

		private void DetailsShowAllViewFilter()
		{
			Properties.Settings.Default.Details_CacheLocation = CacheLocation.UserData | CacheLocation.LibraryCache;
		}

		private void DetailsShowUserDataViewFilter()
		{
			Properties.Settings.Default.Details_CacheLocation = CacheLocation.UserData;
		}

		private void DetailsShowLibraryCacheViewFilter()
		{
			Properties.Settings.Default.Details_CacheLocation = CacheLocation.LibraryCache;
		}

		private void ToggleDetailsLibraryCacheViewFilter()
		{
			if (Properties.Settings.Default.Details_CacheLocation.HasFlag(CacheLocation.LibraryCache))
			{
				Properties.Settings.Default.Details_CacheLocation &= ~CacheLocation.LibraryCache;
			}
			else
			{
				Properties.Settings.Default.Details_CacheLocation |= CacheLocation.LibraryCache;
			}
		}

		private void ToggleDetailsUserDataViewFilter()
		{
			if (Properties.Settings.Default.Details_CacheLocation.HasFlag(CacheLocation.UserData))
			{
				Properties.Settings.Default.Details_CacheLocation &= ~CacheLocation.UserData;
			}
			else
			{
				Properties.Settings.Default.Details_CacheLocation |= CacheLocation.UserData;
			}
		}

		private static void ToggleAlternateListColumnHeaders()
		{
			Properties.Settings.Default.View_UseAlternateColumnHeaders = !Properties.Settings.Default.View_UseAlternateColumnHeaders;
		}

		private void ListShowAllViewFilter()
		{
			Properties.Settings.Default.List_CacheLocation = CacheLocation.UserData | CacheLocation.LibraryCache;
		}

		private void ListShowUserDataViewFilter()
		{
			Properties.Settings.Default.List_CacheLocation = CacheLocation.UserData;
		}

		private void ListShowLibraryCacheViewFilter()
		{
			Properties.Settings.Default.List_CacheLocation = CacheLocation.LibraryCache;
		}

		private void ToggleListLibraryCacheViewFilter()
		{
			if (Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache))
			{
				Properties.Settings.Default.List_CacheLocation &= ~CacheLocation.LibraryCache;
			}
			else
			{
				Properties.Settings.Default.List_CacheLocation |= CacheLocation.LibraryCache;
			}
		}

		private void ToggleListUserDataViewFilter()
		{
			if (Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData))
			{
				Properties.Settings.Default.List_CacheLocation &= ~CacheLocation.UserData;
			}
			else
			{
				Properties.Settings.Default.List_CacheLocation |= CacheLocation.UserData;
			}
		}

		private void ShowOptions()
		{
			using (var dialog = new OptionsDialog
			{
			})
			{
				// NOTE: let the user cancel out of the dialog and manually refresh the list if they want.

				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					// TODO: add a property to OptionsDialog that marks it as being dirty
					// so that I can avoid refreshing the list unless absolutely necessary.

					CollectAppIDsAndPopulateListView();
					UpdateSortCriteriaDependentControls();
					BuildVdfColumnsSortingMenu();
				}
			}
		}

		private void UpdateShortcutsVdf()
		{
			var changelog = new List<string>();
			/*
			var existingFileExtensions = new Dictionary<ulong, string>();

			foreach (var fileName in Directory.EnumerateFiles(SteamUtils.GetNonSteamAppIconCachePath(), "*.*", SearchOption.TopDirectoryOnly))
			{
				var fileExtension = Path.GetExtension(fileName);

				if (!validUserIconAssetFileExtensions.Contains(fileExtension, StringComparer.InvariantCultureIgnoreCase))
				{
					continue;
				}

				if (!ulong.TryParse(Path.GetFileNameWithoutExtension(fileName), out var appID))
				{
					continue;
				}

				if (!shortcuts.Items.TryGetValue(appID, out var shortcut))
				{
					continue;
				}

				if (!string.IsNullOrEmpty(shortcut.Icon))
				{
					continue;
				}

				existingFileExtensions.Add(appID, fileExtension);
				shortcut.Icon = fileName;
			}
			*/

			// pre-change everything.
			/*
			foreach (var shortcut in shortcuts.Items)
			{
				if (!existingFileExtensions.TryGetValue(shortcut.Key, out var fileExtension))
				{
					fileExtension = ".png";
				}

				var iconPath = Path.ChangeExtension(Path.Combine(SteamUtils.GetNonSteamAppIconCachePath(), shortcut.Key.ToString(CultureInfo.InvariantCulture)), fileExtension);

				if (!string.IsNullOrEmpty(shortcut.Value.Icon))
				{
					changelog.Add($"[{shortcut.Key.ToString(CultureInfo.InvariantCulture)}]");
					changelog.Add(shortcut.Value.Icon);
					changelog.Add("");
				}

				shortcut.Value.Icon = iconPath;
			}
			*/

			var shortcutsVdfFullName = Path.Combine(Properties.Settings.Default.SteamAppPath, "userdata", Properties.Settings.Default.SteamUserID.ToString(CultureInfo.InvariantCulture), "config", "shortcuts.vdf");
			var shortcutsVdfBackupFullName = Path.Combine(Properties.Settings.Default.BackupPath, "userdata", Properties.Settings.Default.SteamUserID.ToString(CultureInfo.InvariantCulture), "config", $"shortcuts ({DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ff", CultureInfo.InvariantCulture)}).vdf.bk");
			var changelogFullName = Path.Combine(Application.StartupPath, "userdata", Properties.Settings.Default.SteamUserID.ToString(CultureInfo.InvariantCulture), "config", $"shortcuts ({DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ff", CultureInfo.InvariantCulture)}).log");

			try
			{
				if (!string.IsNullOrEmpty(Path.GetDirectoryName(shortcutsVdfBackupFullName)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(shortcutsVdfBackupFullName));
				}

				if (!string.IsNullOrEmpty(Path.GetDirectoryName(changelogFullName)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(changelogFullName));
				}

				File.WriteAllLines(changelogFullName, changelog);
				File.Copy(shortcutsVdfFullName, shortcutsVdfBackupFullName, overwrite: true);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ParentForm, ex.Message, "I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Program.LogError(ex);
				return;
			}

			using (var stream = File.OpenWrite(shortcutsVdfFullName))
			{
				var vdfWriter = new VdfWriter(stream);

				vdfWriter.Write(shortcuts.Root);
			}
		}

		private void ShowAbout()
		{
			var sb = new StringBuilder();
			var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

			sb.AppendLine($"{fileVersionInfo.ProductName} {fileVersionInfo.FileVersion}");

			MessageBox.Show(this, sb.ToString(), "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		#endregion

		#region Implementation

		private bool AutoInitBackupDirectory()
		{
			Properties.Settings.Default.BackupPath = SteamUtils.GetDefaultBackupFolder();

			try
			{
				Directory.CreateDirectory(Properties.Settings.Default.BackupPath);
			}
			catch (Exception ex)
			{
				Program.LogError($"Couldn't create the backup directory '{Properties.Settings.Default.BackupPath}'.");
				Program.LogError(ex);
				return false;
			}

			return true;
		}

		private bool AutoInitNonSteamAppIconCacheDirectory()
		{
			Properties.Settings.Default.NonSteamAppIconCachePath = SteamUtils.GetDefaultNonSteamAppIconCacheFolder();

			try
			{
				Directory.CreateDirectory(Properties.Settings.Default.NonSteamAppIconCachePath);
			}
			catch (Exception ex)
			{
				Program.LogError($"Couldn't create the default non steam app icon cache directory '{Properties.Settings.Default.NonSteamAppIconCachePath}'.");
				Program.LogError(ex);
				return false;
			}

			return true;
		}

		private bool AutoFindUser()
		{
			var foundUserIDs = SteamUtils.EnumerateUserIDs();
			var foundUserIDCount = foundUserIDs.Count();

			if (foundUserIDCount > 1)
			{
				var userID = PromptUser(foundUserIDs);

				if (userID != null)
				{
					Properties.Settings.Default.SteamUserID = userID.Value;

					return true;
				}
			}
			else if (foundUserIDCount == 1)
			{
				Properties.Settings.Default.SteamUserID = foundUserIDs.First();
				return true;
			}

			return false;
		}

		private ulong? PromptUser(IEnumerable<ulong> userIDs)
		{
			using (var dialog = new ComboBoxDialog
			{
				Text = "Select the Steam user",
				SelectedValue = userIDs.First(),
				Values = userIDs.Cast<object>().ToArray(),
			})
			{
				dialog.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					return (ulong)dialog.SelectedValue;
				}
			}

			return null;
		}

		private string PromptSteamPath()
		{
			if (ModifierKeys.HasFlag(Keys.Shift))
			{
				using (var dialog = new OpenFileDialog
				{
					CheckFileExists = false,
					CheckPathExists = false,
					Title = "Steam program location",
				})
				{
					if (dialog.ShowDialog(this) == DialogResult.OK)
					{
						var path = Path.GetDirectoryName(dialog.FileName);

						if (!string.IsNullOrEmpty(Path.GetDirectoryName(path)))
						{
							Directory.CreateDirectory(Path.GetDirectoryName(path));
						}

						return path;
					}
				}
			}
			else
			{
				using (var dialog = new FolderBrowserDialog
				{
					Description = "Steam program location:",
					RootFolder = Environment.SpecialFolder.MyComputer,
					SelectedPath = Properties.Settings.Default.SteamAppPath,
					ShowNewFolderButton = false,
				})
				{
					// work-around for the treeview not scrolling to the selected node.
					// FIXME: it doesn't work if it takes too long to populate the tree.
					SendKeys.Send("{TAB}{TAB}{RIGHT}");

					if (dialog.ShowDialog(this) == DialogResult.OK)
					{
						return dialog.SelectedPath;
					}
				}
			}

			return null;
		}

		private bool AutoFindSteamPath()
		{
			var path = SteamUtils.FindSteamPathFromProgramFiles()
				?? SteamUtils.FindSteamPathFromRegistry()
				?? PromptSteamPath();

			if (string.IsNullOrEmpty(path))
			{
				return false;
			}

			Properties.Settings.Default.SteamAppPath = path;

			return true;
		}

		private string PromptNonSteamAppIconCachePath()
		{
			if (ModifierKeys.HasFlag(Keys.Shift))
			{
				using (var dialog = new OpenFileDialog
				{
					CheckFileExists = false,
					CheckPathExists = false,
					Title = "Non-Steam app icon cache location",
				})
				{
					if (dialog.ShowDialog(this) == DialogResult.OK)
					{
						var path = Path.GetDirectoryName(dialog.FileName);

						if (!string.IsNullOrEmpty(path))
						{
							Directory.CreateDirectory(path);
						}

						return path;
					}
				}
			}
			else
			{
				using (var dialog = new FolderBrowserDialog
				{
					Description = "Non-Steam app icon cache location:",
					RootFolder = Environment.SpecialFolder.MyComputer,
					SelectedPath = Properties.Settings.Default.NonSteamAppIconCachePath,
					ShowNewFolderButton = true,
				})
				{
					// work-around for the treeview not scrolling to the selected node.
					// FIXME: it doesn't work if it takes too long to populate the tree.
					SendKeys.Send("{TAB}{TAB}{RIGHT}");

					if (dialog.ShowDialog(this) == DialogResult.OK)
					{
						return dialog.SelectedPath;
					}
				}
			}

			return null;
		}

		private void CollectAppIDsAndPopulateListView()
		{
			if (string.IsNullOrEmpty(Properties.Settings.Default.SteamAppPath)
				|| Properties.Settings.Default.SteamUserID == 0)
			{
				return;
			}

			CollectAppIDs();
			PopulateListView();
		}

		private void CollectAppIDs()
		{
			assets.Clear();

			if (Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData))
			{
				FetchAssets(CacheLocation.UserData);
			}

			if (Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache))
			{
				FetchAssets(CacheLocation.LibraryCache);
			}
		}

		private void FetchAssets(CacheLocation cacheLocation)
		{
			var path = (string)null;
			var regex = (Regex)null;

			// this is too inefficient...
			/*
			if (cacheLocation == CacheLocation.UserData)
			{
				path = SteamUtils.GetIconCachePath();
				regex = SteamUtils.IconFileNameRegex;

				foreach (var fileName in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly))
				{
					var match = regex.Match(Path.GetFileName(fileName));

					if (match.Success)
					{
						// look up icon in ALL appids then create a new entry for this appid
					}
				}
			}
			*/

			switch (cacheLocation)
			{
				case CacheLocation.LibraryCache:
					path = SteamUtils.GetLibraryCachePath();
					regex = SteamUtils.AppIDLibraryCacheFileNameRegex;
					break;
				case CacheLocation.UserData:
					path = SteamUtils.GetUserGridsPath();
					regex = SteamUtils.AppIDUserDataFileNameRegex;
					break;
				default:
					return;
			}

			if (cacheLocation == CacheLocation.UserData)
			{
				foreach (var kvp in shortcuts.Items)
				{
					if (!assets.ContainsKey(kvp.Key))
					{
						assets.Add(kvp.Key, new LibraryAssetsGroup());

						if (!string.IsNullOrEmpty(Properties.Settings.Default.NonSteamAppIconCachePath))
						{
							if (shortcuts.Items.TryGetValue(Properties.Settings.Default.AppID, out var shortcut))
							{
								// NOTE: update the display only for icons that Steam uses (with the current un/modded shortcuts.vdf), not what this app uses.

								//if (!string.IsNullOrEmpty(shortcut.Icon))
								{
									foreach (var fileExt in validUserIconAssetFileExtensions)
									{
										if (!Properties.Settings.Default.AllowProgramsAsIcon
											&& fileExt.Equals(".exe"))
										{
											continue;
										}

										//var originalIconFileName = shortcut.Icon;
										var iconPath = Path.ChangeExtension(Path.Combine(SteamUtils.GetNonSteamAppIconCachePath(), Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)), fileExt);

										if (File.Exists(iconPath)
											//|| File.Exists(originalIconFileName)
											)
										{
											assets[kvp.Key].UserData.Icon = true;
										}

										if (assets[kvp.Key].UserData.Icon)
										{
											break;
										}
									}
								}
							}
						}
					}
				}
			}

			if (!Directory.Exists(path))
			{
				Program.LogError($"Cannot fetch assets. Directory {path} does not exist.");
				return;
			}

			foreach (var fileName in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly))
			{
				var match = regex.Match(Path.GetFileName(fileName));

				if (match.Success)
				{
					if (!ulong.TryParse(match.Groups["AppID"].Value, out var appID))
					{
						continue;
					}

					if (!assets.ContainsKey(appID))
					{
						assets.Add(appID, new LibraryAssetsGroup());

						if (cacheLocation == CacheLocation.UserData)
						{
							if (appInfos.Items.TryGetValue(appID, out var appInfo))
							{
								// NOTE: there's some games without one, and one which has a clienticns instead.
								var clientIconTypes = new Dictionary<string, string>
								{
									{ "clienticon", ".ico" },
									{ "clienticns", ".icns" },
								};

								foreach (var clientIconType in clientIconTypes)
								{
									if (appInfo.Data.Node.ContainsKey(clientIconType.Key))
									{
										var iconPath = Path.ChangeExtension(Path.Combine(SteamUtils.GetIconCachePath(), appInfo.Data.Node.GetString(clientIconType.Key)), clientIconType.Value);

										if (Properties.Settings.Default.AutoDownloadMissingAssets
											&& !File.Exists(iconPath))
										{
											var url = $"https://steamcdn-a.akamaihd.net/steamcommunity/public/images/apps/{appID.ToString(CultureInfo.InvariantCulture)}/{appInfo.Data.Node.GetString(clientIconType.Key)}{clientIconType.Value}";

											webClient.DownloadFile(url, iconPath);
										}

										if (File.Exists(iconPath))
										{
											assets[appID].UserData.Icon = true;
										}

										break;
									}
								}
							}
						}
					}

					switch (cacheLocation)
					{
						case CacheLocation.UserData:
							switch (match.Groups["AssetType"].Value)
							{
								case "p":
									assets[appID].UserData.LibraryCapsule = true;
									break;
								case "_hero":
									assets[appID].UserData.HeroGraphic = true;
									break;
								case "_logo":
									assets[appID].UserData.Logo = true;
									break;
								case "":
									assets[appID].UserData.Header = true;
									break;
							}
							break;
						case CacheLocation.LibraryCache:
							switch (match.Groups["AssetType"].Value)
							{
								case "_library_600x900":
									assets[appID].LibraryCache.LibraryCapsule = true;
									break;
								case "_library_hero":
									assets[appID].LibraryCache.HeroGraphic = true;
									break;
								case "_logo":
									assets[appID].LibraryCache.Logo = true;
									break;
								case "_header":
									assets[appID].LibraryCache.Header = true;
									break;
								case "_icon":
									assets[appID].LibraryCache.Icon = true;
									break;
							}
							break;
					}
				}
			}
		}

		private int AppIDColumnIndex;
		private int TitleColumnIndex;

		private void PopulateListView()
		{
			InitializeColumnHeaders();

			cachedListViewItems.Clear();
			filteredListViewItems.Clear();
			sortedListViewItems.Clear();

			foreach (var item in assets)
			{
				appInfos.Items.TryGetValue(item.Key, out var appInfo);
				shortcuts.Items.TryGetValue(item.Key, out var shortcut);

				var listViewSubItems = new List<ListViewItem.ListViewSubItem>();

				if (appInfos.Items.Count == 0
					&& shortcuts.Items.Count == 0)
				{
					listViewSubItems.Add(new ListViewItem.ListViewSubItem { Text = item.Key.ToString(CultureInfo.InvariantCulture), Tag = item.Key });
					listViewSubItems.Add(new ListViewItem.ListViewSubItem { Text = "", Tag = "" });
				}
				else
				{
					var appName = (string)appInfo?.Data?.Node?.FindValueByPath("appinfo/common/name")
						?? (string)shortcut?.Node?.FindValueByPath("appname")
						?? (string)shortcut?.Node?.FindValueByPath("AppName")
						?? "";

					listViewSubItems.Add(new ListViewItem.ListViewSubItem { Text = appName, Tag = appName });
					listViewSubItems.Add(new ListViewItem.ListViewSubItem { Text = item.Key.ToString(CultureInfo.InvariantCulture), Tag = item.Key });
				}

				if (Properties.Settings.Default.List_Column_Types is null)
				{
					Properties.Settings.Default.List_Column_Types = new System.Collections.Specialized.StringCollection();
				}

				foreach (var columnHeaderTypes in Properties.Settings.Default.List_Column_Types)
				{
					if (string.IsNullOrEmpty(columnHeaderTypes))
					{
						continue;
					}

					var tokens = columnHeaderTypes.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
					var value = (object)null;
					var content = (string)null;
					var color = (Color?)null;

					foreach (var columnHeaderType in tokens)
					{
						if (string.IsNullOrEmpty(columnHeaderType))
						{
							continue;
						}

						var propertyType = VdfUtils.GetVdfPropertyTypeByPath(columnHeaderType);
						value = appInfo?.Data?.Node?.FindValueByPath(columnHeaderType) ?? shortcut?.Node?.FindValueByPath(columnHeaderType);
						content = VdfUtils.FormatVdfValue(propertyType, value);
						color = VdfUtils.GetColor(propertyType, value);

						if (value != null)
						{
							break;
						}
					}

					listViewSubItems.Add(new ListViewItem.ListViewSubItem
					{
						ForeColor = (color.HasValue && !color.Value.IsEmpty) ? color.Value : ListView.DefaultForeColor,
						Text = content ?? "",
						Tag = value,
					});
				}

				if (Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData))
				{
					AddAssetsListViewItems(item, listViewSubItems, item.Value.UserData);
				}

				if (Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache))
				{
					AddAssetsListViewItems(item, listViewSubItems, item.Value.LibraryCache);
				}

				cachedListViewItems.Add(new ListViewItem(listViewSubItems.ToArray(), 0)
				{
					UseItemStyleForSubItems = false,
					Tag = appInfo ?? (object)shortcut ?? item.Key,
				});
			}

			AppIDColumnIndex = ListViewItemSorter.GetColumnIndex(appIDListView, ColumnHeaderType.AppID);
			TitleColumnIndex = ListViewItemSorter.GetColumnIndex(appIDListView, ColumnHeaderType.Title);

			FilterAndSortListView();

			runWithToolStripMenuItem.Enabled = CanRunWith();
			copyToolStripMenuItem.Enabled = CanCopy();
			selectAllToolStripMenuItem.Enabled = CanSelectAll();
		}

		private void AddAssetsListViewItems(KeyValuePair<ulong, LibraryAssetsGroup> item, List<ListViewItem.ListViewSubItem> listViewSubItems, LibraryAssets libraryAssets)
		{
			var okString = Properties.Settings.Default.List_Assets_Exist_String;
			var ngString = Properties.Settings.Default.List_Assets_Missing_String;
			var okColor = Properties.Settings.Default.List_Assets_Exist_Color;
			var ngColor = Properties.Settings.Default.List_Assets_Missing_Color;

			if (Properties.Settings.Default.View_Show_LibraryCapsule)
			{
				listViewSubItems.Add(new ListViewItem.ListViewSubItem { Text = libraryAssets.LibraryCapsule ? okString : ngString, ForeColor = libraryAssets.LibraryCapsule ? okColor : ngColor, Tag = libraryAssets.LibraryCapsule });
			}

			if (Properties.Settings.Default.View_Show_HeroGraphic)
			{
				listViewSubItems.Add(new ListViewItem.ListViewSubItem { Text = libraryAssets.HeroGraphic ? okString : ngString, ForeColor = libraryAssets.HeroGraphic ? okColor : ngColor, Tag = libraryAssets.HeroGraphic });
			}

			if (Properties.Settings.Default.View_Show_Logo)
			{
				listViewSubItems.Add(new ListViewItem.ListViewSubItem { Text = libraryAssets.Logo ? okString : ngString, ForeColor = libraryAssets.Logo ? okColor : ngColor, Tag = libraryAssets.Logo });
			}

			if (Properties.Settings.Default.View_Show_Header)
			{
				listViewSubItems.Add(new ListViewItem.ListViewSubItem { Text = libraryAssets.Header ? okString : ngString, ForeColor = libraryAssets.Header ? okColor : ngColor, Tag = libraryAssets.Header });
			}

			if (Properties.Settings.Default.View_Show_Icon)
			{
				listViewSubItems.Add(new ListViewItem.ListViewSubItem { Text = libraryAssets.Icon ? okString : ngString, ForeColor = libraryAssets.Icon ? okColor : ngColor, Tag = libraryAssets.Icon });
			}
		}

		private void InitializeColumnHeaders()
		{
			appIDListView.Columns.Clear();

			var useAlt = Properties.Settings.Default.View_UseAlternateColumnHeaders;
			var columnHeaders = new List<ColumnHeader>();

			if (appInfos.Items.Count == 0
				&& shortcuts.Items.Count == 0)
			{
				columnHeaders.Add(new ColumnHeader { Text = "AppID", Tag = ColumnHeaderType.AppID });
				columnHeaders.Add(new ColumnHeader { Text = "Title", Tag = ColumnHeaderType.Title });
			}
			else
			{
				columnHeaders.Add(new ColumnHeader { Text = "Title", Tag = ColumnHeaderType.Title, DisplayIndex = 1, });
				columnHeaders.Add(new ColumnHeader { Text = "AppID", Tag = ColumnHeaderType.AppID, DisplayIndex = 0, });
			}

			if (Properties.Settings.Default.List_Column_Types is null)
			{
				Properties.Settings.Default.List_Column_Types = new System.Collections.Specialized.StringCollection();
			}

			foreach (var columnHeaderTypes in Properties.Settings.Default.List_Column_Types)
			{
				if (string.IsNullOrEmpty(columnHeaderTypes))
				{
					continue;
				}

				var tokens = columnHeaderTypes.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
				var columnHeaderType = tokens[0];

				if (string.IsNullOrEmpty(columnHeaderType))
				{
					continue;
				}

				var tokens2 = columnHeaderType.Split(new char[] { '/' });
				var lastPart = tokens2[tokens2.Length - 1];

				var tag = new VdfColumn
				{
					PathFilter = columnHeaderType,
					ValueFilter = columnHeaderType,
					PropertyType = VdfUtils.GetVdfPropertyTypeByPath(columnHeaderType),
				};

				columnHeaders.Add(new ColumnHeader { Text = lastPart, Tag = tag });
			}

			if (Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.UserData))
			{
				var prefix = "u";

				if (Properties.Settings.Default.View_Show_LibraryCapsule)
				{
					columnHeaders.Add(new ColumnHeader { Text = prefix + (useAlt ? "C" : "Capsule"), Tag = ColumnHeaderType.UserDataLibraryCapsule });
				}

				if (Properties.Settings.Default.View_Show_HeroGraphic)
				{
					columnHeaders.Add(new ColumnHeader { Text = prefix + (useAlt ? "H" : "Hero"), Tag = ColumnHeaderType.UserDataHeroGraphic });
				}

				if (Properties.Settings.Default.View_Show_Logo)
				{
					columnHeaders.Add(new ColumnHeader { Text = prefix + (useAlt ? "L" : "Logo"), Tag = ColumnHeaderType.UserDataLogo });
				}

				if (Properties.Settings.Default.View_Show_Header)
				{
					columnHeaders.Add(new ColumnHeader { Text = prefix + (useAlt ? "B" : "Header"), Tag = ColumnHeaderType.UserDataHeader });
				}

				if (Properties.Settings.Default.View_Show_Icon)
				{
					columnHeaders.Add(new ColumnHeader { Text = prefix + (useAlt ? "I" : "Icon"), Tag = ColumnHeaderType.UserDataIcon });
				}
			}

			if (Properties.Settings.Default.List_CacheLocation.HasFlag(CacheLocation.LibraryCache))
			{
				var prefix = "l";

				if (Properties.Settings.Default.View_Show_LibraryCapsule)
				{
					columnHeaders.Add(new ColumnHeader { Text = prefix + (useAlt ? "C" : "Capsule"), Tag = ColumnHeaderType.LibraryCacheLibraryCapsule });
				}

				if (Properties.Settings.Default.View_Show_HeroGraphic)
				{
					columnHeaders.Add(new ColumnHeader { Text = prefix + (useAlt ? "H" : "Hero"), Tag = ColumnHeaderType.LibraryCacheHeroGraphic });
				}

				if (Properties.Settings.Default.View_Show_Logo)
				{
					columnHeaders.Add(new ColumnHeader { Text = prefix + (useAlt ? "L" : "Logo"), Tag = ColumnHeaderType.LibraryCacheLogo });
				}

				if (Properties.Settings.Default.View_Show_Header)
				{
					columnHeaders.Add(new ColumnHeader { Text = prefix + (useAlt ? "B" : "Header"), Tag = ColumnHeaderType.LibraryCacheHeader });
				}

				if (Properties.Settings.Default.View_Show_Icon)
				{
					columnHeaders.Add(new ColumnHeader { Text = prefix + (useAlt ? "I" : "Icon"), Tag = ColumnHeaderType.LibraryCacheIcon });
				}
			}

			appIDListView.Columns.AddRange(columnHeaders.ToArray());

			ApplyListViewColumnUserOrder();
		}

		private void ApplyListViewColumnUserOrder()
		{
			if (Properties.Settings.Default.List_Column_Order is null)
			{
				Properties.Settings.Default.List_Column_Order = new System.Collections.Specialized.StringCollection();

				// quick hack...
				appIDListView_ColumnReordered(appIDListView, new ColumnReorderedEventArgs(0, 0, appIDListView.Columns[0]));
			}

			// if we removed or added a column, then just forget about the ordering (just not worth the trouble...)

			if (Properties.Settings.Default.List_Column_Order.Count != appIDListView.Columns.Count)
			{
				Properties.Settings.Default.List_Column_Order.Clear();
				return;
			}

			for (var i = 0; i < Properties.Settings.Default.List_Column_Order.Count; ++i)
			{
				var value = Properties.Settings.Default.List_Column_Order[i];
				var columnIndex = 0;

				if (!Enum.TryParse<ColumnHeaderType>(value, ignoreCase: true, out var columnHeaderType)
					|| (columnIndex = ListViewItemSorter.GetColumnIndex(appIDListView, columnHeaderType)) == -1)
				{
					columnIndex = ListViewItemSorter.GetColumnIndex(appIDListView, value);
				}

				appIDListView.Columns[columnIndex].DisplayIndex = i;
			}
		}

		private void AutoResizeColumns()
		{
			var items = filteredListViewItems.Count == 0 ? cachedListViewItems : filteredListViewItems;

			if (items.Count == 0)
			{
				appIDListView.AutoResizeColumn(items, appIDListView.Columns[AppIDColumnIndex], ColumnHeaderAutoResizeStyleEx.Max, Properties.Settings.Default.List_Column_AppID_MinAutoWidth, Properties.Settings.Default.List_Column_AppID_MaxAutoWidth);
			}
			else
			{
				foreach (ColumnHeader columnHeader in appIDListView.Columns)
				{
					if (columnHeader.Index == AppIDColumnIndex)
					{
						appIDListView.AutoResizeColumn(items, columnHeader, ColumnHeaderAutoResizeStyleEx.Max, Properties.Settings.Default.List_Column_AppID_MinAutoWidth, Properties.Settings.Default.List_Column_AppID_MaxAutoWidth);
					}
					else if (columnHeader.Index == TitleColumnIndex)
					{
						appIDListView.AutoResizeColumn(items, columnHeader, ColumnHeaderAutoResizeStyleEx.Max, Properties.Settings.Default.List_Column_Title_MinAutoWidth, Properties.Settings.Default.List_Column_Title_MaxAutoWidth);
					}
					else
					{
						appIDListView.AutoResizeColumn(items, columnHeader, ColumnHeaderAutoResizeStyleEx.Max);
					}
				}
			}
		}

		private void SelectFirstAppIDFromListView()
		{
			if (appIDListView.SelectedIndices.Count > 0
				&& appIDListView.SelectedIndices[0] is var firstSelectedIndex
				&& sortedListViewItems[firstSelectedIndex] is var firstSelectedItem
				&& GetListViewItemAppID(firstSelectedItem) is var selectedAppID)
			{
				Properties.Settings.Default.AppID = selectedAppID.Value;
			}
			else if (sortedListViewItems.Count > 0
				&& sortedListViewItems[0] is var firstItem
				&& GetListViewItemAppID(firstItem) is var firstAppID)
			{
				Properties.Settings.Default.AppID = firstAppID.Value;
			}
		}

		private void UpdateIconCache()
		{
			// NOTE: there's some games without one, and at least one which has a clienticns instead.
			var clientIconTypes = new Dictionary<string, string>
				{
					{ "clienticon", ".ico" },
					{ "clienticns", ".icns" },
				};

			var missingIcons = new List<IconCacheUpdateContext>();

			foreach (var kvp in appInfos.Items)
			{
				var appInfo = kvp.Value;

				foreach (var clientIconType in clientIconTypes)
				{
					if (appInfo.Data.Node.ContainsKey(clientIconType.Key)
						&& appInfo.Data.AppID is var appID)
					{
						var iconPath = Path.ChangeExtension(Path.Combine(SteamUtils.GetIconCachePath(), appInfo.Data.Node.GetString(clientIconType.Key)), clientIconType.Value);
						var url = $"https://steamcdn-a.akamaihd.net/steamcommunity/public/images/apps/{appID.ToString(CultureInfo.InvariantCulture)}/{appInfo.Data.Node.GetString(clientIconType.Key)}{clientIconType.Value}";

						if (!File.Exists(iconPath))
						{
							missingIcons.Add(new IconCacheUpdateContext
							{
								AppID = appID,
								Path = iconPath,
								Url = url,
								IconType = clientIconType.Key,
								IconExtension = clientIconType.Value,
							});
						}

						break;
					}
				}
			}

			// TODO: probably show this in a small progress tool window.

			if (missingIcons.Count == 0)
			{
				MessageBox.Show(this, "The cache is up to date already.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (MessageBox.Show(this, $"The cache is missing {missingIcons.Count} icons. Do you want to download them now?", "Confirmation Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
			{
				return;
			}

			var errorCount = 0;

			foreach (var context in missingIcons)
			{
				try
				{
					webClient.DownloadFile(context.Url, context.Path);

					if (File.Exists(context.Path))
					{
						assets[context.AppID].UserData.Icon = true;
					}
				}
				catch (Exception ex)
				{
					Program.LogError(ex);
					++errorCount;
				}
			}

			if (errorCount > 0)
			{
				var message = $"{errorCount} files could not be downloaded.";

				MessageBox.Show(this, message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Program.LogError(message);
			}
		}

		private void UpdateNonSteamAppIconCacheFromExecutablesOnly()
		{
			UpdateNonSteamAppIconCache(ShortcutIconProvider.Executable);
		}

		private void UpdateNonSteamAppIconCacheFromCustomIconsOnly()
		{
			UpdateNonSteamAppIconCache(ShortcutIconProvider.Custom);
		}

		private void UpdateNonSteamAppIconCache()
		{
			UpdateNonSteamAppIconCache(ShortcutIconProvider.Executable | ShortcutIconProvider.Custom);
		}

		[Flags]
		public enum ShortcutIconProvider
		{
			None = 0x00,
			Executable = 0x01,
			Custom = 0x02,
		}

		private static readonly string[] executableFileExtensions = new string[] { ".exe " };

		private void UpdateNonSteamAppIconCache(ShortcutIconProvider shortcutIconProvider)
		{
			if (string.IsNullOrEmpty(SteamUtils.GetNonSteamAppIconCachePath()))
			{
				if (MessageBox.Show(this, $"Non-steam app icons need to be cached somewhere. Do you want to set it now?", "Confirmation Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
				{
					return;
				}

				var path = PromptNonSteamAppIconCachePath();

				if (path is null)
				{
					return;
				}

				Properties.Settings.Default.NonSteamAppIconCachePath = path;
			}

			var missingIcons = new List<NonSteamAppIconCacheUpdateContext>();

			foreach (var kvp in shortcuts.Items)
			{
				var shortcut = kvp.Value;
				var appID = shortcut.Node.GetUInt("appid");

				if (appID is null
					|| appID == 0)
				{
					continue;
				}

				var hasAnyValidProvider = false;

				if (shortcutIconProvider.HasFlag(ShortcutIconProvider.Custom)
					&& !string.IsNullOrEmpty(Steam.Vdf.Shortcut.GetStringWithoutQuotes(shortcut.Node.GetString("icon") ?? shortcut.Node.GetString("Icon"))))
				{
					hasAnyValidProvider = true;
				}

				if (shortcutIconProvider.HasFlag(ShortcutIconProvider.Executable)
					&& !string.IsNullOrEmpty(Steam.Vdf.Shortcut.GetStringWithoutQuotes(shortcut.Node.GetString("exe") ?? shortcut.Node.GetString("Exe"))))
				{
					hasAnyValidProvider = true;
				}

				if (!hasAnyValidProvider)
				{
					continue;
				}

				var found = false;
				var validFileExtensions = (IEnumerable<string>)null;

				if (shortcutIconProvider == ShortcutIconProvider.Executable)
				{
					validFileExtensions = executableFileExtensions;
				}
				else
				{
					validFileExtensions = validUserIconAssetFileExtensions;
				}

				foreach (var supportedFileExtension in validFileExtensions)
				{
					/*
					if (!shortcutIconProvider.HasFlag(ShortcutIconProvider.Executable)
					//if (!Properties.Settings.Default.AllowProgramsAsIcon
						&& supportedFileExtension.Equals(".exe", StringComparison.InvariantCultureIgnoreCase))
					{
						continue;
					}
					*/
					//var iconPath = Path.ChangeExtension(Path.Combine(SteamUtils.GetNonSteamAppIconCachePath(), shortcut.AppID.Value.ToString(CultureInfo.InvariantCulture)), ".ico");
					var userIconTestFullName = Path.ChangeExtension(Path.Combine(SteamUtils.GetNonSteamAppIconCachePath(), appID.Value.ToString(CultureInfo.InvariantCulture)) + (Properties.Settings.Default.Asset_NonSteamAppIcon_NameSuffix ?? ""), supportedFileExtension);

					if (File.Exists(userIconTestFullName))
					{
						Program.LogError($"{kvp.Key}: icon already exists ({(shortcut.Node.GetString("appname") ?? shortcut.Node.GetString("AppName"))})");
						found = true;
						break;
					}
				}

				if (found)
				{
					continue;
				}

				try
				{
					Program.LogError($"{kvp.Key}: icon is missing ({(shortcut.Node.GetString("appname") ?? shortcut.Node.GetString("AppName"))})");

					var shortcutExecutable = Steam.Vdf.Shortcut.GetStringWithoutQuotes(shortcut.Node.GetString("exe") ?? shortcut.Node.GetString("Exe"));
					var shortcutIcon = Steam.Vdf.Shortcut.GetStringWithoutQuotes(shortcut.Node.GetString("icon") ?? shortcut.Node.GetString("Icon"));
					var userIconFullNameWithoutExtension = Path.Combine(SteamUtils.GetNonSteamAppIconCachePath(), appID.Value.ToString(CultureInfo.InvariantCulture) + (Properties.Settings.Default.Asset_NonSteamAppIcon_NameSuffix ?? ""));
					var originalAssetFullName = shortcutIconProvider == ShortcutIconProvider.Executable
						//&& Properties.Settings.Default.AllowProgramsAsIcon
						|| (string.IsNullOrEmpty(shortcutIcon)) ?
						shortcutExecutable : shortcutIcon;
					var originalAssetFileExtension = Path.GetExtension(originalAssetFullName);
					var userIconFullName = Path.ChangeExtension(userIconFullNameWithoutExtension, originalAssetFileExtension);

					missingIcons.Add(new NonSteamAppIconCacheUpdateContext
					{
						AppID = appID.Value,
						UserIconFullName = userIconFullName,
						OriginalAssetFullName = originalAssetFullName,
					});
				}
				catch (Exception ex)
				{
					Program.LogError(ex);
				}
			}

			// TODO: do this in a small tool popup window.
			if (missingIcons.Count == 0)
			{
				MessageBox.Show(this, "The cache is up to date already.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (MessageBox.Show(this, $"The cache is missing {missingIcons.Count} icons. Do you want to copy them now?", "Confirmation Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
			{
				return;
			}

			var errorCount = 0;

			foreach (var context in missingIcons)
			{
				if (ToggleCollection.Instance.IsOn("Log/Verbose"))
				{
					Console.WriteLine($"{context.AppID}: processing '{context.OriginalAssetFullName}' => '{context.UserIconFullName}'");
				}

				if (!File.Exists(context.OriginalAssetFullName))
				{
					Program.LogError($"Error: cannot find file for non-steam app icon '{context.OriginalAssetFullName}'.");
					++errorCount;
					continue;
				}

				try
				{
					if (!string.IsNullOrEmpty(Path.GetDirectoryName(context.UserIconFullName)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(context.UserIconFullName));
					}

					var originalAssetFilExtension = Path.GetExtension(context.OriginalAssetFullName);

					if (!Properties.Settings.Default.AllowProgramsAsIcon
						&& !string.IsNullOrEmpty(originalAssetFilExtension)
						&& originalAssetFilExtension.Equals(".exe", StringComparison.InvariantCultureIgnoreCase))
					{
						var userIconPngFullPath = Path.ChangeExtension(context.UserIconFullName, Path.GetExtension(".png"));

						if (ToggleCollection.Instance.IsOn("Log/Verbose"))
						{
							Console.WriteLine($"{context.AppID}: extracting icon from program '{context.OriginalAssetFullName}' to '{userIconPngFullPath}'");
						}

						IconExtractorUtils.ExtractAll(context.OriginalAssetFullName, userIconPngFullPath, ImageFormat.Png);
					}
					else
					{
						if (ToggleCollection.Instance.IsOn("Log/Verbose"))
						{
							Console.WriteLine($"{context.AppID}: copying '{context.OriginalAssetFullName}' to '{context.UserIconFullName}'");
						}

						File.Copy(context.OriginalAssetFullName, context.UserIconFullName, overwrite: true);
					}

					if (File.Exists(context.UserIconFullName))
					{
						assets[context.AppID].UserData.Icon = true;
					}
				}
				catch (Exception ex)
				{
					Program.LogError(ex);
					++errorCount;
				}
			}

			if (errorCount > 0)
			{
				var message = $"{errorCount} files could not be copied.";

				MessageBox.Show(this, message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Program.LogError(message);
			}
		}

		private void PopulateOverlaysMenu()
		{
			foreach (var overlay in assetOverlays.Items)
			{
				var menuItems = new List<ToolStripItem>();

				{
					var check = Check("");

					var menuItem = new ToolStripMenuItem("&None", image: null, OverlayToolStripMenuItem_Click)
					{
						Checked = check,
						Enabled = !check,
						Tag = "",
					};

					menuItems.Add(menuItem);

					if (overlay.Value.Count > 0)
					{
						menuItems.Add(new ToolStripSeparator());
					}
				}

				foreach (var item in overlay.Value)
				{
					var check = Check(item.Name);

					var menuItem = new ToolStripMenuItem($"&{item.Name}", image: null, OverlayToolStripMenuItem_Click)
					{
						Checked = check,
						Enabled = !check,
						Tag = item.Name,
					};

					menuItems.Add(menuItem);
				}

				foreach (ToolStripItem categoryItem in overlaysToolStripMenuItem.DropDownItems)
				{
					if (categoryItem is ToolStripMenuItem categoryMenuItem
						&& categoryMenuItem.Tag is AssetType assetType
						&& assetType == overlay.Key)
					{
						categoryMenuItem.DropDownItems.Clear();
						categoryMenuItem.DropDownItems.AddRange(menuItems.ToArray());
						break;
					}
				}

				bool Check(string name)
				{
					var check = false;

					switch (overlay.Key)
					{
						case AssetType.LibraryCapsule:
							check = name.Equals(Properties.Settings.Default.View_Overlay_LibraryCapsule, StringComparison.CurrentCultureIgnoreCase);
							break;
						case AssetType.HeroGraphic:
							check = name.Equals(Properties.Settings.Default.View_Overlay_HeroGraphic, StringComparison.CurrentCultureIgnoreCase);
							break;
						case AssetType.Logo:
							check = name.Equals(Properties.Settings.Default.View_Overlay_Logo, StringComparison.CurrentCultureIgnoreCase);
							break;
						case AssetType.Header:
							check = name.Equals(Properties.Settings.Default.View_Overlay_Header, StringComparison.CurrentCultureIgnoreCase);
							break;
						case AssetType.Icon:
							check = name.Equals(Properties.Settings.Default.View_Overlay_Icon, StringComparison.CurrentCultureIgnoreCase);
							break;
					}

					return check;
				}

				void OverlayToolStripMenuItem_Click(object sender, EventArgs e)
				{
					var toolStripMenuItem = (ToolStripMenuItem)sender;
					var name = (string)toolStripMenuItem.Tag;

					switch (overlay.Key)
					{
						case AssetType.LibraryCapsule:
							Properties.Settings.Default.View_Overlay_LibraryCapsule = name;
							break;
						case AssetType.HeroGraphic:
							Properties.Settings.Default.View_Overlay_HeroGraphic = name;
							break;
						case AssetType.Logo:
							Properties.Settings.Default.View_Overlay_Logo = name;
							break;
						case AssetType.Header:
							Properties.Settings.Default.View_Overlay_Header = name;
							break;
						case AssetType.Icon:
							Properties.Settings.Default.View_Overlay_Icon = name;
							break;
					}

					CheckOverlayMenuItemGroup(overlay.Key, name);
				}
			}
		}

		private void OnListLibraryAssetsStyleChanged()
		{
			appIDListView.BeginUpdate();

			foreach (ListViewItem listViewItem in sortedListViewItems)
			{
				foreach (ListViewItem.ListViewSubItem listViewSubItem in listViewItem.SubItems)
				{
					// TODO: do an additional check to make sure the column is a of a correct type?

					if (listViewSubItem.Tag is bool check)
					{
						listViewSubItem.Text = check ? Properties.Settings.Default.List_Assets_Exist_String : Properties.Settings.Default.List_Assets_Missing_String;
						listViewSubItem.ForeColor = check ? Properties.Settings.Default.List_Assets_Exist_Color : Properties.Settings.Default.List_Assets_Missing_Color;
					}
				}
			}

			appIDListView.EndUpdate();
		}

		private void OnDetailsViewChanged()
		{
			var showUserData = Properties.Settings.Default.Details_CacheLocation.HasFlag(CacheLocation.UserData);
			var showLibraryCache = Properties.Settings.Default.Details_CacheLocation.HasFlag(CacheLocation.LibraryCache);

			assetTableLayoutPanel.SuspendLayout();

			if (showUserData && showLibraryCache)
			{
				assetTableLayoutPanel.ColumnStyles[0].Width = 50f;
				assetTableLayoutPanel.ColumnStyles[1].Width = 50f;
			}
			else if (showUserData && !showLibraryCache)
			{
				assetTableLayoutPanel.ColumnStyles[0].Width = 100.0f;
				assetTableLayoutPanel.ColumnStyles[1].Width = 0.0f;
			}
			else if (!showUserData && showLibraryCache)
			{
				assetTableLayoutPanel.ColumnStyles[0].Width = 0.0f;
				assetTableLayoutPanel.ColumnStyles[1].Width = 100.0f;
			}
			else
			{
				assetTableLayoutPanel.ColumnStyles[0].Width = 0.0f;
				assetTableLayoutPanel.ColumnStyles[1].Width = 0.0f;
			}

			assetTableLayoutPanel.ResumeLayout(performLayout: true);

			userDataUserPictureBoxGroup.Visible = showUserData;
			libraryCacheUserPictureBoxGroup.Visible = showLibraryCache;

			userDataUserPictureBoxGroup.Asleep = !showUserData;
			libraryCacheUserPictureBoxGroup.Asleep = !showLibraryCache;
		}

		private void CheckOverlayMenuItemGroup(AssetType assetType, string name)
		{
			var categoryMenuItem = (ToolStripMenuItem)null;

			foreach (ToolStripMenuItem menuItem in overlaysToolStripMenuItem.DropDownItems)
			{
				if (menuItem.Tag is AssetType menuItemAssetType
					&& menuItemAssetType == assetType)
				{
					categoryMenuItem = menuItem;
					break;
				}
			}

			if (categoryMenuItem is null)
			{
				return;
			}

			foreach (ToolStripItem menuItem in categoryMenuItem.DropDownItems)
			{
				if (menuItem is ToolStripMenuItem overlayMenuItem
					&& overlayMenuItem.Tag is string overlayName)
				{
					var check = overlayName.Equals(name, StringComparison.CurrentCultureIgnoreCase);

					overlayMenuItem.Checked = check;
					overlayMenuItem.Enabled = !check;
				}
			}
		}


		private void LoadAppInfo()
		{
			var appInfosVdfPath = Path.Combine(Properties.Settings.Default.SteamAppPath, "appcache", "appinfo.vdf");

			if (!File.Exists(appInfosVdfPath))
			{
				Program.LogError("WARNING: no appinfo.vdf file found.");
			}
			else
			{
				try
				{
					appInfos.Load(appInfosVdfPath);

					if (ToggleCollection.Instance.IsOn("Log/Information"))
					{
						Console.WriteLine($"\tFound {appInfos.Items.Count} apps.");
					}
				}
				catch (Exception ex)
				{
					Program.LogError(ex);
				}
			}

			UpdateAppInfoDependentControls();
		}

		private void LoadShortcuts()
		{
			var shortcutsVdfPath = Path.Combine(Properties.Settings.Default.SteamAppPath, "userdata", Properties.Settings.Default.SteamUserID.ToString(CultureInfo.InvariantCulture), "config", "shortcuts.vdf");

			if (!File.Exists(shortcutsVdfPath))
			{
				Program.LogError("WARNING: no shortcuts.vdf file found.");
			}
			else
			{
				try
				{
					shortcuts.Load(shortcutsVdfPath);

					if (ToggleCollection.Instance.IsOn("Log/Information"))
					{
						Console.WriteLine($"\tFound {shortcuts.Items.Count} shortcuts.");
					}
				}
				catch (Exception ex)
				{
					Program.LogError(ex);
				}
			}

			UpdateShortcutsDependentControls();
		}

		#endregion

		private void appIDPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowProperties();
		}

		private void statusBarToolStripMenuItem_Click(object sender, EventArgs e)
		{
			listStatusStrip.Visible = !listStatusStrip.Visible;
		}

		private void mainStatusStrip_VisibleChanged(object sender, EventArgs e)
		{
			statusBarToolStripMenuItem.Checked = listStatusStrip.Visible;
		}

		private void listViewAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			viewListShowAllToolStripMenuItem.PerformClick();
		}

		private void listViewUserDataOnlyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			viewListUserDataToolStripMenuItem.PerformClick();
		}

		private void listViewLibraryCacheOnlyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			viewListLibraryCacheToolStripMenuItem.PerformClick();
		}

		private void listSortByAppIDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			appIDToolStripMenuItem.PerformClick();
		}

		private void listSortByTitleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			titleToolStripMenuItem.PerformClick();
		}

		private void listSortByUserDataLibraryCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			userDataLibraryCapsuleToolStripMenuItem.PerformClick();
		}

		private void listSortByUserDataHeroGraphicToolStripMenuItem_Click(object sender, EventArgs e)
		{
			userDataHeroGraphicToolStripMenuItem.PerformClick();
		}

		private void listSortByUserDataLogoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			userDataLogoToolStripMenuItem.PerformClick();
		}

		private void listSortByUserDataHeaderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			userDataHeaderToolStripMenuItem.PerformClick();
		}

		private void listSortByUserDataIconToolStripMenuItem_Click(object sender, EventArgs e)
		{
			userDataIconToolStripMenuItem.PerformClick();
		}

		private void listSortByLibraryCacheLibraryCapsuleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			libraryCacheLibraryCapsuleToolStripMenuItem.PerformClick();
		}

		private void listSortByLibraryCacheHeroGraphicToolStripMenuItem_Click(object sender, EventArgs e)
		{
			libraryCacheHeroGraphicToolStripMenuItem.PerformClick();
		}

		private void listSortByLibraryCacheLogoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			libraryCacheLogoToolStripMenuItem.PerformClick();
		}

		private void listSortByLibraryCacheHeaderToolStripMenuItem_Click(object sender, EventArgs e)
		{
			libraryCacheHeaderToolStripMenuItem.PerformClick();
		}

		private void listSortByLibraryCacheIconToolStripMenuItem_Click(object sender, EventArgs e)
		{
			libraryCacheIconToolStripMenuItem.PerformClick();
		}

		private void listSortOrderNoneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			noneToolStripMenuItem.PerformClick();
		}

		private void listSortOrderAscendingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ascendingToolStripMenuItem.PerformClick();
		}

		private void listSortOrderDescendingToolStripMenuItem_Click(object sender, EventArgs e)
		{
			descendingToolStripMenuItem.PerformClick();
		}

		private void toolsDatabaseLoadAppInfoAtStartToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.Database_AppInfo_LoadAtStart = !Properties.Settings.Default.Database_AppInfo_LoadAtStart;
		}

		private void toolsDatabaseLoadAppInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			appInfos.Clear();
			LoadAppInfo();
			CollectAppIDsAndPopulateListView();
			SelectFirstAppIDFromListView();
		}

		private void toolsDatabaseLoadShortcutsAtStartToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Properties.Settings.Default.Database_Shortcuts_LoadAtStart = !Properties.Settings.Default.Database_Shortcuts_LoadAtStart;
		}

		private void toolsDatabaseLoadShortcutsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			shortcuts.Clear();
			LoadShortcuts();
			CollectAppIDsAndPopulateListView();
			SelectFirstAppIDFromListView();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SelectAll();
		}

		private void exportAppInfoAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ExportAppInfoAs();
		}

		private void exportShortcutsAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ExportShortcutsAs();
		}

		private void focusOnPreviousToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FocusOnPreviousSelectedItem();
		}

		private void focusOnNextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FocusOnNextSelectedItem();
		}

		private void saveAppInfoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UpdateAppInfoVdf();
		}

		private void findToolStripMenuItem_Click(object sender, EventArgs e)
		{
			filterTextBox.Focus();
		}

		private void goToToolStripMenuItem_Click(object sender, EventArgs e)
		{
			appIDTextBox.Focus();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (appIDListView.SelectedIndices.Count > 0)
			{
				CopySelectedItems();
			}
			else
			{
				CopyAllItems();
			}
		}

		private void githubToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				using (var process = Process.Start("Explorer", "https://github.com/TakoNekko/SteamGridManager"))
				{
				}
			}
			catch (Exception ex)
			{
				Program.LogError(ex);
			}
		}

		private void ShowProperties()
		{
			if (!CanShowProperties())
			{
				return;
			}

			if (appIDListView.SelectedIndices[0] is var selectedIndex
				&& sortedListViewItems[selectedIndex] is var selectedItem
				&& GetListViewItemAppID(selectedItem) is var appID)
			{
				using (var dialog = new AppPropertiesDialog
				{
					Info = new AppPropertiesDialog.InfoBlock
					{
						AppInfos = appInfos,
						Shortcuts = shortcuts,
						AppID = appID.Value,
					},
				})
				{
					dialog.ShowDialog(this);

					// TODO: displayed properties may have changed, so the list view item may need to be updated too.
					RefreshListViewItem(selectedItem);
				}
			}
		}

		private bool CanShowProperties()
			=> appIDListView.SelectedIndices.Count == 1
				&& appIDListView.SelectedIndices[0] is var selectedIndex
				&& sortedListViewItems[selectedIndex] is var selectedItem
				&& GetListViewItemAppID(selectedItem) is var appID
				&& (appInfos.Items.ContainsKey(appID.Value)
				|| shortcuts.Items.ContainsKey(appID.Value));

		private void RefreshListViewItem(ListViewItem listViewItem)
		{
			// TODO: this won't do anything... unless I finally implement appIDListView_CacheVirtualItems.

			//appIDListView.RedrawItems(listViewItem.Index, listViewItem.Index, invalidateOnly: false);
		}

		private void UpdateListViewItemSelectionDependentItems()
		{
			runWithToolStripMenuItem.Enabled = CanRunWith();
			copyToolStripMenuItem.Enabled = CanCopy();
			selectAllToolStripMenuItem.Enabled = CanSelectAll();
			appIDPropertiesToolStripMenuItem.Enabled = CanShowProperties();
			focusOnPreviousToolStripMenuItem.Enabled = CanFocusOnPrevious();
			focusOnNextToolStripMenuItem.Enabled = CanFocusOnNext();
		}

		private bool CanSelectAll()
			=> appIDListView.SelectedIndices.Count != sortedListViewItems.Count;

		private void SelectAll()
		{
			// NOTE: SelectAll doesn't raise any events.
			appIDListView.SelectAll();
			OnSelectionChanged();
		}

		private void ExportAppInfoAs()
		{
			using (var dialog = new SaveFileDialog
			{
				Title = "Export AppInfo As",
				FileName = "appinfo",
				Filter = "Valve Formatted Text Files|*.acf;*.txt",
				AddExtension = true,
			})
			{
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					var sb = new StringBuilder();
					var acfWriter = new AcfWriter();

					try
					{

						Directory.CreateDirectory(Path.GetDirectoryName(dialog.FileName));

						foreach (var kvp in appInfos.Items)
						{
							var appInfo = kvp.Value;

							sb.AppendLine($"//\"{"AppID"}\" \"{appInfo.Data.AppID}\"");
							sb.AppendLine($"//\"{"ChangeNumber"}\" \"{appInfo.Data.ChangeNumber}\"");
							sb.AppendLine($"//\"{"Hash"}\" \"{appInfo.Data.Hash}\"");
							sb.AppendLine($"//\"{"InfoState"}\" \"{appInfo.Data.InfoState}\"");
							sb.AppendLine($"//\"{"LastUpdated"}\" \"{appInfo.Data.LastUpdated.ToRelativeTime().ToString(Properties.Settings.Default.VdfDefinition_RelativeTime_Format)}\"");
							sb.AppendLine($"//\"{"PicsToken"}\" \"{appInfo.Data.PicsToken}\"");
							sb.AppendLine($"//\"{"Size"}\" \"{appInfo.Data.Size}\"");

							acfWriter.Write(sb, appInfo.Data.Node);
						}

						File.WriteAllText(dialog.FileName, sb.ToString());
					}
					catch (Exception ex)
					{
						Program.LogError(ex);
					}
				}
			}
		}

		private void ExportShortcutsAs()
		{
			using (var dialog = new SaveFileDialog
			{
				Title = "Export Shortcuts As",
				FileName = "shortcuts",
				Filter = "Valve Formatted Text Files|*.acf;*.txt",
				AddExtension = true,
			})
			{
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					var sb = new StringBuilder();
					var acfWriter = new AcfWriter();

					try
					{
						acfWriter.Write(sb, shortcuts.Root);
						Directory.CreateDirectory(Path.GetDirectoryName(dialog.FileName));
						File.WriteAllText(dialog.FileName, sb.ToString());
					}
					catch (Exception ex)
					{
						Program.LogError(ex);
					}
				}
			}
		}

		private void UpdateAppInfoVdf()
		{
			var appInfosVdfFullName = Path.Combine(Properties.Settings.Default.SteamAppPath, "appcache", "appinfo.vdf");
			var appInfosVdfBackupFullName = Path.Combine(Properties.Settings.Default.BackupPath, "appcache", $"appinfo ({DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ff", CultureInfo.InvariantCulture)}).vdf.bk");

			try
			{
				if (!string.IsNullOrEmpty(Path.GetDirectoryName(appInfosVdfBackupFullName)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(appInfosVdfBackupFullName));
				}

				File.Copy(appInfosVdfFullName, appInfosVdfBackupFullName, overwrite: true);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ParentForm, ex.Message, "I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Program.LogError(ex);
				return;
			}

			using (var stream = File.OpenWrite(appInfosVdfFullName))
			{
				var vdfWriter = new VdfWriter(stream, leaveOpen: true);

				using (var binaryWriter = new BinaryWriter(stream, Encoding.Default, leaveOpen: true))
				{
					const uint signature = 0x07564427U;
					var universe = appInfos.Universe == (uint)Steam.Universe.Invalid
						? (uint)Steam.Universe.Public
						: appInfos.Universe;

					binaryWriter.Write(signature);
					binaryWriter.Write(universe);
				}

				foreach (var appInfo in appInfos.Items)
				{
					var data = appInfo.Value.Data;

					using (var binaryWriter = new BinaryWriter(stream, Encoding.Default, leaveOpen: true))
					{
						binaryWriter.Write((uint)data.AppID);
						binaryWriter.Write(data.Size);
						binaryWriter.Write(data.InfoState);
						binaryWriter.Write(data.LastUpdated);
						binaryWriter.Write(data.PicsToken);
						binaryWriter.Write(data.Hash.ToArray());
						binaryWriter.Write(data.ChangeNumber);
					}

					vdfWriter.Write(data.Node, writeDelimiters: false);
				}

				using (var binaryWriter = new BinaryWriter(stream, Encoding.Default, leaveOpen: true))
				{
					var appID = (uint)0;

					binaryWriter.Write(appID);

					// HACK
					binaryWriter.Write((byte)0);
				}
			}
		}

		private void FocusOnPreviousSelectedItem()
		{
			var focusedItemIndex = FindFocusedItemIndex();

			if (focusedItemIndex == -1)
			{
				return;
			}

			--focusedItemIndex;

			if (focusedItemIndex < 0)
			{
				// wrap around.
				focusedItemIndex = appIDListView.SelectedIndices.Count - 1;
			}

			var desiredItem = sortedListViewItems[appIDListView.SelectedIndices[focusedItemIndex]];

			desiredItem.EnsureVisible();

			focusedAppID = GetListViewItemAppID(desiredItem);
		}

		private void FocusOnNextSelectedItem()
		{
			var focusedItemIndex = FindFocusedItemIndex();

			if (focusedItemIndex == -1)
			{
				return;
			}

			++focusedItemIndex;

			if (focusedItemIndex > appIDListView.SelectedIndices.Count - 1)
			{
				// wrap around.
				focusedItemIndex = 0;
			}

			var desiredItem = sortedListViewItems[appIDListView.SelectedIndices[focusedItemIndex]];

			desiredItem.EnsureVisible();

			focusedAppID = GetListViewItemAppID(desiredItem);
		}

		private void FocusOnSelectedItem()
		{
			var focusedItemIndex = FindFocusedItemIndex();

			if (focusedItemIndex == -1)
			{
				return;
			}

			var desiredItem = sortedListViewItems[appIDListView.SelectedIndices[focusedItemIndex]];

			desiredItem.EnsureVisible();

			focusedAppID = GetListViewItemAppID(desiredItem);
		}

		private int FindFocusedItemIndex()
		{
			for (var i = 0; i < appIDListView.SelectedIndices.Count; ++i)
			{
				var selectedItem = sortedListViewItems[appIDListView.SelectedIndices[i]];
				var appID = GetListViewItemAppID(selectedItem);

				if (appID == focusedAppID)
				{
					return i;
				}
			}

			return -1;
		}

		private ulong? GetListViewItemAppID(ListViewItem item)
		{
			if (item.Tag is AppInfo appInfo)
			{
				return appInfo.Data.AppID;
			}
			else if (item.Tag is Steam.Vdf.Shortcut shortcut)
			{
				return shortcut.Node.GetUInt("appid");
			}
			else if (item.Tag is ulong appID)
			{
				return appID;
			}

			return null;
		}

		private bool CanFocusOnPrevious()
			=> appIDListView.SelectedIndices.Count > 0;

		private bool CanFocusOnNext()
			=> appIDListView.SelectedIndices.Count > 0;

		private void CopySelectedItems()
		{
			var selectedItems = new List<ListViewItem>();

			foreach (int index in appIDListView.SelectedIndices)
			{
				selectedItems.Add(sortedListViewItems[index]);
			}

			Copy(selectedItems);
		}

		private void CopyAllItems()
		{
			Copy(sortedListViewItems);
		}

		private void Copy(IEnumerable<ListViewItem> items)
		{
			var stringBuilder = new StringBuilder();
			var count = appIDListView.Columns.Count;

			foreach (var item in items)
			{
				for (var i = 0; i < count; ++i)
				{
					var subItem = item.SubItems[appIDListView.Columns[i].DisplayIndex];

					stringBuilder.Append(subItem.Text);

					if (i < count - 1)
					{
						stringBuilder.Append("\t");
					}
				}

				if (count > 1)
				{
					stringBuilder.AppendLine();
				}
			}

			Clipboard.SetText(stringBuilder.ToString());
		}

		private bool CanCopy()
			=> sortedListViewItems.Count > 0;


		#region External Applications

		private bool CanRunWith()
			=> appIDListView.SelectedIndices.Count == 1;

		private void Items_AddingNew(object sender, AddingNewEventArgs e)
		{
			PopulateExternalApplicationsSubMenu();
		}

		private void Items_ListChanged(object sender, ListChangedEventArgs e)
		{
			var refresh = false;

			switch (e.ListChangedType)
			{
				case ListChangedType.ItemMoved:
				case ListChangedType.ItemDeleted:
				case ListChangedType.ItemAdded:
					refresh = true;
					break;
				case ListChangedType.ItemChanged:
					switch (e.PropertyDescriptor.Name)
					{
						case nameof(ExternalApplication.Title):
							refresh = true;
							break;
					}
					break;
			}

			if (refresh)
			{
				PopulateExternalApplicationsSubMenu();
			}
		}

		public ExternalApplicationCollection ExternalApplications
			=> ExternalApplicationCollection.Instance;

		private void ExternalApplications_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(ExternalApplicationCollection.Items):
					PopulateExternalApplicationsSubMenu();
					break;
			}
		}

		private void PopulateExternalApplicationsSubMenu()
		{
			PopulateExternalApplicationsSubMenu(runWithToolStripMenuItem, "Run");
		}

		private void PopulateExternalApplicationsSubMenu(ToolStripMenuItem targetMenu, string verb)
		{
			ExternalApplicationUtils.PopulateExternalApplicationsSubMenu(ExternalApplications.Items, targetMenu, verb, StartExternalApplication);
		}

		private void StartExternalApplication(ExternalApplication application)
		{
			var selectedAppIDs = GetSelectedAppIDs();

			application?.Start(appInfos, shortcuts, selectedAppIDs[0], path: null, assetType: null);
		}

		#endregion
	}
}
