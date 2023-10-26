// using System;
// using System.Collections.Generic;
// using Minsk.CodeAnalysis.Syntax;

// namespace Minsk.CodeAnalysis.Binding
// {
//     internal sealed class Binder
//     {
//         private readonly List<string> _diagnostics = new List<string>();

//         public IEnumerable<string> Diagnostics => _diagnostics;

//         public BoundExpression BindExpression(ExpressionSyntax syntax)
//         {
//             switch (syntax.Kind)
//             {
//                 case SyntaxKind.LiteralExpression:
//                     return BindLiteralExpression((LiteralExpressionSyntax)syntax);
//                 case SyntaxKind.UnaryExpression:
//                     return BindUnaryExpression((UnaryExpressionSyntax)syntax);
//                 case SyntaxKind.BinaryExpression:
//                     return BindBinaryExpression((BinaryExpressionSyntax)syntax);
//                 default:
//                     throw new Exception($"Unexpected syntax {syntax.Kind}");
//             }
//         }

//         private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
//         {
//             var value = syntax.Value ?? 0;
//             return new BoundLiteralExpression(value);
//         }

//         private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
//         {
//             var boundOperand = BindExpression(syntax.Operand);
//             var boundOperatorKind = BindUnaryOperatorKind(syntax.OperatorToken.Kind, boundOperand.Type);

//             if (boundOperatorKind == null)
//             {
//                 _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}.");
//                 return boundOperand;
//             }

//             return new BoundUnaryExpression(boundOperatorKind.Value, boundOperand);
//         }

//         private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
//         {
//             var boundLeft = BindExpression(syntax.Left);
//             var boundRight = BindExpression(syntax.Right);
//             var boundOperatorKind = BindBinaryOperatorKind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

//             if (boundOperatorKind == null)
//             {
//                 _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}.");
//                 return boundLeft;
//             }

//             return new BoundBinaryExpression(boundLeft, boundOperatorKind.Value, boundRight);
//         }

//         private BoundUnaryOperatorKind? BindUnaryOperatorKind(SyntaxKind kind, Type operandType)
//         {
//             if (operandType != typeof(int))
//                 return  null;

//             switch (kind)
//             {
//                 case SyntaxKind.PlusToken:
//                     return BoundUnaryOperatorKind.Identity;
//                 case SyntaxKind.MinusToken:
//                     return BoundUnaryOperatorKind.Negation;
//                 default:
//                     throw new Exception($"Unexpected unary operator {kind}");
//             }
//         }

//         private BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType)
//         {
//             if (leftType != typeof(int) || rightType != typeof(int))
//                 return  null;

//             switch (kind)
//             {
//                 case SyntaxKind.PlusToken:
//                     return BoundBinaryOperatorKind.Addition;
//                 case SyntaxKind.MinusToken:
//                     return BoundBinaryOperatorKind.Subtraction;
//                 case SyntaxKind.MultiplicationToken:
//                     return BoundBinaryOperatorKind.Multiplication;
//                 case SyntaxKind.DivideToken:
//                     return BoundBinaryOperatorKind.Division;
//                 default:
//                     throw new Exception($"Unexpected binary operator {kind}");
//             }
//         }
//     }
// }
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
    // Beside the AST for parsing the logic, this Tree will store info
    // about types to enable type checking later on.

    // The binder walks our Already Existing Syntax Tree and creates this
    // structure.


    // Our construct to walk the (already existing) Syntax-Tree and create
    // our representation of Types
    internal sealed class Binder
    {
        // because our Binder might fail we collect _diagostics about errors and expose those with an IEnumerable     
        private readonly List<string> _diagnostics = new List<string>();
        public IEnumerable<string> Diagnostics => _diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);
                default:
                    throw new Exception($"ERROR: Unexpected syntayx {syntax.Kind} in Binder.BindExpression()");
            }
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);

        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperatorKind = BindUnaryOperatorKind(syntax.OperatorToken.Kind, boundOperand.Type);
            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type {boundOperand.Type}");
                return boundOperand;
            }
            return new BoundUnaryExpression(boundOperatorKind.Value, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperatorKind = BindBinaryOperatorKind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);
            if (boundOperatorKind == null)
            {
                _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}");
                return boundLeft;
            }
            return new BoundBinaryExpression(boundLeft, boundOperatorKind.Value, boundRight);
        }

        private BoundUnaryOperatorKind? BindUnaryOperatorKind(SyntaxKind kind, Type operandType)
        {
            if (operandType == typeof(int))
            {
                switch (kind)
                {
                    case SyntaxKind.PlusToken:
                        return BoundUnaryOperatorKind.Identity;
                    case SyntaxKind.MinusToken:
                        return BoundUnaryOperatorKind.Negation;
                }
            }
            if (operandType == typeof(bool))
            {
                switch (kind)
                {
                    case SyntaxKind.BangToken:
                        return BoundUnaryOperatorKind.LogicalNegation;
                }
            }
            return null;    // we have no info about what this could be here
            // throw new Exception($"ERROR: Unexpected unary operator {kind} in Binder.BindExpression()");
        }

        private BoundBinaryOperatorKind? BindBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType)
        {
            if (leftType == typeof(int) && rightType == typeof(int))
            {
                switch (kind)
                {
                    case SyntaxKind.PlusToken:
                        return BoundBinaryOperatorKind.Addition;
                    case SyntaxKind.MinusToken:
                        return BoundBinaryOperatorKind.Subtraction;
                    case SyntaxKind.MultiplicationToken:
                        return BoundBinaryOperatorKind.Multiplication;
                    case SyntaxKind.DivideToken:
                        return BoundBinaryOperatorKind.Division;
                }
            }
            if (leftType == typeof(bool) && rightType == typeof(bool))
            {
                switch (kind)
                {
                    case SyntaxKind.AmpersandAmpersandToken:
                        return BoundBinaryOperatorKind.LogicalAnd;
                    case SyntaxKind.PipePipeToken:
                        return BoundBinaryOperatorKind.LogicalOr;
                }
            }
            return null;    // we have no info about what this could be here
            // throw new Exception($"ERROR: Unexpected binary operator {kind} in Binder.BindExpression()");

        }
    }
}