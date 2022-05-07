using System;
using System.Collections.Generic;
using System.Text;

namespace MathEvaluator
{
    public static partial class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Math Expression Evaluator");
            Console.WriteLine("Use only with expressions in INFIX notation.");
            Console.WriteLine("Ex: 12 + 3 * ( 4 / 3 + ( 3 ^ 2 ) + 3 ) - 5");
            Console.WriteLine("Result: 47");
            Console.WriteLine();

            string? expression = null;
            if (args.Length == 0)
            {
                // get input
                while (expression == null)
                {
                    Console.Write("Input: ");
                    expression = Console.ReadLine();
                }
            } else
            {
                expression = args[0];
            }

            // tokenize string expression
            List<Token> input = Algorithm.TokenizeExpression(expression);

            // convert infix expression to postfix
            List<Token> postFix = Algorithm.ShuntingYard(input);

            // evaluate postfix expression
            double result = Algorithm.EvaluatePostFix(postFix);

            // Print Result
            Console.WriteLine($"{(args.Length != 0 ? "" : "Result: ")}{{0}}", result);
        }
    }
}