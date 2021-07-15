using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Threading;
using Rinsk.CodeAnalysis.Text;

namespace Rinsk.CodeAnalysis.Syntax
{
	public sealed class SyntaxTree
	{
		public SourceText Text { get; }

		internal SyntaxNode? GetParent(SyntaxNode syntaxNode)
		{
			throw new NotImplementedException();
		}
	}
}
