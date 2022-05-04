using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEvaluator
{
    public class Token
    {
        public double Data { get; set; }
        public TokenType Type { get; set; }
        public Operator Operator { get; set; }

        public override string ToString()
        {
            if (Type == TokenType.Number)
                return Data.ToString();
            switch (Operator)
            {
                case Operator.Addition:
                    return "+";
                case Operator.Subtraction:
                    return "-";
                case Operator.Multiplication:
                    return "*";
                case Operator.Division:
                    return "/";
                case Operator.Power:
                    return "^";
                case Operator.OpenParenthesis:
                    return "(";
                case Operator.CloseParenthesis:
                    return ")";
            }
            return "";
        }

        public static Token operator+(Token tk1, Token tk2)
        {
            return new()
            {
                Type = TokenType.Number,
                Data = tk1.Data + tk2.Data
            };
        }

        public static Token operator-(Token tk1, Token tk2)
        {
            return new()
            {
                Type = TokenType.Number,
                Data = tk1.Data - tk2.Data
            };
        }

        public static Token operator*(Token tk1, Token tk2)
        {
            return new()
            {
                Type = TokenType.Number,
                Data = tk1.Data * tk2.Data
            };
        }

        public static Token operator/(Token tk1, Token tk2)
        {
            return new()
            {
                Type = TokenType.Number,
                Data = tk1.Data / tk2.Data
            };
        }

        public static Token operator^(Token tk1, Token tk2)
        {
            return new()
            {
                Type = TokenType.Number,
                Data = Math.Pow(tk1.Data, tk2.Data)
            };
        }
    }
}
