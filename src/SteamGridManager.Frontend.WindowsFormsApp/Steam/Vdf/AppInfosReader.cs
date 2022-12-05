using System.Collections.Generic;
using System.IO;

namespace Steam.Vdf
{
	public class AppInfosReader
	{
		private readonly List<AppInfoData> items = new List<AppInfoData>();
		public IReadOnlyList<AppInfoData> Items => items;

		public uint Universe { get; }

		public AppInfosReader(string path)
		{
			const long MinimumFileLength = 56;
			const uint KnownSignature = 0x07564427;

			using (var fileStream = File.OpenRead(path))
			{
				if (fileStream.Length < MinimumFileLength)
				{
					throw new InvalidDataException($"Invalid file length. Got {fileStream.Length} bytes. Expected at least {MinimumFileLength} bytes.");
				}

				var vdfReader = new VdfReader(fileStream, leaveOpen: true);

				using (var binaryReader = new BinaryReader(fileStream))
				{
					var signature = binaryReader.ReadUInt32();

					if (signature != KnownSignature)
					{
						throw new InvalidDataException($"Invalid file signature. Got 0x{signature:x8}. Expected 0x{KnownSignature:x8}.");
					}

					Universe = binaryReader.ReadUInt32();

					do
					{
						var appID = binaryReader.ReadUInt32();

						if (appID == 0)
						{
							break;
						}

						var item = new AppInfoData
						{
							AppID = appID,
							Size = binaryReader.ReadUInt32(),
							InfoState = binaryReader.ReadUInt32(),
							LastUpdated = binaryReader.ReadUInt32(),
							PicsToken = binaryReader.ReadUInt64(),
							Hash = binaryReader.ReadBytes(20),
							ChangeNumber = binaryReader.ReadUInt32(),
							Node = vdfReader.ReadObject(),
						};

						items.Add(item);
					} while (true);
				}
			}
		}
	}
}
