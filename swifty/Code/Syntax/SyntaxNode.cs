using System.Collections.Generic;

namespace swifty.Code.Syntaxt {
    public abstract class SyntaxNode {
        public abstract SyntaxKind Kind {get;}
        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}