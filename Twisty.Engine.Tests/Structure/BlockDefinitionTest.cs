using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Tests.DataFactories;

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
	public void CreateWithNullFace_ThrowArgumentNullException()
	{
		// 1. Prepare
		BlockDefinition b;
		BlockFace f = null;
		BlockFace[] faces = null;

		// 2. Execute
		void a1() => b = new("id", Cartesian3dCoordinate.XAxis, f);
		void a2() => b = new("id", Cartesian3dCoordinate.XAxis, faces);

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

		// 2. Execute
		void a() => b = new("id", Cartesian3dCoordinate.XAxis, faces);

		// 3. Verify
		Assert.Throws<ArgumentException>(a);
	}

	#endregion Test Methods
}
