using System.Collections.Generic;
using System.Collections.Immutable;

namespace swifty.Code.Annotation {
    internal sealed class AnnotationScope {
        private Dictionary<string, VariableSymbol> _symbolTable = new Dictionary<string, VariableSymbol>();
        public AnnotationScope(AnnotationScope parent) {
            Parent = parent;
        }
        public AnnotationScope Parent {get;}
        public bool TryDeclare(VariableSymbol symbol) {
            if (_symbolTable.ContainsKey(symbol.Name)) {
                return false;
            }
            _symbolTable.Add(symbol.Name, symbol);
            return true;
        }
        public bool TryLookup(string name, out VariableSymbol symbol) {
            if (_symbolTable.TryGetValue(name, out symbol)) {
                return true;
            }
            if (Parent == null) {
                return false;
            }
            return Parent.TryLookup(name, out symbol);
        }
        public ImmutableArray<VariableSymbol> GetDeclaredVariables() {
            return _symbolTable.Values.ToImmutableArray();
        }
    }
}