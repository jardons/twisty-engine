using System;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Class describing a rotation axis around the RotationCore.
	/// The axis is defined by an angle coming from the RotationCore center and goind trough on of the faces.
	/// </summary>
	public class RotationAxis
	{
		/// <summary>
		/// Create a new RotationAxis object.
		/// </summary>
		/// <param name="id">Id of the RotationAxis.</param>
		/// <param name="axis">Rotation Axis coordinate starting from the core center.</param>
		/// <exception cref="System.ArgumentNullException">Axis id is mandatory.</exception>
		/// <exception cref="System.ArgumentException">Axis id cannot be an empty or a white string.</exception>
		public RotationAxis(string id, Cartesian3dCoordinate axis)
		{
			if (id == null)
				throw new ArgumentNullException(nameof(id), "Axis id is mandatory.");

			if (string.IsNullOrWhiteSpace(id))
				throw new ArgumentException("Axis id cannot be an empty or a white string.", nameof(id));

			this.Id = id;
			this.Vector = axis;
		}

		#region Public Properties

		/// <summary>
		/// Gets the Id of the RotationAxis.
		/// </summary>
		public string Id { get; }

		/// <summary>
		/// Gets the Axis coordinate used for the rotation.
		/// </summary>
		public Cartesian3dCoordinate Vector { get; }

		#endregion Public Properties
	}
}