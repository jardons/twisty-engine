using System;
using System.Collections.Generic;
using System.Linq;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure.Rubiks
{
	/// <summary>
	/// Class describing Rubiks cube of size N.
	/// The value N is indicating the count of rows per face of the cube.
	/// </summary>
	public class RubikCube : CubicRotationCore
	{
		/// <summary>
		/// Create a new Rubiks cube of size N.
		/// </summary>
		/// <param name="n">Indicate the number of rows per face of the cube that is currently generated.</param>
		/// <exception cref="ArgumentException">Size of the Rubik's Cube should be bigger than 1.</exception>
		public RubikCube(int n)
			: base(GenerateBlocks(n), GenerateAxes())
		{
			this.N = n;
		}

		/// <summary>
		/// Gets the count of layer in this cube.
		/// </summary>
		public int N { get; }

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
			double theta = isClockwise ? Math.PI / 2.0 : -Math.PI / 2.0;

			// Perform the manipulation for the 4 corners.
			base.SwitchAndRotate(blocks.OfType<CornerBlock>().ToList(), axis.Vector, theta);

			// Perform the edges rotation.
			base.SwitchAndRotate(blocks.OfType<EdgeBlock>().ToList(), axis.Vector, theta);

			// Perform the center rotation.
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
			// Select all blocks that will be included in the rotation.
			var blocks = base.GetBlocksForFace(axis.Vector).OfType<IPositionnedByCartesian3dVector>().ToList();
			if (blocks.Count == 0)
				return blocks;

			Plane p = new Plane(axis.Vector, blocks[0].Position);
			blocks.Sort(new CircularVectorComparer(p));
			if (isClockwise)
				blocks.Reverse();

			return blocks;
		}

		/// <summary>
		/// Generate the axes that will be available for the rotation of the cube.
		/// </summary>
		/// <returns>The list of axis available on a Rubik's cube.</returns>
		private static IEnumerable<RotationAxis> GenerateAxes()
		{
			return new List<RotationAxis>()
			{
				new RotationAxis(ID_FACE_UP, POSITION_FACE_UP),
				new RotationAxis(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new RotationAxis(ID_FACE_FRONT, POSITION_FACE_FRONT),
				new RotationAxis(ID_FACE_BACK, POSITION_FACE_BACK),
				new RotationAxis(ID_FACE_RIGHT, POSITION_FACE_RIGHT),
				new RotationAxis(ID_FACE_LEFT, POSITION_FACE_LEFT),
			};
		}

		/// <summary>
		/// Generate the blocks that will form the current cube of size N.
		/// </summary>
		/// <param name="n">Indicate the number of rows per face of the cube that is currently generated.</param>
		/// <exception cref="ArgumentException">Size of the Rubik's Cube should be bigger than 1.</exception>
		/// <returns>The list of block that represent the current cube.</returns>
		private static IEnumerable<Block> GenerateBlocks(int n)
		{
			if (n <= 1)
				throw new ArgumentException("Size of the Rubik's Cube should be bigger than 1.", nameof(n));

			List<Block> blocks = new List<Block>();

			// Corner are identical in all cubes.
			AddCornersToList(blocks);

			// Center are only available for cube of size N where N is odd.
			if (n % 2 == 1)
				AddCentersToList(blocks);

			// For block of size N > 3, Single Face blocks must be created between center and borders.

			// Border count is related to the value of N.
			if (n > 2)
				AddEdgesToList(blocks);

			return blocks;
		}

		/// <summary>
		/// Add the edges for Rubiks' cube bigger than 2.
		/// </summary>
		/// <param name="blocks">List of block to which created edges will be added.</param>
		private static void AddEdgesToList(IList<Block> blocks)
		{
			blocks.Add(new EdgeBlock(POSITION_FACE_DOWN + POSITION_FACE_FRONT,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT)));
			blocks.Add(new EdgeBlock(POSITION_FACE_DOWN + POSITION_FACE_LEFT,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT)));
			blocks.Add(new EdgeBlock(POSITION_FACE_DOWN + POSITION_FACE_BACK,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK)));
			blocks.Add(new EdgeBlock(POSITION_FACE_DOWN + POSITION_FACE_RIGHT,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT)));

			blocks.Add(new EdgeBlock(POSITION_FACE_UP + POSITION_FACE_FRONT,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT)));
			blocks.Add(new EdgeBlock(POSITION_FACE_UP + POSITION_FACE_LEFT,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT)));
			blocks.Add(new EdgeBlock(POSITION_FACE_UP + POSITION_FACE_BACK,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK)));
			blocks.Add(new EdgeBlock(POSITION_FACE_UP + POSITION_FACE_RIGHT,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT)));

			blocks.Add(new EdgeBlock(POSITION_FACE_LEFT + POSITION_FACE_FRONT,
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT),
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT)));
			blocks.Add(new EdgeBlock(POSITION_FACE_FRONT + POSITION_FACE_RIGHT,
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT),
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT)));
			blocks.Add(new EdgeBlock(POSITION_FACE_RIGHT + POSITION_FACE_BACK,
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT),
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK)));
			blocks.Add(new EdgeBlock(POSITION_FACE_BACK + POSITION_FACE_LEFT,
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK),
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT)));
		}

		#endregion Private Members
	}
}