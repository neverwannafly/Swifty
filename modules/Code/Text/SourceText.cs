using System.Collections.Immutable;

namespace swifty.Code.Text {
    public struct SourceText {
        private readonly string _text;
        private SourceText(string text) {
            _text = text;
            Lines = ParseLines(this, text);
        }
        public ImmutableArray<TextLine> Lines {get;}
        public char this[int index] => _text[index];
        public int Length => _text.Length;
        private static ImmutableArray<TextLine> ParseLines(SourceText sourceText, string text) {
            var result = ImmutableArray.CreateBuilder<TextLine>();
            var position = 0;
            var lineStart = 0;
            while (position < text.Length) {
                var lineBreakWidth = GetLineBreakWidth(text, position);
                if (lineBreakWidth == 0) {
                    position++;
                } else {
                    AddLine(sourceText, position, lineStart, lineBreakWidth, result);
                    position += lineBreakWidth;
                    lineStart = position;
                }
            }
            if (position >= lineStart) {
                AddLine(sourceText, position, lineStart, 0, result);
            }
            return result.ToImmutable();
        }
        public int GetLineIndex(int position) {
            var lower = 0;
            var upper = Lines.Length-1;
            while (lower <= upper) {
                var mid = (lower+upper)/2;
                var start = Lines[mid].Start;
                if (start==position) {
                    return mid;
                }
                else if (start < position) {
                    lower = mid + 1;
                } else {
                    upper = mid - 1;
                }
            }
            return lower-1;
        }
        private static void AddLine(SourceText sourceText, int position, int lineStart, int lineBreakWidth, ImmutableArray<TextLine>.Builder res) {
            var lineLength = position - lineStart;
            var eolLength = lineLength + lineBreakWidth;
            var line = new TextLine(sourceText, lineStart, lineLength, eolLength);
            res.Add(line);
        }
        private static int GetLineBreakWidth(string text, int i) {
            var ch = i>=text.Length ? '\0' : text[i];
            var lookAhead = i+1>=text.Length ? '\0' : text[i+1];
            if (ch=='\r' && lookAhead=='\n') return 2;
            if (ch=='\r' || ch=='\n') return 1;
            return 0;
        }
        public static SourceText From(string text) {
            return new SourceText(text);
        }
        public override string ToString() => _text;
        public string ToString(int start, int length) => _text.Substring(start, length);
        public string ToString(TextSpan span) => _text.Substring(span.Start, span.Length);

    }
}