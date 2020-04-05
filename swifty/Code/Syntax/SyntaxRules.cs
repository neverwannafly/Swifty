namespace swifty.Code.Syntaxt {
    internal static class SyntaxRules {
        internal static int GetBinaryOperatorPrecendence(this SyntaxKind kind) {
            switch(kind) {
                case SyntaxKind.DivideToken:    return 5;
                case SyntaxKind.StarToken:      return 4;
                case SyntaxKind.PlusToken:      return 3;
                case SyntaxKind.MinusToken:     return 3;
                case SyntaxKind.AndToken:       return 2;
                case SyntaxKind.OrToken:        return 1;
                default:                        return 0;
            }
        }
        internal static int GetUnaryOperatorPrecedence(this SyntaxKind kind) {
            switch(kind) {
                case SyntaxKind.AndToken:       return 8;
                case SyntaxKind.OrToken:        return 7;
                case SyntaxKind.PlusToken:      return 6;
                case SyntaxKind.MinusToken:     return 6;
                default:                        return 0;
            }
        }

        internal static SyntaxKind GetKeywordKind(string text) {
            switch(text) {
                case "true": return SyntaxKind.TrueKeyword;
                case "false": return SyntaxKind.FalseKeyword;
                default: return SyntaxKind.IdentifierToken;
            }
        }
    }
}