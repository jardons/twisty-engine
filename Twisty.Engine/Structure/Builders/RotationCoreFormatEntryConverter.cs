using System.Text.Json;
using System.Text.Json.Serialization;

namespace Twisty.Engine.Structure.Builders;

/// <summary>
/// JSON Converter for <see cref="IRotationCoreFormatEntry"/> object.
/// </summary>
public class RotationCoreFormatEntryConverter<T> : JsonConverter<IRotationCoreFormatEntry<T>>
{
	/// <inheritdoc />
	public override IRotationCoreFormatEntry<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		using JsonDocument doc = JsonDocument.ParseValue(ref reader);

		JsonElement root = doc.RootElement;
		if (!root.TryGetProperty("Type", out JsonElement elementTypeJson))
			throw new JsonException($"{nameof(RotationCoreFormatEntryConverter<T>)} cannot identify target class if 'Type' atribute is missing.");

		var typeValue = elementTypeJson.GetString();
		if (!Enum.TryParse<RotationCoreFormatEntryType>(typeValue, true, out RotationCoreFormatEntryType type))
			throw new JsonException($"{nameof(RotationCoreFormatEntryConverter<T>)} was not able to find {nameof(IRotationCoreFormatEntry<T>)} implementation for the Type '{typeValue}'.");

		return type switch
		{
			RotationCoreFormatEntryType.Load => JsonSerializer.Deserialize<RotationCoreFormatLoad<T>>(root.GetRawText(), options),
			RotationCoreFormatEntryType.Union => JsonSerializer.Deserialize<RotationCoreFormatUnion<T>>(root.GetRawText(), options),
			_ => throw new JsonException($"{nameof(RotationCoreFormatEntryConverter<T>)} was not able to find {nameof(IRotationCoreFormatEntry<T>)} implementation for the Type '{typeValue}'.")
		};
	}

	/// <inheritdoc />
	public override void Write(Utf8JsonWriter writer, IRotationCoreFormatEntry<T> value, JsonSerializerOptions options)
		=> JsonSerializer.Serialize(writer, value, options);
}
