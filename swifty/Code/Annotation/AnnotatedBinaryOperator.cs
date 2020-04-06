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
            new AnnotatedBinaryOperator(SyntaxKind.AndToken, AnnotatedBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new AnnotatedBinaryOperator(SyntaxKind.OrToken, AnnotatedBinaryOperatorKind.LogicalOr, typeof(bool)),
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