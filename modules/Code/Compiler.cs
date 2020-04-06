using System;
using System.Linq;
using swifty.Code.Annotation;
using swifty.Code.Syntaxt;

namespace swifty.Code {
    public sealed class Compiler {
        public Compiler(SyntaxTree syntax) {
            Syntax = syntax;
        }
        public SyntaxTree Syntax {get;}
        public EvaluationResult EvaluationResult() {
            var annotator = new Annotator();
            var annotatedExpression = annotator.AnnotateExpression(Syntax.Root);
            var diagnostics = Syntax.Diagnostics.Concat(annotator.Diagnostics).ToArray();
            if (diagnostics.Any()) {
                return new EvaluationResult(diagnostics, null);
            }
            var evaluator = new Evaluator(annotatedExpression);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<string>(), value);
        }
    }
}