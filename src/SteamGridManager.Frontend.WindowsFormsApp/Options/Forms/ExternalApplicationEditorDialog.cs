using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Forms
{
	using Extensions.ListView;
	using Extensions.TextBox;
	using Helpers;

	public partial class ExternalApplicationEditorDialog : Form, INotifyPropertyChanged
	{
		#region Events

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		#endregion

		#region Properties

		private ExternalApplication externalApplication = new ExternalApplication();

		public ExternalApplication ExternalApplication
		{
			get => externalApplication;
			set
			{
				if (externalApplication != value)
				{
					externalApplication = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ExternalApplication)));
					OnExternalApplicationChanged();
				}
			}
		}

		private void OnExternalApplicationChanged()
		{
			ClearDataBindings();
			InitializeDataBindings();
			PopulateAssetTypeNames();

			if (!string.IsNullOrEmpty(ExternalApplication.Verb))
			{
				typeComboBox.SelectedIndex = typeComboBox.FindStringExact(ExternalApplication.Verb);
			}
			else
			{
				typeComboBox.SelectedIndex = 0;
			}
		}

		#endregion

		#region Constructors

		public ExternalApplicationEditorDialog()
		{
			InitializeComponent();

			if (!DesignMode)
			{
				InitializeDataSource();
				PopulateInterpolationSymbolContextMenu();
				UpdateControlStates();
			}
		}

		#endregion

		#region Event Handlers

		private void locationBrowseButton_Click(object sender, EventArgs e)
		{
			BrowseLocation();
		}

		private void argumentsInsertButton_Click(object sender, EventArgs e)
		{
			ShowInterpolationSymbolContextMenu();
		}

		private void titleTextBox_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

				if (fileNames.Length == 1)
				{
					e.Effect = DragDropEffects.Link;
					return;
				}
			}

			e.Effect = DragDropEffects.None;
		}

		private void titleTextBox_DragDrop(object sender, DragEventArgs e)
		{
			if (e.AllowedEffect == DragDropEffects.None)
			{
				return;
			}

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

				if (fileNames.Length == 1)
				{
					titleTextBox.Text = Path.GetFileNameWithoutExtension(fileNames[0]);
				}
			}
		}

		private void locationTextBox_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

				if (fileNames.Length == 1
					&& !string.IsNullOrEmpty(fileNames[0])
					&& Path.GetExtension(fileNames[0]) is var fileExtension
					&& !string.IsNullOrEmpty(fileExtension)
					&& fileExtension.Equals(".exe", StringComparison.InvariantCultureIgnoreCase)
					&& File.Exists(fileNames[0]))
				{
					e.Effect = DragDropEffects.Link;
					return;
				}
			}

			e.Effect = DragDropEffects.None;
		}

		private void locationTextBox_DragDrop(object sender, DragEventArgs e)
		{
			if (e.AllowedEffect == DragDropEffects.None)
			{
				return;
			}

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

				if (fileNames.Length == 1
					&& !string.IsNullOrEmpty(fileNames[0])
					&& Path.GetExtension(fileNames[0]) is var fileExtension
					&& !string.IsNullOrEmpty(fileExtension)
					&& fileExtension.Equals(".exe", StringComparison.InvariantCultureIgnoreCase)
					&& File.Exists(fileNames[0]))
				{
					locationTextBox.Text = fileNames[0];
				}
			}
		}

		private void startingDirectoryTextBox_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

				if (fileNames.Length == 1
					&& !string.IsNullOrEmpty(fileNames[0]))
				{
					e.Effect = DragDropEffects.Link;
					return;
				}
			}

			e.Effect = DragDropEffects.None;
		}

		private void startingDirectoryTextBox_DragDrop(object sender, DragEventArgs e)
		{
			if (e.AllowedEffect == DragDropEffects.None)
			{
				return;
			}

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

				if (fileNames.Length == 1
					&& !string.IsNullOrEmpty(fileNames[0]))
				{
					if (Directory.Exists(fileNames[0]))
					{
						startingDirectoryTextBox.Text = fileNames[0];
					}
					else
					{
						startingDirectoryTextBox.Text = Path.GetDirectoryName(fileNames[0]);
					}
				}
			}
		}

		private void titleTextBox_TextChanged(object sender, EventArgs e)
		{
			UpdateControlStates();
		}

		private void locationTextBox_TextChanged(object sender, EventArgs e)
		{
			UpdateControlStates();
		}

		private void assetTypeNameListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if (e.Label is null)
			{
				e.CancelEdit = true;
				return;
			}

			var copy = new List<string>(ExternalApplication.AssetTypeNames);

			copy[e.Item] = e.Label;

			ExternalApplication.AssetTypeNames = copy;
		}

		private void renameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (assetTypeNameListView.SelectedIndices.Count != 1)
			{
				return;
			}

			var selectedIndex = assetTypeNameListView.SelectedIndices[0];
			var selectedItem = assetTypeNameListView.Items[selectedIndex];

			selectedItem.BeginEdit();
		}

		private void assetTypeNameListView_SelectedIndexChanged(object sender, EventArgs e)
		{
			renameToolStripMenuItem.Enabled = CanQuickEditAssetTypeName();
		}

		#endregion

		#region Implementation

		private void InitializeDataSource()
		{
			typeComboBox.DataSource = new string[] { "Open", "Edit", "Use", "Run", };
		}

		private void ClearDataBindings()
		{
			titleTextBox.DataBindings.Clear();
			locationTextBox.DataBindings.Clear();
			argumentsTextBox.DataBindings.Clear();
			startingDirectoryTextBox.DataBindings.Clear();
			typeComboBox.DataBindings.Clear();
		}

		private void InitializeDataBindings()
		{
			titleTextBox.DataBindings.Add(nameof(titleTextBox.Text), ExternalApplication, nameof(ExternalApplication.Title), formattingEnabled: false, DataSourceUpdateMode.OnPropertyChanged);
			locationTextBox.DataBindings.Add(nameof(locationTextBox.Text), ExternalApplication, nameof(ExternalApplication.Location), formattingEnabled: false, DataSourceUpdateMode.OnPropertyChanged);
			argumentsTextBox.DataBindings.Add(nameof(argumentsTextBox.Text), ExternalApplication, nameof(ExternalApplication.Arguments), formattingEnabled: false, DataSourceUpdateMode.OnPropertyChanged);
			startingDirectoryTextBox.DataBindings.Add(nameof(startingDirectoryTextBox.Text), ExternalApplication, nameof(ExternalApplication.StartingDirectory), formattingEnabled: false, DataSourceUpdateMode.OnPropertyChanged);
			typeComboBox.DataBindings.Add(nameof(typeComboBox.SelectedValue), ExternalApplication, nameof(ExternalApplication.Verb), formattingEnabled: true, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void PopulateAssetTypeNames()
		{
			var listViewItems = new List<ListViewItem>();
			var assetTypes = Enum.GetValues(typeof(AssetType));

			for (var i = 0; i < assetTypes.Length; ++i)
			{
				var assetType = (AssetType)assetTypes.GetValue(i);

				listViewItems.Add(new ListViewItem(new string[]
				{
					i < ExternalApplication.AssetTypeNames.Count ? ExternalApplication.AssetTypeNames[i] : "",
					$"{Enum.GetName(typeof(AssetType), assetType)}",
				})
				{
					Tag = assetType,
				});
			}

			assetTypeNameListView.BeginUpdate();
			assetTypeNameListView.Items.Clear();
			assetTypeNameListView.Items.AddRange(listViewItems.ToArray());
			assetTypeNameListView.EndUpdate();
			assetTypeNameListView.AutoResizeColumns(ColumnHeaderAutoResizeStyleEx.Max);
		}

		private void ShowInterpolationSymbolContextMenu()
		{
			argumentsInsertContextMenuStrip.Show(argumentsInsertButton, Point.Empty);
		}

		private void BrowseLocation()
		{
			using (var dialog = new OpenFileDialog
			{
				Filter = "Executable Files|*.exe|All Files|*.*",
			})
			{
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					locationTextBox.Text = dialog.FileName;
				}
			}
		}

		private void PopulateInterpolationSymbolContextMenu()
		{
			var keywords = new string[]
			{
				"AppID",
				"AppName",
				"Vdf:",
				null,
				"SteamUserID",
				null,
				"AssetType",
				null,
				"FullName",
				"FileName",
				"FileTitle",
				"FileExtension",
				null,
				"DirectoryName",
			};

			foreach (var keyword in keywords)
			{
				if (keyword is null)
				{
					argumentsInsertContextMenuStrip.Items.Add(new ToolStripSeparator());
				}
				else
				{
					argumentsInsertContextMenuStrip.Items.Add(keyword, image: null, onClick: (sender, e) =>
					{
						argumentsTextBox.InsertText(ExternalApplicationUtils.GetFormat(keyword));
					});
				}
			}
		}

		private void UpdateControlStates()
		{
			okButton.Enabled = CanConfirmForm();
		}

		private bool CanConfirmForm()
			=> titleTextBox.Text.Length > 0
			&& locationTextBox.Text.Length > 0;

		private bool CanQuickEditAssetTypeName()
			=> assetTypeNameListView.SelectedIndices.Count == 1;

		#endregion
	}
}
