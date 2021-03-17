using Twisty.Engine.Structure;

namespace Twisty.Engine.Operations
{
	/// <summary>
	/// Class describing an operation to perform a rotation of all blocks aroung an axis.
	/// </summary>
	public class AxisOperation : IOperation
	{
		/// <summary>
		/// Create a new AxisOperation.
		/// </summary>
		/// <param name="axisId">Id of the axis around which the rotation is executed.</param>
		/// <param name="theta">Rotation angle to execute in radians. A positive value will indicate a clockwise rotation.</param>
		public AxisOperation(string axisId, double theta)
		{
			this.AxisId = axisId;
			this.Theta = theta;
		}

		/// <summary>
		/// Gets the Id of the axis around which the rotation is executed.
		/// </summary>
		public string AxisId { get; }

		/// <summary>
		/// Gets the rotation angle to execute in radians. A positive value will indicate a clockwise rotation.
		/// </summary>
		public double Theta { get; }

		#region IOperation

		/// <summary>
		/// Execute the current operation on the provided IRotatable core.
		/// </summary>
		/// <param name="core">IRotatable core on which the operation is executed.</param>
		public void ExecuteOn(IRotatable core)
		{
			core.RotateAround(core.GetAxis(AxisId), this.Theta);
		}

		#endregion IOperation
	}
}
