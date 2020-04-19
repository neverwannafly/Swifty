using Xunit;
using System.Collections.Generic;
using swifty.Code;
using swifty.Code.Syntaxt;

namespace swifty.tests.Code.Text {
    public class EvaluatorTest {

        [Theory]
        [InlineData("1",1)]
        [InlineData("true",true)]
        [InlineData("false",false)]
       public void Evaluator_Computes_Correct_Value(string text, object expectedValue) {
           var syntaxTree = SyntaxTree.Parse(text);
           var compiler = new Compiler(syntaxTree);
           var variables = new Dictionary<VariableSymbol,object>();
           var res = compiler.EvaluationResult(variables);

           Assert.Empty(res.Diagnostics);
           Assert.Equal(expectedValue, res.Value);
       }
    }
}