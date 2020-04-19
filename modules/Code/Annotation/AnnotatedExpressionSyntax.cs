namespace swifty.Code.Annotation {
    internal sealed class AnnotatedExpressionStatement : AnnotatedStatement {
        public AnnotatedExpressionStatement(AnnotatedExpression expression) {
            Expression = expression;
        }
        public AnnotatedExpression Expression {get;}
        public override AnnotatedKind Kind => AnnotatedKind.ExpressionStatement;
    }
}