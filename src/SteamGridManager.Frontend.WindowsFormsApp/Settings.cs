using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace SteamGridManager.Frontend.WindowsFormsApp.Properties
{
	internal sealed partial class Settings
	{
		#region Constructors

		public Settings()
		{
			SettingsLoaded += Settings_SettingsLoaded;
			SettingsSaving += Settings_SettingsSaving;
		}

		#endregion

		#region Event Handlers

		private void Settings_SettingsSaving(object sender, CancelEventArgs e)
		{
			Default.IsReady = false;

			SerializeExternalApplications();
			SerializeToggles();
			SerializeCustomColors();
		}

		private void Settings_SettingsLoaded(object sender, SettingsLoadedEventArgs e)
		{
			DeserializeExternalApplications();
			DeserializeToggles();
			DeserializeCustomColors();
		}

		#endregion

		#region Implementation

		private static void SerializeExternalApplications()
		{
			if (Default.ExternalApplications is null)
			{
				Default.ExternalApplications = new StringCollection();
			}

			Default.ExternalApplications.Clear();
			Default.ExternalApplications.AddRange(ExternalApplicationCollection.Instance.Items.Select(item => item.ToString()).ToArray());
		}

		private static void DeserializeExternalApplications()
		{
			if (Default.ExternalApplications is null)
			{
				Default.ExternalApplications = new StringCollection();
			}

			ExternalApplicationCollection.Instance.Items.Clear();

			foreach (var source in Default.ExternalApplications)
			{
				if (ExternalApplication.TryParse(source, out var value))
				{
					ExternalApplicationCollection.Instance.Items.Add(value);
				}
			}
		}

		private static void SerializeToggles()
		{
			if (Default.Toggles is null)
			{
				Default.Toggles = new StringCollection();
			}

			Default.Toggles.Clear();
			Default.Toggles.AddRange(ToggleCollection.Instance.Items.Select(item => $"{item.Key}={item.Value}").ToArray());
		}

		private static void DeserializeToggles()
		{
			if (Default.Toggles is null)
			{
				Default.Toggles = new StringCollection();
			}

			ToggleCollection.Instance.Clear();

			foreach (var source in Default.Toggles)
			{
				if (TryParseKeyValuePair(source, out var value))
				{
					ToggleCollection.Instance.Add(value.Key, value.Value);
				}
			}
		}

		private static bool TryParseKeyValuePair(string source, out KeyValuePair<string, bool> value)
		{
			if (string.IsNullOrEmpty(source))
			{
				Program.LogError($"Settings: null or empty string.");
				value = default;
				return false;
			}

			var tokens = source.Split(new char[] { '=' }, 2);

			if (tokens.Length != 2)
			{
				Program.LogError($"Settings: Invalid key value pair. Got {tokens.Length}. Expected {2}.");
				value = default;
				return false;
			}

			if (!bool.TryParse(tokens[1], out var boolValue))
			{
				Program.LogError($"Settings: Invalid value. Got {tokens[1]}. Expected a boolean string.");
				value = default;
				return false;
			}

			value = new KeyValuePair<string, bool>(tokens[0], boolValue);
			return true;
		}

		private static void SerializeCustomColors()
		{
			if (Default.CustomColors is null)
			{
				Default.CustomColors = new StringCollection();
			}

			Default.CustomColors.Clear();
			Default.CustomColors.AddRange(CustomColorsCollection.Instance.Items.Select(item => $"{item.Key}={string.Join(",", item.Value.Select(x => x.ToArgb().ToString(CultureInfo.InvariantCulture)))}").ToArray());
		}

		private static void DeserializeCustomColors()
		{
			if (Default.CustomColors is null)
			{
				Default.CustomColors = new StringCollection();
			}

			CustomColorsCollection.Instance.Clear();

			foreach (var source in Default.CustomColors)
			{
				if (TryParseColors(source, out var value))
				{
					CustomColorsCollection.Instance.Add(value.Key, value.Value);
				}
			}
		}

		private static bool TryParseColors(string source, out KeyValuePair<string, Color[]> value)
		{
			if (string.IsNullOrEmpty(source))
			{
				Program.LogError($"Settings: null or empty string.");
				value = default;
				return false;
			}

			var tokens = source.Split(new char[] { '=' }, 2);

			if (tokens.Length != 2)
			{
				Program.LogError($"Settings: Invalid key value pair. Got {tokens.Length}. Expected {2}.");
				value = default;
				return false;
			}

			var colorTokens = tokens[1].Split(new char[] { ',' });
			var list = new List<Color>();

			foreach (var colorToken in colorTokens)
			{
				//var namedColor = Color.FromName(color);
				if (!int.TryParse(colorToken.Trim(), out var intValue))
				{
					Program.LogError($"Settings: Invalid color value. Got {colorToken}. Expected an unsigned integer.");
					continue;
				}

				list.Add(Color.FromArgb(intValue));
			}

			value = new KeyValuePair<string, Color[]>(tokens[0], list.ToArray());
			return true;
		}


		#endregion
	}
}
