namespace swifty.Code.Syntaxt {
    public enum SyntaxKind {
        // Tokens
        NumberToken,
        WhitespaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        DivideToken,
        EqualToken,
        NotEqualToken,
        NotToken,
        AndToken,
        OrToken,
        AssignmentToken,
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
        NameExpression,
        AssignmentExpression,
    }
}