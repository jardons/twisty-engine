using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure.Rubiks;
using Xunit;

namespace Twisty.Engine.Tests.Structure.Rubiks
{
	[Trait("Category", "Structure")]
	public class RubikCubeTest
	{
		#region Test Data

		public static readonly TheoryData<string, SphericalVector> FacesIds = new TheoryData<string, SphericalVector>()
		{
			{ RubikCube.FACE_ID_BACK, RubikCube.FACE_POSITION_BACK },
			{ RubikCube.FACE_ID_FRONT, RubikCube.FACE_POSITION_FRONT },
			{ RubikCube.FACE_ID_RIGHT, RubikCube.FACE_POSITION_RIGHT },
			{ RubikCube.FACE_ID_LEFT, RubikCube.FACE_POSITION_LEFT },
			{ RubikCube.FACE_ID_UP, RubikCube.FACE_POSITION_UP },
			{ RubikCube.FACE_ID_DOWN, RubikCube.FACE_POSITION_DOWN },
		};

		// (string faceId, bool isClockwise, SphericalVector blockPosition, string checkedFace, string expectedBlockFace)
		public static readonly TheoryData<string, bool, SphericalVector, string, string> FacesRotations = new TheoryData<string, bool, SphericalVector, string, string>()
		{
			{ RubikCube.FACE_ID_UP, true, RubikCube.BLOCK_POSITION_CORNER_UP_FRONT_LEFT, RubikCube.FACE_ID_FRONT, RubikCube.FACE_ID_RIGHT },
			{ RubikCube.FACE_ID_UP, false, RubikCube.BLOCK_POSITION_CORNER_UP_FRONT_LEFT, RubikCube.FACE_ID_FRONT, RubikCube.FACE_ID_LEFT },
		};

		#endregion Test Data

		#region Test Methods

		[Theory]
		[MemberData(nameof(RubikCubeTest.FacesIds), MemberType = typeof(RubikCubeTest))]
		public void RubikCube_CreateSize2_ContainsFourBlockPerFace(string faceId, SphericalVector v)
		{
			// 1. Prepare
			RubikCube c = new RubikCube(2);

			// 2. Execute
			int count = c.GetBlocksForFace(v).Count();

			// 3. Verify
			Assert.Equal(4, count);
		}

		[Theory]
		[MemberData(nameof(RubikCubeTest.FacesRotations), MemberType = typeof(RubikCubeTest))]
		public void RubikCube_RotateOnceOnSize2_FindExpectedFace(string faceId, bool isClockwise, SphericalVector blockPosition, string checkedFace, string expectedBlockFace)
		{
			// 1. Prepare
			RubikCube c = new RubikCube(2);
			var axis = c.Axes.FirstOrDefault(a => a.Id == faceId);
			var checkedAxis = c.Axes.FirstOrDefault(a => a.Id == checkedFace);
			var initialBlock = c.Blocks.FirstOrDefault(b => b.Position == blockPosition);

			// 2. Execute
			c.RotateAround(axis, isClockwise);
			var block = c.Blocks.FirstOrDefault(b => b.Position == blockPosition);
			var face = block.GetBlockFace(checkedAxis.Vector);

			// 3. Verify
			Assert.NotEqual(initialBlock.Id, block.Id);
			Assert.NotNull(face);
			Assert.Equal(expectedBlockFace, face.Id);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(0)]
		[InlineData(-1)]
		public void RubikCube_CreateInvalidSize_ThrowArgumentException(int size)
		{
			// 1. Prepare
			RubikCube c;

			// 2. Execute
			Action a = () => c = new RubikCube(size);

			// 3. Verify
			Assert.Throws<ArgumentException>(a);
		}

		[Theory]
		[InlineData(2, 8)]
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

		#endregion Test Methods
	}
}
