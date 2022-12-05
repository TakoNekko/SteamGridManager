using System;

namespace SteamGridManager.Frontend.WindowsFormsApp.Extensions.TextBox
{
	using TextBox = System.Windows.Forms.TextBox;

	internal static partial class TextBoxExtensionMethods
	{
		public static void InsertText(this TextBox textBox, string append)
		{
			if (textBox is null) throw new ArgumentNullException(nameof(textBox));
			else if (append is null) throw new ArgumentNullException(nameof(append));
			else if (append.Length == 0) return;

			var text = textBox.Text;
			var selectionStart = textBox.SelectionStart;

			if (textBox.SelectionLength != 0)
			{
				text = text.Remove(textBox.SelectionStart, textBox.SelectionLength);
			}

			text = text.Insert(textBox.SelectionStart, append);

			textBox.Text = text;
			textBox.SelectionLength = 0;
			textBox.SelectionStart = selectionStart + append.Length;
		}
	}
}
