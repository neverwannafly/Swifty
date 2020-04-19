using System;
using System.Linq;
using System.Collections.Generic;
using swifty.Code.Annotation;
using swifty.Code.Syntaxt;
using System.Threading;

namespace swifty.Code {
    public sealed class Compiler {
        private AnnotationGlobalScope _global;
        public Compiler(SyntaxTree syntax) : this(null, syntax) {
            
        }
        private Compiler(Compiler prev, SyntaxTree syntax) {
            Previous = prev;
            Syntax = syntax;
        }
        public SyntaxTree Syntax {get;}
        public Compiler Previous {get;}
        internal AnnotationGlobalScope GlobalScope {
            get {
                if (_global == null) {
                    // Only the first thread on seeing _global as null can set it's value. This is done to induce thread safety.
                    var global = Annotator.AnnotateGlobalScope(Previous?.GlobalScope, Syntax.Root);
                    Interlocked.CompareExchange(ref _global, global, null);
                }
                return _global;
            }
        }
        public Compiler ContinueWith(SyntaxTree syntaxTree) {
            return new Compiler(this, syntaxTree);
        }
        public EvaluationResult EvaluationResult(Dictionary<VariableSymbol,object> symbolTable) {
            var diagnostics = Syntax.Diagnostics.Concat(GlobalScope.Diagnostics);
            if (diagnostics.Any()) {
                return new EvaluationResult(diagnostics, null);
            }
            var evaluator = new Evaluator(GlobalScope.Expression, symbolTable);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<Diagnostic>(), value);
        }
    }
}