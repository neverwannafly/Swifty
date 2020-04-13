using System;
using System.Linq;
using System.Collections.Generic;

using swifty.Code;
using swifty.Code.Syntaxt;

namespace swifty {
    internal static class Program {
        private static void Main() {

            var symbolTable = new Dictionary<VariableSymbol,object>();

            while (true) {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("> ");
                string line = Console.ReadLine();

                var syntaxTree = SyntaxTree.Parse(line);
                var text = syntaxTree.SourceText;

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(syntaxTree.Root.ToString());

                var compiler = new Compiler(syntaxTree);
                var result = compiler.EvaluationResult(symbolTable);

                object value = result.Value;
                var diagnostics = result.Diagnostics;

                if (!diagnostics.Any()) {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(value);
                } else {
                    foreach(var diagnostic in diagnostics) {
                        int lineIndex = text.GetLineIndex(diagnostic.Span.Start);
                        int lineNumber = lineIndex+1;
                        int character = diagnostic.Span.Start - text.Lines[lineIndex].Start + 1;
                        printDiagnostics(line, diagnostic, lineNumber, character);
                    }
                }
            }
        }
        static void printDiagnostics(string line, Diagnostic diagnostic, int lineNumber, int character) {
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
            Console.Write($"[{lineNumber} {character}] : ");
            Console.WriteLine(diagnostic);
        }
    }
}
