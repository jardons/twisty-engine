using Twisty.Engine.Structure;

namespace Twisty.Engine.Operations
{
    /// <summary>
    /// Class describing an operation to perform a rotation of all blocks aroung an axis.
    /// </summary>
    public class LayerOperation : IOperation
    {
        /// <summary>
        /// Create a new LayerOperation.
        /// </summary>
        /// <param name="axisId">Id of the axis around which the rotation is executed.</param>
        /// <param name="theta">Rotation angle to execute in radians. A positive value will indicate a clockwise rotation.</param>
        /// <param name="layerIndex">Index of the layer to rotate. Layers are identified starting at 0 for the external one.</param>
        public LayerOperation(string axisId, double theta, int layerIndex)
        {
            this.AxisId = axisId;
            this.Theta = theta;
            this.Interval = new LayerInterval(layerIndex);
        }

        /// <summary>
        /// Create a new LayerOperation.
        /// </summary>
        /// <param name="axisId">Id of the axis around which the rotation is executed.</param>
        /// <param name="theta">Rotation angle to execute in radians. A positive value will indicate a clockwise rotation.</param>
        /// <param name="interval">Plane interval delimiting the layer on which the rotation will be performed.</param>
        public LayerOperation(string axisId, double theta, LayerInterval interval)
        {
            this.AxisId = axisId;
            this.Theta = theta;
            this.Interval = interval;
        }

        /// <summary>
        /// Gets the Id of the axis around which the rotation is executed.
        /// </summary>
        public string AxisId { get; }

        /// <summary>
        /// Gets the rotation angle to execute in radians. A positive value will indicate a clockwise rotation.
        /// </summary>
        public double Theta { get; }

        /// <summary>
        /// Gets the Plane interval delimiting the layer on which the rotation will be performed.
        /// </summary>
        public LayerInterval Interval { get; }

        #region IOperation

        /// <summary>
        /// Execute the current operation on the provided IRotatable core.
        /// </summary>
        /// <param name="core">IRotatable core on which the operation is executed.</param>
        /// <returns>A boolean indicating if whether the operation was succesful or not.</returns>
        public bool ExecuteOn(IRotatable core)
        {
            try
            {
                core.RotateAround(core.GetAxis(AxisId), this.Theta);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion IOperation
    }
}
