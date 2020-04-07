using System;

namespace swifty.Code.Annotation {
    internal sealed class AnnotatedAssignmentExpression : AnnotatedExpression {
        public AnnotatedAssignmentExpression(VariableSymbol symbol, AnnotatedExpression expression) {
            Symbol = symbol;
            Expression = expression;
        }
        public VariableSymbol Symbol {get;}
        public AnnotatedExpression Expression {get;}
        public override Type Type => Symbol.Type;
        public override AnnotatedKind Kind => AnnotatedKind.AssignmentExpression;
    }
}