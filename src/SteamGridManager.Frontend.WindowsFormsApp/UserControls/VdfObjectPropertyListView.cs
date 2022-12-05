using Steam.Vdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.UserControls
{
	using Extensions.Control;
	using Extensions.ListView;
	using Helpers;

	public partial class VdfObjectPropertyListView : UserControl, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		private readonly bool designMode;
		private readonly int PropertyColumnIndex = 1;
		private readonly int ValueColumnIndex = 0;

		#region Properties

		public AppInfos AppInfos { get; set; }

		public Shortcuts Shortcuts { get; set; }

		private ulong appID;

		public ulong AppID
		{
			get => appID;
			set
			{
				if (appID != value)
				{
					appID = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(AppID)));
					OnAppIDChanged();
				}
			}
		}


		private VdfObject vdfObject;

		public VdfObject VdfObject
		{
			get => vdfObject;
			set
			{
				if (vdfObject != value)
				{
					vdfObject = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(VdfObject)));
					OnVdfObjectChanged();
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

		#endregion

		#region Constructors

		public VdfObjectPropertyListView()
		{
			InitializeComponent();

			designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

			if (!designMode)
			{
				this.ApplyWindowTheme();

				InitializeListView();
				UpdateControlStates();
			}
		}

		#endregion

		#region Property Changes

		protected virtual void OnAppIDChanged()
		{
			if (AppID == 0)
			{
				return;
			}

			if (AppInfos.Items.TryGetValue(appID, out var appInfo))
			{
				PopulateListViewWithAppInfo(appInfo);
			}
			else if (Shortcuts.Items.TryGetValue(appID, out var shortcut))
			{
				PopulateListViewWithShortcut(shortcut);
			}
		}

		protected virtual void OnVdfObjectChanged()
		{
			PopulateListViewWithVdfObject();
		}

		#endregion

		#region Event Handlers

		#region List View

		private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
		{
			if (e.Column == 0)
			{
				listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
			}
			else if (e.Column == 1)
			{
				listView.AutoResizeColumns(ColumnHeaderAutoResizeStyleEx.Max);
			}
		}

		private void listView_MouseClick(object sender, MouseEventArgs e)
		{
		}

		private void listView_ItemActivate(object sender, EventArgs e)
		{
			ActivateSelectedItem();
		}

		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateControlStates();
		}

		private void listView_BeforeLabelEdit(object sender, LabelEditEventArgs e)
		{
			var selectedItem = listView.Items[e.Item];

			// NOTE: disable inline editing of vdf object for now. (perhaps later, it could accept ACF)
			if (selectedItem.Tag is VdfObject)
			{
				e.CancelEdit = true;
			}
		}

		private void listView_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if (e.Label is null)
			{
				e.CancelEdit = true;
				return;
			}

			var selectedItem = listView.Items[e.Item];
			var propertyName = selectedItem.SubItems[PropertyColumnIndex].Text;
			var propertyValue = selectedItem.SubItems[ValueColumnIndex].Text;

			var value = (object)null;

			if (selectedItem.Tag is VdfObject)
			{
				// TODO? parse ACF?
				/*
				if (vdfObject.TryParse(e.Label, out var var newValue))
				{
					value = newValue;
				}
				else
				{
					Program.LogError($"Not a valid property value. Got \"{e.Label}\". Expected a valid ACF stream.");
				}
				*/
			}
			else if (selectedItem.Tag is string)
			{
				value = e.Label;
			}
			else if (selectedItem.Tag is uint)
			{
				if (uint.TryParse(e.Label, NumberStyles.Integer, CultureInfo.InvariantCulture, out var newValue))
				{
					value = newValue;
				}
				/*else if (uint.TryParse(e.Label, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out newValue))
				{
					value = newValue;
				}*/
				else
				{
					Program.LogError($"Not a valid property value. Got {e.Label}. Expected an unsigned integer.");
				}
			}

			if (value is null)
			{
				e.CancelEdit = true;
				return;
			}

			if (ToggleCollection.Instance.IsOn("Confirmation/InlineEditProperty"))
			{
				if (MessageBox.Show(this, "Are you sure you want to change the value of this property?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				{
					e.CancelEdit = true;
					return;
				}
			}

			if (AppInfos != null
				&& AppInfos.Items.TryGetValue(appID, out var appInfo))
			{
				appInfo.Data.Node[propertyName] = value;
				OnAppIDChanged();
			}
			else if (Shortcuts != null
				&& Shortcuts.Items.TryGetValue(appID, out var shortcut))
			{
				shortcut.Node[propertyName] = value;
				OnAppIDChanged();
			}
			else if (VdfObject != null)
			{
				VdfObject[propertyName] = value;
				OnVdfObjectChanged();
			}
		}

		private void listView_Resize(object sender, EventArgs e)
		{
			// FIXME: -2 doesn't seem to work on columns with custom DisplayIndex.
			listView.Columns[listView.Columns.Count - 1].Width = -2;
		}

		#endregion

		#region List View Item Context Menu

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AddNewItem();
		}

		private void quickEditToolStripMenuItem_Click(object sender, EventArgs e)
		{
			QuickEditSelectedItem();
		}

		private void editToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditSelectedItem();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CopySelectedItemsToClipboard();
		}

		private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CopyAllItemsToClipboard();
		}

		private void copyPathToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CopyPathOfSelectedItem();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TryPasteVdfObject();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SelectAll();
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RemoveSelectedItems();
		}

		#endregion

		#endregion

		#region Commands

		private void ActivateSelectedItem()
		{
			if (listView.SelectedIndices.Count != 1)
			{
				return;
			}

			var selectedItem = listView.Items[listView.SelectedIndices[0]];

			if (selectedItem.Tag is VdfObject vdfObjectValue)
			{
				switch (Properties.Settings.Default.PropertiesDialog_ObjectAction)
				{
					case VdfPropertyAction.Edit:
						editToolStripMenuItem.PerformClick();
						break;
					case VdfPropertyAction.InlineEdit:
						quickEditToolStripMenuItem.PerformClick();
						break;
					case VdfPropertyAction.Open:
						using (var dialog = new AppPropertiesDialog
						{
							Info = new AppPropertiesDialog.InfoBlock
							{
								Path = System.IO.Path.Combine(Path ?? "", selectedItem.SubItems[PropertyColumnIndex].Text).Replace("\\", "/"),
								AppInfos = AppInfos,
								Shortcuts = Shortcuts,
								VdfObject = vdfObjectValue.DeepClone(),
							},
						})
						{
							if (dialog.ShowDialog(this) == DialogResult.OK)
							{
								(selectedItem.Tag as VdfObject).Copy(dialog.Info.VdfObject);
							}
						}
						break;
				}
			}
			else
			{
				switch (Properties.Settings.Default.PropertiesDialog_DefaultAction)
				{
					case VdfPropertyAction.Edit:
						editToolStripMenuItem.PerformClick();
						break;
					case VdfPropertyAction.InlineEdit:
						quickEditToolStripMenuItem.PerformClick();
						break;
				}
			}
		}

		private void QuickEditSelectedItem()
		{
			if (listView.SelectedIndices.Count != 1)
			{
				return;
			}

			var selectedItem = listView.Items[listView.SelectedIndices[0]];

			if (selectedItem.Tag is VdfObject
				|| selectedItem.Tag is string
				|| selectedItem.Tag is uint)
			{
				selectedItem.BeginEdit();
			}
		}

		// TODO: but first, gotta make VdfObject serializable.
		private void CopySelectedItemsAsObjectsToClipboard()
		{
			var objects = new List<VdfObject>();

			foreach (int index in listView.SelectedIndices)
			{
				var selectedItem = listView.Items[index];

				if (selectedItem.SubItems[PropertyColumnIndex].Tag is VdfObject vdfObject)
				{
					objects.Add(vdfObject.DeepClone());
				}
			}

			Clipboard.SetDataObject(objects, copy: false);
		}

		private void CopySelectedItemsToClipboard()
		{
			var maxHeaderWidth = 0;

			foreach (int index in listView.SelectedIndices)
			{
				var selectedItem = listView.Items[index];

				maxHeaderWidth = Math.Max(maxHeaderWidth, selectedItem.SubItems[PropertyColumnIndex].Text.Length);
			}

			var stringBuilder = new StringBuilder();
			var single = listView.SelectedIndices.Count == 1;

			foreach (int index in listView.SelectedIndices)
			{
				var selectedItem = listView.Items[index];
				var format = single
					? Properties.Settings.Default.PropertyEditor_CopyTextFormat_Single
					: Properties.Settings.Default.PropertyEditor_CopyTextFormat_Many;

				stringBuilder.AppendLine(format
					.Replace("{Key}", $"{selectedItem.SubItems[PropertyColumnIndex].Text.PadRight(maxHeaderWidth)}")
					.Replace("{Value}", $"{selectedItem.SubItems[ValueColumnIndex].Text}"));
			}

			Clipboard.SetText(stringBuilder.ToString());
		}

		private void CopyAllItemsToClipboard()
		{
			if (vdfObject != null)
			{
				var stringBuilder = new StringBuilder();
				var acfWriter = new AcfWriter();

				acfWriter.Write(stringBuilder, vdfObject);

				Clipboard.SetText(stringBuilder.ToString());
			}
			else if (appID != 0)
			{
				if (AppInfos != null
					&& AppInfos.Items.TryGetValue(appID, out var appInfo))
				{
					var stringBuilder = new StringBuilder();
					var acfWriter = new AcfWriter();

					acfWriter.Write(stringBuilder, appInfo.Data.Node);

					Clipboard.SetText(stringBuilder.ToString());
				}
				else if (Shortcuts != null
					&& Shortcuts.Items.TryGetValue(appID, out var shortcut))
				{
					var stringBuilder = new StringBuilder();
					var acfWriter = new AcfWriter();

					acfWriter.Write(stringBuilder, shortcut.Node);

					Clipboard.SetText(stringBuilder.ToString());
				}
			}
		}

		private void CopyPathOfSelectedItem()
		{
			if (listView.SelectedIndices.Count != 1)
			{
				return;
			}

			var selectedItem = listView.Items[listView.SelectedIndices[0]];
			// FIXME: it seems that I stored only the property name in path instead of the full path...
			var path = System.IO.Path.Combine(Path ?? "", selectedItem.SubItems[PropertyColumnIndex].Text).Replace("\\", "/");

			Clipboard.SetText(path);
		}

		private void TryPasteVdfObject()
		{
			var dataObject = Clipboard.GetDataObject();

			if (dataObject.GetData(typeof(List<VdfObject>)) is List<VdfObject> objects)
			{
				foreach (var vdfObject in objects)
				{
					// TODO: insert cloned objects.
				}
			}
		}

		#endregion

		private void AddNewItem()
		{
			using (var dialog = new VdfPropertyEditorDialog
			{
				Path = Path,
			})
			{
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					var propertyName = dialog.PropertyName;
					
					if (AppInfos != null
						&& AppInfos.Items.TryGetValue(appID, out var appInfo))
					{
						appInfo.Data.Node[propertyName] = dialog.Value;
						OnAppIDChanged();
					}
					else if (Shortcuts != null
						&& Shortcuts.Items.TryGetValue(appID, out var shortcut))
					{
						shortcut.Node[propertyName] = dialog.Value;
						OnAppIDChanged();
					}
					else if (VdfObject != null)
					{
						VdfObject[propertyName] = dialog.Value;
						OnVdfObjectChanged();
					}
				}
			}
		}

		private void EditSelectedItem()
		{
			if (listView.SelectedIndices.Count != 1)
			{
				return;
			}

			var selectedItem = listView.Items[listView.SelectedIndices[0]];
			var propertyName = selectedItem.SubItems[PropertyColumnIndex].Text;

			// TODO: support for empty objects.

			if (selectedItem.Tag is null)
			{
				if (ToggleCollection.Instance.IsOn("Notification/EmptyObjectSelection"))
				{
					MessageBox.Show(ParentForm, "Empty objects are currently not supported. You may delete and recreate it.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				return;
			}

			using (var dialog = new VdfPropertyEditorDialog
			{
				TypeIsReadOnly = true,
				Path = Path,
				PropertyName = propertyName,
				// FIXME: this is passing null for empty (tag) objects
				Value = selectedItem.Tag,
			})
			{
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					if (AppInfos != null
						&& AppInfos.Items.TryGetValue(appID, out var appInfo))
					{
						if (!dialog.PropertyName.Equals(propertyName))
						{
							appInfo.Data.Node.Remove(propertyName);
						}
						appInfo.Data.Node[dialog.PropertyName] = dialog.Value;
						OnAppIDChanged();
					}
					else if (Shortcuts != null
						&& Shortcuts.Items.TryGetValue(appID, out var shortcut))
					{
						if (!dialog.PropertyName.Equals(propertyName))
						{
							shortcut.Node.Remove(propertyName);
						}
						shortcut.Node[dialog.PropertyName] = dialog.Value;
						OnAppIDChanged();
					}
					else if (VdfObject != null)
					{
						if (!dialog.PropertyName.Equals(propertyName))
						{
							VdfObject.Remove(propertyName);
						}
						VdfObject[dialog.PropertyName] = dialog.Value;
						OnVdfObjectChanged();
					}
				}
			}
		}

		private void SelectAll()
		{
			listView.SelectAll();
			UpdateControlStates();
		}

		private void RemoveSelectedItems()
		{
			if (listView.SelectedIndices.Count < 1)
			{
				return;
			}

			if (ToggleCollection.Instance.IsOn("Confirmation/DeleteProperty"))
			{
				if (listView.SelectedIndices.Count > 1)
				{
					if (MessageBox.Show(this, "Are you sure you want to delete these properties?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					{
						return;
					}
				}
				else
				{
					if (MessageBox.Show(this, "Are you sure you want to delete this property?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
					{
						return;
					}
				}
			}

			for (var i = listView.SelectedIndices.Count; i > 0; --i)
			{
				var selectedItem = listView.Items[listView.SelectedIndices[i - 1]];
				var propertyName = selectedItem.SubItems[PropertyColumnIndex].Text;

				if (AppInfos != null
					&& AppInfos.Items.TryGetValue(appID, out var appInfo))
				{
					if (!appInfo.Data.Node.ContainsKey(propertyName))
					{
						Program.LogError($"appinfo does not have key '{propertyName}'.");
						continue;
					}

					appInfo.Data.Node.Remove(propertyName);
					OnAppIDChanged();
				}
				else if (Shortcuts != null
					&& Shortcuts.Items.TryGetValue(appID, out var shortcut))
				{
					if (!shortcut.Node.ContainsKey(propertyName))
					{
						Program.LogError($"shorcut does not have key '{propertyName}'.");
						continue;
					}

					shortcut.Node.Remove(propertyName);
					OnAppIDChanged();
				}
				else if (VdfObject != null)
				{
					// FIXME: handle case where user rename parent node.

					if (!VdfObject.ContainsKey(propertyName))
					{
						Program.LogError($"object does not have key '{propertyName}'.");
						continue;
					}

					VdfObject.Remove(propertyName);
					OnVdfObjectChanged();
				}
			}
		}

		#region Implementation

		private void InitializeListView()
		{
			listView.SetDoubleBuffered(true);
			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		private void UpdateControlStates()
		{
			var singleSelection = listView.SelectedIndices.Count == 1;
			var selectedItem = singleSelection ? listView.Items[listView.SelectedIndices[0]] : null;
			// override for appinfo metadata
			var forceReadOnly = selectedItem != null
				&& selectedItem.Group != null
				&& selectedItem.Group.Tag is bool isGroupReadOnly
				&& isGroupReadOnly;
			
			newToolStripMenuItem.Enabled = CanAddItem();
			editToolStripMenuItem.Enabled = CanEdit() && !forceReadOnly;
			quickEditToolStripMenuItem.Enabled = CanQuickEdit() && !forceReadOnly;
			copyToolStripMenuItem.Enabled = CanCopy();
			copyAllToolStripMenuItem.Enabled = CanCopyAll();
			copyPathToolStripMenuItem.Enabled = CanCopyPath();
			pasteToolStripMenuItem.Enabled = CanPaste();
			selectAllToolStripMenuItem.Enabled = CanSelectAll();
			deleteToolStripMenuItem.Enabled = CanDelete() && !forceReadOnly;

			// TODO: hide it for now, feature not implemented yet.
			copyPathToolStripMenuItem.Visible = false;
		}

		private bool CanAddItem()
			=> true;

		private bool CanEdit()
			=> listView.SelectedIndices.Count == 1;

		private bool CanQuickEdit()
			=> listView.SelectedIndices.Count == 1
			&& listView.SelectedIndices[0] is var selectedIndex
			&& listView.Items[selectedIndex] is var selectedItem
			&& !(selectedItem.Tag is VdfObject);

		private bool CanCopy()
			=> listView.SelectedIndices.Count >= 1;

		private bool CanCopyAll()
			=> listView.Items.Count	>= 1;

		private bool CanCopyPath()
			=> listView.SelectedIndices.Count == 1;

		private bool CanPaste()
			=> Clipboard.GetDataObject()?.GetData(typeof(List<VdfObject>)) is List<VdfObject>;
		
		private bool CanSelectAll()
			=> listView.Items.Count >= 1
			&& listView.SelectedIndices.Count != listView.Items.Count;

		private bool CanDelete()
			=> listView.SelectedIndices.Count >= 1;

		private void PopulateListViewWithAppInfo(AppInfo appInfo)
		{
			Text = $"{appID} - App Properties";

			var metadataGroup = new ListViewGroup("Metadata") { Tag = true };
			var dataGroup = new ListViewGroup("Properties") { Tag = false };
			var groups = new ListViewGroup[]
			{
					metadataGroup,
					dataGroup,
			};
			var items = new List<ListViewItem>(appInfo.Data.Node.Count);

			// metadata
			{
				items.Add(new ListViewItem(new string[] { $"{appInfo.Data.AppID}", "App ID" }, metadataGroup)
				{
					Tag = appInfo.Data.AppID,
				});

				items.Add(new ListViewItem(new string[] { $"{appInfo.Data.ChangeNumber}", "Change Number" }, metadataGroup)
				{
					Tag = appInfo.Data.ChangeNumber,
				});

				items.Add(new ListViewItem(new string[] { $"{string.Join("", appInfo.Data.Hash.Select(x => $"{x:X2}"))}", "Hash" }, metadataGroup)
				{
					Tag = appInfo.Data.Hash,
				});

				items.Add(new ListViewItem(new string[] { $"{appInfo.Data.InfoState}", "Info State" }, metadataGroup)
				{
					Tag = appInfo.Data.InfoState,
				});

				items.Add(new ListViewItem(new string[] { $"{appInfo.Data.LastUpdated.ToRelativeTime()}", "Last Updated" }, metadataGroup)
				{
					Tag = appInfo.Data.LastUpdated,
				});

				items.Add(new ListViewItem(new string[] { $"{appInfo.Data.PicsToken}", "Pics Token" }, metadataGroup)
				{
					Tag = appInfo.Data.PicsToken,
				});

				items.Add(new ListViewItem(new string[] { $"{appInfo.Data.Size}", "Size" }, metadataGroup)
				{
					Tag = appInfo.Data.Size,
				});
			}

			foreach (var kvp in appInfo.Data.Node)
			{
				var propertyType = VdfUtils.GetVdfPropertyTypeByPath(System.IO.Path.Combine(Path ?? "", kvp.Key).Replace("\\", "/"));
				var content = kvp.Value is VdfObject ? "..." : VdfUtils.FormatVdfValue(propertyType, kvp.Value);
				var color = VdfUtils.GetColor(propertyType, kvp.Value);

				var item = new ListViewItem(new string[] { content, kvp.Key }, dataGroup)
				{
					ForeColor = (color.HasValue && !color.Value.IsEmpty) ? color.Value : ListView.DefaultForeColor,
					UseItemStyleForSubItems = true,
					Tag = kvp.Value,
				};

				items.Add(item);
			}

			if (items.Count == 0)
			{
				return;
			}

			listView.BeginUpdate();
			listView.Groups.Clear();
			listView.Items.Clear();
			listView.Groups.AddRange(groups);
			listView.Items.AddRange(items.ToArray());
			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyleEx.Max);
			listView.EndUpdate();
		}

		private void PopulateListViewWithShortcut(Steam.Vdf.Shortcut shortcut)
		{
			Text = $"{appID} - Shortcut Properties";

			var items = new List<ListViewItem>(shortcut.Node.Count);

			foreach (var kvp in shortcut.Node)
			{
				var propertyType = VdfUtils.GetVdfPropertyTypeByPath(System.IO.Path.Combine(Path ?? "", kvp.Key).Replace("\\", "/"));
				var content = kvp.Value is VdfObject ? "..." : VdfUtils.FormatVdfValue(propertyType, kvp.Value);
				var color = VdfUtils.GetColor(propertyType, kvp.Value);

				var item = new ListViewItem(new string[] { content, kvp.Key })
				{
					ForeColor = (color.HasValue && !color.Value.IsEmpty) ? color.Value : ListView.DefaultForeColor,
					UseItemStyleForSubItems = true,
					Tag = kvp.Value,
				};

				items.Add(item);
			}

			if (items.Count == 0)
			{
				return;
			}

			listView.BeginUpdate();
			listView.Groups.Clear();
			listView.Items.Clear();
			listView.Items.AddRange(items.ToArray());
			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyleEx.Max);
			listView.EndUpdate();
		}

		private void PopulateListViewWithVdfObject()
		{
			Text = $"{Path} - Object Properties";

			var items = new List<ListViewItem>(VdfObject.Count);

			foreach (var kvp in VdfObject)
			{
				var propertyType = VdfUtils.GetVdfPropertyTypeByPath(System.IO.Path.Combine(Path ?? "", kvp.Key).Replace("\\", "/"));
				var content = kvp.Value is VdfObject ? "..." : VdfUtils.FormatVdfValue(propertyType, kvp.Value);
				var color = VdfUtils.GetColor(propertyType, kvp.Value);

				var item = new ListViewItem(new string[] { content, kvp.Key })
				{
					ForeColor = (color.HasValue && !color.Value.IsEmpty) ? color.Value : ListView.DefaultForeColor,
					UseItemStyleForSubItems = true,
					Tag = kvp.Value,
				};

				items.Add(item);
			}

			if (items.Count == 0)
			{
				return;
			}

			listView.BeginUpdate();
			listView.Items.Clear();
			listView.Items.AddRange(items.ToArray());
			listView.AutoResizeColumns(ColumnHeaderAutoResizeStyleEx.Max);
			listView.EndUpdate();
		}

		#endregion
	}
}
