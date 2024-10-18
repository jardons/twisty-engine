namespace Twisty.Engine.Structure.Builders;

/// <summary>
/// CLass describing how to create a RotationCore from definition files.
/// </summary>
public class RotationCoreFormat
{
	/// <summary>
	/// Gets the CoreFace provider that will generate CoreFace for the RotationCore.
	/// </summary>
	public IRotationCoreFormatEntry<CoreFace> Faces { get; set; }

	/// <summary>
	/// Gets the RotationAxis provider that will generate RotationAxis for the RotationCore.
	/// </summary>
	public IRotationCoreFormatEntry<RotationAxis> Axes { get; set; }

	/// <summary>
	/// Gets the BlockDefinition provider that will generate BlockDefinition for the RotationCore.
	/// </summary>
	public IRotationCoreFormatEntry<BlockDefinition> Blocks { get; set; }
}
