using System.Collections.Generic;
using System.Linq;

namespace swifty.Code {
    public sealed class EvaluationResult {
        public EvaluationResult(IEnumerable<string> diagnostics, object value) {
            Diagnostics = diagnostics.ToArray();
            Value = value;
        }
        public IReadOnlyList<string> Diagnostics {get;}
        public object Value {get;}
    }
}