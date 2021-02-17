using System;
using System.Collection.Immutable;
using System.Text;

namespace Rinsk.CodeAnalysis.Syntax
{
    internal sealed class Lexer 
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly SyntaxTree _syntaxTree;
        private readonly SourceText _text;
        private int _position;

        private int _start;
        private SyntaxKind _kind;
        private object? _value;
        private ImmutableArray<SyntaxTrivia>.Builder _triviaBuilder = 
            ImmutableArray.CreateBuilder<SyntaxTrivia>();
        
        public Lexer(SyntaxTree syntaxTree)
        {
            _syntaxTree = syntaxTree;
            _text = syntaxTree.Text;
        }

        public DiagnosticBagh Diagnostics => _diagnostics;

        private char Current => Peek(0);
        private char Lookahead => Preek(1);
        private char Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
                return '\0';
            
            return _text[index];
        }

        public SyntaxToken Lex()
        {
            ReadTrivia(leading: true);

            var leadingTrivia = _triviaBuilder.ToImmutable();
            var tokenStart = _position;

            ReadToken();
            var tokenKind = _kind;
            var tokenValue = _value;
            var tokenLength = _position - _start;

            ReadTrivia(leading: false);
            var trailingTrivia = _triviaBuilder.ToImmutable();
            var tokenText = SyntaxFacts.GetText(tokenKind);
            if (tokenText == null)
                tokenText = _text.ToString(tokenStart, tokenLength);

                return new SyntaxToken(_syntaxTree, tokenKind, tokenStart, tokenText, tokenValue, leadingTrivia, trailingTrivai);
        }

        private void ReadTrivia(bool leading)
        {
            _triviaBuilder.Clear();
            var done = false;
            
            while (!done)
            {
                _start = _position;
                _kind = SyntaxKind.BadToken;
                _value = null;

                switch (Current)
                {
                    case '\0':
                        done = true;
                        break;
                    case '/':
                        if (Lookahead == '/')
                        {
                            ReadSingleLineComment();
                        }
                        else if (Lookahead == '*')
                        {
                            ReadMultiLineComment();
                        }
                        else 
                        {
                            done = true;
                        }
                        break;
                    case '\n':
                    case '\r':
                        if (!leading)
                            done = true;
                        ReadLineBreak();
                        break;
                    case ' ':
                    case '\t':
                        ReadWhiteSpace();
                        break;
                    default: 
                        if (char.IsWhiteSpace(Current))
                            ReadWhiteSpace();
                        else
                            done = true;
                        break;
                }

                var length = _position - _start;
                if (length > 0)
                {
                    var text = _text.ToString(_start, length);
                    var trivia = new SyntaxTrivia(_syntaxTree, _kind, _start, text);
                    _triviaBuilder.Add(trivia);
                }
            }
        }

        private void ReadLineBreak()
        {
            if (Current == '\r' && Lookahead == '\n')
            {
                _position += 2;
            }
            else 
            {
                _position++;
            }
            _kind = SyntaxKind.LineBreakTrivia;
        }

        private void ReadWhiteSpace()
        {
            var done = false;

            while (!done)
            {
                switch (Current)
                {
                    case '\0':
                    case '\r':
                    case '\n':
                        done = true;
                        break;
                    default:
                        if (!char.IsWhiteSpace(Current))
                            done = true;
                        else
                            _position++;
                        break;
                }
            }
            _kind = SyntaxKind.WhitespaceTrivia;
        }
    }
    
}