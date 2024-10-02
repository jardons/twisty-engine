using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure;

/// <summary>
/// Class describing a rotation axis around the RotationCore.
/// The axis is defined by an angle coming from the RotationCore center and goind trough on of the faces.
/// </summary>
public class RotationAxis
{
	/// <summary>
	/// Create a new RotationAxis object.
	/// </summary>
	/// <param name="id">Id of the RotationAxis.</param>
	/// <param name="vector">Rotation Axis coordinate starting from the core center.</param>
	/// <param name="layersDistances">
	/// List distances to each layers on the current axis indexed per id.
	/// Layer will be created on origin if null.
	/// </param>
	/// <exception cref="System.ArgumentNullException">Axis id is mandatory.</exception>
	/// <exception cref="System.ArgumentException">Axis id cannot be an empty or a white string.</exception>
	public RotationAxis(string id, Cartesian3dCoordinate vector, IDictionary<string, double> layersDistances = null)
	{
		if (id is null)
			throw new ArgumentNullException(nameof(id), "Axis id is mandatory.");

		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Axis id cannot be an empty or a white string.", nameof(id));

		if (vector.IsZero)
			throw new ArgumentException("Axis cannot be on initial point.", nameof(vector));

		this.Id = id;
		this.Vector = vector;
		this.Layers = layersDistances is null
			? [ new LayerSeparator($"L_{id}", new Plane(vector, 0.0)) ]
			: layersDistances.Select(kv => new LayerSeparator(kv.Key, new Plane(vector, kv.Value)));
	}

	/// <summary>
	/// Create a new RotationAxis object.
	/// </summary>
	/// <param name="id">Id of the RotationAxis.</param>
	/// <param name="vector">Rotation Axis coordinate starting from the core center.</param>
	/// <param name="layers">Collection of the layer on this axis.</param>
	/// <exception cref="System.ArgumentNullException">Axis id is mandatory.</exception>
	/// <exception cref="System.ArgumentException">Axis id cannot be an empty or a white string.</exception>
	[JsonConstructor]
	public RotationAxis(string id, Cartesian3dCoordinate vector, IEnumerable<LayerSeparator> layers)
	{
		if (id is null)
			throw new ArgumentNullException(nameof(id), "Axis id is mandatory.");

		if (string.IsNullOrWhiteSpace(id))
			throw new ArgumentException("Axis id cannot be an empty or a white string.", nameof(id));

		if (vector.IsZero)
			throw new ArgumentException("Axis cannot be on initial point.", nameof(vector));

		this.Id = id;
		this.Vector = vector;
		this.Layers = layers.ToArray();
	}

	#region Public Properties

	/// <summary>
	/// Gets the Id of the RotationAxis.
	/// </summary>
	public string Id { get; }

	/// <summary>
	/// Gets the Axis coordinate used for the rotation.
	/// </summary>
	public Cartesian3dCoordinate Vector { get; }

	/// <summary>
	/// List of layers separator aligned on this rotation axis.
	/// </summary>
	public IEnumerable<LayerSeparator> Layers { get; }

	#endregion Public Properties

	#region Public Methods

	/// <summary>
	/// Gets the Upper Layer of the axis, providing the limitation to the closest layer from the face.
	/// </summary>
	/// <returns>The Upper Layer of the axis, providing the limitation to the closest layer from the face.</returns>
	public LayerSeparator GetUpperLayer()
		=> Layers.OrderByDescending(l => Math.Abs(l.Plane.D)).First();

	#endregion Public Methods
}