using System;
using System.Linq;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Skewb;
using Twisty.Engine.Tests.Assertions;
using Xunit;

namespace Twisty.Engine.Tests.Structure.Skewb
{
	[Trait("Category", "Structure")]
	public class SkewbTest
	{
		private const int PRECISION_DOUBLE = 10;

		#region Test Methods

		[Theory]
		[InlineData(SkewbCube.ID_AXIS_DOWN_BACK_LEFT)]
		[InlineData(SkewbCube.ID_AXIS_DOWN_BACK_RIGHT)]
		[InlineData(SkewbCube.ID_AXIS_DOWN_FRONT_LEFT)]
		[InlineData(SkewbCube.ID_AXIS_DOWN_FRONT_RIGHT)]
		[InlineData(SkewbCube.ID_AXIS_UP_BACK_LEFT)]
		[InlineData(SkewbCube.ID_AXIS_UP_BACK_RIGHT)]
		[InlineData(SkewbCube.ID_AXIS_UP_FRONT_LEFT)]
		[InlineData(SkewbCube.ID_AXIS_UP_FRONT_RIGHT)]
		public void Skewb_Rotate_CenterStayInPlace(string axisId)
		{
			// 1. Prepare
			SkewbCube c = new SkewbCube();
			var axis = c.GetAxis(axisId);
			var center = c.Blocks.Where(b => b.Position.IsSameVector(axis.Vector)).First();
			var initialPosition = center.Position;

			// 2. Execute
			c.RotateAround(axis, true);

			// 3. Verify
			Assert.Equal(initialPosition.X, center.Position.X, PRECISION_DOUBLE);
			Assert.Equal(initialPosition.Y, center.Position.Y, PRECISION_DOUBLE);
			Assert.Equal(initialPosition.Z, center.Position.Z, PRECISION_DOUBLE);
		}

		[Theory]
		[InlineData(SkewbCube.ID_AXIS_UP_FRONT_LEFT, true)]
		[InlineData(SkewbCube.ID_AXIS_UP_FRONT_LEFT, false)]
		[InlineData(SkewbCube.ID_AXIS_UP_FRONT_RIGHT, true)]
		[InlineData(SkewbCube.ID_AXIS_UP_FRONT_RIGHT, false)]
		[InlineData(SkewbCube.ID_AXIS_UP_BACK_LEFT, true)]
		[InlineData(SkewbCube.ID_AXIS_UP_BACK_LEFT, false)]
		[InlineData(SkewbCube.ID_AXIS_UP_BACK_RIGHT, true)]
		[InlineData(SkewbCube.ID_AXIS_UP_BACK_RIGHT, false)]
		[InlineData(SkewbCube.ID_AXIS_DOWN_FRONT_LEFT, true)]
		[InlineData(SkewbCube.ID_AXIS_DOWN_FRONT_LEFT, false)]
		[InlineData(SkewbCube.ID_AXIS_DOWN_FRONT_RIGHT, true)]
		[InlineData(SkewbCube.ID_AXIS_DOWN_FRONT_RIGHT, false)]
		[InlineData(SkewbCube.ID_AXIS_DOWN_BACK_LEFT, true)]
		[InlineData(SkewbCube.ID_AXIS_DOWN_BACK_LEFT, false)]
		[InlineData(SkewbCube.ID_AXIS_DOWN_BACK_RIGHT, true)]
		[InlineData(SkewbCube.ID_AXIS_DOWN_BACK_RIGHT, false)]
		public void Skewb_RotateOnce_HasValidFaces(string axisId, bool isClockwise)
		{
			// 1. Prepare
			SkewbCube c = new SkewbCube();
			var axis = c.GetAxis(axisId);

			// 2. Execute
			c.RotateAround(axis, isClockwise);

			// 3. Verify
			foreach (CoreFace face in c.Faces)
				Assert.Equal(5, c.GetBlocksForFace(face.Id).Count());
			RotationCoreAssert.ExposeAllFaces(c);
		}

		[Fact]
		public void Skewb_Create_HasValidFaces()
		{
			// 1. Prepare

			// 2. Execute
			SkewbCube c = new SkewbCube();

			// 3. Verify
			foreach (CoreFace face in c.Faces)
				Assert.Equal(5, c.GetBlocksForFace(face.Id).Count());
			RotationCoreAssert.ExposeAllFaces(c);
		}

		#endregion Test Methods
	}
}
