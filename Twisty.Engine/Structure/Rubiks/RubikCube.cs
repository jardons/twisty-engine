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
			: base(GenerateBlocks(n), GenerateAxes(n))
		{
			this.N = n;
		}

		/// <summary>
		/// Gets the count of layer in this cube.
		/// </summary>
		public int N { get; }

		#region Private Members

		/// <summary>
		/// Generate the axes that will be available for the rotation of the cube.
		/// </summary>
		/// <param name="n">Number of layers to use in the Cube.</param>
		/// <returns>The list of axis available on a Rubik's cube.</returns>
		private static IEnumerable<RotationAxis> GenerateAxes(int n)
		{
			return new List<RotationAxis>()
			{
				CreateAxis(ID_FACE_UP, POSITION_FACE_UP, n),
				CreateAxis(ID_FACE_DOWN, POSITION_FACE_DOWN, n),
				CreateAxis(ID_FACE_FRONT, POSITION_FACE_FRONT, n),
				CreateAxis(ID_FACE_BACK, POSITION_FACE_BACK, n),
				CreateAxis(ID_FACE_RIGHT, POSITION_FACE_RIGHT, n),
				CreateAxis(ID_FACE_LEFT, POSITION_FACE_LEFT, n),
			};
		}

		/// <summary>
		/// Create an axis with the layer description for the current size of Rubik's Cube.
		/// </summary>
		/// <param name="id">Id of hte axis.</param>
		/// <param name="direction">Direction of the axis vector.</param>
		/// <param name="n">Number of layers to use in the Cube.</param>
		/// <returns>A newly generated RotationAxis.</returns>
		private static RotationAxis CreateAxis(string id, Cartesian3dCoordinate direction, int n)
		{
			Dictionary<string, double> layers = new Dictionary<string, double>();
			int mod2 = n % 2;
			if (mod2 == 0)
				layers.Add($"L0_{id}", 0.0);

			double layerSize = 2.0 / n;
			int n2 = (n / 2) + mod2;
			for (int i = 1; i < n2; ++i)
				layers.Add($"L{i}_{id}", -layerSize * (Convert.ToDouble(i) - 0.5));

			return new RotationAxis(id, direction, layers);
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
			blocks.Add(CreateEdgeBlock(POSITION_FACE_DOWN + POSITION_FACE_FRONT,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT)));
			blocks.Add(CreateEdgeBlock(POSITION_FACE_DOWN + POSITION_FACE_LEFT,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT)));
			blocks.Add(CreateEdgeBlock(POSITION_FACE_DOWN + POSITION_FACE_BACK,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK)));
			blocks.Add(CreateEdgeBlock(POSITION_FACE_DOWN + POSITION_FACE_RIGHT,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT)));

			blocks.Add(CreateEdgeBlock(POSITION_FACE_UP + POSITION_FACE_FRONT,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT)));
			blocks.Add(CreateEdgeBlock(POSITION_FACE_UP + POSITION_FACE_LEFT,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT)));
			blocks.Add(CreateEdgeBlock(POSITION_FACE_UP + POSITION_FACE_BACK,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK)));
			blocks.Add(CreateEdgeBlock(POSITION_FACE_UP + POSITION_FACE_RIGHT,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT)));

			blocks.Add(CreateEdgeBlock(POSITION_FACE_LEFT + POSITION_FACE_FRONT,
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT),
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT)));
			blocks.Add(CreateEdgeBlock(POSITION_FACE_FRONT + POSITION_FACE_RIGHT,
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT),
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT)));
			blocks.Add(CreateEdgeBlock(POSITION_FACE_RIGHT + POSITION_FACE_BACK,
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT),
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK)));
			blocks.Add(CreateEdgeBlock(POSITION_FACE_BACK + POSITION_FACE_LEFT,
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK),
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT)));
		}

		/// <summary>
		/// Create an edge block.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="face1"></param>
		/// <param name="face2"></param>
		/// <returns></returns>
		private static Block CreateEdgeBlock(Cartesian3dCoordinate position, BlockFace face1, BlockFace face2)
			=> new($"E_{face1.Id}{face2.Id}", position, face1, face2);

		#endregion Private Members
	}
}