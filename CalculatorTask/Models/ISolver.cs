using System.Collections.Generic;

namespace CalculatorTask.Models
{
    public interface ISolver
    {
        string Solve(List<Token> input);
    }
}
