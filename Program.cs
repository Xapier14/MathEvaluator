using System;
using System.Collections.Generic;
using System.Text;

namespace MathEvaluator
{
    public static partial class Program
    {
        public static void Main(string[] args)
        {
            // get input
            string? expression = null;
            while (expression == null)
            {
                Console.Write("Input INFIX Math Expression: ");
                expression = Console.ReadLine();
            }

            // tokenize string expression
            List<Token> input = Algorithm.TokenizeExpression(expression);

            // convert infix expression to postfix
            List<Token> postFix = Algorithm.ShuntingYard(input);

            // evaluate postfix expression
            double result = Algorithm.EvaluatePostFix(postFix);

            // Print Result
            Console.WriteLine($"RESULT: {result}");
        }
    }
}