using Battle.Logic.Encounters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Battle.Logic.Map
{
    public static class MapCore
    {
        /// <summary>
        /// Initialize a map array
        /// </summary>
        /// <param name="xMax">x size</param>
        /// <param name="yMax">y size</param>
        /// <param name="zMax">z size</param>
        /// <param name="initialString">The initial string to initialize the map with - usually ""</param>
        /// <returns>The populated map/array</returns>
        public static string[,,] InitializeMap(int xMax, int yMax, int zMax, string initialString = "")
        {
            string[,,] map = new string[xMax, yMax, zMax];

            //Initialize the map
            for (int y = 0; y < yMax; y++)
            {
                for (int z = 0; z < zMax; z++)
                {
                    for (int x = 0; x < xMax; x++)
                    {
                        map[x, y, z] = initialString;
                    }
                }
            }

            return map;
        }

        //Test all locations within range to see if they are reachable
        public static List<Vector3> GetMapArea(string[,,] map, Vector3 sourceLocation, int range, bool lookingForFOV = true, bool includeSourceLocation = false)
        {
            int startingX = (int)sourceLocation.X;
            int startingY = (int)sourceLocation.Y;
            int startingZ = (int)sourceLocation.Z;

            //Use the range to find the borders in each primary direction from the starting location
            int minX = startingX - range;
            if (minX < 0)
            {
                minX = 0;
            }
            int maxX = startingX + range;
            if (maxX > map.GetLength(0) - 1)
            {
                maxX = map.GetLength(0) - 1;
            }
            int minY = startingY - range;
            if (minY < 0)
            {
                minY = 0;
            }
            int maxY = startingY + range;
            if (maxY > map.GetLength(1) - 1)
            {
                maxY = map.GetLength(1) - 1;
            }
            int minZ = startingZ - range;
            if (minZ < 0)
            {
                minZ = 0;
            }
            int maxZ = startingZ + range;
            if (maxZ > map.GetLength(2) - 1)
            {
                maxZ = map.GetLength(2) - 1;
            }

            //Get a list of all border squares
            HashSet<Vector3> borderTiles = new HashSet<Vector3>();
            //Add the top and bottom rows
            for (int x = minX; x <= maxX; x++)
            {
                borderTiles.Add(new Vector3(x, minY, minZ));
                borderTiles.Add(new Vector3(x, minY, maxZ));
                borderTiles.Add(new Vector3(x, maxY, minZ));
                borderTiles.Add(new Vector3(x, maxY, maxZ));
            }
            //Add the left and right sides
            for (int z = minZ; z < maxZ; z++)
            {
                borderTiles.Add(new Vector3(minX, minY, z));
                borderTiles.Add(new Vector3(maxX, minY, z));
                borderTiles.Add(new Vector3(minX, maxY, z));
                borderTiles.Add(new Vector3(maxX, maxY, z));
            }

            //For each border tile, draw a line from the starting point to the border
            HashSet<Vector3> results = new HashSet<Vector3>();
            foreach (Vector3 borderItem in borderTiles)
            {
                if (borderItem == new Vector3(5,1,9))
                {
                    int i = 5;
                }
                List<Vector3> singleLineCheck = FieldOfView.GetPointsOnLine(new Vector3(startingX, startingY, startingZ), borderItem);
                if (singleLineCheck.Count > 0 &&
                    singleLineCheck[singleLineCheck.Count - 1].X == startingX &&
                    singleLineCheck[singleLineCheck.Count - 1].Z == startingZ)
                {
                    //Reverse the list, so that items are in order from source to destination
                    singleLineCheck.Reverse();
                }
                double lineLength = GetLengthOfLine(singleLineCheck[0], singleLineCheck[singleLineCheck.Count - 1], 1);
                double lineSegment = lineLength / singleLineCheck.Count;
                double currentLength = 0;
                for (int i = 0; i < singleLineCheck.Count; i++)
                {
                    currentLength += lineSegment;
                    Vector3 fovItem = singleLineCheck[i];
                    if (fovItem.X >= 0 && fovItem.Y >= 0 && fovItem.Z >= 0)
                    {
                        //If we find an object, stop adding tiles
                        if (lookingForFOV && map[(int)fovItem.X, (int)fovItem.Y, (int)fovItem.Z] == MapObjectType.FullCover)
                        {
                            //Add the wall
                            results.Add(fovItem);
                            //Then break!
                            break;
                        }
                        else if ((int)fovItem.X == startingX && (int)fovItem.Y == startingY && (int)fovItem.Z == startingZ)
                        {
                            //Don't add this one, it's the origin/ where the character is looking from
                        }
                        else
                        {
                            results.Add(fovItem);
                        }
                    }
                    //We don't round, so this will extend the range a tiny part - but I think that is ok.
                    if (currentLength >= range)
                    {
                        break;
                    }
                }
            }
            if (includeSourceLocation)
            {
                results.Add(sourceLocation);
            }

            return results.ToList();
        }

        public static double GetLengthOfLine(Vector3 start, Vector3 end, int decimals = 0)
        {
            return Math.Round(Vector3.Distance(start, end), decimals);            
        }

        public static string GetMapString(string[,,] map, bool stripOutBlanks = false)
        {
            int xMax = map.GetLength(0);
            int yMax = map.GetLength(1);
            int zMax = map.GetLength(2);
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            for (int z = zMax - 1; z >= 0; z--)
            {
                StringBuilder sbLine = new StringBuilder();
                for (int x = 0; x < xMax; x++)
                {
                    //If there are multiple levels and underground on y=0, use y=1 value
                    if (yMax > 0 && map[x, 0, z] == MapObjectType.Underground)
                    {
                        if (map[x, 1, z] != "")
                        {
                            sbLine.Append(map[x, 1, z]);
                        }
                        else
                        {
                            sbLine.Append(MapObjectType.NoCoverYIs1); // •
                        }
                        sbLine.Append(' ');
                    }
                    else
                    {
                        if (map[x, 0, z] != "")
                        {
                            sbLine.Append(map[x, 0, z]);
                        }
                        else
                        {
                            sbLine.Append(MapObjectType.NoCoverYIs0); // ·
                        }
                        sbLine.Append(' ');
                    }
                }
                sbLine.Append(Environment.NewLine);
                //If there is nothing on the map line, don't display it.
                //This optimizes the ASCII maps to remove white space
                if (stripOutBlanks)
                {
                    int dotCount = sbLine.ToString().Split('·').Count() - 1;
                    if (dotCount < xMax)
                    {
                        sb.Append(sbLine.ToString());
                    }
                }
                else
                {
                    sb.Append(sbLine.ToString());
                }
            }
            return sb.ToString();
        }

        public static string[,,] ApplyListToMap(string[,,] map, List<Vector3> list, string tile)
        {
            foreach (Vector3 item in list)
            {
                //Check that the square is empty - we don't want to overwrite something that exists and only put a tile on an unused tile
                if (map[(int)item.X, (int)item.Y, (int)item.Z] == "")
                {
                    map[(int)item.X, (int)item.Y, (int)item.Z] = tile;
                }
                else if (map[(int)item.X, (int)item.Y, (int)item.Z] == MapObjectType.NoCoverYIs1)
                {
                    map[(int)item.X, (int)item.Y, (int)item.Z] = "◙";
                }
            }
            return map;
        }

        public static string[,,] ApplyListToExistingMap(string[,,] map, List<Vector3> list, string tile)
        {
            foreach (Vector3 item in list)
            {
                map[(int)item.X, (int)item.Y, (int)item.Z] = tile;
            }
            return map;
        }

        public static string GetMapStringWithItems(string[,,] map, List<Vector3> list)
        {
            string[,,] mapNew = MapCore.ApplyListToMap((string[,,])map.Clone(), list, "o");
            string mapString = MapCore.GetMapString(mapNew);
            return mapString;
        }

        public static string GetMapStringWithItemLayers(string[,,] map, List<Vector3> baseList, List<Vector3> overlayList)
        {
            string[,,] mapNew = MapCore.ApplyListToMap((string[,,])map.Clone(), baseList, "o");
            mapNew = MapCore.ApplyListToExistingMap(mapNew, overlayList, "O");

            string mapString = MapCore.GetMapString(mapNew);
            return mapString;
        }

        //public static string GetMapStringWithAIValuesFirst(string[,,] mapTemplate, List<KeyValuePair<Vector3, int>> list)
        //{
        //    string[,,] map = (string[,,])mapTemplate.Clone();
        //    foreach (KeyValuePair<Vector3, int> item in list)
        //    {
        //        map[(int)item.Key.X, (int)item.Key.Y, (int)item.Key.Z] = item.Value.ToString();
        //    }
        //    return MapCore.GetMapString(map);
        //}

        public static string GetMapStringWithAIValuesSecond(string[,,] mapTemplate, List<KeyValuePair<Vector3, AIAction>> list)
        {
            string[,,] map = (string[,,])mapTemplate.Clone();
            if (list != null)
            {
                foreach (KeyValuePair<Vector3, AIAction> item in list)
                {
                    map[(int)item.Key.X, (int)item.Key.Y, (int)item.Key.Z] = item.Value.Score.ToString();
                }
            }
            return MapCore.GetMapString(map, true);
        }

        public static string GetMapStringWithMapMask(string[,,] map, string[,,] mapMask)
        {
            int xMax = map.GetLength(0);
            //int yMax = map.GetLength(1);
            int zMax = map.GetLength(2);
            int xMaskMax = map.GetLength(0);
            //int yMaskMax = map.GetLength(1);
            int zMaskMax = map.GetLength(2);
            if (xMax != xMaskMax && zMax != zMaskMax)
            {
                throw new Exception("Mask map is not the same size as the parent map");
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            int y = 0;
            for (int z = zMax - 1; z >= 0; z--)
            {
                for (int x = 0; x < xMax; x++)
                {
                    if (mapMask[x, y, z] != "")
                    {
                        sb.Append(mapMask[x, y, z]);
                    }
                    else
                    {
                        if (map[x, y, z] != "")
                        {
                            sb.Append(map[x, y, z]);
                        }
                        else
                        {
                            sb.Append(MapObjectType.NoCoverYIs0);
                        }
                    }
                    sb.Append(' ');
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public static List<Vector3> FindAdjacentTile(string[,,] map, Vector3 currentLocation, string tileToFind)
        {
            int width = map.GetLength(0);
            //int height = map.GetLength(1);
            int breadth = map.GetLength(2);
            List<Vector3> result = new List<Vector3>();

            //Make adjustments to ensure that the search doesn't go off the edges of the map
            int xMin = Convert.ToInt32(currentLocation.X) - 1;
            if (xMin < 0)
            {
                xMin = 0;
            }
            int xMax = Convert.ToInt32(currentLocation.X) + 1;
            if (xMax > width - 1)
            {
                xMax = width - 1;
            }
            int zMin = Convert.ToInt32(currentLocation.Z) - 1;
            if (zMin < 0)
            {
                zMin = 0;
            }
            int zMax = Convert.ToInt32(currentLocation.Z) + 1;
            if (zMax > breadth - 1)
            {
                zMax = breadth - 1;
            }

            //Get possible tiles, within constraints of map, including only square titles from current position (not diagonally)
            if (map[Convert.ToInt32(currentLocation.X), 0, zMax] == tileToFind)
            {
                result.Add(new Vector3(currentLocation.X, 0f, zMax));
            }
            if (map[xMax, 0, Convert.ToInt32(currentLocation.Z)] == tileToFind)
            {
                result.Add(new Vector3(xMax, 0f, currentLocation.Z));
            }
            if (map[Convert.ToInt32(currentLocation.X), 0, zMin] == tileToFind)
            {
                result.Add(new Vector3(currentLocation.X, 0f, zMin));
            }
            if (map[xMin, 0, Convert.ToInt32(currentLocation.Z)] == tileToFind)
            {
                result.Add(new Vector3(xMin, 0f, currentLocation.Z));
            }
            return result;
        }
    }
}
