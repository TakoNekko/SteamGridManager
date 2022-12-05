using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls
{
	public partial class PathBox : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		public PathType PathType { get; set; }
		public PathOpenType PathOpenType { get; set; }
		public string InitialDirectory { get; set; }
		public bool CheckFileExists { get; set; }
		public bool CheckDirectoryExists { get; set; }
		public bool MultiSelect { get; set; }   // not currently implemented
		public int FilterIndex { get; set; }
		public string Filter { get; set; }
		public string Title { get; set; }


		private string defaultPath;

		public string DefaultPath
		{
			get => defaultPath;
			set
			{
				if (defaultPath != value)
				{
					defaultPath = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DefaultPath)));
					OnDefaultPathChanged();
				}
			}
		}


		private string path;

		public string Path
		{
			get => path;
			set
			{
				if (path != value)
				{
					path = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Path)));
				}
			}
		}

		public TextBox TextBox => pathTextBox;
		public Button BrowseButton => browseButton;
		public Button SelectButton => selectButton;
		public Button ResetButton => resetButton;

		private readonly bool designMode;

		public PathBox()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				InitializeDataBindings();
			}
		}
		private void OnDefaultPathChanged()
		{
			if (DefaultPath != null)
			{
				tableLayoutPanel1.SetColumnSpan(selectButton, 1);
				resetButton.Visible = true;
			}
			else
			{
				tableLayoutPanel1.SetColumnSpan(selectButton, 2);
				resetButton.Visible = false;
			}
		}

		private void InitializeDataBindings()
		{
			pathTextBox.DataBindings.Add(nameof(pathTextBox.Text), this, nameof(Path), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void pathTextBox_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

				if (fileNames.Length == 1
					&& ((PathType == PathType.Directory
						&& Directory.Exists(fileNames[0]))
						|| (PathType == PathType.File
						&& File.Exists(fileNames[0]))
					))
				{
					e.Effect = DragDropEffects.Link;
					return;
				}
			}

			e.Effect = DragDropEffects.None;
		}

		private void pathTextBox_DragDrop(object sender, DragEventArgs e)
		{
			if (e.AllowedEffect == DragDropEffects.None)
			{
				return;
			}

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

				if (fileNames.Length == 1
					&& ((PathType == PathType.Directory
						&& Directory.Exists(fileNames[0]))
						|| (PathType == PathType.File
						&& File.Exists(fileNames[0]))
					))
				{
					Path = fileNames[0];
				}
			}
		}

		private void browseButton_Click(object sender, EventArgs e)
		{
			switch (PathType)
			{
				case PathType.File:
					using (var dialog = PathOpenType == PathOpenType.Open ? new OpenFileDialog
					{
						InitialDirectory = string.IsNullOrEmpty(InitialDirectory) ? System.IO.Path.GetDirectoryName(Path) : InitialDirectory,
						CheckFileExists = CheckFileExists,
						CheckPathExists = CheckDirectoryExists,
						Multiselect = MultiSelect,
						Filter = Filter,
						FilterIndex = FilterIndex,
						Title = Title,
					} : (FileDialog)new SaveFileDialog
					{
						InitialDirectory = string.IsNullOrEmpty(InitialDirectory) ? System.IO.Path.GetDirectoryName(Path) : InitialDirectory,
						CheckFileExists = CheckFileExists,
						CheckPathExists = CheckDirectoryExists,
						Filter = Filter,
						FilterIndex = FilterIndex,
						Title = Title,
					})
					{
						if (dialog.ShowDialog(ParentForm) == DialogResult.OK)
						{
							Path = dialog.FileName;
						}
					}
					break;
				case PathType.Directory:
					if (ModifierKeys == Keys.Shift)
					{
						using (var dialog = new FolderBrowserDialog
						{
							RootFolder = Environment.SpecialFolder.MyComputer,
							SelectedPath = Path,
							ShowNewFolderButton = PathOpenType == PathOpenType.Save,
						})
						{
							if (dialog.ShowDialog(ParentForm) == DialogResult.OK)
							{
								Path = dialog.SelectedPath;
							}
						}
					}
					else
					{
						using (var dialog = new OpenFileDialog
						{
							FileName = (PathOpenType == PathOpenType.Save ? "Save" : "Open"),
							InitialDirectory = string.IsNullOrEmpty(InitialDirectory) ? Path : InitialDirectory,
							CheckFileExists = CheckFileExists,
							CheckPathExists = CheckDirectoryExists,
							Multiselect = MultiSelect,
							Title = Title,
						})
						{
							if (dialog.ShowDialog(ParentForm) == DialogResult.OK)
							{
								Path = System.IO.Path.GetDirectoryName(dialog.FileName);
							}
						}
					}
					break;
			}
		}

		private void selectButton_Click(object sender, EventArgs e)
		{
			try
			{
				using (var process = Process.Start(new ProcessStartInfo
				{
					FileName = "Explorer",
					Arguments = $"/e, /select, \"{Path.Replace('/', '\\')}\""
				}))
				{
				}
			}
			catch (Exception ex)
			{
				Program.LogError(ex);
			}
		}

		private void resetButton_Click(object sender, EventArgs e)
		{
			Path = DefaultPath;
		}

		private void pathTextBox_TextChanged(object sender, EventArgs e)
		{
			selectButton.Enabled = PathType == PathType.Directory ? Directory.Exists(pathTextBox.Text) : File.Exists(pathTextBox.Text);
			resetButton.Enabled = !pathTextBox.Text.Equals(DefaultPath);
		}
	}

	public enum PathType
	{
		None,
		Directory,
		File,
	}

	public enum PathOpenType
	{
		None,
		Open,
		Save,
	}
}
