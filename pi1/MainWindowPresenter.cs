using pi1.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace pi1
{
	public class MainWindowPresenter
	{
		private ICalculator _calculator;
		private IOperationsHistoryRepository _operationsHistoryRepository;

		private MainWindow View { get; }
		private MainWindowVM ViewModel { get; set; }

		public MainWindowPresenter(MainWindow view, 
			MainWindowVM viewModel, 
			ICalculator calculator, 
			IOperationsHistoryRepository operationsHistoryRepository)
		{
			View = view;
			ViewModel = viewModel;
			_calculator = calculator;
			_operationsHistoryRepository = operationsHistoryRepository;
			OnInitialize();
		}

		public void OnInitialize()
		{
			ViewModel.OperationItems =
				new ObservableCollection<OperationItem>(_operationsHistoryRepository.GetAll());
			ViewModel.CalculateCommand = new RelayCommand(Calculate, ExressionContainsOnlyDecimalSymbols);

			View.DataContext = ViewModel;
			View.Show();
		}

		private void Calculate()
		{
			var oldExpression = ViewModel.Expression;
			ViewModel.Expression = _calculator.Calculate(
				ViewModel.Expression.Substring(0, ViewModel.Expression.Length - 1)).ToString();
			ViewModel.OperationItems.Insert(0,
				new OperationItem { Expression = $"{oldExpression}{ViewModel.Expression}" });
		}

		private bool ExressionContainsOnlyDecimalSymbols()
		{
			return ViewModel.Expression == null || ViewModel.Expression.Contains("бесконечность") == false;
		}
	}
}
