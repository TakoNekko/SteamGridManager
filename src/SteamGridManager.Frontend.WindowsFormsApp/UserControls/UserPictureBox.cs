using Steam.Vdf;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Label = System.Windows.Forms.Label;

namespace SteamGridManager.Frontend.WindowsFormsApp.UserControls
{
	using Extensions.Control;
	using Extensions.ExternalApplication;
	using Helpers;

	public partial class UserPictureBox : UserControl, INotifyPropertyChanged
	{
		#region Events

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		#endregion

		#region Types

		public class DownloadContext
		{
			//public string TemporaryFileName { get; set; }
			public bool HideProgress { get; set; }
		}

		#endregion

		#region Fields

		private readonly bool designMode;

		//private PictureBox backgroundWorkerPictureBox;

		#endregion

		#region Getters

		public GroupBox GroupBox => groupBox;
		public PictureBox PictureBox => pictureBox;
		public Label Placeholder => placeholderLabel;

		#endregion

		#region Properties

		public AppInfos AppInfos { get; set; }
		public Shortcuts Shortcuts { get; set; }


		private AssetFileWatcherSystem assetFileWatcherSystem;

		public AssetFileWatcherSystem AssetFileWatcherSystem
		{
			get => assetFileWatcherSystem;
			set
			{
				if (assetFileWatcherSystem != value)
				{
					if (assetFileWatcherSystem != null)
					{
						assetFileWatcherSystem.LibraryCache.Changed -= LibraryCache_Changed;
						assetFileWatcherSystem.Grids.Changed -= Grids_Changed;
						assetFileWatcherSystem.CachedIcons.Changed -= CachedIcons_Changed;
						assetFileWatcherSystem.NonSteamAppsCachedIcons.Changed -= NonSteamAppsCachedIcons_Changed;
					}

					assetFileWatcherSystem = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(AssetFileWatcherSystem)));

					AssetFileWatcherSystem.LibraryCache.Changed += LibraryCache_Changed;
					AssetFileWatcherSystem.Grids.Changed += Grids_Changed;
					AssetFileWatcherSystem.CachedIcons.Changed += CachedIcons_Changed;
					AssetFileWatcherSystem.NonSteamAppsCachedIcons.Changed += NonSteamAppsCachedIcons_Changed;
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
					//if (!designMode)
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
					//if (!designMode)
					{
						OnAssetOverlaysChanged();
					}
				}
			}
		}

		private AssetType assetType;

		public AssetType AssetType
		{
			get => assetType;
			set
			{
				if (assetType != value)
				{
					assetType = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(AssetType)));
					//if (!designMode)
					{
						OnAssetTypeChanged();
					}
				}
			}
		}

		private string resolvedPath;

		public string ResolvedPath
		{
			get => resolvedPath;
			set
			{
				if (resolvedPath != value)
				{
					resolvedPath = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ResolvedPath)));
					if (!designMode)
					{
						OnResolvedPathChanged();
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

		#endregion

		#region Constructors

		private bool isDragging;

		public UserPictureBox()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				this.SetDoubleBuffered(true);

				Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;

				placeholderLabel.Visible = true;
				errorLabel.Visible = false;
				pictureBox.Visible = false;

				UpdateClipboarDependentControls();

				if (CacheLocation != CacheLocation.None
					&& AssetType != AssetType.None)
				{
					innerPanel.BackColor = (Color)Properties.Settings.Default[$"Details_{CacheLocation}_{AssetType}_BackColor"];
				}

				//ExternalApplications.PropertyChanged += ExternalApplications_PropertyChanged;
				ExternalApplications.Items.ListChanged += Items_ListChanged;
				ExternalApplications.Items.AddingNew += Items_AddingNew;
				//PopulateExternalApplicationsSubMenu();

				// NOTE: feature currently disabled.
				//pictureBox.MouseHover += InnerPanel_MouseHover;
				//pictureBox.MouseLeave += InnerPanel_MouseLeave;
			}
		}

		private void NonSteamAppsCachedIcons_Changed(object sender, FileSystemEventArgs e)
		{
			BeginInvoke(new Action(() =>
			{
				if (CacheLocation != CacheLocation.UserData
					|| AssetType != AssetType.Icon)
				{
					return;
				}

				if (ToggleCollection.Instance.IsOn("Log/FileSystemWatch"))
				{
					Console.WriteLine($"NonSteamAppsCachedIcons: ChangeType={e.ChangeType} Name={e.Name} FullPath={e.FullPath}");
				}

				ProcessFileChange(e);
			}));
		}

		private void CachedIcons_Changed(object sender, FileSystemEventArgs e)
		{
			BeginInvoke(new Action(() =>
			{
				if (CacheLocation != CacheLocation.LibraryCache
					|| AssetType != AssetType.Icon)
				{
					return;
				}

				if (ToggleCollection.Instance.IsOn("Log/FileSystemWatch"))
				{
					Console.WriteLine($"CachedIcons: ChangeType={e.ChangeType} Name={e.Name} FullPath={e.FullPath}");
				}

				ProcessFileChange(e);
			}));
		}

		private void Grids_Changed(object sender, FileSystemEventArgs e)
		{
			BeginInvoke(new Action(() =>
			{
				if (CacheLocation != CacheLocation.UserData
					/*|| AssetType == AssetType.Icon*/)	//  user is allowed to set the path for non-steam app icons to the same path used for grids.
				{
					return;
				}

				if (ToggleCollection.Instance.IsOn("Log/FileSystemWatch"))
				{
					Console.WriteLine($"Grids: ChangeType={e.ChangeType} Name={e.Name} FullPath={e.FullPath}");
				}

				var fileTitle = Path.GetFileNameWithoutExtension(e.Name);
				var match = Regex.Match(fileTitle, @"(\d+)([\w_]*)");

				if (!match.Success)
				{
					return;
				}

				switch (match.Groups[2].Value)
				{
					case "p":
						if (AssetType != AssetType.LibraryCapsule)
							return;
						break;
					case "_hero":
						if (AssetType != AssetType.HeroGraphic)
							return;
						break;
					case "_logo":
						if (AssetType != AssetType.Logo)
							return;
						break;
					case "":
					case null:
						if (AssetType != AssetType.Header)
							return;
						break;
					case "i":
						if (AssetType != AssetType.Icon)
							return;
						break;
				}

				ProcessFileChange(e);
			}));
		}

		private void LibraryCache_Changed(object sender, FileSystemEventArgs e)
		{
			BeginInvoke(new Action(() =>
			{
				if (CacheLocation != CacheLocation.LibraryCache
					|| AssetType == AssetType.Icon)
				{
					return;
				}

				if (ToggleCollection.Instance.IsOn("Log/FileSystemWatch"))
				{
					Console.WriteLine($"LibraryCache: ChangeType={e.ChangeType} Name={e.Name} FullPath={e.FullPath}");
				}

				var fileTitle = Path.GetFileNameWithoutExtension(e.Name);
				var match = Regex.Match(fileTitle, @"(\d+)([\w_]*)");

				if (!match.Success)
				{
					return;
				}

				switch (match.Groups[2].Value)
				{
					case "_library_600x900":
						if (AssetType != AssetType.LibraryCapsule)
							return;
						break;
					case "_library_hero":
						if (AssetType != AssetType.HeroGraphic)
							return;
						break;
					case "_logo":
						if (AssetType != AssetType.Logo)
							return;
						break;
					case "_header":
						if (AssetType != AssetType.Header)
							return;
						break;
					case "_icon":
						if (AssetType != AssetType.Icon)
							return;
						break;
				}

				ProcessFileChange(e);
			}));
		}

		private void ProcessFileChange(FileSystemEventArgs e)
		{
			switch (e.ChangeType)
			{
				case WatcherChangeTypes.Created:
				case WatcherChangeTypes.Changed:
					if (AreFilePathsEqual(e.FullPath, ResolvedPath))
					{
						resolvedPath = null;
						ResolvedPath = e.FullPath;
					}
					break;
				case WatcherChangeTypes.Deleted:
					if (AreFilePathsEqual(e.FullPath, ResolvedPath))
					{
						Clear();
					}
					break;
				case WatcherChangeTypes.Renamed:
					if (AreFilePathsEqual(e.FullPath, ResolvedPath)
						&& !File.Exists(ResolvedPath))
					{
						Clear();
					}
					break;
			}
		}

		private static bool AreFilePathsEqual(string x, string y)
			// TODO: think of a more robust way to compare file paths.
			=> x != null
				&& x.Equals(y, StringComparison.CurrentCultureIgnoreCase);

		private void PictureBox_MouseLeave(object sender, EventArgs e)
		{
			switch (Properties.Settings.Default.Details_Asset_BorderActivation)
			{
				case AssetBorder.MouseOverOnly:
					pictureBox.BorderStyle = BorderStyle.None;
					break;
			}
		}

		private void PictureBox_MouseHover(object sender, EventArgs e)
		{
			switch (Properties.Settings.Default.Details_Asset_BorderActivation)
			{
				case AssetBorder.MouseOverOnly:
					pictureBox.BorderStyle = BorderStyle.FixedSingle;
					break;
			}
		}

		#endregion

		#region Property Changes

		protected virtual void OnCacheLocationChanged()
		{
			//resolvedPath = null;
			ResolvedPath = ResolveAssetFullPath();

			UpdateCacheLocationDepedentControls();
			UpdateClipboarDependentControls();
			UpdateNonSteamAppUserIconMenu();
		}

		protected virtual void OnAssetOverlaysChanged()
		{
			OnOverlayChanged();
		}

		protected virtual void OnOverlayChanged()
		{
			var width = pictureBox.BackgroundImage is null ? 0 : pictureBox.BackgroundImage.Width;
			var height = pictureBox.BackgroundImage is null ? 0 : pictureBox.BackgroundImage.Height;

			switch (assetType)
			{
				case AssetType.LibraryCapsule:
					AssetOverlays?.GetAsync(AssetType.LibraryCapsule, Properties.Settings.Default.View_Overlay_LibraryCapsule, width, height, SetOverlayImage);
					break;
				case AssetType.HeroGraphic:
					AssetOverlays?.GetAsync(AssetType.HeroGraphic, Properties.Settings.Default.View_Overlay_HeroGraphic, width, height, SetOverlayImage);
					break;
				case AssetType.Logo:
					AssetOverlays?.GetAsync(AssetType.Logo, Properties.Settings.Default.View_Overlay_Logo, width, height, SetOverlayImage);
					break;
				case AssetType.Header:
					AssetOverlays?.GetAsync(AssetType.HeroGraphic, Properties.Settings.Default.View_Overlay_Header, width, height, SetOverlayImage);
					break;
				case AssetType.Icon:
					AssetOverlays?.GetAsync(AssetType.Icon, Properties.Settings.Default.View_Overlay_Icon, width, height, SetOverlayImage);
					break;
			}
		}

		protected virtual void OnAssetTypeChanged()
		{
			switch (AssetType)
			{
				case AssetType.LibraryCapsule:
					groupBox.Text = "Library Capsule";
					break;
				case AssetType.HeroGraphic:
					groupBox.Text = "Hero Graphic";
					break;
				case AssetType.Logo:
					groupBox.Text = "Logo";
					break;
				case AssetType.Header:
					groupBox.Text = "Header";
					break;
				case AssetType.Icon:
					groupBox.Text = "Icon";
					break;
			}

			UpdateNonSteamAppUserIconMenu();
			OnOverlayChanged();
		}

		protected virtual void OnResolvedPathChanged()
		{
			if (ResolvedPath is null)
			{
				Clear();
				return;
			}

			if (Properties.Settings.Default.Details_WatchFileChangesSimple)
			{
				try
				{
					fileSystemWatcher.Path = Path.GetDirectoryName(ResolvedPath);
					fileSystemWatcher.Filter = Path.ChangeExtension(Path.GetFileName(ResolvedPath), ".*");
					fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
					fileSystemWatcher.EnableRaisingEvents = true;
				}
				catch (Exception ex)
				{
					Program.LogError(ex);
				}

				fileSystemTimer.Enabled = true;
			}

			viewInExplorerToolStripMenuItem.Enabled = true;
			removeToolStripMenuItem.Enabled = true;

			previewToolStripMenuItem.Enabled = false;

			openWithToolStripMenuItem.Enabled = true;
			editWithToolStripMenuItem.Enabled = true;
			//useWithToolStripMenuItem.Enabled = true;

			copyToolStripMenuItem.Enabled = true;

			var isApplicableNonSteamAppIcon =
				CacheLocation == CacheLocation.UserData
				&& AssetType == AssetType.Icon
				&& Shortcuts != null
				&& Shortcuts.Items.ContainsKey(Properties.Settings.Default.AppID)
				&& Shortcuts.Items[Properties.Settings.Default.AppID].Node.GetString("icon") is string shortcutIcon
				&& !Steam.Vdf.Shortcut.GetStringWithoutQuotes(shortcutIcon).Equals(ResolvedPath);

			setAsCustomIconToolStripMenuItem.Enabled = isApplicableNonSteamAppIcon;

			placeholderLabel.Visible = false;
			errorLabel.Visible = false;
			pictureBox.Visible = false;
			pictureBox.BackgroundImage = null;

			progressBar.Value = 0;

			try
			{
				// TODO: don't rely on file extension but scan the file header instead?

				var fileExt = Path.GetExtension(ResolvedPath);

				if (fileExt.Equals(".tga", StringComparison.InvariantCultureIgnoreCase))
				{
					pictureBox.BackgroundImage = Paloma.TargaImage.LoadTargaImage(ResolvedPath);
				}
				else if (fileExt.Equals(".exe", StringComparison.InvariantCultureIgnoreCase))
				{
					var extractor = new TsudaKageyu.IconExtractor(ResolvedPath);
					var allIcons = extractor.GetAllIcons();
					var largestIcon = (Icon)null;

					foreach (var icon in allIcons)
					{
						var bestIcon = (Icon)null;

						if (Properties.Settings.Default.IconConversion_UseClosest)
						{
							bestIcon = new Icon(icon, 256, 256);
						}
						else
						{
							bestIcon = icon;
						}

						if (largestIcon is null
							|| largestIcon.Size.Width < bestIcon.Size.Width
							|| largestIcon.Size.Height < bestIcon.Size.Height)
						{
							largestIcon = bestIcon;
						}
						else
						{
							//icon.Dispose();
						}
					}

					if (largestIcon != null)
					{
						using (var memoryStream = new MemoryStream())
						{
							try
							{
								largestIcon.ToVistaBitmap().Save(memoryStream, ImageFormat.Png);
								if (ToggleCollection.Instance.IsOn("Log/Verbose"))
								{
									Console.WriteLine("Converted from 256x256 icon entry.");
								}
							}
							catch
							{
								largestIcon.Save(memoryStream);
								if (ToggleCollection.Instance.IsOn("Log/Verbose"))
								{
									Console.WriteLine($"Fallback to {largestIcon.Width}x{largestIcon.Height}.");
								}
							}

							pictureBox.BackgroundImage = new Bitmap(memoryStream);
						}
					}

					foreach (var icon in allIcons)
					{
						icon.Dispose();
					}
				}
				else
				{
					//backgroundWorkerPictureBox?.Dispose();

					// NOTE: work-around because PictureBox keeps a lock on the file until the control itself is disposed of (instead of disposing it as soon as it's loaded)
					var backgroundWorkerPictureBox = new PictureBox
					{
						WaitOnLoad = true
					};
					backgroundWorkerPictureBox.LoadCompleted += backgroundWorkerPictureBox_LoadCompleted;
					backgroundWorkerPictureBox.LoadProgressChanged += backgroundWorkerPictureBox_LoadProgressChanged;

					var downloadContext = new DownloadContext();

					try
					{
						if (new FileInfo(ResolvedPath).Length < Properties.Settings.Default.Details_Progress_MinFileSizeThreshold)
						{
							downloadContext.HideProgress = true;
							progressBar.Tag = downloadContext;
						}
					}
					catch { }

					if (ToggleCollection.Instance.IsOn("Log/Verbose"))
					{
						Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: starting async load '{resolvedPath}'...");
					}

					try
					{
						backgroundWorkerPictureBox.LoadAsync(ResolvedPath);
					}
					catch (Exception ex)
					{
						progressBar.Visible = false;
						pictureBox.Visible = false;
						errorLabel.Visible = true;

						Program.LogError($"Could not load image file {ResolvedPath}.");

						if (ToggleCollection.Instance.IsOn("Log/Verbose"))
						{
							Program.LogError(ex);
						}
					}
				}
			}
			catch (Exception ex)
			{
				progressBar.Visible = false;
				pictureBox.Visible = false;
				errorLabel.Visible = true;
				Program.LogError(ex);
			}
		}

		public void Clear()
		{
			viewInExplorerToolStripMenuItem.Enabled = false;
			removeToolStripMenuItem.Enabled = false;

			previewToolStripMenuItem.Enabled = false;

			openWithToolStripMenuItem.Enabled = false;
			editWithToolStripMenuItem.Enabled = false;
			//useWithToolStripMenuItem.Enabled = false;

			copyToolStripMenuItem.Enabled = false;

			setAsCustomIconToolStripMenuItem.Enabled = false;

			placeholderLabel.Visible = true;
			errorLabel.Visible = false;
			pictureBox.Visible = false;
			pictureBox.BackgroundImage = null;

			if (Properties.Settings.Default.Details_WatchFileChangesSimple)
			{
				try
				{
					fileSystemWatcher.EnableRaisingEvents = false;
				}
				catch (Exception ex)
				{
					Program.LogError(ex);
				}

				fileSystemTimer.Enabled = false;
			}
		}

		protected virtual void OnAsleepChanged()
		{
			if (Asleep)
			{
				// force unload
				//ResolvedPath = null;
			}
			else
			{
				// force reload
				var savedValue = resolvedPath;
				resolvedPath = null;
				ResolvedPath = savedValue;
			}
		}

		#endregion

		#region Event Handlers

		#region Settings

		private void Default_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals($"Details_{CacheLocation}_{AssetType}_BackColor", StringComparison.InvariantCultureIgnoreCase))
			{
				innerPanel.BackColor = (Color)Properties.Settings.Default[$"Details_{CacheLocation}_{AssetType}_BackColor"];
				return;
			}

			if (!Properties.Settings.Default.IsReady)
			{
				return;
			}

			switch (e.PropertyName)
			{
				case nameof(Properties.Settings.Default.SteamAppPath):
					if (Asleep)
					{
						resolvedPath = ResolveAssetFullPath();
						return;
					}
					try
					{
						ResolvedPath = ResolveAssetFullPath();
					}
					catch (Exception ex)
					{
						Program.LogError(ex);
					}
					break;
				case nameof(Properties.Settings.Default.SteamUserID):
					if (Asleep)
					{
						resolvedPath = ResolveAssetFullPath();
						return;
					}
					ResolvedPath = ResolveAssetFullPath();
					break;
				case nameof(Properties.Settings.Default.AppID):
					if (Asleep)
					{
						resolvedPath = ResolveAssetFullPath();
						UpdateNonSteamAppUserIconMenu();
						return;
					}
					ResolvedPath = ResolveAssetFullPath();
					UpdateNonSteamAppUserIconMenu();
					// TODO: thread-safety.
					//backgroundWorkerPictureBox?.CancelAsync();
					break;
				case nameof(Properties.Settings.Default.List_CacheLocation):
					/*
					ResolvedPath = ResolveAssetFullPath();
					{
						var isAssetModdable = Properties.Settings.Default.CacheLocation == CacheLocation.UserData;

						fromFileToolStripMenuItem.Visible = isAssetModdable;
						fromURLToolStripMenuItem.Visible = isAssetModdable;
						toolStripSeparator1.Visible = isAssetModdable;
						toolStripSeparator2.Visible = isAssetModdable;
						removeToolStripMenuItem.Visible = isAssetModdable;
					}
					*/
					break;
				case nameof(Properties.Settings.Default.View_Overlay_LibraryCapsule):
				case nameof(Properties.Settings.Default.View_Overlay_HeroGraphic):
				case nameof(Properties.Settings.Default.View_Overlay_Logo):
				case nameof(Properties.Settings.Default.View_Overlay_Header):
				case nameof(Properties.Settings.Default.View_Overlay_Icon):
					OnOverlayChanged();
					break;
				case nameof(Properties.Settings.Default.Details_Asset_Preview_BackColor_ShowMenu):
					toolStripSeparator3.Visible = !Properties.Settings.Default.Details_Asset_Preview_BackColor_ShowMenu;
					setAsCustomIconToolStripMenuItem.Visible = !Properties.Settings.Default.Details_Asset_Preview_BackColor_ShowMenu;
					break;
				case nameof(Properties.Settings.Default.Details_Asset_BorderActivation):
					switch (Properties.Settings.Default.Details_Asset_BorderActivation)
					{
						case AssetBorder.Always:
							innerPanel.BorderStyle = BorderStyle.FixedSingle;
							break;
						default:
							innerPanel.BorderStyle = BorderStyle.None;
							break;
					}
					break;
			}
		}

		#endregion

		#region Picture box

		private void pictureBox_BackgroundImageChanged(object sender, EventArgs e)
		{
			if (pictureBox.BackgroundImage is null)
			{
				return;
			}

			progressBar.Visible = false;
			placeholderLabel.Visible = false;
			errorLabel.Visible = false;
			pictureBox.Visible = true;

			previewToolStripMenuItem.Enabled = true;
			/*
			var isAssetModdable = CacheLocation == CacheLocation.UserData;

			innerPanel.AllowDrop = isAssetModdable;
			fromFileToolStripMenuItem.Visible = isAssetModdable;
			fromURLToolStripMenuItem.Visible = isAssetModdable;
			setFromClipboardToolStripMenuItem.Visible = isAssetModdable;
			toolStripSeparator1.Visible = isAssetModdable;
			toolStripSeparator2.Visible = isAssetModdable;
			removeToolStripMenuItem.Visible = isAssetModdable;
			*/
			OnOverlayChanged();
		}

		private void pictureBox_MouseClick(object sender, MouseEventArgs e)
		{
			innerPanel_MouseClick(innerPanel, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
		}

		private void pictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			innerPanel_MouseDoubleClick(innerPanel, new MouseEventArgs(e.Button, e.Clicks, e.X, e.Y, e.Delta));
		}

		private void pictureBox_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (e.EscapePressed)
			{
				isDragging = false;
				e.Action = DragAction.Cancel;
			}
			else if (e.Action == DragAction.Drop)
			{
				isDragging = false;
			}
		}

		private void pictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (!isDragging)
				{
					isDragging = true;
					pictureBox.DoDragDrop(GetCopy(), DragDropEffects.All);
				}
			}
		}

		#endregion

		#region Background Worker Picture Box

		private void backgroundWorkerPictureBox_LoadCompleted(object sender, AsyncCompletedEventArgs e)
		{
			var backgroundWorkerPictureBox = (PictureBox)sender;

			if (ToggleCollection.Instance.IsOn("Log/Verbose"))
			{
				Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: image load context is {ResolvedPath}");
			}

			if (e.Error != null)
			{
				if (ToggleCollection.Instance.IsOn("Log/Verbose"))
				{
					Program.LogError($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: image load error.");
				}
				Program.LogError(e.Error);

				backgroundWorkerPictureBox.Dispose();
				//this.backgroundWorkerPictureBox = null;

				return;
			}
			else if (e.Cancelled)
			{
				Program.LogError($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: image load cancelled.");

				backgroundWorkerPictureBox.Dispose();
				//this.backgroundWorkerPictureBox = null;

				return;
			}

			if (ToggleCollection.Instance.IsOn("Log/Verbose"))
			{
				Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: image loaded.");
			}

			// ditch the file lock.
			pictureBox.BackgroundImage = new Bitmap(backgroundWorkerPictureBox.Image);

			backgroundWorkerPictureBox.Dispose();
			//this.backgroundWorkerPictureBox = null;
		}

		private void backgroundWorkerPictureBox_LoadProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			if (progressBar.Tag is DownloadContext downloadContext)
			{
				if (downloadContext.HideProgress)
				{
					return;
				}
			}

			var show = progressBar.Value == 0;

			progressBar.Value = e.ProgressPercentage;
			if (show)
			{
				progressBar.Visible = true;
			}
		}

		#endregion

		#region Placeholder Label

		private void placeholderLabel_MouseClick(object sender, MouseEventArgs e)
		{
			//PerformNoAssetAction();
		}

		private void placeholderLabel_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			PerformNoAssetAction();
		}

		#endregion

		#region Inner Panel

		private void innerPanel_DragEnter(object sender, DragEventArgs e)
		{
			if (!Try())
			{
				innerPanel.BackColor = Properties.Settings.Default.Details_DragAndDrop_InvalidDrop_Color;

				e.Effect = DragDropEffects.None;
				return;
			}

			innerPanel.BackColor = Properties.Settings.Default.Details_DragAndDrop_ValidDrop_Color;

			e.Effect = DragDropEffects.Link;

			bool Try()
			{
				var validExtensions = (string[])null;

				switch (CacheLocation)
				{
					case CacheLocation.LibraryCache:
						validExtensions = validLibraryAssetFileExtensions;
						break;
					case CacheLocation.UserData:
						if (AssetType == AssetType.Icon)
						{
							validExtensions = validIconAssetFileExtensions;
						}
						else
						{
							validExtensions = validLibraryAssetFileExtensions;
						}
						break;
					default:
						return false;
				}

				if (validExtensions != null)
				{
					if (e.Data.GetDataPresent(DataFormats.FileDrop))
					{
						var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

						foreach (var fileName in fileNames)
						{
							if (!string.IsNullOrEmpty(fileName)
								&& Path.GetExtension(fileName) is string fileExtension
								&& !string.IsNullOrEmpty(fileExtension)
								&& validExtensions.Contains(fileExtension, StringComparer.CurrentCultureIgnoreCase))
							{
								return true;
							}
						}
					}
					else if (e.Data.GetDataPresent("UniformResourceLocatorW")
						&& e.Data.GetData("UniformResourceLocatorW", autoConvert: false) is MemoryStream memoryStream
						&& memoryStream.Length > 2)
					{
						using (var binaryReader = new BinaryReader(memoryStream))
						{
							var url = Encoding.Unicode.GetString(binaryReader.ReadBytes((int)(memoryStream.Length - 2)));

							if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
							{
								Program.LogError($"{url} is not a valid URL.");
								return false;
							}

							if (Path.GetExtension(uri.LocalPath) is var fileExtension
								&& !string.IsNullOrEmpty(fileExtension)
								&& validExtensions.Contains(fileExtension, StringComparer.CurrentCultureIgnoreCase))
							{
								return true;
							}

							return false;
						}
					}
				}

				return false;
			}
		}

		private void innerPanel_DragOver(object sender, DragEventArgs e)
		{
		}

		private void innerPanel_DragLeave(object sender, EventArgs e)
		{
			innerPanel.BackColor = (Color)Properties.Settings.Default[$"Details_{CacheLocation}_{AssetType}_BackColor"];
		}

		private void innerPanel_DragDrop(object sender, DragEventArgs e)
		{
			innerPanel.BackColor = (Color)Properties.Settings.Default[$"Details_{CacheLocation}_{AssetType}_BackColor"];

			if (e.AllowedEffect == DragDropEffects.None)
			{
				return;
			}

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

				foreach (var fileName in fileNames)
				{
					if (string.IsNullOrEmpty(fileName))
					{
						continue;
					}

					if (!ConfirmAddOrReplaceAsset(fileName))
					{
						return;
					}

					if (!string.IsNullOrEmpty(ResolvedPath)
						&& File.Exists(ResolvedPath))
					{
						if (ToggleCollection.Instance.IsOn("Log/Verbose"))
						{
							Console.WriteLine($"removing existing user asset {ResolvedPath}...");
						}

						var wasWatching = fileSystemWatcher.EnableRaisingEvents;

						if (Properties.Settings.Default.Details_WatchFileChangesSimple)
						{
							try
							{
								fileSystemWatcher.EnableRaisingEvents = false;
							}
							catch (Exception ex)
							{
								Program.LogError(ex);
							}
							fileSystemTimer.Enabled = false;
						}

						MoveAssetToRecycleBin();

						if (Properties.Settings.Default.Details_WatchFileChangesSimple)
						{
							try
							{
								fileSystemWatcher.EnableRaisingEvents = wasWatching;
							}
							catch (Exception ex)
							{
								Program.LogError(ex);
							}
							fileSystemTimer.Enabled = wasWatching;
						}
					}

					if (ToggleCollection.Instance.IsOn("Log/Verbose"))
					{
						Console.WriteLine($"add or replacing asset {ResolvedPath}...");
					}

					if (TryConvertIconIfNecessary(fileName, out var newFileName)
						|| TryConvertProgramIconIfNecessary(fileName, out newFileName))
					{
						if (ToggleCollection.Instance.IsOn("Log/Verbose"))
						{
							Console.WriteLine($"converted from icon/program to {newFileName}...");
						}
						resolvedPath = null;
						ResolvedPath = newFileName;
						return;
					}

					if (!CopyFileAndUpdate(fileName))
					{
						Program.LogError($"aborting file drop... failed to copy file.");
						return;
					}

					/*
					if (ToggleCollection.Instance.IsOn("Log/Verbose"))
					{
						Console.WriteLine($"refreshing asset {resolvedPath}...");
					}

					//OnResolvedPathChanged();

					try
					{
						pictureBox.LoadAsync(resolvedPath);
					}
					catch (Exception ex)
					{
						progressBar.Visible = false;
						pictureBox.Visible = false;
						errorLabel.Visible = true;
						//MessageBox.Show(ParentForm, resolvedPath + Environment.NewLine + ex.ToString(), "Image Not Valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					*/
					break;
				}
			}
			else if (e.Data.GetData("UniformResourceLocatorW", autoConvert: false) is MemoryStream memoryStream
				&& memoryStream.Length > 2)
			{
				using (var binaryReader = new BinaryReader(memoryStream))
				{
					var url = Encoding.Unicode.GetString(binaryReader.ReadBytes((int)(memoryStream.Length - 2)));

					if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
					{
						Program.LogError($"{url} is not a valid URL.");
						return;
					}

					if (ToggleCollection.Instance.IsOn("Confirmation/DownloadAsset"))
					{
						if (MessageBox.Show(ParentForm, "Are you sure you want to download this file?" + Environment.NewLine + Environment.NewLine + uri, "Confirmation Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
						{
							return;
						}
					}

					DownloadAsset(uri);
					//OnResolvedPathChanged();
				}
			}
		}

		private void innerPanel_MouseClick(object sender, MouseEventArgs e)
		{
			switch (e.Button)
			{
				case MouseButtons.Left:
					PerformAssetAction(Properties.Settings.Default.Details_Asset_MouseLeftClickAction);
					break;
				case MouseButtons.Right:
					PerformAssetAction(Properties.Settings.Default.Details_Asset_MouseRightClickAction);
					break;
				case MouseButtons.Middle:
					PerformAssetAction(Properties.Settings.Default.Details_Asset_MouseMiddleClickAction);
					break;
			}
		}

		private void innerPanel_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			switch (e.Button)
			{
				case MouseButtons.Left:
					PerformAssetAction(Properties.Settings.Default.Details_Asset_MouseLeftDoubleClickAction);
					break;
				case MouseButtons.Right:
					PerformAssetAction(Properties.Settings.Default.Details_Asset_MouseRightDoubleClickAction);
					break;
				case MouseButtons.Middle:
					PerformAssetAction(Properties.Settings.Default.Details_Asset_MouseMiddleDoubleClickAction);
					break;
			}
		}

		#endregion

		#region Error Label

		private void errorLabel_MouseClick(object sender, MouseEventArgs e)
		{
			//PerformNoAssetAction();
		}

		private void errorLabel_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			PerformUnsupportedAssetAction();
		}

		#endregion

		#region File System Watcher + Timer

		private bool fileChanged;

		private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
		{
			fileChanged = true;
		}

		private void fileSystemTimer_Tick(object sender, System.EventArgs e)
		{
			if (!fileChanged)
			{
				return;
			}

			if (ToggleCollection.Instance.IsOn("Log/FileSystemWatch"))
			{
				Console.WriteLine("fswatch: Reloading updated asset file...");
			}

			fileChanged = false;

			// TODO: manually raise the event because the path might be same, but its content changed...
			resolvedPath = null;
			ResolvedPath = ResolveAssetFullPath();
		}

		#endregion

		#region External Applications

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
			PopulateExternalApplicationsSubMenu(openWithToolStripMenuItem, "Open");
			PopulateExternalApplicationsSubMenu(editWithToolStripMenuItem, "Edit");
			PopulateExternalApplicationsSubMenu(useWithToolStripMenuItem, "Use");
		}

		private void PopulateExternalApplicationsSubMenu(ToolStripMenuItem targetMenu, string verb)
		{
			ExternalApplicationUtils.PopulateExternalApplicationsSubMenu(ExternalApplications.Items, targetMenu, verb, StartExternalApplication);
		}

		private void OpenWithDefaultExternalApplication()
			=> OpenWithDefaultExternalApplication("Open");

		private void EditWithDefaultExternalApplication()
			=> OpenWithDefaultExternalApplication("Edit");

		private void UseWithDefaultExternalApplication()
			=> OpenWithDefaultExternalApplication("Use");

		private void OpenWithDefaultExternalApplication(string verb)
		{
			var startInfo = new ProcessStartInfo(ResolvedPath)
			{
				WorkingDirectory = Path.GetDirectoryName(ResolvedPath),
			};

			// NOTE: the Verbs property is somewhat bugged - e.g., it returns only "printto" for PNG's even though "Open" and "Edit" are associated with different programs.

			if (!string.IsNullOrEmpty(verb))
			{
				if (verb.Equals("Open", StringComparison.OrdinalIgnoreCase))
				{
					startInfo.Verb = "Open";
				}
				else if (verb.Equals("Edit", StringComparison.OrdinalIgnoreCase))
				{
					startInfo.Verb = "Edit";
				}
			}

			try
			{
				using (var process = Process.Start(startInfo))
				{
				}
			}
			catch (Exception ex)
			{
				Program.LogError(ex);
			}
		}

		private void StartExternalApplication(ExternalApplication application)
		{
			application?.Start(AppInfos, Shortcuts, Properties.Settings.Default.AppID, ResolvedPath, AssetType);
		}

		#endregion

		#endregion

		#region Class Overrides

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			User32.NativeMethods.AddClipboardFormatListener(Handle);
		}

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
		}

		protected override void DestroyHandle()
		{
			User32.NativeMethods.RemoveClipboardFormatListener(Handle);

			base.DestroyHandle();
		}

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case User32.NativeMethods.WM_CLIPBOARDUPDATE:
					OnClipboardUpdate(EventArgs.Empty);
					break;
			}

			base.WndProc(ref m);
		}

		protected virtual void OnClipboardUpdate(EventArgs e)
		{
			UpdateClipboarDependentControls();
		}

		#endregion

		#region Menu

		private void previewToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowPreview();
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenWithDefaultExternalApplication();
		}

		private void editToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditWithDefaultExternalApplication();
		}

		private void useToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UseWithDefaultExternalApplication();
		}

		private void viewInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SelectInExplorer();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Copy();
		}

		private void fromFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BrowseForFile();
		}

		private void fromURLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PromptUrl();
		}

		private void setFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PasteFromClipboard();
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DeleteUserAssetFile();
		}

		private void setAsCustomIconToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UpdateShortcutsVdf();
		}

		private void setBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (var dialog = new ColorDialog
			{
				CustomColors = CustomColorsCollection.Instance.Get("General")?.Select(x => x.ToArgb()).ToArray(),
			})
			{
				if (CacheLocation != CacheLocation.None
					&& AssetType != AssetType.None)
				{
					var color = (Color)Properties.Settings.Default[$"Details_{CacheLocation}_{AssetType}_BackColor"];

					dialog.Color = color == Color.Transparent ? Color.White : color;
				}

				if (dialog.ShowDialog(ParentForm) == DialogResult.OK)
				{
					if (dialog.CustomColors != null)
					{
						CustomColorsCollection.Instance.Set("General", dialog.CustomColors.Select(argb => Color.FromArgb(argb)).ToArray());
					}

					if (CacheLocation != CacheLocation.None
						&& AssetType != AssetType.None)
					{
						var color = ModifierKeys == Keys.Alt
							? Color.Transparent
							: dialog.Color;

						Properties.Settings.Default[$"Details_{CacheLocation}_{AssetType}_BackColor"] = color;
					}
				}
			}
		}

		#endregion

		#region Commands

		private DataObject GetCopy()
		{
			var dataObject = new DataObject();

			if (pictureBox.BackgroundImage != null)
			{
				dataObject.SetImage(pictureBox.BackgroundImage);
			}

			if (!string.IsNullOrEmpty(ResolvedPath))
			{
				var filePaths = new StringCollection
				{
					ResolvedPath
				};

				dataObject.SetFileDropList(filePaths);
				dataObject.SetText(ResolvedPath);
			}

			return dataObject;
		}

		private void Copy()
		{
			var dataObject = GetCopy();

			if (dataObject.ContainsImage()
				|| dataObject.ContainsFileDropList())
			{
				Clipboard.SetDataObject(dataObject);
			}
		}

		private void PerformNoAssetAction()
		{
			if (ModifierKeys == Keys.Shift)
			{
				if (!CanPromptUrl())
				{
					return;
				}
				PromptUrl();
			}
			else
			{
				if (!CanBrowseForFile())
				{
					return;
				}
				BrowseForFile();
			}
		}

		private void PerformUnsupportedAssetAction()
		{
			if (!CanSelectInExplorer())
			{
				return;
			}
			SelectInExplorer();
		}

		private bool CanShowPreview()
			=> PictureBox.BackgroundImage != null;

		private bool CanOpenWithDefaultExternalApplication()
			=> File.Exists(ResolvedPath);

		private bool CanEditWithDefaultExternalApplication()
			=> File.Exists(ResolvedPath);

		private bool CanUseWithDefaultExternalApplication()
			=> true;

		private bool CanSelectInExplorer()
			=> File.Exists(ResolvedPath);

		private bool CanDeleteFile()
			=> CacheLocation == CacheLocation.UserData
			&& File.Exists(ResolvedPath);

		private bool CanBrowseForFile()
			=> CacheLocation == CacheLocation.UserData;

		private bool CanPromptUrl()
			=> CacheLocation == CacheLocation.UserData;

		private bool CanPasteFromClipboard()
			=> CacheLocation == CacheLocation.UserData
			&& HasClipboardValidData();

		private bool HasClipboardValidData()
		{
			try
			{
				return Clipboard.ContainsImage()
					|| Clipboard.ContainsFileDropList();
			}
			catch (Exception ex)
			{
				Program.LogError(ex);
				return false;
			}
		}

		private void BrowseForFile()
		{
			var filter = (string[])null;

			if (AssetType == AssetType.Icon)
			{
				filter = new string[]
					{
						"PNG Files|*.png",
						"TGA Files|*.tga",
						"Program Files|*.exe",
						"Icon Files|*.ico",
						"All Supported Files|*.png;*.tga;*.exe;*.ico",
						"All Files|*.*",
					};
			}
			else
			{
				filter = new string[]
					{
						"PNG Files|*.png",
						"Jpeg Files|*.jpg;*.jpeg",//;*.jpe;*.jif;*.jfif;*.jfi
						"All Supported Files|*.png;*.jpg;*.jpeg",//;*.jpe;*.jif;*.jfif;*.jfi
						"All Files|*.*"
					};
			}

			using (var dialog = new OpenFileDialog
			{
				Filter = string.Join("|", filter),
				FilterIndex = filter.Length - 1,
			})
			{
				if (dialog.ShowDialog(ParentForm) == DialogResult.OK)
				{
					var fileName = dialog.FileName;

					if (TryConvertIconIfNecessary(fileName, out var newFileName)
						|| TryConvertProgramIconIfNecessary(fileName, out newFileName))
					{
						if (!Properties.Settings.Default.Details_WatchFileChanges)
						{
							resolvedPath = null;
							ResolvedPath = newFileName;
						}
						return;
					}

					CopyFileAndUpdate(fileName);
				}
			}
		}

		private void PromptUrl()
		{
			using (var dialog = new ComboBoxDialog
			{
				Text = "Enter URL",
			})
			{
			retry:
				if (dialog.ShowDialog(ParentForm) == DialogResult.OK)
				{
					if (!Uri.TryCreate((string)dialog.SelectedValue, UriKind.Absolute, out var uri))
					{
						MessageBox.Show($"Please input a valid URL.");
						goto retry;
					}

					DownloadAsset(uri);
				}
			}
		}

		private bool ConfirmAddOrReplaceAsset(string fileName)
		{
			if (!string.IsNullOrEmpty(ResolvedPath)
				&& File.Exists(ResolvedPath))
			{
				if (ToggleCollection.Instance.IsOn("Confirmation/ReplaceAsset"))
				{
					if (MessageBox.Show(ParentForm, "Are you sure you want to replace this file?" + Environment.NewLine + Environment.NewLine + ResolvedPath, "Confirmation Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					{
						return false;
					}
				}
			}
			else if (!string.IsNullOrEmpty(fileName)
				&& fileName.Equals("<CLIPBOARD>"))
			{
				if (ToggleCollection.Instance.IsOn("Confirmation/PasteAsset"))
				{
					if (MessageBox.Show(ParentForm, "Are you sure you want to use the image present on the clipboard?", "Confirmation Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					{
						return false;
					}
				}
			}
			else
			{
				if (ToggleCollection.Instance.IsOn("Confirmation/AddAsset"))
				{
					if (MessageBox.Show(ParentForm, "Are you sure you want to use this file?" + Environment.NewLine + Environment.NewLine + fileName, "Confirmation Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
					{
						return false;
					}
				}
			}

			return true;
		}

		private void PasteFromClipboard()
		{
			if (!ConfirmAddOrReplaceAsset("<CLIPBOARD>"))
			{
				return;
			}

			// Try to copy without conversion for supported types.
			{
				var fileExtensionByMediaTypes = new Dictionary<string, string>
				{
					{ "image/png", ".png" },
					{ "image/bmp", ".bmp" },
					{ "image/ico", ".ico" },
					{ "image/jpeg", ".jpeg" },
				};

				foreach (var kvp in fileExtensionByMediaTypes)
				{
					if (!Clipboard.ContainsData(kvp.Key))
					{
						continue;
					}

					var convertedFullName = Path.ChangeExtension(GetAssetPath(), kvp.Value);
					var data = Clipboard.GetData(kvp.Key);

					if (data is MemoryStream memoryStream)
					{
						EnsureNoAssetsPresent();

						using (var fileStream = File.OpenWrite(convertedFullName))
						{
							memoryStream.Position = 0;
							memoryStream.CopyTo(fileStream);
							memoryStream.Position = 0;
						}

						if (!Properties.Settings.Default.Details_WatchFileChanges)
						{
							resolvedPath = null;
							ResolvedPath = convertedFullName;
						}

						return;
					}
				}
			}

			// format "System.Drawing.Bitmap" returns the image with alpha,
			// fallback on GetImage only as a last resort because it discards the alpha channel.
			{
				var convertedFullName = Path.ChangeExtension(GetAssetPath(), ".png");
				var image = (Image)Clipboard.GetData("System.Drawing.Bitmap")
					?? Clipboard.GetImage();

				if (image != null)
				{
					image.Save(convertedFullName, ImageFormat.Png);

					if (!Properties.Settings.Default.Details_WatchFileChanges)
					{
						resolvedPath = null;
						ResolvedPath = convertedFullName;
					}

					return;
				}
			}

			if (Clipboard.ContainsFileDropList())
			{
				var validExtensions = (string[])null;

				switch (CacheLocation)
				{
					case CacheLocation.LibraryCache:
						validExtensions = validLibraryAssetFileExtensions;
						break;
					case CacheLocation.UserData:
						if (AssetType == AssetType.Icon)
						{
							validExtensions = validIconAssetFileExtensions;
						}
						else
						{
							validExtensions = validLibraryAssetFileExtensions;
						}
						break;
				}

				foreach (var fileName in Clipboard.GetFileDropList())
				{
					if (!validExtensions.Contains(Path.GetExtension(fileName), StringComparer.CurrentCultureIgnoreCase))
					{
						continue;
					}

					if (TryConvertIconIfNecessary(fileName, out var newFileName)
						|| TryConvertProgramIconIfNecessary(fileName, out newFileName))
					{
						if (!Properties.Settings.Default.Details_WatchFileChanges)
						{
							resolvedPath = null;
							ResolvedPath = newFileName;
						}
						return;
					}

					// TODO? more reliable check.
					if (GetAssetPath().Equals(fileName, StringComparison.CurrentCultureIgnoreCase))
					{
						if (!Properties.Settings.Default.Details_WatchFileChanges)
						{
							resolvedPath = null;
							ResolvedPath = fileName;
						}
						return;
					}

					CopyFileAndUpdate(fileName);

					return;
				}
			}

			Program.LogError("Clipboard does not contain a valid image data nor filename.");
		}

		private void EnsureNoAssetsPresent()
		{
			if (!Properties.Settings.Default.Asset_DeleteAllVariantsBeforeCopy)
			{
				return;
			}

			MoveAssetToRecycleBin();
		}

		private void ShowPreview()
		{
			using (var dialog = new Form
			{
				AutoScroll = true,
				FormBorderStyle = FormBorderStyle.SizableToolWindow,
				ShowInTaskbar = false,
				StartPosition = FormStartPosition.CenterScreen,
			})
			{
				dialog.BackColor = Properties.Settings.Default.Details_Asset_Preview_BackColor;

				var cancelButton = new Button { DialogResult = DialogResult.Cancel, TabStop = false, Size = Size.Empty };
				var pictureBox = new PictureBox { BackgroundImage = PictureBox.BackgroundImage, Image = PictureBox.Image, BackgroundImageLayout = ImageLayout.Center, SizeMode = PictureBoxSizeMode.CenterImage, Size = new Size(PictureBox.BackgroundImage.Width, PictureBox.BackgroundImage.Height) };

				dialog.MouseClick += PictureBox_MouseClick;
				dialog.MouseDoubleClick += PictureBox_MouseDoubleClick;

				pictureBox.MouseClick += PictureBox_MouseClick;
				pictureBox.MouseDoubleClick += PictureBox_MouseDoubleClick;

				void PictureBox_MouseClick(object sender, MouseEventArgs e)
				{
					switch (e.Button)
					{
						case MouseButtons.Left:
							PerformPreviewAction(Properties.Settings.Default.Details_Asset_Preview_MouseLeftClickAction);
							break;
						case MouseButtons.Right:
							PerformPreviewAction(Properties.Settings.Default.Details_Asset_Preview_MouseRightClickAction);
							break;
						case MouseButtons.Middle:
							PerformPreviewAction(Properties.Settings.Default.Details_Asset_Preview_MouseMiddleClickAction);
							break;
					}
				}

				void PictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
				{
					switch (e.Button)
					{
						case MouseButtons.Left:
							PerformPreviewAction(Properties.Settings.Default.Details_Asset_Preview_MouseLeftDoubleClickAction);
							break;
						case MouseButtons.Right:
							PerformPreviewAction(Properties.Settings.Default.Details_Asset_Preview_MouseRightDoubleClickAction);
							break;
						case MouseButtons.Middle:
							PerformPreviewAction(Properties.Settings.Default.Details_Asset_Preview_MouseMiddleDoubleClickAction);
							break;
					}
				}

				void PerformPreviewAction(PreviewAction previewAction)
				{
					switch (previewAction)
					{
						case PreviewAction.ChooseBackgroundColor:
							if (ModifierKeys == Keys.Alt)
							{
								dialog.BackColor = Color.Empty;
								Properties.Settings.Default.Details_Asset_Preview_BackColor = dialog.BackColor;
							}
							else
							{
								ChooseBackColor();
							}
							break;
						case PreviewAction.Close:
							dialog.Close();
							break;
						case PreviewAction.ToggleImageSizeMode:
							switch (pictureBox.SizeMode)
							{
								case PictureBoxSizeMode.Zoom:
									UseOriginalSize();
									break;
								case PictureBoxSizeMode.CenterImage:
									UseFitWindow();
									break;
							}
							break;
					}
				}

				void ChooseBackColor()
				{
					using (var colorDialog = new ColorDialog
					{
						Color = dialog.BackColor,
						CustomColors = CustomColorsCollection.Instance.Get("General")?.Select(x => x.ToArgb()).ToArray(),
					})
					{
						if (colorDialog.ShowDialog(dialog) == DialogResult.OK)
						{
							if (colorDialog.CustomColors != null)
							{
								CustomColorsCollection.Instance.Set("General", colorDialog.CustomColors.Select(argb => Color.FromArgb(argb)).ToArray());
							}

							var color = ModifierKeys == Keys.Alt
								? Color.Empty
								: colorDialog.Color;

							dialog.BackColor = color;
							Properties.Settings.Default.Details_Asset_Preview_BackColor = color;
						}
					}
				}

				void UseFitWindow()
				{
					// FIXME: changing the pictureBox's SizeMode has no effect when pictureBox's Dock is set to Fill and the dialog's FormWindowState is set to Maximized.

					pictureBox.Dock = DockStyle.Fill;
					pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
					pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
					dialog.Text = GetTitle();
					dialog.ClientSize = new Size(dialog.ClientSize.Width, dialog.ClientSize.Height);
				}

				void UseOriginalSize()
				{
					pictureBox.Dock = DockStyle.None;
					pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
					pictureBox.Size = new Size(PictureBox.BackgroundImage.Width, PictureBox.BackgroundImage.Height);
					pictureBox.BackgroundImageLayout = ImageLayout.Center;

					dialog.ClientSize = pictureBox.Size;
					dialog.Text = GetTitle();
				}

				string GetTitle()
				{
					return $"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)} - {groupBox.Text} - {PictureBox.BackgroundImage.Width}x{PictureBox.BackgroundImage.Height} - {(pictureBox.SizeMode == PictureBoxSizeMode.CenterImage ? "1:1" : "Fit")}";
				}

				dialog.Text = GetTitle();
				dialog.Controls.Add(cancelButton);
				dialog.Controls.Add(pictureBox);
				dialog.CancelButton = cancelButton;
				dialog.ClientSize = pictureBox.Size;
				dialog.KeyPreview = true;
				dialog.PreviewKeyDown += Dialog_PreviewKeyDown;

				void Dialog_PreviewKeyDown(object _sender, PreviewKeyDownEventArgs _e)
				{
					if (_e.KeyCode == Keys.Space)
					{
						if (pictureBox.Image is null)
						{
							pictureBox.Image = PictureBox.Image;
						}
						else
						{
							pictureBox.Image = null;
						}
					}
				}

				dialog.ShowDialog(ParentForm);
			}

			GC.Collect();
			GC.WaitForPendingFinalizers();
		}

		private void SelectInExplorer()
		{
			try
			{
				using (Process.Start(new ProcessStartInfo
				{
					FileName = "Explorer",
					Arguments = $"/e, /select, \"{ResolvedPath.Replace('/', '\\')}\"",
				}))
				{
				}
			}
			catch (Exception ex)
			{
				Program.LogError(ex);
			}
		}

		private void DeleteUserAssetFile()
		{
			if (!File.Exists(resolvedPath))
			{
				return;
			}

			if (ToggleCollection.Instance.IsOn("Confirmation/RemoveAsset"))
			{
				if (MessageBox.Show(ParentForm, "Are you sure you want to delete this file?" + Environment.NewLine + Environment.NewLine + resolvedPath, "Confirmation Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
				{
					return;
				}
			}

			MoveAssetToRecycleBin();
		}

		private void UpdateShortcutsVdf()
		{
			var shortcutsVdfFullName = Path.Combine(Properties.Settings.Default.SteamAppPath, "userdata", Properties.Settings.Default.SteamUserID.ToString(CultureInfo.InvariantCulture), "config", "shortcuts.vdf");
			var shortcutsVdfBackupFullName = Path.Combine(Properties.Settings.Default.BackupPath, "userdata", Properties.Settings.Default.SteamUserID.ToString(CultureInfo.InvariantCulture), "config", $"shortcuts ({DateTime.Now.ToString("yyyy MM dd HH mm ss ff", CultureInfo.InvariantCulture)}).vdf.bk");

			try
			{
				if (!string.IsNullOrEmpty(Path.GetDirectoryName(shortcutsVdfBackupFullName)))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(shortcutsVdfBackupFullName));
				}
				File.Copy(shortcutsVdfFullName, shortcutsVdfBackupFullName, overwrite: true);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ParentForm, ex.Message, "I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Program.LogError(ex);
				return;
			}

			Shortcuts.Items[Properties.Settings.Default.AppID].Node["icon"] = ResolvedPath;

			using (var stream = File.OpenWrite(shortcutsVdfFullName))
			{
				var vdfWriter = new VdfWriter(stream);

				vdfWriter.Write(Shortcuts.Root);
			}
		}

		#endregion

		#region Implementation

		private void UpdateCacheLocationDepedentControls()
		{
			var isAssetModdable = CacheLocation == CacheLocation.UserData;

			innerPanel.AllowDrop = isAssetModdable;
			fromFileToolStripMenuItem.Visible = isAssetModdable;
			fromURLToolStripMenuItem.Visible = isAssetModdable;
			setFromClipboardToolStripMenuItem.Visible = isAssetModdable;
			toolStripSeparator1.Visible = isAssetModdable;
			toolStripSeparator2.Visible = isAssetModdable;
			removeToolStripMenuItem.Visible = isAssetModdable;

			if (isAssetModdable)
			{
				placeholderLabel.Text = "Double-click or drop an image file here.";
				placeholderLabel.Enabled = true;
			}
			else
			{
				placeholderLabel.Text = "No asset could be found for this item.";
				placeholderLabel.Enabled = false;
			}
		}

		private void UpdateClipboarDependentControls()
		{
			setFromClipboardToolStripMenuItem.Enabled = CanPasteFromClipboard();
		}

		private void PerformAssetAction(AssetAction assetAction)
		{
			switch (assetAction)
			{
				case AssetAction.Preview:
					if (ModifierKeys == Keys.Shift)
					{
						if (!CanOpenWithDefaultExternalApplication())
						{
							return;
						}
						OpenWithDefaultExternalApplication();
					}
					else
					{
						if (!CanShowPreview())
						{
							return;
						}
						ShowPreview();
					}
					break;
				case AssetAction.Open:
					if (ModifierKeys == Keys.Shift)
					{
						if (!CanEditWithDefaultExternalApplication())
						{
							return;
						}
						EditWithDefaultExternalApplication();
					}
					else
					{
						if (!CanOpenWithDefaultExternalApplication())
						{
							return;
						}
						OpenWithDefaultExternalApplication();
					}
					break;
				case AssetAction.Edit:
					if (ModifierKeys == Keys.Shift)
					{
						if (!CanOpenWithDefaultExternalApplication())
						{
							return;
						}
						OpenWithDefaultExternalApplication();
					}
					else
					{
						if (!CanEditWithDefaultExternalApplication())
						{
							return;
						}
						EditWithDefaultExternalApplication();
					}
					break;
				case AssetAction.Use:
					if (!CanUseWithDefaultExternalApplication())
					{
						return;
					}
					UseWithDefaultExternalApplication();
					break;
				case AssetAction.Select:
					if (!CanSelectInExplorer())
					{
						return;
					}
					SelectInExplorer();
					break;
				case AssetAction.OpenFile:
					if (ModifierKeys == Keys.Shift)
					{
						if (!CanPromptUrl())
						{
							return;
						}
						PromptUrl();
					}
					else
					{
						if (!CanBrowseForFile())
						{
							return;
						}
						BrowseForFile();
					}
					break;
				case AssetAction.OpenUrl:
					if (ModifierKeys == Keys.Shift)
					{
						if (!CanBrowseForFile())
						{
							return;
						}
						BrowseForFile();
					}
					else
					{
						if (!CanPromptUrl())
						{
							return;
						}
						PromptUrl();
					}
					break;
			}
		}

		private bool TryConvertIconIfNecessary(string fileName, out string newFileName)
		{
			var fileExtension = Path.GetExtension(fileName);

			if (!fileExtension.Equals(".ico", StringComparison.InvariantCultureIgnoreCase))
			{
				newFileName = null;
				return false;
			}

			using (var bitmap = new Bitmap(fileName))
			{
				newFileName = Path.ChangeExtension(GetAssetPath(), ".png");

				bitmap.Save(newFileName, ImageFormat.Png);

				return true;
			}
		}

		private bool TryConvertProgramIconIfNecessary(string fileName, out string newFileName)
		{
			if (Properties.Settings.Default.AllowProgramsAsIcon)
			{
				if (ToggleCollection.Instance.IsOn("Log/Verbose"))
				{
					//Console.WriteLine("no conversion requested, using link to executable directly.");
				}
				newFileName = null;
				return false;
			}

			var fileExtension = Path.GetExtension(fileName);

			if (!fileExtension.Equals(".exe", StringComparison.InvariantCultureIgnoreCase))
			{
				if (ToggleCollection.Instance.IsOn("Log/Verbose"))
				{
					//Console.WriteLine("not an executable file.");
				}
				newFileName = null;
				return false;
			}

			newFileName = Path.ChangeExtension(GetAssetPath(), ".png");

			if (ToggleCollection.Instance.IsOn("Log/Verbose"))
			{
				Console.WriteLine($"extracting icon from executable file to {newFileName}...");
			}

			try
			{
				IconExtractorUtils.ExtractAll(fileName, newFileName, ImageFormat.Png);
			}
			catch (Exception ex)
			{
				Program.LogError(ex);
				MessageBox.Show(ParentForm, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return true;
		}

		private bool CopyFileAndUpdate(string fileName)
		{
			var path = Path.ChangeExtension(GetAssetPath(), Path.GetExtension(fileName));

			if (string.IsNullOrEmpty(path))
			{
				Program.LogError($"Cancelling... Could not resolve final path to asset. (with filename '{fileName}', path '{GetAssetPath()}', and extension '{Path.GetExtension(fileName)}')");
				return false;
			}

			if (ToggleCollection.Instance.IsOn("Log/Verbose"))
			{
				Console.WriteLine($"Copy from: {fileName}");
				Console.WriteLine($"Copy to: {path}");
			}

			EnsureNoAssetsPresent();

			try
			{
				File.Copy(fileName, path, overwrite: true);
			}
			catch (Exception ex)
			{
				Program.LogError(ex);
				if (ToggleCollection.Instance.IsOn("Notification/CopyError"))
				{
					MessageBox.Show(ParentForm, $"Source: {fileName}{Environment.NewLine}Destination: {path}{Environment.NewLine}{ex}", "I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				return false;
			}

			if (!Properties.Settings.Default.Details_WatchFileChanges)
			{
				resolvedPath = null;
				ResolvedPath = path;
			}

			return true;
		}

		private void MoveAssetToRecycleBin()
		{
			if (!string.IsNullOrEmpty(resolvedPath))
			{
				var fileExtension = Path.GetExtension(resolvedPath);

				if (CacheLocation == CacheLocation.UserData
					&& AssetType != AssetType.Icon)
				{
					if (fileExtension.Equals(".png", StringComparison.CurrentCultureIgnoreCase))
					{
						SendFileToRecycleBin(Path.ChangeExtension(resolvedPath, ".jpg"));
						SendFileToRecycleBin(Path.ChangeExtension(resolvedPath, ".jpeg"));
					}
					else if (fileExtension.Equals(".jpg", StringComparison.CurrentCultureIgnoreCase)
						|| fileExtension.Equals(".jpeg", StringComparison.CurrentCultureIgnoreCase))
					{
						SendFileToRecycleBin(Path.ChangeExtension(resolvedPath, ".png"));
					}
				}

				if (!SendFileToRecycleBin(ResolvedPath))
				{
					//return;
				}
			}

			placeholderLabel.Visible = true;
			pictureBox.Visible = false;
			pictureBox.BackgroundImage = null;
		}

		private bool SendFileToRecycleBin(string fileName)
		{
			if (ToggleCollection.Instance.IsOn("Log/Verbose"))
			{
				Console.WriteLine($"Deleting file '{fileName}'...");
			}

			if (!File.Exists(fileName))
			{
				if (ToggleCollection.Instance.IsOn("Log/Verbose"))
				{
					Console.WriteLine($"File does not exist.");
				}

				return true;
			}

			var fileOp = new Shell32.NativeMethods.SHFILEOPSTRUCT
			{
				wFunc = Shell32.NativeMethods.FO_DELETE,
				pFrom = $"{fileName}\0\0",
				fFlags = Shell32.NativeMethods.FOF_ALLOWUNDO | Shell32.NativeMethods.FOF_NOCONFIRMATION,
			};

			if (Shell32.NativeMethods.SHFileOperation(ref fileOp) != 0)
			{
				Program.LogError($"File {fileName} could not be sent to the recycle bin.");
				//File.Delete(resolvedPath);
				return false;
			}

			if (ToggleCollection.Instance.IsOn("Log/Verbose"))
			{
				Console.WriteLine($"File successfully sent to the recycle bin.");
			}

			return true;
		}

		private static readonly string[] validLibraryAssetFileExtensions = new string[]
		{
			".png",
			".jpg", ".jpeg"//, ".jpe", ".jif", ".jfif", ".jfi", ".pjpeg", ".pjp"
		};

		private static readonly string[] validIconAssetFileExtensions = new string[]
		{
			// TODO? split code between cached icons (.ico only) and user-applied icons (.png, .tga, or .exe)

			".png",
			".tga",
			".exe",

			".ico"
		};

		private string ResolveAssetFullPath()
		{
			var assetPath = GetAssetPath();
			var validExtensions = (string[])null;

			switch (CacheLocation)
			{
				case CacheLocation.LibraryCache:
					validExtensions = validLibraryAssetFileExtensions;
					break;
				case CacheLocation.UserData:
					if (AssetType == AssetType.Icon)
					{
						validExtensions = validIconAssetFileExtensions;
					}
					else
					{
						validExtensions = validLibraryAssetFileExtensions;
					}
					break;
			}

			foreach (var fileExtension in validExtensions)
			{
				var path = Path.ChangeExtension(assetPath, fileExtension);

				if (File.Exists(path))
				{
					return path;
				}
			}

			if (ToggleCollection.Instance.IsOn("Log/AssetFileNotFound"))
			{
				Program.LogError($"Could not find any asset file matching path '{assetPath}'");
			}

			return null;
		}

		private string GetAssetPath()
		{
			if (ToggleCollection.Instance.IsOn("Log/GetAssetPath"))
			{
				Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: GetAssetPath()");
			}

			if (Properties.Settings.Default.AppID == 0)
			{
				return "";
			}

			var basePath = (string)null;

			if (CacheLocation == CacheLocation.UserData
				&& AssetType == AssetType.Icon)
			{
				if (Properties.Settings.Default.AppID <= int.MaxValue)
				{
					if (AppInfos != null)
					{
						if (AppInfos.Items.TryGetValue(Properties.Settings.Default.AppID, out var appInfo))
						{
							if (appInfo.Data.Node.ContainsKey("clienticon"))
							{
								// icons are small, so don't bother downloading async.

								basePath = Path.ChangeExtension(Path.Combine(SteamUtils.GetIconCachePath(), appInfo.Data.Node.GetString("clienticon")), ".ico");

								/*
								if (!File.Exists(basePath))
								{
									Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: file not found... Update the icon cache to download it.");
									return "";
								}
								
								if (!File.Exists(basePath))
								{
									if (Properties.Settings.Default.AutoDownloadMissingIcons)
									{
										//using (WebClient webClient = new WebClient())
										var webClient = MainForm.WebClient;
										var url = $"https://steamcdn-a.akamaihd.net/steamcommunity/public/images/apps/{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}/{appInfo.Node.Items["clienticon"].Value}.ico";

										Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: downloading 'clienticon' from '{url}' to '{basePath}'...");

										webClient.DownloadFile(url, basePath);
									}
								}
								else
								*/

								/*if (ToggleCollection.Instance.IsOn("Log/ClientIconPropertyFound"))
								{
									Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: found 'clienticon' in '{basePath}'.");
								}*/

								return Path.ChangeExtension(basePath, null);
							}
							else
							{
								/*if (ToggleCollection.Instance.IsOn("Log/ClientIconPropertyNotFound"))
								{
									Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: missing 'clienticon' property.");
								}*/
							}
						}
						else
						{
							//if (ToggleCollection.Instance.IsOn("Log/AppInfoNotFound"))
							{
								//Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: no associated app info.");
							}
						}
					}
					else
					{
						//Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: AppInfos is null.");
					}
				}
				else
				{
					//Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: no app info (appid > max_int).");
				}

				if (Properties.Settings.Default.NonSteamAppIcon_AllowDirectLinks)
				{
					if (Shortcuts != null)
					{
						if (string.IsNullOrEmpty(Properties.Settings.Default.NonSteamAppIconCachePath))
						{
							Program.LogError($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: the non-steam app icon cache path is not set up.");
							return "";
						}

						if (Shortcuts.Items.TryGetValue(Properties.Settings.Default.AppID, out var shortcut))
						{
							basePath = Path.ChangeExtension(Path.Combine(SteamUtils.GetNonSteamAppIconCachePath(), Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)), ".png");

							if (shortcut.Node.ContainsKey("icon"))
							{
								var originalIconFileName = shortcut.Node.GetString("icon");

								if (!string.IsNullOrEmpty(originalIconFileName)
									&& File.Exists(originalIconFileName))
								{
									if (Properties.Settings.Default.NonSteamAppIcon_AutoExtract
										&& !File.Exists(basePath))
									{
										if (TryConvertIconIfNecessary(originalIconFileName, out var newFileName)
											|| TryConvertProgramIconIfNecessary(originalIconFileName, out newFileName))
										{
											if (ToggleCollection.Instance.IsOn("Log/Verbose"))
											{
												Console.WriteLine($"converted from icon/program to {newFileName}...");
											}

											basePath = newFileName;

											return Path.ChangeExtension(basePath, null);
										}
									}

									basePath = Path.ChangeExtension(Path.Combine(SteamUtils.GetNonSteamAppIconCachePath(), Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)), Path.GetExtension(originalIconFileName));

									if (!File.Exists(basePath))
									{
										if (ToggleCollection.Instance.IsOn("Log/Verbose"))
										{
											Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: copying 'icon' from '{originalIconFileName}' to '{basePath}'...");
										}

										//EnsureNoAssetsPresent();

										try
										{
											File.Copy(originalIconFileName, basePath, overwrite: true);
										}
										catch (Exception ex)
										{
											Program.LogError(ex);
										}
									}
								}

								/*if (ToggleCollection.Instance.IsOn("Log/IconPropertyFound"))
								{
									Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: found cached 'icon' in '{basePath}'.");
								}*/

								return Path.ChangeExtension(basePath, null);
							}
							else
							{
								/*if (ToggleCollection.Instance.IsOn("Log/IconPropertyNotFound"))
								{
									Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: missing or empty 'icon' property.");
								}*/
							}
						}
						else
						{
							//Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: no associated shortcut.");
						}
					}
					else
					{
						//Console.WriteLine($"{Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture)}[{CacheLocation}][{AssetType}]: Shortcut is null.");
					}

					return "";
				}

				// assume cached shortcut icon.
				{
					basePath = Path.ChangeExtension(Path.Combine(SteamUtils.GetNonSteamAppIconCachePath(), Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture) + (Properties.Settings.Default.Asset_NonSteamAppIcon_NameSuffix ?? "")), ".png");

					return Path.ChangeExtension(basePath, null);
				}
			}

			switch (CacheLocation)
			{
				case CacheLocation.LibraryCache:
					basePath = SteamUtils.GetLibraryCachePath();
					break;
				case CacheLocation.UserData:
					if (AssetType == AssetType.Icon)
					{
						basePath = SteamUtils.GetIconCachePath();
					}
					else
					{
						basePath = SteamUtils.GetUserGridsPath();
					}
					break;
			}

			var suffix = (string)null;
			var appPath = Path.Combine(basePath, Properties.Settings.Default.AppID.ToString(CultureInfo.InvariantCulture));

			switch (CacheLocation)
			{
				case CacheLocation.LibraryCache:
					switch (AssetType)
					{
						case AssetType.LibraryCapsule:
							suffix = "_library_600x900";
							break;

						case AssetType.HeroGraphic:
							suffix = "_library_hero";
							break;

						case AssetType.Logo:
							suffix = "_logo";
							break;

						case AssetType.Header:
							suffix = "_header";
							break;

						case AssetType.Icon:
							suffix = "_icon";
							break;
					}
					break;
				case CacheLocation.UserData:
					switch (AssetType)
					{
						case AssetType.LibraryCapsule:
							suffix = "p";
							break;

						case AssetType.HeroGraphic:
							suffix = "_hero";
							break;

						case AssetType.Logo:
							suffix = "_logo";
							break;

						case AssetType.Header:
							suffix = "";
							break;

						case AssetType.Icon:
							suffix = Properties.Settings.Default.Asset_NonSteamAppIcon_NameSuffix ?? "";
							break;
					}
					break;
			}

			return appPath + suffix;
		}

		private void UpdateNonSteamAppUserIconMenu()
		{
			var isNonSteamAppUserIcon =
				CacheLocation == CacheLocation.UserData
				&& AssetType == AssetType.Icon
				&& Shortcuts != null
				&& Shortcuts.Items.ContainsKey(Properties.Settings.Default.AppID);

			toolStripSeparator3.Visible = isNonSteamAppUserIcon;
			setAsCustomIconToolStripMenuItem.Visible = isNonSteamAppUserIcon;
		}

		private void SetOverlayImage(Image image)
		{
			pictureBox.Image = image;
		}

		private void DownloadAsset(Uri uri)
		{
			using (var webClient = new WebClient
			{

			})
			{
				// the issue of using a temporary file is that it can't be deleted right away since we load the image asynchronously afterward...
				// It could be deleted only once the image is finished loading but then that means we'd need to keep the original source location...
				/*
				var tempFileName = (string)null;

				try
				{
					tempFileName = Path.GetTempFileName();
				}
				catch (Exception ex)
				{
					MessageBox.Show(ParentForm, $"Could not get temporary file name.{Environment.NewLine}{ex}", "I/O Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
				*/
				var tempFileName = Path.ChangeExtension(GetAssetPath(), Path.GetExtension(uri.AbsoluteUri));

				progressBar.Value = 0;
				progressBar.Tag = null;

				EnsureNoAssetsPresent();

				webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36");
				webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
				webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

				try
				{
					webClient.DownloadFileAsync(uri, tempFileName);
				}
				catch (Exception ex)
				{
					Program.LogError(ex);
					return;
				}

				void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
				{
					var show = progressBar.Value == 0;

					progressBar.Value = e.ProgressPercentage;
					if (show)
					{
						progressBar.Visible = true;
					}
				}

				void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
				{
					if (e.Error != null)
					{
						if (ToggleCollection.Instance.IsOn("Notification/DownloadError"))
						{
							MessageBox.Show(ParentForm, $"Could not download file.{Environment.NewLine}{e.Error}", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						return;
					}
					else if (e.Cancelled)
					{
						return;
					}

					/*
					if (!CopyFileAndUpdate(tempFileName))
					{
						return;
					}

					File.Delete(tempFileName);
					*/

					if (!Properties.Settings.Default.Details_WatchFileChanges)
					{
						resolvedPath = null;
						ResolvedPath = tempFileName;
					}

					progressBar.Visible = false;
				}
			}
		}

		#endregion

		private static partial class User32
		{
			internal static partial class NativeMethods
			{
				public const int WM_CLIPBOARDUPDATE = 0x031d;

				[DllImport("User32.dll", SetLastError = true)]
				[return: MarshalAs(UnmanagedType.Bool)]
				public static extern bool AddClipboardFormatListener(IntPtr hwnd);

				[DllImport("User32.dll", SetLastError = true)]
				[return: MarshalAs(UnmanagedType.Bool)]
				public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);
			}
		}

		private static partial class Shell32
		{
			internal static partial class NativeMethods
			{
				[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
				public struct SHFILEOPSTRUCT
				{
					public IntPtr hWnd;
					[MarshalAs(UnmanagedType.U4)]
					public int wFunc;
					public string pFrom;
					public string pTo;
					public short fFlags;
					[MarshalAs(UnmanagedType.Bool)]
					public bool fAnyOperationsAborted;
					public IntPtr hNameMappings;
					public string lpszProgressTitle;
				}

				public const int FO_DELETE = 0x0003;
				public const int FOF_NOCONFIRMATION = 0x0010;
				public const int FOF_ALLOWUNDO = 0x0040;

				[DllImport("Shell32.dll", CharSet = CharSet.Auto)]
				public static extern int SHFileOperation(ref SHFILEOPSTRUCT fileOp);
			}
		}
	}
}
