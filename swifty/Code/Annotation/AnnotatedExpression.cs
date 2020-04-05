using System;

namespace swifty.Code.Annotation {
    internal abstract class AnnotatedExpression : AnnotatedNode {
        public abstract Type Type {get;}
    }
}