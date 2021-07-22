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
	public class AlgorithmConsoleViewModel : BaseViewModel
	{
		// Services
		private readonly IRotationCoreService m_RotationCoreService;

		// Private Fields
		private string m_InputAlgoritm;

		public AlgorithmConsoleViewModel(IRotationCoreService rotationCoreService)
		{
			m_RotationCoreService = rotationCoreService;

			History = new ObservableCollection<string>();

			this.ExecuteCommand = new RelayCommand(
				(p) => this.ExecuteInput(),
				(p) => this.HasInputAlgorithm && this.IsValidAlgorithm
			);

			// Link Action to fields.
			this.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(HasInputAlgorithm)
						|| e.PropertyName == nameof(IsValidAlgorithm))
					ExecuteCommand.RaiseCanExecuteChanged();
			};
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

					this.History.Clear();
					this.TriggerPropertyChanged(nameof(History));
				}
			}
		}

		public string InputAlgoritm
		{
			get => m_InputAlgoritm;
			set
			{
				bool oldHasInput = this.HasInputAlgorithm;
				bool oldValid = this.IsValidAlgorithm;

				m_InputAlgoritm = value;

				this.IsValidAlgorithm = m_RotationCoreService.TryCleanCommand(this.Core.Id, m_InputAlgoritm, out string command);
				if (this.IsValidAlgorithm)
					m_InputAlgoritm = command;

				// Trigger View Update.
				this.TriggerPropertyChanged(nameof(InputAlgoritm));
				if (oldHasInput != this.HasInputAlgorithm)
					this.TriggerPropertyChanged(nameof(HasInputAlgorithm));
				if (oldValid != this.IsValidAlgorithm)
					this.TriggerPropertyChanged(nameof(IsValidAlgorithm));
			}
		}

		public bool HasInputAlgorithm
			=> !string.IsNullOrWhiteSpace(m_InputAlgoritm);

		public bool IsValidAlgorithm { get; private set; }

		public ObservableCollection<string> History { get; }

		public RelayCommand ExecuteCommand { get; private set; }

		#region Private Members

		private void ExecuteInput()
		{
			m_RotationCoreService.RunCommand(this.Core, this.InputAlgoritm);
			AddToHistory(this.InputAlgoritm);
			this.InputAlgoritm = string.Empty;
		}

		private void AddToHistory(string command)
		{
			if (string.IsNullOrWhiteSpace(command))
				return;

			// If item is already in history, avoid duplicate.
			if (this.History.Contains(command))
				this.History.Remove(command);

			// Add at the top of history.
			this.History.Insert(0, command);

			// Trigger View Update.
			this.TriggerPropertyChanged(nameof(History));
		}

		#endregion Private Members
	}
}
