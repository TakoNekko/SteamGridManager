using System.Globalization;
using System.Text;

namespace Steam.Vdf
{
	public class AcfWriter
	{
		public StringBuilder StringBuilder { get; }

		public void Write(StringBuilder stringBuilder, VdfObject vdfObject)
			=> Write(stringBuilder, vdfObject, depth: 0);

		public void Write(StringBuilder stringBuilder, VdfObject vdfObject, int depth)
		{
			stringBuilder.AppendLine($"\"{vdfObject.Key}\"");
			stringBuilder.AppendLine($"{{");
			WriteRecursive(stringBuilder, vdfObject, depth);
			stringBuilder.AppendLine($"}}");
		}

		public void WriteRecursive(StringBuilder stringBuilder, VdfObject obj, int depth)
		{
			var margin = depth > 0 ? new string('\t', depth) : "";

			// CHECKME: unecessary?
			{
				if (obj.Value is VdfObject objectValue)
				{
					stringBuilder.AppendLine($"{margin}\"{obj.Key}\"");
					stringBuilder.AppendLine($"{margin}\"{{");
					WriteRecursive(stringBuilder, objectValue, depth + 1);
					stringBuilder.AppendLine($"{margin}}}");
				}
				else if (obj.Value is string stringValue)
				{
					stringBuilder.AppendLine($"{margin}\"{obj.Key}\"\t\t\"{stringValue.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"");
				}
				else if (obj.Value is uint uintValue)
				{
					stringBuilder.AppendLine($"{margin}\"{obj.Key}\"\t\t\"{uintValue.ToString(CultureInfo.InvariantCulture)}\"");
				}
			}

			foreach (var item in obj)
			{
				if (item.Value is VdfObject objectValue2)
				{
					stringBuilder.AppendLine($"{margin}\"{item.Key}\"");
					stringBuilder.AppendLine($"{margin}{{");
					WriteRecursive(stringBuilder, objectValue2, depth + 1);
					stringBuilder.AppendLine($"{margin}}}");
				}
				else if (item.Value is string stringValue2)
				{
					stringBuilder.AppendLine($"{margin}\"{item.Key}\"\t\t\"{stringValue2.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"");
				}
				else if (item.Value is uint uintValue2)
				{
					stringBuilder.AppendLine($"{margin}\"{item.Key}\"\t\t\"{uintValue2.ToString(CultureInfo.InvariantCulture)}\"");
				}
			}
		}
	}
}

