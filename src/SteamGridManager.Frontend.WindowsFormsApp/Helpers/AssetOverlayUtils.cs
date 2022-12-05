using System;
using System.IO;

namespace SteamGridManager.Frontend.WindowsFormsApp.Helpers
{
	public static class AssetOverlayUtils
	{
		public static string GetOverlaysAssetFileName(AssetType category, string assetName, string fileName)
			=> Path.Combine(
					GetOverlaysAssetFolder(category, assetName),
					fileName
				);

		public static string GetOverlaysAssetFolder(AssetType category, string assetName)
			=> Path.Combine(
					GetOverlaysCategoryFolder(category),
					assetName
				);

		public static string GetOverlaysCategoryFolder(AssetType category)
			=> Path.Combine(
					Properties.Settings.Default.Overlays_Path,
					$"{category}"
				);

		public static string GetDefaultOverlaysRootFolder()
			=> Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
					"Ash",
					"SteamGridManager",
					"overlays"
				);
	}
}
