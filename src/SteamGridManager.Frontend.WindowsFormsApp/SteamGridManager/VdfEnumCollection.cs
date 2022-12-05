using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SteamGridManager
{
	public class VdfEnumCollection : INotifyPropertyChanged
	{
		#region Singleton

		private static readonly Lazy<VdfEnumCollection> instance = new Lazy<VdfEnumCollection>(() => new VdfEnumCollection());
		public static VdfEnumCollection Instance => instance.Value;
		private VdfEnumCollection() { }

		#endregion

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

		private readonly Dictionary<string, VdfEnum> items = new Dictionary<string, VdfEnum>();

		public IReadOnlyDictionary<string, VdfEnum> Items => items;

		#endregion

		#region Methods

		public VdfEnum GetValue(string key)
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

		public void Add(string key, VdfEnum value)
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
