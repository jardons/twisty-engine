using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Tests.DataFactories;
using Twisty.Engine.Structure.Topology;
using Twisty.Engine.Structure.Rubiks;

namespace Twisty.Engine.Tests.Structure;

[Trait("Category", "Structure/Topology")]
public class TopologicContextTest
{
	#region Test Methods

	[Fact]
	public void CreateEmpty_IsEmpty()
	{
		// 1. Prepare

		// 2. Execute
		var context = new TopologicContext([]);

		// 3. Verify
		Assert.NotNull(context.TopologicIds);
		Assert.Empty(context.TopologicIds);
	}

	[Theory]
	[InlineData("a", 0)]
	[InlineData("b", 1)]
	[InlineData("c", 2)]
	[InlineData("d", 3)]
	public void GetTopologicIdIndexInSortedRange_GetIndex(string key, int expected)
	{
		// 1. Prepare
		string[] availableKeys = ["a", "b", "c", "d"];
		var context = new TopologicContext(availableKeys);

		// 2. Execute
		int i = context.GetTopologicIdIndex(key);

		// 3. Verify
		Assert.NotNull(context.TopologicIds);
		Assert.Equal(expected, i);
		Assert.Equal(availableKeys.Length, context.TopologicIds.Count);
	}

	[Theory]
	[InlineData("a", 0)]
	[InlineData("b", 1)]
	[InlineData("c", 2)]
	[InlineData("d", 3)]
	public void GetTopologicIdIndexInUnsortedRange_GetIndex(string key, int expected)
	{
		// 1. Prepare
		string[] availableKeys = ["d", "b", "a", "c"];
		var context = new TopologicContext(availableKeys);

		// 2. Execute
		int i = context.GetTopologicIdIndex(key);

		// 3. Verify
		Assert.NotNull(context.TopologicIds);
		Assert.Equal(expected, i);
		Assert.Equal(availableKeys.Length, context.TopologicIds.Count);
	}

	[Fact]
	public void GetTopologicIdIndexOutOfRange_ThrowsKeyNotFoundException()
	{
		// 1. Prepare
		string[] availableKeys = ["d", "b", "a", "c"];
		var context = new TopologicContext(availableKeys);

		// 2. Execute
		Action a = () => context.GetTopologicIdIndex("not there");

		// 3. Verify
		Assert.Throws<KeyNotFoundException>(a);
	}

	#endregion Test Methods
}