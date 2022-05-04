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
            List<Token> postfix = new();
            Stack<Token> operators = new();

            // for each token in the infix expression
            foreach (Token token in infixTokens)
            {
                // if this is a number
                if (token.Type == TokenType.Number)
                {
                    // add to post fix
                    postfix.Add(token);
                } else
                {
                    // if open parenthesis
                    if (token.Operator == Operator.OpenParenthesis)
                    {
                        // push to stack
                        operators.Push(token);
                        continue;
                    }
                    // if close parenthesis
                    if (token.Operator == Operator.CloseParenthesis)
                    {
                        // keep popping operator stack to postfix expression until open parenthesis pair is found
                        while (operators.Peek().Operator != Operator.OpenParenthesis)
                        {
                            postfix.Add(operators.Pop());
                        }
                        // discard open parenthesis
                        operators.Pop();
                        continue;
                    }
                    // if any thing else (other operators)
                    while (operators.Count > 0)
                    {
                        // while operator precedence is lower or the same and is not an open parenthesis, pop operator stack to postfix expression
                        if (CheckLowerOrSamePrecedence(token, operators.Peek()) && operators.Peek().Operator != Operator.OpenParenthesis)
                            postfix.Add(operators.Pop());
                        else
                            break;
                    }
                    operators.Push(token);
                }
            }
            // pop the rest of the operator stack to the postfix expression
            while (operators.Count > 0)
                postfix.Add(operators.Pop());

            return postfix;
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
