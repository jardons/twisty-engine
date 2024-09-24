using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Tests.DataFactories;

/// <summary>
/// Factory class providinbg BlockFaces instances.
/// </summary>
internal static class BlockFacesFactory
{
	public static BlockFace[] GetFaces(string facesId)
		=> facesId.Split(',').Select(id => id switch
		{
			"X" => new BlockFace("X", Cartesian3dCoordinate.XAxis),
			"Y" => new BlockFace("Y", Cartesian3dCoordinate.YAxis),
			"Z" => new BlockFace("Z", Cartesian3dCoordinate.ZAxis),
			"-X" => new BlockFace("-X", Cartesian3dCoordinate.XAxis.Reverse),
			"-Y" => new BlockFace("-Y", Cartesian3dCoordinate.YAxis.Reverse),
			"-Z" => new BlockFace("-Z", Cartesian3dCoordinate.ZAxis.Reverse),
			"AlmostX" => new BlockFace("AlmostX", new Cartesian3dCoordinate(1, 0.1, 0.1)),
			"C1" => new BlockFace("C1", new Cartesian3dCoordinate(1, 1, 1)),
			"C2" => new BlockFace("C2", new Cartesian3dCoordinate(-1, -1, -1)),
			_ => throw new NotImplementedException()
		}).ToArray();
}
