using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Twisty.Runner.Models;
using Twisty.Runner.Services;
using Twisty.Runner.ViewModels.Abstracts;
using Twisty.Runner.Wpf;

namespace Twisty.Runner.ViewModels
{
	public class AlgorithmLibraryViewModel : BaseViewModel
	{
		// Services
		private readonly IRotationCoreService m_RotationCoreService;
		private readonly AlgorithmsStorageService m_AlgorithmsService;

		public AlgorithmLibraryViewModel(IRotationCoreService rotationCoreService, AlgorithmsStorageService algorithmsService)
		{
			m_RotationCoreService = rotationCoreService;
			m_AlgorithmsService = algorithmsService;

			Library = new ObservableCollection<AlgorithmEntry>();

			this.ExecuteCommand = new RelayCommand(
				(command) => this.Execute(command as string)
			);
		}

		private RotationCoreObject m_Core;
		public RotationCoreObject Core
		{
			get => m_Core;
			set
			{
				if (!ReferenceEquals(value, m_Core))
				{
					m_Core = value;
					this.TriggerPropertyChanged(nameof(Core));

					LoadLibrary();
				}
			}
		}

		public ObservableCollection<AlgorithmEntry> Library { get; }

		public RelayCommand ExecuteCommand { get; private set; }

		#region Private Members

		private void LoadLibrary()
		{
			this.Library.Clear();

			if (this.m_Core is null)
				return;

			foreach (var a in this.m_AlgorithmsService.ReadAlgorithms(this.m_Core.Id))
				this.Library.Add(a);
		}

		private void Execute(string command)
		{
			m_RotationCoreService.RunCommand(this.Core, command);
		}

		#endregion Private Members
	}
}
