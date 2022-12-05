using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SteamGridManager
{
	public class VdfEnum : INotifyPropertyChanged
	{
		#region Events

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public event EventHandler<string> ItemAdded = delegate { };

		public event EventHandler<string> ItemRemoved = delegate { };

		public event EventHandler<string> ItemChanged = delegate { };

		public event EventHandler ItemsChanged = delegate { };

		#endregion

		#region Fields

		private bool enableRaiseEvents = true;

		#endregion

		#region Properties

		private readonly Dictionary<string, uint> items = new Dictionary<string, uint>();

		public IReadOnlyDictionary<string, uint> Items => items;

		#endregion

		#region Methods
		public uint? GetValue(string key)
		{
			if (items.TryGetValue(key, out var value))
			{
				return value;
			}

			return null;
		}

		public void BeginUpdate()
		{
			enableRaiseEvents = false;
		}

		public void EndUpdate()
		{
			ItemsChanged?.Invoke(this, EventArgs.Empty);
			enableRaiseEvents = true;
		}

		public void Add(string key, uint value)
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
	}

}
