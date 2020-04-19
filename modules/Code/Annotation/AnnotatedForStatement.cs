namespace swifty.Code.Annotation {
    internal sealed class AnnotatedForStatement : AnnotatedStatement {
        public AnnotatedForStatement(VariableSymbol variable, AnnotatedExpression lowerBound, AnnotatedExpression upperBound, AnnotatedStatement body) {
            Variable = variable;
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Body = body;
        }
        public VariableSymbol Variable {get;} 
        public AnnotatedExpression LowerBound {get;}
        public AnnotatedExpression UpperBound {get;}
        public AnnotatedStatement Body {get;}
        public override AnnotatedKind Kind => AnnotatedKind.ForStatement;
    }
}