namespace Minsk.CodeAnalysis.Syntax
{
    // The Lexer/Tokenizer splits the Input(String) -> SyntaxTokens
    internal sealed class Lexer
    {
        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new List<string>(); // used for passing errors etc up.

        public Lexer(string text)
        {
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics; // exposing our Error Handling
        private char Current => Peek(0);
        private char Lookahead => Peek(1);

      
        // Peek(0) returns current char, peek(1) the one after etc... Does NOT consume token
        private char Peek(int ahead)
        {
            if (_position + ahead >= _text.Length)
                return '\0';
            return _text[_position + ahead];
        }

        // iterator to return the next position
        private void Next()
        {
            _position++;
        }

        public SyntaxToken NextToken()
        {
            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EOFToken, _position, "\0", null);
            }

            // Token is Value Number - of Digit 0-9
            if (char.IsDigit(Current))
            {
                var start = _position;
                while (char.IsDigit(Current))
                    Next();
                var length = _position - start;
                var text = _text.Substring(start, length);
                if (!int.TryParse(text, out var value))
                    _diagnostics.Add($"ERROR: failed TryParse '{text}' to INT32, in Lexer.NextToken()");
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            // Token is whitespace
            if (char.IsWhiteSpace(Current))
            {
                var start = _position;
                while (char.IsWhiteSpace(Current))
                    Next();
                var length = _position - start;
                var text = _text.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            // Token is text based (keyword, identifier like variable name etc.) 

            if (char.IsLetter(Current))
            {
                var start = _position;
                while (char.IsLetter(Current))
                    Next();
                var length = _position - start;
                var text = _text.Substring(start, length);
                var kind = SyntaxFacts.GetKeywordKind(text);    // keyword-recognition gets handled here
                return new SyntaxToken(kind, start, text, null);
            }
            switch (Current)
            {
                // arithmetic operators + - * / ( )
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.DivideToken, _position++, "/", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.MultiplicationToken, _position++, "*", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
                // logical operators
                case '&':
                    {
                        if (Lookahead == '&')
                            return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, _position += 2, "&&", null);
                        break;
                    }
                case '|':
                    {
                        if (Lookahead == '|')
                            return new SyntaxToken(SyntaxKind.PipePipeToken, _position += 2, "||", null);
                        break;
                    }
                case '=':
                    {
                        if (Lookahead == '=')
                            return new SyntaxToken(SyntaxKind.EqualsEqualsToken, _position += 2, "==", null);
                        break;
                    }
                case '!':
                    {
                        if (Lookahead == '=')
                            return new SyntaxToken(SyntaxKind.BangEqualstoken, _position += 2, "!=", null);
                        else
                            return new SyntaxToken(SyntaxKind.BangToken, _position++, "!", null);
                    }
            }

            // badtoken default case:
            _diagnostics.Add($"ERROR: bad character input: '{Current}' in Lexer.NextToken()");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }
}
