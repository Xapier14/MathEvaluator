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
            
            // check if expression is not yet supplied
            if (args.Length == 0)
            {
                Console.WriteLine("Math Expression Evaluator");
                Console.WriteLine("Use only with expressions in INFIX notation.");
                Console.WriteLine("Ex: 12 + 3 * ( 4 / 3 + ( 3 ^ 2 ) + 3 ) - 5");
                Console.WriteLine("Result: 47");
                Console.WriteLine();
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
            LinkedList<Token>? input = null;
            
            try
            {
                input = Algorithm.TokenizeExpression(expression);
            } catch (Exception)
            {
                Console.WriteLine("Tokenization Error. Please double-check your syntax.");
                Environment.Exit(-1); // EXIT CODE 1 for tokenization error.
            }

            // verify parameter count
            if (!Tools.VerifyPararenthesisCount(input))
            {
                Console.WriteLine("Input Error. Please double-check your parenthesis pairs.");
                Environment.Exit(-2); // EXIT CODE 2 for parenthesis count error.
            }

            // check token length
            if (input.Count == 0)
            {
                Console.WriteLine("Input Error. Please double-check your syntax.");
                Environment.Exit(-3); // EXIT CODE 3 for empty input error.
            }

            // convert infix expression to postfix
            LinkedList<Token>? postFix = null;
            try
            {
                postFix = Algorithm.ShuntingYard(input);
            } catch (Exception)
            {
                Console.WriteLine("Postfix Algorithm Error. Please double-check your syntax.");
                Environment.Exit(-4); // EXIT CODE 4 for tokenization error.
            }

            // evaluate postfix expression
            double result = 0;
            try
            {
                result = Algorithm.EvaluatePostFix(postFix);
            } catch (Exception)
            {
                Console.WriteLine("Evaluation Error. Please double-check your syntax.");
                Environment.Exit(-5); // EXIT CODE 5 for tokenization error.
            }

            // Print Result
            Console.WriteLine($"{(args.Length != 0 ? "" : "Result: ")}{{0}}", result);
        }
    }
}