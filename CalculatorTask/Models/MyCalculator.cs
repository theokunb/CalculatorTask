namespace CalculatorTask.Models
{
    public class MyCalculator
    {
        private readonly IConvertor _convertor;
        private readonly ISolver _solver;
        private readonly Validator _validator;

        public MyCalculator(IConvertor convertor, ISolver solver, Validator validator)
        {
            _convertor = convertor;
            _solver = solver;
            _validator = validator;
        }

        public Validator Validator => _validator;

        public string ProcessExpression(string input)
        {
            var postfixForm = _convertor.Convert(input);
            var result = _solver.Solve(postfixForm);
            return result;
        }
    }
}
