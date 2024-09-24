namespace Twisty.Engine.Materialization;

/// <summary>
/// Model object representing the coordinates allowing to represent a RotationCore object in 3d.
/// </summary>
public class MaterializedObject
{
	/// <summary>
	/// Create a new MaterializedObject from the list of his internal parts.
	/// </summary>
	/// <param name="id">ID of the object.</param>
	/// <param name="shapes">Collection of shapes forming the object.</param>
	internal MaterializedObject(string id, IEnumerable<MaterializedObjectShape> shapes)
	{
		this.Id = id;
		this.Shapes = shapes.ToArray();
	}

	/// <summary>
	/// Gets the collection of Shapes forming the object.
	/// </summary>
	public IEnumerable<MaterializedObjectShape> Shapes { get; }

	/// <summary>
	/// Gets the Id of the object.
	/// </summary>
	public string Id { get; }

	/// <summary>
	/// Merge the provided extensionBlock into the provided block.
	/// </summary>
	/// <param name="targetObject">Object in which the extension will be merged.</param>
	/// <param name="extensionObject">Object containing additional shapes for the target objects.</param>
	/// <returns>A new Materialized object representing the extended target object.</returns>
	public static MaterializedObject Merge(MaterializedObject targetObject, MaterializedObject extensionObject)
	{
		// Start from original shapes list instead of recreating it.
		var shapes = targetObject.Shapes.ToList();

		foreach (var shapeToMerge in extensionObject.Shapes)
		{
			// We expect only one face per color in current implementation.
			var targetShape = shapes.FirstOrDefault(p => p.Color == shapeToMerge.Color);

			if (targetShape is null)
				shapes.Add(shapeToMerge);
			else
			{
				// Replace previous shape with merged one.
				shapes.Remove(targetShape);
				shapes.Add(MaterializedObjectShape.Merge(targetShape, shapeToMerge));
			}
		}

		return new MaterializedObject(targetObject.Id, shapes);
	}
}
