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
		public class TestBlock : Block
		{
			public TestBlock(BlockFace f)
				: base(f)
			{
			}

			public TestBlock(IEnumerable<BlockFace> f)
				: base(f)
			{
			}

			public override string Id => string.Empty;
		}

		#region Test Methods

		[Fact]
		public void Block_CreateBlockWithNullFace_ThrowArgumentNullException()
		{
			// 1. Prepare
			Block b;
			BlockFace f = null;
			IEnumerable<BlockFace> faces = null;

			// 2. Execute
			Action a1 = () => b = new TestBlock(f);
			Action a2 = () => b = new TestBlock(faces);

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
			Action a = () => b = new TestBlock(faces);

			// 3. Verify
			Assert.Throws<ArgumentException>(a);
		}

		[Fact]
		public void Block_CreateAndGetFace_ShouldFindFace()
		{
			// 1. Prepare
			SphericalVector faceOrientation = new SphericalVector(Math.PI, Math.PI);

			// 2. Execute
			Block b = new TestBlock(new BlockFace("test", faceOrientation));
			BlockFace f = b.GetBlockFace(faceOrientation);

			// 3. Verify
			Assert.NotNull(f);
		}

		[Theory]
		// Base Axis Rotations
		[InlineData("(0 0 1)", Math.PI /2.0, "(1 0 0)", "(0 -1 0)")]
		[InlineData("(0 0 1)", Math.PI / 2.0, "(0 1 0)", "(1 0 0)")]
		[InlineData("(0 0 1)", Math.PI / 2.0, "(-1 0 0)", "(0 1 0)")]
		[InlineData("(0 0 1)", Math.PI / 2.0, "(0 -1 0)", "(-1 0 0)")]
		// Skewb rotations (120 degree corners)
		[InlineData("(1 1 1)", Math.PI * 2.0 / 3.0, "(1 0 0)", "(0 0 1)")]
		[InlineData("(1 1 1)", -Math.PI * 2.0 / 3.0, "(1 0 0)", "(0 1 0)")]
		public void Block_RotateAndGetFace_FindFace(string rotationAxisCc, double theta, string faceCc, string expectedCc)
		{
			// 1. Prepare
			Cartesian3dCoordinate axis = new Cartesian3dCoordinate(rotationAxisCc);
			Cartesian3dCoordinate faceOrientation = new Cartesian3dCoordinate(faceCc);
			Cartesian3dCoordinate expectedOrientation = new Cartesian3dCoordinate(expectedCc);
			Block b = new TestBlock(new BlockFace("test", faceOrientation));

			// 2. Execute
			b.RotateAround(axis, theta);
			BlockFace f = b.GetBlockFace(expectedOrientation);

			// 3. Verify
			Assert.NotNull(f);
		}

		[Fact]
		public void Block_CreateAndGetFaceWithBadOrientation_ReturnNull()
		{
			// 1. Prepare
			SphericalVector faceOrientation = new SphericalVector(Math.PI, Math.PI);
			Block b = new TestBlock(new BlockFace("test", faceOrientation));

			// 2. Execute
			BlockFace f = b.GetBlockFace(new SphericalVector(100, 100));

			// 3. Verify
			Assert.Null(f);
		}

		[Theory]
		[InlineData("(0 0 1)", "(1 0 0)", Math.PI / 2.0)]
		public void Block_CreateRotateAndGetFaceWithOriginalOrientation_ReturnNull(string axisCc, string originalFace, double theta)
		{
			// 1. Prepare
			Cartesian3dCoordinate axis = new Cartesian3dCoordinate(axisCc);
			Cartesian3dCoordinate faceOrientation = new Cartesian3dCoordinate(originalFace);
			Block b = new TestBlock(new BlockFace("test", faceOrientation));

			b.RotateAround(axis, theta);

			// 2. Execute
			BlockFace f = b.GetBlockFace(faceOrientation);

			// 3. Verify
			Assert.Null(f);
		}

		#endregion Test Methods
	}
}