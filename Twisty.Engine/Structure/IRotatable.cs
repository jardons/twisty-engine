using System.Collections.Generic;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Interface providing operation on object providing rotations.
	/// </summary>
	public interface IRotatable
	{
		/// <summary>
		/// Gets the list of axes available for rotation around this core.
		/// </summary>
		IEnumerable<RotationAxis> Axes { get; }

		/// <summary>
		/// Gets an axis using its id.
		/// </summary>
		/// <param name="axisId">Id of the axis we are looking up.</param>
		/// <returns>Axis for the corresponding or null if not found.</returns>
		RotationAxis GetAxis(string axisId);

		/// <summary>
		/// Rotate a face around a specified rotation axis.
		/// </summary>
		/// <param name="axis">Rotation axis aroung which the rotation will be executed.</param>
		/// <param name="theta">Angle of the rotation to execute.</param>
		/// <param name="distance">
		/// Distance of the center above which blocks will be rotated.
		/// If null, All blocks around the axis are rotated.
		/// </param>
		void RotateAround(RotationAxis axis, double theta, LayerSeparator aboveLayer = null);
	}
}