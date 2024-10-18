using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Twisty.Engine.Structure.Builders;

/// <summary>
/// Class providing the possibility to load RotationCore from definitions formats files.
/// </summary>
public class RotationCoreBuilder
{
	private readonly RotationCoreBuilderContext m_Context;

	/// <summary>
	/// Create a new RotationCoreBuilder configured to load RotationCore from the provided definitions path.
	/// </summary>
	/// <param name="definitionsPah">Path to the folder containing definitions files.</param>
	public RotationCoreBuilder(string definitionsPah)
	{
		m_Context = new RotationCoreBuilderContext
		{
			DefinitionsPath = definitionsPah,
			SerializerOptions = new JsonSerializerOptions
			{
				NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
				Converters = {
					new RotationCoreFormatEntryConverter<CoreFace>(),
					new RotationCoreFormatEntryConverter<RotationAxis>(),
					new RotationCoreFormatEntryConverter<BlockDefinition>()
				}
			}
		};
	}

	/// <summary>
	/// LOad the RotationCore with the provided id from the rotation core definition files.
	/// </summary>
	/// <param name="coreId">Id of the RotationCore to load.</param>
	/// <returns>The RotationCore marching the provided id.</returns>
	public RotationCore LoadRotationCore(string coreId)
	{
		string filePath = Path.Combine(m_Context.DefinitionsPath, "Cores", coreId + ".json");
		using StreamReader reader = new(filePath);

		var format = JsonSerializer.Deserialize<RotationCoreFormat>(reader.ReadToEnd(), m_Context.SerializerOptions);

		return new RotationCore(
			format.Blocks.GetValues(m_Context),
			format.Axes.GetValues(m_Context),
			format.Faces.GetValues(m_Context));
	}
}
