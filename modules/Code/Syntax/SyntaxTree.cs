using System.Collections.Generic;
using System.Linq;

using swifty.Code.Text;

namespace swifty.Code.Syntaxt {
    public sealed class SyntaxTree {
        private SyntaxTree(SourceText sourceText) {
            var parser = new Parser(sourceText);
            Root = parser.ParseCompilationUnit();
            Diagnostics = parser.Diagnostics.ToArray();
            SourceText = sourceText;
        }
        public SourceText SourceText {get;}
        public IReadOnlyList<Diagnostic> Diagnostics {get;}
        public CompilationUnitSyntax Root {get;}
        public static SyntaxTree Parse(string text) {
            var sourceText = SourceText.From(text);
            return Parse(sourceText);
        }
        private static SyntaxTree Parse(SourceText sourceText) {
            return new SyntaxTree(sourceText);
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