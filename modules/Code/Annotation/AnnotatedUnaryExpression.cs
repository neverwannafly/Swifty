using System;

namespace swifty.Code.Annotation {
    internal sealed class AnnotatedUnaryExpression : AnnotatedExpression {
        public AnnotatedUnaryExpression(AnnotatedUnaryOperator op, AnnotatedExpression operand) {
            Operator = op;
            Operand = operand;
        }
        public override AnnotatedKind Kind => AnnotatedKind.UnaryExpression;
        public override Type Type => Operator.ResultType;
        public AnnotatedUnaryOperator Operator {get;}
        public AnnotatedExpression Operand {get;}
    }
}