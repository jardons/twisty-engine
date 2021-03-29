using System;
using System.Collections.Generic;
using System.Text;

namespace Twisty.Runner.Models.Model3d
{
	public class Core3d
	{
		public Core3d(string id, IEnumerable<Core3dObject> objects)
		{
			this.Id = id;
			this.Objects = objects;
		}

		public string Id { get; }

		public IEnumerable<Core3dObject> Objects { get; }
	}
}
