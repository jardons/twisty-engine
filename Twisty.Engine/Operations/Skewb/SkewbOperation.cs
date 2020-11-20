using Twisty.Engine.Structure.Skewb;

namespace Twisty.Engine.Operations.Skewb
{
	/// <summary>
	/// Class describing an operation to perform on a Skewb cube.
	/// </summary>
	public class SkewbOperation : IOperation<SkewbCube>
	{
		/// <summary>
		/// Create a new SkewbOperation.
		/// </summary>
		/// <param name="axisId">Id of the axis around which the rotation is executed.</param>
		/// <param name="isClockwise">Boolean indicating whether the rotation is clockwise or not.</param>
		public SkewbOperation(string axisId, bool isClockwise)
		{
			this.AxisId = axisId;
			this.IsClockwise = isClockwise;
		}

		/// <summary>
		/// Gets the Id of the axis around which the rotation is executed.
		/// </summary>
		public string AxisId { get; }

		/// <summary>
		/// Gets a boolean indicating whether the rotation is clockwise or not.
		/// </summary>
		public bool IsClockwise { get; }

		#region IOperation<SkewbCube>

		/// <summary>
		/// Execute the current operation on the provided Core.
		/// </summary>
		/// <param name="core">Rotation core on which the operation is executed.</param>
		public void ExecuteOn(SkewbCube core)
		{
			core.RotateAround(core.GetAxis(AxisId), IsClockwise);
		}

		#endregion IOperation<SkewbCube>
	}
}
