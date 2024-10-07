using System.Collections.Generic;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure
{
	/// <summary>
	/// Class describing a Rotation Core providing the faces in a cubic position.
	/// </summary>
	public class CubicRotationCore : RotationCore
	{
		#region Const Members

		public const string ID_FACE_DOWN = "D";
		public const string ID_FACE_UP = "U";
		public const string ID_FACE_RIGHT = "R";
		public const string ID_FACE_LEFT = "L";
		public const string ID_FACE_FRONT = "F";
		public const string ID_FACE_BACK = "B";

		public static readonly Cartesian3dCoordinate POSITION_CORNER_UP_FRONT_LEFT = new Cartesian3dCoordinate(1.0, -1.0, 1.0);
		public static readonly Cartesian3dCoordinate POSITION_CORNER_UP_FRONT_RIGHT = new Cartesian3dCoordinate(1.0, 1.0, 1.0);
		public static readonly Cartesian3dCoordinate POSITION_CORNER_UP_BACK_LEFT = new Cartesian3dCoordinate(-1.0, -1.0, 1.0);
		public static readonly Cartesian3dCoordinate POSITION_CORNER_UP_BACK_RIGHT = new Cartesian3dCoordinate(-1.0, 1.0, 1.0);
		public static readonly Cartesian3dCoordinate POSITION_CORNER_DOWN_FRONT_LEFT = new Cartesian3dCoordinate(1.0, -1.0, -1.0);
		public static readonly Cartesian3dCoordinate POSITION_CORNER_DOWN_FRONT_RIGHT = new Cartesian3dCoordinate(1.0, 1.0, -1.0);
		public static readonly Cartesian3dCoordinate POSITION_CORNER_DOWN_BACK_LEFT = new Cartesian3dCoordinate(-1.0, -1.0, -1.0);
		public static readonly Cartesian3dCoordinate POSITION_CORNER_DOWN_BACK_RIGHT = new Cartesian3dCoordinate(-1.0, 1.0, -1.0);

		public static readonly Cartesian3dCoordinate POSITION_FACE_UP = new Cartesian3dCoordinate(0.0, 0.0, 1.0);
		public static readonly Cartesian3dCoordinate POSITION_FACE_DOWN = new Cartesian3dCoordinate(0.0, 0.0, -1.0);
		public static readonly Cartesian3dCoordinate POSITION_FACE_RIGHT = new Cartesian3dCoordinate(0.0, 1.0, 0.0);
		public static readonly Cartesian3dCoordinate POSITION_FACE_LEFT = new Cartesian3dCoordinate(0.0, -1.0, 0.0);
		public static readonly Cartesian3dCoordinate POSITION_FACE_FRONT = new Cartesian3dCoordinate(1.0, 0.0, 0.0);
		public static readonly Cartesian3dCoordinate POSITION_FACE_BACK = new Cartesian3dCoordinate(-1.0, 0.0, 0.0);

		#endregion Const Members

		/// <summary>
		/// Create a new CubicRotationCore from blocks an axis collections.
		/// </summary>
		/// <param name="blocks">Collection of all the blocks forming the Cube.</param>
		/// <param name="axes">Collection of the ROtation axes around the Cube.</param>
		public CubicRotationCore(IEnumerable<BlockDefinition> blocks, IEnumerable<RotationAxis> axes)
			: base(blocks, axes, GenerateFaces()) { }

		#region Protected Members

		/// <summary>
		/// Add the 6 centers for a cube using center on their face axis.
		/// </summary>
		/// <param name="blocks">List of block to which created centers will be added.</param>
		protected static void AddCentersToList(IList<BlockDefinition> blocks)
		{
			blocks.Add(new BlockDefinition($"CF_{ID_FACE_DOWN}", POSITION_FACE_DOWN, new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN)));
			blocks.Add(new BlockDefinition($"CF_{ID_FACE_UP}", POSITION_FACE_UP, new BlockFace(ID_FACE_UP, POSITION_FACE_UP)));
			blocks.Add(new BlockDefinition($"CF_{ID_FACE_LEFT}", POSITION_FACE_LEFT, new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT)));
			blocks.Add(new BlockDefinition($"CF_{ID_FACE_RIGHT}", POSITION_FACE_RIGHT, new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT)));
			blocks.Add(new BlockDefinition($"CF_{ID_FACE_FRONT}", POSITION_FACE_FRONT, new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT)));
			blocks.Add(new BlockDefinition($"CF_{ID_FACE_BACK}", POSITION_FACE_BACK, new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK)));
		}

		/// <summary>
		/// Add blocks for the 8 corners of the cube.
		/// </summary>
		/// <param name="blocks">List of block to which created corners will be added.</param>
		protected static void AddCornersToList(IList<BlockDefinition> blocks)
		{
			// 4 bottoms corners.
			blocks.Add(new BlockDefinition(
				$"C{ID_FACE_DOWN}{ID_FACE_FRONT}{ID_FACE_RIGHT}",
				POSITION_CORNER_DOWN_FRONT_RIGHT,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT),
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT)
			));

			blocks.Add(new BlockDefinition(
				$"C{ID_FACE_DOWN}{ID_FACE_BACK}{ID_FACE_RIGHT}",
				POSITION_CORNER_DOWN_BACK_RIGHT,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK),
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT)
			));

			blocks.Add(new BlockDefinition(
				$"C{ID_FACE_DOWN}{ID_FACE_FRONT}{ID_FACE_LEFT}",
				POSITION_CORNER_DOWN_FRONT_LEFT,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT),
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT)
			));

			blocks.Add(new BlockDefinition(
				$"C{ID_FACE_DOWN}{ID_FACE_BACK}{ID_FACE_LEFT}",
				POSITION_CORNER_DOWN_BACK_LEFT,
				new BlockFace(ID_FACE_DOWN, POSITION_FACE_DOWN),
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK),
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT)
			));

			// 4 tops corners.
			blocks.Add(new BlockDefinition(
				$"C{ID_FACE_UP}{ID_FACE_FRONT}{ID_FACE_RIGHT}",
				POSITION_CORNER_UP_FRONT_RIGHT,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT),
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT)
			));

			blocks.Add(new BlockDefinition(
				$"C{ID_FACE_UP}{ID_FACE_BACK}{ID_FACE_RIGHT}",
				POSITION_CORNER_UP_BACK_RIGHT,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK),
				new BlockFace(ID_FACE_RIGHT, POSITION_FACE_RIGHT)
			));

			blocks.Add(new BlockDefinition(
				$"C{ID_FACE_UP}{ID_FACE_FRONT}{ID_FACE_LEFT}",
				POSITION_CORNER_UP_FRONT_LEFT,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_FRONT, POSITION_FACE_FRONT),
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT)
			));

			blocks.Add(new BlockDefinition(
				$"C{ID_FACE_UP}{ID_FACE_BACK}{ID_FACE_LEFT}",
				POSITION_CORNER_UP_BACK_LEFT,
				new BlockFace(ID_FACE_UP, POSITION_FACE_UP),
				new BlockFace(ID_FACE_BACK, POSITION_FACE_BACK),
				new BlockFace(ID_FACE_LEFT, POSITION_FACE_LEFT)
			));
		}

		#endregion Protected Members

		#region Private Members

		/// <summary>
		/// Generate the faces that will be represent the cube.
		/// </summary>
		/// <returns>The list of faces available on a cube.</returns>
		private static IEnumerable<CoreFace> GenerateFaces()
		{
			return [
				new CoreFace(ID_FACE_UP, new Plane(POSITION_FACE_UP, -1.0)),
				new CoreFace(ID_FACE_DOWN, new Plane(POSITION_FACE_DOWN, -1.0)),
				new CoreFace(ID_FACE_FRONT, new Plane(POSITION_FACE_FRONT, -1.0)),
				new CoreFace(ID_FACE_BACK, new Plane(POSITION_FACE_BACK, -1.0)),
				new CoreFace(ID_FACE_RIGHT, new Plane(POSITION_FACE_RIGHT, -1.0)),
				new CoreFace(ID_FACE_LEFT, new Plane(POSITION_FACE_LEFT, -1.0)),
			];
		}

		#endregion Private Members
	}
}
