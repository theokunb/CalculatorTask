using System.Collections.Generic;

namespace CalculatorTask.Models
{
    public class Validator
    {
        private readonly Rule _rule;

        public Validator(List<Token> tokenList)
        {
            _rule = new Rule(tokenList);
        }

        public bool CanSolve => _rule.IsLineValidToSolve();

        public bool CanAppend(string param, string line)
        {
            if (line.Length == 0)
            {
                return _rule.AllowFirst(param);
            }
            else
            {
                char lastChar = line[line.Length - 1];
                return _rule.AllowFollow(lastChar, param);
            }
        }
    }
}
