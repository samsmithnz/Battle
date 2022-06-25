using Battle.Logic.Characters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L0")]
    public class CoverWestTests
    {
        // In the diagrams
        // "." is open space
        // CoverType.FullCover is cover/an object
        // "S" is the player
        // "E" is the enemy/opponent
        // "E" will look at "S", and the cover system will determine if "S" is in cover or not
        // If a player is not in cover, they are 'flanked', and vulnerable

        [TestMethod]
        public void Test_WithWestCover_2_OClock_Enemy()
        {
            // Arrange
            //  Flanked
            //  · · · E 
            //  · ■ S · 
            //  · · · .
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 1)
            };
            List<Vector3> enemyLocations = new()
            {
                new(3, 0, 2)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithWestCover_3_OClock_Enemy()
        {
            // Arrange
            //  Flanked
            // 2 · · · · 
            // 1 · ■ S E 
            // 0 · · · .
            //   0 1 2 3
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 1)
            };
            List<Vector3> enemyLocations = new()
            {
                new(3, 0, 1)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithWestCover_4_OClock_Enemy()
        {
            // Arrange
            //  Flanked
            //  · · · · 
            //  · ■ S · 
            //  · · · E
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 1)
            };
            List<Vector3> enemyLocations = new()
            {
                new(3, 0, 0)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithWestCover_5_OClock__Flanking()
        {
            // Arrange
            //  Flanked
            //  · · · · 
            //  · ■ S · 
            //  · · E .
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 1)
            };
            List<Vector3> enemyLocations = new()
            {
                new(2, 0, 0)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithWestCover_6_OClock_Enemy2()
        {
            // Arrange
            //  Flanked
            //  · · · · 
            //  · ■ S · 
            //  · E · .
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 1)
            };
            List<Vector3> enemyLocations = new()
            {
                new(1, 0, 0)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithWestCover_7_OClock_Enemy()
        {
            // Arrange
            //  Flanked
            //  · · · · 
            //  · ■ S · 
            //  E · · .
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 1)
            };
            List<Vector3> enemyLocations = new()
            {
                new(3, 0, 1)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithWestCover_9_OClock_Enemy()
        {
            // Arrange
            //  In Cover
            //  · · · · 
            //  E ■ S · 
            //  · · · ·            
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 1)
            };
            List<Vector3> enemyLocations = new()
            {
                new(0, 0, 1)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == true);
        }

        [TestMethod]
        public void Test_WithWestCover_10_OClock_Enemy()
        {
            // Arrange
            //  In Cover
            //  E · · · 
            //  · ■ S · 
            //  · · · ·               
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 1)
            };
            List<Vector3> enemyLocations = new()
            {
                new(0, 0, 2)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == true);
        }

        [TestMethod]
        public void Test_WithWestCover_11_OClock_Enemy2()
        {
            // Arrange
            //  Flanked
            //  · E · · 
            //  · ■ S · 
            //  · · · ·             
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 1)
            };
            List<Vector3> enemyLocations = new()
            {
                new(1, 0, 2)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
        }

        [TestMethod]
        public void Test_WithWestCover_12_OClock_Enemy()
        {
            // Arrange
            //  Flanked
            // 2 · · E · 
            // 1 · ■ S · 
            // 0 · · · ·   
            //   0 1 2 3          
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> highCoverLocations = new()
            {
                new(1, 0, 1)
            };
            List<Vector3> enemyLocations = new()
            {
                new(2, 0, 2)
            };

            // Act
            string[,,] map = CoverUtility.InitializeMap(width, 1, height, highCoverLocations, null);
            CoverState coverResult = CharacterCover.CalculateCover(map, startingLocation, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.InFullCover == false);
        }

    }
}