using System;
using System.Linq;
using System.Collections.Generic;
using swifty.Code.Syntaxt;
using System.Collections.Immutable;

namespace swifty.Code.Annotation {    
    sealed class Annotator {
        private AnnotationScope _scope;
        private readonly DiagnosisHandler _diagnostics = new DiagnosisHandler();
        public Annotator(AnnotationScope parent) {
            _scope = new AnnotationScope(parent);
        }
        public static AnnotationGlobalScope AnnotateGlobalScope(AnnotationGlobalScope previous, CompilationUnitSyntax syntax) {
            var parentScope = CreateParentScope(previous);
            var annotator = new Annotator(parentScope);
            var expression = annotator.AnnotateExpression(syntax.Expression);
            var symbols = annotator._scope.GetDeclaredVariables();
            var diagnostics = annotator.Diagnostics.ToImmutableArray();
            if (previous != null) {
                diagnostics = diagnostics.InsertRange(0, previous.Diagnostics);
            }
            return new AnnotationGlobalScope(previous, diagnostics, symbols, expression);
        }
        private static AnnotationScope CreateParentScope(AnnotationGlobalScope previous) {
            var stack = new Stack<AnnotationGlobalScope>();
            while (previous != null) {
                stack.Push(previous);
                previous = previous.Previous;
            }
            AnnotationScope parent = null;
            while (stack.Count > 0) {
                previous = stack.Pop();
                var scope = new AnnotationScope(parent);
                foreach (var v in previous.Symbols) {
                    scope.TryDeclare(v);
                }
                parent = scope;
            }
            return parent;
        }
        public DiagnosisHandler Diagnostics => _diagnostics;
        public AnnotatedExpression AnnotateExpression(ExpressionSyntax syntax) {
            switch(syntax.Kind) {
                case SyntaxKind.BinaryExpression: return AnnotateBinaryExpression((BinaryExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression: return AnnotateUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.LiteralExpression: return AnnotateLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.ParathesisExpression: return AnnotateParanthesisExpression((ParanthesisExpressionSyntax)syntax);
                case SyntaxKind.NameExpression: return AnnotateNameExpression((NameExpressionSyntax)syntax);
                case SyntaxKind.AssignmentExpression: return AnnotateAssignmentExpression((AssignmentExpressionSyntax)syntax);
                default: throw new Exception($"Unexpected Syntax {syntax.Kind}");
            }
        }
        public AnnotatedExpression AnnotateNameExpression(NameExpressionSyntax syntax) {
            var name = syntax.IdentifierToken.Text;
            _scope.TryLookup(name, out var symbol);
            if (symbol == null) {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return new AnnotatedLiteralExpression(0);
            }
            return new AnnotatedVariableExpression(symbol);
        }
        public AnnotatedExpression AnnotateAssignmentExpression(AssignmentExpressionSyntax syntax) {
            var name = syntax.IdentifierToken.Text;
            var annotatedExpression = AnnotateExpression(syntax.Expression);
            var symbol = new VariableSymbol(name, annotatedExpression.Type);

            if (!_scope.TryDeclare(symbol)) {
                _diagnostics.ReportVariableAlreadyDeclared(syntax.IdentifierToken.Span, name);
            }
            return new AnnotatedAssignmentExpression(symbol, annotatedExpression);
        }
        public AnnotatedExpression AnnotateParanthesisExpression(ParanthesisExpressionSyntax syntax) {
            return AnnotateExpression(syntax.Expression);
        }
        public AnnotatedExpression AnnotateBinaryExpression(BinaryExpressionSyntax syntax) {
            var annotateLeft = AnnotateExpression(syntax.Left);
            var annotateRight = AnnotateExpression(syntax.Right);
            var annotateOperatorKind = AnnotatedBinaryOperator.Annotate(syntax.OperatorToken.Kind, annotateLeft.Type, annotateRight.Type);
            if (annotateOperatorKind==null) {
                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, annotateLeft.Type,syntax.OperatorToken.Text, annotateRight.Type);
                return annotateRight;
            }
            return new AnnotatedBinaryExpression(annotateLeft, annotateOperatorKind, annotateRight);
        }
        public AnnotatedExpression AnnotateUnaryExpression(UnaryExpressionSyntax syntax) {
            var annotateOperand = AnnotateExpression(syntax.Operand);
            var annotateOperatorKind = AnnotatedUnaryOperator.Annotate(syntax.OperatorToken.Kind, annotateOperand.Type);
            if (annotateOperatorKind==null) {
                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, annotateOperand.Type);
                return annotateOperand;
            }
            return new AnnotatedUnaryExpression(annotateOperatorKind, annotateOperand);
        }
        public AnnotatedExpression AnnotateLiteralExpression(LiteralExpressionSyntax syntax) {
            var value = syntax.Value ?? 0;
            return new AnnotatedLiteralExpression(value);
        }
    }
}