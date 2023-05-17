using System.Collections.Generic;
using System.Linq;

namespace CalculatorTask.Models
{
    public class PostfixConvertor : IConvertor
    {
        private readonly List<Operator> operators;
        private readonly List<Number> numbers;
        private readonly List<Bkt> bkts;
        private Stack<Token> stack;
        private Token top => stack.First();
        private Bkt leftBkt => bkts.Find(bkt => bkt.GetType().Equals(typeof(LeftBkt)));
        private Bkt rightBkt => bkts.Find(bkt => bkt.GetType().Equals(typeof(RightBkt)));

        public PostfixConvertor(List<Token> tokenList)
        {
            stack = new Stack<Token>();
            operators = new List<Operator>();
            bkts = new List<Bkt>();
            numbers = new List<Number>();

            foreach (var element in tokenList)
            {
                if (element.GetType().IsSubclassOf(typeof(Operator)))
                    operators.Add(element as Operator);
                else if (element.GetType().IsSubclassOf(typeof(Bkt)))
                    bkts.Add(element as Bkt);
                else if (element.GetType().Equals(typeof(Number)))
                    numbers.Add(element as Number);
            }
        }

        public List<Token> Convert(string input)
        {
            List<Token> queue = new List<Token>();

            while (input.Length > 0)
            {
                var firstElement = input.First().ToString();

                if (numbers.Exists(number => number.Val == firstElement))
                {
                    Number num = new Number();
                    queue.Add(num.Parse(ref input));
                }
                else if (operators.Exists(oper => oper.Val == firstElement))
                {
                    var currentOperator = operators.Find(oper => oper.Val == firstElement);

                    if (stack.Count == 0 || top == leftBkt)
                    {
                        stack.Push(currentOperator.Parse(ref input));
                    }
                    else if (currentOperator.Priopity > (top as Operator).Priopity)
                    {
                        stack.Push(currentOperator.Parse(ref input));
                    }
                    else if (currentOperator.Priopity <= (top as Operator).Priopity)
                    {
                        while (stack.Count > 0)
                        {
                            var popElement = stack.Pop();
                            if (popElement == leftBkt || (popElement as Operator).Priopity < currentOperator.Priopity)
                                break;
                            queue.Add(popElement);
                        }

                        stack.Push(currentOperator.Parse(ref input));
                    }
                }
                else if (firstElement == leftBkt.Val)
                {
                    var currentOperator = new LeftBkt();
                    stack.Push(currentOperator.Parse(ref input));
                }
                else if (firstElement == rightBkt.Val)
                {
                    var currentOperator = new RightBkt();
                    currentOperator.Parse(ref input);

                    while (stack.Count > 0)
                    {
                        var popElement = stack.Pop();

                        if (popElement == leftBkt)
                            break;

                        queue.Add(popElement);
                    }
                }
            }

            while (stack.Count > 0)
            {
                var popElement = stack.Pop();
                queue.Add(popElement);
            }

            return queue;
        }
    }
}
