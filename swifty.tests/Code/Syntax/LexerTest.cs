using swifty.Code.Syntaxt;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace swifty.tests.Code.Syntax {
    public class LexerTest {
        [Theory]
        [MemberData(nameof(GetTokenData))]
        public void Token_Test(SyntaxKind kind, string text) {
            var tokens = SyntaxTree.ParseToken(text);
            var token = Assert.Single(tokens);
            Assert.Equal(kind, token.Kind);
            Assert.Equal(text, token.Text);
        }
        [Theory]
        [MemberData(nameof(GetTokenPairData))]
        public void Token_Pair_Test(SyntaxKind kind1, string text1, SyntaxKind kind2, string text2) {
            var text = text1 + text2;
            var tokens = SyntaxTree.ParseToken(text).ToArray();
            Assert.Equal(2, tokens.Length);
            Assert.Equal(tokens[0].Kind, kind1);
            Assert.Equal(tokens[0].Text, text1);
            Assert.Equal(tokens[1].Kind, kind2);
            Assert.Equal(tokens[1].Text, text2);
        }
        public static IEnumerable<Object[]> GetTokenData() {
            foreach (var t in GetToken()) {
                yield return new object[] {t.kind, t.text};
            }
        }
        public static IEnumerable<Object[]> GetTokenPairData() {
            foreach(var t in GetTokenPair()) {
                yield return new object[] { t.kind1, t.text1, t.kind2, t.text2 };
            }
        }
        private static bool RequireSeperator(SyntaxKind kind1, SyntaxKind kind2) {
            if (kind1==SyntaxKind.IdentifierToken || kind2==SyntaxKind.IdentifierToken) {
                return true;
            }
            var kw1 = kind1.ToString().EndsWith("Keyword");
            var kw2 = kind2.ToString().EndsWith("Keyword");
            if (kw1 && kw2) {
                return true;
            }
            if (kw1 && kind2==SyntaxKind.IdentifierToken) {
                return true;
            }
            if (kind1==SyntaxKind.IdentifierToken && kw2) {
                return true;
            }
            if (kind1==SyntaxKind.NumberToken && kind2==SyntaxKind.NumberToken) {
                return true;
            }
            if (kind1==SyntaxKind.NotToken || kind2==SyntaxKind.NotToken) {
                return true;
            }
            return false;
        }
        private static IEnumerable<(SyntaxKind  kind, string text)> GetToken() {
            return new[] {
                (SyntaxKind.IdentifierToken, "mango"),
                (SyntaxKind.IdentifierToken, "repl"),
                (SyntaxKind.IdentifierToken, "quora"),
                (SyntaxKind.NumberToken, "12321"),
                (SyntaxKind.NumberToken, "192"),
                (SyntaxKind.NumberToken, "000121"),
                (SyntaxKind.WhitespaceToken, " "),
                (SyntaxKind.WhitespaceToken, "\n"),
                (SyntaxKind.WhitespaceToken, "\r"),
                (SyntaxKind.WhitespaceToken, "\r\n"),
                (SyntaxKind.WhitespaceToken, "      "),
                (SyntaxKind.PlusToken, "+"),
                (SyntaxKind.MinusToken, "-"),
                (SyntaxKind.StarToken, "*"),
                (SyntaxKind.DivideToken, "/"),
                (SyntaxKind.EqualToken, "=="),
                (SyntaxKind.NotEqualToken, "!="),
                (SyntaxKind.NotToken, "!"),
                (SyntaxKind.LogicalAndToken, "&&"),
                (SyntaxKind.LogicalOrToken, "||"),
                (SyntaxKind.AssignmentToken, ":="),
                // (SyntaxKind.XorToken, "^"),
                (SyntaxKind.LeftParanthesisToken, "("),
                (SyntaxKind.RightParanthesisToken, ")"),
                // Keywords
                (SyntaxKind.TrueKeyword, "true"),
                (SyntaxKind.FalseKeyword, "false"),
            };
        }
        private static IEnumerable<(SyntaxKind  kind1, string text1, SyntaxKind  kind2, string text2)> GetTokenPair() {
            foreach(var t1 in GetToken()) {
                if (t1.kind!=SyntaxKind.WhitespaceToken) {
                    foreach(var t2 in GetToken()) {
                        if (!RequireSeperator(t1.kind, t2.kind)) {
                            yield return (t1.kind, t1.text, t2.kind, t2.text);
                        }
                    }
                }
            }
        }
    }
}
