using System.Collections.Immutable;

namespace Rinsk.CodeAnalysis.Text
{
	public sealed class SourceText
	{
		private readonly string _text;

		public int Length => _text.Length;
		public char this[int index] => _text[index];
		public string ToString(int start, int length) => _text.Substring(start, length);
	}
}
