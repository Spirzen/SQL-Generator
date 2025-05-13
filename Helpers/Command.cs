using System;
using System.Windows.Input;

namespace SQL_Generator.Helpers
{
    /// <summary>
    /// Реализация интерфейса ICommand для обработки команд в MVVM.
    /// </summary>
    public class Command : ICommand
    {
        /// <summary>
        /// Действие, которое будет выполнено при вызове команды.
        /// </summary>
        private readonly Action _execute;

        /// <summary>
        /// Функция, определяющая, может ли команда быть выполнена.
        /// </summary>
        private readonly Func<bool> _canExecute;

        /// <summary>
        /// Создает новый экземпляр команды.
        /// </summary>
        /// <param name="execute">Действие, которое будет выполнено при вызове команды.</param>
        /// <param name="canExecute">Функция, определяющая возможность выполнения команды (опционально).</param>
        public Command(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Определяет, может ли команда быть выполнена в текущем состоянии.
        /// </summary>
        /// <param name="parameter">Параметр команды (не используется).</param>
        /// <returns>True, если команда может быть выполнена; иначе False.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        /// <summary>
        /// Выполняет логику команды.
        /// </summary>
        /// <param name="parameter">Параметр команды (не используется).</param>
        public void Execute(object parameter)
        {
            _execute();
        }

        /// <summary>
        /// Событие, возникающее при изменении состояния возможности выполнения команды.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}