using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Extensions.ListView
{
	using ListView = System.Windows.Forms.ListView;

	internal static partial class ListViewExtensionMethods
	{
		public static void SelectRange(this ListView listView, IEnumerable<int> indices)
		{
			if (listView is null) throw new ArgumentNullException(nameof(listView));
			else if (indices is null) throw new ArgumentNullException(nameof(indices));

			foreach (var index in indices)
			{
				SetItemState(listView, itemIndex: index, mask: NativeMethods.LVIS_SELECTED, value: NativeMethods.LVIS_SELECTED);
			}
		}

		public static void DeselectRange(this ListView listView, IEnumerable<int> indices)
		{
			if (listView is null) throw new ArgumentNullException(nameof(listView));
			else if (indices is null) throw new ArgumentNullException(nameof(indices));

			foreach (var index in indices)
			{
				SetItemState(listView, itemIndex: index, mask: NativeMethods.LVIS_SELECTED, value: 0);
			}
		}

		public static void SelectAll(this ListView listView)
			=> SetItemState(listView, itemIndex: -1, mask: NativeMethods.LVIS_SELECTED, value: NativeMethods.LVIS_SELECTED);

		public static void DeselectAll(this ListView listView)
			=> SetItemState(listView, itemIndex: -1, mask: NativeMethods.LVIS_SELECTED, value: 0);

		private static void SetItemState(this ListView listView, int itemIndex, int mask, int value)
		{
			var lvItem = new NativeMethods.LVITEM
			{
				stateMask = mask,
				state = value,
			};

			//new HandleRef(listView, listView.Handle)
			NativeMethods.SendMessageLVItem(listView.Handle, NativeMethods.LVM_SETITEMSTATE, itemIndex, ref lvItem);
		}
	}

	public enum ColumnHeaderAutoResizeStyleEx
	{
		None,
		HeaderSize,
		ColumnContent,
		Min,
		Max,
	}

	internal static partial class ListViewExtensionMethods
	{
		public static void AutoResizeColumns(this ListView listView, ColumnHeaderAutoResizeStyleEx style)
			=> AutoResizeColumns(listView, virtualListViewItems: null, style, minWidth: null, maxWidth: null);

		public static void AutoResizeColumns(this ListView listView, ColumnHeaderAutoResizeStyleEx style, int? minWidth, int? maxWidth)
			=> AutoResizeColumns(listView, virtualListViewItems: null, style, minWidth, maxWidth);

		public static void AutoResizeColumns(this ListView listView, IEnumerable<ListViewItem> virtualListViewItems, ColumnHeaderAutoResizeStyleEx style)
			=> AutoResizeColumns(listView, virtualListViewItems, style, minWidth: null, maxWidth: null);

		public static void AutoResizeColumns(this ListView listView, IEnumerable<ListViewItem> virtualListViewItems, ColumnHeaderAutoResizeStyleEx style, int? minWidth, int? maxWidth)
		{
			foreach (ColumnHeader columnHeader in listView.Columns)
			{
				AutoResizeColumn(listView, virtualListViewItems, columnHeader, style, minWidth, maxWidth);
			}
		}

		public static void AutoResizeColumn(this ListView listView, ColumnHeader columnHeader, ColumnHeaderAutoResizeStyleEx style)
			=> AutoResizeColumn(listView, virtualListViewItems: null, columnHeader, style, minWidth: null, maxWidth: null);

		public static void AutoResizeColumn(this ListView listView, IEnumerable<ListViewItem> virtualListViewItems, ColumnHeader columnHeader, ColumnHeaderAutoResizeStyleEx style)
			=> AutoResizeColumn(listView, virtualListViewItems, columnHeader, style, minWidth: null, maxWidth: null);

		public static void AutoResizeColumn(this ListView listView, ColumnHeader columnHeader, ColumnHeaderAutoResizeStyleEx style, int? minWidth, int? maxWidth)
			=> AutoResizeColumn(listView, virtualListViewItems: null, columnHeader, style, minWidth, maxWidth);

		public static void AutoResizeColumn(this ListView listView, IEnumerable<ListViewItem> virtualListViewItems, ColumnHeader columnHeader, ColumnHeaderAutoResizeStyleEx style, int? minWidth, int? maxWidth)
		{
			var headerLength = 0;
			var contentLength = 0;
			var maxLength = 0;

			if (style == ColumnHeaderAutoResizeStyleEx.HeaderSize
				|| style == ColumnHeaderAutoResizeStyleEx.Min
				|| style == ColumnHeaderAutoResizeStyleEx.Max)
			{
				headerLength = TextRenderer.MeasureText(columnHeader.Text, listView.Font).Width;
				maxLength = headerLength;
			}

			if (style == ColumnHeaderAutoResizeStyleEx.ColumnContent
				|| style == ColumnHeaderAutoResizeStyleEx.Min
				|| style == ColumnHeaderAutoResizeStyleEx.Max)
			{
				if (virtualListViewItems is null)
				{
					contentLength = 0;

					foreach (ListViewItem lvi in listView.Items)
					{
						contentLength = Math.Max(contentLength, TextRenderer.MeasureText(lvi.SubItems[columnHeader.Index].Text, listView.Font).Width);
					}
				}
				else
				{
					contentLength = virtualListViewItems.Max(lvi => TextRenderer.MeasureText(lvi.SubItems[columnHeader.Index].Text, listView.Font).Width);
				}

				maxLength = contentLength;
			}

			if (style == ColumnHeaderAutoResizeStyleEx.Min)
			{
				maxLength = Math.Min(headerLength, contentLength);
			}
			else if (style == ColumnHeaderAutoResizeStyleEx.Max)
			{
				maxLength = Math.Max(headerLength, contentLength);
			}

			if (minWidth.HasValue)
			{
				maxLength = Math.Max(maxLength, minWidth.Value);
			}

			if (maxWidth.HasValue)
			{
				maxLength = Math.Min(maxLength, maxWidth.Value);
			}

			columnHeader.Width = maxLength;
		}
	}

	internal static partial class ListViewExtensionMethods
	{
		internal static partial class NativeMethods
		{
			public const int LVM_SETITEMSTATE = LVM_FIRST + 43;

			public const int LVIS_SELECTED = 2;

			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
			public struct LVITEM
			{
				public int mask;
				public int iItem;
				public int iSubItem;
				public int state;
				public int stateMask;
				[MarshalAs(UnmanagedType.LPTStr)]
				public string pszText;
				public int cchTextMax;
				public int iImage;
				public IntPtr lParam;
				public int iIndent;
				public int iGroupId;
				public int cColumns;
				public IntPtr puColumns;
			}

			[DllImport("User32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
			//HandleRef
			public static extern IntPtr SendMessageLVItem(IntPtr hWnd, int msg, int wParam, ref LVITEM lvi);
		}
	}

	#region https://stackoverflow.com/a/254139 (Listview column header sorting indicator)
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal static partial class ListViewExtensionMethods
	{
		internal static partial class NativeMethods
		{
			[StructLayout(LayoutKind.Sequential)]
			public struct HDITEM
			{
				public Mask mask;
				public int cxy;
				[MarshalAs(UnmanagedType.LPTStr)]
				public string pszText;
				public IntPtr hbm;
				public int cchTextMax;
				public Format fmt;
				public IntPtr lParam;
				// _WIN32_IE >= 0x0300 
				public int iImage;
				public int iOrder;
				// _WIN32_IE >= 0x0500
				public uint type;
				public IntPtr pvFilter;
				// _WIN32_WINNT >= 0x0600
				public uint state;

				[Flags]
				public enum Mask
				{
					Format = 0x4,       // HDI_FORMAT
				};

				[Flags]
				public enum Format
				{
					SortDown = 0x200,   // HDF_SORTDOWN
					SortUp = 0x400,     // HDF_SORTUP
				};
			};

			public const int LVM_FIRST = 0x1000;
			public const int LVM_GETHEADER = LVM_FIRST + 31;

			public const int HDM_FIRST = 0x1200;
			public const int HDM_GETITEM = HDM_FIRST + 11;
			public const int HDM_SETITEM = HDM_FIRST + 12;

			[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, IntPtr lParam);

			[DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 msg, IntPtr wParam, ref HDITEM lParam);
		}

		public static void SetSortIcon(this ListView listViewControl, int columnIndex, SortOrder order)
		{
			IntPtr columnHeader = NativeMethods.SendMessage(listViewControl.Handle, NativeMethods.LVM_GETHEADER, IntPtr.Zero, IntPtr.Zero);
			for (int columnNumber = 0; columnNumber <= listViewControl.Columns.Count - 1; columnNumber++)
			{
				var columnPtr = new IntPtr(columnNumber);
				var item = new NativeMethods.HDITEM
				{
					mask = NativeMethods.HDITEM.Mask.Format
				};

				if (NativeMethods.SendMessage(columnHeader, NativeMethods.HDM_GETITEM, columnPtr, ref item) == IntPtr.Zero)
				{
					throw new Win32Exception();
				}

				if (order != SortOrder.None && columnNumber == columnIndex)
				{
					switch (order)
					{
						case SortOrder.Ascending:
							item.fmt &= ~NativeMethods.HDITEM.Format.SortDown;
							item.fmt |= NativeMethods.HDITEM.Format.SortUp;
							break;
						case SortOrder.Descending:
							item.fmt &= ~NativeMethods.HDITEM.Format.SortUp;
							item.fmt |= NativeMethods.HDITEM.Format.SortDown;
							break;
					}
				}
				else
				{
					item.fmt &= ~NativeMethods.HDITEM.Format.SortDown & ~NativeMethods.HDITEM.Format.SortUp;
				}

				if (NativeMethods.SendMessage(columnHeader, NativeMethods.HDM_SETITEM, columnPtr, ref item) == IntPtr.Zero)
				{
					throw new Win32Exception();
				}
			}
		}
	}
	#endregion
}
