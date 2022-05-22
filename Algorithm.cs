using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEvaluator
{
    public static class Algorithm
    {
        /// <summary>
        /// Tokenizes an expression string
        /// Best Case: O(n)
        /// Average Case: O(n)
        /// Worst Case: O(n)
        /// </summary>
        /// <param name="expression">the expression string</param>
        /// <returns>list of tokens</returns>
        public static LinkedList<Token> TokenizeExpression(string expression)
        {
            LinkedList<Token> tokens = new();

            // number buffer for multi-digit numbers
            string numberBuffer = "";

            // for each character
            string trimmed = expression.Trim();
            for (int i = 0; i < trimmed.Length; ++i)
            {
                char c = trimmed[i];

                // ignore spaces
                if (c == ' ')
                    continue;
                // if the character is a digit or is the start of a negative number
                if (IsDigit(c) || (c == '-' && numberBuffer.Length == 0 && IsDigit(trimmed, i+1) && (tokens.Count() == 0 || IsPEMDAS(tokens.LastOrDefault((Token?)null)))))
                {
                    // we append this to the numberBuffer to accomodate multi-digit numbers
                    numberBuffer += c;
                    continue;
                } else
                {
                    // if the numberBuffer has data
                    if (numberBuffer != "")
                    {
                        // parse the number into a double and make a token
                        Token numberToken = new()
                        {
                            Type = TokenType.Number,
                            Data = double.Parse(numberBuffer)
                        };
                        numberBuffer = "";
                        // and add it to the return list
                        tokens.AddLast(numberToken);
                    }

                    // we prepare a new operator token just in case:
                    Token operatorToken = new()
                    {
                        Type = TokenType.Operator
                    };

                    // continuing, we check if the character is a valid operator
                    switch (c)
                    {
                        case '+':
                            operatorToken.Operator = Operator.Addition;
                            break;
                        case '-':
                            operatorToken.Operator = Operator.Subtraction;
                            break;
                        case '*':
                            operatorToken.Operator = Operator.Multiplication;
                            break;
                        case '/':
                            operatorToken.Operator = Operator.Division;
                            break;
                        case '^':
                            operatorToken.Operator = Operator.Power;
                            break;
                        case '(':
                            operatorToken.Operator = Operator.OpenParenthesis;
                            break;
                        case ')':
                            operatorToken.Operator = Operator.CloseParenthesis;
                            break;
                        default:
                            continue;
                    }

                    // if the operator was valid, we add the newly made token to the return list
                    tokens.AddLast(operatorToken);
                }
            }

            // one last check if the numberBuffer has data
            if (numberBuffer != "")
            {
                // we still have one more number token
                Token numberToken = new()
                {
                    Type = TokenType.Number,
                    Data = double.Parse(numberBuffer)
                };
                tokens.AddLast(numberToken);
            }

            return tokens;
        }

        /// <summary>
        /// Dijkstra's Shunting-Yard Algorithm
        /// Converts an infix expression to postfix notation
        /// Best Case: O(n)
        /// Average Case: O(n)
        /// Worst Case: O(n^2)
        /// </summary>
        /// <param name="infixTokens">List of tokens in ifix notation</param>
        /// <returns>A list of tokens in postfix notation</returns>
        public static LinkedList<Token> ShuntingYard(LinkedList<Token> infixTokens)
        {
            LinkedList<Token> postFix = new();
            Stack<Token> operators = new();

            // Doubles current capacity to ensure a capacity of 4096
            operators.EnsureCapacity(4096);

            foreach (Token token in infixTokens)
            {
                // if token is a number, just add it to output postFix list
                if (token.Type == TokenType.Number)
                {
                    postFix.AddLast(token);
                } else
                {
                    // if open parenthesis, push to stack and continue to next input token
                    if (token.Operator == Operator.OpenParenthesis)
                    {
                        operators.Push(token);
                        continue;
                    }
                    
                    // if clost parenthesis, pop from stack until open parenthesis
                    if (token.Operator == Operator.CloseParenthesis)
                    {
                        while (operators.Peek().Operator != Operator.OpenParenthesis)
                        {
                            postFix.AddLast(operators.Pop());
                        }

                        // discard open parenthesis
                        operators.Pop();
                        continue;
                    }

                    // assume token is some emdas operator
                    while (operators.Count > 0)
                    {
                        if (CheckLowerOrSamePrecedence(token, operators.Peek()))
                        {
                            Token top = operators.Pop();
                            postFix.AddLast(top);
                        } else
                        {
                            break;
                        }
                    }
                    operators.Push(token);
                }
            }

            while (operators.TryPop(out Token? token))
            {
                postFix.AddLast(token);
            }

            return postFix;
        }

        /// <summary>
        /// Evaluates a list of tokens in postfix notation
        /// Best Case: O(n)
        /// Average Case: O(n)
        /// Worst Case: O(n)
        /// </summary>
        /// <param name="postfixExpression">List of tokens in postfix notation</param>
        /// <returns>The evaluated result</returns>
        public static double EvaluatePostFix(LinkedList<Token> postfixExpression)
        {
            Stack<Token> buffer = new();
            foreach (Token token in postfixExpression)
            {
                if (token.Type == TokenType.Number)
                {
                    buffer.Push(token);
                    continue;
                }

                Token tk2 = buffer.Pop();
                Token tk1 = buffer.Pop();
                switch (token.Operator)
                {
                    case Operator.Addition:
                        buffer.Push(tk1 + tk2);
                        break;
                    case Operator.Subtraction:
                        buffer.Push(tk1 - tk2);
                        break;
                    case Operator.Multiplication:
                        buffer.Push(tk1 * tk2);
                        break;
                    case Operator.Division:
                        buffer.Push(tk1 / tk2);
                        break;
                    case Operator.Power:
                        buffer.Push(tk1 ^ tk2);
                        break;
                }
            }
            if (buffer.Count != 1)
                throw new Exception("Unknown Error!");
            return buffer.Pop().Data;
        }


        #region AUXILLIARY FUNCTIONS
        private static bool IsPEMDAS(Token? token)
        {
            if (token is null)
                return false;
            return token.Operator == Operator.Addition ||
                   token.Operator == Operator.Subtraction ||
                   token.Operator == Operator.Multiplication ||
                   token.Operator == Operator.Division ||
                   token.Operator == Operator.Power;
        }
        private static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }
        private static bool IsDigit(string str, int i)
        {
            try
            {
                return IsDigit(str[i]);
            } catch (Exception)
            {
                return false;
            }
        }
        private static bool CheckLowerOrSamePrecedence(Token token1, Token token2)
        {
            if (token2.Operator == Operator.OpenParenthesis ||
                token2.Operator == Operator.CloseParenthesis)
                return false;
            // power operator has the highest precedence
            if (token1.Operator == Operator.Power)
                return false;
            // if token1 is either x or / and token2 is + or -
            if ((token1.Operator == Operator.Multiplication || token1.Operator == Operator.Division) &&
                (token2.Operator == Operator.Addition || token2.Operator == Operator.Subtraction))
                return false;
            return true;
        }
        #endregion
    }
}
