using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure.Topology;

public class AngularTopologyBuilder : ITopologyBuilder
{
	public string GetTopologicId(Block block)
	{
		var list = block.Faces.Select(f => f.Position).ToList();
		var plane = new Plane(block.InitialPosition, Cartesian3dCoordinate.Zero);
		var comparer = new CircularVectorComparer(plane);
		list.Sort(comparer);

		string[] idParts = new string[list.Count * 2 - 1];

		// Create id based on two informations allowing to identify position of all parts :
		// * Angle from the face to the plane perpendicular to the block axis.
		// * Angle between the face and previous face.
		idParts[0] = Stringify(plane.GetThetaTo(list[0]));

		for (int i = 1; i < list.Count; i++)
		{
			idParts[i * 2 - 1] = Stringify(list[i - 1].GetThetaTo(list[i]));
			idParts[i * 2] = Stringify(plane.GetThetaTo(list[i]));
		}

		string id = string.Join('-', idParts);

		// Simple blocks don't need the extension part.
		if (block.Bandage is null)
			return id;

		var extensions = block.Bandage.Extensions
			.OrderBy(b => b.InitialPosition, comparer)
			.ToList();

		idParts = new string[extensions.Count * 2 + 1];

		// Start from central block.
		idParts[0] = id;

		idParts[1] = $"{Stringify(plane.GetThetaTo(extensions[0].InitialPosition))}*{GetTopologicId(extensions[0])}";

		// Create id based on two informations allowing to identify position of all extensions :
		// * Angle from the extension block to the plane perpendicular to the principal block axis.
		// * Angle between the extension block axix and next previous face.
		for (int i = 1; i < list.Count; i++)
		{
			idParts[i * 2] = Stringify(extensions[i - 1].InitialPosition.GetThetaTo(extensions[i].InitialPosition));
			idParts[i * 2 + 1] = $"{Stringify(plane.GetThetaTo(extensions[i].InitialPosition))}*{GetTopologicId(extensions[i])}";
		}

		return string.Join('#', idParts);
	}

	/// <summary>
	/// Transform an angle to its string representaiton to use in the generated TopologicId.
	/// </summary>
	/// <param name="angle"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static string Stringify(double angle)
		=> double.Round(angle * 100.0).ToString();
}
