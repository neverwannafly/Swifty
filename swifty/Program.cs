using System;
using System.Collections.Generic;
using System.Linq;

namespace swifty {
    class Program {
        static void Main(string[] args) {
            while (true) {
                Console.Write("> ");
                string line = Console.ReadLine();
                var parser = new Parser(line);
                var expression = parser.Parse();

                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                print(expression);
                Console.ForegroundColor = color;
            }
        }
        static void print(SyntaxNode node, string indent="") {
            Console.Write(indent);
            Console.Write(node.Kind);
            if (node is SyntaxToken t && t.Value!=null) {
                Console.Write(" ");
                Console.Write(t.Value);
            }
            Console.WriteLine();
            indent += "    ";
            foreach (var child in node.GetChildren()) {
                print(child, indent);
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
        NumberExpression,
        BinaryExpression,
    }
    class SyntaxToken : SyntaxNode {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value) {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }
        public override SyntaxKind Kind {get;}
        public int Position {get;}
        public string Text {get;}
        public object Value {get;}
        public override IEnumerable<SyntaxNode> GetChildren() {
            return Enumerable.Empty<SyntaxNode>();
        }
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

    abstract class SyntaxNode {
        public abstract SyntaxKind Kind {get;}
        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
    abstract class ExpressionSyntax : SyntaxNode {

    }
    sealed class NumberExpressionSyntax : ExpressionSyntax {
        public NumberExpressionSyntax(SyntaxToken numberToken) {
            NumberToken = numberToken;
        }
        public override SyntaxKind Kind => SyntaxKind.NumberExpression;
        public SyntaxToken NumberToken {get;}
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return NumberToken;
        }
    }
    sealed class BinaryExpressionSyntax : ExpressionSyntax {
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right) {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }
        public ExpressionSyntax Left {get;}
        public SyntaxToken OperatorToken {get;}
        public ExpressionSyntax Right {get;}
        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;  
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }

    class Parser {
        private readonly SyntaxToken[] _tokens;
        private int _position;
        public Parser(string text) {
            Lexer lexer = new Lexer(text);
            SyntaxToken token;
            List<SyntaxToken> tokens = new List<SyntaxToken>();
            do {
                token = lexer.NextToken();
                if (token.Kind != SyntaxKind.WhitespaceToken || token.Kind!=SyntaxKind.BadToken) {
                    tokens.Add(token);
                }
            } while (token.Kind!=SyntaxKind.EndofFileToken);
            _tokens = tokens.ToArray();
        }
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
        private SyntaxToken Match(SyntaxKind kind) {
            if (Current.Kind == kind) return NextToken();
            return new SyntaxToken(kind, Current.Position, null, null);
        }
        public ExpressionSyntax Parse() {
            var left = ParsePrimaryExpression();
            while (Current.Kind == SyntaxKind.PlusToken || Current.Kind==SyntaxKind.MinusToken) {
                var operationToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operationToken, right);
            }
            return left;
        }
        private ExpressionSyntax ParsePrimaryExpression() {
            var numberToken = Match(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }
    }

}
