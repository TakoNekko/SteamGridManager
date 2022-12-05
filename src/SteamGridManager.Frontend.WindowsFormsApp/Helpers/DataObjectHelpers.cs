using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SteamGridManager.Frontend.WindowsFormsApp.Helpers
{
	using Extensions;

	public static class DataObjectHelpers
	{
		public static bool TryGetAppId(IDataObject dataObject, out ulong value)
		{
			value = 0;

			if (dataObject is null)
			{
				return false;
			}

			const string format = "Chromium Web Custom MIME Data Format";

			if (!dataObject.GetDataPresent(format))
			{
				//Program.LogError("no data present");
				return false;
			}

			var dataValue = dataObject.GetData(format);

			if (dataValue is null)
			{
				//Program.LogError("could not get data");
				return false;
			}

			var memoryStream = dataValue as MemoryStream;

			if (memoryStream is null)
			{
				//Program.LogError("could not convert");
				return false;
			}

			using (var binaryReader = new BinaryReader(memoryStream))
			{
				var pickleSize = binaryReader.ReadUInt32();
				var pickleCount = binaryReader.ReadUInt32();

				if (pickleCount < 1)
				{
					//Program.LogError("no pickles");
					return false;
				}

				var keyLength = binaryReader.ReadUInt32();
				var keyData = binaryReader.ReadPickleStringUtf16(keyLength);

				if (!keyData.Equals("text/appids"))
				{
					//Program.LogError("key data isn't text/appids");
					return false;
				}

				var valueLength = binaryReader.ReadUInt32();
				var valueData = binaryReader.ReadPickleStringUtf16(valueLength);

				if (!ulong.TryParse(valueData, out value))
				{
					//Program.LogError("pickle: could not cast to ulong.");
					return false;
				}

				return true;
			}
		}
	}

	namespace Extensions
	{
		internal static partial class ExtensionMethods
		{
			public static string ReadPickleStringUtf16(this BinaryReader binaryReader, uint length)
			{
				var padding = length > 4 && ((length % 4) != 0) ? 2 : 0;
				var bytes = binaryReader.ReadBytes((int)(length * 2 + padding));

				return Encoding.Unicode.GetString(bytes, 0, (int)length * 2);
			}
		}
	}
}
