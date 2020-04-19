using Xunit;
using swifty.Code.Text;

namespace swifty.tests.Code.Text {
    public class SourceTextTest {

        [Theory]
        [InlineData(".",1)]
        [InlineData(".\r\n",2)]
        [InlineData(".\r\n\r\n",3)]
       public void SourceText_IncludesLastLine(string text, int expectedLineCount) {
           var st = SourceText.From(text);
           Assert.Equal(expectedLineCount, st.Lines.Length);
       }
    }
}