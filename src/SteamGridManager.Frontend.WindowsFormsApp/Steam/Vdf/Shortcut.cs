namespace Steam.Vdf
{
	public class Shortcut
	{
		public VdfObject Node { get; set; }

		public static string Escape(string value)
			=> value
				.Replace("\\", "\\\\")
				.Replace("\"", "\\\"");

		public static string GetStringWithoutQuotes(string value)
			=> string.IsNullOrEmpty(value)
				? value
				: (value.StartsWith("\"")
					&& value.EndsWith("\""))
						? value.Substring(1, value.Length - 2)
						: value;
	}
}
