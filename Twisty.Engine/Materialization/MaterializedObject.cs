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
	/// <param name="parts">Collection of parts forming the object.</param>
	internal MaterializedObject(string id, IEnumerable<MaterializedObjectShape> parts)
	{
		this.Id = id;
		this.Parts = parts.ToArray();
	}

	/// <summary>
	/// Gets the collection of Parts forming the object.
	/// </summary>
	public IEnumerable<MaterializedObjectShape> Parts { get; }

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
		// Start from original parts list instead of recreating it.
		var parts = targetObject.Parts.ToList();

		foreach (var partToMerge in extensionObject.Parts)
		{
			// We expect only one face per color in current implementation.
			var targetPart = parts.FirstOrDefault(p => p.Color == partToMerge.Color);

			if (targetPart is null)
				parts.Add(partToMerge);
			else
			{
				// Replace previous part with merged one.
				parts.Remove(targetPart);
				parts.Add(MaterializedObjectShape.Merge(targetPart, partToMerge));
			}
		}

		return new MaterializedObject(targetObject.Id, parts);
	}
}
