using System;
using System.Collections.Generic;
using System.Text;
using Twisty.Engine.Structure.Rubiks;
using Twisty.Engine.Structure.Skewb;

namespace Twisty.Engine.Structure
{
    /// <summary>
    /// Factory providing RotationCore manipulation operations.
    /// </summary>
    public class CoreFactory
    {
        /// <summary>
        /// Create the ROtationCOre for the provide core type id.
        /// </summary>
        /// <param name="coreTypeId">Id of the core type to create.</param>
        /// <returns>Newly created RotationCore.</returns>
        public RotationCore CreateCore(string coreTypeId)
        {
            return coreTypeId switch
            {
                "Rubik[2]" => new RubikCube(2),
                "Rubik[3]" => new RubikCube(3),
                "Rubik[3]-bandageA" => new RubikCube(3) { RotationValidators = [ GetBandagagesForZCubeA() ] },
                "Skewb" => new SkewbCube(),
                _ => null,
            };
        }

        private BandagesCollection<string> GetBandagagesForZCubeA()
        {
            var bandages = new BandagesCollection<string>();

            bandages.Band($"C{RubikCube.ID_FACE_UP}{RubikCube.ID_FACE_FRONT}{RubikCube.ID_FACE_RIGHT}",
                [
                    $"CF_{RubikCube.ID_FACE_UP}", $"CF_{RubikCube.ID_FACE_RIGHT}", $"CF_{RubikCube.ID_FACE_FRONT}",
                    $"E_{RubikCube.ID_FACE_UP}{RubikCube.ID_FACE_RIGHT}", $"E_{RubikCube.ID_FACE_UP}{RubikCube.ID_FACE_FRONT}", $"E_{RubikCube.ID_FACE_FRONT}{RubikCube.ID_FACE_RIGHT}"
                ]);

            return bandages;
        }
    }
}
