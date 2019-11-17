using System;
using System.Collections.Generic;
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

		public const string FACE_ID_BOTTOM = "white";
		public const string FACE_ID_TOP = "yellow";
		public const string FACE_ID_RIGHT = "red";
		public const string FACE_ID_LEFT = "orange";
		public const string FACE_ID_FRONT = "blue";
		public const string FACE_ID_BACK = "green";

		public static readonly SphericalVector FACE_POSITION_BOTTOM = new SphericalVector(0.0, Math.PI);
		public static readonly SphericalVector FACE_POSITION_TOP = new SphericalVector(0.0, 0.0);
		public static readonly SphericalVector FACE_POSITION_RIGHT = new SphericalVector(Math.PI / 2.0, Math.PI / 2.0);
		public static readonly SphericalVector FACE_POSITION_LEFT = new SphericalVector(Math.PI * 1.5, Math.PI / 2.0);
		public static readonly SphericalVector FACE_POSITION_FRONT = new SphericalVector(0.0, Math.PI / 2.0);
		public static readonly SphericalVector FACE_POSITION_BACK = new SphericalVector(Math.PI, Math.PI / 2.0);

		#endregion Const Members

		/// <summary>
		/// Create a new Rubiks cube of size N.
		/// </summary>
		/// <param name="n">Indicate the number of rows per face of the cube that is currently generated.</param>
		/// <exception cref="ArgumentException">Size of the Rubik's Cube should be bigger than 1.</exception>
		public RubikCube(int n)
			: base(GenerateBlocks(n))
		{
		}

		#region Private Members

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
					new SphericalVector(Math.PI / 4.0, Math.PI / 4.0),
					new BlockFace(FACE_ID_BOTTOM, FACE_POSITION_BOTTOM),
					new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT),
					new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)
				));

			blocks.Add(new RubikCornerBlock(
				new SphericalVector(Math.PI / 4.0 * 3.0, Math.PI / 4.0),
				new BlockFace(FACE_ID_BOTTOM, FACE_POSITION_BOTTOM),
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK),
				new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)
			));

			blocks.Add(new RubikCornerBlock(
				new SphericalVector(Math.PI / 4.0 * 5.0, Math.PI / 4.0),
				new BlockFace(FACE_ID_BOTTOM, FACE_POSITION_BOTTOM),
				new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT),
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)
			));

			blocks.Add(new RubikCornerBlock(
				new SphericalVector(Math.PI / 4.0 * 7.0, Math.PI / 4.0),
				new BlockFace(FACE_ID_BOTTOM, FACE_POSITION_BOTTOM),
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK),
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)
			));

			// 4 tops corners.
			blocks.Add(new RubikCornerBlock(
				new SphericalVector(Math.PI / 4.0, Math.PI / 4.0 * 3.0),
				new BlockFace(FACE_ID_TOP, FACE_POSITION_TOP),
				new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT),
				new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)
			));

			blocks.Add(new RubikCornerBlock(
				new SphericalVector(Math.PI / 4.0 * 3.0, Math.PI / 4.0 * 3.0),
				new BlockFace(FACE_ID_TOP, FACE_POSITION_TOP),
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK),
				new BlockFace(FACE_ID_RIGHT, FACE_POSITION_RIGHT)
			));

			blocks.Add(new RubikCornerBlock(
				new SphericalVector(Math.PI / 4.0 * 5.0, Math.PI / 4.0 * 3.0),
				new BlockFace(FACE_ID_TOP, FACE_POSITION_TOP),
				new BlockFace(FACE_ID_FRONT, FACE_POSITION_FRONT),
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)
			));

			blocks.Add(new RubikCornerBlock(
				new SphericalVector(Math.PI / 4.0 * 7.0, Math.PI / 4.0 * 3.0),
				new BlockFace(FACE_ID_TOP, FACE_POSITION_TOP),
				new BlockFace(FACE_ID_BACK, FACE_POSITION_BACK),
				new BlockFace(FACE_ID_LEFT, FACE_POSITION_LEFT)
			));
		}

		#endregion Private Members
	}
}