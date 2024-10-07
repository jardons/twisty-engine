using System.Text.Json.Serialization;
using System.Text.Json;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Tests.Assertions;
using Twisty.Engine.Tests.DataFactories;
using System.Collections.Generic;

namespace Twisty.Engine.Tests.Structure;

[Trait("Category", "Structure")]
public class BlockDefinitionTest
{
	#region Test Methods

	[Fact]
	public void CreateWithNullId_ThrowArgumentNullException()
	{
		// 1. Prepare
		BlockDefinition b;
		var faces = BlockFacesFactory.GetFaces("X");

		// 2. Execute
		void a1() => b = new(null, Cartesian3dCoordinate.XAxis, faces[0]);
		void a2() => b = new(null, Cartesian3dCoordinate.XAxis, faces);

		// 3. Verify
		Assert.Throws<ArgumentNullException>(a1);
		Assert.Throws<ArgumentNullException>(a2);
	}

	[Theory]
	[InlineData("")]
	[InlineData("\t")]
	[InlineData(" ")]
	[InlineData("    ")]
	[InlineData("\n")]
	public void CreateWithEmptyId_ThrowArgumentNullException(string id)
	{
		// 1. Prepare
		BlockDefinition b;
		var faces = BlockFacesFactory.GetFaces("X");

		// 2. Execute
		void a1() => b = new(id, Cartesian3dCoordinate.XAxis, faces[0]);
		void a2() => b = new(id, Cartesian3dCoordinate.XAxis, faces);

		// 3. Verify
		Assert.Throws<ArgumentException>(a1);
		Assert.Throws<ArgumentException>(a2);
	}

	[Fact]
	public void CreateWithNullFace_ThrowArgumentException()
	{
		// 1. Prepare
		BlockDefinition b;
		BlockFace f = null;

		// 2. Execute
		void a() => b = new("id", Cartesian3dCoordinate.XAxis, f);

		// 3. Verify
		Assert.Throws<ArgumentException>(a);
	}

	[Fact]
	public void CreateWithNullFacesArray_ThrowArgumentNullException()
	{
		// 1. Prepare
		BlockDefinition b;
		BlockFace[] faces = null;
		IReadOnlyCollection<BlockFace> readOnlyFaces = null;

		// 2. Execute
		void a1() => b = new("id", Cartesian3dCoordinate.XAxis, faces);
		void a2() => b = new("id", Cartesian3dCoordinate.XAxis, readOnlyFaces);

		// 3. Verify
		Assert.Throws<ArgumentNullException>(a1);
		Assert.Throws<ArgumentNullException>(a2);
	}

	[Fact]
	public void CreateWithEmptyFaces_ThrowArgumentException()
	{
		// 1. Prepare
		BlockDefinition b;
		BlockFace[] faces = [];
		IReadOnlyCollection<BlockFace> readOnlyFaces = [];

		// 2. Execute
		void a1() => b = new("id", Cartesian3dCoordinate.XAxis, faces);
		void a2() => b = new("id", Cartesian3dCoordinate.XAxis, readOnlyFaces);

		// 3. Verify
		Assert.Throws<ArgumentException>(a1);
		Assert.Throws<ArgumentException>(a2);
	}


	[Fact]
	public void JsonSerialize_Expected()
	{
		// 1. Prepare
		BlockDefinition definition = new("test_id", new(1.0, 2.0, 3.0), new BlockFace("face_id", new Cartesian3dCoordinate(4.0, 5.0, 6.0)));

		var options = new JsonSerializerOptions
		{
			NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
		};

		// 2. Execute
		var json = JsonSerializer.Serialize(definition, options);

		// 3. Verify
		Assert.Equal("{\"InitialPosition\":{\"X\":1,\"Y\":2,\"Z\":3},\"Id\":\"test_id\",\"Faces\":[{\"Position\":{\"X\":4,\"Y\":5,\"Z\":6},\"Id\":\"face_id\"}]}", json);
	}

	[Theory]
	[InlineData("{\"InitialPosition\":{\"X\":1,\"Y\":2,\"Z\":3},\"Id\":\"test_id\",\"Faces\":[{\"Position\":{\"X\":4,\"Y\":5,\"Z\":6},\"Id\":\"face_id\"}]}", 1)]
	[InlineData("{\"InitialPosition\":{\"X\":1,\"Y\":2,\"Z\":3},\"Id\":\"test_id\",\"Faces\":[{\"Position\":{\"X\":4,\"Y\":5,\"Z\":6},\"Id\":\"face_id\"},{\"Position\":{\"X\":7,\"Y\":8,\"Z\":9},\"Id\":\"face_id\"}]}", 2)]
	public void JsonDeserialize_Expected(string json, int expectedFacesCount)
	{
		// 1. Prepare
		// Nothing to prepare.

		// 2. Execute
		var definition = JsonSerializer.Deserialize<BlockDefinition>(json);

		// 3. Verify
		Assert.NotNull(definition);
		Assert.NotNull(definition.Id);
		Assert.Equal(expectedFacesCount, definition.Faces.Count);
	}

	#endregion Test Methods
}

