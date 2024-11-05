using System.Text.Json.Serialization;
using System.Text.Json;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Tests.DataFactories;
using Twisty.Engine.Tests.Assertions;

namespace Twisty.Engine.Tests.Structure;

[Trait("Category", "Structure")]
public class SateliteDefinitionTest
{
	#region Test Class

	/// <summary>
	/// Use abstract class with minimal implementation for testing.
	/// </summary>
	private class TestSateliteDefinition : SateliteDefinition
	{
		[JsonConstructor]
		public TestSateliteDefinition(string id, Cartesian3dCoordinate initialPosition)
			: base(id, initialPosition)
		{
		}
	}

	#endregion Test Class

	#region Test Methods

	[Fact]
	public void CreateWithNullId_ThrowArgumentNullException()
	{
		// 1. Prepare
		TestSateliteDefinition b;

		// 2. Execute
		void a() => b = new(null, Cartesian3dCoordinate.XAxis);

		// 3. Verify
		Assert.Throws<ArgumentNullException>(a);
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
		TestSateliteDefinition b;

		// 2. Execute
		void a() => b = new(id, Cartesian3dCoordinate.XAxis);

		// 3. Verify
		Assert.Throws<ArgumentException>(a);
	}

	[Fact]
	public void JsonSerialize_Expected()
	{
		// 1. Prepare
		TestSateliteDefinition definition = new("test_id", new(1.0, 2.0, 3.0));

		var options = new JsonSerializerOptions
		{
			NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
		};

		// 2. Execute
		var json = JsonSerializer.Serialize(definition, options);

		// 3. Verify
		Assert.Equal("{\"InitialPosition\":{\"X\":1,\"Y\":2,\"Z\":3},\"Id\":\"test_id\"}", json);
	}

	[Theory]
	[InlineData("{\"InitialPosition\":{\"X\":1,\"Y\":2,\"Z\":3},\"Id\":\"test_id\"}", "test_id", "(1, 2, 3)")]
	[InlineData("{\"Id\":\"test_id\",\"InitialPosition\":{\"X\":1,\"Y\":2,\"Z\":3}}", "test_id", "(1, 2, 3)")]
	public void JsonDeserialize_Expected(string json, string expectedId, string expectedCoodrinatesString)
	{
		// 1. Prepare
		var expectedCoordinates = new Cartesian3dCoordinate(expectedCoodrinatesString);

		// 2. Execute
		var definition = JsonSerializer.Deserialize<TestSateliteDefinition>(json);

		// 3. Verify
		Assert.NotNull(definition);
		Assert.NotNull(definition.Id);
		Assert.Equal(expectedId, definition.Id);
		GeometryAssert.SamePoint(expectedCoordinates, definition.InitialPosition);
	}

	#endregion Test Methods
}

