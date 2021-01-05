using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Materialization
{
	public class StandardMaterializer
	{
		private readonly RotationCore m_Core;

		public StandardMaterializer(RotationCore core)
		{
			m_Core = core;
		}

		public Cartesian3dCoordinate GetCenter(string blockId, string faceId)
		{
			CoreFace cubeFace = m_Core.GetFace(faceId);
			Block b = m_Core.GetBlock(blockId);

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
		public IList<Cartesian3dCoordinate> GetFaceVertices(CoreFace face, Cartesian3dCoordinate internalPoint)
		{
			var planes = this.GetFaceBondaries(face, internalPoint);
			List<Cartesian3dCoordinate> points = new List<Cartesian3dCoordinate>(planes.Count);

			// As it's a loop, previous border of first row is the last one.
			var previousLine = planes[planes.Count - 1].Plane.GetIntersection(face.Plane);
			var previousPoint = planes[0].Plane.GetIntersection(previousLine);
			points.Add(previousPoint);

			for (int i = 0; i < planes.Count - 1; ++i)
			{
				// Get Next point.
				var currentLine = planes[i].Plane.GetIntersection(face.Plane);
				var currentPoint = planes[i + 1].Plane.GetIntersection(currentLine);

				points.Add(currentPoint);
			}

			// We will sort the points to ensure we are always providing them in the same rotation direction.
			var comparer = new CircularVectorComparer(face.Plane, internalPoint);
			points.Sort(comparer);

			return points;
		}

		public IList<IPlanar> GetFaceBondaries(CoreFace face, Cartesian3dCoordinate internalPoint)
		{
			var facesPlanes = m_Core.Faces.Where(f => f.Id != face.Id);
			var axisPlanes = m_Core.Axes.SelectMany(a => a.Layers);

			var planar = facesPlanes.OfType<IPlanar>().Concat(axisPlanes)
				// Parallels planes will never have intersection with current face for corners.
				.Where(o => !o.Plane.IsParallelTo(face.Plane));

			// Filter Planes to only keep the closest when using the same normal.
			List<IPlanar> result = FilterToClosestPlanar(planar, internalPoint);

			// Exclude non parallel plane with parralel intersections.
			//result = FilterToClosestIntersection(result, face, center);

			// In order to keep only the closest intersection, we need to sort the planes.
			var comparer = new CircularVectorComparer(face.Plane, internalPoint);
			result.Sort(comparer);

			return result;
		}

		private List<IPlanar> FilterToClosestPlanar(IEnumerable<IPlanar> planars, Cartesian3dCoordinate center)
		{
			// Keep cuts from the closest to the fartest.
			var tuples = planars.Select(o => new Tuple<double, IPlanar>(
				o.Plane.GetDistanceTo(center),
				o));

			List<IPlanar> result = new List<IPlanar>();
			foreach (IPlanar p in tuples.OrderBy(t => t.Item1).Select(t => t.Item2))
			{
				if (!result.Any(o => o.Plane.Normal.IsSameVector(p.Plane.Normal)))
					result.Add(p);
			}

			return result;
		}

		private List<IPlanar> FilterToClosestIntersection(IEnumerable<IPlanar> planars, CoreFace face, Cartesian3dCoordinate center)
		{
			// Keep cuts from the closest to the fartest.
			var tuples = planars
				.Select(o => new Tuple<ParametricLine, IPlanar>(
					o.Plane.GetIntersection(face.Plane),
					o))
				.Select(t => new Tuple<double, ParametricLine, IPlanar>(
					t.Item1.GetDistanceTo(center),
					t.Item1,
					t.Item2));

			List<Tuple<double, ParametricLine, IPlanar>> result = new List<Tuple<double, ParametricLine, IPlanar>>();
			foreach (var t in tuples.OrderBy(t => t.Item1))
			{
				if (!result.Any(o => o.Item2.IsParallelTo(t.Item2)))
					result.Add(t);
			}

			return result.Select(t => t.Item3).ToList();
		}
	}
}
