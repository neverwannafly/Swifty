using System;
using swifty.Code.Syntaxt;
using System.Collections.Generic;

namespace swifty.Code.Annotation {    
    sealed class Annotator {
        private readonly List<string> _diagnostics = new List<string>();
        public IEnumerable<string> Diagnostics => _diagnostics;
        public AnnotatedExpression AnnotateExpression(ExpressionSyntax syntax) {
            switch(syntax.Kind) {
                case SyntaxKind.BinaryExpression: return AnnotateBinaryExpression((BinaryExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression: return AnnotateUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.LiteralExpression: return AnnotateLiteralExpression((LiteralExpressionSyntax)syntax);
                default: throw new Exception($"Unexpected Syntax {syntax.Kind}");
            }
        }
        public AnnotatedExpression AnnotateBinaryExpression(BinaryExpressionSyntax syntax) {
            var annotateLeft = AnnotateExpression(syntax.Left);
            var annotateRight = AnnotateExpression(syntax.Right);
            var annotateOperatorKind = AnnotateBinaryOperatorKind(syntax.OperatorToken.Kind, annotateLeft.Type, annotateRight.Type);
            if (annotateOperatorKind==null) {
                _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' isnt defined for types {annotateLeft.Type} and {annotateRight.Type}");
                return annotateRight;
            }
            // .Value return value of a nullable class
            return new AnnotatedBinaryExpression(annotateLeft, annotateOperatorKind.Value, annotateRight);
        }
        public AnnotatedExpression AnnotateUnaryExpression(UnaryExpressionSyntax syntax) {
            var annotateOperand = AnnotateExpression(syntax.Operand);
            var annotateOperatorKind = AnnotateUnaryOperatorKind(syntax.OperatorToken.Kind, annotateOperand.Type);
            if (annotateOperatorKind==null) {
                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' isnt defined for type {annotateOperand.Type}");
                return annotateOperand;
            }
            // .Value return value of a nullable class
            return new AnnotatedUnaryExpression(annotateOperatorKind.Value, annotateOperand);
        }
        public AnnotatedExpression AnnotateLiteralExpression(LiteralExpressionSyntax syntax) {
            var value = syntax.Value ?? 0;
            return new AnnotatedLiteralExpression(value);
        }
        public AnnotatedBinaryOperatorKind? AnnotateBinaryOperatorKind(SyntaxKind kind, Type leftType, Type rightType) {
            if (leftType!=typeof(int) || rightType!=typeof(int)) {
                return null;
            }
            switch(kind) {
                case SyntaxKind.PlusToken: return AnnotatedBinaryOperatorKind.Addition;
                case SyntaxKind.MinusToken: return AnnotatedBinaryOperatorKind.Subtraction;
                case SyntaxKind.StarToken: return AnnotatedBinaryOperatorKind.Multiplication;
                case SyntaxKind.DivideToken: return AnnotatedBinaryOperatorKind.Division;
                default: throw new Exception($"Unexpected binary operator {kind}");
            }
        }
        public AnnotatedUnaryOperatorKind? AnnotateUnaryOperatorKind(SyntaxKind kind, Type type) {
            if (type != typeof(int)) return null;
            switch(kind) {
                case SyntaxKind.PlusToken: return AnnotatedUnaryOperatorKind.Identity;
                case SyntaxKind.MinusToken: return AnnotatedUnaryOperatorKind.Negation;
                default: throw new Exception($"Unexpected unary operator {kind}");
            }
        }
    }
}