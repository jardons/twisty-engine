using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;
using Twisty.Engine.Geometry;

namespace Twisty.Engine.Structure;

/// <summary>
/// Class describing a planar separator between 2 rotation layer.
/// </summary>
[DebuggerDisplay("{GetType().Name} ({Id} [{Plane.A}, {Plane.B}, {Plane.C}, {Plane.D}])")]
public class LayerSeparator : IPlanar
{
	[JsonConstructor]
	public LayerSeparator(string id, Plane p)
	{
		this.Id = id;
		this.Plane = p;
	}

	/// <summary>
	/// Gets the Id of this layer separator.
	/// </summary>
	public string Id { get; }

	/// <summary>
	/// Plane defining the position of the LayerSeparator.
	/// </summary>
	public Plane Plane { get; }
}
