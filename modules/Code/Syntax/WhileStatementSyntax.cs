using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    public sealed class WhileStatementSyntax : StatementSyntax {
        public WhileStatementSyntax(SyntaxToken keyword, ExpressionSyntax condition, StatementSyntax body) {
            Keyword = keyword;
            Condition = condition;
            Body = body;
        }
        public SyntaxToken Keyword {get;} 
        public ExpressionSyntax Condition {get;}
        public StatementSyntax Body {get;}
        public override SyntaxKind Kind => SyntaxKind.WhileStatementSyntax;
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return Keyword;
            yield return Condition;
            yield return Body;
        }
    }
}