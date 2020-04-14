using System.Collections.Immutable;

namespace swifty.Code.Annotation {
    internal sealed class AnnotatedBlockStatement : AnnotatedStatement {
        public AnnotatedBlockStatement(ImmutableArray<AnnotatedStatement> statements) {
            Statements = statements;
        }
        public ImmutableArray<AnnotatedStatement> Statements {get;}
        public override AnnotatedKind Kind => AnnotatedKind.BlockStatement;
    }
}