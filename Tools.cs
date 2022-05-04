﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEvaluator
{
    public static class Tools
    {
        public static string Stringify(IEnumerable<Token> tokens)
        {
            StringBuilder sb = new();
            foreach (Token token in tokens)
            {
                if (token.Type == TokenType.Number)
                {
                    sb.Append(token.Data);
                }
                else
                {
                    switch (token.Operator)
                    {
                        case Operator.Addition:
                            sb.Append('+');
                            break;
                        case Operator.Subtraction:
                            sb.Append('-');
                            break;
                        case Operator.Multiplication:
                            sb.Append('*');
                            break;
                        case Operator.Division:
                            sb.Append('/');
                            break;
                        case Operator.Power:
                            sb.Append('^');
                            break;
                        case Operator.OpenParenthesis:
                            sb.Append('(');
                            break;
                        case Operator.CloseParenthesis:
                            sb.Append(')');
                            break;
                    }
                }
                sb.Append(' ');
            }
            return sb.ToString();
        }
    }
}
