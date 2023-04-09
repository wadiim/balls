using System;
using System.Windows.Input;

namespace ViewModel
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            m_Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            m_CanExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return m_CanExecute == null || (parameter == null ? m_CanExecute() : m_CanExecute());
        }

        public virtual void Execute(object parameter)
        {
            m_Execute();
        }

        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private readonly Action m_Execute;
        private readonly Func<bool> m_CanExecute;
    }
}