using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Twisty.Engine.Geometry;
using Twisty.Engine.Materialization;
using Twisty.Engine.Structure;
using Twisty.Engine.Structure.Rubiks;
using Twisty.Runner.Wpf;

namespace Twisty.Runner.Views
{
	/// <summary>
	/// Interaction logic for StandardView.xaml
	/// </summary>
	public partial class RotationCoreStandardView : UserControl
	{
		private RotationCore m_Core;
		private Dictionary<string, List<ModelVisual3D>> m_3dObjects;

		public RotationCoreStandardView()
		{
			m_Core = new RubikCube(3);
			m_3dObjects = new Dictionary<string, List<ModelVisual3D>>();

			foreach (var b in m_Core.Blocks)
			{
				m_3dObjects.Add(b.Id, CreateMesh(b));
			}

			InitializeComponent();

			foreach (var o in m_3dObjects.Values.SelectMany(o => o))
				MyAnimatedObject.Children.Add(o);

			this.MouseDoubleClick += RotationCoreStandardView_MouseDoubleClick;

		}

		private void RotationCoreStandardView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			int i = 0;
			foreach (var o in m_3dObjects.Values.SelectMany(o => o))
			{
				o.Transform = GetTransform();

				// TODO : analyse how to animate.
			/*
				string name = "temp" + (i++).ToString();
				canvasBox.RegisterName(name, o);

				AxisAngleRotation3D r = new AxisAngleRotation3D();
				r.Angle = 90;
				r.Axis = new Vector3D(1.0, 0.0, 0.0);

				SplineRotation3DKeyFrame kf = new SplineRotation3DKeyFrame();
				kf.Value = r;

				Rotation3DAnimationUsingKeyFrames animation = new Rotation3DAnimationUsingKeyFrames();
				animation.KeyFrames.Add(kf);
				myRotateTransform.Rotation.BeginAnimation(AxisAngleRotation3D.AxisProperty, myVectorAnimation);

				// Demonstrates the From and To properties used together.
				// Animates the rectangle's Width property from 50 to 300 over 10 seconds.
				DoubleAnimation myDoubleAnimation = new DoubleAnimation();
				myDoubleAnimation.From = 50;
				myDoubleAnimation.To = 300;
				myDoubleAnimation.Duration =
					new Duration(TimeSpan.FromSeconds(10));

				//Storyboard.SetTargetName(myDoubleAnimation, name);
				Storyboard.SetTargetName(animation, name);

				Storyboard myStoryboard = new Storyboard();
				//myStoryboard.Children.Add(myDoubleAnimation);
				myStoryboard.Children.Add(animation);

				myStoryboard.Begin();

				canvasBox.UnregisterName(name);*/
			}
		}

		private List<ModelVisual3D> CreateMesh(Engine.Structure.Block b)
		{
			List<ModelVisual3D> visuals = new List<ModelVisual3D>();

			foreach (BlockFace face in b.Faces)
			{
				// Create visual object.
				MeshGeometry3D geo = new MeshGeometry3D();
				GeometryModel3D model = new GeometryModel3D { Geometry = geo };
				ModelVisual3D visual = new ModelVisual3D { Content = model };
				visuals.Add(visual);

				StandardMaterializer materializer = new StandardMaterializer(m_Core);

				// Get the face from the cube as the block face don't contain face Plane coordinates.
				CoreFace cubeFace = m_Core.GetFace(face.Id);

				Cartesian3dCoordinate center = materializer.GetCenter(b.Id, face.Id);

				geo.Positions.Add(center.ToPoint3D()); // Center will always be position 0

				var bondaries = materializer.GetFaceBondaries(cubeFace, center);

				// As it's a loop, previous border of first row is the last one.
				var previousLine = bondaries[bondaries.Count - 1].Plane.GetIntersection(cubeFace.Plane);
				var previousPoint = bondaries[0].Plane.GetIntersection(previousLine);
				geo.Positions.Add(previousPoint.ToPoint3D());

				for (int i = 0; i < bondaries.Count - 1; ++i)
				{
					// Get Next point.
					var currentLine = bondaries[i].Plane.GetIntersection(cubeFace.Plane);
					var currentPoint = bondaries[i + 1].Plane.GetIntersection(currentLine);

					// Update positions.
					geo.Positions.Add(currentPoint.ToPoint3D());

					geo.TriangleIndices.Add(0);     // Center
					geo.TriangleIndices.Add(i + 1); // Previous
					geo.TriangleIndices.Add(i + 2); // Current
				}

				// Last triangle link to begining
				geo.TriangleIndices.Add(0);                 // Center
				geo.TriangleIndices.Add(bondaries.Count);   // Previous
				geo.TriangleIndices.Add(1);                 // Back to first


				model.Material = GetBrush(face.Id);
				model.BackMaterial = GetBrush(face.Id);

				//if (b.Id.Contains("U"))
				//	model.Transform = GetTransform();
			}

			return visuals;
		}

		private DiffuseMaterial GetBrush(string faceId)
		{
			Color c;
			switch (faceId)
			{
				case "F":
					c = Color.FromRgb(0, 0, 255);
					break;
				case "L":
					c = Color.FromRgb(255, 105, 0);
					break;
				case "R":
					c = Color.FromRgb(255, 0, 0);
					break;
				case "D":
					c = Color.FromRgb(255, 255, 255);
					break;
				case "B":
					c = Color.FromRgb(0, 250, 0);
					break;
				case "U":
					c = Color.FromRgb(255, 255, 0);
					break;
			}

			SolidColorBrush brush = new SolidColorBrush(c);
			return new DiffuseMaterial { Brush = brush };
		}

		private Transform3D GetTransform()
		{
			Transform3DGroup group = new Transform3DGroup();

			// Create and apply a transformation that rotates the object.
			RotateTransform3D myRotateTransform3D = new RotateTransform3D();
			AxisAngleRotation3D myAxisAngleRotation3d = new AxisAngleRotation3D();
			myAxisAngleRotation3d.Axis = new Vector3D(0, 1, 0);
			myAxisAngleRotation3d.Angle = 180;
			myRotateTransform3D.Rotation = myAxisAngleRotation3d;

			group.Children.Add(myRotateTransform3D);
			/*
			myRotateTransform3D = new RotateTransform3D();
			myAxisAngleRotation3d = new AxisAngleRotation3D();
			myAxisAngleRotation3d.Axis = new Vector3D(0, 1, 0);
			myAxisAngleRotation3d.Angle = 180;
			myRotateTransform3D.Rotation = myAxisAngleRotation3d;

			group.Children.Add(myRotateTransform3D);

			myRotateTransform3D = new RotateTransform3D();
			myAxisAngleRotation3d = new AxisAngleRotation3D();
			myAxisAngleRotation3d.Axis = new Vector3D(1, 0, 0);
			myAxisAngleRotation3d.Angle = 180;
			myRotateTransform3D.Rotation = myAxisAngleRotation3d;

			group.Children.Add(myRotateTransform3D);*/

			return group;
		}
	}
}
