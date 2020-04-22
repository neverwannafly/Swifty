using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    public sealed class VariableDeclarationSyntax : StatementSyntax {
        public VariableDeclarationSyntax(SyntaxToken constKeyword, SyntaxToken datatypeKeyword, SyntaxToken identifier, SyntaxToken assignmentToken, ExpressionSyntax initializer, bool isReadOnly=false) {
            ConstKeyword = constKeyword;
            DatatypeKeyword = datatypeKeyword;
            Identifier = identifier;
            AssignmentToken = assignmentToken;
            Initializer = initializer;
            IsReadOnly = isReadOnly;
        }
        public SyntaxToken ConstKeyword {get;} 
        public SyntaxToken DatatypeKeyword {get;} 
        public SyntaxToken Identifier {get;} 
        public SyntaxToken AssignmentToken {get;}
        public ExpressionSyntax Initializer {get;}
        public bool IsReadOnly {get;}
        public override SyntaxKind Kind => SyntaxKind.VariableDeclarationStatement;
        public override IEnumerable<SyntaxNode> GetChildren() {
            if (ConstKeyword!=null) {
                yield return ConstKeyword;
            }
            yield return DatatypeKeyword;
            yield return Identifier;
            yield return AssignmentToken;
            yield return Initializer;
        }
    }
}