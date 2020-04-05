using System;
using swifty.Code.Syntaxt;
namespace swifty.Code {
    class Evaluator {
        private readonly ExpressionSyntax _root;
        public Evaluator(ExpressionSyntax root) {
            _root = root;
        }
        public int Evaluate() {
            return EvaluateExpression(_root);
        }
        private int EvaluateExpression(ExpressionSyntax root) {
            if (root is LiteralExpressionSyntax n) {
                return (int)n.LiteralToken.Value;
            }
            if (root is UnaryExpressionSyntax u) {
                int operand = EvaluateExpression(u.Operand);
                switch (u.OperatorToken.Kind) {
                    case SyntaxKind.PlusToken: return operand;
                    case SyntaxKind.MinusToken: return -operand;
                    default: throw new Exception($"Unexpected Unary expression {u.OperatorToken.Kind}");
                }
            }
            if (root is BinaryExpressionSyntax b) {
                int left = EvaluateExpression(b.Left);
                int right = EvaluateExpression(b.Right);

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