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
			{ RubikCube.FACE_ID_TOP, RubikCube.FACE_POSITION_TOP },
			{ RubikCube.FACE_ID_BOTTOM, RubikCube.FACE_POSITION_BOTTOM },
		};

		// (string faceId, bool isClockwise, SphericalVector blockPosition, string checkedFace, string expectedBlockFace)
		public static readonly TheoryData<string, bool, SphericalVector, string, string> FacesRotations = new TheoryData<string, bool, SphericalVector, string, string>()
		{
			{ RubikCube.FACE_ID_TOP, true, new SphericalVector(Math.PI / 4.0, Math.PI / 4.0 * 3.0), RubikCube.FACE_ID_FRONT, RubikCube.FACE_ID_LEFT },
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
		public void RubikCube_CreateSize2AndRotateOnce_FindExpectedFace(string faceId, bool isClockwise, SphericalVector blockPosition, string checkedFace, string expectedBlockFace)
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
			RubikCube c = new RubikCube(size);

			// 2. Execute
			int count = c.Blocks.Count();

			// 3. Verify
			Assert.Equal(blocksCount, count);
		}

		#endregion Test Methods
	}
}
