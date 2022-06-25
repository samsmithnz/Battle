using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Map
{
    public static class MovementPossibileTiles
    {
        public static List<KeyValuePair<Vector3, int>> GetMovementPossibileTiles(string[,,] map, Vector3 sourceLocation, int range, int actionPoints)
        {
            List<KeyValuePair<Vector3, int>> results = new List<KeyValuePair<Vector3, int>>();
            List<Vector3> verifiedTiles = new List<Vector3>();
            //Loop through the action points from 1 to n
            for (int i = 1; i <= actionPoints; i++)
            {
                //Get a list of possible tiles for this range
                List<Vector3> possibleTiles = MapCore.GetMapArea(map, sourceLocation, range * i, false);
                foreach (Vector3 item in possibleTiles)
                {
                    if ( item == new Vector3(5,0,6))
                    {
                        int j = 5;
                    }
                    //Find a path to this target location
                    PathFindingResult result = PathFinding.FindPath(map, sourceLocation, item);
                    //If we could find a path, the cost of the path is within the range, and we haven't already identified a route to this tile, add it
                    if (result.Tiles.Count > 0 && result.Tiles[result.Tiles.Count - 1].TraversalCost <= range * i && !verifiedTiles.Contains(item))
                    {
                        //The current tile isn't tracked, add it as a verified tile
                        verifiedTiles.Add(item);
                        //Add the verified tile and the action points to reach the tile
                        results.Add(new KeyValuePair<Vector3, int>(item, i));
                    }
                }
            }
            return results;
        }

        //Convert the key value pair of location and action points to a list of locations (stripping off the action points)
        public static List<Vector3> ExtractVectorListFromKeyValuePair(List<KeyValuePair<Vector3, int>> list, int filter = 0)
        {
            List<Vector3> results = new List<Vector3>();
            foreach (KeyValuePair<Vector3, int> item in list)
            {
                if (filter == 0)
                {
                    results.Add(item.Key);
                }
                else if (item.Value == filter)
                {
                    results.Add(item.Key);
                }
            }
            return results;
        }
    }
}
