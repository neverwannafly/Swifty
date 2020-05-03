using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using swifty.Code.Syntaxt;
using swifty.Code.Text;

namespace swifty.Code {
    internal sealed class DiagnosisHandler : IEnumerable<Diagnostic> {
        private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();
        private void Report(TextSpan span, string message) {
            var diagnostics = new Diagnostic(span, message);
            _diagnostics.Add(diagnostics);
        }
        public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void AddRange(DiagnosisHandler diagnostics) {
            _diagnostics.AddRange(diagnostics._diagnostics);
        }
        public void Concat(DiagnosisHandler diagnostics) {
            _diagnostics.Concat(diagnostics._diagnostics);
        }
        public void ReportInvalidNumber(TextSpan span, string text, Type type) {
            var message = $"LEXICAL_ERROR: The number {text} isnt valid {type}.";
            Report(span, message);
        }
        public void ReportBadCharacter(int position, char current) {
            var message = $"LEXICAL_ERROR: Bad character input : '{current}'.";
            var span = new TextSpan(position,1);
            Report(span, message);
        }
        public void ReportUnexpectedToken(TextSpan span, SyntaxKind expectedKind, SyntaxKind actualKind) {
            var message = $"SYNTACTIC_ERROR: Unexpected token <{actualKind}>, expected <{expectedKind}>.";
            Report(span, message);
        }
        public void ReportUndefinedUnaryOperator(TextSpan span, string text, Type type) {
            var message = $"SEMANTIC_ERROR: Unary operator '{text}' isnt defined for type {type}";
            Report(span, message);
        }
        public void ReportUndefinedBinaryOperator(TextSpan span, Type leftType, string text, Type rightType) {
            var message = $"SEMANTIC_ERROR: Binary operator '{text}' isnt defined for types {leftType} and {rightType}.";
            Report(span, message);
        }
        public void ReportUndefinedName(TextSpan span, string name) {
            var message = $"SEMANTIC_ERROR: Variable '{name}' doesnt exist.";
            Report(span, message);
        }
        public void ReportCannotConvert(TextSpan span, Type type1, Type type2) {
            var message = $"SEMANTIC_ERROR: Cannot convert type '{type1}' to '{type2}'";
            Report(span, message);
        }
        public void ReportVariableAlreadyDeclared(TextSpan span, string name) {
            var message = $"SEMANTIC_ERROR: Variable '{name}' is already declared.";
            Report(span, message);
        }
        public void ReportVariableReadOnly(TextSpan span, string var) {
            var message = $"SEMANTIC_ERROR: Variable '{var}' is declared as readonly and hence cannot be reassigned";
            Report(span, message);
        }
        public void ReportInvalidRightValue(TextSpan span, string name, Type expected, Type actual) {
            var message = $"SEMANTIC_ERROR: Inconsistent Lvalue and Rvalue for Variable '{name}', expected '{expected}' but got '{actual}'";
            Report(span, message);
        }
        public void ReportInvalidCharacter(TextSpan span, string text, Type type) {
            var message = $"LEXICAL_ERROR: The character {text} isnt valid {type}.";
            Report(span, message);
        }
    }
}