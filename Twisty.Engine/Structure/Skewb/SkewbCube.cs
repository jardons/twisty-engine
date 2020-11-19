using System;
using System.Collections.Generic;
using System.Linq;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure.Skewb
{
	/// <summary>
	/// Class describing a Skewb cube.
	/// </summary>
	public class SkewbCube : CubicRotationCore
	{
		#region Const Members

		public const string ID_AXIS_UP_FRONT_LEFT = "UFL";
		public const string ID_AXIS_UP_FRONT_RIGHT = "UFR";
		public const string ID_AXIS_UP_BACK_LEFT = "UBL";
		public const string ID_AXIS_UP_BACK_RIGHT = "UBR";
		public const string ID_AXIS_DOWN_FRONT_LEFT = "DFL";
		public const string ID_AXIS_DOWN_FRONT_RIGHT = "DFR";
		public const string ID_AXIS_DOWN_BACK_LEFT = "DBL";
		public const string ID_AXIS_DOWN_BACK_RIGHT = "DBR";

		#endregion Const Members

		/// <summary>
		/// Create a new Skewb cube.
		/// </summary>
		public SkewbCube()
			: base(GenerateBlocks(), GenerateAxes())
		{
		}

		/// <summary>
		/// Rotate a face around a specified rotation axis.
		/// </summary>
		/// <param name="axis">Rotation axis aroung which the rotation will be executed.</param>
		/// <param name="isClockwise">Boolean indicating if whether the rotation is clockwise or not.</param>
		public void RotateAround(RotationAxis axis, bool isClockwise)
		{
			// Gets Blocks ordered in rotation order.
			var blocks = GetOrderedBlocks(axis, isClockwise);
			if (blocks.Count == 0)
				return;

			// Convert the rotation direction to the correct angle.
			double theta = isClockwise ? Math.PI * (2.0 / 3.0) : -Math.PI * (2.0 / 3.0);

			// Perform the manipulation for the central corners.
			var corners = blocks.OfType<CornerBlock>();
			corners.Where(c => c.Position.IsSameVector(axis.Vector)).FirstOrDefault().RotateAround(axis.Vector, theta);

			// Perform the manipulation for the 3 external corners.
			base.SwitchAndRotate(corners.Where(c => !c.Position.IsSameVector(axis.Vector)).ToList(), axis.Vector, theta);

			// Perform the centers rotation.
			base.SwitchAndRotate(blocks.OfType<CenterBlock>().ToList(), axis.Vector, theta);
		}

		#region Private Members

		/// <summary>
		/// Gets the block ordered in a rotational order arround the provided axis.
		/// </summary>
		/// <param name="axis">Axis around which the blocks will be ordered.</param>
		/// <param name="isClockwise">Boolean indicating if whether the rotation direction is clockwise or not.</param>
		/// <returns>The ordered collection of blocks.</returns>
		private IList<IPositionnedByCartesian3dVector> GetOrderedBlocks(RotationAxis axis, bool isClockwise)
		{
			Plane p = new Plane(axis.Vector, 0.0);

			// Select all blocks that will be included in the rotation.
			var blocks = this.Blocks.Where(b => p.IsAbovePlane(b.Position)).OfType<IPositionnedByCartesian3dVector>().ToList();
			if (blocks.Count == 0)
				return blocks;

			blocks.Sort(new CircularVectorComparer(p));
			if (!isClockwise)
				blocks.Reverse();

			return blocks;
		}

		/// <summary>
		/// Generate the axes that will be available for the rotation of the cube.
		/// </summary>
		/// <returns>The list of axis available on a Skewb cube.</returns>
		private static IEnumerable<RotationAxis> GenerateAxes()
		{
			return new List<RotationAxis>()
			{
				new RotationAxis(ID_AXIS_UP_FRONT_LEFT, POSITION_CORNER_UP_FRONT_LEFT),
				new RotationAxis(ID_AXIS_UP_FRONT_RIGHT, POSITION_CORNER_UP_FRONT_RIGHT),
				new RotationAxis(ID_AXIS_UP_BACK_LEFT, POSITION_CORNER_UP_BACK_LEFT),
				new RotationAxis(ID_AXIS_UP_BACK_RIGHT, POSITION_CORNER_UP_BACK_RIGHT),
				new RotationAxis(ID_AXIS_DOWN_FRONT_LEFT, POSITION_CORNER_DOWN_FRONT_LEFT),
				new RotationAxis(ID_AXIS_DOWN_FRONT_RIGHT, POSITION_CORNER_DOWN_FRONT_RIGHT),
				new RotationAxis(ID_AXIS_DOWN_BACK_LEFT, POSITION_CORNER_DOWN_BACK_LEFT),
				new RotationAxis(ID_AXIS_DOWN_BACK_RIGHT, POSITION_CORNER_DOWN_BACK_RIGHT),
			};
		}

		/// <summary>
		/// Generate the blocks that will form the current Skewb Cube.
		/// </summary>
		/// <param name="n">Indicate the number of rows per face of the cube that is currently generated.</param>
		/// <returns>The list of block that represent the current cube.</returns>
		private static IEnumerable<Block> GenerateBlocks()
		{
			List<Block> blocks = new List<Block>();

			CubicRotationCore.AddCornersToList(blocks);
			CubicRotationCore.AddCentersToList(blocks);

			return blocks;
		}

		#endregion Private Members
	}
}
