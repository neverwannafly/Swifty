using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using swifty.Code;
using swifty.Code.Syntaxt;
using swifty.Code.Text;

namespace swifty {
    internal static class Program {
        private static void InteractiveApp() {
            var symbolTable = new Dictionary<VariableSymbol,object>();
            var textBuilder = new StringBuilder();
            bool showTree = true;
            Compiler prev = null;

            while (true) {
                Console.ForegroundColor = ConsoleColor.DarkCyan;

                if (textBuilder.Length  == 0) {
                    Console.Write("> ");
                } else {
                    Console.Write(". ");
                }

                string input = Console.ReadLine();
                bool isBlank = String.IsNullOrWhiteSpace(input);
                
                if (textBuilder.Length==0) {
                    if (isBlank) break;
                    if (input == "#CLEAR") {
                        Console.Clear();
                        continue;
                    }
                    if (input == "#TREE") {
                        showTree = true;
                        continue;
                    }
                    if (input == "#HIDETREE") {
                        showTree = false;
                        continue;
                    }
                    if (input == "#RESET") {
                        prev = null;
                        continue;
                    }
                }

                textBuilder.AppendLine(input);
                var feed = textBuilder.ToString();

                var syntaxTree = SyntaxTree.Parse(feed);
                if (!isBlank && syntaxTree.Diagnostics.Any()) {
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.Magenta;
                if (showTree) {
                    Console.Write(syntaxTree.Root.ToString());
                }

                var compiler = prev == null ? new Compiler(syntaxTree) : prev.ContinueWith(syntaxTree);
                var result = compiler.EvaluationResult(symbolTable);

                object value = result.Value;
                var diagnostics = result.Diagnostics;

                if (!diagnostics.Any()) {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine(value);
                    prev = compiler;
                } else {
                    foreach(var diagnostic in diagnostics) {
                        int lineIndex = syntaxTree.SourceText.GetLineIndex(diagnostic.Span.Start);
                        var line = syntaxTree.SourceText;
                        int character = diagnostic.Span.Start - line.Lines[lineIndex].Start + 1;
                        printDiagnostics(line, diagnostic, lineIndex, character);
                    }
                }
                textBuilder.Clear();
            }
        }
        static void printDiagnostics(SourceText line, Diagnostic diagnostic, int lineIndex, int character) {
            Console.WriteLine();

            var lineInfo = line.Lines[lineIndex];
            var prefixSpan = TextSpan.FromBound(lineInfo.Start, diagnostic.Span.Start);
            var suffixSpan = TextSpan.FromBound(diagnostic.Span.End, lineInfo.End);

            var prefix = line.ToString(prefixSpan);
            var error = line.ToString(diagnostic.Span);
            var suffix = line.ToString(suffixSpan);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"\t{prefix}");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(error);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(suffix);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"[{lineIndex+1} {character}] : ");
            Console.WriteLine(diagnostic);
        }
        private static void Main(string[] args) {
            if ((args.Length==1 && args[0]=="--interactive") || args.Length==0) {
                InteractiveApp();
            } else if (args.Length==1 && args[0]=="--build") {
                Build();
            }
        }
        private static void Build() {
            var logger = new Logger();
            var symbolTable = new Dictionary<VariableSymbol,object>();
            var textBuilder = new StringBuilder();
            Compiler prev = null;

            string[] lines = System.IO.File.ReadAllLines("buffer.t");

            foreach (string input in lines) {
                bool isBlank = String.IsNullOrWhiteSpace(input);
                textBuilder.AppendLine(input);
                var feed = textBuilder.ToString();
                var syntaxTree = SyntaxTree.Parse(feed);
                if (!isBlank && syntaxTree.Diagnostics.Any()) {
                    continue;
                }
                var compiler = prev == null ? new Compiler(syntaxTree) : prev.ContinueWith(syntaxTree);
                var result = compiler.EvaluationResult(symbolTable);
                var diagnostics = result.Diagnostics;
                logger.AddLog("syntaxTree", syntaxTree.Root.ToString());
                if (!diagnostics.Any()) {
                    if (lines.Last() == input) {
                        logger.AddLog("compilationResult", "Compiled Successfully");
                        logger.AddLog("result", result.Value.ToString());
                    }
                    prev = compiler;
                } else {
                    if (lines.Last() == input) {
                        logger.AddLog("compilationResult", "Compilation Failed");
                    }
                    foreach(var diagnostic in diagnostics) {
                        int lineIndex = syntaxTree.SourceText.GetLineIndex(diagnostic.Span.Start);
                        var line = syntaxTree.SourceText;
                        int character = diagnostic.Span.Start - line.Lines[lineIndex].Start + 1;
                        logger.AddLog("error", $"[{lineIndex+1} {character}] : {diagnostic.Message}");
                    }
                }
                textBuilder.Clear();
            }
            logger.PrintLog();
        }
    }
}
