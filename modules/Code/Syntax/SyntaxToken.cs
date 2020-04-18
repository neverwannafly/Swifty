using System.Collections.Generic;
using swifty.Code.Text;
using System.Linq;
using System;

namespace swifty.Code.Syntaxt {
    public class SyntaxToken : SyntaxNode {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value, Type type=null) {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
            Type = type;
        }
        public override SyntaxKind Kind {get;}
        public int Position {get;}
        public string Text {get;}
        public object Value {get;}
        public Type Type {get;}
        public override TextSpan Span => new TextSpan(Position, Text.Length);
        public override IEnumerable<SyntaxNode> GetChildren() {
            return Enumerable.Empty<SyntaxNode>();
        }
        public bool IsValidDeclaration() {
            if (Kind==SyntaxKind.VariableDeclarationStatement && Type!=null) {
                return true;
            }
            return true;
        }
    }
}