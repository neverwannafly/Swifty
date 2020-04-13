namespace swifty.Code.Text {
    public sealed class TextLine {
        // EOL : End of line length marker.
        public TextLine(SourceText text, int start, int length, int eolLength) {
            Text = text;
            Start = start;
            Length = length;
            EOLlength = eolLength;
        }
        public SourceText Text {get;}
        public int Start {get;}
        public int Length {get;}
        public int EOLlength {get;}
        public TextSpan Span => new TextSpan(Start, Length);
        public TextSpan EOLSpan => new TextSpan(Start, EOLlength);
        public override string ToString() => Text.ToString(Span);
        public string ToString(int start, int length) => Text.ToString(start, length);
        public string ToString(TextSpan span) => Text.ToString(span.Start, span.Length);
    }
}