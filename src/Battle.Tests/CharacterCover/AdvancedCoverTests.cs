﻿using Battle.Logic.CharacterCover;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.CharacterCover
{
    [TestClass]
    [TestCategory("L0")]
    public class AdvancedCoverTests
    {

        [TestMethod]
        public void Test_WithNortheastCovers_TwoEnemysInCover_PlayerStillInCover()
        {
            // Arrange
            //  In Cover
            // 3 □ E □ □
            // 2 □ ■ □ □ 
            // 1 □ S ■ E 
            // 0 □ □ □ □
            //   0 1 2 3          
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            coverLocations.Add(new(1, 0, 2));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(3, 0, 1));
            enemyLocations.Add(new(1, 0, 3));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height, startingLocation, coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
            Assert.IsTrue(coverResult.InNorthCover == true);
            Assert.IsTrue(coverResult.InEastCover == true);
            Assert.IsTrue(coverResult.InSouthCover == false);
            Assert.IsTrue(coverResult.InWestCover == false);
        }

        [TestMethod]
        public void Test_WithNortheastCovers_TwoEnemysNotInCover_PlayerStillInCover()
        {
            // Arrange
            //  In Cover
            //  E □ □ □
            //  □ ■ □ □ 
            //  □ S ■ □
            //  □ □ □ E             
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            coverLocations.Add(new(1, 0, 2));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(3, 0, 0));
            enemyLocations.Add(new(0, 0, 3));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height, startingLocation, coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
            Assert.IsTrue(coverResult.InNorthCover == true);
            Assert.IsTrue(coverResult.InEastCover == true);
            Assert.IsTrue(coverResult.InSouthCover == false);
            Assert.IsTrue(coverResult.InWestCover == false);
        }

        [TestMethod]
        public void Test_WithNortheastCovers_TwoEnemysNotInCover_PlayerFlanked()
        {
            // Arrange
            //  Flanked
            //  □ □ □ □
            //  E ■ □ □  
            //  □ S ■ □
            //  □ □ E □             
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            coverLocations.Add(new(1, 0, 2));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(2, 0, 0));
            enemyLocations.Add(new(0, 0, 2));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height, startingLocation, coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
            Assert.IsTrue(coverResult.InNorthCover == true);
            Assert.IsTrue(coverResult.InEastCover == true);
            Assert.IsTrue(coverResult.InSouthCover == false);
            Assert.IsTrue(coverResult.InWestCover == false);
        }


        [TestMethod]
        public void Test_WithNorthwestCovers_TwoEnemysInCover_PlayerStillInCover()
        {
            // Arrange
            //  In Cover
            // 3 □ □ E □
            // 2 □ □ ■ □ 
            // 1 E ■ S □ 
            // 0 □ □ □ □
            //   0 1 2 3          
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            coverLocations.Add(new(2, 0, 2));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(0, 0, 1));
            enemyLocations.Add(new(2, 0, 3));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height, startingLocation, coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
            Assert.IsTrue(coverResult.InNorthCover == true);
            Assert.IsTrue(coverResult.InEastCover == false);
            Assert.IsTrue(coverResult.InSouthCover == false);
            Assert.IsTrue(coverResult.InWestCover == true);
        }

        [TestMethod]
        public void Test_WithNorthwestCovers_TwoEnemysNotInCover_PlayerStillInCover()
        {
            // Arrange
            //  In Cover
            //  □ □ □ E
            //  □ □ ■ □ 
            //  □ ■ S □
            //  E □ □ □            
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            coverLocations.Add(new(2, 0, 2));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(0, 0, 0));
            enemyLocations.Add(new(3, 0, 3));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height, startingLocation, coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
            Assert.IsTrue(coverResult.InNorthCover == true);
            Assert.IsTrue(coverResult.InEastCover == false);
            Assert.IsTrue(coverResult.InSouthCover == false);
            Assert.IsTrue(coverResult.InWestCover == true);
        }

        [TestMethod]
        public void Test_WithNorthwestCovers_TwoEnemysNotInCover_PlayerFlanked()
        {
            // Arrange
            //  Flanked
            //  □ □ □ □
            //  □ □ ■ E 
            //  □ ■ S □
            //  □ E □ □            
            Vector3 startingLocation = new(2, 0, 1);
            int width = 4;
            int height = 4;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(1, 0, 1));
            coverLocations.Add(new(2, 0, 2));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(1, 0, 0));
            enemyLocations.Add(new(3, 0, 2));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height, startingLocation, coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == false);
            Assert.IsTrue(coverResult.InNorthCover == true);
            Assert.IsTrue(coverResult.InEastCover == false);
            Assert.IsTrue(coverResult.InSouthCover == false);
            Assert.IsTrue(coverResult.InWestCover == true);
        }


        [TestMethod]
        public void Test_WithTwoCover_SouthEnemy()
        {
            // Arrange
            //  In Cover
            //  □ □ □ □ 
            //  □ S ■ □ 
            //  □ ■ E □
            Vector3 startingLocation = new(1, 0, 1);
            int width = 4;
            int height = 3;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 1));
            coverLocations.Add(new(1, 0, 0));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(2, 0, 0));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height, startingLocation, coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
            Assert.IsTrue(coverResult.InNorthCover == false);
            Assert.IsTrue(coverResult.InEastCover == true);
            Assert.IsTrue(coverResult.InSouthCover == true);
            Assert.IsTrue(coverResult.InWestCover == false);
        }

        [TestMethod]
        public void Test_WithFourCovers_FourEnemysNotInCover_PlayerInCover()
        {
            // Arrange
            // In Cover
            // 4 □ □ E □ □
            // 3 □ □ ■ □ □  
            // 2 E ■ S ■ E
            // 1 □ □ ■ □ □
            // 0 □ □ E □ □ 
            //   0 1 2 3 4            
            Vector3 startingLocation = new(2, 0, 2);
            int width = 5;
            int height = 5;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 3));
            coverLocations.Add(new(3, 0, 2));
            coverLocations.Add(new(2, 0, 1));
            coverLocations.Add(new(1, 0, 2));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(2, 0, 0));
            enemyLocations.Add(new(0, 0, 2));
            enemyLocations.Add(new(2, 0, 4));
            enemyLocations.Add(new(4, 0, 2));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height, startingLocation, coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
            Assert.IsTrue(coverResult.InNorthCover == true);
            Assert.IsTrue(coverResult.InEastCover == true);
            Assert.IsTrue(coverResult.InSouthCover == true);
            Assert.IsTrue(coverResult.InWestCover == true);
        }

        [TestMethod]
        public void Test_WithFourCovers_FourEnemysInCover_PlayerInCover()
        {
            // Arrange
            // In Cover
            // 4 □ □ □ □ □
            // 3 □ E ■ E □  
            // 2 □ ■ S ■ □
            // 1 □ E ■ E □
            // 0 □ □ □ □ □ 
            //   0 1 2 3 4            
            Vector3 startingLocation = new(2, 0, 2);
            int width = 5;
            int height = 5;
            List<Vector3> coverLocations = new();
            coverLocations.Add(new(2, 0, 3));
            coverLocations.Add(new(3, 0, 2));
            coverLocations.Add(new(2, 0, 1));
            coverLocations.Add(new(1, 0, 2));
            List<Vector3> enemyLocations = new();
            enemyLocations.Add(new(1, 0, 1));
            enemyLocations.Add(new(1, 0, 3));
            enemyLocations.Add(new(3, 0, 3));
            enemyLocations.Add(new(3, 0, 1));

            // Act
            string[,] map = CoverUtility.InitializeMap(width, height, startingLocation, coverLocations);
            CoverState coverResult = Cover.CalculateCover(startingLocation, width, height, map, enemyLocations);

            // Assert
            Assert.IsTrue(coverResult.IsInCover == true);
            Assert.IsTrue(coverResult.InNorthCover == true);
            Assert.IsTrue(coverResult.InEastCover == true);
            Assert.IsTrue(coverResult.InSouthCover == true);
            Assert.IsTrue(coverResult.InWestCover == true);
        }

    }
}
