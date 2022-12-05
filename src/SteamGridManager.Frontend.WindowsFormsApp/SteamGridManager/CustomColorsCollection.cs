using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace SteamGridManager
{
	public class CustomColorsCollection : INotifyPropertyChanged
	{
		#region Singleton

		private static readonly Lazy<CustomColorsCollection> instance = new Lazy<CustomColorsCollection>(() => new CustomColorsCollection());
		public static CustomColorsCollection Instance => instance.Value;
		private CustomColorsCollection() { }

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public event EventHandler<string> ItemAdded = delegate { };

		public event EventHandler<string> ItemRemoved = delegate { };

		public event EventHandler<string> ItemChanged = delegate { };

		public event EventHandler ItemsChanged = delegate { };

		#endregion

		#region Properties

		private readonly Dictionary<string, Color[]> items = new Dictionary<string, Color[]>();

		public IReadOnlyDictionary<string, Color[]> Items => items;

		#endregion

		#region Methods
		public Color[] Get(string key)
			//=> Get(key, new Color[] { Color.Empty });
			=> Get(key, defaultColors: null);

		public Color[] Get(string key, Color[] defaultColors)
		{
			if (!TryGet(key, out var colors))
			{
				colors = defaultColors;
			}

			return colors;
		}

		public bool TryGet(string key, out Color[] colors)
			=> items.TryGetValue(key, out colors);

		public void Set(string key, Color[] value)
			=> items[key] = value;

		public void BeginUpdate()
		{
			enableRaiseEvents = false;
		}

		public void EndUpdate()
		{
			ItemsChanged?.Invoke(this, EventArgs.Empty);
			enableRaiseEvents = true;
		}

		public void Add(string key, Color[] value)
		{
			items.Add(key, value);

			if (enableRaiseEvents)
			{
				ItemAdded?.Invoke(this, key);
			}
		}

		public void Remove(string key)
		{
			items.Remove(key);

			if (enableRaiseEvents)
			{
				ItemRemoved?.Invoke(this, key);
			}
		}

		public void Clear()
		{
			items.Clear();

			if (enableRaiseEvents)
			{
				ItemsChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Fields

		private bool enableRaiseEvents = true;

		#endregion
	}
}
