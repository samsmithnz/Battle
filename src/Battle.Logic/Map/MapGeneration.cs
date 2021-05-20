﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Battle.Logic.Utility;

namespace Battle.Logic.Map
{
    public static class MapGeneration
    {
        public static string[,] GenerateRandomMap(string[,] map, int xMax, int zMax, int probOfMapBeingBlocked)
        {
            for (int z = 0; z < zMax; z++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    if (((x != 0 && z != 0) || (x != xMax - 1 && z != zMax - 1)) && probOfMapBeingBlocked > RandomNumber.GenerateRandomNumber(1, 100))
                    {
                        map[x, z] = "■";
                    }
                }
            }
            return map;
        }

        public static void DebugPrintOutMap(string[,] map, int xMax, int zMax)
        {
            for (int z = 0; z < zMax; z++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    if (map[x, z] != "")
                    {
                        Console.WriteLine(" this.map[" + x + ", " + z + "] = "+ map[x, z] + ";");
                    }
                }
            }
        }

        public static string GetMapString(string[,] map, int xMax, int zMax)
        {
            StringBuilder sb = new();
            sb.Append(Environment.NewLine);
            for (int z = zMax-1; z >= 0; z--)
            {
                for (int x = 0; x < xMax; x++)
                {
                    if (map[x, z] != "")
                    {
                        sb.Append(map[x, z] + " ");
                    }
                    else
                    {
                        sb.Append("□ ");
                    }
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public static string[,] ApplyListToMap(string[,] map, List<Vector3> list, string tile)
        {
            foreach (Vector3 item in list)
            {
                if (map[(int)item.X,(int)item.Z] == "")
                {
                    map[(int)item.X, (int)item.Z] = tile;
                }                
            }

            return map;
        }

    }
}
