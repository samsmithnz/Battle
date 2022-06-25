using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Map
{
    public static class PathFinding
    {
        private static int _width;
        private static int _height;
        private static int _breadth;
        private static MapTile[,,] _tiles;
        private static Vector3 _endLocation;

        /// <summary>
        /// Attempts to find a path from the start location to the end location based on the supplied SearchParameters
        /// </summary>
        /// <returns>A List of Points representing the path. If no path was found, the returned list is empty.</returns>
        public static PathFindingResult FindPath(string[,,] map, Vector3 startLocation, Vector3 endLocation)
        {
            _endLocation = endLocation;
            InitializeTiles(map);
            MapTile startTile = _tiles[(int)startLocation.X, (int)startLocation.Y, (int)startLocation.Z];
            startTile.State = TileState.Open;
            MapTile endTile = _tiles[(int)endLocation.X, (int)endLocation.Y, (int)endLocation.Z];

            // The start tile is the first entry in the 'open' list
            PathFindingResult result = new PathFindingResult();
            bool success = Search(startTile, endTile);
            if (success)
            {
                // If a path was found, follow the parents from the end tile to build a list of locations
                MapTile tile = endTile;
                while (tile.ParentTile != null)
                {
                    result.Tiles.Add(tile);
                    result.Path.Add(tile.Location);
                    tile = tile.ParentTile;
                }

                // Reverse the list so it's in the correct order when returned
                result.Path.Reverse();
                result.Tiles.Reverse();
            }

            return result;
        }

        /// <summary>
        /// Builds the tile grid from a simple grid of booleans indicating areas which are and aren't walkable
        /// </summary>
        /// <param name="map">A boolean representation of a grid in which true = walkable and false = not walkable</param>
        private static void InitializeTiles(string[,,] map)
        {
            _width = map.GetLength(0);
            _height = map.GetLength(1);
            _breadth = map.GetLength(2);
            _tiles = new MapTile[_width, _height, _breadth];
            for (int y = 0; y < _height; y++)
            {
                for (int z = 0; z < _breadth; z++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        _tiles[x, y, z] = new MapTile(x, y, z, map[x, y, z], _endLocation);
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to find a path to the destination tile using <paramref name="currentTile"/> as the starting location
        /// </summary>
        /// <param name="currentTile">The tile from which to find a path</param>
        /// <returns>True if a path to the destination has been found, otherwise false</returns>
        private static bool Search(MapTile currentTile, MapTile endTile)
        {
            // Set the current tile to Closed since it cannot be traversed more than once
            currentTile.State = TileState.Closed;
            List<MapTile> nextTiles = GetAdjacentWalkableTiles(currentTile);

            // Sort by F-value so that the shortest possible routes are considered first
            nextTiles.Sort((tile1, tile2) => tile1.F.CompareTo(tile2.F));
            foreach (MapTile nextTile in nextTiles)
            {
                if (nextTile.Location == new Vector3(5, 0, 4))
                {
                    int i = 4;
                }
                // Check whether the end tile has been reached
                if (nextTile.Location == endTile.Location)
                {
                    return true;
                }
                else
                {
                    // If not, check the next set of tiles
                    if (Search(nextTile, endTile)) // Note: Recurses back into Search(Tile)
                    {
                        return true;
                    }
                }
            }

            // The method returns false if this path leads to be a dead end
            return false;
        }

        /// <summary>
        /// Returns any tiles that are adjacent to <paramref name="fromTile"/> and may be considered to form the next step in the path
        /// </summary>
        /// <param name="fromTile">The tile from which to return the next possible tiles in the path</param>
        /// <returns>A list of next possible tiles in the path</returns>
        private static List<MapTile> GetAdjacentWalkableTiles(MapTile fromTile)
        {
            List<MapTile> walkableTiles = new List<MapTile>();
            IEnumerable<Vector3> nextLocations = GetAdjacentLocations(fromTile.Location);

            foreach (Vector3 location in nextLocations)
            {
                int x = (int)location.X;
                int y = (int)location.Y;
                int z = (int)location.Z;
                if (location == new Vector3(5, 0, 5))
                {
                    int i = 5;
                }

                // Stay within the grid's boundaries
                if (x < 0 || x >= _width || z < 0 || z >= _breadth || y < 0 || y >= _height)
                {
                    continue;
                }

                MapTile tile = _tiles[x, y, z];

                // Ignore non-walkable tiles
                if (tile.TileType != "" && tile.TileType != MapObjectType.Ladder)
                {
                    continue;
                }

                // Ignore already-closed tiles
                if (tile.State == TileState.Closed)
                {
                    continue;
                }

                //// Already-open tiles are only added to the list if their G-value is lower going via this route.
                if (tile.State == TileState.Open)
                {
                    //float traversalCost = MapTile.GetTraversalCost(tile.Location, tile.ParentTile.Location);
                    //float gTemp = fromTile.G + traversalCost;
                    //if (gTemp < tile.G)
                    //{
                    //    tile.ParentTile = fromTile;
                    //    walkableTiles.Add(tile);
                    //}
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    tile.ParentTile = fromTile;
                    tile.State = TileState.Open;
                    walkableTiles.Add(tile);
                }
            }

            return walkableTiles;
        }

        /// <summary>
        /// Returns the eight locations immediately adjacent (orthogonally and diagonally) to <paramref name="fromLocation"/>
        /// </summary>
        /// <param name="fromLocation">The location from which to return all adjacent points</param>
        /// <returns>The locations as an IEnumerable of Points</returns>
        private static IEnumerable<Vector3> GetAdjacentLocations(Vector3 fromLocation)
        {
            List<Vector3> list = new List<Vector3>();
            list.AddRange(new Vector3[]
                {
                    new Vector3(fromLocation.X - 1, fromLocation.Y, fromLocation.Z - 1),
                    new Vector3(fromLocation.X - 1, fromLocation.Y, fromLocation.Z - 0),
                    new Vector3(fromLocation.X - 1, fromLocation.Y, fromLocation.Z + 1),
                    new Vector3(fromLocation.X - 0, fromLocation.Y, fromLocation.Z + 1),
                    new Vector3(fromLocation.X + 1, fromLocation.Y, fromLocation.Z + 1),
                    new Vector3(fromLocation.X + 1, fromLocation.Y, fromLocation.Z + 0),
                    new Vector3(fromLocation.X + 1, fromLocation.Y, fromLocation.Z - 1),
                    new Vector3(fromLocation.X + 0, fromLocation.Y, fromLocation.Z - 1)
                });
            //Add the top and bottom
            if (fromLocation.Y == 1)
            {
                list.Add(new Vector3(fromLocation.X, fromLocation.Y - 1, fromLocation.Z));
            }
            else if (fromLocation.Y == 0)
            {
                list.Add(new Vector3(fromLocation.X, fromLocation.Y + 1, fromLocation.Z));
            }
            return list;
        }
    }
}
