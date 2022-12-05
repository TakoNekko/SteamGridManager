using SteamGridManager.Frontend.WindowsFormsApp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace SteamGridManager
{
	public class AssetFileWatcherSystem
	{
		#region Singleton

		private static readonly Lazy<AssetFileWatcherSystem> instance = new Lazy<AssetFileWatcherSystem>(() => new AssetFileWatcherSystem());
		public static AssetFileWatcherSystem Instance => instance.Value;
		private AssetFileWatcherSystem()
		{
			fileSystemWatchers = new List<FileSystemWatcher>
			{
				libraryCacheFileSystemWatcher,
				gridsFileSystemWatcher,
				cachedIconsFileSystemWatcher,
				nonSteamAppsCachedIconsFileSystemWatcher,
			};
		}

		#endregion

		#region Fields

		private readonly List<FileSystemWatcher> fileSystemWatchers = new List<FileSystemWatcher>();
		private readonly FileSystemWatcher libraryCacheFileSystemWatcher = new FileSystemWatcher();
		private readonly FileSystemWatcher gridsFileSystemWatcher = new FileSystemWatcher();
		private readonly FileSystemWatcher cachedIconsFileSystemWatcher = new FileSystemWatcher();
		private readonly FileSystemWatcher nonSteamAppsCachedIconsFileSystemWatcher = new FileSystemWatcher();

		#endregion

		#region Properties

		public IReadOnlyList<FileSystemWatcher> FileSystemWatchers => fileSystemWatchers;
		public FileSystemWatcher LibraryCache => libraryCacheFileSystemWatcher;
		public FileSystemWatcher Grids => gridsFileSystemWatcher;
		public FileSystemWatcher CachedIcons => cachedIconsFileSystemWatcher;
		public FileSystemWatcher NonSteamAppsCachedIcons => nonSteamAppsCachedIconsFileSystemWatcher;

		#endregion
		
		public void Initialize()
		{
			libraryCacheFileSystemWatcher.Path = SteamUtils.GetLibraryCachePath();
			gridsFileSystemWatcher.Path = SteamUtils.GetUserGridsPath();
			cachedIconsFileSystemWatcher.Path = SteamUtils.GetIconCachePath();
			nonSteamAppsCachedIconsFileSystemWatcher.Path = SteamUtils.GetNonSteamAppIconCachePath();

			foreach (var watcher in FileSystemWatchers)
			{
				watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;

				// NOTE: it's pointless to set the Filter property since it doesn't support multiple filters (e.g., "*.png;*.jpeg")
			}
		}

		public void Enable()
		{
			foreach (var watcher in FileSystemWatchers)
			{
				watcher.EnableRaisingEvents = true;
			}
		}

		public void Disable()
		{
			foreach (var watcher in FileSystemWatchers)
			{
				watcher.EnableRaisingEvents = false;
			}
		}
	}
}
