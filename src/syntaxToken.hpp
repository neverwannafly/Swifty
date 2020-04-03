#ifndef syntaxtoken_hpp
#define syntaxtoken_hpp

#include "util.hpp"

class SyntaxToken{
public:
    SyntaxKind kind;
    int position;
    std::string text;
    int val;
    SyntaxToken(SyntaxKind kind, int position, std::string text, int val);
};

#endif