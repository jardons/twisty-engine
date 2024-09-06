using System.Drawing;
using Twisty.Engine.Geometry;
using Twisty.Engine.Materialization;
using Twisty.Engine.Tests.Assertions;

namespace Twisty.Engine.Tests.Operations.Rubiks;

[Trait("Category", "Materialization")]
public class MaterializedObjectShapeTest
{
	#region Test Methods

	[Theory]
	// Merge 2 triangles.
	[InlineData("triangle_left_1", "triangle_right_1")]
	[InlineData("triangle_left_2", "triangle_right_1")]
	[InlineData("triangle_left_3", "triangle_right_1")]
	[InlineData("triangle_left_1", "triangle_right_2")]
	[InlineData("triangle_left_2", "triangle_right_2")]
	[InlineData("triangle_left_3", "triangle_right_2")]
	[InlineData("triangle_left_1", "triangle_right_3")]
	[InlineData("triangle_left_2", "triangle_right_3")]
	[InlineData("triangle_left_3", "triangle_right_3")]
	[InlineData("triangle_left_1", "rect_right_1")]
	[InlineData("triangle_left_2", "rect_right_1")]
	[InlineData("triangle_left_3", "rect_right_1")]
	[InlineData("triangle_left_1", "rect_right_2")]
	[InlineData("triangle_left_2", "rect_right_2")]
	[InlineData("triangle_left_3", "rect_right_2")]
	[InlineData("triangle_left_1", "rect_right_3")]
	[InlineData("triangle_left_2", "rect_right_3")]
	[InlineData("triangle_left_3", "rect_right_3")]
	[InlineData("triangle_left_1", "rect_right_4")]
	[InlineData("triangle_left_2", "rect_right_4")]
	[InlineData("triangle_left_3", "rect_right_4")]
	public void MergeOnOneSide_Expected(string firstPartId, string secondPartId)
	{
		// 1. Prepare
		var part1 = GetPart(firstPartId);
		var part2 = GetPart(secondPartId);

		// 2. Execute
		var r = MaterializedObjectShape.Merge(part1, part2);

		// 3. Verify
		Assert.NotNull(r);
		Assert.Equal(part1.Color, r.Color);
		Assert.Equal(part1.Points.Count() + part2.Points.Count() - 2, r.Points.Count());
	}

	[Fact]
	public void MergeSquareToSquare_Expected()
	{
		// 1. Prepare
		var complexShape = new MaterializedObjectShape(Color.White, [
			new Cartesian3dCoordinate(2, 2, 0), new Cartesian3dCoordinate(2, -1, 0),
			new Cartesian3dCoordinate(1, -1, 0), new Cartesian3dCoordinate(1, 1, 0),
			new Cartesian3dCoordinate(-1, 1, 0), new Cartesian3dCoordinate(-1, 2, 0)
		]);
		var smallSquare = new MaterializedObjectShape(Color.White, [
			new Cartesian3dCoordinate(1, 1, 0),
			new Cartesian3dCoordinate(1, -1, 0),
			new Cartesian3dCoordinate(-1, -1, 0),
			new Cartesian3dCoordinate(-1, 1, 0),
		]);

		// 2. Execute
		var r = MaterializedObjectShape.Merge(complexShape, smallSquare);

		// 3. Verify
		Assert.NotNull(r);
		Assert.Equal(complexShape.Color, r.Color);
		Assert.Equal(6, r.Points.Count());
	}

	#endregion Test Methods

	private static MaterializedObjectShape GetPart(string id)
	{
		return id switch
		{
			// Triangle oriented left with 3 possible ordering.
			"triangle_left_1" => new MaterializedObjectShape(Color.White, [new Cartesian3dCoordinate(1, 1, 0), new Cartesian3dCoordinate(1, -1, 0), new Cartesian3dCoordinate(0, 1, 0)]),
			"triangle_left_2" => new MaterializedObjectShape(Color.White, [new Cartesian3dCoordinate(0, 1, 0), new Cartesian3dCoordinate(1, 1, 0), new Cartesian3dCoordinate(1, -1, 0)]),
			"triangle_left_3" => new MaterializedObjectShape(Color.White, [new Cartesian3dCoordinate(1, -1, 0), new Cartesian3dCoordinate(0, 1, 0), new Cartesian3dCoordinate(1, 1, 0)]),
			// Triangle oriented right with 3 possible ordering.
			"triangle_right_1" => new MaterializedObjectShape(Color.White, [new Cartesian3dCoordinate(1, -1, 0), new Cartesian3dCoordinate(1, 1, 0), new Cartesian3dCoordinate(2, 1, 0)]),
			"triangle_right_2" => new MaterializedObjectShape(Color.White, [new Cartesian3dCoordinate(2, 1, 0), new Cartesian3dCoordinate(1, -1, 0), new Cartesian3dCoordinate(1, 1, 0)]),
			"triangle_right_3" => new MaterializedObjectShape(Color.White, [new Cartesian3dCoordinate(1, 1, 0), new Cartesian3dCoordinate(2, 1, 0), new Cartesian3dCoordinate(1, -1, 0)]),
			// Rectangle oriented right with 4 possible ordering.
			"rect_right_1" => new MaterializedObjectShape(Color.White, [new Cartesian3dCoordinate(1, -1, 0), new Cartesian3dCoordinate(1, 1, 0), new Cartesian3dCoordinate(2, 1, 0), new Cartesian3dCoordinate(2, -1, 0)]),
			"rect_right_2" => new MaterializedObjectShape(Color.White, [new Cartesian3dCoordinate(2, -1, 0), new Cartesian3dCoordinate(1, -1, 0), new Cartesian3dCoordinate(1, 1, 0), new Cartesian3dCoordinate(2, 1, 0)]),
			"rect_right_3" => new MaterializedObjectShape(Color.White, [new Cartesian3dCoordinate(2, 1, 0), new Cartesian3dCoordinate(2, -1, 0), new Cartesian3dCoordinate(1, -1, 0), new Cartesian3dCoordinate(1, 1, 0)]),
			"rect_right_4" => new MaterializedObjectShape(Color.White, [new Cartesian3dCoordinate(1, 1, 0), new Cartesian3dCoordinate(2, 1, 0), new Cartesian3dCoordinate(2, -1, 0), new Cartesian3dCoordinate(1, -1, 0)]),
			_ => throw new NotImplementedException()
		};
	}
}
