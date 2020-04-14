using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    public sealed class CompilationUnitSyntax : SyntaxNode {
        public CompilationUnitSyntax(StatementSyntax statement, SyntaxToken endOfFile) {
            Statement = statement;
            EndOfFileToken = endOfFile;
        }
        public StatementSyntax Statement {get;}
        public SyntaxToken EndOfFileToken {get;}
        public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return Statement;
            yield return EndOfFileToken;
        }
    }
}