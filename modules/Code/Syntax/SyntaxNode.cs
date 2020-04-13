using System.Collections.Generic;
using System.Linq;

namespace swifty.Code.Syntaxt {
    public abstract class SyntaxNode {
        public virtual TextSpan Span {
            get {
                var first = GetChildren().First().Span;
                var last = GetChildren().Last().Span;
                return TextSpan.FromBound(first.Start, last.End);
            }
        }
        public abstract SyntaxKind Kind {get;}
        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}