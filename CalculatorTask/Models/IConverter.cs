using System.Collections.Generic;

namespace CalculatorTask.Models
{
    public interface IConvertor
    {
        List<Token> Convert(string input);
    }
}
