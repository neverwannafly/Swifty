namespace swifty.Code.Annotation {
    internal sealed class AnnotatedIfStatement : AnnotatedStatement {
        public AnnotatedIfStatement(AnnotatedExpression condition, AnnotatedStatement thenStatement, AnnotatedStatement elseStatement) {
            Condition = condition;
            ThenStatement = thenStatement;
            ElseStatement = elseStatement;
        }
        public AnnotatedExpression Condition {get;}
        public AnnotatedStatement ThenStatement {get;}
        public AnnotatedStatement ElseStatement {get;}
        public override AnnotatedKind Kind => AnnotatedKind.IfStatement;
    }
}