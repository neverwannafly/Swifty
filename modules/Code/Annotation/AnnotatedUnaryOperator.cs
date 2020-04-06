using swifty.Code.Syntaxt;
using System;

namespace swifty.Code.Annotation {    
    internal sealed class AnnotatedUnaryOperator {
        private AnnotatedUnaryOperator(SyntaxKind syntaxKind, AnnotatedUnaryOperatorKind kind, Type operandType) : this(syntaxKind, kind, operandType, operandType) {}
        private AnnotatedUnaryOperator(SyntaxKind syntaxKind, AnnotatedUnaryOperatorKind kind, Type operandType, Type resultType) {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OperandType = operandType;
            ResultType = resultType;
        }
        public SyntaxKind SyntaxKind {get;}
        public AnnotatedUnaryOperatorKind Kind {get;}
        public Type OperandType {get;}
        public Type ResultType {get;}
        private static AnnotatedUnaryOperator[] _operators = {
            new AnnotatedUnaryOperator(SyntaxKind.NotToken, AnnotatedUnaryOperatorKind.LogicalNegation, typeof(bool)),
            new AnnotatedUnaryOperator(SyntaxKind.PlusToken, AnnotatedUnaryOperatorKind.Identity, typeof(int)),
            new AnnotatedUnaryOperator(SyntaxKind.MinusToken, AnnotatedUnaryOperatorKind.Negation, typeof(int)),
        };
        public static AnnotatedUnaryOperator Annotate(SyntaxKind syntaxKind, Type operandType) {
            foreach (var op in _operators) {
                if (op.SyntaxKind==syntaxKind && op.OperandType==operandType) {
                    return op;
                }
            }
            return null;
        }
    }
}