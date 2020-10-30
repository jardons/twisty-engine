using System;
using System.Linq;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Skewb;
using Xunit;

namespace Twisty.Engine.Tests.Structure.Skewb
{
	[Trait("Category", "Structure")]
	public class SkewbTest
	{
		#region Test Methods

		[Fact]
		public void Skewb_CreateAndCountBlocksPerFace_ShouldMatch()
		{
			// 1. Prepare
			int i = 0;
			int[] results = new int[6];

			// 2. Execute
			SkewbCube c = new SkewbCube();
			foreach (CoreFace face in c.Faces)
				results[i++] = c.GetBlocksForFace(face.Id).Count();

			// 3. Verify
			for (i = 0; i < results.Length; ++i)
				Assert.Equal(5, results[i]);
		}

		#endregion Test Methods
	}
}
