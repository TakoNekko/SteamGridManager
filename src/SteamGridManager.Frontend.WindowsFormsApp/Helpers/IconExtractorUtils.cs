using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using TsudaKageyu;

namespace SteamGridManager.Frontend.WindowsFormsApp.Helpers
{
	public static class IconExtractorUtils
	{
		#region Constants

		private static readonly Dictionary<ImageFormat, string> fileExtensionByImageFormat = new Dictionary<ImageFormat, string>
		{
			{ ImageFormat.Png, ".png" },
			{ ImageFormat.Jpeg, ".jpeg" },
			{ ImageFormat.Gif, ".gif" },
			{ ImageFormat.Tiff, ".tiff" },
			{ ImageFormat.Bmp, ".bmp" },
			{ ImageFormat.Exif, ".exif" },
			{ ImageFormat.Emf, ".emf" },
			{ ImageFormat.Icon, ".ico" },
			{ ImageFormat.MemoryBmp, ".bmp" },
			{ ImageFormat.Wmf, ".wmf" },
		};

		#endregion

		#region Methods

		public static Bitmap ToVistaBitmap(this Icon icon)
		{
			try
			{
				var rawBytes = (byte[])null;

				using (var stream = new MemoryStream())
				{
					icon.Save(stream);

					rawBytes = stream.ToArray();
				}

				var entryCount = (int)BitConverter.ToInt16(rawBytes, 4);
				var offset = 0;

				// skip header.
				offset += 6;

				for (var i = 0; i < entryCount; ++i, offset += 16)
				{
					var width = (int)rawBytes[offset];
					var height = (int)rawBytes[offset + 1];

					if (width != 0 || height != 0)
					{
						continue;
					}

					var bitDepth = BitConverter.ToInt16(rawBytes, offset + 6);
					var dataSize = BitConverter.ToInt32(rawBytes, offset + 8);
					var dataOffset = BitConverter.ToInt32(rawBytes, offset + 12);

					var bitmap = (Bitmap)null;
					var bottomUp = true;

					if (bitDepth == 32)
					{
						if (HasPngSignature(rawBytes, dataOffset))
						{
							var output = new MemoryStream(rawBytes, dataOffset, dataSize, writable: false, publiclyVisible: false);

							bitmap = new Bitmap(output);

							bottomUp = false;
						}
						else
						{
							var bitmapWidth = BitConverter.ToInt32(rawBytes, dataOffset + 4);
							var bitmapHeight = BitConverter.ToInt32(rawBytes, dataOffset + 8);
							var bitCount = BitConverter.ToInt16(rawBytes, dataOffset + 14);

							if (bitmapHeight < 0)
							{
								bitmapHeight = -bitmapHeight;
								bottomUp = false;
							}
							else
							{
								bottomUp = true;
							}

							bitmap = new Bitmap(bitmapWidth, bitmapHeight, PixelFormat.Format32bppArgb);

							var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);

							try
							{
								Marshal.Copy(rawBytes, dataOffset + Marshal.SizeOf(typeof(BITMAPINFOHEADER)), bitmapData.Scan0, bitmap.Width * bitmap.Height * bitCount / 8);
							}
							finally
							{
								bitmap.UnlockBits(bitmapData);
							}

							if (bottomUp)
							{
								bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
							}
						}
					}
					else
					{
						// TODO: read XOR and 1-bit AND mask and composite them together.

						if (HasPngSignature(rawBytes, dataOffset))
						{
							Program.LogError("WARNING: support for 24-bit icon pngs is not implemented yet.");

							// TODO

							return null;
						}
						else
						{
							Program.LogError("WARNING: support for 24-bit icon bitmaps is not implemented yet.");

							// TODO

							return null;
						}
					}

					// FIXME: this is a hack... width and height don't actually have to be the same,
					// but for this app, we need to get rid of the mask, if present.
					var isMaskPresent = bitmap != null
						&& bitmap.Height == bitmap.Width * 2;

					if (isMaskPresent)
					{
						using (var temp = new Bitmap(bitmap))
						{
							bitmap = temp.Clone(new Rectangle(0, bitmap.Height / 2, bitmap.Width, bitmap.Height / 2), bitmap.PixelFormat);
						}

						// FIXME: doesn't work correctly.
						//bitmap = GetCompositeBitmap(bitmap, bottomUp);
					}

					return bitmap;
				}
			}
			catch (Exception ex)
			{
				Program.LogError(ex);
			}

			return null;
		}
		private static bool HasPngSignature(byte[] rawBytes, int dataOffset)
			=> rawBytes.Length >= dataOffset + 8
				&& BitConverter.ToInt32(rawBytes, dataOffset) is var num1
				&& num1 == 0x474E5089	//0x89 "PNG"
				&& BitConverter.ToInt32(rawBytes, dataOffset + 4) is var num2
				&& num2 == 0x0A1A0A0D;	//\r\n 0x1A \n

		[StructLayout(LayoutKind.Sequential)]
		private struct BITMAPINFOHEADER
		{
			public uint biSize;
			public int biWidth;
			public int biHeight;
			public ushort biPlanes;
			public ushort biBitCount;
			public uint biCompression;
			public uint biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public uint biClrUsed;
			public uint biClrImportant;

			public void Init()
			{
				biSize = (uint)Marshal.SizeOf(this);
			}
		}

		public static void ExtractAll(string originalFullName, string cacheIconFullName, ImageFormat imageFormat)
		{
			var convertedFullName = (string)null;
			var extractor = new IconExtractor(originalFullName);
			var largestIcon = (Icon)null;
			var largestBpp = 0;
			var iconIndex = 0;

			if (!fileExtensionByImageFormat.TryGetValue(imageFormat, out var fileExtension))
			{
				throw new ArgumentOutOfRangeException(nameof(imageFormat));
			}

			foreach (var icon in extractor.GetAllIcons())
			{
				++iconIndex;

				if (Properties.Settings.Default.IconConversion_SaveIconEntry)
				{
					convertedFullName = Path.ChangeExtension($"{Path.ChangeExtension(cacheIconFullName, null)} ({iconIndex})", fileExtension);

					var iconFullName = Path.ChangeExtension(convertedFullName, ".ico");

					using (var fileStream = File.OpenWrite(iconFullName))
					{
						icon.Save(fileStream);
					}
				}

				var bestIcon = (Icon)null;
				
				if (Properties.Settings.Default.IconConversion_UseClosest)
				{
					bestIcon = new Icon(icon, 256, 256);
				}
				else
				{
					bestIcon = icon;
				}

				using (var bitmap = bestIcon.ToBitmap())
				{
					if (largestIcon is null
						|| largestIcon.Size.Width < bestIcon.Size.Width
						|| largestIcon.Size.Height < bestIcon.Size.Height)
					{
						var bpp = GetBitsPerPixel(bitmap.PixelFormat);

						if (largestBpp > bpp)
						{
						}
						else
						{
							largestIcon = bestIcon;
							largestBpp = bpp;
						}
					}

					if (Properties.Settings.Default.IconConversion_SaveIconEntries)
					{
						convertedFullName = Path.ChangeExtension($"{Path.ChangeExtension(cacheIconFullName, null)} ({iconIndex}) ({bestIcon.Size.Width}x{bestIcon.Size.Height}x{bitmap.PixelFormat.ToString().Replace("Format", "")})", fileExtension);

						bitmap.Save(convertedFullName, imageFormat);
					}
				}
			}

			if (largestIcon != null)
			{
				convertedFullName = Path.ChangeExtension(cacheIconFullName, fileExtension);

				using (var largestBitmap = largestIcon.ToVistaBitmap() ?? largestIcon.ToBitmap())
				{
					largestBitmap.Save(convertedFullName, imageFormat);
				}
			}
		}

		#endregion

		#region Implementation

		private static int GetBitsPerPixel(PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case PixelFormat.Format16bppArgb1555:
				case PixelFormat.Format16bppGrayScale:
				case PixelFormat.Format16bppRgb555:
				case PixelFormat.Format16bppRgb565:
					return 16;
				case PixelFormat.Format1bppIndexed:
					return 1;
				case PixelFormat.Format24bppRgb:
					return 24;
				case PixelFormat.Format32bppArgb:
				case PixelFormat.Format32bppPArgb:
				case PixelFormat.Format32bppRgb:
					return 32;
				case PixelFormat.Format48bppRgb:
					return 48;
				case PixelFormat.Format4bppIndexed:
					return 4;
				case PixelFormat.Format64bppArgb:
				case PixelFormat.Format64bppPArgb:
					return 64;
				case PixelFormat.Format8bppIndexed:
					return 8;
				default:
					return 0;
			}
		}

		#endregion
	}
}
