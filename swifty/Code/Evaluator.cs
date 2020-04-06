using System;
using swifty.Code.Syntaxt;
using swifty.Code.Annotation;
namespace swifty.Code {
    internal class Evaluator {
        private readonly AnnotatedExpression _root;
        public Evaluator(AnnotatedExpression root) {
            _root = root;
        }
        public object Evaluate() {
            return EvaluateExpression(_root);
        }
        private object EvaluateExpression(AnnotatedExpression root) {
            if (root is AnnotatedLiteralExpression n) {
                return n.Value;
            }
            if (root is AnnotatedUnaryExpression u) {
                object operand = EvaluateExpression(u.Operand);
                switch (u.Operator.Kind) {
                    case AnnotatedUnaryOperatorKind.Identity: return (int)operand;
                    case AnnotatedUnaryOperatorKind.Negation: return -(int)operand;
                    case AnnotatedUnaryOperatorKind.LogicalNegation: return !(bool)operand;
                    default: throw new Exception($"Unexpected Unary expression {u.Operator.Kind}");
                }
            }
            if (root is AnnotatedBinaryExpression b) {
                object left = EvaluateExpression(b.Left);
                object right = EvaluateExpression(b.Right);

                switch(b.Operator.Kind) {
                    case AnnotatedBinaryOperatorKind.Addition: return (int)left + (int)right;
                    case AnnotatedBinaryOperatorKind.Subtraction: return (int)left - (int)right;
                    case AnnotatedBinaryOperatorKind.Multiplication: return (int)left * (int)right;
                    case AnnotatedBinaryOperatorKind.Division: {
                        if ((int)right == 0) {
                            throw new Exception("ERROR: Division by Zero");
                        }
                        return (int)left / (int)right;
                    }
                    case AnnotatedBinaryOperatorKind.LogicalAnd : return (bool)left && (bool)right;
                    case AnnotatedBinaryOperatorKind.LogicalOr: return (bool)left || (bool)right;
                    default: throw new Exception($"Unexpected binary operator {b.Operator.Kind}");
                }
            }
            throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}