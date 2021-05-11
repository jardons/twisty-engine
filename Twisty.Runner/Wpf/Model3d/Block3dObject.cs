using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Twisty.Runner.Models.Model3d;

namespace Twisty.Runner.Wpf.Model3d
{
	public class Block3dObject
	{
		private List<BlockPart3dObject> m_3dModels;

		public Block3dObject(Core3dObject coreObject)
		{
			this.Key = coreObject.Id;
			m_3dModels = new();

			Model3DGroup group = new();

			foreach (var part in coreObject.Parts)
			{
				BlockPart3dObject part3d = new(part);

				m_3dModels.Add(part3d);
				group.Children.Add(part3d.GeometryModel);
			}

			this.Visual = new ModelVisual3D { Content = group };
		}

		public string Key { get; }
		public ModelVisual3D Visual { get; }

		public void SetColor(Color c)
		{
			foreach (var p in m_3dModels)
				p.SetColor(c);
		}

		public void ResetColor()
		{
			foreach (var p in m_3dModels)
				p.ResetColor();
		}

		public void ApplyRotation(IEnumerable<Rotation> rotations)
		{
			Transform3DGroup group = new();

			// Use Euler angles to provide correct orientation on every blocks.
			foreach (var rotation in rotations)
			{
				group.Children.Add(new RotateTransform3D
				{
					Rotation = new AxisAngleRotation3D
					{
						Axis = rotation.Axis,
						Angle = rotation.Angle
					}
				});
			}

			this.Visual.Transform = group;
		}
	}
}
