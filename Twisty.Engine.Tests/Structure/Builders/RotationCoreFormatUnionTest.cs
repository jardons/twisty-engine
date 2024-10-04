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
public class RotationCoreFormatUnionTest
{
	[Fact]
	public void GetValuesEmpty_Empty()
	{
		// 1. Prepare
		var union = new RotationCoreFormatUnion<int> { Entries = [] };

		// 2. Execute
		var values = union.GetValues();

		// 3. Verify
		Assert.NotNull(values);
		Assert.Empty(values);
	}

	[Fact]
	public void GetValuesNull_Empty()
	{
		// 1. Prepare
		var union = new RotationCoreFormatUnion<int> { Entries = null };

		// 2. Execute
		var values = union.GetValues();

		// 3. Verify
		Assert.NotNull(values);
		Assert.Empty(values);
	}

	[Fact]
	public void GetSingleSourceValues_Expected()
	{
		// 1. Prepare
		int[] list = [1, 2, 3, 5];
		var mock = new Mock<IRotationCoreFormatEntry<int>>();
		mock.Setup(o => o.GetValues()).Returns(list);

		var union = new RotationCoreFormatUnion<int> { Entries = [mock.Object] };

		// 2. Execute
		var values = union.GetValues();

		// 3. Verify
		Assert.NotNull(values);
		Assert.Equivalent(list, values, true);
	}

	[Fact]
	public void GetDoubleValues_Expected()
	{
		// 1. Prepare
		int[] list1 = [1, 2, 3, 5];
		var mock1 = new Mock<IRotationCoreFormatEntry<int>>();
		mock1.Setup(o => o.GetValues()).Returns(list1);

		int[] list2 = [4, 9];
		var mock2 = new Mock<IRotationCoreFormatEntry<int>>();
		mock2.Setup(o => o.GetValues()).Returns(list2);

		var expected = list1.Concat(list2);

		var union = new RotationCoreFormatUnion<int> { Entries = [mock1.Object, mock2.Object] };

		// 2. Execute
		var values = union.GetValues();

		// 3. Verify
		Assert.NotNull(values);
		Assert.Equivalent(expected, values, true);
	}

	[Fact]
	public void GetTripleValues_Expected()
	{
		// 1. Prepare
		int[] list1 = [1, 2, 3, 5];
		var mock1 = new Mock<IRotationCoreFormatEntry<int>>();
		mock1.Setup(o => o.GetValues()).Returns(list1);

		int[] list2 = [4, 9];
		var mock2 = new Mock<IRotationCoreFormatEntry<int>>();
		mock2.Setup(o => o.GetValues()).Returns(list2);

		int[] list3 = [12, 15];
		var mock3 = new Mock<IRotationCoreFormatEntry<int>>();
		mock3.Setup(o => o.GetValues()).Returns(list3);

		var expected = list1.Concat(list2).Concat(list3);

		var union = new RotationCoreFormatUnion<int> { Entries = [mock1.Object, mock2.Object, mock3.Object] };

		// 2. Execute
		var values = union.GetValues();

		// 3. Verify
		Assert.NotNull(values);
		Assert.Equivalent(expected, values, true);
	}
}
