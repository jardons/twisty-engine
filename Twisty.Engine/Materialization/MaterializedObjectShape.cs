using System.Drawing;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Materialization;

/// <summary>
/// Object representing the vertices for one shape of a RotationCore object in 3d.
/// </summary>
public class MaterializedObjectShape
{
	/// <summary>
	/// Create a new MaterializedObjectShape from the list of points delimiting its border.
	/// </summary>
	/// <param name="color">Color used for this object part.</param>
	/// <param name="parts">Ordered collection of vertice delimiting this shape.</param>
	public MaterializedObjectShape(Color color, IEnumerable<Cartesian3dCoordinate> points)
	{
		this.Points = points;
		this.Color = color;
	}

	/// <summary>
	/// Gets the color of this part.
	/// </summary>
	public Color Color { get; }

	/// <summary>
	/// Gets the ordered list of vertices defining this shape.
	/// </summary>
	public IEnumerable<Cartesian3dCoordinate> Points { get; }

	/// <summary>
	/// Merge the provided extention shape in the targetShape.
	/// </summary>
	/// <param name="targetShape">Target shape that will be extended with the extensionShape.</param>
	/// <param name="extensionShape">Shape that will be used to extend the target shape.</param>
	/// <returns>A new MaterializedObjectShape instance repesneting the extended target shape.</returns>
	public static MaterializedObjectShape Merge(MaterializedObjectShape targetShape, MaterializedObjectShape extensionShape)
	{
		var loopedPoints = targetShape.Points.ToArray();
		var reservePoints = extensionShape.Points.ToArray();

		// Take the first point not shared with extension in order to :
		// * Ensure it will be part of the final shape border and not on the shared border.
		// * Ensure we start to iterate on the border of the correct shape, avoiding to keep points wrongly linked through the center of the shape.
		int i = 0;
		while (Array.IndexOf(reservePoints, loopedPoints[i]) >= 0)
			++i;

		List<Cartesian3dCoordinate> points = [loopedPoints[i]];

		while (!loopedPoints[i = (i + 1) % loopedPoints.Length].IsSamePoint(points[0]))
		{
			points.Add(loopedPoints[i]);

			var j = Array.IndexOf(reservePoints, loopedPoints[i]);
			if (j >= 0)
				(reservePoints, loopedPoints, i) = (loopedPoints, reservePoints, j);
		}

		return new MaterializedObjectShape(targetShape.Color, points);
	}
}
