using System.Collections.Generic;
using System.IO;

namespace Steam.Vdf
{
	public class Shortcuts
	{
		private readonly Dictionary<ulong, Shortcut> items = new Dictionary<ulong, Shortcut>();
		public IReadOnlyDictionary<ulong, Shortcut> Items => items;

		public VdfObject Root { get; private set; }

		public void Clear()
			=> items.Clear();

		public void Load(string path)
		{
			using (var stream = File.OpenRead(path))
			{
				var reader = new VdfReader(stream);
				var root = reader.ReadObject();

				// FIXME: bleh, poor design, the root is supposed to be shortcuts, not be a parent of a shortcuts node.

				if (root is null
					|| !root.ContainsKey("shortcuts"))
				{
					System.Console.WriteLine("The root object is expected to be of type 'shortcuts'.");
					return;
				}

				var shortcuts = root["shortcuts"] as VdfObject;

				Root = shortcuts;

				Root.Key = "shortcuts";

				foreach (var item in shortcuts)
				{
					if (item.Value is VdfObject value
						&& value != null
						&& value.ContainsKey("appid")
						&& value["appid"] is uint appID)
					{
						var shortcut = new Shortcut
						{
							Node = value,
						};

						items.Add(appID, shortcut);
					}
				}
			}
		}
	}
}
