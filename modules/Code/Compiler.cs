using System;
using System.Linq;
using System.Collections.Generic;
using swifty.Code.Annotation;
using swifty.Code.Syntaxt;

namespace swifty.Code {
    public sealed class Compiler {
        public Compiler(SyntaxTree syntax) {
            Syntax = syntax;
        }
        public SyntaxTree Syntax {get;}
        public EvaluationResult EvaluationResult(Dictionary<VariableSymbol,object> symbolTable) {
            var annotator = new Annotator(symbolTable);
            var annotatedExpression = annotator.AnnotateExpression(Syntax.Root);
            var diagnostics = Syntax.Diagnostics.Concat(annotator.Diagnostics);
            if (diagnostics.Any()) {
                return new EvaluationResult(diagnostics, null);
            }
            var evaluator = new Evaluator(annotatedExpression, symbolTable);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<Diagnostic>(), value);
        }
    }
}