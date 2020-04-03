#include "util.hpp"

bool isDigit(char ch) {
    return '0'<=ch && ch<='9';
}
bool isWhitespace(char ch) {
    return std::isspace(static_cast<unsigned char>(ch));
}
bool tryIntegerParse(std::string text, int &value) {
    bool conversion = true;
    try {
        value = std::stoi(text);
    } catch(const std::exception &err) {
        value = 0;
        conversion = false;
    }
    return conversion;
}