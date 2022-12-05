using Steam.Vdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Helpers
{
	public static partial class VdfUtils
	{
		public static VdfPropertyType GetVdfPropertyTypeByPath(string path)
		{
			if (Properties.Settings.Default.VdfPropertyType_Object_KnownPaths.Contains(path))
			{
				return VdfPropertyType.Object;
			}
			else if (Properties.Settings.Default.VdfPropertyType_String_KnownPaths.Contains(path))
			{
				return VdfPropertyType.String;
			}
			else if (Properties.Settings.Default.VdfPropertyType_UInt_KnownPaths.Contains(path))
			{
				return VdfPropertyType.UInt;
			}
			else if (Properties.Settings.Default.VdfPropertyType_Boolean_KnownPaths.Contains(path))
			{
				return VdfPropertyType.Boolean;
			}
			else if (Properties.Settings.Default.VdfPropertyType_RelativeTime_KnownPaths.Contains(path))
			{
				return VdfPropertyType.RelativeTime;
			}
			// TODO: use VdfEnumCollection instead.
			else if (Properties.Settings.Default.VdfPropertyType_Enum_KnownPaths.Contains(path))
			{
				return VdfPropertyType.Enum;
			}

			return VdfPropertyType.None;
		}

		public static string FormatVdfValue(VdfPropertyType propertyType, object value)
		{
			var content = (string)null;

			switch (propertyType)
			{
				case VdfPropertyType.Object:
					{
						if (value is VdfObject vdfObjectValue)
						{
							var sb = new StringBuilder();
							var acfWriter = new AcfWriter();

							acfWriter.Write(sb, vdfObjectValue);
							content = sb.ToString();
						}
					}
					break;
				case VdfPropertyType.String:
					{
						if (value is string stringValue)
						{
							content = stringValue;
						}
					}
					break;
				case VdfPropertyType.UInt:
					{
						if (value is uint uintValue)
						{
							var cultureInfo = GetCultureInfo(Properties.Settings.Default.VdfDefinition_UInt_Culture);

							content = cultureInfo is null ? uintValue.ToString() : uintValue.ToString(cultureInfo);
						}
					}
					break;
				case VdfPropertyType.Boolean:
					{
						if (value is uint uintValue)
						{
							if (Properties.Settings.Default.VdfDefinition_Boolean_FormatAsString)
							{
								content = uintValue == 0 ? Properties.Settings.Default.VdfDefinition_Boolean_FalseString : Properties.Settings.Default.VdfDefinition_Boolean_TrueString;
							}
							else
							{
								content = uintValue.ToString(CultureInfo.InvariantCulture);
							}
						}
					}
					break;
				case VdfPropertyType.RelativeTime:
					{
						if (value is uint uintValue)
						{
							var cultureInfo = GetCultureInfo(Properties.Settings.Default.VdfDefinition_RelativeTime_Culture);
							var relativeTime = uintValue.ToRelativeTime();

							content = string.IsNullOrEmpty(Properties.Settings.Default.VdfDefinition_RelativeTime_Format)
								? relativeTime.ToString() : relativeTime.ToString(Properties.Settings.Default.VdfDefinition_RelativeTime_Format, cultureInfo);
						}
					}
					break;
				case VdfPropertyType.Enum:
					{
						if (value is uint uintValue)
						{
							// TODO
						}
					}
					break;
				default:
					content = value?.ToString();
					break;
			}

			return content;
		}

		public static CultureInfo GetCultureInfo(VdfDefinitionUIntCulture cultureType)
		{
			var cultureInfo = (CultureInfo)null;

			switch (cultureType)
			{
				case VdfDefinitionUIntCulture.Invariant:
					cultureInfo = CultureInfo.InvariantCulture;
					break;
				case VdfDefinitionUIntCulture.Current:
					cultureInfo = CultureInfo.CurrentCulture;
					break;
				case VdfDefinitionUIntCulture.CurrentUI:
					cultureInfo = CultureInfo.CurrentUICulture;
					break;
				case VdfDefinitionUIntCulture.DefaultThreadCurrent:
					cultureInfo = CultureInfo.DefaultThreadCurrentCulture;
					break;
				case VdfDefinitionUIntCulture.DefaultThreadCurrentUI:
					cultureInfo = CultureInfo.DefaultThreadCurrentUICulture;
					break;
				case VdfDefinitionUIntCulture.InstalledUI:
					cultureInfo = CultureInfo.InstalledUICulture;
					break;
			}

			return cultureInfo;
		}

		public static Color? GetColor(VdfPropertyType propertyType, object value)
		{
			var color = (Color?)null;

			if (value is VdfObject)
			{
				color = Properties.Settings.Default.VdfDefinition_Object_Color;
			}
			else if (value is string)
			{
				color = Properties.Settings.Default.VdfDefinition_String_Color;
			}
			else if (value is uint)
			{
				color = Properties.Settings.Default.VdfDefinition_UInt_Color;
			}

			switch (propertyType)
			{
				case VdfPropertyType.Object:
					color = Properties.Settings.Default.VdfDefinition_Object_Color;
					break;
				case VdfPropertyType.String:
					color = Properties.Settings.Default.VdfDefinition_String_Color;
					break;
				case VdfPropertyType.UInt:
					color = Properties.Settings.Default.VdfDefinition_UInt_Color;
					break;
				case VdfPropertyType.Boolean:
					color = Properties.Settings.Default.VdfDefinition_Boolean_Color;
					break;
				case VdfPropertyType.RelativeTime:
					color = Properties.Settings.Default.VdfDefinition_RelativeTime_Color;
					break;
				case VdfPropertyType.Enum:
					color = Properties.Settings.Default.VdfDefinition_Enum_Color;
					break;
			}

			return color;
		}

		public static void PopulateMenuWithVdfFilter(IEnumerable<string> filters, ToolStripMenuItem after, Action<string> onClick, Func<string, bool> check)
		{
			foreach (var filter in filters)
			{
				if (string.IsNullOrEmpty(filter))
				{
					continue;
				}

				var paths = filter.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);

				foreach (var path in paths)
				{
					if (string.IsNullOrEmpty(path))
					{
						continue;
					}

					var parts = path.Split(new char[] { '/' });

					BuildVdfColumnMenu(parts, after, onClick, check);

					if (!Properties.Settings.Default.List_Menu_VdfColumn_ShowAllFilters)
					{
						break;
					}
				}
			}
		}

		private static void BuildVdfColumnMenu(string[] parts, ToolStripMenuItem menu, Action<string> onClick, Func<string, bool> check)
		{
			var parentMenu = menu;
			var depth = 0;

			foreach (var part in parts)
			{
				++depth;
				var matches = parentMenu.DropDownItems.Find(part, searchAllChildren: false);

				if (matches is null || matches.Length == 0)
				{
					var fullPath = Path.Combine(parts.TakeWhile((element, index) => index < depth).ToArray()).Replace('\\', '/');

					var menuItem = new ToolStripMenuItem//(part, image: null, onClick: (sender, e) => { })
					{
						Text = part,
						Name = part,
						Tag = fullPath,//depth,
						//CheckOnClick = true,
						Checked = check.Invoke(fullPath)
						//Enabled = shouldEnable.Invoke(part),
					};

					//if (Properties.Settings.Default.List_Menu_VdfColumn_AllowSelectPath
					//	|| depth == parts.Length)
					{
						menuItem.Click += toolStripMenuItem_Click;
					}

					parentMenu.DropDownItems.Add(menuItem);

					parentMenu = menuItem;
				}
				else
				{
					parentMenu = matches[0] as ToolStripMenuItem;
				}
			}

			void toolStripMenuItem_Click(object sender, EventArgs e)
			{
				var toolStripMenuItem = (ToolStripMenuItem)sender;

				if (!Properties.Settings.Default.List_Menu_VdfColumn_AllowSelectPath
					&& toolStripMenuItem.HasDropDownItems)
				{
					return;
				}
				/*
				//var _depth = (int)toolStripMenuItem.Tag;
				var fullPath = toolStripMenuItem.Text;
				var parent = toolStripMenuItem.OwnerItem;

				for (var i = 0; i < _depth; ++i)
				{
					fullPath = Path.Combine(parent.Text, fullPath).Replace('\\', '/');
					parent = parent.OwnerItem;
				}
				*/
				var fullPath = (string)toolStripMenuItem.Tag;

				onClick.Invoke(fullPath);

				toolStripMenuItem.Checked = !toolStripMenuItem.Checked;

				/*
				var value = toolStripMenuItem.Checked;

				for (var node = toolStripMenuItem.OwnerItem as ToolStripMenuItem; node != null; node = toolStripMenuItem.OwnerItem as ToolStripMenuItem)
				{
					if (!(node.Tag is string))
					{
						break;
					}

					node.Checked = value;
				}
				*/
			}
		}
	}

	public enum VdfPropertyType
	{
		None,
		Object,
		String,
		UInt,
		Boolean,
		RelativeTime,
		Enum,
	}
}
