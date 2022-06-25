using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Map
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class MovementTests
    {
        [TestMethod]
        public void MovementWithNoOverwatchTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 destination = new(8, 0, 0);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange, fred.ActionPointsCurrent);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (KeyValuePair<Vector3, int> item in movementPossibileTiles)
            {
                if (item.Key == destination)
                {
                    destinationCheck = item.Key;
                }
            }
            Assert.AreEqual(destination, destinationCheck);
            PathFindingResult pathFindingResult = PathFinding.FindPath(map, fred.Location, destination);
            List<MovementAction> movementResults = CharacterMovement.MoveCharacter(map, fred, pathFindingResult, null, null, diceRolls);

            //Assert
            Assert.IsTrue(pathFindingResult != null);
            Assert.AreEqual(destination, fred.Location);
            Assert.AreEqual(8, pathFindingResult.Path.Count);
            Assert.AreEqual(8, movementResults.Count);
            Assert.AreEqual(new(0, 0, 0), movementResults[0].StartLocation);
            Assert.AreEqual(new(1, 0, 0), movementResults[0].EndLocation);
            Assert.AreEqual("Fred is moving from <0, 0, 0> to <1, 0, 0>", movementResults[0].Log[0]);
            Assert.AreEqual(null, movementResults[0].OverwatchEncounterResults);

            string log = @"
Fred is moving from <0, 0, 0> to <1, 0, 0>
Fred is moving from <1, 0, 0> to <2, 0, 0>
Fred is moving from <2, 0, 0> to <3, 0, 0>
Fred is moving from <3, 0, 0> to <4, 0, 0>
Fred is moving from <4, 0, 0> to <5, 0, 0>
Fred is moving from <5, 0, 0> to <6, 0, 0>
Fred is moving from <6, 0, 0> to <7, 0, 0>
Fred is moving from <7, 0, 0> to <8, 0, 0>
";
            Assert.AreEqual(log, CharacterMovement.LogString(movementResults));
        }

        [TestMethod]
        public void MovementInRangeTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 destination = new(8, 0, 0);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));

            //Act
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange, fred.ActionPointsCurrent);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (KeyValuePair<Vector3, int> item in movementPossibileTiles)
            {
                if (item.Key == destination)
                {
                    destinationCheck = item.Key;
                }
            }

            //Assert
            Assert.AreEqual(destination, destinationCheck);
        }

        [TestMethod]
        public void MovementOutOfRangeTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 destination = new(8, 0, 1);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.ActionPointsCurrent = 1;

            //Act
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange, fred.ActionPointsCurrent);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (KeyValuePair<Vector3, int> item in movementPossibileTiles)
            {
                if (item.Key == destination)
                {
                    destinationCheck = item.Key;
                }
            }

            //Assert
            Assert.AreEqual(Vector3.Zero, destinationCheck);
        }

        [TestMethod]
        public void MovementJustInRangeTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 destination = new(7, 0, 1);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));

            //Act
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange, fred.ActionPointsCurrent);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (KeyValuePair<Vector3, int> item in movementPossibileTiles)
            {
                if (item.Key == destination)
                {
                    destinationCheck = item.Key;
                }
            }

            //Assert
            Assert.AreEqual(destination, destinationCheck);
        }

        [TestMethod]
        public void MovementRange8Test()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(40, 1, 40);
            Vector3 destination = new(12, 0, 20);
            Character fred = CharacterPool.CreateFredHero(map, new(20, 0, 20));
            fred.ActionPointsCurrent = 2;

            //Act
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange, fred.ActionPointsCurrent);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (KeyValuePair<Vector3, int> item in movementPossibileTiles)
            {
                if (item.Key == destination)
                {
                    destinationCheck = item.Key;
                }
            }
            Assert.AreEqual(destination, destinationCheck);
            PathFindingResult pathFindingResult = PathFinding.FindPath(map, fred.Location, destination);
            List<MovementAction> movementResults = CharacterMovement.MoveCharacter(map, fred, pathFindingResult, null, null, null);

            string mapStringExpected = MapCore.GetMapStringWithItems(map, MovementPossibileTiles.ExtractVectorListFromKeyValuePair(movementPossibileTiles));
            string mapExpected = @"
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · o · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · o o o o o · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · o o o o o o o o o · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · o o o o o o o o o o o o o o o · · · · · · · · · · · · 
· · · · · · · · · · · o o o o o o o o o o o o o o o o o o o · · · · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · 
· · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · 
· · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · 
· · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · 
· · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · 
· · · · o o o o o o o o P o o o o o o o · o o o o o o o o o o o o o o o o · · · 
· · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · 
· · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · 
· · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · 
· · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · 
· · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · · · · o o o o o o o o o o o o o o o o o o o · · · · · · · · · · 
· · · · · · · · · · · · · o o o o o o o o o o o o o o o · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · o o o o o o o o o · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · o o o o o · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · o · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
";

            //Assert
            Assert.AreEqual(mapExpected, mapStringExpected);
            Assert.AreEqual(1, fred.ActionPointsCurrent);
            Assert.AreEqual(8, movementResults.Count);
            string log = @"
Fred is moving from <20, 0, 20> to <19, 0, 20>
Fred is moving from <19, 0, 20> to <18, 0, 20>
Fred is moving from <18, 0, 20> to <17, 0, 20>
Fred is moving from <17, 0, 20> to <16, 0, 20>
Fred is moving from <16, 0, 20> to <15, 0, 20>
Fred is moving from <15, 0, 20> to <14, 0, 20>
Fred is moving from <14, 0, 20> to <13, 0, 20>
Fred is moving from <13, 0, 20> to <12, 0, 20>
";
            Assert.AreEqual(log, CharacterMovement.LogString(movementResults));

        }

        [TestMethod]
        public void MovementRange16Test()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(40, 1, 40);
            Vector3 destination = new(6, 0, 20);
            Character fred = CharacterPool.CreateFredHero(map, new(20, 0, 20));
            fred.ActionPointsCurrent = 2;

            //Act
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange, fred.ActionPointsCurrent);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (KeyValuePair<Vector3, int> item in movementPossibileTiles)
            {
                if (item.Key == destination)
                {
                    destinationCheck = item.Key;
                }
            }
            Assert.AreEqual(destination, destinationCheck);
            PathFindingResult pathFindingResult = PathFinding.FindPath(map, fred.Location, destination);
            List<MovementAction> movementResults = CharacterMovement.MoveCharacter(map, fred, pathFindingResult, null, null, null);

            string mapStringExpected = MapCore.GetMapStringWithItems(map, MovementPossibileTiles.ExtractVectorListFromKeyValuePair(movementPossibileTiles));
            string mapExpected = @"
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · o · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · o o o o o · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · o o o o o o o o o · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · o o o o o o o o o o o o o o o · · · · · · · · · · · · 
· · · · · · · · · · · o o o o o o o o o o o o o o o o o o o · · · · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · 
· · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · 
· · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · 
· · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · 
· · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · 
· · · · o o P o o o o o o o o o o o o o · o o o o o o o o o o o o o o o o · · · 
· · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · 
· · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · 
· · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · 
· · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · 
· · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · 
· · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · · · · o o o o o o o o o o o o o o o o o o o · · · · · · · · · · 
· · · · · · · · · · · · · o o o o o o o o o o o o o o o · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · o o o o o o o o o · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · o o o o o · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · o · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
";

            //Assert
            Assert.AreEqual(mapExpected, mapStringExpected);
            Assert.AreEqual(0, fred.ActionPointsCurrent);
            Assert.AreEqual(14, movementResults.Count);
            string log = @"
Fred is moving from <20, 0, 20> to <19, 0, 20>
Fred is moving from <19, 0, 20> to <18, 0, 20>
Fred is moving from <18, 0, 20> to <17, 0, 20>
Fred is moving from <17, 0, 20> to <16, 0, 20>
Fred is moving from <16, 0, 20> to <15, 0, 20>
Fred is moving from <15, 0, 20> to <14, 0, 20>
Fred is moving from <14, 0, 20> to <13, 0, 20>
Fred is moving from <13, 0, 20> to <12, 0, 20>
Fred is moving from <12, 0, 20> to <11, 0, 20>
Fred is moving from <11, 0, 20> to <10, 0, 20>
Fred is moving from <10, 0, 20> to <9, 0, 20>
Fred is moving from <9, 0, 20> to <8, 0, 20>
Fred is moving from <8, 0, 20> to <7, 0, 20>
Fred is moving from <7, 0, 20> to <6, 0, 20>
";
            Assert.AreEqual(log, CharacterMovement.LogString(movementResults));
        }

        [TestMethod]
        public void MovementRange8AndRange16LayeredTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(40, 1, 40);
            Character fred = CharacterPool.CreateFredHero(map, new(20, 0, 20));
            Vector3 destination = new(6, 0, 20);

            //Act
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, fred.Location, fred.MobilityRange, fred.ActionPointsCurrent);
            Vector3 destinationCheck = Vector3.Zero;
            foreach (KeyValuePair<Vector3, int> item in movementPossibileTiles)
            {
                if (item.Key == destination)
                {
                    destinationCheck = item.Key;
                }
            }
            Assert.AreEqual(destination, destinationCheck);
            PathFindingResult pathFindingResult = PathFinding.FindPath(map, fred.Location, destination);
            List<MovementAction> movementResults = CharacterMovement.MoveCharacter(map, fred, pathFindingResult, null, null, null);

            List<Vector3> movementPossibileTilesRange8 = MovementPossibileTiles.ExtractVectorListFromKeyValuePair(movementPossibileTiles, 1);
            List<Vector3> movementPossibileTilesRange16 = MovementPossibileTiles.ExtractVectorListFromKeyValuePair(movementPossibileTiles, 2);
            string mapStringExpected = MapCore.GetMapStringWithItemLayers(map, movementPossibileTilesRange16, movementPossibileTilesRange8);
            string mapExpected = @"
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · o · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · o o o o o · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · o o o o o o o o o · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · o o o o o o o o o o o o o o o · · · · · · · · · · · · 
· · · · · · · · · · · o o o o o o o o o o o o o o o o o o o · · · · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · 
· · · · · · · · o o o o o o o o o o o o O o o o o o o o o o o o o · · · · · · · 
· · · · · · · o o o o o o o o o o o O O O O O o o o o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o o O O O O O O O O O o o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o O O O O O O O O O O O o o o o o o o o · · · · · · 
· · · · · · o o o o o o o o O O O O O O O O O O O O O o o o o o o o o · · · · · 
· · · · · · o o o o o o o o O O O O O O O O O O O O O o o o o o o o o · · · · · 
· · · · · o o o o o o o o O O O O O O O O O O O O O O O o o o o o o o o · · · · 
· · · · · o o o o o o o o O O O O O O O O O O O O O O O o o o o o o o o · · · · 
· · · · o o P o o o o o O O O O O O O O · O O O O O O O O o o o o o o o o · · · 
· · · · · o o o o o o o o O O O O O O O O O O O O O O O o o o o o o o o · · · · 
· · · · · o o o o o o o o O O O O O O O O O O O O O O O o o o o o o o o · · · · 
· · · · · · o o o o o o o o O O O O O O O O O O O O O o o o o o o o o · · · · · 
· · · · · · o o o o o o o o O O O O O O O O O O O O O o o o o o o o o · · · · · 
· · · · · · · o o o o o o o o O O O O O O O O O O O o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o o O O O O O O O O O o o o o o o o o o · · · · · · 
· · · · · · · o o o o o o o o o o o O O O O O o o o o o o o o o o o · · · · · · 
· · · · · · · · o o o o o o o o o o o o O o o o o o o o o o o o o · · · · · · · 
· · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · · o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · 
· · · · · · · · · · · o o o o o o o o o o o o o o o o o o o · · · · · · · · · · 
· · · · · · · · · · · · · o o o o o o o o o o o o o o o · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · o o o o o o o o o · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · o o o o o · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · o · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
";

            //Assert
            Assert.AreEqual(mapExpected, mapStringExpected);
            Assert.AreEqual(0, fred.ActionPointsCurrent);
            Assert.AreEqual(14, movementResults.Count);
            string log = @"
Fred is moving from <20, 0, 20> to <19, 0, 20>
Fred is moving from <19, 0, 20> to <18, 0, 20>
Fred is moving from <18, 0, 20> to <17, 0, 20>
Fred is moving from <17, 0, 20> to <16, 0, 20>
Fred is moving from <16, 0, 20> to <15, 0, 20>
Fred is moving from <15, 0, 20> to <14, 0, 20>
Fred is moving from <14, 0, 20> to <13, 0, 20>
Fred is moving from <13, 0, 20> to <12, 0, 20>
Fred is moving from <12, 0, 20> to <11, 0, 20>
Fred is moving from <11, 0, 20> to <10, 0, 20>
Fred is moving from <10, 0, 20> to <9, 0, 20>
Fred is moving from <9, 0, 20> to <8, 0, 20>
Fred is moving from <8, 0, 20> to <7, 0, 20>
Fred is moving from <7, 0, 20> to <6, 0, 20>
";
            Assert.AreEqual(log, CharacterMovement.LogString(movementResults));
        }
    }
}
