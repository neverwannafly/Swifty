using System.Collections.Generic;
using System.Linq;

namespace swifty.Code.Syntaxt {
    public sealed class SyntaxTree {
        public SyntaxTree(IEnumerable<Diagnostic> diagnostics, ExpressionSyntax root, SyntaxToken endofFileToken) {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndofFileToken = endofFileToken;
        }
        public IReadOnlyList<Diagnostic> Diagnostics {get;}
        public ExpressionSyntax Root {get;}
        public SyntaxToken EndofFileToken {get;}
        public static SyntaxTree Parse(string text) {
            var parser = new Parser(text);
            return parser.Parse();
        }
        public static IEnumerable<SyntaxToken> ParseToken(string text) {
            var lexer = new Lexer(text);
            while (true) {
                SyntaxToken token = lexer.Lex();
                if (token.Kind == SyntaxKind.EndofFileToken) break;
                yield return token;
            }
        }
    }
}