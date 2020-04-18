namespace swifty.Code.Syntaxt {
    internal static class SyntaxRules {
        internal static int GetBinaryOperatorPrecendence(this SyntaxKind kind) {
            switch(kind) {
                case SyntaxKind.DivideToken:    return 5;
                case SyntaxKind.StarToken:      return 5;
                case SyntaxKind.PlusToken:      return 4;
                case SyntaxKind.MinusToken:     return 4;
                case SyntaxKind.EqualToken:     return 3;
                case SyntaxKind.NotEqualToken:  return 3;
                case SyntaxKind.AndToken:       return 2;
                case SyntaxKind.OrToken:        return 1;
                default:                        return 0;
            }
        }
        internal static int GetUnaryOperatorPrecedence(this SyntaxKind kind) {
            switch(kind) {
                case SyntaxKind.NotToken:
                return 9;
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
                case "int": return SyntaxKind.IntKeyword;
                case "bool": return SyntaxKind.BoolKeyword;
                case "const": return SyntaxKind.ConstKeyword;
                default: return SyntaxKind.IdentifierToken;
            }
        }
    }
}