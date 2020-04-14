using System.Collections.Generic;
using swifty.Code.Text;

namespace swifty.Code.Syntaxt {
    internal sealed class Parser {
        private readonly SyntaxToken[] _tokens;
        private readonly SourceText _text;
        private DiagnosisHandler _diagnostics = new DiagnosisHandler();
        private int _position;
        public Parser(SourceText text) {
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
            _text = text;
        }
        public DiagnosisHandler Diagnostics => _diagnostics;
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
            _diagnostics.ReportUnexpectedToken(Current.Span, kind, Current.Kind);
            return new SyntaxToken(kind, Current.Position, null, null);
        }
        public CompilationUnitSyntax ParseCompilationUnit() {
            var expression = ParseExpression();
            var endofFileToken = MatchToken(SyntaxKind.EndofFileToken);
            return new CompilationUnitSyntax(expression, endofFileToken);
        }
        private ExpressionSyntax ParseExpression(int parentPrecedence = 0) {
            return ParseAssignmentExpression();
        }
        private ExpressionSyntax ParseAssignmentExpression() {
            if (Current.Kind == SyntaxKind.IdentifierToken && Peek(1).Kind==SyntaxKind.AssignmentToken) {
                var identifierToken = NextToken();
                var operatorToken = NextToken();
                var right = ParseAssignmentExpression();
                return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
            }
            return ParseBinaryExpression();
        }
        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0) {
            ExpressionSyntax left;
            int unaryPrec = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryPrec!=0 && unaryPrec >= parentPrecedence) {
                SyntaxToken opToken = NextToken();
                ExpressionSyntax operand = ParseBinaryExpression(unaryPrec);
                left = new UnaryExpressionSyntax(opToken, operand);
            } else {
                left = ParsePrimaryExpression();
                // Console.WriteLine(left.Kind);
            }
            while (true) {
                int precedence = Current.Kind.GetBinaryOperatorPrecendence();
                if (precedence == 0 || precedence <= parentPrecedence) break;
                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax right = ParseBinaryExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }
        private ExpressionSyntax ParsePrimaryExpression() {
            switch(Current.Kind) {
                case SyntaxKind.LeftParanthesisToken : {
                    SyntaxToken left = NextToken();
                    ExpressionSyntax expression = ParseExpression();
                    SyntaxToken right = MatchToken(SyntaxKind.RightParanthesisToken);
                    return new ParanthesisExpressionSyntax(left, expression, right);
                }
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword: {
                    var keywordToken = NextToken();
                    var value = (keywordToken.Kind == SyntaxKind.TrueKeyword);
                    return new LiteralExpressionSyntax(keywordToken, value);
                }
                case SyntaxKind.IdentifierToken: {
                    var identifierToken = NextToken();
                    return new NameExpressionSyntax(identifierToken);
                }
                default: {
                    SyntaxToken numberToken = MatchToken(SyntaxKind.NumberToken);
                    return new LiteralExpressionSyntax(numberToken);
                }
            }
        }
    }
}