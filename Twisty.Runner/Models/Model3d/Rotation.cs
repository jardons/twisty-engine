using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;
using Twisty.Engine.Geometry;
using Twisty.Engine.Geometry.Rotations;
using Twisty.Runner.Views;
using Twisty.Runner.Wpf;

namespace Twisty.Runner.Models.Model3d
{
	/// <summary>
	/// Object describing a Rortation used in WPF models.
	/// </summary>
	public class Rotation
	{
		/// <summary>
		/// Create a new Rotation.
		/// </summary>
		/// <param name="rotation">Rotation to present to the WPF application.</param>
		public Rotation(SimpleRotation3d rotation)
		{
			this.Axis = rotation.Axis.ToWpfVector3D();
			this.Angle = -CoordinateConverter.ConvertRadianToDegree(rotation.Angle);
		}

		/// <summary>
		/// Gets the Rotation axis.
		/// </summary>
		public Vector3D Axis { get; }

		/// <summary>
		/// Gets the rotation angle in degrees counter-clockwise.
		/// </summary>
		public double Angle { get; }
	}
}
