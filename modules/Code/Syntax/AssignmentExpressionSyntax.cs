using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    // Assignment operator needs to be right associative
    public sealed class AssignmentExpressionSyntax : ExpressionSyntax {
        public AssignmentExpressionSyntax(SyntaxToken identifierToken, SyntaxToken equalToken, ExpressionSyntax expression) {
           IdentifierToken = identifierToken;
           EqualToken = equalToken;
           Expression = expression;
        }
        public override SyntaxKind Kind => SyntaxKind.AssignmentExpression;  
        public SyntaxToken IdentifierToken {get;}
        public SyntaxToken EqualToken {get;}
        public ExpressionSyntax Expression {get;}
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return IdentifierToken;
            yield return EqualToken;
            yield return Expression;
        }
    }
}