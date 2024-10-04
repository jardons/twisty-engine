using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Twisty.Engine.Structure.Builders;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Tests.Structure.Builders;

[Trait("Category", "Structure/Builders")]
public class RotationCoreFormatUnionTest
{
	[Fact]
	public void GetValuesEmpty_Empty()
	{
		// 1. Prepare
		var context = GetContext();

		var union = new RotationCoreFormatUnion<int> { Entries = [] };

		// 2. Execute
		var values = union.GetValues(context);

		// 3. Verify
		Assert.NotNull(values);
		Assert.Empty(values);
	}

	[Fact]
	public void GetValuesNull_Empty()
	{
		// 1. Prepare
		var context = GetContext();

		var union = new RotationCoreFormatUnion<int> { Entries = null };

		// 2. Execute
		var values = union.GetValues(context);

		// 3. Verify
		Assert.NotNull(values);
		Assert.Empty(values);
	}

	[Fact]
	public void GetSingleSourceValues_Expected()
	{
		// 1. Prepare
		var context = GetContext();

		int[] list = [1, 2, 3, 5];
		var mock = new Mock<IRotationCoreFormatEntry<int>>();
		mock.Setup(o => o.GetValues(context)).Returns(list);

		var union = new RotationCoreFormatUnion<int> { Entries = [mock.Object] };

		// 2. Execute
		var values = union.GetValues(context);

		// 3. Verify
		Assert.NotNull(values);
		Assert.Equivalent(list, values, true);
		mock.Verify(m => m.GetValues(context), Times.Once);
	}

	[Fact]
	public void GetDoubleValues_Expected()
	{
		// 1. Prepare
		var context = GetContext();

		int[] list1 = [1, 2, 3, 5];
		var mock1 = new Mock<IRotationCoreFormatEntry<int>>();
		mock1.Setup(o => o.GetValues(context)).Returns(list1);

		int[] list2 = [4, 9];
		var mock2 = new Mock<IRotationCoreFormatEntry<int>>();
		mock2.Setup(o => o.GetValues(context)).Returns(list2);

		var expected = list1.Concat(list2);

		var union = new RotationCoreFormatUnion<int> { Entries = [mock1.Object, mock2.Object] };

		// 2. Execute
		var values = union.GetValues(context);

		// 3. Verify
		Assert.NotNull(values);
		Assert.Equivalent(expected, values, true);
		mock1.Verify(m => m.GetValues(context), Times.Once);
		mock2.Verify(m => m.GetValues(context), Times.Once);
	}

	[Fact]
	public void GetTripleValues_Expected()
	{
		// 1. Prepare
		var context = GetContext();

		int[] list1 = [1, 2, 3, 5];
		var mock1 = new Mock<IRotationCoreFormatEntry<int>>();
		mock1.Setup(o => o.GetValues(context)).Returns(list1);

		int[] list2 = [4, 9];
		var mock2 = new Mock<IRotationCoreFormatEntry<int>>();
		mock2.Setup(o => o.GetValues(context)).Returns(list2);

		int[] list3 = [12, 15];
		var mock3 = new Mock<IRotationCoreFormatEntry<int>>();
		mock3.Setup(o => o.GetValues(context)).Returns(list3);

		var expected = list1.Concat(list2).Concat(list3);

		var union = new RotationCoreFormatUnion<int> { Entries = [mock1.Object, mock2.Object, mock3.Object] };

		// 2. Execute
		var values = union.GetValues(context);

		// 3. Verify
		Assert.NotNull(values);
		Assert.Equivalent(expected, values, true);
		mock1.Verify(m => m.GetValues(context), Times.Once);
		mock2.Verify(m => m.GetValues(context), Times.Once);
		mock3.Verify(m => m.GetValues(context), Times.Once);
	}

	private RotationCoreBuilderContext GetContext()
		=> new RotationCoreBuilderContext { DefinitionsPath = "path" };
}
