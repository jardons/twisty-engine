namespace Twisty.Engine.Structure.Skewb;

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

	#region Private Members

	/// <summary>
	/// Generate the axes that will be available for the rotation of the cube.
	/// </summary>
	/// <returns>The list of axis available on a Skewb cube.</returns>
	private static IEnumerable<RotationAxis> GenerateAxes()
	{
		return [
			new RotationAxis(ID_AXIS_UP_FRONT_LEFT, POSITION_CORNER_UP_FRONT_LEFT),
			new RotationAxis(ID_AXIS_UP_FRONT_RIGHT, POSITION_CORNER_UP_FRONT_RIGHT),
			new RotationAxis(ID_AXIS_UP_BACK_LEFT, POSITION_CORNER_UP_BACK_LEFT),
			new RotationAxis(ID_AXIS_UP_BACK_RIGHT, POSITION_CORNER_UP_BACK_RIGHT),
			new RotationAxis(ID_AXIS_DOWN_FRONT_LEFT, POSITION_CORNER_DOWN_FRONT_LEFT),
			new RotationAxis(ID_AXIS_DOWN_FRONT_RIGHT, POSITION_CORNER_DOWN_FRONT_RIGHT),
			new RotationAxis(ID_AXIS_DOWN_BACK_LEFT, POSITION_CORNER_DOWN_BACK_LEFT),
			new RotationAxis(ID_AXIS_DOWN_BACK_RIGHT, POSITION_CORNER_DOWN_BACK_RIGHT),
		];
	}

	/// <summary>
	/// Generate the blocks that will form the current Skewb Cube.
	/// </summary>
	/// <param name="n">Indicate the number of rows per face of the cube that is currently generated.</param>
	/// <returns>The list of block that represent the current cube.</returns>
	private static List<BlockDefinition> GenerateBlocks()
	{
		List<BlockDefinition> blocks = [];

		CubicRotationCore.AddCornersToList(blocks);
		CubicRotationCore.AddCentersToList(blocks);

		return blocks;
	}

	#endregion Private Members
}
