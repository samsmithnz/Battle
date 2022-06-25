using Battle.Logic.Game;
using Battle.Logic.Map;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Characters
{
    public static class CharacterCover
    {
        /// <summary>
        /// Calculate if the player is in cover.
        /// </summary>
        /// <param name="map">string[,,] map array</param>
        /// <param name="defenderPosition">defender location</param>
        /// <param name="attackers">list of attacker characters</param>
        /// <returns></returns>
        public static CoverState CalculateCover(string[,,] map, Vector3 defenderPosition, List<Character> attackers)
        {
            List<Vector3> attackerLocations = new List<Vector3>();
            if (attackers != null)
            {
                foreach (Character character in attackers)
                {
                    attackerLocations.Add(character.Location);
                }
            }
            return CalculateCover(map, defenderPosition, attackerLocations);
        }

        /// <summary>
        /// Calculate if the player is in cover.
        /// </summary>
        /// <param name="map">string[,,] map array</param>
        /// <param name="defenderPosition">defender location</param>
        /// <param name="attackerLocations">List of attacker locations</param>
        /// <returns></returns>
        public static CoverState CalculateCover(string[,,] map, Vector3 defenderPosition, List<Vector3> attackerLocations)
        {
            CoverState result = new CoverState();
            List<Vector3> coverTiles = FindAdjacentCover(map, defenderPosition);
            int coverLineNorth = -1;
            int coverLineEast = -1;
            int coverLineSouth = -1;
            int coverLineWest = -1;

            //if (coverTiles.Count == 0)
            //{
            //    result.InFullCover = false;
            //    result.InHalfCover = false;
            //    if (attackerLocations.Count > 0)
            //    {
            //        result.IsFlanked = true; //ignore cover/ the character isn't in cover
            //    }
            //    return result;
            //}
            //else
            //{
            // Work out where the cover is relative to the player
            foreach (Vector3 coverTileItem in coverTiles)
            {
                if (defenderPosition.X < coverTileItem.X)
                {
                    if (map[(int)defenderPosition.X + 1, (int)defenderPosition.Y, (int)defenderPosition.Z] == MapObjectType.FullCover)
                    {
                        result.InEastFullCover = true;
                        result.InFullCover = true;
                    }
                    else
                    {
                        result.InEastHalfCover = true;
                        result.InHalfCover = true;
                    }
                    coverLineEast = Convert.ToInt32(coverTileItem.X) - 0;
                }
                if (defenderPosition.X > coverTileItem.X)
                {
                    if (map[(int)defenderPosition.X - 1, (int)defenderPosition.Y, (int)defenderPosition.Z] == MapObjectType.FullCover)
                    {
                        result.InWestFullCover = true;
                        result.InFullCover = true;
                    }
                    else
                    {
                        result.InWestHalfCover = true;
                        result.InHalfCover = true;
                    }
                    coverLineWest = Convert.ToInt32(coverTileItem.X) + 0;
                }
                if (defenderPosition.Z < coverTileItem.Z)
                {
                    if (map[(int)defenderPosition.X, (int)defenderPosition.Y, (int)defenderPosition.Z + 1] == MapObjectType.FullCover)
                    {
                        result.InNorthFullCover = true;
                        result.InFullCover = true;
                    }
                    else
                    {
                        result.InNorthHalfCover = true;
                        result.InHalfCover = true;
                    }
                    coverLineNorth = Convert.ToInt32(coverTileItem.Z) - 0;
                }
                if (defenderPosition.Z > coverTileItem.Z)
                {
                    if (map[(int)defenderPosition.X, (int)defenderPosition.Y, (int)defenderPosition.Z - 1] == MapObjectType.FullCover)
                    {
                        result.InSouthFullCover = true;
                        result.InFullCover = true;
                    }
                    else
                    {
                        result.InSouthHalfCover = true;
                        result.InHalfCover = true;
                    }
                    coverLineSouth = Convert.ToInt32(coverTileItem.Z) + 0;
                }
            }
            //}

            if (attackerLocations != null && attackerLocations.Count > 0)
            {
                //Work out where the enemy is relative to the cover
                foreach (Vector3 enemyItem in attackerLocations)
                {
                    //Now check over all of the levels of cover

                    //Enemy is located NorthEast
                    if (enemyItem.Z >= defenderPosition.Z && enemyItem.X >= defenderPosition.X)
                    {
                        if (!result.InNorthFullCover && !result.InNorthHalfCover && !result.InEastFullCover && !result.InEastHalfCover) //No cover in North or East = always flanked by Northeast Enenmy
                        {
                            result.IsFlanked = true;
                            break;
                        }
                        else if ((result.InNorthFullCover || result.InNorthHalfCover) && enemyItem.Z <= coverLineNorth && !result.InEastFullCover) //There is cover in the North, but the enemy is past it + no East cover
                        {
                            result.IsFlanked = true;
                            break;
                        }
                        else if ((result.InEastFullCover || result.InEastHalfCover) && enemyItem.X <= coverLineEast && !result.InNorthFullCover) //There is cover in the East, but the enemy is past it + no North cover
                        {
                            result.IsFlanked = true;
                            break;
                        }
                    }

                    //Enemy is located NorthWest
                    if (enemyItem.Z >= defenderPosition.Z && enemyItem.X <= defenderPosition.X)
                    {
                        if (!result.InNorthFullCover && !result.InNorthHalfCover && !result.InWestFullCover && !result.InWestHalfCover)
                        {
                            result.IsFlanked = true;
                            break;
                        }
                        else if ((result.InNorthFullCover || result.InNorthHalfCover) && enemyItem.Z <= coverLineNorth && !result.InWestFullCover)
                        {
                            result.IsFlanked = true;
                            break;
                        }
                        else if ((result.InWestFullCover || result.InWestHalfCover) && enemyItem.X >= coverLineWest && !result.InNorthFullCover)
                        {
                            result.IsFlanked = true;
                            break;
                        }
                    }

                    //Enemy is located SouthEast
                    if (enemyItem.Z <= defenderPosition.Z && enemyItem.X >= defenderPosition.X)
                    {
                        if (!result.InSouthFullCover && !result.InSouthHalfCover && !result.InEastFullCover && !result.InEastHalfCover)
                        {
                            result.IsFlanked = true;
                            break;
                        }
                        else if ((result.InSouthFullCover || result.InSouthHalfCover) && enemyItem.Z >= coverLineSouth && !result.InEastFullCover)
                        {
                            result.IsFlanked = true;
                            break;
                        }
                        else if ((result.InEastFullCover || result.InEastHalfCover) && enemyItem.X <= coverLineEast && !result.InSouthFullCover)
                        {
                            result.IsFlanked = true;
                            break;
                        }
                    }

                    //Enemy is located SouthWest
                    if (enemyItem.Z <= defenderPosition.Z && enemyItem.X <= defenderPosition.X)
                    {
                        if (!result.InSouthFullCover && !result.InSouthHalfCover && !result.InWestFullCover && !result.InWestHalfCover)
                        {
                            result.IsFlanked = true;
                            break;
                        }
                        else if ((result.InSouthFullCover || result.InSouthHalfCover) && enemyItem.Z >= coverLineSouth && !result.InWestFullCover)
                        {
                            result.IsFlanked = true;
                            break;
                        }
                        else if ((result.InWestFullCover || result.InWestHalfCover) && enemyItem.X >= coverLineWest && !result.InSouthFullCover)
                        {
                            result.IsFlanked = true;
                            break;
                        }
                    }
                }
            }

            //If the player is standing in the open, they are flanked
            if (!result.InFullCover && !result.InHalfCover)
            {
                result.IsFlanked = true;
            }

            return result;
        }

        public static bool RefreshCoverStates(Mission mission)
        {
            foreach (Team team in mission.Teams)
            {
                List<Vector3> opponents = GetOpponentCharacterLocations(mission.Teams, team.Name);

                foreach (Character character in team.Characters)
                {
                    character.CoverState = CharacterCover.CalculateCover(mission.Map, character.Location, opponents);
                }
            }
            return true;
        }

        private static List<Vector3> GetOpponentCharacterLocations(List<Team> teams, string currentTeamName)
        {
            List<Vector3> opponents = new List<Vector3>();
            foreach (Team team in teams)
            {
                if (team.Name != currentTeamName)
                {
                    foreach (Character character in team.Characters)
                    {
                        opponents.Add(character.Location);
                    }
                }
            }
            return opponents;
        }

        /// <summary>
        /// Look at adjacent squares for cover
        /// </summary>
        /// <returns>A List of Vector3 objects for each item of cover</returns>
        private static List<Vector3> FindAdjacentCover(string[,,] map, Vector3 currentLocation)
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
            if (map[Convert.ToInt32(currentLocation.X), 0, zMax] == MapObjectType.FullCover || map[Convert.ToInt32(currentLocation.X), 0, zMax] == MapObjectType.HalfCover)
            {
                result.Add(new Vector3(currentLocation.X, 0f, zMax));
            }
            if (map[xMax, 0, Convert.ToInt32(currentLocation.Z)] == MapObjectType.FullCover || map[xMax, 0, Convert.ToInt32(currentLocation.Z)] == MapObjectType.HalfCover)
            {
                result.Add(new Vector3(xMax, 0f, currentLocation.Z));
            }
            if (map[Convert.ToInt32(currentLocation.X), 0, zMin] == MapObjectType.FullCover || map[Convert.ToInt32(currentLocation.X), 0, zMin] == MapObjectType.HalfCover)
            {
                result.Add(new Vector3(currentLocation.X, 0f, zMin));
            }
            if (map[xMin, 0, Convert.ToInt32(currentLocation.Z)] == MapObjectType.FullCover || map[xMin, 0, Convert.ToInt32(currentLocation.Z)] == MapObjectType.HalfCover)
            {
                result.Add(new Vector3(xMin, 0f, currentLocation.Z));
            }
            return result;
        }
    }
}
