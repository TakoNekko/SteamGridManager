using System.Collections.Generic;

namespace Steam.Vdf
{
	public class AppInfoData
	{
		public ulong AppID { get; set; }
		public uint Size { get; set; }
		public uint InfoState { get; set; }
		public uint LastUpdated { get; set; }
		public ulong PicsToken { get; set; }
		public IReadOnlyList<byte> Hash { get; set; }
		public uint ChangeNumber { get; set; }
		public IReadOnlyList<byte> Hash2 { get; set; }

		public VdfObject Node { get; set; }
	}
}
