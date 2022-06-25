using Battle.Logic.Map;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Characters
{
    public class CoverUtility
    {
        public static string[,,] InitializeMap(int xMax, int yMax, int zMax, List<Vector3> highCoverLocations, List<Vector3> lowCoverLocations)
        {
            string[,,] map = new string[xMax, yMax, zMax];

            //Initialize the map
            int y = 0;
            for (int z = 0; z < zMax; z++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    map[x, y, z] = "";
                }
            }

            //assign cover locations, (currently just "■", for "high cover wall")
            if (highCoverLocations != null && highCoverLocations.Count > 0)
            {
                foreach (Vector3 item in highCoverLocations)
                {
                    map[(int)item.X, (int)item.Y, (int)item.Z] = MapObjectType.FullCover;
                }
            }
            //assign cover locations, (currently just "□", for "low cover wall")
            if (lowCoverLocations != null && lowCoverLocations.Count > 0)
            {
                foreach (Vector3 item in lowCoverLocations)
                {
                    map[(int)item.X, (int)item.Y, (int)item.Z] = MapObjectType.HalfCover;
                }
            }

            return map;
        }

    }
}
