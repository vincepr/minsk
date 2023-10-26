// https://www.youtube.com/watch?v=3XM9vUGduhk&list=PLRAdsfhKI4OWNOSfS7EUu5GRAVmze1t2y&index=2
// 13:50
using Minsk.CodeAnalysis;
using Minsk.CodeAnalysis.Binding;
using Minsk.CodeAnalysis.Syntax;

namespace Minsk
{

    // 1 + 2 * 3
    // gets parsed into a treel like:
    //
    //    +
    //   / \
    //  1   *
    //     / \
    //    2   3

    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("type: '#showTree' \t: enable parse-tree information");
            Console.WriteLine("type: '#cls' \t: clear the Terminal");
            bool showTree = false;
            while (true)
            {
                // get input
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    return;

                // pseudo commands for custom flags
                if (line == "#showTree")
                {
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "ENABLED showing parse-tree." : "NOT showing parse-tree");
                    continue;
                }
                else if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                // create the syntax tree - 
                var syntaxTree = SyntaxTree.Parse(line);
                // next parse the syntax tree again and add some additional info like Types to it -> bound-tree
                var binder = new Binder();
                var boundExpression = binder.BindExpression(syntaxTree.Root);
                var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();


                // optional showing of parse-tree
                if (showTree)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                // print the result of the evaluation
                if (!diagnostics.Any())
                {
                    var eval = new Evaluator(boundExpression);
                    var result = eval.Evaluate();
                    Console.WriteLine(result.ToString());
                }
                // print diagnostics instead if we encountered an ERROR   
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var d in diagnostics)
                        Console.WriteLine(d);
                    Console.ResetColor();
                }
            }
        }

        // format node-tree like:
        //   ├─PlusToken
        //   └─NumberExpression
        //     └─NumberToken 2
        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            Console.Write(indent);
            Console.Write(isLast ? "└─" : "├─");
            Console.Write(node.Kind);
            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }
            Console.WriteLine();
            indent += isLast ? "  " : "│ ";
            var lastChild = node.GetChildren().LastOrDefault();
            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent, child == lastChild);
        }
    }
}



