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
	public void JsonSerializeWithNoLayout_Expected()
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

	[Theory]
	[InlineData("{\"Id\":\"my_id\",\"Vector\":{\"X\":1,\"Y\":1,\"Z\":1},\"Layers\":[{\"Id\":\"L_my_id\",\"Plane\":{\"Normal\":{\"X\":1,\"Y\":1,\"Z\":1},\"D\":0}}]}", "my_id")]
	[InlineData("{\"Id\":\"my_id\",\"Vector\":{\"X\":1,\"Y\":1,\"Z\":1}}", "my_id")]
	public void JsonDeserializeWithNoLayout_Expected(string json, string expectedId)
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

	#endregion Test Methods
}