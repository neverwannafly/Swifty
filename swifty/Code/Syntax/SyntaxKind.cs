namespace swifty.Code.Syntaxt {
    public enum SyntaxKind {
        // Tokens
        NumberToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        DivideToken,
        NotToken,
        AndToken,
        OrToken,
        XorToken,
        LeftParanthesisToken,
        RightParanthesisToken,
        EndofFileToken,
        IdentifierToken,
        BadToken,
        // Keywords
        TrueKeyword,
        FalseKeyword,
        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParathesisExpression,
        UnaryExpression,
    }
}