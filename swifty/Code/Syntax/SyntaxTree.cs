using System.Collections.Generic;
using System.Linq;

namespace swifty.Code.Syntaxt {
    public sealed class SyntaxTree {
        public SyntaxTree(IEnumerable<string> diagnostics, ExpressionSyntax root, SyntaxToken endofFileToken) {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndofFileToken = endofFileToken;
        }
        public IReadOnlyList<string> Diagnostics {get;}
        public ExpressionSyntax Root {get;}
        public SyntaxToken EndofFileToken {get;}
        public static SyntaxTree Parse(string text) {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
}