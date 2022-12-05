using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SteamGridManager
{
	public class AssetOverlayCollection
	{
		#region Constants

		private static readonly string[] validImageFileExtensions = new string[]
		{
			".png",
			".jpg", ".jpeg", ".jpe", ".jif", ".jfif",
			".gif",
			".bmp",
			".tiff", ".tif",
			".emf",
		};

		#endregion

		#region Properties

		private readonly Dictionary<AssetType, List<AssetOverlay>> overlays = new Dictionary<AssetType, List<AssetOverlay>>();

		public IReadOnlyDictionary<AssetType, List<AssetOverlay>> Items => overlays;

		#endregion

		#region Methods

		public void FetchAvailableOverlayFiles()
		{
			overlays.Clear();

			foreach (var enumName in Enum.GetNames(typeof(AssetType)))
			{
				var enumValue = (AssetType)Enum.Parse(typeof(AssetType), enumName);

				overlays.Add(enumValue, new List<AssetOverlay>());

				var path = GetOverlayCategoryPath(enumValue);

				if (!Directory.Exists(path))
				{
					continue;
				}

				foreach (var overlayFullName in Directory.EnumerateDirectories(path, "*.*", SearchOption.TopDirectoryOnly))
				{
					var overlay = new AssetOverlay
					{
						Name = Path.GetFileName(overlayFullName),
						Variants = FetchOverlayVariants(overlayFullName),
					};

					overlays[enumValue].Add(overlay);
				}
			}
		}

		private static List<AssetOverlayVariant> FetchOverlayVariants(string path)
		{
			var variants = new List<AssetOverlayVariant>();

			foreach (var variantFullName in Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly))
			{
				var fileTitle = Path.GetFileNameWithoutExtension(variantFullName);
				var fileExtension = Path.GetExtension(variantFullName);

				if (!validImageFileExtensions.Contains(fileExtension.ToLower()))
				{
					continue;
				}

				//(?<Name>\\w*)\\s*[_-]?\\s*
				var match = Regex.Match(fileTitle, "^(?<Width>\\d+)\\s*[x_-]?\\s*(?<Height>\\d+)$");

				if (!match.Success)
				{
					continue;
				}

				var variant = new AssetOverlayVariant
				{
					FullName = variantFullName,
					//Name = match.Groups["Name"].Value,
					Width = int.Parse(match.Groups["Width"].Value),
					Height = int.Parse(match.Groups["Height"].Value),
				};

				variants.Add(variant);
			}

			return variants;
		}

		public void GetAsync(AssetType key, string name, int width, int height, Action<Image> set)
		{
			if (string.IsNullOrEmpty(name))
			{
				set?.Invoke(null);
				return;
			}

			if (overlays.TryGetValue(key, out var overlay))
			{
				var index = overlay.FindIndex(x => x.Name.Equals(name));

				if (index == -1)
				{
					return;
				}

				var item = overlay[index];

				var bestVariant = FindBestVariant(item.Variants, width, height);

				if (bestVariant.Image != null)
				{
					set?.Invoke(bestVariant.Image);
					return;
				}

				try
				{
					// TODO: load async.

					var image = Image.FromFile(bestVariant.FullName);

					bestVariant.Image = image;

					if (image != null)
					{
						set?.Invoke(image);
					}
				}
				catch { }
			}
		}

		public AssetOverlayVariant FindBestVariant(IEnumerable<AssetOverlayVariant> variants, int width, int height)
		{
			var bestMatch = (AssetOverlayVariant)null;

			foreach (var variant in variants)
			{
				// TODO: take aspect ratio into account?

				if (width == variant.Width
					&& height == variant.Height)
				{
					return variant;
				}
				else if (bestMatch is null)
				{
					bestMatch = variant;
				}
				else  if (Math.Abs(width - variant.Width) < Math.Abs(width - bestMatch.Width))
				{
					bestMatch = variant;
				}
			}

			return bestMatch;
		}

		public string GetOverlayPath(AssetType key, string name)
			=> Path.Combine(
					OverlaysBasePath,
					key.ToString(),
					name
				);

		public string GetOverlayCategoryPath(AssetType key)
			=> Path.Combine(
					OverlaysBasePath,
					key.ToString()
				);

		public string OverlaysBasePath
			=> Path.Combine(
					Application.StartupPath,
					"assets",
					"images",
					"overlays"
				);

		#endregion
	}
}
