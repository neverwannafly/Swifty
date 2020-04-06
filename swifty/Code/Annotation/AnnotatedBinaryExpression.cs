using System;
namespace swifty.Code.Annotation {

    internal sealed class AnnotatedBinaryExpression : AnnotatedExpression {
        public AnnotatedBinaryExpression(AnnotatedExpression left,AnnotatedBinaryOperator op, AnnotatedExpression right) {
            Left = left;
            Operator = op;
            Right = right;
        }
        public override AnnotatedKind Kind => AnnotatedKind.BinaryExpression;
        public override Type Type => Operator.ResultType;
        public AnnotatedExpression Left {get;}
        public AnnotatedBinaryOperator Operator {get;}
        public AnnotatedExpression Right {get;}
    }
}