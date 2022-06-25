using Battle.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CoverNorthTests
    {
        // In the diagrams
        // "." is open space
        // CoverType.FullCover is cover/an object
        // "S" is the player
        // "E" is the enemy/opponent
        // "E" will look at "S", and the cover system will determine if "S" is in cover or not
        // If a player is not in cover, they are 'flanked', and vulnerable

        [TestMethod]
        public void Test_WithNorthCover_1_O_Clock_Enemy()
        {
            // Arrange
            //  In Cover
            //  · · E .
            //  · ■ · · 
            //  · S · · 
            //  · · · .
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = new()
            {
                new(2, 0, 3)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == true);
        }

        [TestMethod]
        public void Test_WithNorthCover_2_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  · · · .
            //  · ■ E · 
            //  · S · · 
            //  · · · .
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = new()
            {
                new(2, 0, 2)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsFlanked == true);
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithNorthCover_3_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  · · · .
            //  · ■ · · 
            //  · S E · 
            //  · · · .
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = new()
            {
                new(2, 0, 1)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsFlanked == true);
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithNorthCover_5_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  · · · .
            //  · ■ · · 
            //  · S · · 
            //  · · E .
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = new()
            {
                new(2, 0, 0)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsFlanked == true);
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithNorthCover_6_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  · · · .
            //  · ■ · · 
            //  · S · · 
            //  · E · .
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = new()
            {
                new(1, 0, 0)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsFlanked == true);
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithNorthCover_7_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  · · · .
            //  · ■ · · 
            //  · S · · 
            //  E · · .
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = new()
            {
                new(0, 0, 0)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsFlanked == true);
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithNorthCover_8_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  · · · .
            //  · ■ · · 
            //  E S · · 
            //  · · · .
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = new()
            {
                new(0, 0, 1)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsFlanked == true);
            Assert.IsTrue(coverResult.InFullCover == false);
        }


        [TestMethod]
        public void Test_WithNorthCover_9_O_Clock_Enemy()
        {
            // Arrange
            //  Flanked
            //  · · · .
            //  E ■ · · 
            //  · S · · 
            //  · · · .
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = new()
            {
                new(0, 0, 2)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsFlanked == true);
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithNorthCover_11_O_Clock_Enemy()
        {
            // Arrange
            //  In Cover
            //  E · · .
            //  · ■ · · 
            //  · S · · 
            //  · · · .
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = new()
            {
                new(0, 0, 3)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == true);
        }

        [TestMethod]
        public void Test_WithNorthCover_12_O_Clock_Enemy()
        {
            // Arrange
            //  In Cover
            //  · E · .
            //  · ■ · · 
            //  · S · · 
            //  · · · .
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 2)
            };
            List<Vector3> enemyLocations = new()
            {
                new(1, 0, 3)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == true);
        }

    }
}