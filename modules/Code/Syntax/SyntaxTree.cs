using System.Collections.Generic;
using System.Linq;

using swifty.Code.Text;

namespace swifty.Code.Syntaxt {
    public sealed class SyntaxTree {
        public SyntaxTree(SourceText sourceText, IEnumerable<Diagnostic> diagnostics, ExpressionSyntax root, SyntaxToken endofFileToken) {
            SourceText = sourceText;
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndofFileToken = endofFileToken;
        }
        public SourceText SourceText {get;}
        public IReadOnlyList<Diagnostic> Diagnostics {get;}
        public ExpressionSyntax Root {get;}
        public SyntaxToken EndofFileToken {get;}
        public static SyntaxTree Parse(string text) {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
            
        }
        private static SyntaxTree Parse(SourceText sourceText) {
            var parser = new Parser(sourceText);
            return parser.Parse();
        }
        public static IEnumerable<SyntaxToken> ParseToken(string text) {
            var sourceText = SourceText.From(text);
            return ParseToken(sourceText);
        }
        private static IEnumerable<SyntaxToken> ParseToken(SourceText text) {
            var lexer = new Lexer(text);
            while (true) {
                SyntaxToken token = lexer.Lex();
                if (token.Kind == SyntaxKind.EndofFileToken) break;
                yield return token;
            }
        }
    }
}