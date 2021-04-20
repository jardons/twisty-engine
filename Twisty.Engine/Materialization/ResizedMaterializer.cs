using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twisty.Engine.Geometry;
using Twisty.Engine.Structure;

namespace Twisty.Engine.Materialization
{
	public enum ResizingMode
	{
		Fixed,
		Ratio
	}

	public class ResizedMaterializer : IMaterializer
	{
		private readonly IMaterializer m_Materializer;
		private readonly double m_Size;
		private readonly ResizingMode m_Mode;

		public ResizedMaterializer(IMaterializer materializer, double size, ResizingMode mode = ResizingMode.Ratio)
		{
			m_Materializer = materializer;
			m_Size = size;
			m_Mode = mode;
		}

		public MaterializedCore Materialize(RotationCore core)
		{
			var intermediate = m_Materializer.Materialize(core);
			Func<IEnumerable<Cartesian3dCoordinate>, IEnumerable<Cartesian3dCoordinate>> convert = m_Mode switch
			{
				ResizingMode.Ratio => ConvertUsingRatioResizing,
				ResizingMode.Fixed => ConvertUsingFixedResizing,
				_ => throw new InvalidOperationException("Resizing mode doesn't exist")
			};

			var objects = intermediate.Objects.Select(
				o => new MaterializedObject(o.Id, o.Parts.Select(
					p => new MaterializedObjectPart(p.Color, convert(p.Points)))
				)
			);

			return new MaterializedCore(objects);
		}

		private IEnumerable<Cartesian3dCoordinate> ConvertUsingFixedResizing(IEnumerable<Cartesian3dCoordinate> points)
		{
			var center = Cartesian3dCoordinate.GetCenterOfMass(points);
			return points.Select(p => ConvertUsingFixedResizing(center, p));
		}

		private Cartesian3dCoordinate ConvertUsingFixedResizing(Cartesian3dCoordinate center, Cartesian3dCoordinate point)
		{
			double distance = center.GetDistanceTo(point);
			return center + ((point - center) * (m_Size / distance));
		}

		private IEnumerable<Cartesian3dCoordinate> ConvertUsingRatioResizing(IEnumerable<Cartesian3dCoordinate> points)
		{
			var center = Cartesian3dCoordinate.GetCenterOfMass(points);
			return points.Select(p => center + ((p - center) * m_Size));
		}
	}
}
