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
        /// Time Complexity: O(n)
        /// </summary>
        /// <param name="expression">the expression string</param>
        /// <returns>list of tokens</returns>
        public static List<Token> TokenizeExpression(string expression)
        {
            List<Token> tokens = new();

            // number buffer for multi-digit numbers
            string numberBuffer = "";

            // for each character
            foreach (char c in expression.Trim())
            {
                // if the character is a digit
                if (IsDigit(c))
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
                        tokens.Add(numberToken);
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
                    tokens.Add(operatorToken);
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
                tokens.Add(numberToken);
            }

            return tokens;
        }

        /// <summary>
        /// Dijkstra's Shunting-Yard Algorithm
        /// Converts an infix expression to postfix notation
        /// Average Time Complexity: O(n)
        /// </summary>
        /// <param name="infixTokens">List of tokens in ifix notation</param>
        /// <returns>A list of tokens in postfix notation</returns>
        public static List<Token> ShuntingYard(List<Token> infixTokens)
        {
            List<Token> postFix = new();
            Stack<Token> operators = new();

            foreach (Token token in infixTokens)
            {
                // if token is a number, just add it to output postFix list
                if (token.Type == TokenType.Number)
                {
                    postFix.Add(token);
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
                            postFix.Add(operators.Pop());
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
                            postFix.Add(top);
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
                postFix.Add(token);
            }

            return postFix;
        }

        /// <summary>
        /// Evaluates a list of tokens in postfix notation
        /// </summary>
        /// <param name="postfixExpression">List of tokens in postfix notation</param>
        /// <returns>The evaluated result</returns>
        public static double EvaluatePostFix(List<Token> postfixExpression)
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
        private static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
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
