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
using Twisty.Engine.Structure.Analysis;
using Twisty.Runner.Models.Model3d;
using Twisty.Runner.ViewModels;
using Twisty.Runner.Wpf.Model3d;

namespace Twisty.Runner.Views
{
	/// <summary>
	/// Interaction logic for StandardView.xaml
	/// </summary>
	public partial class RotationCoreStandardView : UserControl
	{
		#region Binding Properties

		private static readonly DependencyProperty ObjectStructuresProperty =
			DependencyProperty.Register("ObjectStructures",
				typeof(Core3d),
				typeof(RotationCoreStandardView),
				new FrameworkPropertyMetadata(
					null,
					FrameworkPropertyMetadataOptions.AffectsRender,
					(DependencyObject defectImageControl, DependencyPropertyChangedEventArgs eventArgs) =>
						((RotationCoreStandardView)defectImageControl).ObjectStructures = eventArgs.NewValue as Core3d
				));

		private static readonly DependencyProperty AlterationsProperty =
			DependencyProperty.Register("Alterations",
				typeof(CoreAlterations),
				typeof(RotationCoreStandardView),
				new FrameworkPropertyMetadata(
					null,
					FrameworkPropertyMetadataOptions.AffectsRender,
					(DependencyObject defectImageControl, DependencyPropertyChangedEventArgs eventArgs) =>
						((RotationCoreStandardView)defectImageControl).Alterations = (CoreAlterations)eventArgs.NewValue
				));

		#endregion Binding Properties

		private IDictionary<string, Block3dObject> m_3dObjects;

		public RotationCoreStandardView()
		{
			m_3dObjects = new Dictionary<string, Block3dObject>();

			InitializeComponent();

			this.DataContext.CameraPosition = new Point3D(6.0, -3.0, 3.0);

			this.showAlterations.Checked += OnShowAlterationsChange;
			this.showAlterations.Unchecked += OnShowAlterationsChange;
		}

		private void OnShowAlterationsChange(object sender, RoutedEventArgs e)
		{
			this.RefreshColors();
		}

		#region Public Properties

		public new RotationCoreStandardViewModel DataContext
			=> (RotationCoreStandardViewModel)base.DataContext;

		public Core3d ObjectStructures
		{
			get => this.GetValue(ObjectStructuresProperty) as Core3d;
			set
			{
				this.SetValue(ObjectStructuresProperty, value);
				this.CreateCanvasContent();
			}
		}

		public CoreAlterations Alterations
		{
			get => (CoreAlterations)this.GetValue(AlterationsProperty);
			set
			{
				this.SetValue(AlterationsProperty, value);
				this.RefreshRotations();
				this.RefreshColors();
			}
		}

		public string CurrentCamera
		{
			get => this.DataContext.ActiveCameraId;
			set { this.DataContext.ActiveCameraId = value; }
		}

		#endregion Public Properties

		#region Private Methods

		private void CreateCanvasContent()
		{
			// Remove Previous instance of the core.
			foreach (var o in m_3dObjects.Values)
				AnimatedCore.Children.Remove(o.Visual);

			m_3dObjects.Clear();

			if (this.ObjectStructures is null)
				return;

			// Create new instance.
			foreach (Core3dObject o in this.ObjectStructures.Objects)
			{
				var v = new Block3dObject(o);

				m_3dObjects.Add(o.Id, v);
				AnimatedCore.Children.Add(v.Visual);
			}
		}

		/// <summary>
		/// Refresh the display state of the Core object.
		/// </summary>
		private void RefreshRotations()
		{
			foreach (var o in this.m_3dObjects.Values)
			{
				o.ApplyRotation(this.Alterations.GetRotations(o.Key));
			}
		}

		/// <summary>
		/// Refresh the display state of the Core object.
		/// </summary>
		private void RefreshColors()
		{
			if (this.showAlterations.IsChecked ?? false)
			{
				foreach (var o in this.m_3dObjects.Values)
				{
					Color c = this.Alterations.GetAlteration(o.Key) switch
					{
						AlterationType.Orientation => Color.FromRgb(100, 100, 100),
						AlterationType.Position => Color.FromRgb(0, 0, 0),
						_ => Color.FromRgb(byte.MaxValue, byte.MaxValue, byte.MaxValue)
					};
					o.SetColor(c);
				}
			}
			else
			{
				foreach (var o in this.m_3dObjects.Values)
					o.ResetColor();
			}
		}

		#endregion Private Methods
	}
}
