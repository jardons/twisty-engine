using Twisty.Engine.Structure;

namespace Twisty.Engine.Materialization
{
	public interface IMaterializer
	{
		MaterializedCore Materialize(RotationCore core);
	}
}