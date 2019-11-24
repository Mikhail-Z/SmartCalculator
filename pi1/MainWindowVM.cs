using pi1.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace pi1
{
	public class MainWindowVM : INotifyPropertyChanged
	{
		private ObservableCollection<OperationItem> _operationItems;
		private ICommand _calculateCommand;
		private string _expression;

		public string Expression
		{
			get
			{
				return _expression;
			}
			set
			{
				_expression = value;
				OnPropertyChanged(nameof(Expression));
			}
		}

		public ObservableCollection<OperationItem> OperationItems 
		{ 
			get
			{
				return _operationItems;
			}
			set
			{
				_operationItems = value;
				OnPropertyChanged(nameof(OperationItems));
			}
		}

		public ICommand CalculateCommand 
		{
			get
			{
				return _calculateCommand;
			}
			set
			{
				_calculateCommand = value;
				OnPropertyChanged(nameof(CalculateCommand));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged([CallerMemberName]string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
		}
	}
}
