using System.Text.Json;

namespace Twisty.Engine.Structure.Builders;

/// <summary>
/// Context class used when interacting with the RotationCoreBuilder.
/// </summary>
public class RotationCoreBuilderContext
{
	/// <summary>
	/// Gets the path to the definition files folder.
	/// </summary>
	public string DefinitionsPath { get; init; }

	/// <summary>
	/// Gets the JSON Serialization options.
	/// </summary>
	public JsonSerializerOptions SerializerOptions { get; init; }
}
