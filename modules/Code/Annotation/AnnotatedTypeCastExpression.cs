using System;
using swifty.Code.Syntaxt;

namespace swifty.Code.Annotation {
    internal sealed class AnnotatedTypeCastExpression : AnnotatedExpression {
        public AnnotatedTypeCastExpression(AnnotatedExpression left, SyntaxKind right) {
            Left = left;
            Right = right;
        }
        public override AnnotatedKind Kind => AnnotatedKind.LiteralExpression;
        public override Type Type => GetResultType();
        public AnnotatedExpression Left {get;}
        public SyntaxKind Right {get;}
        public Type GetResultType() {
            switch (Right) {
                case SyntaxKind.BoolKeyword: return typeof(bool);
                case SyntaxKind.IntKeyword: return typeof(int);
                default: return typeof(object);
            }
        }
    }
}