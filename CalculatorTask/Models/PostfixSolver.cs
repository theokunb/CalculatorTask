using System.Collections.Generic;

namespace CalculatorTask.Models
{
    public class PostfixSolver : ISolver
    {
        private readonly List<Token> _operators;
        private Stack<Token> _stack;

        public PostfixSolver(List<Token> tokens)
        {
            _stack = new Stack<Token>();
            _operators = new List<Token>();

            foreach (var element in tokens)
            {
                if (element.GetType().IsSubclassOf(typeof(Operator)))
                    _operators.Add(element);
            }
        }

        public string Solve(List<Token> input)
        {
            foreach (var element in input)
            {
                if (element.GetType().Equals(typeof(Number)))
                {
                    _stack.Push(element);
                }
                else if (_operators.Exists(oper => oper == element))
                {
                    var firstPop = _stack.Pop();
                    var secondPop = _stack.Pop();
                    var currentOperator = _operators.Find(oper => oper == element) as Operator;
                    _stack.Push(currentOperator.Solve(secondPop, firstPop));
                }
            }

            return _stack.Pop().Val;
        }
    }
}
