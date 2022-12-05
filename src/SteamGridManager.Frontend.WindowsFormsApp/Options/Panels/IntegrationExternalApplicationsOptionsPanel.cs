using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.Panels
{
	using Extensions.ListView;
	using Forms;

	public partial class IntegrationExternalApplicationsOptionsPanel : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		private ExternalApplicationCollection externalApplications;

		public ExternalApplicationCollection ExternalApplications
		{
			get => externalApplications;
			set
			{
				if (externalApplications != value)
				{
					externalApplications = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ExternalApplications)));
					if (!designMode)
					{
						OnExternalApplicationsChanged();
					}
				}
			}
		}

		private readonly bool designMode;

		public IntegrationExternalApplicationsOptionsPanel()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				ExternalApplications  = ExternalApplicationCollection.Instance;
			}
		}

		private void OnExternalApplicationsChanged()
		{
			var otherGroup = new ListViewGroup("Other", HorizontalAlignment.Left);
			var viewerGroup = new ListViewGroup("Viewers", HorizontalAlignment.Left);
			var editorGroup = new ListViewGroup("Editors", HorizontalAlignment.Left);
			var userGroup = new ListViewGroup("Generic", HorizontalAlignment.Left);
			var runnerGroup = new ListViewGroup("Runners", HorizontalAlignment.Left);
			var listViewGroups = new ListViewGroup[]
			{
				otherGroup,
				viewerGroup,
				editorGroup,
				userGroup,
				runnerGroup,
			};

			listView.BeginUpdate();
			listView.Groups.Clear();
			listView.Groups.AddRange(listViewGroups);
			listView.EndUpdate();

			PopulateListViewItems();
		}

		private void PopulateListViewItems()
		{
			var otherGroup = listView.Groups[0];
			var viewerGroup = listView.Groups[1];
			var editorGroup = listView.Groups[2];
			var userGroup = listView.Groups[3];
			var runnerGroup = listView.Groups[4];
			var listViewItems = new List<ListViewItem>();

			foreach (var item in ExternalApplications.Items)
			{
				var group = (ListViewGroup)null;

				if (string.IsNullOrEmpty(item.Verb))
				{
					group = otherGroup;
				}
				else if (item.Verb.Equals("Open", StringComparison.InvariantCultureIgnoreCase))
				{
					group = viewerGroup;
				}
				else if (item.Verb.Equals("Edit", StringComparison.InvariantCultureIgnoreCase))
				{
					group = editorGroup;
				}
				else if (item.Verb.Equals("Use", StringComparison.InvariantCultureIgnoreCase))
				{
					group = userGroup;
				}
				else if (item.Verb.Equals("Run", StringComparison.InvariantCultureIgnoreCase))
				{
					group = runnerGroup;
				}
				else
				{
					group = otherGroup;
				}

				listViewItems.Add(new ListViewItem(new string[]
				{
					item.Title ?? "",
					item.Location ?? "",
					item.Arguments ?? "",
					item.StartingDirectory ?? "",
					item.Verb ?? "",
				}, group)
				{
					Tag = item,
				});
			}

			listView.BeginUpdate();
			listView.Items.Clear();
			listView.Items.AddRange(listViewItems.ToArray());
			listView.EndUpdate();
			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyleEx.Max);

			selectAllToolStripMenuItem.Enabled = CanSelectAll();
		}

		private void listView_DragDrop(object sender, DragEventArgs e)
		{
			if (e.AllowedEffect == DragDropEffects.None)
			{
				return;
			}

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

				foreach (var fileName in fileNames)
				{
					var fileExtension = Path.GetExtension(fileName);

					if (string.IsNullOrEmpty(fileExtension)
						|| !fileExtension.Equals(".exe", StringComparison.InvariantCultureIgnoreCase)
						|| !File.Exists(fileName))
					{
						continue;
					}

					ExternalApplications.Items.Add(new ExternalApplication
					{
						Title = Path.GetFileNameWithoutExtension(fileName),
						Location = fileName,
						StartingDirectory = Path.GetDirectoryName(fileName),
						Verb = "Open"
					});

					PopulateListViewItems();
				}
			}
			// TODO: handle listviewitem drop
		}

		private void listView_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: true);

				foreach (var fileName in fileNames)
				{
					var fileExtension = Path.GetExtension(fileName);

					if (string.IsNullOrEmpty(fileExtension)
						|| !fileExtension.Equals(".exe", StringComparison.InvariantCultureIgnoreCase)
						|| !File.Exists(fileName))
					{
						continue;
					}

					e.Effect = DragDropEffects.Link;
					return;
				}
			}
			// TODO: handle listviewitem drop

			e.Effect = DragDropEffects.None;
		}

		private void listView_DragLeave(object sender, EventArgs e)
		{
			// TODO: handle listviewitem drop
		}

		private void listView_DragOver(object sender, DragEventArgs e)
		{
			// TODO: handle listviewitem drop
		}

		private void listView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			// TODO: handle listviewitem drop
		}

		private void listView_ItemActivate(object sender, EventArgs e)
		{
			editToolStripMenuItem.PerformClick();
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateControlStates();
		}

		private void UpdateControlStates()
		{
			newToolStripMenuItem.Enabled = CanAddItem();
			editToolStripMenuItem.Enabled = CanEditSelectedItem();
			deleteToolStripMenuItem.Enabled = CanDeleteSelectedItems();
			selectAllToolStripMenuItem.Enabled = CanSelectAll();
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!CanAddItem())
			{
				return;
			}

			AddItem();
		}

		private void editToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!CanEditSelectedItem())
			{
				return;
			}

			EditSelectedItem();
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!CanDeleteSelectedItems())
			{
				return;
			}

			DeleteSelectedItems();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!CanSelectAll())
			{
				return;
			}

			SelectAll();
		}

		private bool CanAddItem()
			=> true;

		private bool CanEditSelectedItem()
			=> listView.SelectedIndices.Count == 1;

		private bool CanDeleteSelectedItems()
			=> listView.SelectedIndices.Count >= 1;

		private void AddItem()
		{
			using (var form = new ExternalApplicationEditorDialog
			{
				ExternalApplication = new ExternalApplication(),
			})
			{
				if (form.ShowDialog(ParentForm) == DialogResult.OK)
				{
					ExternalApplications.Items.Add(form.ExternalApplication);

					PopulateListViewItems();
				}
			}
		}

		private void EditSelectedItem()
		{
			var selectedIndex = listView.SelectedIndices[0];
			var selectedItem = listView.Items[selectedIndex];
			var externalApplication = selectedItem.Tag as ExternalApplication;

			if (externalApplication is null)
			{
				return;
			}

			using (var form = new ExternalApplicationEditorDialog
			{
				ExternalApplication = externalApplication.Clone(),
			})
			{
				if (form.ShowDialog(ParentForm) == DialogResult.OK)
				{
					ExternalApplications.Items.Insert(selectedIndex, form.ExternalApplication);
					ExternalApplications.Items.RemoveAt(selectedIndex + 1);
					//selectedItem.Tag = form.ExternalApplication;
					PopulateListViewItems();
				}
			}
		}

		private void DeleteSelectedItems()
		{
			for (var i = listView.SelectedIndices.Count; i > 0; --i)
			{
				ExternalApplications.Items.RemoveAt(listView.SelectedIndices[i - 1]);
			}

			PopulateListViewItems();
		}

		private bool CanSelectAll()
			=> listView.SelectedIndices.Count != listView.Items.Count;

		private void SelectAll()
		{
			listView.SelectAll();
			UpdateControlStates();
		}
	}
}
