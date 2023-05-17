using CalculatorTask.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace CalculatorTask.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly MyCalculator _myCalculator;
        private readonly InputController _inputController;
        private static readonly string[] _commandParameters = { "clear", "back" };
        private string _result = string.Empty;

        public MainViewModel()
        {
            List<Token> myTokens = new List<Token>()
            {
                new AddOperator(),
                new SubOperator(),
                new MulOperator(),
                new DivOperator(),
                new LeftBkt(),
                new RightBkt(),
                new Point()
            };

            for (int i = 0; i < 10; i++)
            {
                myTokens.Add(new Number(i.ToString()));
            }

            _myCalculator = new MyCalculator(new PostfixConvertor(myTokens), new PostfixSolver(myTokens), new Validator(myTokens));

            _inputController = new InputController(_commandParameters);

            CommandPerformButton = new Command((param) => PerformButton(param.ToString()));
            CommandSolveButton = new Command((param) => SolveButton());
            CommandBkt = new Command((param) => PerformButtonBkt(param.ToString()));
        }

        public ICommand CommandPerformButton { get; }
        public ICommand CommandSolveButton { get; }
        public ICommand CommandBkt { get; }
        public string Result
        {
            get => _result;
            set
            {
                if (_result == value)
                    return;
                _result = value;
                OnPropertyChanged();
            }
        }
        public string ClearParameter => _commandParameters[0];
        public string BackParameter => _commandParameters[1];
        public string Input => _inputController.Line;

        private void InputChangedResetResult()
        {
            Result = string.Empty;
            OnPropertyChanged(nameof(Input));
        }

        private void PerformButton(string param)
        {
            if (_commandParameters.Contains(param))
            {
                _inputController.ApplyCommand(param);
                InputChangedResetResult();

            }
            else if (_myCalculator.Validator.CanAppend(param, Input))
            {
                _inputController.Append(param);
                InputChangedResetResult();
            }
        }

        private void SolveButton()
        {
            if (_myCalculator.Validator.CanSolve)
            {
                Result = _myCalculator.ProcessExpression(Input);
            }
        }

        private void PerformButtonBkt(string param)
        {
            foreach (char bkt in param)
            {
                if (_myCalculator.Validator.CanAppend(bkt.ToString(), Input))
                {
                    _inputController.Append(bkt.ToString());
                    InputChangedResetResult();
                    return;
                }
            }
        }
    }
}
