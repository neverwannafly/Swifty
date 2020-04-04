using System;
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
            // while(Current.Kind == SyntaxKind.WhitespaceToken) { NextToken();}
            if (Current.Kind == kind) return NextToken();
            _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }
        public SyntaxTree Parse() {
            var expression = ParseExpression();
            var endofFileToken = MatchToken(SyntaxKind.EndofFileToken);
            return new SyntaxTree(_diagnostics ,expression, endofFileToken);
        }
        private ExpressionSyntax ParseExpression() {
            return ParseTerm();
        }
        private ExpressionSyntax ParseTerm() {
            var left = ParseFactor();
            while (Current.Kind == SyntaxKind.PlusToken || Current.Kind==SyntaxKind.MinusToken) {
                var operationToken = NextToken();
                var right = ParseFactor();
                left = new BinaryExpressionSyntax(left, operationToken, right);
            }
            return left;
        }
        private ExpressionSyntax ParseFactor() {
            var left = ParsePrimaryExpression();
            while (Current.Kind==SyntaxKind.StarToken || Current.Kind==SyntaxKind.DivideToken) {
                var operationToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operationToken, right);
            }
            return left;
        }
        private ExpressionSyntax ParsePrimaryExpression() {
            if (Current.Kind == SyntaxKind.LeftParanthesisToken) {
                var left = NextToken();
                var expression = ParseExpression();
                var right = MatchToken(SyntaxKind.RightParanthesisToken);
                return new ParanthesisExpressionSyntax(left, expression, right);
            }
            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}