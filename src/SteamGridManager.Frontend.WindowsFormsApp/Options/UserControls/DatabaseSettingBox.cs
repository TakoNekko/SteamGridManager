using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls
{
	public partial class DatabaseSettingBox : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public GroupBox GroupBox => groupBox;
		public CheckBox LoadAtStartCheckBox => loadAtStartCheckBox;
		public Button LoadButton => loadButton;
		public Button UnloadButton => unloadButton;
		public Button SelectInExplorerButton => selectInExplorerButton;


		private string databasePath;

		public string DatabasePath
		{
			get => databasePath;
			set
			{
				if (databasePath != value)
				{
					databasePath = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DatabasePath)));
					OnDatabasePathChanged();
				}
			}
		}
		/*
		private IDatabase database;

		public IDatabase Database
		{
			get => database;
			set
			{
				if (database != value)
				{
					database = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Database)));
					OnDatabaseChanged();
				}
			}
		}
		*/
		public DatabaseSettingBox()
		{
			InitializeComponent();

			textBox.DataBindings.Add(nameof(textBox.Text), this, nameof(DatabasePath), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);

			// caller is expected to bind the checkbox. e.g.,
			//loadAtStartCheckBox.DataBindings.Add(nameof(loadAtStartCheckBox.Checked), Properties.Settings.Default, nameof(Properties.Settings.Default.Database_AppInfo_LoadAtStart), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);

			groupBox.Text = Text;
		}

		protected virtual void OnDatabasePathChanged()
		{
			//textBox.Text = DatabasePath;
		}

		protected virtual void OnDatabaseChanged()
		{
		}

		private void loadButton_Click(object sender, EventArgs e)
		{
			//database.Read(databasePath);
			// caller must implement this.
		}

		private void unloadButton_Click(object sender, EventArgs e)
		{
			// caller must implement this.
		}

		private void selectInExplorerButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(DatabasePath))
			{
				return;
			}

			Process.Start(new ProcessStartInfo()
			{
				FileName = "Explorer",
				Arguments = $"/e, /select, \"{DatabasePath.Replace('/', '\\')}\""
			});
		}
	}
}
