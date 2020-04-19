using System;

namespace swifty.Code {
    public sealed class VariableSymbol {
        internal VariableSymbol(string name, Type type, bool isReadonly) {
            Name = name;
            Type = type;
            IsReadOnly = isReadonly;
        }
        public string Name {get;}
        public Type Type {get;}
        public bool IsReadOnly {get;}
    }
}