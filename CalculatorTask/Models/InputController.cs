using System.Collections.Generic;

namespace CalculatorTask.Models
{
    public class InputController
    {
        private delegate void ExecuteCommand();
        private readonly Dictionary<string, ExecuteCommand> _commands;
        private string _line = string.Empty;

        public InputController(string[] param)
        {
            _commands = new Dictionary<string, ExecuteCommand>
            {
                { param[0], ClearLine },
                { param[1], RemoveLast }
            };
        }

        public string Line => _line;

        private void ClearLine()
        {
            _line = string.Empty;
        }

        private void RemoveLast()
        {
            if (_line.Length > 0)
                _line = _line.Remove(_line.Length - 1);
        }

        public void ApplyCommand(string parameter)
        {
            _commands[parameter].Invoke();
        }
        public void Append(string param)
        {
            _line += param;
        }
    }
}
