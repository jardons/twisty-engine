using System;
using System.Linq;
using System.Collections.Generic;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using System.Text.Json.Serialization;
using System.Text.Json;
using Twisty.Engine.Tests.Assertions;

namespace Twisty.Engine.Tests.Structure;

[Trait("Category", "Structure")]
public class RotationAxisTest
{
	#region Test Methods

	[Fact]
	public void JsonSerializeWithNoLayer_Expected()
	{
		// 1. Prepare
		RotationAxis axis = new("my_id", new(1, 1, 1));

		var options = new JsonSerializerOptions
		{
			NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
		};

		// 2. Execute
		var json = JsonSerializer.Serialize(axis, options);

		// 3. Verify
		Assert.NotNull(json);
		Assert.Equal("{\"Id\":\"my_id\",\"Vector\":{\"X\":1,\"Y\":1,\"Z\":1},\"Layers\":[{\"Id\":\"L_my_id\",\"Plane\":{\"Normal\":{\"X\":1,\"Y\":1,\"Z\":1},\"D\":0}}]}", json);
	}

	[Fact]
	public void JsonSerializeWithOneLayer_Expected()
	{
		// 1. Prepare
		RotationAxis axis = new("my_id", new(1, 1, 1), [new LayerSeparator("test_layer", new Plane(0, 0, 1, 0.5))]);

		var options = new JsonSerializerOptions
		{
			NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
		};

		// 2. Execute
		var json = JsonSerializer.Serialize(axis, options);

		// 3. Verify
		Assert.NotNull(json);
		Assert.Equal("{\"Id\":\"my_id\",\"Vector\":{\"X\":1,\"Y\":1,\"Z\":1},\"Layers\":[{\"Id\":\"test_layer\",\"Plane\":{\"Normal\":{\"X\":0,\"Y\":0,\"Z\":1},\"D\":0.5}}]}", json);
	}

	[Theory]
	[InlineData("{\"Id\":\"my_id\",\"Vector\":{\"X\":1,\"Y\":1,\"Z\":1},\"Layers\":[{\"Id\":\"L_my_id\",\"Plane\":{\"Normal\":{\"X\":1,\"Y\":1,\"Z\":1},\"D\":0}}]}", "my_id")]
	[InlineData("{\"Id\":\"my_id\",\"Vector\":{\"X\":1,\"Y\":1,\"Z\":1}}", "my_id")]
	public void JsonDeserializeWithNoLayer_Expected(string json, string expectedId)
	{
		// 1. Prepare
		// Nothing to prepare.	

		// 2. Execute
		var axis = JsonSerializer.Deserialize<RotationAxis>(json);

		// 3. Verify
		Assert.NotNull(axis);
		Assert.Equal(expectedId, axis.Id);
		Assert.NotNull(axis.Layers);
		Assert.Single(axis.Layers);
		Assert.Equal(0.0, axis.Layers.First().Plane.D, GeometryAssert.PRECISION_DOUBLE);
	}

	[Theory]
	[InlineData("{\"Id\":\"my_id\",\"Vector\":{\"X\":1,\"Y\":1,\"Z\":1},\"Layers\":[{\"Id\":\"test_layer\",\"Plane\":{\"Normal\":{\"X\":0,\"Y\":0,\"Z\":1},\"D\":0.5}}]}", "my_id")]
	public void JsonDeserializeWithOneLayer_Expected(string json, string expectedId)
	{
		// 1. Prepare
		// Nothing to prepare.	

		// 2. Execute
		var axis = JsonSerializer.Deserialize<RotationAxis>(json);

		// 3. Verify
		Assert.NotNull(axis);
		Assert.Equal(expectedId, axis.Id);
		Assert.NotNull(axis.Layers);
		Assert.Single(axis.Layers);
	}

	#endregion Test Methods
}