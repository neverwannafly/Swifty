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
                drawTree(syntaxTree.Root);

                var compiler = new Compiler(syntaxTree);
                var result = compiler.EvaluationResult();

                object value = result.Value;
                var diagnostics = result.Diagnostics;

                if (!diagnostics.Any()) {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(value);
                } else {
                    foreach(var diagnostic in diagnostics) {
                        printDiagnostics(line, diagnostic);
                    }
                }
            }
        }
        static void printDiagnostics(string line, Diagnostic diagnostic) {
            Console.WriteLine();
            var prefix = line.Substring(0, diagnostic.Span.Start);
            var error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
            var suffix = line.Substring(diagnostic.Span.End);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"\t{prefix}");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(error);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(suffix);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(diagnostic);
        }
        static void drawTree(SyntaxNode node, string indent="", bool isLast=true) {
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
                drawTree(child, indent, child==lastChild);
            }
        }
    }
}
