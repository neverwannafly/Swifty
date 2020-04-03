#include "lexer.hpp"

Lexer::Lexer(std::string &text) {
    _text = text;
}
SyntaxToken *Lexer::nextToken() {
    // detect if digit
    if (_position >= _text.length()) {
        return new SyntaxToken(EndofFileToken, _position, "\0", 0);
    }
    if (isDigit(getCurrent())) {
        int start = _position;
        while (isDigit(getCurrent())) next();
        int length = _position - start;
        std::string text = _text.substr(start, length);
        int val; tryIntegerParse(text, val);
        return new SyntaxToken(NumberToken, start, text, val);
    }
    if (isWhitespace(getCurrent())) {
        int start = _position;
        while (isWhitespace(getCurrent())) next();
        int length = _position - start;
        std::string text = _text.substr(start, length);
        int val; tryIntegerParse(text, val);
        return new SyntaxToken(WhitespaceToken, start, text, val);
    }
    switch(getCurrent()) {
        case '+': return new SyntaxToken(PlusToken, _position++, "+", 0);
        case '-': return new SyntaxToken(MinusToken, _position++, "-", 0);
        case '*': return new SyntaxToken(MultiplyToken, _position++, "*", 0);
        case '/': return new SyntaxToken(DivideToken, _position++, "/", 0);
        case '(': return new SyntaxToken(LeftParanthesisToken, _position++, "(", 0);
        case ')': return new SyntaxToken(RightParanthesisToken, _position++, ")", 0);
        default: _position++; return new SyntaxToken(BadToken, _position-1, _text.substr(_position-1, 1), 0);
    }
}
void Lexer::next() {
    _position++;
}
char Lexer::getCurrent() {
    if (_position >= _text.length()) { return '\0'; }
    return _text[_position];
}