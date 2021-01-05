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
using Twisty.Runner.Wpf;

namespace Twisty.Runner.Views
{
	/// <summary>
	/// Interaction logic for StandardView.xaml
	/// </summary>
	public partial class RotationCoreStandardView : UserControl
	{
		private RotationCore m_Core;
		private Dictionary<string, ModelVisual3D> m_3dObjects;

		public RotationCoreStandardView()
		{
			m_3dObjects = new Dictionary<string, ModelVisual3D>();

			InitializeComponent();

			this.MouseDoubleClick += RotationCoreStandardView_MouseDoubleClick;
			this.viewSelector.SelectionChanged += OnCameraSelectionChanged;
			this.SetCameraDirection();
		}

		private void OnCameraSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var val = e.AddedItems.OfType<ComboBoxItem>().FirstOrDefault()?.Tag as string;
			if (string.IsNullOrWhiteSpace(val))
				return;

			var coordinates = val.Split(",").Select(s => Convert.ToDouble(s)).ToList();

			this.camera.Position = new Point3D(coordinates[0], coordinates[1], coordinates[2]);
			this.SetCameraDirection();
		}

		/// <summary>
		/// Gets/Sets the RotationCore object visualised in this control.
		/// </summary>
		public RotationCore Core
		{
			get => m_Core;
			set
			{
				m_Core = value;
				this.CreateCanvasContent();
			}
		}

		/// <summary>
		/// Gets the list of ids for the various available cameras.
		/// </summary>
		public IList<string> CameraIds
			=> this.viewSelector.Items.OfType<ComboBoxItem>().Select(i => (string)i.Tag).ToList();

		public string CurrentCamera
		{
			get => (string)((ComboBoxItem)this.viewSelector.Items.CurrentItem).Tag;
			set
			{
				var ids = this.CameraIds;
				if (!ids.Contains(value))
					throw new ArgumentOutOfRangeException(nameof(value));

				var current = this.viewSelector.Items.OfType<ComboBoxItem>().FirstOrDefault(o => o.IsSelected);

				if (((string)current.Tag) == value)
					return; // No change.

				current.IsSelected = false;
				this.viewSelector.Items.OfType<ComboBoxItem>().FirstOrDefault(o => (string)o.Tag == value).IsSelected = true;
			}
		}

		/// <summary>
		/// Refresh the display state of the Core object.
		/// </summary>
		public void Refresh()
		{
			foreach (var kvp in m_3dObjects)
			{
				var o = kvp.Value;
				o.Transform = GetTransform(m_Core.GetBlock(kvp.Key));
			}
		}

		private void CreateCanvasContent()
		{
			// Remove Previous instance of the core.
			foreach (var o in m_3dObjects.Values)
				MyAnimatedObject.Children.Remove(o);

			m_3dObjects.Clear();

			// Create new instance.
			foreach (var b in m_Core.Blocks)
			{
				m_3dObjects.Add(b.Id, CreateVisuals(b));
			}

			foreach (var o in m_3dObjects.Values)
				MyAnimatedObject.Children.Add(o);
		}

		/// <summary>
		/// Set camera direction to ensure it's oriented in the center direction.
		/// </summary>
		private void SetCameraDirection()
			=> this.camera.LookDirection = new Vector3D(
				-this.camera.Position.X,
				-this.camera.Position.Y,
				-this.camera.Position.Z);

		private void RotationCoreStandardView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			int i = 0;
			foreach (var kvp in m_3dObjects)
			{
				var o = kvp.Value;
				o.Transform = GetTransform(m_Core.GetBlock(kvp.Key));

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

		private ModelVisual3D CreateVisuals(Engine.Structure.Block b)
		{
			Model3DGroup group = new Model3DGroup();
			StandardMaterializer materializer = new StandardMaterializer(m_Core);

			foreach (BlockFace face in b.Faces)
			{
				// Create visual object.
				GeometryModel3D model = new GeometryModel3D();
				group.Children.Add(model);

				// Get the face from the cube as the block face don't contain face Plane coordinates.
				CoreFace cubeFace = m_Core.GetFace(face.Id);

				Cartesian3dCoordinate center = materializer.GetCenter(b.Id, face.Id);
				var points = materializer.GetFaceVertices(cubeFace, center);

				model.Geometry = GetMesh(points, center);
				model.Material = GetBrush(face.Id);
				model.BackMaterial = GetBrush(null);
			}

			return new ModelVisual3D { Content = group };
		}

		/// <summary>
		/// Create the Mesh for a single block face.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="center"></param>
		/// <returns></returns>
		private MeshGeometry3D GetMesh(IList<Cartesian3dCoordinate> points, Cartesian3dCoordinate center)
		{
			// Mesh will be created by suming triangle sharing a common vertice in the center of the surface.
			MeshGeometry3D geo = new MeshGeometry3D();
			geo.Positions.Add(center.ToWpfPoint3D()); // Center will always be position 0
			geo.Positions.Add(points[0].ToWpfPoint3D());

			for (int i = 1; i < points.Count; ++i)
			{
				// Update positions.
				geo.Positions.Add(points[i].ToWpfPoint3D());

				geo.TriangleIndices.Add(0);     // Center
				geo.TriangleIndices.Add(i);     // Previous
				geo.TriangleIndices.Add(i + 1); // Current
			}

			// Last triangle link to begining
			geo.TriangleIndices.Add(0);                 // Center
			geo.TriangleIndices.Add(points.Count);      // Previous
			geo.TriangleIndices.Add(1);                 // Back to first

			return geo;
		}

		private DiffuseMaterial GetBrush(string faceId = null)
		{
			Color c = faceId switch
			{
				"F" => Color.FromRgb(0, 0, 255),
				"L" => Color.FromRgb(255, 105, 0),
				"R" => Color.FromRgb(255, 0, 0),
				"D" => Color.FromRgb(255, 255, 255),
				"B" => Color.FromRgb(0, 250, 0),
				"U" => Color.FromRgb(255, 255, 0),
				// Return black when not able to match a color.
				_ => Color.FromRgb(0, 0, 0),
			};

			return new DiffuseMaterial { Brush = new SolidColorBrush(c) };
		}

		private Transform3D GetTransform(Engine.Structure.Block b)
		{
			Transform3DGroup group = new Transform3DGroup();

			// Use Euler angles to provide correct orientation on every blocks.
			foreach (var rotation in b.Orientation.GetEulerAngles())
			{
				group.Children.Add(new RotateTransform3D
				{
					Rotation = new AxisAngleRotation3D
					{
						Axis = rotation.Axis.ToWpfVector3D(),
						Angle = -CoordinateConverter.ConvertRadianToDegree(rotation.Angle)
					}
				});
			}

			return group;
		}
	}
}
