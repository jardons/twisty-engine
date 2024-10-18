using System.IO;
using System.Text.Json;

namespace Twisty.Engine.Structure.Builders;

/// <summary>
/// RotationCoreFormat allowing to load data from a JSON file.
/// </summary>
/// <typeparam name="T">Type of the data contained in the JSON file.</typeparam>
public class RotationCoreFormatLoad<T> : IRotationCoreFormatEntry<T>
{
	/// <inheritdoc />
	public RotationCoreFormatEntryType Type => RotationCoreFormatEntryType.Load;

	/// <summary>
	/// Gets the Id of the format to load from a JSON file.
	/// </summary>
	public string Id { get; set; }

	/// <inheritdoc />
	public IEnumerable<T> GetValues(RotationCoreBuilderContext context)
	{
		string filePath = Path.Combine(context.DefinitionsPath, typeof(T).Name, Id + ".json");
		using StreamReader reader = new(filePath);

		return JsonSerializer.Deserialize<T[]>(reader.ReadToEnd(), context.SerializerOptions);
	}
}
