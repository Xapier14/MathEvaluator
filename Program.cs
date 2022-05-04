using System;
using System.Collections.Generic;
using System.Text;

namespace MathEvaluator
{
    public static partial class Program
    {
        public static void Main(string[] args)
        {
            string? expression = null;
            if (args.Length == 0)
            {
                // get input
                while (expression == null)
                {
                    Console.Write("Input INFIX Math Expression: ");
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