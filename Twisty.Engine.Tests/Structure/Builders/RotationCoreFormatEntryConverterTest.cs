using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Twisty.Engine.Structure.Builders;

namespace Twisty.Engine.Tests.Structure.Builders;

[Trait("Category", "Structure/Builders")]
public class RotationCoreFormatEntryConverterTest
{
	[Theory]
	[InlineData(typeof(RotationCoreFormatLoad))]
	[InlineData(typeof(RotationCoreFormatUnion))]
	public void JsonSerializeIRotationCoreFormat_Json(Type type)
	{
		// 1. Prepare
		var obj = Activator.CreateInstance(type);
		var options = GetOptions();

		// 2. Execute
		var json = JsonSerializer.Serialize(obj, options);

		// 3. Verify
		Assert.NotNull(json);
		// Only testing Serialize class redirection, JSON validity should be tested in specific class Unit Tests.
	}

	[Theory]
	[InlineData(RotationCoreFormatEntryType.Load, typeof(RotationCoreFormatLoad))]
	[InlineData(RotationCoreFormatEntryType.Union, typeof(RotationCoreFormatUnion))]
	public void JsonDeserializeRotationCoreFormat_Expected(RotationCoreFormatEntryType typeId, Type type)
	{
		// 1. Prepare
		// Use simpler JSON possible as we test correct class selection.
		string json = "{\"Type\":\"" + typeId.ToString() + "\"}";
		var options = GetOptions();

		// 2. Execute
		var obj = JsonSerializer.Deserialize<IRotationCoreFormatEntry>(json, options);

		// 3. Verify
		Assert.IsType(type, obj);
		// Only testing Deserialize class redirection, fields value should be tested in each specific class Unit Tests.
	}

	[Fact]
	public void JsonDeserializeInvalidRotationCoreFormat_JsonException()
	{
		// 1. Prepare
		string json = "{\"Type\":\"invalidType\"}";
		var options = GetOptions();
		object obj;

		// 2. Execute
		void a() => obj = JsonSerializer.Deserialize<IRotationCoreFormatEntry>(json, options);

		// 3. Verify
		Assert.Throws<JsonException>(a);
	}

	[Theory]
	[InlineData("None")]
	[InlineData("")]
	[InlineData("\t")]
	public void JsonDeserializeNoRotationCoreFormat_JsonException(string type)
	{
		// 1. Prepare
		string json = "{\"Type\":\"" + type + "\"}";
		var options = GetOptions();
		object obj;

		// 2. Execute
		void a() => obj = JsonSerializer.Deserialize<IRotationCoreFormatEntry>(json, options);

		// 3. Verify
		Assert.Throws<JsonException>(a);
	}

	[Fact]
	public void JsonDeserializeEmptyObject_JsonException()
	{
		// 1. Prepare
		string json = "{}";
		var options = GetOptions();
		object obj;

		// 2. Execute
		void a() => obj = JsonSerializer.Deserialize<IRotationCoreFormatEntry>(json, options);

		// 3. Verify
		Assert.Throws<JsonException>(a);
	}

	/// <summary>
	/// Gets the generic JSON serialization options to use in our tests.
	/// </summary>
	/// <returns>Generic JSON serialization options to use in our tests.</returns>
	private JsonSerializerOptions GetOptions()
	{
		return new JsonSerializerOptions
		{
			NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
			Converters = { new RotationCoreFormatEntryConverter() }
		};
	}
}
