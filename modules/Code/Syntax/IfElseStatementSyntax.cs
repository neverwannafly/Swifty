using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    public sealed class IfStatementSyntax : StatementSyntax {
        public IfStatementSyntax(SyntaxToken ifKeyword, ExpressionSyntax condition, StatementSyntax thenStatement, ElseClauseSyntax elseClause) {
            IfKeyword = ifKeyword;
            Condition = condition;
            ThenStatement = thenStatement;
            ElseClause = elseClause;
        }
        public SyntaxToken IfKeyword {get;}
        public ExpressionSyntax Condition {get;}
        public StatementSyntax ThenStatement {get;}
        public ElseClauseSyntax ElseClause {get;}
        public override SyntaxKind Kind => SyntaxKind.IfStatementSyntax;
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return IfKeyword;
            yield return Condition;
            yield return ThenStatement;
            if (ElseClause != null) {
                yield return ElseClause;
            }
        }
    }
    public sealed class ElseClauseSyntax : SyntaxNode {
        public ElseClauseSyntax(SyntaxToken elseKeyword, StatementSyntax elseStatement) {
            ElseKeyword = elseKeyword;
            ElseStatement = elseStatement;
        }
        public SyntaxToken ElseKeyword {get;}
        public StatementSyntax ElseStatement {get;}
        public override SyntaxKind Kind => SyntaxKind.ElseClauseSyntax;
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return ElseKeyword;
            yield return ElseStatement;
        }
    }
}