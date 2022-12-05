using System.Collections.Generic;

namespace Steam.Vdf
{
	public class AppInfos
	{
		private readonly Dictionary<ulong, AppInfo> items = new Dictionary<ulong, AppInfo>();
		public IReadOnlyDictionary<ulong, AppInfo> Items => items;

		public uint Universe { get; private set; }

		public void Clear()
			=> items.Clear();

		public void Load(string path)
		{
			var reader = new AppInfosReader(path);

			Universe = reader.Universe;

			foreach (var item in reader.Items)
			{
				if (item.AppID != 0)
				{
					var appInfo = new AppInfo { Data = item };

					items.Add(item.AppID, appInfo);
				}
			}
		}
	}
}
