using System;

namespace Twisty.Engine.Geometry;

/// <summary>
/// Converter class allowing to project 3D coordinates on a 2D Plane.
/// </summary>
public class CartesianCoordinatesFlattener
{
    private readonly Func<Cartesian3dCoordinate, Cartesian2dCoordinate> m_From3dTo2d;

    /// <summary>
    /// Create a new CartesianCoordinatesFlattener for a specific Plane.
    /// </summary>
    /// <param name="p">Plane on which all converted coordinates will be projected.</param>
    public CartesianCoordinatesFlattener(Plane p)
    {
        this.Plane = p;
        m_From3dTo2d = Get2dConvertion(p);
    }

    /// <summary>
    /// Gets the Plane on which the points are projected.
    /// </summary>
    public Plane Plane { get; }

    /// <summary>
    /// Convert the provided 3D coordiantes to their projected 2D counterpart on the Plane used to create this CartesianCoordinatesFlattener.
    /// </summary>
    /// <param name="c3d">2D coordinates to project on the Plane and converts in 2D.</param>
    /// <returns>2D coordinates projected on the flattening reference Plane.</returns>
    public Cartesian2dCoordinate ConvertTo2d(Cartesian3dCoordinate c3d) => m_From3dTo2d(c3d);

    /// <summary>
    /// Gets the 2D coordinates on this flattener plane of the closest intersection point with another provided Plane.
    /// </summary>
    /// <param name="p">Plane intersecting the current Plane.</param>
    /// <param name="point">3D coordinates of the point of reference used to calculate the closest point.</param>
    /// <returns>2D coordinates of the closest intersection point projected on the flattening reference Plane.</returns>
    public Cartesian2dCoordinate GetClosestPoint(Plane p, Cartesian3dCoordinate point)
    {
        var l = this.Plane.GetIntersection(p);
        var perpendicular = l.GetPerpendicular(point);
        return ConvertTo2d(l.GetIntersection(perpendicular));
    }

    #region Private Members

    /// <summary>
    /// Gets the convertion function the most adapted to the provided Plane.
    /// </summary>
    /// <param name="p">Plane used to project the points in 2D.</param>
    /// <returns>A function that can be used to convert 3D coordinates to 2D.</returns>
    private Func<Cartesian3dCoordinate, Cartesian2dCoordinate> Get2dConvertion(Plane p)
    {
        if (p.Normal.IsOnX)
            return p.Normal.X > 0.0 ? GetFromFront : GetFromBack;

        if (p.Normal.IsOnY)
            return p.Normal.Y > 0.0 ? GetFromRight : GetFromLeft;

        if (p.Normal.IsOnZ)
            return p.Normal.Z > 0.0 ? GetFromTop : GetFromBottom;

        return GetFromProjection;
    }

    /// <summary>
    /// Gets the 2D coordinates as seen from the top.
    /// </summary>
    /// <param name="cc">3D coordinates to convert.</param>
    /// <returns>The converted 2D coordinates.</returns>
    private Cartesian2dCoordinate GetFromTop(Cartesian3dCoordinate cc) => new(cc.Y, -cc.X);

    /// <summary>
    /// Gets the 2D coordinates as seen from the bottom.
    /// </summary>
    /// <param name="cc">3D coordinates to convert.</param>
    /// <returns>The converted 2D coordinates.</returns>
    private Cartesian2dCoordinate GetFromBottom(Cartesian3dCoordinate cc) => new(cc.Y, cc.X);

    /// <summary>
    /// Gets the 2D coordinates as seen from the front.
    /// </summary>
    /// <param name="cc">3D coordinates to convert.</param>
    /// <returns>The converted 2D coordinates.</returns>
    private Cartesian2dCoordinate GetFromFront(Cartesian3dCoordinate cc) => new(cc.Y, cc.Z);

    /// <summary>
    /// Gets the 2D coordinates as seen from the back.
    /// </summary>
    /// <param name="cc">3D coordinates to convert.</param>
    /// <returns>The converted 2D coordinates.</returns>
    private Cartesian2dCoordinate GetFromBack(Cartesian3dCoordinate cc) => new(-cc.Y, cc.Z);

    /// <summary>
    /// Gets the 2D coordinates as seen from the right.
    /// </summary>
    /// <param name="cc">3D coordinates to convert.</param>
    /// <returns>The converted 2D coordinates.</returns>
    private Cartesian2dCoordinate GetFromRight(Cartesian3dCoordinate cc) => new(-cc.X, cc.Z);

    /// <summary>
    /// Gets the 2D coordinates as seen from the left.
    /// </summary>
    /// <param name="cc">3D coordinates to convert.</param>
    /// <returns>The converted 2D coordinates.</returns>
    private Cartesian2dCoordinate GetFromLeft(Cartesian3dCoordinate cc) => new(cc.X, cc.Z);

    /// <summary>
    /// Gets the 2D coordinates as seen from a calculated plane.
    /// </summary>
    /// <param name="cc">3D coordinates to convert.</param>
    /// <returns>The converted 2D coordinates.</returns>
    private Cartesian2dCoordinate GetFromProjection(Cartesian3dCoordinate cc)
    {
        // Project the coordinates on the reference
        ParametricLine projectionLine = this.Plane.GetPerpendicular(cc);
        Cartesian3dCoordinate planeCoordinate = this.Plane.GetIntersection(projectionLine);

        Cartesian3dCoordinate ccOnX = planeCoordinate.TransposeFromReferential(this.Plane.Normal);

        return GetFromFront(ccOnX);
    }

    #endregion Private Members
}