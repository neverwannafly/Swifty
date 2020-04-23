using swifty.Code.Syntaxt;
using System;

namespace swifty.Code.Annotation {    
    internal sealed class AnnotatedBinaryOperator {
        private AnnotatedBinaryOperator(SyntaxKind syntaxKind, AnnotatedBinaryOperatorKind kind, Type leftType) : this(syntaxKind, kind, leftType, leftType, leftType) {}
        private AnnotatedBinaryOperator(SyntaxKind syntaxKind, AnnotatedBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType) {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            ResultType = resultType;
        }
        public SyntaxKind SyntaxKind {get;}
        public AnnotatedBinaryOperatorKind Kind {get;}
        public Type LeftType {get;}
        public Type RightType {get;}
        public Type ResultType {get;}
        private static AnnotatedBinaryOperator[] _operators = {
            new AnnotatedBinaryOperator(SyntaxKind.PlusToken, AnnotatedBinaryOperatorKind.Addition, typeof(int)),
            new AnnotatedBinaryOperator(SyntaxKind.MinusToken, AnnotatedBinaryOperatorKind.Subtraction, typeof(int)),
            new AnnotatedBinaryOperator(SyntaxKind.StarToken, AnnotatedBinaryOperatorKind.Multiplication, typeof(int)),
            new AnnotatedBinaryOperator(SyntaxKind.DivideToken, AnnotatedBinaryOperatorKind.Division, typeof(int)),
            new AnnotatedBinaryOperator(SyntaxKind.EqualToken, AnnotatedBinaryOperatorKind.Equality, typeof(int), typeof(int), typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.EqualToken, AnnotatedBinaryOperatorKind.Equality, typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.NotEqualToken, AnnotatedBinaryOperatorKind.Inequality, typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.NotEqualToken, AnnotatedBinaryOperatorKind.Inequality, typeof(int), typeof(int), typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.NotEqualToken, AnnotatedBinaryOperatorKind.Inequality, typeof(int), typeof(int), typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.LogicalAndToken, AnnotatedBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.LogicalOrToken, AnnotatedBinaryOperatorKind.LogicalOr, typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.LessThanEqualToken, AnnotatedBinaryOperatorKind.LessThanEqual, typeof(int), typeof(int), typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.GreaterThanEqualToken, AnnotatedBinaryOperatorKind.GreaterThanEqual, typeof(int), typeof(int), typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.LessThanToken, AnnotatedBinaryOperatorKind.LessThan, typeof(int), typeof(int), typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.GreaterThanToken, AnnotatedBinaryOperatorKind.GreaterThan, typeof(int), typeof(int), typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.XorToken, AnnotatedBinaryOperatorKind.Xor, typeof(int)), 
            new AnnotatedBinaryOperator(SyntaxKind.AndToken, AnnotatedBinaryOperatorKind.And, typeof(int)),
            new AnnotatedBinaryOperator(SyntaxKind.OrToken, AnnotatedBinaryOperatorKind.Or, typeof(int)),
            new AnnotatedBinaryOperator(SyntaxKind.XorToken, AnnotatedBinaryOperatorKind.Xor, typeof(bool)), 
            new AnnotatedBinaryOperator(SyntaxKind.AndToken, AnnotatedBinaryOperatorKind.And, typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.OrToken, AnnotatedBinaryOperatorKind.Or, typeof(bool)),
        };
        public static AnnotatedBinaryOperator Annotate(SyntaxKind syntaxKind, Type leftType, Type rightType) {
            foreach (var op in _operators) {
                if (op.SyntaxKind==syntaxKind && op.LeftType==leftType && op.RightType==rightType) {
                    return op;
                }
            }
            return null;
        }
    }
}