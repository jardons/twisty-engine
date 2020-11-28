using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;
using Xunit;

namespace Twisty.Engine.Tests.Assertions
{
	public static class RotationCoreAssert
	{
		public static void ExposeAllFaces(RotationCore core)
		{
			foreach (var coreFace in core.Faces)
			{
				Plane p = new Plane(coreFace.Plane.Normal, Cartesian3dCoordinate.Zero);

				foreach (var block in core.GetBlocksForFace(coreFace.Id))
				{
					Assert.True(p.IsAbovePlane(block.Position));
				}
			}
		}
	}
}
