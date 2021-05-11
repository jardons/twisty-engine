using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twisty.Engine.Geometry.Rotations;
using Twisty.Engine.Materialization;
using Twisty.Engine.Operations;
using Twisty.Engine.Operations.Rubiks;
using Twisty.Engine.Operations.Skewb;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Analysis;
using Twisty.Runner.Models;
using Twisty.Runner.Models.Model3d;

namespace Twisty.Runner.Services
{
	public interface IRotationCoreService
	{
		public RotationCoreObject CreateNewCore(string coreTypeId, string materializerId, Action onRotationChange);

		CoreAlterations CalculateAlterations(RotationCoreObject core);

		void RunCommand(RotationCoreObject core, string command);

		bool TryCleanCommand(string coreId, string command, out string cleanedCommand);
	}

	internal class RotationCoreService : IRotationCoreService
	{
		#region Private Classes

		private class OperationRunnerSpy : IOperationRunner
		{
			private readonly IOperationRunner m_Runner;
			private readonly Action m_OnAfterExecute;

			public OperationRunnerSpy(IOperationRunner runner, Action onAfterExecute)
			{
				m_Runner = runner;
				m_OnAfterExecute = onAfterExecute;
			}

			public void Execute(IEnumerable<IOperation> operations)
			{
				m_Runner.Execute(operations);
				m_OnAfterExecute();
			}

			public void Execute(IOperation operation)
			{
				m_Runner.Execute(operation);
				m_OnAfterExecute();
			}
		}

		#endregion Private Classes

		// Fixed Tools
		private readonly CoreFactory m_Factory;

		// State fields
		private readonly IDictionary<string, IOperationsParser> m_Parsers;

		public RotationCoreService()
		{
			m_Factory = new CoreFactory();
			m_Parsers = new Dictionary<string, IOperationsParser>();
		}

		public RotationCoreObject CreateNewCore(string coreTypeId, string materializerId, Action onRotationChange)
		{
			var materializer = GetMaterializer(materializerId);

			RotationCore core = m_Factory.CreateCore(coreTypeId);
			var runner = new OperationRunnerSpy(new OperationRunner(core), onRotationChange);
			var core3d = new Core3d(coreTypeId, materializer.Materialize(core).Objects.Select(o => new Core3dObject(o)));

			return new RotationCoreObject(coreTypeId, core, runner, core3d);
		}

		public CoreAlterations CalculateAlterations(RotationCoreObject core)
		{
			var positions = new Dictionary<string, IReadOnlyList<SimpleRotation3d>>();

			foreach (var block in core.Core.Blocks)
				positions[block.Id] = block.Orientation.GetEulerAngles();

			ResolutionAnalyzer analyzer = new(core.Core);

			return new CoreAlterations(positions, analyzer.GetAlterations());
		}

		public void RunCommand(RotationCoreObject core, string command)
		{
			if (string.IsNullOrWhiteSpace(command))
				return;

			var operations = GetParser(core.Id).Parse(command);
			core.Runner.Execute(operations);
		}

		public bool TryCleanCommand(string coreId, string command, out string cleanedCommand)
		{
			if (string.IsNullOrWhiteSpace(command))
			{
				cleanedCommand = string.Empty;
				return true;
			}

			return GetParser(coreId).TryClean(command, out cleanedCommand);
		}

		#region Private Members

		private IOperationsParser GetParser(string id)
		{
			if (m_Parsers.ContainsKey(id))
				return m_Parsers[id];

			IOperationsParser p = id switch
			{
				"Rubik[2]" => new RubikOperationsParser(),
				"Rubik[3]" => new RubikOperationsParser(),
				"Skewb" => new SkewbOperationsParser(),
				_ => throw new ArgumentException("Unknow parser id.", nameof(id)),
			};

			m_Parsers.Add(id, p);
			return p;
		}

		private static IMaterializer GetMaterializer(string materializerId)
			=> materializerId switch
			{
				"fixed" => new ResizedMaterializer(new StandardMaterializer(), 0.25, ResizingMode.Fixed),
				"ratio" => new ResizedMaterializer(new StandardMaterializer(), 0.6, ResizingMode.Ratio),
				_ => new StandardMaterializer()
			};

		#endregion Private Members
	}
}
