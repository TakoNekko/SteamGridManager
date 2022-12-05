using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Helpers
{
	public static class ExternalApplicationUtils
	{
		public static string GetFormat(string format)
			=> $"${{{format}}}";

		public static void PopulateExternalApplicationsSubMenu(IEnumerable<ExternalApplication> externalApplications, ToolStripMenuItem targetMenu, string verb, Action<ExternalApplication> start)
		{
			var separator = (ToolStripSeparator)null;

			for (var i = targetMenu.DropDownItems.Count; i > 0; --i)
			{
				var item = targetMenu.DropDownItems[i - 1];

				if (item is ToolStripSeparator separatorItem)
				{
					separator = separatorItem;
				}
				else if (item.Tag is ExternalApplication)
				{
					targetMenu.DropDownItems.RemoveAt(i - 1);
				}
			}

			foreach (var application in externalApplications)
			{
				var menuItem = new ToolStripMenuItem(application.Title, image: null, onClick: (sender, e) => { start?.Invoke(application); })
				{
					Tag = application
				};

				if (string.IsNullOrEmpty(application.Verb)
					|| !application.Verb.Equals(verb, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

				targetMenu.DropDownItems.Add(menuItem);
			}

			// i.e., 2 = default program + separator.
			if (separator != null)
			{
				separator.Visible = targetMenu.DropDownItems.Count > 2;
			}
		}
	}
}
