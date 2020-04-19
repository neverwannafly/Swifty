using System;
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
            var expression = annotator.AnnotateStatement(syntax.Statement);
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
        public AnnotatedExpression AnnotateExpression(ExpressionSyntax syntax, Type targetType) {
            var result = AnnotateExpression(syntax);
            if (result.Type != targetType) {
                _diagnostics.ReportCannotConvert(syntax.Span, result.Type, targetType);
            }
            return result;
        }
        public AnnotatedStatement AnnotateStatement(StatementSyntax syntax) {
            switch(syntax.Kind) {
                case SyntaxKind.BlockStatement: return AnnotateBlockStatement((BlockStatementSyntax)syntax);
                case SyntaxKind.ExpressionStatement: return AnnotateExpressionStatement((ExpressionStatementSyntax)syntax);
                case SyntaxKind.VariableDeclarationStatement: return AnnotateVariableDeclaration((VariableDeclarationSyntax)syntax);
                case SyntaxKind.IfStatementSyntax: return AnnotateIfStatement((IfStatementSyntax)syntax);
                case SyntaxKind.WhileStatementSyntax: return AnnotateWhileStatement((WhileStatementSyntax)syntax);
                case SyntaxKind.ForStatementSyntax: return AnnotateForStatement((ForStatementSyntax)syntax);
                default: throw new Exception($"Unexpected Syntax {syntax.Kind}");
            }
        }
        private AnnotatedStatement AnnotateForStatement(ForStatementSyntax statement) {
            _scope = new AnnotationScope(_scope);

            var lowerBound = AnnotateExpression(statement.LowerBound, typeof(int));
            var upperBound = AnnotateExpression(statement.UpperBound, typeof(int));
            var name = statement.Identifier.Text;
            var variable = new VariableSymbol(name, typeof(int), false);
            _scope.TryDeclare(variable);
            var body = AnnotateStatement(statement.Body);

            _scope = _scope.Parent;
            return new AnnotatedForStatement(variable, lowerBound, upperBound, body);
        }
        private AnnotatedStatement AnnotateWhileStatement(WhileStatementSyntax statement) {
            var condition = AnnotateExpression(statement.Condition, typeof(bool));
            var body = AnnotateStatement(statement.Body);
            return new AnnotatedWhileStatement(condition, body);
        }
        private AnnotatedStatement AnnotateIfStatement(IfStatementSyntax statement) {
            var condition = AnnotateExpression(statement.Condition, typeof(bool));
            var thenStatement = AnnotateStatement(statement.ThenStatement);
            var elseStatement = statement.ElseClause == null ? null : AnnotateStatement(statement.ElseClause.ElseStatement);
            return new AnnotatedIfStatement(condition, thenStatement, elseStatement) ;
        }
        public AnnotatedStatement AnnotateVariableDeclaration(VariableDeclarationSyntax statement) {
            var expression = AnnotateExpression(statement.Initializer);
            var name = statement.Identifier.Text;
            var variable = new VariableSymbol(name, expression.Type, statement.IsReadOnly);
            if (!_scope.TryDeclare(variable)) {
                _diagnostics.ReportVariableAlreadyDeclared(statement.Identifier.Span, name);
            }
            return new AnnotateVariableDeclaration(variable, expression);
        }
        private AnnotatedStatement AnnotateBlockStatement(BlockStatementSyntax statement) {
            var statements = ImmutableArray.CreateBuilder<AnnotatedStatement>();
            _scope = new AnnotationScope(_scope);
            foreach (var statementSyntax in statement.Statements) {
                var st =  AnnotateStatement(statementSyntax);
                statements.Add(st);
            }
            _scope = _scope.Parent;
            return new AnnotatedBlockStatement(statements.ToImmutable());
        }
        public AnnotatedStatement AnnotateExpressionStatement(ExpressionStatementSyntax statement) {
            var expression = AnnotateExpression(statement.Expression);
            return new AnnotatedExpressionStatement(expression);
        }
        public AnnotatedExpression AnnotateNameExpression(NameExpressionSyntax syntax) {
            var name = syntax.IdentifierToken.Text;
            if (string.IsNullOrEmpty(name)) {
                return new AnnotatedLiteralExpression(0);
            }
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

            if (!_scope.TryLookup(name, out var symbol)) {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.Span, name);
                return annotatedExpression;
            }

            // check if variable is readonly and dont assign anything
            if (symbol.IsReadOnly) {
                _diagnostics.ReportVariableReadOnly(syntax.EqualToken.Span, syntax.IdentifierToken.Text);
                return annotatedExpression;
            }

            if (annotatedExpression.Type != symbol.Type) {
                _diagnostics.ReportCannotConvert(syntax.Expression.Span, annotatedExpression.Type, symbol.Type);
                return annotatedExpression;
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