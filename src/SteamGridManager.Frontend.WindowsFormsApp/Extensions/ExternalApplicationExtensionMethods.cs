using Steam.Vdf;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace SteamGridManager.Frontend.WindowsFormsApp.Extensions.ExternalApplication
{
	using ExternalApplication = SteamGridManager.ExternalApplication;

	public static class ExternalApplicationExtensionMethods
	{
		public static void Start(this ExternalApplication application, AppInfos appInfos, Shortcuts shortcuts, ulong? appID, string path, AssetType? assetType)
		{
			var cancellationPending = false;
			var appInfo = (AppInfo)null;
			var shortcut = (Shortcut)null;
			var regex = new Regex(@"\${([@\w/\-:]+)}");
			var startInfo = new ProcessStartInfo(Format(application.Location), Format(application.Arguments))
			{
				//Verb = application.Verb,
				WorkingDirectory = Format(application.StartingDirectory),
			};

			if (cancellationPending)
			{
				return;
			}

			try
			{
				using (var process = Process.Start(startInfo))
				{
				}
			}
			catch (Exception ex)
			{
				Program.LogError(ex);
			}

			bool RequestAppOrShortcutInfo()
			{
				if (appInfo is null
					&& appID != null
					&& appInfos != null
					&& appInfos.Items.TryGetValue(appID.Value, out appInfo))
				{
					return true;
				}
				else if (shortcut is null
					&& appID != null
					&& shortcuts != null
					&& shortcuts.Items.TryGetValue(appID.Value, out shortcut))
				{
					return true;
				}

				return false;
			}

			string Format(string format)
			{
				if (cancellationPending)
				{
					return "";
				}

				return regex.Replace(format, (match) =>
				{
					var identifier = match.Groups[1].Value;

					if (identifier.StartsWith("vdf:", System.StringComparison.OrdinalIgnoreCase))
					{
						identifier = identifier.Substring("vdf:".Length);

						RequestAppOrShortcutInfo();

						var vdfValue = (object)null;

						if (appInfo != null)
						{
							vdfValue = (string)appInfo?.Data?.Node?.FindValueByPath(identifier);
						}
						else if (shortcut != null)
						{
							vdfValue = (string)shortcut?.Node?.FindValueByPath(identifier);
						}

						if (vdfValue is VdfObject vdfObjectValue)
						{
							// TODO?
							return $"{vdfObjectValue}";
						}
						else if (vdfValue is string stringValue)
						{
							return stringValue;
						}
						else if (vdfValue is uint uintValue)
						{
							return uintValue.ToString(CultureInfo.InvariantCulture);
						}
					}
					else
					{
						switch (identifier)
						{
							case "AppID":
								return appID.Value.ToString(CultureInfo.InvariantCulture);
							case "AppName":
								{
									RequestAppOrShortcutInfo();

									var appName = "";

									if (appInfo != null)
									{
										appName = (string)appInfo?.Data?.Node?.FindValueByPath("appinfo/common/name");
									}
									else if (shortcut != null)
									{
										appName = (string)shortcut?.Node?.FindValueByPath("appname");
									}

									return appName;
								}
							case "SteamUserID":
								return Properties.Settings.Default.SteamUserID.ToString(CultureInfo.InvariantCulture);
							case "AssetType":
								{
									var assetTypeName = "";

									if (assetType != null
										&& application.AssetTypeNames != null
										&& application.AssetTypeNames.Count > (int)assetType.Value)
									{
										assetTypeName = application.AssetTypeNames[(int)assetType.Value];
									}

									return assetTypeName;
								}
							case "FullName":
								return path;
							case "FileName":
								return Path.GetFileName(path);
							case "FileTitle":
								return Path.GetFileNameWithoutExtension(path);
							case "FileExtension":
								return Path.GetExtension(path);
							case "DirectoryName":
								return Path.GetDirectoryName(path);
							default:
								using (var dialog = new ComboBoxDialog
								{
								})
								{
									dialog.Label.Text = $"Please enter a value for '{identifier}'";

									if (dialog.ShowDialog(System.Windows.Forms.Form.ActiveForm) == System.Windows.Forms.DialogResult.OK)
									{
										return (string)dialog.SelectedValue;
									}
									else
									{
										cancellationPending = true;
									}
								}

								return identifier;
						}
					}

					return match.Value;
				});
			}
		}
	}
}
