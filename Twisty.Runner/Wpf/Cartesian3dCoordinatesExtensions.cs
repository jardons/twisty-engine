using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;
using Twisty.Engine.Geometry;

namespace Twisty.Runner.Wpf
{
	/// <summary>
	/// Cartesian3dCoordinate extensions for WPF usages.
	/// </summary>
	internal static class Cartesian3dCoordinateExtensions
	{
		/// <summary>
		/// Convert the Cartesian3dCoordinate to a Point3D object. 
		/// </summary>
		/// <param name="c">Cartesian3dCoordinate to convert to a Point3D object.</param>
		/// <returns>Point3D object witht he same coordiantes as the provided Cartesian3dCoordinate object.</returns>
		public static Point3D ToPoint3D(this Cartesian3dCoordinate c) => new Point3D(c.X, c.Y, c.Z);
	}
}
