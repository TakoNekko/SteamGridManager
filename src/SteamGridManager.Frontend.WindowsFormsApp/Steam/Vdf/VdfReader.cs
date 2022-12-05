using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Steam.Vdf
{
	public class VdfReader
	{
		public Stream Stream { get; }
		public BinaryReader BinaryReader { get; }
		private bool LeaveOpen { get; }

		public VdfReader(Stream stream)
			: this(stream, leaveOpen: false) { }

		public VdfReader(Stream stream, bool leaveOpen)
		{
			Stream = stream;
			LeaveOpen = leaveOpen;
		}

		public IList<VdfObject> ReadObjects()
		{
			var items = new List<VdfObject>();

			using (var binaryReader = new BinaryReader(Stream, Encoding.Default, leaveOpen: LeaveOpen))
			{
				do
				{
					var entry = ReadObjectRecursive(binaryReader);

					if (entry is null)
					{
						break;
					}

					items.Add(entry);
				} while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length);
			}

			return items;
		}

		public VdfObject ReadObject()
		{
			using (var binaryReader = new BinaryReader(Stream, Encoding.Default, leaveOpen: LeaveOpen))
			{
				var item = ReadObjectRecursive(binaryReader);

				return item;
			}
		}

		private VdfObject ReadObjectRecursive(BinaryReader binaryReader)
		{
			var item = (VdfObject)null;

			do
			{
				var type = binaryReader.ReadByte();

				if (type == 8)
				{
					break;
				}

				var key = ReadString(binaryReader);
				var value = (object)null;

				switch (type)
				{
					case 0:
						value = ReadObjectRecursive(binaryReader);
						break;

					case 1:
						value = ReadString(binaryReader);
						break;

					case 2:
						value = ReadUInt32(binaryReader);
						break;

					default:
						throw new ArgumentOutOfRangeException($"{type.ToString(CultureInfo.InvariantCulture)} is not a valid type.");
				}

				if (item is null)
				{
					item = new VdfObject();
				}

				item[key] = value;
			} while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length);

			return item;
		}

		private uint ReadUInt32(BinaryReader binaryReader)
		{
			return binaryReader.ReadUInt32();
		}

		private string ReadString(BinaryReader binaryReader)
		{
			// TODO: block-buffered reads.

			var bytes = new List<byte>();

			try
			{
				do
				{
					var b = binaryReader.ReadByte();

					if (b == 0)
					{
						break;
					}

					bytes.Add(b);
				} while (true);
			}
			catch (Exception ex)
			{
				throw new InvalidDataException("Reached unxpected end of stream while reading string value.", ex);
			}

			return Encoding.UTF8.GetString(bytes.ToArray());
		}
	}
}
