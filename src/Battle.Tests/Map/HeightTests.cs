using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class HeightTests
    {
        [TestMethod]
        public void HeightTest()
        {
            //Arrange
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(10, 2, 10, ".")
            };
            //Move the top 4 rows of the map to be on Y=1, instead of Y=0
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        if (z <= 5 && y == 0)
                        {
                            mission.Map[x, y, z] = "";
                        }
                        else if (z <= 5 && y == 1)
                        {
                            mission.Map[x, y, z] = "";
                        }
                        else if (z > 5 && y == 0)
                        {
                            mission.Map[x, y, z] = MapObjectType.Underground;
                        }
                        else if (z > 5 && y == 1)
                        {
                            mission.Map[x, y, z] = "";
                        }
                    }
                }
            }
            mission.Map[5, 0, 5] = MapObjectType.Ladder;
            Character fred = CharacterPool.CreateFredHero(mission.Map, new(1, 0, 1));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new(8, 1, 8));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            mission.Teams.Add(team1);
            mission.Teams.Add(team2);

            //Assert
            string currentMap = MapCore.GetMapString(mission.Map, false);
            string expectedMap = @"
• • • • • • • • • • 
• • • • • • • • P • 
• • • • • • • • • • 
• • • • • • • • • • 
· · · · · ╬ · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· P · · · · · · · · 
· · · · · · · · · · 
";
            Assert.AreEqual(expectedMap, currentMap);
        }

        [TestMethod]
        public void MoveUpLadderTest()
        {
            //Arrange
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(10, 2, 10, ".")
            };
            //Move the top 4 rows of the map to be on Y=1, instead of Y=0
            for (int y = 0; y < 2; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    for (int z = 0; z < 10; z++)
                    {
                        if (z <= 5 && y == 0)
                        {
                            mission.Map[x, y, z] = "";
                        }
                        else if (z > 5 && y == 0)
                        {
                            //Underground
                            mission.Map[x, y, z] = MapObjectType.Underground;
                        }
                        else if (y == 1)
                        {
                            mission.Map[x, y, z] = "";
                        }
                    }
                }
            }
            mission.Map[5, 0, 5] = MapObjectType.Ladder;
            Character fred = CharacterPool.CreateFredHero(mission.Map, new(5, 0, 3));
            fred.MobilityRange = 3;
            fred.ActionPointsCurrent = 1;
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new(8, 1, 8));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            mission.Teams.Add(team1);
            mission.Teams.Add(team2);
            Vector3 destination = new(5, 1, 7);

            //Act
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(mission.Map, fred.Location, fred.MobilityRange, fred.ActionPointsCurrent);
            PathFindingResult pathFindingResult = PathFinding.FindPath(mission.Map, fred.Location, destination);
            //List<MovementAction> movementResults = CharacterMovement.MoveCharacter(mission.Map, fred, pathFindingResult, team1, team2, mission.RandomNumbers);

            //Assert
            Assert.AreEqual(27,movementPossibileTiles.Count);
            string currentMap = MapCore.GetMapString(mission.Map, false);
            string expectedMap = @"
• • • • • • • • • • 
• • • • • • • • P • 
• • • • • • • • • • 
• • • • • • • • • • 
· · · · · ╬ · · · · 
· · · · · · · · · · 
· · · · · P · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
";
            Assert.AreEqual(expectedMap, currentMap);

            string mapPossibleTilesCurrent = MapCore.GetMapStringWithItems(mission.Map, MovementPossibileTiles.ExtractVectorListFromKeyValuePair(movementPossibileTiles));
            string mapPossibleTilesExpected = @"
• • • • • • • • • • 
• • • • • • • • P • 
• • • • • • • • • • 
• • • • • • • • • • 
· · · o o ╬ o o · · 
· · · o o o o o · · 
· · o o o P o o o · 
· · · o o o o o · · 
· · · o o o o o · · 
· · · · · o · · · · 
";
            Assert.AreEqual(mapPossibleTilesExpected, mapPossibleTilesCurrent);
            Assert.AreEqual(5, pathFindingResult.Path.Count);
            Assert.IsTrue(pathFindingResult.Path.Count > 0);
        }

    }
}
