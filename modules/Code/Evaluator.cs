using System;
using System.Linq;
using System.Collections.Generic;
using swifty.Code.Annotation;
using swifty.Code.Syntaxt;

namespace swifty.Code {

    public sealed class Compiler {
        public Compiler(SyntaxTree syntax) {
            Syntax = syntax;
        }
        public SyntaxTree Syntax {get;}
        public EvaluationResult EvaluationResult() {
            var annotator = new Annotator();
            var annotatedExpression = annotator.AnnotateExpression(Syntax.Root);
            var diagnostics = Syntax.Diagnostics.Concat(annotator.Diagnostics).ToArray();
            if (diagnostics.Any()) {
                return new EvaluationResult(diagnostics, null);
            }
            var evaluator = new Evaluator(annotatedExpression);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<string>(), value);
        }
    }

    public sealed class EvaluationResult {
        public EvaluationResult(IEnumerable<string> diagnostics, object value) {
            Diagnostics = diagnostics.ToArray();
            Value = value;
        }
        public IReadOnlyList<string> Diagnostics {get;}
        public object Value {get;}
    }

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
                    case AnnotatedBinaryOperatorKind.Equality: return Equals(left, right);
                    case AnnotatedBinaryOperatorKind.Inequality:
                    return !Equals(left, right);
                    default: throw new Exception($"Unexpected binary operator {b.Operator.Kind}");
                }
            }
            throw new Exception($"Unexpected node {root.Kind}");
        }
    }
}