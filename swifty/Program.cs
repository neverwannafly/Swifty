using System;
using System.Collections.Generic;
using System.Linq;

namespace swifty {
    class Program {
        static void Main(string[] args) {
            while (true) {
                Console.Write("> ");
                string line = Console.ReadLine();
                var syntaxTree = SyntaxTree.Parse(line);
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                print(syntaxTree.Root);
                Console.ForegroundColor = color;

                if (!syntaxTree.Diagnostics.Any()) {
                    Evaluator eval = new Evaluator(syntaxTree.Root);
                    int result = eval.Evaluate();
                    Console.WriteLine(result);
                } else {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach(var diagnostic in syntaxTree.Diagnostics) {
                        Console.WriteLine(diagnostic);
                    }
                    Console.ForegroundColor = color;
                }
            }
        }
        static void print(SyntaxNode node, string indent="", bool isLast=true) {
            var marker = isLast? "└──" : "├──";
            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);
            if (node is SyntaxToken t && t.Value!=null) {
                Console.Write($" {t.Value}");
            }
            indent += isLast ? "    ":"|    ";
            Console.WriteLine();
            var lastChild = node.GetChildren().LastOrDefault();
            foreach (var child in node.GetChildren()) {
                print(child, indent, child==lastChild);
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
        ParathesisExpression,
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
        private List<string> _diagnostics = new List<string>();
        public Lexer(string text) {
            _text = text;
        }
        public IEnumerable<string> Diagnostics => _diagnostics;
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
                if (!int.TryParse(text, out var value)){
                    _diagnostics.Add($"The number {_text} is invalid Int32");
                };
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
                default: {  
                    _diagnostics.Add($"ERROR: Bad character input : '{Current}'");
                    return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position-1, 1), null);
                }
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
    sealed class ParanthesisExpressionSyntax: ExpressionSyntax {
        public ParanthesisExpressionSyntax(SyntaxToken leftParanthesis, ExpressionSyntax expr, SyntaxToken rightParanthesis) {
            LeftParanthesisToken = leftParanthesis;
            Expression = expr;
            RightParanthesisToken = rightParanthesis;
        }
        public SyntaxToken LeftParanthesisToken {get;}
        public ExpressionSyntax Expression {get;}
        public SyntaxToken RightParanthesisToken {get;}
        public override SyntaxKind Kind => SyntaxKind.ParathesisExpression;
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return LeftParanthesisToken;
            yield return Expression;
            yield return RightParanthesisToken;
        }
    }
    sealed class SyntaxTree {
        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endofFileToken) {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndofFileToken = endofFileToken;
        }
        public IReadOnlyList<string> Diagnostics {get;}
        public ExpressionSyntax Root {get;}
        public SyntaxToken EndofFileToken {get;}
        public static SyntaxTree Parse(string text) {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
    class Parser {
        private readonly SyntaxToken[] _tokens;
        private List<string> _diagnostics = new List<string>();
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
        private SyntaxToken Match(SyntaxKind kind) {
            // while(Current.Kind == SyntaxKind.WhitespaceToken) { NextToken();}
            if (Current.Kind == kind) return NextToken();
            _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }
        private ExpressionSyntax ParseExpression() {
            return ParseTerm();
        }
        public SyntaxTree Parse() {
            var expression = ParseTerm();
            var endofFileToken = Match(SyntaxKind.EndofFileToken);
            return new SyntaxTree(_diagnostics ,expression, endofFileToken);
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
                var right = Match(SyntaxKind.RightParanthesisToken);
                return new ParanthesisExpressionSyntax(left, expression, right);
            }
            var numberToken = Match(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }
    }
    class Evaluator {
        private readonly ExpressionSyntax _root;
        public Evaluator(ExpressionSyntax root) {
            _root = root;
        }
        public int Evaluate() {
            return EvaluateExpression(_root);
        }
        private int EvaluateExpression(ExpressionSyntax root) {
            if (root is NumberExpressionSyntax n) {
                return (int)n.NumberToken.Value;
            }
            if (root is BinaryExpressionSyntax b) {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch(b.OperatorToken.Kind) {
                    case SyntaxKind.PlusToken: return left + right;
                    case SyntaxKind.MinusToken: return left - right;
                    case SyntaxKind.StarToken: return left * right;
                    case SyntaxKind.DivideToken: return left / right;
                    default: throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}");
                }
            }
            if (root is ParanthesisExpressionSyntax p) {
                return EvaluateExpression(p.Expression);
            }
            throw new Exception($"Unexpected node {root.Kind}");
        }
    }

}
