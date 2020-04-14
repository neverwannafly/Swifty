using System.Collections.Immutable;
using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    public sealed class BlockStatementSyntax : StatementSyntax {
        public BlockStatementSyntax(SyntaxToken openBraceToken, ImmutableArray<StatementSyntax> statements, SyntaxToken closeBraceToken) {
            OpenBraceToken = openBraceToken;
            Statements = statements;
            CloseBraceToken = closeBraceToken;
        }
        public SyntaxToken OpenBraceToken {get;}
        public ImmutableArray<StatementSyntax> Statements {get;}
        public SyntaxToken CloseBraceToken {get;}

        public override SyntaxKind Kind => SyntaxKind.BlockStatement;
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return OpenBraceToken;
            foreach(var st in Statements) {
                yield return st;
            }
            yield return CloseBraceToken;
        }
    }
}