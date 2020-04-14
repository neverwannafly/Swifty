using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    public sealed class CompilationUnitSyntax : SyntaxNode {
        public CompilationUnitSyntax(ExpressionSyntax root, SyntaxToken endOfFile) {
            Expression = root;
            EndOfFileToken = endOfFile;
        }
        public ExpressionSyntax Expression {get;}
        public SyntaxToken EndOfFileToken {get;}
        public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return Expression;
            yield return EndOfFileToken;
        }
    }
}