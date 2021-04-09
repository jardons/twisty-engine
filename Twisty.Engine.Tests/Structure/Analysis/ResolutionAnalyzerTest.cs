using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Analysis;
using Xunit;

namespace Twisty.Engine.Tests.Structure.Analysis
{
	public class ResolutionAnalyzerTest
	{
		[Theory]
		[InlineData("(1 0 0)")]
		[InlineData("(1 1 1)")]
		public void ResolutionAnalyzer_GetAlterationForUnchanged_GetNone(string facePosition)
		{
			// 1. Prepare
			Cartesian3dCoordinate cc = new(facePosition);
			Block b = new("lonely", Cartesian3dCoordinate.XAxis, new BlockFace("face", cc));
			ResolutionAnalyzer a = new(CreateStructure(b));

			// 2. Execute
			AlterationType r = a.GetAlterations(b);

			// 3. Verify
			Assert.Equal(AlterationType.None, r);
		}

		[Theory]
		[InlineData("(1 0 0)")]
		[InlineData("(1 1 1)")]
		public void ResolutionAnalyzer_RotateOnceAndGetAlteration_GetPosition(string facePosition)
		{
			// 1. Prepare
			Cartesian3dCoordinate cc = new(facePosition);

			BlockFace bf1 = new("face1", cc);
			BlockFace bf2 = new("face2", cc);
			Block b = new("lonely", Cartesian3dCoordinate.XAxis, bf1);
			Block originalBlock = new("other", Cartesian3dCoordinate.YAxis, bf2);

			ResolutionAnalyzer a = new(CreateStructure(b, originalBlock));

			// 2. Execute
			b.RotateAround(Cartesian3dCoordinate.ZAxis, Math.PI / 2.0);
			AlterationType r = a.GetAlterations(b);

			// 3. Verify
			Assert.Equal(AlterationType.Position, r);
		}

		[Theory]
		[InlineData("(1 0 0)", "(-1 0 0)", "(0 0 1)")]
		[InlineData("(1 1 1)", "(-1 -1 1)", "(0 0 1)")]
		public void ResolutionAnalyzer_SwitchSimilarOppositeBlockAndGetAlteration_GetNone(string facePosition, string originalPosition, string rotationAxis)
		{
			// 1. Prepare
			Cartesian3dCoordinate axis = new(rotationAxis);

			BlockFace bf1 = new("face", new Cartesian3dCoordinate(facePosition));
			BlockFace bf2 = new("face", new Cartesian3dCoordinate(originalPosition));
			Block b = new("lonely", bf1.Position, bf1);
			Block originalBlock = new("other", bf2.Position, bf2);

			ResolutionAnalyzer a = new(CreateStructure(b, originalBlock));

			// 2. Execute
			b.RotateAround(Cartesian3dCoordinate.ZAxis, Math.PI);
			originalBlock.RotateAround(axis, Math.PI);
			AlterationType r = a.GetAlterations(b);

			// 3. Verify
			Assert.Equal(AlterationType.None, r);
		}

		[Theory]
		[InlineData("(1 0 0)", "(-1 0 0)", "(0 0 1)")]
		[InlineData("(1 1 1)", "(-1 -1 1)", "(0 0 1)")]
		public void ResolutionAnalyzer_SwitchDifferentOppositeBlockAndGetAlteration_GetPosition(string facePosition, string originalPosition, string rotationAxis)
		{
			// 1. Prepare
			Cartesian3dCoordinate axis = new(rotationAxis);

			BlockFace bf1 = new("face1", new Cartesian3dCoordinate(facePosition));
			BlockFace bf2 = new("face2", new Cartesian3dCoordinate(originalPosition));
			Block b = new("lonely", bf1.Position, bf1);
			Block originalBlock = new("other", bf2.Position, bf2);

			ResolutionAnalyzer a = new(CreateStructure(b, originalBlock));

			// 2. Execute
			b.RotateAround(Cartesian3dCoordinate.ZAxis, Math.PI);
			originalBlock.RotateAround(axis, Math.PI);
			AlterationType r = a.GetAlterations(b);

			// 3. Verify
			Assert.Equal(AlterationType.Position, r);
		}

		[Theory]
		[InlineData("(1 0 0)")]
		[InlineData("(1 1 1)")]
		public void ResolutionAnalyzer_RotateBlockOnFaceAxisAndGetAlteration_GetNone(string facePosition)
		{
			// 1. Prepare
			Cartesian3dCoordinate axis = new(facePosition);
			BlockFace bf = new("face", axis);
			Block b = new("lonely", axis, bf);

			ResolutionAnalyzer a = new(CreateStructure(b));

			// 2. Execute
			b.RotateAround(axis, Math.PI / 3.0);
			AlterationType r = a.GetAlterations(b);

			// 3. Verify
			Assert.Equal(AlterationType.None, r);
		}

		[Theory]
		[InlineData("(1 0 0)", "(0 1 0)")]
		[InlineData("(1 1 1)", "(-1 -1 1)")]
		public void ResolutionAnalyzer_RotateBlockOnAxisWithoutFaceAndGetAlteration_GetOrientation(string facePosition, string rotationAxis)
		{
			// 1. Prepare
			Cartesian3dCoordinate axis = new(rotationAxis);

			BlockFace bf = new("face", Cartesian3dCoordinate.XAxis);
			Block b = new("lonely", new Cartesian3dCoordinate(facePosition), bf);

			ResolutionAnalyzer a = new(CreateStructure(b));

			// 2. Execute
			b.RotateAround(axis, Math.PI / 3.0);
			AlterationType r = a.GetAlterations(b);

			// 3. Verify
			Assert.Equal(AlterationType.Orientation, r);
		}

		#region Private Members

		private static IBlocksStructure CreateStructure(Block b, Block originalBlock = null)
		{
			Mock<IBlocksStructure> mock = new();

			List<Block> blocks = new() { b };
			if (originalBlock is not null)
			{
				blocks.Add(originalBlock);
				mock.Setup(o => o.GetBlockForInitialPosition(It.IsAny<Cartesian3dCoordinate>())).Returns(originalBlock);
			}
			else
				mock.Setup(o => o.GetBlockForInitialPosition(It.IsAny<Cartesian3dCoordinate>())).Returns(b);

			mock.Setup(o => o.GetBlock(It.IsAny<string>())).Returns(b);
			mock.SetupGet(o => o.Blocks).Returns(blocks);

			return mock.Object;
		}

		#endregion Private Members
	}
}
