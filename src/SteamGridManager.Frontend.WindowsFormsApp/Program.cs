using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			if (Environment.GetCommandLineArgs().Contains("--console"))
			{
				if (!Kernel32.NativeMethods.AllocConsole())
				{
					//NativeMethods.AttachConsole(NativeMethods.ATTACH_PARENT_PROCESS);
				}

				Console.OutputEncoding = Encoding.UTF8;
			}

			try
			{
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new MainForm());
			}
			catch (Exception ex)
			{
				LogError(ex);

				if (Kernel32.NativeMethods.GetConsoleWindow() != IntPtr.Zero)
				{
					Console.WriteLine();
					Console.WriteLine("Press any key to exit.");
					Console.ReadKey();
				}
			}
		}

		public static void LogError(object message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.WriteLine(message);
			Console.ResetColor();
		}

		private static partial class Kernel32
		{
			internal static partial class NativeMethods
			{
				public const int ATTACH_PARENT_PROCESS = -1;

				public const int ERROR_ACCESS_DENIED = 0x5;

				public const int ERROR_INVALID_HANDLE = 0x6;

				public const int ERROR_INVALID_PARAMETER = 0x57;

				[DllImport("Kernel32.dll", SetLastError = true)]
				[return: MarshalAs(UnmanagedType.Bool)]
				public static extern bool AttachConsole(int dwProcessId);

				[DllImport("Kernel32.dll", SetLastError = true)]
				[return: MarshalAs(UnmanagedType.Bool)]
				public static extern bool AllocConsole();

				[DllImport("Kernel32.dll", SetLastError = true)]
				public static extern IntPtr GetConsoleWindow();
			}
		}
	}
}
