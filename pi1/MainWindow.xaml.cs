using NLog;
using pi1.Constants;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace pi1
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
		private ICalculator _calculator;

		public MainWindow(ICalculator calculator)
		{
			InitializeComponent();
			_calculator = calculator;

			foreach (UIElement c in LayoutRoot.Children)
			{
				if (c is Button)
				{
					((Button)c).Click += ButtonClick;
				}
			}
		}

		private void ButtonClick(object sender, RoutedEventArgs e)
		{
			string s = (string)((Button)e.OriginalSource).Content;
			_logger.Log(LogLevel.Debug, $"Нажата клавиша: {s}"); ;
			if (s == "=")
			{
				var result = _calculator.Calculate(textBlock.Text);
				textBlock.Text = result.ToString();
			}
			else if (s == "C")
			{
				textBlock.Text = "";
			}
			else
			{
				textBlock.Text += s;
			}
		}
	}
}
