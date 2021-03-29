using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Twisty.Runner.Models.Model3d;
using Twisty.Runner.Services;
using Twisty.Runner.Models;
using System.Windows.Input;
using Twisty.Runner.Wpf;

namespace Twisty.Runner.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		// Services
		private readonly IRotationCoreService m_RotationCoreService;

		// Internal models
		private RotationCoreObject m_Core;
		private CoreRotations m_VisualPositions;

		public MainWindowViewModel(IRotationCoreService rotationCoreService)
		{
			m_RotationCoreService = rotationCoreService;
			this.LoadCore("Rubik[3]");

			this.SelectCore = new RelayCommand(
				(p) => this.LoadCore((string)p)
			);
		}

		/// <summary>
		/// Gets the RotationCoreObject used in this window.
		/// </summary>
		public RotationCoreObject Core
		{
			get => m_Core;
			set
			{
				m_Core = value;
				if (PropertyChanged != null)
					this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(Core)));
			}
		}

		public CoreRotations VisualPositions
		{
			get => m_VisualPositions;
			set
			{
				m_VisualPositions = value;
				if (PropertyChanged != null)
					this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(VisualPositions)));
			}
		}

		public ICommand SelectCore { get; }

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion INotifyPropertyChanged Members

		private void LoadCore(string id)
		{
			this.Core = m_RotationCoreService.CreateNewCore(id, () => RefreshPositions());
			this.RefreshPositions();
		}

		private void RefreshPositions()
		{
			this.VisualPositions = this.m_RotationCoreService.CalculatePositions(this.Core);
		}
	}
}
