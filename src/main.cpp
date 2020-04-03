#include "lexer.hpp"

int main(int argc, char **argv) {
    while (true) {
        std::cout << " > ";
        std::string line;
        std::getline(std::cin, line);
        Lexer *lexer = new Lexer(line);
        while (true) {
            SyntaxToken *token = lexer->nextToken();
            if (token->kind == EndofFileToken) {
                break;
            }
            std::cout << token->kind << ": " << token->text;
            if (token->val!=(int)NULL) {
                std::cout << " (" << token->val << ")";
            }
            std::cout << std::endl;
        }
    }
    return 0;
}