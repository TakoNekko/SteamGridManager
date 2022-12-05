using System;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SteamGridManager.Frontend.WindowsFormsApp.Extensions.Control
{
	using Control = System.Windows.Forms.Control;
	using NativeMethods;

	internal static partial class ControlExtensionMethods
	{
		public static void ApplyWindowTheme(this Control control)
		{
			if (!string.IsNullOrEmpty(Properties.Settings.Default.WindowTheme))
			{
				SetWindowThemeRecursive(control, subAppName: Properties.Settings.Default.WindowTheme, subIDList: null);
			}
		}

		public static void ResetWindowTheme(this Control control)
			=> SetWindowThemeRecursive(control, subAppName: null, subIDList: null);

		public static void UseExplorerWindowTheme(this Control control)
			=> SetWindowThemeRecursive(control, subAppName: "Explorer", subIDList: null);

		private static void SetWindowThemeRecursive(this Control control, string subAppName, string subIDList)
		{
			if (control is null) throw new ArgumentNullException(nameof(control));

			UxTheme.NativeMethods.SetWindowTheme(control.Handle, subAppName, subIDList);

			foreach (Control child in control.Controls)
			{
				SetWindowThemeRecursive(child, subAppName, subIDList);
			}
		}
	}

	internal static partial class ControlExtensionMethods
	{
		public static void SetDoubleBuffered(this Control control, bool enabled)
		{
			if (control is null) throw new ArgumentNullException(nameof(control));

			typeof(Control)
				.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
				.SetValue(control, enabled, new object[] { });
		}

		public static void SetStylePublic(this Control control, ControlStyles styles, bool enabled)
		{
			if (control is null) throw new ArgumentNullException(nameof(control));

			typeof(Control)
				.GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic)
				.Invoke(control, new object[] { styles, enabled });
		}
	}

	namespace NativeMethods
	{
		internal static partial class UxTheme
		{
			internal static partial class NativeMethods
			{
				[DllImport("UxTheme.dll", CharSet = CharSet.Unicode)]
				public static extern int SetWindowTheme([In] IntPtr hWnd, [In] string pszSubAppName, [In] string pszSubIdList);
			}
		}
	}
}
