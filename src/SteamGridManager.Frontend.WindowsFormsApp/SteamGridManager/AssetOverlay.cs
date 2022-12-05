using System.Collections.Generic;

namespace SteamGridManager
{
	public class AssetOverlay
	{
		public string Name { get; set; }
		public IReadOnlyList<AssetOverlayVariant> Variants { get; set; }
	}
}
