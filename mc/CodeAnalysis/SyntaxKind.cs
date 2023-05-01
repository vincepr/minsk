namespace Minsk.CodeAnalysis
{
    public enum SyntaxKind
    {   
        // Tokens:
        BadToken,
        EOFToken,
        WhitespaceToken,
        NumberToken,        // i32 at the moment

        // Operators
        PlusToken,
        MinusToken,
        MultiplicationToken,
        DivideToken,
        OpenParenthesisToken,
        CloseParenthesisToken,

        // Expressions
        LiteralExpression,
        BinaryExpression,
        ParenthesizedExpressionSyntax
    }
}