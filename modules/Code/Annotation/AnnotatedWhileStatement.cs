namespace swifty.Code.Annotation {
    internal sealed class AnnotatedWhileStatement : AnnotatedStatement {
        public AnnotatedWhileStatement(AnnotatedExpression condition, AnnotatedStatement body) {
            Condition = condition;
            Body = body;
        }
        public AnnotatedExpression Condition {get;}
        public AnnotatedStatement Body {get;}
        public override AnnotatedKind Kind => AnnotatedKind.WhileStatement;
    }
}