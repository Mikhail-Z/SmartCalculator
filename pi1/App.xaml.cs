using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using pi1.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace pi1
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public IServiceProvider ServiceProvider { get; private set; }

		public App()
		{
			var serviceCollection = new ServiceCollection();
			ConfigureServices(serviceCollection);

			ServiceProvider = serviceCollection.BuildServiceProvider();
		}

		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
			var mwp = new MainWindowPresenter(
				mainWindow,
				new MainWindowVM(),
				ServiceProvider.GetRequiredService<ICalculator>(),
				ServiceProvider.GetRequiredService<IOperationsHistoryRepository>());

		}

		private void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<ICalculator>(provider => new RPNCalculator());
			services.AddSingleton<IOperationsHistoryRepository, OperationsHistoryMemoryRepository>();
			services.AddSingleton<MainWindow>();
		}
	}
}
