using System;
using System.Linq;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Rubiks;
using Twisty.Engine.Tests.Assertions;
using Xunit;

namespace Twisty.Engine.Tests.Structure.Rubiks
{
	[Trait("Category", "Structure")]
	public class RubikCubeTest
	{
		#region Test Methods

		[Theory]
		[InlineData(RubikCube.ID_FACE_UP, true, "(1 -1 1)", RubikCube.ID_FACE_FRONT, RubikCube.ID_FACE_RIGHT)]
		[InlineData(RubikCube.ID_FACE_UP, false, "(1 -1 1)", RubikCube.ID_FACE_FRONT, RubikCube.ID_FACE_LEFT)]
		public void RubikCube_RotateOnceOnSize2_FindExpectedFace(string axisId, bool isClockwise, string blockCoordinate, string checkedFaceId, string expectedBlockFace)
		{
			// 1. Prepare
			Cartesian3dCoordinate blockPosition = new Cartesian3dCoordinate(blockCoordinate);
			RubikCube c = new RubikCube(2);
			var axis = c.GetAxis(axisId);
			var initialBlock = c.Blocks.FirstOrDefault(b => b.Position.IsSameVector(blockPosition));
			var targetFaceVector = c.GetFace(checkedFaceId).Plane.Normal;

			// 2. Execute
			c.RotateAround(axis, isClockwise);
			var block = c.Blocks.FirstOrDefault(b => b.Position.IsSameVector(blockPosition));
			var face = block.GetBlockFace(targetFaceVector);

			// 3. Verify
			Assert.NotEqual(initialBlock.Id, block.Id);
			Assert.NotNull(face);
			Assert.Equal(expectedBlockFace, face.Id);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(Int32.MinValue)]
		public void RubikCube_CreateInvalidSize_ThrowArgumentException(int size)
		{
			// 1. Prepare
			RubikCube c;

			// 2. Execute
			void a() => c = new RubikCube(size);

			// 3. Verify
			Assert.Throws<ArgumentException>(a);
		}

		[Theory]
		[InlineData(2, 8)]
		[InlineData(3, 26)]
		public void RubikCube_CreateAndCountBlocks_ShouldMatch(int size, int blocksCount)
		{
			// 1. Prepare
			// Nothing

			// 2. Execute
			RubikCube c = new RubikCube(size);
			int count = c.Blocks.Count();

			// 3. Verify
			Assert.Equal(blocksCount, count);
		}

		[Theory]
		[InlineData(2, 4)]
		[InlineData(3, 9)]
		public void RubikCube_CreateAndCountBlocksPerFace_ShouldMatch(int size, int blocksCount)
		{
			// 1. Prepare
			int i = 0;
			int[] results = new int[6];

			// 2. Execute
			RubikCube c = new RubikCube(size);
			foreach (RotationAxis axis in c.Axes)
				results[i++] = c.GetBlocksForFace(axis.Vector).Count();

			// 3. Verify
			for (i = 0; i < results.Length; ++i)
				Assert.Equal(blocksCount, results[i]);
		}

		[Theory]
		[InlineData(RubikCube.ID_FACE_BACK)]
		[InlineData(RubikCube.ID_FACE_FRONT)]
		[InlineData(RubikCube.ID_FACE_LEFT)]
		[InlineData(RubikCube.ID_FACE_RIGHT)]
		[InlineData(RubikCube.ID_FACE_UP)]
		[InlineData(RubikCube.ID_FACE_DOWN)]
		public void RubikCube_Rotate_CenterStayInPlace(string axisId)
		{
			// 1. Prepare
			RubikCube c = new RubikCube(3);
			var axis = c.GetAxis(axisId);
			var center = c.Blocks.Where(b => b.Position.IsSameVector(axis.Vector)).First();
			var initialPosition = center.Position;

			// 2. Execute
			c.RotateAround(axis, true);

			// 3. Verify
			Assert.Equal(initialPosition.X, center.Position.X, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(initialPosition.Y, center.Position.Y, GeometryAssert.PRECISION_DOUBLE);
			Assert.Equal(initialPosition.Z, center.Position.Z, GeometryAssert.PRECISION_DOUBLE);
		}

		#endregion Test Methods
	}
}
