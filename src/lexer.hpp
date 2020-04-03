#ifndef lexer_hpp
#define lexer_hpp

#include "syntaxToken.hpp"
#include "util.hpp"

class Lexer {
private:
    std::string _text;
    int _position;
    char current;
public:
    Lexer(std::string &text);
    void next();
    SyntaxToken *nextToken();
    char getCurrent();
};

#endif