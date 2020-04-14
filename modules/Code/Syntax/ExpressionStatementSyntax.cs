using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    public sealed class ExpressionStatementSyntax : StatementSyntax {
        public ExpressionStatementSyntax(ExpressionSyntax expression) {
            Expression = expression;
        }
        public ExpressionSyntax Expression {get;}
        public override SyntaxKind Kind => SyntaxKind.ExpressionStatement;
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return Expression;
        }
    }
}