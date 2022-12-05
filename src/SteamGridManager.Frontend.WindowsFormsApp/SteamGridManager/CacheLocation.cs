using System;

namespace SteamGridManager
{
	[Flags]
	public enum CacheLocation
	{
		None = 0x00,
		UserData = 0x01,
		LibraryCache = 0x02,
	}
}
