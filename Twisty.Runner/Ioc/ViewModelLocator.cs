using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Runner.ViewModels;

namespace Twisty.Runner.Ioc
{
	/// <summary>
	/// Class allowing to generate ViewModels using the View Model Locator pattern.
	/// </summary>
	internal class ViewModelLocator
	{
		private readonly ServiceProvider m_Provider;

		/// <summary>
		/// Create a new ViewModelLocator with the provided IOC bindings.
		/// </summary>
		/// <param name="provider">Ioc provider.</param>
		public ViewModelLocator(ServiceProvider provider)
			=> m_Provider = provider;

		/// <summary>
		/// Gets a new MainWindowViewModel.
		/// </summary>
		public MainWindowViewModel MainWindowViewModel
			=> m_Provider.GetService<MainWindowViewModel>();

		/// <summary>
		/// Gets a new AlgorithmConsoleViewModel.
		/// </summary>
		public AlgorithmConsoleViewModel AlgorithmConsoleViewModel
			=> m_Provider.GetService<AlgorithmConsoleViewModel>();

		/// <summary>
		/// Gets a new RotationCoreStandardViewModel.
		/// </summary>
		public RotationCoreStandardViewModel RotationCoreStandardViewModel
			=> m_Provider.GetService<RotationCoreStandardViewModel>();
	}
}
