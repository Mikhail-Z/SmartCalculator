using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace pi1
{
	class RelayCommand : ICommand
	{
		private Action methodToExecute;
		private Func<bool> canExecute;
		public event EventHandler CanExecuteChanged;

		public RelayCommand(Action method, Func<bool> canExecute)
		{
			methodToExecute = method;
			this.canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return canExecute();
		}

		public void Execute(object parameter)
		{
			methodToExecute.Invoke();
		}
	}
}
