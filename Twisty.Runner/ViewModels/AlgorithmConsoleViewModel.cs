using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Runner.Models;
using Twisty.Runner.Services;
using Twisty.Runner.Wpf;

namespace Twisty.Runner.ViewModels
{
	public class AlgorithmConsoleViewModel : INotifyPropertyChanged
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
				(p) => this.HasInputAlgoritm
			);

			// Link Action to fields.
			this.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(HasInputAlgoritm))
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
					this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(Core)));
				}
			}
		}

		public string InputAlgoritm
		{
			get => m_InputAlgoritm;
			set
			{
				bool oldHasInput = this.HasInputAlgoritm;

				m_InputAlgoritm = value;
				var parser = new RubikOperationsParser();
				if (parser.TryClean(m_InputAlgoritm, out string command))
					m_InputAlgoritm = command;

				// Trigger View Update.
				this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(InputAlgoritm)));
				if (oldHasInput != this.HasInputAlgoritm)
					this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(HasInputAlgoritm)));
			}
		}

		public bool HasInputAlgoritm
			=> !string.IsNullOrWhiteSpace(m_InputAlgoritm);

		public ObservableCollection<string> History { get; }

		public RelayCommand ExecuteCommand { get; private set; }

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion INotifyPropertyChanged Members

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
			this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(History)));
		}

		#endregion Private Members
	}
}
