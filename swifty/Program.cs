using System;

namespace swifty {
    class Program {
        static void Main(string[] args) {
            while (true) {
                string line = Console.ReadLine();
                Lexer lexer = new Lexer(line);
                while (true) {
                    SyntaxToken token = lexer.NextToken();
                    if (token.Kind == SyntaxKind.EndofFileToken) {
                        break;
                    }
                    Console.Write($"{token.Kind} : `{token.Text}`");
                    if (token.Value!=null) {
                        Console.Write($" -> {token.Value}");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
    enum SyntaxKind {
        NumberToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        DivideToken,
        LeftParanthesisToken,
        RightParanthesisToken,
        EndofFileToken,
        BadToken,
    }
    class SyntaxToken {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value) {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }
        public SyntaxKind Kind {get;}
        public int Position {get;}
        public string Text {get;}
        public object Value {get;}
    }
    class Lexer {
        private readonly string _text;
        private int _position;
        public Lexer(string text) {
            _text = text;
        }
        private char Current {
            get {
                if (_position>=_text.Length) return '\0';
                return _text[_position];
            }
        }
        private void Next() {
            _position++;
        }
        public SyntaxToken NextToken() {
            if (_position >= _text.Length) {
                return new SyntaxToken(SyntaxKind.EndofFileToken, _position, "\0", null);
            }
            if (char.IsDigit(Current)) {
                int start = _position;
                while (char.IsDigit(Current)) Next();
                int len = _position - start;
                string text = _text.Substring(start, len);
                int.TryParse(text, out var value);
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }
            if (char.IsWhiteSpace(Current)) {
                int start = _position;
                while (char.IsWhiteSpace(Current)) Next();
                int len = _position - start;
                string text = _text.Substring(start, len);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }
            switch(Current) {
                case '+':  return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':  return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':  return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                case '/':  return new SyntaxToken(SyntaxKind.DivideToken, _position++, "/", null);
                case '(':  return new SyntaxToken(SyntaxKind.LeftParanthesisToken, _position++, "(", null);
                case ')':  return new SyntaxToken(SyntaxKind.RightParanthesisToken, _position++, ")", null);
                default:  return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position-1, 1), null);
            }
        }
    }

}
