using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Rinsk.CodeAnalysis.Text;
namespace Rinsk.CodeAnalysis.Syntax
{
	public sealed class SyntaxToken : SyntaxNode
	{
		internal SyntaxToken(
			SyntaxTree syntaxTree, 
			SyntaxKind kind, 
			int position, 
			string? text, 
			object? value,  
			ImmutableArray<SyntaxTrivia> leadingTrivia, 
			ImmutableArray<SyntaxTrivia> trailinTrivia) : base(syntaxTree) 
		{

		}

		public override SyntaxKind Kind { get; }
		public int Position { get; }
		public string Text { get; }
		public object? Value { get; }

		public override TextSpan Span => new TextSpan(Position, Text.Length);
		
		public override TextSpan FullSpan 
		{
			get 
			{
				var start = LeadingTrivia.Length == 0 
					? Span.Start
					: LeadingTrivia.First().Span.Start;
				var end = TrailingTrivia.Length == 0
					? Span.End
					: TrailingTrivia.First().Span.End;
				return TextSpan.FromBounds(start, end);
			}
		}

		public ImmutableArray<SyntaxTrivia> LeadingTrivia {get; } 
		public ImmutableArray<SyntaxTrivia> TrailingTrivia {get;}

		public override IEnumerable<SyntaxNode> GetChildren()
		{
			return Array.Empty<SyntaxNode>();
		}


		/// <symmary> 
		/// A token is missing if it was inserted by the parser and doesn't appear in source.
		/// <summary>
		public bool IsMissing { get; }

	}
}
