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
            RotationCore core = coreTypeId switch
            {
                "Rubik[2]" => new RubikCube(2),
                "Rubik[3]" or "Rubik[3]-bandageA" => new RubikCube(3),
                "Skewb" => new SkewbCube(),
                _ => null,
            };

            if (coreTypeId == "Rubik[3]-bandageA")
            {
                core.RotationValidators = [GetBandagagesForZCubeA(core)];
			}

            return core;
        }

        private BandagesCollection GetBandagagesForZCubeA(RotationCore core)
        {
            var bandages = new BandagesCollection(core);

            bandages.Band($"C{RubikCube.ID_FACE_UP}{RubikCube.ID_FACE_FRONT}{RubikCube.ID_FACE_RIGHT}",
                [
                    $"CF_{RubikCube.ID_FACE_UP}", $"CF_{RubikCube.ID_FACE_RIGHT}", $"CF_{RubikCube.ID_FACE_FRONT}",
                    $"E_{RubikCube.ID_FACE_UP}{RubikCube.ID_FACE_RIGHT}", $"E_{RubikCube.ID_FACE_UP}{RubikCube.ID_FACE_FRONT}", $"E_{RubikCube.ID_FACE_FRONT}{RubikCube.ID_FACE_RIGHT}"
                ]);

            return bandages;
        }
    }
}
