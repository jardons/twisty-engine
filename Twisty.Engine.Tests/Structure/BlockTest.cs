using System;
using System.Collections.Generic;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Xunit;

namespace Twisty.Engine.Tests.Structure
{
	[Trait("Category", "Structure")]
	public class BlockTest
	{
		#region Test Data

		public static readonly TheoryData<SphericalVector, double, SphericalVector> RotationsValues = new TheoryData<SphericalVector, double, SphericalVector>()
		{
			{SphericalVector.Origin, Math.PI / 2.0, new SphericalVector(0.0, Math.PI / 2.0)},
		};

		#endregion Test Data

		#region Test Methods

		[Fact]
		public void Block_CreateBlockWithNullFace_ThrowArgumentNullException()
		{
			// 1. Prepare
			Block b;
			BlockFace f = null;
			IEnumerable<BlockFace> faces = null;

			// 2. Execute
			Action a1 = () => b = new Block(f);
			Action a2 = () => b = new Block(faces);

			// 3. Verify
			Assert.Throws<ArgumentNullException>(a1);
			Assert.Throws<ArgumentNullException>(a2);
		}

		[Fact]
		public void Block_CreateBlockWithEmptyFaces_ThrowArgumentException()
		{
			// 1. Prepare
			Block b;
			IEnumerable<BlockFace> faces = new List<BlockFace>(0);

			// 2. Execute
			Action a = () => b = new Block(faces);

			// 3. Verify
			Assert.Throws<ArgumentException>(a);
		}

		[Fact]
		public void Block_CreateAndGetFace_ShouldFindFace()
		{
			// 1. Prepare
			SphericalVector faceOrientation = new SphericalVector(Math.PI, Math.PI);
			Block b = new Block(new BlockFace("test", faceOrientation));

			// 2. Execute
			BlockFace f = b.GetBlockFace(faceOrientation);

			// 3. Verify
			Assert.NotNull(f);
		}

		[Theory]
		[MemberData(nameof(BlockTest.RotationsValues), MemberType = typeof(BlockTest))]
		public void Block_CreateRotateAndGetFace_ShouldFindFace(SphericalVector axis, double theta, SphericalVector expected)
		{
			// 1. Prepare
			SphericalVector faceOrientation = new SphericalVector(Math.PI, Math.PI);
			Block b = new Block(new BlockFace("test", faceOrientation));

			b.RotateAround(axis, theta);
			faceOrientation = faceOrientation.RotateAround(axis, theta);

			// 2. Execute
			BlockFace f = b.GetBlockFace(faceOrientation);

			// 3. Verify
			Assert.NotNull(f);
		}

		[Theory]
		[MemberData(nameof(BlockTest.RotationsValues), MemberType = typeof(BlockTest))]
		public void Block_CreateRotateTwiceAndGetFace_ShouldFindFace(SphericalVector axis, double theta, SphericalVector expected)
		{
			// 1. Prepare
			SphericalVector faceOrientation = new SphericalVector(Math.PI, Math.PI);
			Block b = new Block(new BlockFace("test", faceOrientation));

			b.RotateAround(axis, theta);
			faceOrientation = faceOrientation.RotateAround(axis, theta);

			b.RotateAround(axis, theta);
			faceOrientation = faceOrientation.RotateAround(axis, theta);

			// 2. Execute
			BlockFace f = b.GetBlockFace(faceOrientation);

			// 3. Verify
			Assert.NotNull(f);
		}

		[Fact]
		public void Block_CreateAndGetFaceWithBadOrientation_ReturnNull()
		{
			// 1. Prepare
			SphericalVector faceOrientation = new SphericalVector(Math.PI, Math.PI);
			Block b = new Block(new BlockFace("test", faceOrientation));

			// 2. Execute
			BlockFace f = b.GetBlockFace(new SphericalVector(100, 100));

			// 3. Verify
			Assert.Null(f);
		}

		[Theory]
		[MemberData(nameof(BlockTest.RotationsValues), MemberType = typeof(BlockTest))]
		public void Block_CreateRotateAndGetFaceWithOriginalOrientation_ReturnNull(SphericalVector axis, double theta, SphericalVector expected)
		{
			// 1. Prepare
			SphericalVector faceOrientation = new SphericalVector(Math.PI / 2.0, Math.PI / 2.0);
			Block b = new Block(new BlockFace("test", faceOrientation));

			b.RotateAround(axis, theta);

			// 2. Execute
			BlockFace f = b.GetBlockFace(faceOrientation);

			// 3. Verify
			Assert.Null(f);
		}

		#endregion Test Methods
	}
}
