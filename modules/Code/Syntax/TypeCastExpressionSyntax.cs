using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    class TypeCastExpressionSyntax : ExpressionSyntax {
        public TypeCastExpressionSyntax(ExpressionSyntax operand, SyntaxToken op, SyntaxToken type) {
            Operand = operand;
            Operator = op;
            Type = type;
        }
        public override SyntaxKind Kind => SyntaxKind.TypeCastExpression;
        public ExpressionSyntax Operand {get;}
        public SyntaxToken Operator {get;}
        public SyntaxToken Type {get;}
        public override IEnumerable<SyntaxNode> GetChildren() {
            yield return Operand;
            yield return Operator;
            yield return Type;
        }
    }
}