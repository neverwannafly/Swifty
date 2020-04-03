prog: src/main.cpp src/lexer.cpp src/syntaxToken.cpp src/util.cpp
	g++ -std=c++14 src/main.cpp src/lexer.cpp src/syntaxToken.cpp src/util.cpp -o prog