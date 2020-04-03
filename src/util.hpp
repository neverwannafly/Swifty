#ifndef util_h
#define util_h

#include <iostream>
#include <cctype>
#include <exception>

enum SyntaxKind{
    NumberToken,
    WhitespaceToken,
    PlusToken,
    MinusToken,
    MultiplyToken,
    DivideToken,
    LeftParanthesisToken,
    RightParanthesisToken,
    BadToken,
    EndofFileToken
};
bool isDigit(char ch);
bool isWhitespace(char ch);
bool tryIntegerParse(std::string text, int &value);

#endif