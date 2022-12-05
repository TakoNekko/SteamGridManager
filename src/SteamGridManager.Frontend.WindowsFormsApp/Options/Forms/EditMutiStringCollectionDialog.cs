using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Forms
{
	public partial class EditMutiStringCollectionDialog : Form, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		private StringCollection values;

		public StringCollection Values
		{
			get => values;
			set
			{
				if (values != value)
				{
					values = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Values)));

					if (value != null)
					{
						stringCollectionListBox1.DataSource = this;
						stringCollectionListBox1.DataMember = nameof(Values);
					}
				}
			}
		}
		
		public EditMutiStringCollectionDialog()
		{
			InitializeComponent();
		}
	}
}
