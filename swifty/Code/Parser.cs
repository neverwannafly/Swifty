using System.Collections.Generic;

namespace swifty.Code {
    internal sealed class Parser {
        private readonly SyntaxToken[] _tokens;
        private List<string> _diagnostics = new List<string>();
        private int _position;
        public Parser(string text) {
            Lexer lexer = new Lexer(text);
            SyntaxToken token;
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            do {
                token = lexer.Lex();
                if (token.Kind != SyntaxKind.WhitespaceToken && token.Kind!=SyntaxKind.BadToken) {
                    tokens.Add(token);
                }
            } while (token.Kind!=SyntaxKind.EndofFileToken);
            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }
        public IEnumerable<string> Diagnostics => _diagnostics;
        private SyntaxToken Peek(int offset) {
            int index = _position + offset;
            if (index >= _tokens.Length) return _tokens[_tokens.Length-1];
            return _tokens[index];
        }
        private SyntaxToken Current => Peek(0);
        private SyntaxToken NextToken() {
            SyntaxToken current = Current;
            _position++;
            return current; 
        }
        private SyntaxToken MatchToken(SyntaxKind kind) {
            if (Current.Kind == kind) return NextToken();
            _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }
        public SyntaxTree Parse() {
            var expression = ParseExpression();
            var endofFileToken = MatchToken(SyntaxKind.EndofFileToken);
            return new SyntaxTree(_diagnostics ,expression, endofFileToken);
        }
        private ExpressionSyntax ParseExpression(int parentPrecedence = 0) {
            ExpressionSyntax left = ParsePrimaryExpression();
            while (true) {
                int precedence = Current.Kind.GetBinaryOperatorPrecendence();
                if (precedence == 0 || precedence <= parentPrecedence) break;
                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax right = ParseExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }
        private ExpressionSyntax ParsePrimaryExpression() {
            if (Current.Kind == SyntaxKind.LeftParanthesisToken) {
                SyntaxToken left = NextToken();
                ExpressionSyntax expression = ParseExpression();
                SyntaxToken right = MatchToken(SyntaxKind.RightParanthesisToken);
                return new ParanthesisExpressionSyntax(left, expression, right);
            }
            SyntaxToken numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}