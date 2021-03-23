using System;
using System.Collections.Generic;
using System.Text;

namespace Twisty.Runner.Models
{
	/// <summary>
	/// Description of camera.
	/// </summary>
	public class CameraDescription
	{
		/// <summary>
		/// Unique id allowing to identify the camera.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Label used to display the camera name.
		/// </summary>
		public string Name { get; set; }
	}
}
