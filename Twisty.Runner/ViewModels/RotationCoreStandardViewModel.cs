using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Twisty.Runner.Models;

namespace Twisty.Runner.ViewModels
{
	public class RotationCoreStandardViewModel : INotifyPropertyChanged
	{
		private Point3D m_CameraPosition;

		public RotationCoreStandardViewModel()
		{
			AvailableCameras = new ObservableCollection<CameraDescription>();

			AvailableCameras.Add(new CameraDescription { Id = "6,-3,3", Name = "Front Left" });
			AvailableCameras.Add(new CameraDescription { Id = "6,3,3", Name = "Front Right" });
			AvailableCameras.Add(new CameraDescription { Id = "6,3,-3", Name = "Bottom Left" });
			AvailableCameras.Add(new CameraDescription { Id = "6,-3,-3", Name = "Bottom Right" });
			AvailableCameras.Add(new CameraDescription { Id = "-6,-3,3", Name = "Back Left" });

			m_CameraPosition = new Point3D(6, -3, 3);
		}

		#region ICoreDisplayViewModel Members

		/// <summary>
		/// LIst of Camera available in the interface.
		/// </summary>
		public ObservableCollection<CameraDescription> AvailableCameras { get; }

		/// <summary>
		/// Id of the used camera in the drop down selector.
		/// </summary>
		public string ActiveCameraId
		{
			get => $"{Convert.ToInt32(CameraPosition.X)},{Convert.ToInt32(CameraPosition.Y)},{Convert.ToInt32(CameraPosition.Z)}";
			set
			{
				var coordinates = value.Split(",").Select(s => Convert.ToDouble(s)).ToList();

				// PropertyChanged is triggered in CameraPosition.
				this.CameraPosition = new Point3D(coordinates[0], coordinates[1], coordinates[2]);
			}
		}

		/// <summary>
		/// Gets/Sets the position of the camera.
		/// </summary>
		public Point3D CameraPosition
		{
			get => m_CameraPosition;
			set
			{
				this.m_CameraPosition = value;

				if (this.PropertyChanged is not null)
				{
					// Trigger View Update.
					this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(CameraPosition)));
					this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(CameraDirection)));
					this.PropertyChanged(this, new PropertyChangedEventArgs(nameof(ActiveCameraId)));
				}
			}
		}

		/// <summary>
		/// Gets the direction vector of the camera.
		/// </summary>
		public Vector3D CameraDirection
			=> new Vector3D(
				-this.CameraPosition.X,
				-this.CameraPosition.Y,
				-this.CameraPosition.Z);

		#endregion ICoreDisplayViewModel Members

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion INotifyPropertyChanged Members
	}
}
