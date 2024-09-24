using System.Runtime.CompilerServices;
using Twisty.Engine.Geometry;
using Twisty.Engine.Materialization.Colors;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Materialization;

public class StandardMaterializer : IMaterializer
{
	public IFaceColorProvider FaceColorProvider { get; init; } = new StandardCubeFaceColorProvider();

	public MaterializedCore Materialize(RotationCore core)
	{
		BandagesCollection bandages = core.RotationValidators.OfType<BandagesCollection>().FirstOrDefault();

		// Create new instance.
		List<MaterializedObject> objects = [];
		IEnumerable<Block> isolatedBlocks;

		if (bandages is not null)
		{
			HashSet<string> parsedBlocks = [];

			// Materialize all blocks.
			foreach (var bandage in bandages)
			{
				var principalBlock = bandage.Principal;
				var materializedBlock = MaterializeObject(core, principalBlock);

				var extendedBlocks = bandage.Extensions
					// Gets blocks from the closest to ensure we can always links extenstions.
					.OrderBy(b => b.Position.GetDistanceTo(principalBlock.Position))
					.Select(b => MaterializeObject(core, b));

				foreach (var block in extendedBlocks)
				{
					materializedBlock = MaterializedObject.Merge(materializedBlock, block);
					parsedBlocks.Add(block.Id);
				}

				parsedBlocks.Add(principalBlock.Id);
				objects.Add(materializedBlock);
			}

			isolatedBlocks = core.Blocks.Where(b => !parsedBlocks.Contains(b.Id));
		}
		else
			isolatedBlocks = core.Blocks;

		// Materialize isolated blocks.
		objects.AddRange(isolatedBlocks.Select(b => MaterializeObject(core, b)));
		
		return new MaterializedCore(objects);
	}

	#region Private Methods

	private MaterializedObject MaterializeObject(RotationCore core, Block b)
	{
		List<MaterializedObjectShape> parts = [];
		foreach (BlockFace face in b.Faces)
		{
			// Get the face from the cube as the block face don't contain face Plane coordinates.
			CoreFace cubeFace = core.GetFace(face.Id);

			Cartesian3dCoordinate center = GetInternalPoint(core, b.Id, face.Id);
			var points = this.GetFaceVertices(core, cubeFace, center);
			parts.Add(new MaterializedObjectShape(FaceColorProvider.GetColor(core, b, face), points));
		}

		return new MaterializedObject(b.Id, parts);
	}

	private static Cartesian3dCoordinate GetInternalPoint(RotationCore core, string blockId, string faceId)
	{
		CoreFace cubeFace = core.GetFace(faceId);
		Block b = core.GetBlock(blockId);

		var c = cubeFace.Plane.GetIntersection(new ParametricLine(b.Position, b.GetBlockFace(faceId).Position)) * 0.95;
		var l = cubeFace.Plane.GetPerpendicular(c);

		return cubeFace.Plane.GetIntersection(l);
	}

	/// <summary>
	/// Provide the vertices of a block face surrounding any point belonging to the block face.
	/// </summary>
	/// <param name="face">Face of the original RotationCore from which the block face will be defined.</param>
	/// <param name="internalPoint">Can be any point inside the block face to evaluate.</param>
	/// <returns>The sorted list of vertices surrounding the block face containing the provided point on the provided CoreFace.</returns>
	private List<Cartesian3dCoordinate> GetFaceVertices(RotationCore core, CoreFace face, Cartesian3dCoordinate internalPoint)
	{
		var planes = GetFaceBondaries(core, face, internalPoint);
		List<Cartesian3dCoordinate> points = new(planes.Count);

		// As it's a loop, previous border of first row is the last one.
		var previousLine = planes[planes.Count - 1].Plane.GetIntersection(face.Plane);
		var previousPoint = planes[0].Plane.GetIntersection(previousLine);
		points.Add(previousPoint);

		for (int i = 0; i < planes.Count - 1; ++i)
		{
			// Get Next point.
			var currentLine = planes[i].Plane.GetIntersection(face.Plane);
			var currentPoint = planes[i + 1].Plane.GetIntersection(currentLine);

			// Ignore when multiple cut provide the same point.
			if (points.Last().IsSamePoint(currentPoint))
				continue;

			if (!IsPointInCore(currentPoint))
				continue;

			if (points.Count > 2 && IsBehindClosestCut(currentPoint, points, internalPoint, planes.Select(p => p.Plane)))
				continue;

			points.Add(currentPoint);
		}

		points = CleanClosestCut(points, internalPoint, planes.Select(p => p.Plane));

		// We will sort the points to ensure we are always providing them in the same rotation direction.
		var comparer = new CircularVectorComparer(face.Plane, internalPoint);
		points.Sort(comparer);

		return points;
	}

	private static List<IPlanar> GetFaceBondaries(RotationCore core, CoreFace face, Cartesian3dCoordinate internalPoint)
	{
		var facesPlanes = core.Faces.Where(f => f.Id != face.Id);
		var axisPlanes = core.Axes.SelectMany(a => a.Layers);

		var planar = facesPlanes.OfType<IPlanar>().Concat(axisPlanes)
			// Parallels planes will never have intersection with current face for corners.
			.Where(o => !o.Plane.IsParallelTo(face.Plane));

		// Filter Planes to only keep the closest when using the same normal.
		List<IPlanar> result = FilterToClosestPlanar(planar, internalPoint, face.Plane);

		// In order to keep only the closest intersection, we need to sort the planes.
		var comparer = new CircularVectorComparer(face.Plane, internalPoint);
		result.Sort(comparer);

		return result;
	}

	private static List<IPlanar> FilterToClosestPlanar(IEnumerable<IPlanar> planars, Cartesian3dCoordinate internalPoint, Plane referencePlane)
	{
		// Keep cuts from the closest to the fartest.
		var tuples = planars.Select(o => new Tuple<IPlanar, double, Cartesian3dCoordinate>(
			o,
			o.Plane.GetDistanceTo(internalPoint),
			referencePlane.GetVectorProjection(o.Plane.Normal)));

		var result = new List<Tuple<IPlanar, double, Cartesian3dCoordinate>>();
		foreach (var t in tuples.OrderBy(t => t.Item2))
		{
			IPlanar p = t.Item1;
			bool isAbovePlane = p.Plane.IsAbovePlane(internalPoint);

			// Skip planes when a closer parrallel plane has beend selected.
			if (result.Any(o =>
					(o.Item3.IsSameVector(t.Item3)
						&& o.Item1.Plane.IsAbovePlane(internalPoint) == isAbovePlane)
					|| (o.Item3.Reverse.IsSameVector(t.Item3)
						&& o.Item1.Plane.IsBelowPlane(internalPoint) == isAbovePlane)))
				continue;

			result.Add(t);
		}

		return result.Select(t => t.Item1).ToList();
	}

	private static bool IsBehindClosestCut(Cartesian3dCoordinate testedPoint, IList<Cartesian3dCoordinate> points, Cartesian3dCoordinate internalPoint,
		IEnumerable<Plane> planes)
	{
		ParametricLine testedLine = ParametricLine.FromTwoPoints(internalPoint, testedPoint);
		double testedDistance = internalPoint.GetDistanceTo(testedPoint);
		for (int i = 0; i < points.Count - 1; ++i)
		{
			var p1 = points[i];
			if (p1.IsSamePoint(testedPoint))
				continue;

			for (int j = i + 1; j < points.Count; ++j)
			{
				var p2 = points[j];
				if (p2.IsSamePoint(testedPoint))
					continue;

				try
				{
					ParametricLine line = ParametricLine.FromTwoPoints(p1, p2);
					// Skip diagonal going through internal point (could be central point).
					if (line.Contains(internalPoint))
						continue;

					// If line don't match a cut, ignore the line.
					if (!planes.Any(p => p.IsOnPlane(line)))
						continue;

					Cartesian3dCoordinate intersection = testedLine.GetIntersection(line);
					double d = internalPoint.GetDistanceTo(intersection);
					if (d < testedDistance && !IsPointBetween(internalPoint, testedPoint, intersection))
						return true;
				}
				catch (GeometricException)
				{
					// No intersection, we can ignore the line.
				}
			}
		}

		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool IsPointBetween(in Cartesian3dCoordinate testedPoint, in Cartesian3dCoordinate p1, in Cartesian3dCoordinate p2)
	{
		double dt1 = testedPoint.GetDistanceTo(p1);
		double dt2 = testedPoint.GetDistanceTo(p2);
		double d12 = p1.GetDistanceTo(p2);

		return d12.IsEqualTo(dt1 + dt2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static List<Cartesian3dCoordinate> CleanClosestCut(IList<Cartesian3dCoordinate> points, Cartesian3dCoordinate internalPoint, IEnumerable<Plane> planes)
		=> points.Where(p => !IsBehindClosestCut(p, points, internalPoint, planes)).ToList();

	/// <summary>
	/// Validate if a point is inside the RotationCore scope or not.
	/// </summary>
	/// <param name="p">Coordinate of the point to validate.</param>
	/// <returns>A boolean indicating if whether the point is inside the RotationCore scope or not</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static bool IsPointInCore(in Cartesian3dCoordinate p) =>
		p.X >= -1.0 && p.X <= 1.0
			&& p.Y >= -1.0 && p.Y <= 1.0
			&& p.Z >= -1.0 && p.Z <= 1.0;

	#endregion Private Methods
}
