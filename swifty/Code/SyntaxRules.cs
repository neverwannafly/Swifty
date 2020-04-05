namespace swifty.Code {
    internal static class SyntaxRules {
        internal static int GetBinaryOperatorPrecendence(this SyntaxKind kind) {
            switch(kind) {
                case SyntaxKind.PlusToken:      return 1;
                case SyntaxKind.MinusToken:     return 1;
                case SyntaxKind.StarToken:      return 2;
                case SyntaxKind.DivideToken:    return 2;
                default: return 0;
            }
        }
    }
}