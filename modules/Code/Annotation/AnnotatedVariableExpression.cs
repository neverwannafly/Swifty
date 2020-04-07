using System;

namespace swifty.Code.Annotation {
    internal sealed class AnnotatedVariableExpression : AnnotatedExpression {
        public AnnotatedVariableExpression(string name, Type type) {
            Name = name;
            Type = type;
        }
        public string Name {get;}
        public override Type Type {get;}
        public override AnnotatedKind Kind => AnnotatedKind.VariableExpression;
    }
}