using System;
using swifty.Code;
using swifty.Code.Syntaxt;
using System.Linq;

namespace swifty {
    internal static class Program {
        private static void Main() {
            while (true) {
                Console.Write("> ");
                string line = Console.ReadLine();
                var syntaxTree = SyntaxTree.Parse(line);
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                print(syntaxTree.Root);
                Console.ForegroundColor = color;

                if (!syntaxTree.Diagnostics.Any()) {
                    Evaluator eval = new Evaluator(syntaxTree.Root);
                    int result = eval.Evaluate();
                    Console.WriteLine(result);
                } else {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach(var diagnostic in syntaxTree.Diagnostics) {
                        Console.WriteLine(diagnostic);
                    }
                    Console.ForegroundColor = color;
                }
            }
        }
        static void print(SyntaxNode node, string indent="", bool isLast=true) {
            var marker = isLast? "└──" : "├──";
            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);
            if (node is SyntaxToken t && t.Value!=null) {
                Console.Write($" {t.Value}");
            }
            indent += isLast ? "   ":"|   ";
            Console.WriteLine();
            var lastChild = node.GetChildren().LastOrDefault();
            foreach (var child in node.GetChildren()) {
                print(child, indent, child==lastChild);
            }
        }
    }
}
