using System.Text.Json;
using System.Text.Json.Serialization;

namespace Twisty.Engine.Structure.Builders;

/// <summary>
/// JSON Converter for <see cref="IRotationCoreFormatEntry"/> object.
/// </summary>
public class RotationCoreFormatEntryConverter : JsonConverter<IRotationCoreFormatEntry>
{
	/// <inheritdoc />
	public override IRotationCoreFormatEntry Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using JsonDocument doc = JsonDocument.ParseValue(ref reader);

		JsonElement root = doc.RootElement;
		if (!root.TryGetProperty("Type", out JsonElement elementTypeJson))
			throw new JsonException($"{nameof(RotationCoreFormatEntryConverter)} cannot identify target class if 'Type' atribute is missing.");

		var typeValue = elementTypeJson.GetString();
		if (!Enum.TryParse<RotationCoreFormatEntryType>(typeValue, true, out RotationCoreFormatEntryType type))
			throw new JsonException($"{nameof(RotationCoreFormatEntryConverter)} was not able to find {nameof(IRotationCoreFormatEntry)} implementation for the Type '{typeValue}'.");

		return type switch
		{
			RotationCoreFormatEntryType.Load => JsonSerializer.Deserialize<RotationCoreFormatLoad>(root.GetRawText(), options),
			RotationCoreFormatEntryType.Union => JsonSerializer.Deserialize<RotationCoreFormatUnion>(root.GetRawText(), options),
			_ => throw new JsonException($"{nameof(RotationCoreFormatEntryConverter)} was not able to find {nameof(IRotationCoreFormatEntry)} implementation for the Type '{typeValue}'.")
		};
	}

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, IRotationCoreFormatEntry value, JsonSerializerOptions options)
		=> JsonSerializer.Serialize(writer, value, options);
}
