using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;

namespace SteamGridManager
{
	public class ExternalApplication : INotifyPropertyChanged
	{
		#region Events

		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		#endregion

		#region Properties

		private string title = "";

		public string Title
		{
			get => title;
			set
			{
				if (title != value)
				{
					title = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
				}
			}
		}


		private string location = "";

		public string Location
		{
			get => location;
			set
			{
				if (location != value)
				{
					location = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Location)));
				}
			}
		}


		private string arguments = "";

		public string Arguments
		{
			get => arguments;
			set
			{
				if (arguments != value)
				{
					arguments = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Arguments)));
				}
			}
		}


		private string startingDirectory = "";

		public string StartingDirectory
		{
			get => startingDirectory;
			set
			{
				if (startingDirectory != value)
				{
					startingDirectory = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(StartingDirectory)));
				}
			}
		}


		private string verb = "Open";

		public string Verb
		{
			get => verb;
			set
			{
				if (verb != value)
				{
					verb = value;
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Verb)));
				}
			}
		}


		private List<string> assetTypeNames = new List<string>
		{
			"none",
			"capsule",
			"hero",
			"logo",
			"header",
			"icon",
		};

		public IReadOnlyList<string> AssetTypeNames
		{
			get => assetTypeNames;
			set
			{
				if (assetTypeNames != value)
				{
					assetTypeNames = new List<string>(value);
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(AssetTypeNames)));
				}
			}
		}

		#endregion

		#region Class Overrides

		public override string ToString()
		{
			const string separator = "=";
			var sb = new StringBuilder();

			sb.AppendLine($"{nameof(Title)}{separator}{Title}");
			sb.AppendLine($"{nameof(Location)}{separator}{Location}");
			sb.AppendLine($"{nameof(Arguments)}{separator}{Arguments}");
			sb.AppendLine($"{nameof(StartingDirectory)}{separator}{StartingDirectory}");
			sb.AppendLine($"{nameof(Verb)}{separator}{Verb}");
			sb.AppendLine($"{nameof(AssetTypeNames)}{separator}{string.Join(",", AssetTypeNames)}");

			return sb.ToString();
		}

		#endregion

		#region Methods

		public ExternalApplication Clone()
			=> new ExternalApplication
				{
					Title = title,
					Location = location,
					Arguments = arguments,
					StartingDirectory = startingDirectory,
					Verb = verb,
					AssetTypeNames = new List<string>(assetTypeNames)
				};

		public static bool TryParse(string source, out ExternalApplication value)
		{
			try
			{
				value = Parse(source);
			}
			catch (Exception ex)
			{
				Frontend.WindowsFormsApp.Program.LogError(ex);
				value = null;
				return false;
			}

			return true;
		}

		public static ExternalApplication Parse(string source)
		{
			var value = new ExternalApplication { };
			var lines = source.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);

			foreach (var line in lines)
			{
				var match = Regex.Match(line, "^(?<Key>[@a-zA-Z_][a-zA-Z0-9_]*)=(?<Value>.*)$");

				if (!match.Success)
				{
					throw new Exception($"\"{line}\" is not a valid key value pair.");
				}

				var propertyName = match.Groups["Key"].Value;
				var propertyValue = match.Groups["Value"].Value;

				switch (propertyName)
				{
					case nameof(Title):
						value.Title = propertyValue;
						break;

					case nameof(Location):
						value.Location = propertyValue;
						break;

					case nameof(Arguments):
						value.Arguments = propertyValue;
						break;

					case nameof(StartingDirectory):
						value.StartingDirectory = propertyValue;
						break;

					case nameof(Verb):
						value.Verb = propertyValue;
						break;

					case nameof(AssetTypeNames):
						value.AssetTypeNames = propertyValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
						break;

					default:
						throw new ArgumentOutOfRangeException(propertyName, $"{propertyName} is not a valid property name.");
				}
			}

			return value;
		}

		#endregion
	}
}
