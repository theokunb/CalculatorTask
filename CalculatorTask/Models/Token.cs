using System;
using System.Text.RegularExpressions;

namespace CalculatorTask.Models
{
    public enum Priopity
    {
        Low,
        Hight
    }

    public abstract class Token
    {
        protected string val = string.Empty;

        public string Val => val;

        public abstract Token Parse(ref string input);

        public static bool operator ==(Token left, Token right)
        {
            return left.val == right.val;
        }

        public static bool operator !=(Token left, Token right)
        {
            return left.val != right.val;
        }

        public static bool operator ==(Token left, string right)
        {
            return left.val == right;
        }

        public static bool operator !=(Token left, string right)
        {
            return left.val != right;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Point : Token
    {
        public Point()
        {
            val = ",";
        }

        public override Token Parse(ref string input)
        {
            throw new NotImplementedException();
        }
    }

    public class Number : Token
    {
        private readonly Regex _regExp;

        public Number()
        {
            _regExp = new Regex(@"^[-]?(0|[1-9]\d*)([.,]\d+)?");
        }

        public Number(string val)
        {
            this.val = val;
            _regExp = new Regex(@"^[-]?(0|[1-9]\d*)([.,]\d+)?");
        }

        public override Token Parse(ref string input)
        {
            val = _regExp.Match(input).ToString();
            input = input.Remove(0, val.Length);

            return this;
        }

        public static Number operator +(Number first, Number second)
        {
            float.TryParse(first.Val, out float firstNumber);
            float.TryParse(second.Val, out float secondNumber);
            string result = (firstNumber + secondNumber).ToString();

            return new Number().Parse(ref result) as Number;
        }
        public static Number operator -(Number first, Number second)
        {
            float.TryParse(first.Val, out float firstNumber);
            float.TryParse(second.Val, out float secondNumber);
            string result = (firstNumber - secondNumber).ToString();

            return new Number().Parse(ref result) as Number;
        }
        public static Number operator *(Number first, Number second)
        {
            float.TryParse(first.Val, out float firstNumber);
            float.TryParse(second.Val, out float secondNumber);
            string result = (firstNumber * secondNumber).ToString();

            return new Number().Parse(ref result) as Number;
        }
        public static Number operator /(Number first, Number second)
        {
            float.TryParse(first.Val, out float firstNumber);
            float.TryParse(second.Val, out float secondNumber);

            if (secondNumber == 0)
                return new Number();

            string result = (firstNumber / secondNumber).ToString();
            return new Number().Parse(ref result) as Number;
        }
    }

    public abstract class Bkt : Token
    {
        public override Token Parse(ref string input)
        {
            input = input.Remove(0, val.Length);
            return this;
        }
    }

    public sealed class LeftBkt : Bkt
    {
        public LeftBkt()
        {
            val = "(";
        }
    }

    public sealed class RightBkt : Bkt
    {
        public RightBkt()
        {
            val = ")";
        }
    }
    public abstract class Operator : Token
    {
        protected Priopity priority;
        public Priopity Priopity => priority;

        public abstract Token Solve(Token left, Token right);

        public override Token Parse(ref string input)
        {
            input = input.Remove(0, val.Length);
            return this;
        }
    }

    public sealed class AddOperator : Operator
    {
        public AddOperator()
        {
            val = "+";
            priority = Priopity.Low;
        }

        public override Token Solve(Token left, Token right)
        {
            return (left as Number) + (right as Number);
        }
    }
    public sealed class SubOperator : Operator
    {
        public SubOperator()
        {
            val = "-";
            priority = Priopity.Low;
        }

        public override Token Solve(Token left, Token right)
        {
            return (left as Number) - (right as Number);
        }
    }
    public sealed class MulOperator : Operator
    {
        public MulOperator()
        {
            val = "*";
            priority = Priopity.Hight;
        }

        public override Token Solve(Token left, Token right)
        {
            return (left as Number) * (right as Number);
        }
    }
    public sealed class DivOperator : Operator
    {
        public DivOperator()
        {
            val = "/";
            priority = Priopity.Hight;
        }

        public override Token Solve(Token left, Token right)
        {
            return (left as Number) / (right as Number);
        }
    }
}
