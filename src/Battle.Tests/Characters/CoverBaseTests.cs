using Battle.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CoverBaseTests
    {
        // In the diagrams
        // "." is open space
        // "■" is cover/an object
        // "S" is the player
        // "E" is the enemy/opponent
        // "E" will look at "S", and the cover system will determine if "S" is in cover or not
        // If a player is not in cover, they are 'flanked', and vulnerable

        [TestMethod]
        public void Test_WithoutCover_NoEnemy()
        {
            // Arrange
            //  · · · 
            //  · S · 
            //  · · · 
            Vector3 startingLocation = new(1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> enemyLocations = null;

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, null, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
            Assert.IsTrue(coverResult.InHalfCover == false);
            Assert.IsTrue(coverResult.InNorthFullCover == false);
            Assert.IsTrue(coverResult.InEastFullCover == false);
            Assert.IsTrue(coverResult.InSouthFullCover == false);
            Assert.IsTrue(coverResult.InWestFullCover == false);
            Assert.IsTrue(coverResult.InNorthHalfCover == false);
            Assert.IsTrue(coverResult.InEastHalfCover == false);
            Assert.IsTrue(coverResult.InSouthHalfCover == false);
            Assert.IsTrue(coverResult.InWestHalfCover == false);
        }

        [TestMethod]
        public void Test_WithoutCover_NoEnemy_TinyMap()
        {
            // Arrange
            //  S 
            Vector3 startingLocation = new(0, 0, 0);
            int width = 1;
            int height = 1;
            List<Vector3> enemyLocations = null;

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, null, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
            Assert.IsTrue(coverResult.InHalfCover == false);
            Assert.IsTrue(coverResult.InNorthFullCover == false);
            Assert.IsTrue(coverResult.InEastFullCover == false);
            Assert.IsTrue(coverResult.InSouthFullCover == false);
            Assert.IsTrue(coverResult.InWestFullCover == false);
            Assert.IsTrue(coverResult.InNorthHalfCover == false);
            Assert.IsTrue(coverResult.InEastHalfCover == false);
            Assert.IsTrue(coverResult.InSouthHalfCover == false);
            Assert.IsTrue(coverResult.InWestHalfCover == false);
        }


        [TestMethod]
        public void Test_WithNorthFullCover_NoEnemy()
        {
            // Arrange
            //  · ■ · 
            //  · S .
            //  · · · 
            Vector3 startingLocation = new(1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = null;

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == true);
            Assert.IsTrue(coverResult.InHalfCover == false);
            Assert.IsTrue(coverResult.InNorthFullCover == true);
            Assert.IsTrue(coverResult.InEastFullCover == false);
            Assert.IsTrue(coverResult.InSouthFullCover == false);
            Assert.IsTrue(coverResult.InWestFullCover == false);
            Assert.IsTrue(coverResult.InNorthHalfCover == false);
            Assert.IsTrue(coverResult.InEastHalfCover == false);
            Assert.IsTrue(coverResult.InSouthHalfCover == false);
            Assert.IsTrue(coverResult.InWestHalfCover == false);
        }

        [TestMethod]
        public void Test_WithNorthHalfCover_NoEnemy()
        {
            // Arrange
            //  · □ · 
            //  · S .
            //  · · · 
            Vector3 startingLocation = new(1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> lowCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = null;

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, null, lowCoverLocations);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
            Assert.IsTrue(coverResult.InHalfCover == true);
            Assert.IsTrue(coverResult.InNorthFullCover == false);
            Assert.IsTrue(coverResult.InEastFullCover == false);
            Assert.IsTrue(coverResult.InSouthFullCover == false);
            Assert.IsTrue(coverResult.InWestFullCover == false);
            Assert.IsTrue(coverResult.InNorthHalfCover == true);
            Assert.IsTrue(coverResult.InEastHalfCover == false);
            Assert.IsTrue(coverResult.InSouthHalfCover == false);
            Assert.IsTrue(coverResult.InWestHalfCover == false);
        }

        [TestMethod]
        public void Test_WithEastFullCover_NoEnemy()
        {
            // Arrange
            //  · · · 
            //  · S ■ 
            //  · · · 
            Vector3 startingLocation = new(1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(2, 0, 1)
            };
            List<Vector3> enemyLocations = null;

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == true);
            Assert.IsTrue(coverResult.InHalfCover == false);
            Assert.IsTrue(coverResult.InNorthFullCover == false);
            Assert.IsTrue(coverResult.InEastFullCover == true);
            Assert.IsTrue(coverResult.InSouthFullCover == false);
            Assert.IsTrue(coverResult.InWestFullCover == false);
            Assert.IsTrue(coverResult.InNorthHalfCover == false);
            Assert.IsTrue(coverResult.InEastHalfCover == false);
            Assert.IsTrue(coverResult.InSouthHalfCover == false);
            Assert.IsTrue(coverResult.InWestHalfCover == false);
        }
        [TestMethod]
        public void Test_WithEastHalfCover_NoEnemy()
        {
            // Arrange
            //  · · · 
            //  · S □ 
            //  · · · 
            Vector3 startingLocation = new(1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> halfCoverLocations = new()
            {
                new(2, 0, 1)
            };
            List<Vector3> enemyLocations = null;

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, null, halfCoverLocations);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
            Assert.IsTrue(coverResult.InHalfCover == true);
            Assert.IsTrue(coverResult.InNorthFullCover == false);
            Assert.IsTrue(coverResult.InEastFullCover == false);
            Assert.IsTrue(coverResult.InSouthFullCover == false);
            Assert.IsTrue(coverResult.InWestFullCover == false);
            Assert.IsTrue(coverResult.InNorthHalfCover == false);
            Assert.IsTrue(coverResult.InEastHalfCover == true);
            Assert.IsTrue(coverResult.InSouthHalfCover == false);
            Assert.IsTrue(coverResult.InWestHalfCover == false);
        }

        [TestMethod]
        public void Test_WithSouthFullCover_NoEnemy()
        {
            // Arrange
            //  · · · 
            //  · S · 
            //  · ■ · 
            Vector3 startingLocation = new(1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 0)
            };
            List<Vector3> enemyLocations = null;

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == true);
            Assert.IsTrue(coverResult.InHalfCover == false);
            Assert.IsTrue(coverResult.InNorthFullCover == false);
            Assert.IsTrue(coverResult.InEastFullCover == false);
            Assert.IsTrue(coverResult.InSouthFullCover == true);
            Assert.IsTrue(coverResult.InWestFullCover == false);
            Assert.IsTrue(coverResult.InNorthHalfCover == false);
            Assert.IsTrue(coverResult.InEastHalfCover == false);
            Assert.IsTrue(coverResult.InSouthHalfCover == false);
            Assert.IsTrue(coverResult.InWestHalfCover == false);
        }

        [TestMethod]
        public void Test_WithSouthHalfCover_NoEnemy()
        {
            // Arrange
            //  · · · 
            //  · S · 
            //  · □ · 
            Vector3 startingLocation = new(1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> halfCoverLocations = new()
            {
                new(1, 0, 0)
            };
            List<Vector3> enemyLocations = null;

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, null, halfCoverLocations);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
            Assert.IsTrue(coverResult.InHalfCover == true);
            Assert.IsTrue(coverResult.InNorthFullCover == false);
            Assert.IsTrue(coverResult.InEastFullCover == false);
            Assert.IsTrue(coverResult.InSouthFullCover == false);
            Assert.IsTrue(coverResult.InWestFullCover == false);
            Assert.IsTrue(coverResult.InNorthHalfCover == false);
            Assert.IsTrue(coverResult.InEastHalfCover == false);
            Assert.IsTrue(coverResult.InSouthHalfCover == true);
            Assert.IsTrue(coverResult.InWestHalfCover == false);
        }

        [TestMethod]
        public void Test_WithWestFullCover_NoEnemy()
        {
            // Arrange
            //  · · · 
            //  ■ S · 
            //  · · · 
            Vector3 startingLocation = new(1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(0, 0, 1)
            };
            List<Vector3> enemyLocations = null;

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == true);
            Assert.IsTrue(coverResult.InHalfCover == false);
            Assert.IsTrue(coverResult.InNorthFullCover == false);
            Assert.IsTrue(coverResult.InEastFullCover == false);
            Assert.IsTrue(coverResult.InSouthFullCover == false);
            Assert.IsTrue(coverResult.InWestFullCover == true);
            Assert.IsTrue(coverResult.InNorthHalfCover == false);
            Assert.IsTrue(coverResult.InEastHalfCover == false);
            Assert.IsTrue(coverResult.InSouthHalfCover == false);
            Assert.IsTrue(coverResult.InWestHalfCover == false);
        }

        [TestMethod]
        public void Test_WithWestHalfCover_NoEnemy()
        {
            // Arrange
            //  · · · 
            //  □ S · 
            //  · · · 
            Vector3 startingLocation = new(1, 0, 1);
            int width = 3;
            int height = 3;
            List<Vector3> lowCoverLocations = new()
            {
                new(0, 0, 1)
            };
            List<Vector3> enemyLocations = null;

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, null, lowCoverLocations);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
            Assert.IsTrue(coverResult.InHalfCover == true);
            Assert.IsTrue(coverResult.InNorthFullCover == false);
            Assert.IsTrue(coverResult.InEastFullCover == false);
            Assert.IsTrue(coverResult.InSouthFullCover == false);
            Assert.IsTrue(coverResult.InWestFullCover == false);
            Assert.IsTrue(coverResult.InNorthHalfCover == false);
            Assert.IsTrue(coverResult.InEastHalfCover == false);
            Assert.IsTrue(coverResult.InSouthHalfCover == false);
            Assert.IsTrue(coverResult.InWestHalfCover == true);
        }

    }
}