using System;
namespace swifty.Code.Annotation {
    internal sealed class AnnotatedBinaryExpression : AnnotatedExpression {
        public AnnotatedBinaryExpression(AnnotatedExpression left,AnnotatedBinaryOperatorKind operatorKind, AnnotatedExpression right) {
            Left = left;
            OperatorKind = operatorKind;
            Right = right;
        }
        public override AnnotatedKind Kind => AnnotatedKind.BinaryExpression;
        public override Type Type => Left.Type;
        public AnnotatedExpression Left {get;}
        public AnnotatedBinaryOperatorKind OperatorKind {get;}
        public AnnotatedExpression Right {get;}
    }
}