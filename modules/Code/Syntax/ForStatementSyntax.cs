using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    public sealed class ForStatementSyntax : StatementSyntax {
        public ForStatementSyntax(SyntaxToken forKeyword, SyntaxToken intKeyword, SyntaxToken identifier, SyntaxToken equalsToken, ExpressionSyntax lowerBound, SyntaxToken toKeyword, ExpressionSyntax upperBound, StatementSyntax body) {
            ForKeyword = forKeyword;
            IntKeyword = intKeyword;
            Identifier = identifier;
            EqualsToken = equalsToken;
            LowerBound = lowerBound;
            ToKeyword = toKeyword;
            UpperBound = upperBound;
            Body = body;
        }
        public SyntaxToken ForKeyword {get;}
        public SyntaxToken IntKeyword {get;}
        public SyntaxToken Identifier {get;} 
        public SyntaxToken EqualsToken {get;}
        public ExpressionSyntax LowerBound {get;} 
        public SyntaxToken ToKeyword {get;}
        public ExpressionSyntax UpperBound {get;}
        public StatementSyntax Body {get;}
        public override SyntaxKind Kind => SyntaxKind.ForStatementSyntax;
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return ForKeyword;
            yield return IntKeyword;
            yield return Identifier;
            yield return EqualsToken;
            yield return LowerBound;
            yield return ToKeyword;
            yield return UpperBound;
            yield return Body;
        }
    }
}