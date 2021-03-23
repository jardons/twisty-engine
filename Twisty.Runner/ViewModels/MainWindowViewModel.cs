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

namespace Twisty.Runner.ViewModels
{
	public class MainWindowViewModel : INotifyPropertyChanged
	{
		private readonly IRotationCoreService m_RotationCoreService;

		private CoreRotations m_VisualPositions;

		public MainWindowViewModel(IRotationCoreService rotationCoreService)
		{
			m_RotationCoreService = rotationCoreService;
			this.Core = m_RotationCoreService.CreateNewCore("todo", () => RefreshCore());
		}

		/// <summary>
		/// Gets the RotationCoreObject used in this window.
		/// </summary>
		public RotationCoreObject Core { get; }

		public IEnumerable<Core3dObject> VisualObjects
			=> this.m_RotationCoreService.Materialize(this.Core).Objects.Select(o => new Core3dObject(o));

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

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion INotifyPropertyChanged Members

		private void RefreshCore()
		{
			this.VisualPositions = this.m_RotationCoreService.CalculatePositions(this.Core);
		}
	}
}
