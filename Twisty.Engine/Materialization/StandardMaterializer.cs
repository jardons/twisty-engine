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
		private RotationCore m_Core;

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

		public IList<IPlanar> GetFaceBondaries(CoreFace face, Cartesian3dCoordinate center)
		{
			var facesPlanes = m_Core.Faces.Where(f => f.Id != face.Id);
			var axisPlanes = m_Core.Axes.SelectMany(a => a.Layers);

			var planar = facesPlanes.OfType<IPlanar>().Concat(axisPlanes)
				// Parallels planes will never have intersection with current face for corners.
				.Where(o => !o.Plane.IsParallelTo(face.Plane));

			// Filter Planes to only keep the closest when using the same normal.
			List<IPlanar> result = FilterToClosestPlanar(planar, center);

			// Exclude non parallel plane with parralel intersections.
			//result = FilterToClosestIntersection(result, face, center);

			// In order to keep only the closest intersection, we need to sort the planes.
			var comparer = new CircularVectorComparer(face.Plane, center);
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
