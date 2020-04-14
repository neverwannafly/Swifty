using System.Collections.Generic;
using swifty.Code.Text;
using System.Linq;

namespace swifty.Code.Syntaxt {
    public class SyntaxToken : SyntaxNode {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value) {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }
        public override SyntaxKind Kind {get;}
        public int Position {get;}
        public string Text {get;}
        public object Value {get;}
        public override TextSpan Span => new TextSpan(Position, Text.Length);
        public override IEnumerable<SyntaxNode> GetChildren() {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}