using System;

namespace swifty.Code.Annotation {
    internal sealed class AnnotatedAssignmentExpression : AnnotatedExpression {
        public AnnotatedAssignmentExpression(string name, AnnotatedExpression expression) {
            Name = name;
            Expression = expression;
        }
        public string Name {get;}
        public AnnotatedExpression Expression {get;}
        public override Type Type => Expression.Type;
        public override AnnotatedKind Kind => AnnotatedKind.AssignmentExpression;
    }
}