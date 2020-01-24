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
	public class RubikCube : RotationCore
	{
		#region Const Members

		public const string FACE_ID_DOWN = "D";
		public const string FACE_ID_UP = "U";
		public const string FACE_ID_RIGHT = "R";
		public const string FACE_ID_LEFT = "L";
		public const string FACE_ID_FRONT = "F";
		public const string FACE_ID_BACK = "B";

		public static readonly Cartesian3dCoordinate FACE_POSITION_UP = new Cartesian3dCoordinate(0.0, 0.0, 1.0);
		public static readonly Cartesian3dCoordinate FACE_POSITION_DOWN = new Cartesian3dCoordinate(0.0, 0.0, -1.0);
		public static readonly Cartesian3dCoordinate FACE_POSITION_RIGHT = new Cartesian3dCoordinate(0.0, 1.0, 0.0);
		public static readonly Cartesian3dCoordinate FACE_POSITION_LEFT = new Cartesian3dCoordinate(0.0, -1.0, 0.0);
		public static readonly Cartesian3dCoordinate FACE_POSITION_FRONT = new Cartesian3dCoordinate(1.0, 0.0, 0.0);
		public static readonly Cartesian3dCoordinate FACE_POSITION_BACK = new Cartesian3dCoordinate(-1.0, 0.0, 0.0);

		public static readonly Cartesian3dCoordinate BLOCK_POSITION_CORNER_UP_FRONT_LEFT = new Cartesian3dCoordinate(1.0, -1.0, 1.0);
		public static readonly Cartesian3dCoordinate BLOCK_POSITION_CORNER_UP_FRONT_RIGHT = new Cartesian3dCoordinate(1.0, 1.0, 1.0);
		public static readonly Cartesian3dCoordinate BLOCK_POSITION_CORNER_UP_BACK_LEFT = new Cartesian3dCoordinate(-1.0, -1.0, 1.0);
		public static readonly Cartesian3dCoordinate BLOCK_POSITION_CORNER_UP_BACK_RIGHT = new Cartesian3dCoordinate(-1.0, 1.0, 1.0);
		public static readonly Cartesian3dCoordinate BLOCK_POSITION_CORNER_DOWN_FRONT_LEFT = new Cartesian3dCoordinate(1.0, -1.0, -1.0);
		public static readonly Cartesian3dCoordinate BLOCK_POSITION_CORNER_DOWN_FRONT_RIGHT = new Cartesian3dCoordinate(1.0, 1.0, -1.0);
		public static readonly Cartesian3dCoordinate BLOCK_POSITION_CORNER_DOWN_BACK_LEFT = new Cartesian3dCoordinate(-1.0, -1.0, -1.0);
		public static readonly Cartesian3dCoordinate BLOCK_POSITION_CORNER_DOWN_BACK_RIGHT = new Cartesian3dCoordinate(-1.0, 1.0, -1.0);

		#endregion Const Members

		/// <summary>
		/// Create a new Rubiks cube of size N.
		/// </summary>
		/// <param name="n">Indicate the number of rows per face of the cube that is currently generated.</param>
		/// <exception cref="ArgumentException">Size of the Rubik's Cube should be bigger than 1.</exception>
		public RubikCube(int n)
			: base(GenerateBlocks(n), GenerateAxes(), GenerateFaces())
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
			double theta = isClockwise ? -Math.PI / 2.0 : Math.PI / 2.0;

			// Perform the manipulation for the 4 corners.
			base.SwitchAndRotate(blocks.OfType<RubikCornerBlock>().ToList(), axis.Vector, theta);

			// Perform the edges rotation.
			base.SwitchAndRotate(blocks.OfType<RubikEdgeBlock>().ToList(), axis.Vector, theta);

			// Perform the center rotation.
			base.SwitchAndRotate(blocks.OfType<RubikCenterBlock>().ToList(), axis.Vector, theta);
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
				new RotationAxis(FACE_ID_UP, FACE_POSITION_UP),
				new RotationAxis(FACE_ID_DOWN, FACE_POSITION_DOWN),
				new RotationAxis(FACE_ID_FRONT, FACE_POSITION_FRONT),
				new RotationAxis(FACE_ID_BACK, FACE_POSITION_BACK),
				new RotationAxis(FACE_ID_RIGHT, FACE_POSITION_RIGHT),
				new RotationAxis(FACE_ID_LEFT, FACE_POSITION_LEFT),
			};
		}

		/// <summary>
		/// Generate the axes that will be available for the rotation of the cube.
		/// </summary>
		/// <returns>The list of faces available on a Rubik's cube.</returns>
		private static IEnumerable<CoreFace> GenerateFaces()
		{
			return new List<CoreFace>()
			{
				new CoreFace(FACE_ID_UP, new Plane(FACE_POSITION_UP, 1.0)),
				new CoreFace(FACE_ID_DOWN, new Plane(FACE_POSITION_DOWN, 1.0)),
				new CoreFace(FACE_ID_FRONT, new Plane(FACE_POSITION_FRONT, 1.0)),
				new CoreFace(FACE_ID_BACK, new Plane(FACE_POSITION_BACK, 1.0)),
				new CoreFace(FACE_ID_RIGHT, new Plane(FACE_POSITION_RIGHT, 1.0)),
				new CoreFace(FACE_ID_LEFT, new Plane(FACE_POSITION_LEFT, 1.0)),
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
			AddCornerToList(blocks);

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
		/// Add the 8 corners common to all type of Rubiks cube.
		/// </summary>
		/// <param name="blocks">List of block to which created corners will be added.</param>
		private static void AddCornerToList(IList<Block> blocks)
		{
			// 4 bottoms corners.
			blocks.Add(new RubikCornerBlock(
				BLOCK_POSITION_CORNER_DOWN_FRONT_RIGHT,
				new BlockFace(FACE_ID_DOWN, FACE_POSITION_DOWN),
				new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT),
				new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)
			));

			blocks.Add(new RubikCornerBlock(
				BLOCK_POSITION_CORNER_DOWN_BACK_RIGHT,
				new BlockFace(FACE_ID_DOWN, FACE_POSITION_DOWN),
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK),
				new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)
			));

			blocks.Add(new RubikCornerBlock(
				BLOCK_POSITION_CORNER_DOWN_FRONT_LEFT,
				new BlockFace(FACE_ID_DOWN, FACE_POSITION_DOWN),
				new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT),
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)
			));

			blocks.Add(new RubikCornerBlock(
				BLOCK_POSITION_CORNER_DOWN_BACK_LEFT,
				new BlockFace(FACE_ID_DOWN, FACE_POSITION_DOWN),
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK),
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)
			));

			// 4 tops corners.
			blocks.Add(new RubikCornerBlock(
				BLOCK_POSITION_CORNER_UP_FRONT_RIGHT,
				new BlockFace(FACE_ID_UP, FACE_POSITION_UP),
				new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT),
				new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)
			));

			blocks.Add(new RubikCornerBlock(
				BLOCK_POSITION_CORNER_UP_BACK_RIGHT,
				new BlockFace(FACE_ID_UP, FACE_POSITION_UP),
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK),
				new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)
			));

			blocks.Add(new RubikCornerBlock(
				BLOCK_POSITION_CORNER_UP_FRONT_LEFT,
				new BlockFace(FACE_ID_UP, FACE_POSITION_UP),
				new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT),
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)
			));

			blocks.Add(new RubikCornerBlock(
				BLOCK_POSITION_CORNER_UP_BACK_LEFT,
				new BlockFace(FACE_ID_UP, FACE_POSITION_UP),
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK),
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)
			));
		}

		/// <summary>
		/// Add the 6 centers for Rubiks' cube of even size.
		/// </summary>
		/// <param name="blocks">List of block to which created centers will be added.</param>
		private static void AddCentersToList(IList<Block> blocks)
		{
			blocks.Add(new RubikCenterBlock(FACE_POSITION_DOWN, new BlockFace(FACE_ID_DOWN, FACE_POSITION_DOWN)));
			blocks.Add(new RubikCenterBlock(FACE_POSITION_UP, new BlockFace(FACE_ID_UP, FACE_POSITION_UP)));
			blocks.Add(new RubikCenterBlock(FACE_POSITION_LEFT, new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)));
			blocks.Add(new RubikCenterBlock(FACE_POSITION_RIGHT, new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)));
			blocks.Add(new RubikCenterBlock(FACE_POSITION_FRONT, new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT)));
			blocks.Add(new RubikCenterBlock(FACE_POSITION_BACK, new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK)));
		}

		/// <summary>
		/// Add the edges for Rubiks' cube bigger than 2.
		/// </summary>
		/// <param name="blocks">List of block to which created edges will be added.</param>
		private static void AddEdgesToList(IList<Block> blocks)
		{
			blocks.Add(new RubikEdgeBlock(FACE_POSITION_DOWN + FACE_POSITION_FRONT,
				new BlockFace(FACE_ID_DOWN, FACE_POSITION_DOWN),
				new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT)));
			blocks.Add(new RubikEdgeBlock(FACE_POSITION_DOWN + FACE_POSITION_LEFT,
				new BlockFace(FACE_ID_DOWN, FACE_POSITION_DOWN),
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)));
			blocks.Add(new RubikEdgeBlock(FACE_POSITION_DOWN + FACE_POSITION_BACK,
				new BlockFace(FACE_ID_DOWN, FACE_POSITION_DOWN),
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK)));
			blocks.Add(new RubikEdgeBlock(FACE_POSITION_DOWN + FACE_POSITION_RIGHT,
				new BlockFace(FACE_ID_DOWN, FACE_POSITION_DOWN),
				new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)));

			blocks.Add(new RubikEdgeBlock(FACE_POSITION_UP + FACE_POSITION_FRONT,
				new BlockFace(FACE_ID_UP, FACE_POSITION_UP),
				new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT)));
			blocks.Add(new RubikEdgeBlock(FACE_POSITION_UP + FACE_POSITION_LEFT,
				new BlockFace(FACE_ID_UP, FACE_POSITION_UP),
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)));
			blocks.Add(new RubikEdgeBlock(FACE_POSITION_UP + FACE_POSITION_BACK,
				new BlockFace(FACE_ID_UP, FACE_POSITION_UP),
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK)));
			blocks.Add(new RubikEdgeBlock(FACE_POSITION_UP + FACE_POSITION_RIGHT,
				new BlockFace(FACE_ID_UP, FACE_POSITION_UP),
				new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)));

			blocks.Add(new RubikEdgeBlock(FACE_POSITION_LEFT + FACE_POSITION_FRONT,
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT),
				new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT)));
			blocks.Add(new RubikEdgeBlock(FACE_POSITION_FRONT + FACE_POSITION_RIGHT,
				new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT),
				new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)));
			blocks.Add(new RubikEdgeBlock(FACE_POSITION_RIGHT + FACE_POSITION_BACK,
				new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT),
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK)));
			blocks.Add(new RubikEdgeBlock(FACE_POSITION_BACK + FACE_POSITION_LEFT,
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK),
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)));
		}

		#endregion Private Members
	}
}