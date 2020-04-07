using System;

namespace swifty.Code.Annotation {
    internal sealed class AnnotatedVariableExpression : AnnotatedExpression {
        public AnnotatedVariableExpression(VariableSymbol symbol) {
            Symbol = symbol;
        }
        public VariableSymbol Symbol {get;}
        public override Type Type => Symbol.Type;
        public override AnnotatedKind Kind => AnnotatedKind.VariableExpression;
    }
}