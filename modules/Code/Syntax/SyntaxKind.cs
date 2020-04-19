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
        LogicalAndToken,
        LogicalOrToken,
        AssignmentToken,
        XorToken,
        LessThanToken,
        LessThanEqualToken,
        GreaterThanToken,
        GreaterThanEqualToken,
        LeftParanthesisToken,
        RightParanthesisToken,
        OpenBraceToken,
        CloseBraceToken,
        EndofFileToken,
        IdentifierToken,
        BadToken,
        // Keywords
        TrueKeyword,
        FalseKeyword,
        IntKeyword,
        BoolKeyword,
        ConstKeyword,
        // Nodes
        CompilationUnit,
        // Statements
        BlockStatement,
        ExpressionStatement,
        VariableDeclarationStatement,
        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParathesisExpression,
        UnaryExpression,
        NameExpression,
        AssignmentExpression,
    }
}