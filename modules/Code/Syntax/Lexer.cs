using swifty.Code.Text;

namespace swifty.Code.Syntaxt {
    internal sealed class Lexer {
        private readonly string _text;
        private int _position;
        private DiagnosisHandler _diagnostics = new DiagnosisHandler();
        public Lexer(string text) {
            _text = text;
        }
        public DiagnosisHandler Diagnostics => _diagnostics;
        private char Current => Peek(0);
        private char LookAhead => Peek(1);
        private char Peek(int offset) {
            int index = _position + offset;
            if (index>=_text.Length) return '\0';
                return _text[index];
        }
        private void Next() {
            _position++;
        }
        public SyntaxToken Lex() {
            if (char.IsWhiteSpace(Current)) {
                int start = _position;
                while (char.IsWhiteSpace(Current)) Next();
                int len = _position - start;
                string text = _text.Substring(start, len);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }
            if (char.IsDigit(Current)) {
                int start = _position;
                while (char.IsDigit(Current)) Next();
                int len = _position - start;
                string text = _text.Substring(start, len);
                if (!int.TryParse(text, out var value)){
                    _diagnostics.ReportInvalidNumber(new TextSpan(start, len), text, typeof(int));
                };
                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }
            if (char.IsLetter(Current)) {
                int start = _position;
                while (char.IsLetter(Current)) Next();
                int len = _position - start;
                string text = _text.Substring(start, len);
                var kind = SyntaxRules.GetKeywordKind(text);
                return new SyntaxToken(kind, start, text, null);
            }
            switch(Current) {
                case '\0': return new SyntaxToken(SyntaxKind.EndofFileToken, _position++, "\0", null);
                case '+':  return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':  return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':  return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                case '/':  return new SyntaxToken(SyntaxKind.DivideToken, _position++, "/", null);
                case '(':  return new SyntaxToken(SyntaxKind.LeftParanthesisToken, _position++, "(", null);
                case ')':  return new SyntaxToken(SyntaxKind.RightParanthesisToken, _position++, ")", null);
                case '!': { 
                    if (LookAhead=='=') {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.NotEqualToken, _position-2, "!=", null);
                    }
                    return new SyntaxToken(SyntaxKind.NotToken, _position++, "!", null); 
                }
                case '&': {
                    if (LookAhead=='&') { 
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.AndToken, _position-2, "&&", null);
                    }
                    break;
                }
                case '|': {
                    if (LookAhead=='|') { 
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.OrToken, _position-2, "||", null);
                    }
                    break;
                }
                case '=' : {
                    if (LookAhead=='=') {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.EqualToken, _position-2, "==", null);
                    }
                    break;
                }
                case ':' : {
                    if (LookAhead=='=') {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.AssignmentToken, _position-1, ":=", null);
                    }
                    break;
                }
            }
            _diagnostics.ReportBadCharacter(_position, Current);
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position-1, 1), null);
        }
    }
}