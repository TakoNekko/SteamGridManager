using System;
using System.ComponentModel;

namespace SteamGridManager
{
	public class ExternalApplicationCollection : INotifyPropertyChanged
	{
		#region Singleton

		private static readonly Lazy<ExternalApplicationCollection> instance = new Lazy<ExternalApplicationCollection>(() => new ExternalApplicationCollection());
		public static ExternalApplicationCollection Instance => instance.Value;
		private ExternalApplicationCollection() { }

		#endregion

		#region Events

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		#endregion

		#region Properties

		private readonly BindingList<ExternalApplication> items = new BindingList<ExternalApplication>();

		public BindingList<ExternalApplication> Items => items;

		#endregion
	}
}
