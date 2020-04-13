using System.Collections.Generic;
using System.Linq;
using System.IO;

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
        static void drawTree(TextWriter writer, SyntaxNode node, string indent="", bool isLast=true) {
            var marker = isLast? "└──" : "├──";
            writer.Write(indent);
            writer.Write(marker);
            writer.Write(node.Kind);
            if (node is SyntaxToken t && t.Value!=null) {
                writer.Write($" {t.Value}");
            }
            indent += isLast ? "   ":"|   ";
            writer.WriteLine();
            var lastChild = node.GetChildren().LastOrDefault();
            foreach (var child in node.GetChildren()) {
                drawTree(writer, child, indent, child==lastChild);
            }
        }
        public override string ToString() {
            using (var writer = new StringWriter()) {
                drawTree(writer, this);
                return writer.ToString();
            }
        }
    }
}