using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Options.UserControls
{
	using Extensions.Control;
	using Extensions.ListView;

	public partial class StringCollectionListBox : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };


		public ListView ListBox => listView;


		private StringCollection strings;


		private object dataSource;

		public object DataSource
		{
			get => dataSource;
			set
			{
				if (dataSource != value)
				{
					dataSource = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DataSource)));

					InitializeDataBindings();
				}
			}
		}


		private string dataMember;

		public string DataMember
		{
			get => dataMember;
			set
			{
				if (dataMember != value)
				{
					dataMember = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(DataMember)));

					InitializeDataBindings();
				}
			}
		}

		private readonly bool designMode;

		public StringCollectionListBox()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				listView.SetDoubleBuffered(true);

				if (!OSFeature.Feature.IsPresent(OSFeature.Themes))
				{
					listView.AllowDrop = false;
				}

				UpdateControlStates();
			}
		}

		private void InitializeDataBindings()
		{
			if (DataSource != null
				&& !string.IsNullOrEmpty(DataMember))
			{
				strings = DataSource.GetType().GetProperty(DataMember).GetValue(DataSource) as StringCollection;

				PopulateListView();

				UpdateControlStates();
			}
		}

		private void StringCollectionListBox_SizeChanged(object sender, EventArgs e)
		{
			if (listView.Width < Width)
			{
				listView.Width = Width;
			}
		}

		private void listView_SizeChanged(object sender, EventArgs e)
		{
			//listView.Columns[listView.Columns.Count - 1].Width = -2;
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateControlStates();
		}

		private void listView_BeforeLabelEdit(object sender, LabelEditEventArgs e)
		{

		}

		private void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if (e.Label is null)
			{
				return;
			}

			SetValue(e.Item, e.Label);
		}

		private void listView_ItemActivate(object sender, EventArgs e)
		{
			editToolStripMenuItem.PerformClick();
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			New();
		}

		private void quickEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			QuickEdit();
		}

		private void editToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Edit();
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Delete();
		}

		private void New()
		{
			if (strings is null)
			{
				return;
			}

			var value = "";

			strings.Add(value);
			listView.Items.Add(value);

			SelectLastItem();
			QuickEdit();
		}

		private void SelectLastItem()
		{
			listView.SelectedIndices.Clear();
			listView.SelectedIndices.Add(listView.Items.Count - 1);
		}

		private void QuickEdit()
		{
			if (listView.SelectedIndices.Count != 1)
			{
				return;
			}

			var selectedIndex = listView.SelectedIndices[0];
			var selectedItem = listView.Items[selectedIndex];

			selectedItem.BeginEdit();
		}


		private bool canEdit;

		public bool CanEdit
		{
			get => canEdit;
			set
			{
				if (canEdit != value)
				{
					canEdit = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(CanEdit)));

					if (value)
					{
						editToolStripMenuItem.Visible = true;
					}

					editToolStripMenuItem.Enabled = value;
				}
			}
		}

		public event EventHandler EditRequested = delegate { };

		private void Edit()
		{
			EditRequested.Invoke(this, EventArgs.Empty);
		}

		private void Delete()
		{
			if (listView.SelectedIndices.Count < 1)
			{
				return;
			}
			else if (strings is null)
			{
				return;
			}

			if (ToggleCollection.Instance.IsOn("Confirmation/DeleteString"))
			{
				if (MessageBox.Show(ParentForm, "Are you sure you want to delete this item?", "Please confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				{
					return;
				}
			}

			listView.BeginUpdate();

			for (var i = listView.SelectedIndices.Count; i > 0; --i)
			{
				var selectedIndex = listView.SelectedIndices[i - 1];

				listView.Items.RemoveAt(selectedIndex);
				strings.RemoveAt(selectedIndex);
			}

			listView.EndUpdate();
		}

		private void PopulateListView()
		{
			if (strings is null)
			{
				return;
			}

			var values = new List<string>();

			foreach (var value in strings)
			{
				values.Add(value);
			}

			listView.BeginUpdate();
			listView.Items.AddRange(values.Select(x => new ListViewItem(x)).ToArray());
			listView.EndUpdate();
			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyleEx.ColumnContent);
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SelectAll();
		}

		private bool CanSelectAll()
			=> listView.SelectedIndices.Count != listView.Items.Count;

		private void SelectAll()
		{
			listView.SelectAll();
			UpdateControlStates();
		}

		private bool CanAddItem()
			=> true;

		private bool CanQuickEdit()
			=> listView.SelectedIndices.Count == 1;

		private bool CanEditItem()
			=> listView.SelectedIndices.Count == 1;

		private bool CanDelete()
			=> listView.SelectedIndices.Count >= 1;

		private void UpdateControlStates()
		{
			newToolStripMenuItem.Enabled = CanAddItem();
			quickEditToolStripMenuItem.Enabled = CanQuickEdit();
			editToolStripMenuItem.Enabled = CanEditItem();
			selectAllToolStripMenuItem.Enabled = CanSelectAll();
			deleteToolStripMenuItem.Enabled = CanDelete();
		}

		public void SetValue(int itemIndex, string value)
		{
			strings[itemIndex] = value;
			listView.Items[itemIndex].Text = value;

			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyleEx.ColumnContent);
		}

		private void listView_DragDrop(object sender, DragEventArgs e)
		{
			var targetIndex = listView.InsertionMark.Index;

			if (targetIndex == -1)
			{
				return;
			}

			if (e.Data.GetData(typeof(ListViewItem)) is ListViewItem listViewItem)
			{
				var oldIndex = listViewItem.Index;

				if (listView.InsertionMark.AppearsAfterItem)
				{
					++targetIndex;
				}

				if (targetIndex <= oldIndex)
				{
					++oldIndex;
				}

				strings.Insert(targetIndex, listViewItem.Text);

				if (e.Effect == DragDropEffects.Move)
				{
					strings.RemoveAt(oldIndex);
				}

				listView.Items.Insert(targetIndex, (ListViewItem)listViewItem.Clone());

				if (e.Effect == DragDropEffects.Move)
				{
					listView.Items.Remove(listViewItem);
				}
			}
		}

		private void listView_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;

			if (e.Data.GetDataPresent(typeof(ListViewItem)))
			{
				if (e.AllowedEffect.HasFlag(DragDropEffects.Copy)
					&& ModifierKeys.HasFlag(Keys.Control))
				{
					e.Effect = DragDropEffects.Copy;
				}
				else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
				{
					e.Effect = DragDropEffects.Move;
				}
			}

			if (e.Effect == DragDropEffects.None)
			{
				return;
			}
		}

		private void listView_DragLeave(object sender, EventArgs e)
		{
			listView.InsertionMark.Index = -1;
		}

		private void listView_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.None;

			if (e.Data.GetDataPresent(typeof(ListViewItem)))
			{
				if (ModifierKeys.HasFlag(Keys.Control)
					&& e.AllowedEffect.HasFlag(DragDropEffects.Copy))
				{
					e.Effect = DragDropEffects.Copy;
				}
				else if (e.AllowedEffect.HasFlag(DragDropEffects.Move))
				{
					e.Effect = DragDropEffects.Move;
				}
			}

			if (e.Effect == DragDropEffects.None)
			{
				return;
			}

			var targetPoint = listView.PointToClient(new Point(e.X, e.Y));
			var targetIndex = listView.InsertionMark.NearestIndex(targetPoint);

			if (targetIndex == -1)
			{
				return;
			}

			var itemBounds = listView.GetItemRect(targetIndex);

			listView.InsertionMark.AppearsAfterItem = targetPoint.Y > (itemBounds.Top + (itemBounds.Height / 2));
			listView.InsertionMark.Index = targetIndex;
		}

		private void listView_ItemDrag(object sender, ItemDragEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				listView.DoDragDrop(e.Item, DragDropEffects.Copy | DragDropEffects.Move);
			}
		}
	}
}
