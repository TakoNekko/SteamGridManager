using System.Globalization;
using System.IO;
using System.Text;

namespace Steam.Vdf
{
	public class VdfWriter
	{
		public Stream Stream { get; }
		public BinaryWriter BinaryWriter { get; }
		private bool LeaveOpen { get; }

		public VdfWriter(Stream stream)
			: this(stream, leaveOpen: false) { }

		public VdfWriter(Stream stream, bool leaveOpen)
		{
			Stream = stream;
			LeaveOpen = leaveOpen;
		}

		public void Write(VdfObject vdfObject)
			=> Write(vdfObject, writeDelimiters: true);

		public void Write(VdfObject vdfObject, bool writeDelimiters)
		{
			using (var binaryWriter = new BinaryWriter(Stream, Encoding.Default, leaveOpen: LeaveOpen))
			{
				if (writeDelimiters)
				{
					binaryWriter.Write((byte)0);
				}

				WriteString(binaryWriter, vdfObject.Key);

				WriteObjectRecursive(binaryWriter, vdfObject);

				if (writeDelimiters)
				{
					binaryWriter.Write((byte)8);
				}
			}
		}

		private void WriteObjectRecursive(BinaryWriter binaryWriter, VdfObject vdfObject)
		{
			foreach (var item in vdfObject.Items)
			{
				var type = 0;

				if (item.Value is VdfObject)
				{
					type = 0;
				}
				else if (item.Value is string)
				{
					type = 1;
				}
				else if (item.Value is uint)
				{
					type = 2;
				}
				else
				{
					SteamGridManager.Frontend.WindowsFormsApp.Program.LogError($"VdfWriter.WriteObjectRecursive: Invalid value type {(item.Value is null ? "(null)" : item.Value?.GetType().Name)} for item {item.Key}.");

					// HACK/FIXME: I may not be correctly deserializing empty tags...

					binaryWriter.Write((byte)type);
					WriteString(binaryWriter, item.Key);
					binaryWriter.Write((byte)8);

					continue;
				}

				binaryWriter.Write((byte)type);

				WriteString(binaryWriter, item.Key);

				if (item.Value is VdfObject vdfObjectValue)
				{
					WriteObjectRecursive(binaryWriter, vdfObjectValue);
				}
				else if (item.Value is string stringValue)
				{
					WriteString(binaryWriter, stringValue);
				}
				else if (item.Value is uint uintValue)
				{
					WriteUInt(binaryWriter, uintValue);
				}
			}

			binaryWriter.Write((byte)8);
		}

		private void WriteUInt(BinaryWriter binaryWriter, uint value)
		{
			binaryWriter.Write(value);
		}

		private void WriteString(BinaryWriter binaryWriter, string value)
		{
			if (value is null)
			{
				return;
			}

			binaryWriter.Write(Encoding.UTF8.GetBytes(value));
			binaryWriter.Write((byte)0);
		}
	}
}

