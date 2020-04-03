#include "syntaxToken.hpp"

SyntaxToken::SyntaxToken(SyntaxKind kind, int position, std::string text, int val) {
    this->kind = kind;
    this->position = position;
    this->text = text;
    this->val = val;
};