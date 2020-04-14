using System.Collections.Immutable;

namespace swifty.Code.Annotation {
    internal sealed class AnnotationGlobalScope {
        public AnnotationGlobalScope(AnnotationGlobalScope prev, ImmutableArray<Diagnostic> diagnostics, ImmutableArray<VariableSymbol> symbols, AnnotatedStatement expression) {
            Previous = prev;
            Diagnostics = diagnostics;
            Symbols = symbols;
            Expression = expression;
        }
        public AnnotationGlobalScope Previous {get;}
        public ImmutableArray<Diagnostic> Diagnostics {get;}
        public ImmutableArray<VariableSymbol> Symbols {get;}
        public AnnotatedStatement Expression {get;}
    }
}