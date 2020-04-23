using System;
using System.Collections.Generic;
using swifty.Code.Annotation;
using swifty.Code.Syntaxt;

namespace swifty.Code {
    internal class Evaluator {
        private readonly AnnotatedStatement _root;
        private readonly Dictionary<VariableSymbol,object> _symbolTable;
        private object _lastValue;
        public Evaluator(AnnotatedStatement root, Dictionary<VariableSymbol,object> symbolTable) {
            _root = root;
            _symbolTable = symbolTable;
        }
        public object Evaluate() {
            EvaluateStatement(_root);
            return _lastValue;
        }
        private void EvaluateStatement(AnnotatedStatement statement) {
            if (statement is AnnotatedBlockStatement b) {
                foreach(var st in b.Statements) {
                    EvaluateStatement(st);
                }
                return;
            }
            if (statement is AnnotatedExpressionStatement e) {
                _lastValue = EvaluateExpression(e.Expression);
                return;
            }
            if (statement is AnnotateVariableDeclaration v) {
                var value = EvaluateExpression(v.Initializer);
                _symbolTable[v.Variable] = value;
                _lastValue = value;
                return;
            }
            if (statement is AnnotatedIfStatement i) {
                var condition = (bool)EvaluateExpression(i.Condition);
                if (condition) {
                    EvaluateStatement(i.ThenStatement);
                } else if (i.ElseStatement!=null) {
                    EvaluateStatement(i.ElseStatement);
                }
                return;
            }
            if (statement is AnnotatedWhileStatement w) {
                while ((bool)EvaluateExpression(w.Condition)) {
                    EvaluateStatement(w.Body);
                }
                return;
            }
            if (statement is AnnotatedForStatement f) {
                var lowerBound = (int)EvaluateExpression(f.LowerBound);
                var upperBound = (int)EvaluateExpression(f.UpperBound);
                for (var idx=lowerBound; idx<upperBound; idx++) {
                    _symbolTable[f.Variable] = idx;
                    EvaluateStatement(f.Body);
                }
                return;
            }
            throw new Exception($"Unexpected node {statement.Kind}");
        }
        private object EvaluateExpression(AnnotatedExpression root) {
            if (root is AnnotatedTypeCastExpression t) {
                var left = EvaluateExpression(t.Left);
                return PerformTypeCast(left, t.Right);
            }
            if (root is AnnotatedLiteralExpression n) {
                return n.Value;
            }
            if (root is AnnotatedVariableExpression v) {
                return _symbolTable[v.Symbol];
            }
            if (root is AnnotatedAssignmentExpression a) {
                var value = EvaluateExpression(a.Expression);
                _symbolTable[a.Symbol] = value;
                return value;
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
                    case AnnotatedBinaryOperatorKind.Xor: return PerformBooleanOperation(left, AnnotatedBinaryOperatorKind.Xor, right);
                    case AnnotatedBinaryOperatorKind.And: return PerformBooleanOperation(left, AnnotatedBinaryOperatorKind.And, right);
                    case AnnotatedBinaryOperatorKind.Or: return PerformBooleanOperation(left, AnnotatedBinaryOperatorKind.Or, right);
                    case AnnotatedBinaryOperatorKind.Equality: return Equals(left, right);
                    case AnnotatedBinaryOperatorKind.Inequality:
                    return !Equals(left, right);
                    case AnnotatedBinaryOperatorKind.GreaterThan: return (int)left > (int)right;
                    case AnnotatedBinaryOperatorKind.GreaterThanEqual: return (int)left >= (int)right;
                    case AnnotatedBinaryOperatorKind.LessThan: return (int)left < (int)right;
                    case AnnotatedBinaryOperatorKind.LessThanEqual: return (int)left <= (int)right;
                    default: throw new Exception($"Unexpected binary operator {b.Operator.Kind}");
                }
            }
            throw new Exception($"Unexpected node {root.Kind}");
        }
        private object PerformBooleanOperation(object left, AnnotatedBinaryOperatorKind op, object right) {
            Type operandTypeLeft = left.GetType();
            Type operandTypeRight = right.GetType();

            if(operandTypeLeft == typeof(int) && operandTypeRight == typeof(int)) {
                return ApplyOperator((int)left, op, (int)right);
            }
            if (operandTypeLeft == typeof(bool) && operandTypeRight == typeof(bool)) {
                return ApplyOperator((bool)left, op, (bool)right);
            }
            throw new Exception("Invalid operands to boolean operators");
        }
        private int ApplyOperator(int left, AnnotatedBinaryOperatorKind op, int right) {
            switch(op) {
                case AnnotatedBinaryOperatorKind.Xor: return left^right;
                case AnnotatedBinaryOperatorKind.And: return left&right;
                case AnnotatedBinaryOperatorKind.Or: return left|right;
            }
            throw new Exception("Invalid BooleanOperator");
        }
        private bool ApplyOperator(bool left, AnnotatedBinaryOperatorKind op, bool right) {
            switch(op) {
                case AnnotatedBinaryOperatorKind.Xor: return left^right;
                case AnnotatedBinaryOperatorKind.And: return left&right;
                case AnnotatedBinaryOperatorKind.Or: return left|right;
            }
            throw new Exception("Invalid BooleanOperator");
        }
        private object PerformTypeCast(object left, SyntaxKind right) {
            if (left.GetType() == typeof(int) && right == SyntaxKind.BoolKeyword) {
                return (int)left != 0;
            }
            if (left.GetType() == typeof(bool) && right == SyntaxKind.IntKeyword) {
                return (bool)left ? 1 : 0;
            }
            if ((left.GetType() == typeof(int) && right == SyntaxKind.IntKeyword) || (left.GetType() == typeof(bool) && right == SyntaxKind.BoolKeyword)) {
                return left;
            }
            Console.WriteLine("Typecast failed");
            return -1;
        }
    }
}