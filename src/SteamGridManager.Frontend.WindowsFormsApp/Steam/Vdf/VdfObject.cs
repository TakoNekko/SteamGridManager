using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Steam.Vdf
{
	public class VdfObject : IEnumerable<KeyValuePair<string, object>>
	{
		#region Properties

		private readonly Dictionary<string, object> items = new Dictionary<string, object>();
		public IReadOnlyDictionary<string, object> Items => items;

		public string Key { get; set; }

		public object Value { get; set; }

		#endregion

		#region Enumerable

		IEnumerator IEnumerable.GetEnumerator()
			=> GetEnumerator();

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		public object this[string key]
		{
			get => items[key];
			set => items[key] = value;
		}

		public int Count
			=> items.Count;

		public bool ContainsKey(string key)
			=> items.ContainsKey(key);

		#endregion

		#region Methods

		public string GetString(string key)
		{
			if (ContainsKey(key)
				&& this[key] is var value)
			{
				if (value is string stringValue)
				{
					return stringValue;
				}
				else if (value is uint uintValue)
				{
					//Console.Error.WriteLine($"WARNING: \"{Path}\" is an integer, but expected a string.");

					return uintValue.ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					return $"{value}";
				}
			}

			return null;
		}

		public bool? GetBoolean(string key)
		{
			if (GetUInt(key) is var uintValue)
			{
				if (uintValue != 0
					&& uintValue != 1)
				{
					//Console.Error.WriteLine($"WARNING: \"{Path}\" is {uintValue}, but expected a boolean.");
				}

				return uintValue != 0;
			}

			return null;
		}

		public DateTime? GetRelativeTime(string key)
		{
			if (GetUInt(key) is var uintValue)
			{
				return uintValue.Value.ToRelativeTime();
			}

			return null;
		}

		public uint? GetUInt(string key)
		{
			if (ContainsKey(key)
				&& this[key] is var value)
			{
				if (value is string stringValue)
				{
					//Console.Error.WriteLine($"WARNING: \"{Path}\" is a string, but expected an integer.");

					if (uint.TryParse(stringValue, out var uintValue))
					{
						return uintValue;
					}
				}
				else if (value is uint uintValue)
				{
					return uintValue;
				}
				else
				{
					return null;
				}
			}

			return null;
		}

		public VdfObject GetObject(string key)
		{
			if (ContainsKey(key)
				&& this[key] is var value)
			{
				if (value is VdfObject objectValue)
				{
					return objectValue;
				}
			}

			return null;
		}

		#endregion

		public VdfObject FindNodeByPath(string path)
		{
			if (path is null) throw new ArgumentNullException(nameof(path));
			else if (path.Length == 0) { return null; }

			var pathParts = path.Split(new char[] { '/' });
			var currentNode = this;

			for (var i = 0; i < pathParts.Length; ++i)
			{
				var pathPart = pathParts[i];

				if (!currentNode.ContainsKey(pathPart))
				{
					return null;
				}

				var subNode = currentNode[pathPart];

				if (subNode is VdfObject subObject)
				{
					currentNode = subObject;
				}
				else
				{
					return null;
				}
			}

			return currentNode;
		}

		public object FindValueByPath(string path)
		{
			if (path is null) throw new ArgumentNullException(nameof(path));
			else if (path.Length == 0) { return null; }

			// TOOD: it'd probably be best to just build every possible path and match against them with a regex.

			var pathParts = path.Split(new char[] { '/' });
			var currentNode = this;

			for (var i = 0; i < pathParts.Length - 1; ++i)
			{
				var pathPart = pathParts[i];

				if (!currentNode.ContainsKey(pathPart))
				{
					return null;
				}

				var subNode = currentNode[pathPart];

				if (subNode is VdfObject subObject)
				{
					currentNode = subObject;
				}
				else
				{
					return null;
				}
			}

			var lastPart = pathParts[pathParts.Length - 1];

			// HACK: bleh, this will do for now...
			// value comparison
			if (lastPart.Contains("="))
			{
				var tokens = lastPart.Split(new char[] { '=' }, 2);
				var key = tokens[0];	// e.g., type
				var value = tokens[1];	// e.g., developer or developer>name

				if (!currentNode.ContainsKey(key))
				{
					return null;
				}

				// redirection
				if (value.Contains(">"))
				{
					tokens = value.Split(new char[] { '>' }, 2);
					value = tokens[0];  // e.g., developer

					if (!$"{currentNode[key]}".Equals(value))
					{
						return null;
					}

					key = tokens[1];	// e.g., name

					if (!currentNode.ContainsKey(key))
					{
						return null;
					}

					return currentNode[key];
				}
				else
				{
					if (!$"{currentNode[key]}".Equals(value))
					{
						return null;
					}

					return currentNode[key];
				}
			}

			if (!currentNode.ContainsKey(lastPart))
			{
				return null;
			}

			return currentNode[lastPart];
		}

		public void Add(string key, object value)
			=> items.Add(key, value);

		public void Remove(string key)
			=> items.Remove(key);

		public object Find(string vdfPath)
		{
			return FindRecursive(vdfPath, Key, Value);
		}

		public static object FindRecursive(string vdfPath, string key, object value)
		{
			Console.WriteLine(key);

			if (vdfPath.Equals(key))
			{
				return value;
			}

			if (value is VdfObject vdfObjectValue)
			{
				foreach (var item in vdfObjectValue)
				{
					var result = FindRecursive(vdfPath, key + "/" + item.Key, item.Value);

					if (result != null)
					{
						return result;
					}
				}
			}

			return null;
		}

		public VdfObject DeepClone()
		{
			var clone = new VdfObject
			{
				Key = Key,
				Value = Value,
			};

			{
				if (Value is VdfObject objectValue)
				{
					clone.Value = objectValue.DeepClone();
				}
				else if (Value is string stringValue)
				{
					clone.Value = stringValue;
				}
				else if (Value is uint uintValue)
				{
					clone.Value = uintValue;
				}
			}

			foreach (var kvp in Items)
			{
				if (kvp.Value is VdfObject objectValue)
				{
					clone.Add(kvp.Key, objectValue.DeepClone());
				}
				else if (kvp.Value is string stringValue)
				{
					clone.Add(kvp.Key, stringValue);
				}
				else if (kvp.Value is uint uintValue)
				{
					clone.Add(kvp.Key, uintValue);
				}
			}

			return clone;
		}

		public void Copy(VdfObject other)
		{
			{
				if (other.Value is VdfObject objectValue)
				{
					Value = objectValue.DeepClone();
				}
				else if (other.Value is string stringValue)
				{
					Value = stringValue;
				}
				else if (other.Value is uint uintValue)
				{
					Value = uintValue;
				}
			}

			items.Clear();

			foreach (var kvp in other.Items)
			{
				if (kvp.Value is VdfObject objectValue)
				{
					Add(kvp.Key, objectValue.DeepClone());
				}
				else if (kvp.Value is string stringValue)
				{
					Add(kvp.Key, stringValue);
				}
				else if (kvp.Value is uint uintValue)
				{
					Add(kvp.Key, uintValue);
				}
			}
		}
	}

	internal static partial class ExtensionMethods
	{
		public static DateTime ToRelativeTime(this uint value)
			=> new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
	}
}
