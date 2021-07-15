using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rinsk.CodeAnalysis.Text;

namespace Rinsk.CodeAnalysis.Syntax
{
    public abstract class SyntaxNode 
    {
        private protected SyntaxNode(SyntaxTree syntaxTree)
        {
            SyntaxTree = syntaxTree;
        }

        public SyntaxTree SyntaxTree { get; }

        public SyntaxNode? Parent => SyntaxTree.GetParent(this);

        public abstract SyntaxKind Kind { get; }

        public virtual TextSpan Span 
        {
            get
            {
                var first = GetChildren().First().Span;
                var last = GetChildren().Last().Span;
                return TextSpan.FromBounds(first.Start, last.End);
            }
        }

        public virtual TextSpan FullSpan 
        {
            get 
            {
                var first = GetChildren().First().FullSpan;
                var last = GetChildren().Last().FullSpan;
                return TextSpan.FromBounds(first.Start, last.End);
            }
        }

        public abstract IEnumerable<SyntaxNode> GetChildren();

    }    
}