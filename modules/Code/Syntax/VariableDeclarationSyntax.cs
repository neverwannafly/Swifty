using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    public sealed class VariableDeclarationSyntax : StatementSyntax {
        public VariableDeclarationSyntax(SyntaxToken keyword, SyntaxToken identifier, SyntaxToken assignmentToken, ExpressionSyntax initializer, bool isReadOnly=false) {
            Keyword = keyword;
            Identifier = identifier;
            AssignmentToken = assignmentToken;
            Initializer = initializer;
            IsReadOnly = isReadOnly;
        }
        public SyntaxToken Keyword {get;} 
        public SyntaxToken Identifier {get;} 
        public SyntaxToken AssignmentToken {get;}
        public ExpressionSyntax Initializer {get;}
        public bool IsReadOnly {get;}
        public override SyntaxKind Kind => SyntaxKind.VariableDeclarationStatement;
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return Keyword;
            yield return Identifier;
            yield return AssignmentToken;
            yield return Initializer;
        }
    }
}