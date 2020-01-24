﻿using System;
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

		#region Test Data

		public static readonly TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate> RotationsValues = new TheoryData<Cartesian3dCoordinate, double, Cartesian3dCoordinate>()
		{
			{ new Cartesian3dCoordinate(0.0, 0.0, 1.0), Math.PI / 2.0, new Cartesian3dCoordinate(0.0, 1.0, 1.0) },
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
		[MemberData(nameof(BlockTest.RotationsValues), MemberType = typeof(BlockTest))]
		public void Block_RotateAndGetFace_ShouldFindFace(Cartesian3dCoordinate axis, double theta, Cartesian3dCoordinate expected)
		{
			// 1. Prepare
			SphericalVector faceOrientation = new SphericalVector(Math.PI, Math.PI);
			Block b = new TestBlock(new BlockFace("test", faceOrientation));

			// 2. Execute
			b.RotateAround(axis, theta);
			faceOrientation = faceOrientation.RotateAround(CoordinateConverter.ConvertToSpherical(axis), theta);
			BlockFace f = b.GetBlockFace(faceOrientation);

			// 3. Verify
			Assert.NotNull(f);
		}

		[Theory]
		[MemberData(nameof(BlockTest.RotationsValues), MemberType = typeof(BlockTest))]
		public void Block_CreateRotateTwiceAndGetFace_ShouldFindFace(Cartesian3dCoordinate axis, double theta, Cartesian3dCoordinate expected)
		{
			// 1. Prepare
			SphericalVector faceOrientation = new SphericalVector(Math.PI, Math.PI);
			Block b = new TestBlock(new BlockFace("test", faceOrientation));

			b.RotateAround(axis, theta);
			faceOrientation = faceOrientation.RotateAround(CoordinateConverter.ConvertToSpherical(axis), theta);

			b.RotateAround(axis, theta);
			faceOrientation = faceOrientation.RotateAround(CoordinateConverter.ConvertToSpherical(axis), theta);

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
			Block b = new TestBlock(new BlockFace("test", faceOrientation));

			// 2. Execute
			BlockFace f = b.GetBlockFace(new SphericalVector(100, 100));

			// 3. Verify
			Assert.Null(f);
		}

		[Theory]
		[MemberData(nameof(BlockTest.RotationsValues), MemberType = typeof(BlockTest))]
		public void Block_CreateRotateAndGetFaceWithOriginalOrientation_ReturnNull(Cartesian3dCoordinate axis, double theta, Cartesian3dCoordinate expected)
		{
			// 1. Prepare
			SphericalVector faceOrientation = new SphericalVector(Math.PI / 2.0, Math.PI / 2.0);
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