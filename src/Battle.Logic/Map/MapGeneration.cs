using Battle.Logic.Utility;
using System;

namespace Battle.Logic.Map
{
    public static class MapGeneration
    {
        public static string[,,] GenerateRandomMap(string[,,] map, int probOfMapBeingBlocked)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            int breadth = map.GetLength(2);
            int y = 0;
            for (int z = 0; z < breadth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (((x != 0 && z != 0) || (x != width - 1 && z != height - 1)) && probOfMapBeingBlocked > RandomNumber.GenerateRandomNumber(1, 100))
                    {
                        map[x, y, z] = MapObjectType.FullCover;
                    }
                }
            }
            return map;
        }

        public static void DebugPrintOutMap(string[,,] map)
        {
            int width = map.GetLength(0);
            //int height = map.GetLength(1);
            int breadth = map.GetLength(2);
            int y = 0;
            for (int z = 0; z < breadth; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[x, y, z] != "")
                    {
                        Console.WriteLine(" this.map[" + x + ", " + z + "] = " + map[x, y, z] + ";");
                    }
                }
            }
        }


    }
}
