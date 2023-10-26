namespace Minsk.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {   
        // Tokens:
        BadToken,
        EOFToken,
        WhitespaceToken,
        NumberToken,        // represents i32 at the moment
        IdentifierToken,    // variable/class names etc

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
        ParenthesizedExpressionSyntax,
        UnaryExpression,
        
        // Keywords
        FalseKeyword,
        TrueKeyword,
    }
}