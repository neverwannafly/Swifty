using System;

namespace swifty.Code.Annotation {
    internal sealed class AnnotatedUnaryExpression : AnnotatedExpression {
        public AnnotatedUnaryExpression(AnnotatedUnaryOperatorKind operatorKind, AnnotatedExpression operand) {
            OperatorKind = operatorKind;
            Operand = operand;
        }
        public override AnnotatedKind Kind => AnnotatedKind.UnaryExpression;
        public override Type Type => Operand.Type;
        public AnnotatedUnaryOperatorKind OperatorKind {get;}
        public AnnotatedExpression Operand {get;}
    }
}