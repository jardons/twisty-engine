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

		public static readonly SphericalVector FACE_POSITION_DOWN = new SphericalVector(0.0, Math.PI);
		public static readonly SphericalVector FACE_POSITION_UP = new SphericalVector(0.0, 0.0);
		public static readonly SphericalVector FACE_POSITION_RIGHT = new SphericalVector(Math.PI / 2.0, Math.PI / 2.0);
		public static readonly SphericalVector FACE_POSITION_LEFT = new SphericalVector(Math.PI * 1.5, Math.PI / 2.0);
		public static readonly SphericalVector FACE_POSITION_FRONT = new SphericalVector(0.0, Math.PI / 2.0);
		public static readonly SphericalVector FACE_POSITION_BACK = new SphericalVector(Math.PI, Math.PI / 2.0);

		public static readonly SphericalVector BLOCK_POSITION_CORNER_UP_FRONT_LEFT = new SphericalVector(Math.PI / 4.0 * 7.0, Math.PI / 4.0);
		public static readonly SphericalVector BLOCK_POSITION_CORNER_UP_FRONT_RIGHT = new SphericalVector(Math.PI / 4.0, Math.PI / 4.0);
		public static readonly SphericalVector BLOCK_POSITION_CORNER_UP_BACK_LEFT = new SphericalVector(Math.PI / 4.0 * 5.0, Math.PI / 4.0);
		public static readonly SphericalVector BLOCK_POSITION_CORNER_UP_BACK_RIGHT = new SphericalVector(Math.PI / 4.0 * 3.0, Math.PI / 4.0);
		public static readonly SphericalVector BLOCK_POSITION_CORNER_DOWN_FRONT_LEFT = new SphericalVector(Math.PI / 4.0 * 7.0, Math.PI / 4.0 * 3.0);
		public static readonly SphericalVector BLOCK_POSITION_CORNER_DOWN_FRONT_RIGHT = new SphericalVector(Math.PI / 4.0, Math.PI / 4.0 * 3.0);
		public static readonly SphericalVector BLOCK_POSITION_CORNER_DOWN_BACK_LEFT = new SphericalVector(Math.PI / 4.0 * 5.0, Math.PI / 4.0 * 3.0);
		public static readonly SphericalVector BLOCK_POSITION_CORNER_DOWN_BACK_RIGHT = new SphericalVector(Math.PI / 4.0 * 3.0, Math.PI / 4.0 * 3.0);

		#endregion Const Members

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
			// Select all blocks that will be included in the rotation.
			var blocks = base.GetBlocksForFace(axis.Vector).OfType<IPositionnedBySphericalVector>().ToList();
			if (blocks.Count == 0)
				return;

			Plane p = new Plane(CoordinateConverter.ConvertToCartesian(axis.Vector), CoordinateConverter.ConvertToCartesian(blocks[0].Position));
			blocks.Sort(new CircularVectorComparer(p));
			if (isClockwise)
				blocks.Reverse();

			// Convert the rotation direction to the correct angle.
			double theta = isClockwise ? -Math.PI / 2.0 : Math.PI / 2.0;

			// Perform the manipulation for the 4 corners.
			base.SwitchAndRotate(blocks.OfType<RubikCornerBlock>().ToList(), axis.Vector, theta);
		}

		#region Private Members

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

			// Center are only available for cube of size N where N is even.

			// For block of size N > 3, Single Face blocks must be created between center and borders.

			// Border count is related to the value of N.

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

		#endregion Private Members
	}
}