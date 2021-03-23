﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Twisty.Runner.Ioc;
using Twisty.Runner.Services;
using Twisty.Runner.ViewModels;

namespace Twisty.Runner
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private ServiceCollection m_Services;
		private ServiceProvider m_Provider;

		public App()
		{
			m_Services = new ServiceCollection();
			Bootstrap();
			m_Provider = m_Services.BuildServiceProvider();

			this.Resources["ViewModelLocator"] = new ViewModelLocator(m_Provider);
		}

		private void OnStartup(object sender, StartupEventArgs e)
		{
			var mainWindow = m_Provider.GetService<MainWindow>();
			mainWindow.Show();
		}

		private void Bootstrap()
		{
			// Services
			m_Services.AddSingleton<IRotationCoreService>(new RotationCoreService());

			// View Models
			m_Services.AddTransient<MainWindowViewModel>();
			m_Services.AddTransient<AlgorithmConsoleViewModel>();
			m_Services.AddTransient<RotationCoreStandardViewModel>();

			// Windows
			m_Services.AddSingleton<MainWindow>();
		}
	}
}
