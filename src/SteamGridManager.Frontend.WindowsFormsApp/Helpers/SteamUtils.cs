using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SteamGridManager.Frontend.WindowsFormsApp.Helpers
{
	public static class SteamUtils
	{
		#region Constants

		public static readonly Regex AppIDLibraryCacheFileNameRegex = new Regex(@"^(?<AppID>\d+)(?<AssetType>_library_600x900|_library_hero|_logo|_header|_icon)?(?<FileExtension>\.png|\.jpg|\.jpeg)$");//|\.jpe|\.jif|\.jfif|\.jfi|\.pjpeg|\.pjp
		public static readonly Regex AppIDUserDataFileNameRegex = new Regex(@"^(?<AppID>\d+)(?<AssetType>p|_hero|_logo)?(?<FileExtension>\.png|\.jpg|\.jpeg)$");//|\.jpe|\.jif|\.jfif|\.jfi|\.pjpeg|\.pjp
		public static readonly Regex IconFileNameRegex = new Regex(@"^(?<AppID>[0-9a-z]+){40}(?<FileExtension>\.ico)$");

		#endregion

		#region Methods

		public static string FindSteamPathFromProgramFiles()
		{
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Steam");

			if (Directory.Exists(path))
			{
				return path;
			}

			path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Steam");

			if (Directory.Exists(path))
			{
				return path;
			}

			return null;
		}

		public static string FindSteamPathFromRegistry()
		{
			try
			{
				if (Registry.CurrentUser
					.OpenSubKey("SOFTWARE\\Valve\\Steam") is var key
					&& key.GetValue("SteamPath") is string value)
				{
					return value;
				}
			}
			catch (Exception ex)
			{
				Program.LogError(ex);
			}

			return null;
		}

		public static IEnumerable<ulong> EnumerateUserIDs()
		{
			var numericText = new Regex(@"^(?<UserID>\d+)$");
			var userDataPath = Path.Combine(
					Properties.Settings.Default.SteamAppPath,
					"userdata"
				);

			if (Directory.Exists(userDataPath))
			{
				foreach (var directoryName in Directory.EnumerateDirectories(userDataPath, "*.*", SearchOption.TopDirectoryOnly))
				{
					var name = Path.GetFileName(directoryName);
					var match = numericText.Match(name);

					if (match.Success)
					{
						yield return ulong.Parse(match.Groups["UserID"].Value);
					}
				}
			}
		}

		public static string GetLibraryCachePath()
			=> Path.Combine(
					Properties.Settings.Default.SteamAppPath,
					"appcache",
					"librarycache"
				);

		public static string GetUserGridsPath()
			=> Path.Combine(
					Properties.Settings.Default.SteamAppPath,
					"userdata",
					Properties.Settings.Default.SteamUserID.ToString(),
					"config",
					"grid"
				);

		public static string GetIconCachePath()
			=> Path.Combine(
					Properties.Settings.Default.SteamAppPath,
					"steam",
					"games"
				);

		public static string GetNonSteamAppIconCachePath()
			=> Properties.Settings.Default.NonSteamAppIconCachePath;

		public static string GetDefaultNonSteamAppIconCacheFolder()
			=> Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
					"Ash",
					"SteamGridManager",
					"nonsteamapps",
					"iconcache"
				);

		public static string GetDefaultBackupFolder()
			=> Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
					"Ash",
					"SteamGridManager",
					"backups"
				);

		#endregion
	}
}
