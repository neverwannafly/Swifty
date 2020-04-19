namespace swifty.Code.Annotation {
    internal sealed class AnnotateVariableDeclaration : AnnotatedStatement {
        public AnnotateVariableDeclaration(VariableSymbol variable, AnnotatedExpression initializer) {
            Variable = variable;
            Initializer = initializer;
        }
        public VariableSymbol Variable {get;} 
        public AnnotatedExpression Initializer {get;}
        public override AnnotatedKind Kind => AnnotatedKind.VariableDeclaration;
    }
}