using System;
using System.Linq;
using System.Collections.Generic;

using swifty.Code;
using swifty.Code.Syntaxt;

namespace swifty {
    internal static class Program {
        private static void Main() {
            while (true) {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("> ");
                string line = Console.ReadLine();

                var syntaxTree = SyntaxTree.Parse(line);

                Console.ForegroundColor = ConsoleColor.Magenta;
                print(syntaxTree.Root);

                var compiler = new Compiler(syntaxTree);
                var result = compiler.EvaluationResult();

                object value = result.Value;
                var diagnostics = result.Diagnostics;

                if (!diagnostics.Any()) {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(value);
                } else {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    foreach(var diagnostic in diagnostics) {
                        Console.WriteLine(diagnostic);
                    }
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
