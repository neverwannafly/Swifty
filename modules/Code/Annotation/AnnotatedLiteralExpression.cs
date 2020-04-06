using System;

namespace swifty.Code.Annotation {
    internal sealed class AnnotatedLiteralExpression : AnnotatedExpression {
        public AnnotatedLiteralExpression(object value) {
            Value = value;
        }
        public override AnnotatedKind Kind => AnnotatedKind.LiteralExpression;
        public override Type Type => Value.GetType();
        public object Value {get;}
    }
}