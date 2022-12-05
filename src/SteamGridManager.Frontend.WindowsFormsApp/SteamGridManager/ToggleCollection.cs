using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SteamGridManager
{
	public class ToggleCollection : INotifyPropertyChanged
	{
		#region Singleton

		private static readonly Lazy<ToggleCollection> instance = new Lazy<ToggleCollection>(() => new ToggleCollection());
		public static ToggleCollection Instance => instance.Value;
		private ToggleCollection() { }

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

		private readonly Dictionary<string, bool> items = new Dictionary<string, bool>();

		public IReadOnlyDictionary<string, bool> Items => items;

		#endregion

		#region Methods

		public bool IsOn(string key)
		{
			if (!items.ContainsKey(key))
			{
				Frontend.WindowsFormsApp.Program.LogError($"Toggle collection does not have key {key}.");
				return false;
			}

			return items[key];
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
		
		public void Add(string key, bool value)
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
