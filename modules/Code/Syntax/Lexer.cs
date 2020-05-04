using swifty.Code.Text;
using System;

namespace swifty.Code.Syntaxt {
    internal sealed class Lexer {
        private readonly SourceText _text;
        private int _position;
        private DiagnosisHandler _diagnostics = new DiagnosisHandler();
        public Lexer(SourceText text) {
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
            if (Current == '#') {
                return ReadComments();
            }
            if (char.IsWhiteSpace(Current)) {
                return ReadWhiteSpace();
            }
            if (char.IsDigit(Current)) {
                return ReadNumber();
            }
            if (char.IsLetter(Current)) {
                return ReadString();
            }
            if (Current == '\'') {
                return ReadStringLiteral();
            }
            return ReadOperators();
        }
        private SyntaxToken ReadWhiteSpace() {
            int start = _position;
            while (char.IsWhiteSpace(Current)) Next();
            int len = _position - start;
            string text = _text.ToString(start, len);
            return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
        }
        private SyntaxToken ReadComments() {
            int start = _position;
            while (Current != '\0' && Current != '\n') Next();
            int len = _position - start;
            string comment = _text.ToString(start, len);
            return new SyntaxToken(SyntaxKind.CommentToken, start, comment, null);
        }
        private SyntaxToken ReadNumber() {
            int start = _position;
            while (char.IsDigit(Current)) Next();
            int len = _position - start;
            string text = _text.ToString(start, len);
            if (!int.TryParse(text, out var value)){
                _diagnostics.ReportInvalidNumber(new TextSpan(start, len), text, typeof(int));
            };
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }
        private SyntaxToken ReadString() {
            int start = _position;
            while (char.IsLetter(Current)) Next();
            int len = _position - start;
            string text = _text.ToString(start, len);
            var kind = SyntaxRules.GetKeywordKind(text);
            return ReadKeywordOrIdentifier(kind, start, text);
        }
        private SyntaxToken ReadKeywordOrIdentifier(SyntaxKind kind, int start, string text) {
            switch(kind) {
                case SyntaxKind.IntKeyword: return new SyntaxToken(kind, start, text, null, typeof(int));
                case SyntaxKind.BoolKeyword: return new SyntaxToken(kind, start, text, null, typeof(bool));
                case SyntaxKind.CharKeyword: return new SyntaxToken(kind, start, text, null, typeof(char));
                default: return new SyntaxToken(kind, start, text, null);
            }
        }
        private SyntaxToken ReadStringLiteral() {
            int start = ++_position;
            while (Current!='\'' && Current != '\0') Next();
            int len = _position - start;
            string text = _text.ToString(start, len);
            if (Current != '\'') {
                _diagnostics.ReportMissingQuotes(new TextSpan(start, len), text);
            }
            if (!char.TryParse(text, out var value)) {
                _diagnostics.ReportInvalidCharacter(new TextSpan(start, len), text, typeof(char));
            }
            _position += 1;
            return new SyntaxToken(SyntaxKind.CharToken, start, text, value);
        }
        private SyntaxToken ReadOperators() {
            switch(Current) {
                case '\0': return new SyntaxToken(SyntaxKind.EndofFileToken, _position++, "", null);
                case '+':  return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
                case '-':  return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
                case '*':  return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
                case '/':  return new SyntaxToken(SyntaxKind.DivideToken, _position++, "/", null);
                case '(':  return new SyntaxToken(SyntaxKind.LeftParanthesisToken, _position++, "(", null);
                case ')':  return new SyntaxToken(SyntaxKind.RightParanthesisToken, _position++, ")", null);
                case '~': return new SyntaxToken(SyntaxKind.BitwiseNegationToken, _position++, "~", null);
                case '>': {
                    if (LookAhead=='=') {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.GreaterThanEqualToken, _position-2, ">=", null);
                    }
                    return new SyntaxToken(SyntaxKind.GreaterThanToken, _position++, ">", null);
                }
                case '<': {
                    if (LookAhead=='=') {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.LessThanEqualToken, _position-2, "<=", null);
                    }
                    return new SyntaxToken(SyntaxKind.LessThanToken, _position++, "<", null);
                }
                case '^' : {
                    return new SyntaxToken(SyntaxKind.XorToken, _position++, "^", null);
                }
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
                        return new SyntaxToken(SyntaxKind.LogicalAndToken, _position-2, "&&", null);
                    }
                    return new SyntaxToken(SyntaxKind.AndToken, _position++, "&", null);
                }
                case '|': {
                    if (LookAhead=='|') { 
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.LogicalOrToken, _position-2, "||", null);
                    }
                    return new SyntaxToken(SyntaxKind.OrToken, _position++, "|", null);
                }
                case '=' : {
                    if (LookAhead=='=') {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.EqualToken, _position-2, "==", null);
                    }
                    if (LookAhead=='>') {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.TypeCastToken, _position-2, "=>", null);
                    }
                    break;
                }
                case ':' : {
                    if (LookAhead=='=') {
                        _position += 2;
                        return new SyntaxToken(SyntaxKind.AssignmentToken, _position-2, ":=", null);
                    }
                    break;
                }
                case '{' : {
                    return new SyntaxToken(SyntaxKind.OpenBraceToken, _position++, "{", null);
                }
                case '}' : {
                    return new SyntaxToken(SyntaxKind.CloseBraceToken, _position++, "}", null);
                }
            }
            _diagnostics.ReportBadCharacter(_position, Current);
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.ToString(_position-1, 1), null);
        }
    }
}