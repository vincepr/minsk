namespace Minsk.CodeAnalysis.Syntax
{
    // Stores information about the Syntax and how to parse and with what priority
    internal static class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                // higher nr gets parsed with priority
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 5;

                // while Precedence==0 -> no Binary Operator found
                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                // higher nr gets parsed with priority
                case SyntaxKind.MultiplicationToken:
                case SyntaxKind.DivideToken:
                    return 4;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 3;

                case SyntaxKind.AmpersandAmpersandToken:
                    return 2;

                case SyntaxKind.PipePipeToken:
                    return 1;

                // while Precedence==0 -> no Binary Operator found
                default:
                    return 0;
            }
        }

        // handles maping over different kinds of keywords (those should get prio over var-identifiers etc)
        public static SyntaxKind GetKeywordKind(string text)
        {
            switch (text)
            {
                case "true":
                    return SyntaxKind.TrueKeyword;
                case "false":
                    return SyntaxKind.FalseKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }
    }
}