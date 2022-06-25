using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Battle.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class PathFindingTests
    {

        [TestMethod]
        public void Test_WithoutWalls_CanFindPath()
        {
            //Arrange
            Vector3 startLocation = new(1, 0, 2);
            Vector3 endLocation = new(5, 0, 2);
            string[,,] map = MapCore.InitializeMap(7, 1, 5);

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(map, startLocation, endLocation);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Any());
            Assert.AreEqual(4, PathFindingResult.Path.Count);
            Assert.AreEqual("<2, 0, 2>", PathFindingResult.Path[0].ToString());
            Assert.AreEqual("<5, 0, 2>", PathFindingResult.Path[3].ToString());
        }

        [TestMethod]
        public void Test_WithOpenWall_CanFindPathAroundWall()
        {
            //Arrange
            //  · · · ■ · · .
            //  · · · ■ · · .
            //  · S · ■ · F .
            //  · · * ■ ■ * .
            //  · · · * * · .

            // Path: 1,2 ; 2,1 ; 3,0 ; 4,0 ; 5,1 ; 5,2
            Vector3 startLocation = new(1, 0, 2);
            Vector3 endLocation = new(5, 0, 2);
            string[,,] map = MapCore.InitializeMap(7, 1, 5);
            map[3, 0, 4] = MapObjectType.FullCover;
            map[3, 0, 3] = MapObjectType.FullCover;
            map[3, 0, 2] = MapObjectType.FullCover;
            map[3, 0, 1] = MapObjectType.FullCover;
            map[4, 0, 1] = MapObjectType.FullCover;

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(map, startLocation, endLocation);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Any());
            Assert.IsTrue(PathFindingResult.GetLastTile() != null);
            Assert.AreEqual(7, PathFindingResult.GetLastTile().TraversalCost);
            Assert.AreEqual(5, PathFindingResult.Path.Count);
        }

        [TestMethod]
        public void Test_WithClosedWall_CannotFindPath()
        {
            //Arrange
            //  · · · ■ · · .
            //  · · · ■ · · .
            //  · S · ■ · F .
            //  · · · ■ · · .
            //  · · · ■ · · .

            // No path
            Vector3 startLocation = new(1, 0, 2);
            Vector3 endLocation = new(5, 0, 2);
            string[,,] map = MapCore.InitializeMap(7, 1, 5);
            map[3, 0, 4] = MapObjectType.FullCover;
            map[3, 0, 3] = MapObjectType.FullCover;
            map[3, 0, 2] = MapObjectType.FullCover;
            map[3, 0, 1] = MapObjectType.FullCover;
            map[3, 0, 0] = MapObjectType.FullCover;

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(map, startLocation, endLocation);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsFalse(PathFindingResult.Path.Any());
            Assert.IsFalse(PathFindingResult.Tiles.Any());
        }


        [TestMethod]
        public void Test_WithMazeWall()
        {
            //Arrange
            //  S ■ ■ · ■ ■ F
            //  * ■ · ■ · ■ .
            //  * ■ · ■ · ■ .
            //  * ■ · ■ · ■ .
            //  ■ · ■ ■ ■ · ■

            // long path
            Vector3 startLocation = new(0, 0, 4);
            Vector3 endLocation = new(6, 0, 4);
            string[,,] map = MapCore.InitializeMap(7, 1, 5);
            map[0, 0, 0] = MapObjectType.FullCover;
            map[1, 0, 4] = MapObjectType.FullCover;
            map[1, 0, 3] = MapObjectType.FullCover;
            map[1, 0, 2] = MapObjectType.FullCover;
            map[1, 0, 1] = MapObjectType.FullCover;
            map[2, 0, 4] = MapObjectType.FullCover;
            map[2, 0, 0] = MapObjectType.FullCover;
            map[3, 0, 3] = MapObjectType.FullCover;
            map[3, 0, 2] = MapObjectType.FullCover;
            map[3, 0, 1] = MapObjectType.FullCover;
            map[3, 0, 0] = MapObjectType.FullCover;
            map[4, 0, 4] = MapObjectType.FullCover;
            map[4, 0, 0] = MapObjectType.FullCover;
            map[5, 0, 4] = MapObjectType.FullCover;
            map[5, 0, 3] = MapObjectType.FullCover;
            map[5, 0, 2] = MapObjectType.FullCover;
            map[5, 0, 1] = MapObjectType.FullCover;
            map[6, 0, 0] = MapObjectType.FullCover;

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(map, startLocation, endLocation);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Any());
            Assert.IsTrue(PathFindingResult.GetLastTile() != null);
            Assert.AreEqual(19, PathFindingResult.GetLastTile().TraversalCost);
            Assert.AreEqual(16, PathFindingResult.Path.Count);
        }


        [TestMethod]
        public void Test_GiantRandomMap_WithInefficentPath()
        {
            //Arrange
            Vector3 startLocation = new(0, 0, 0);
            Vector3 endLocation = new(69, 0, 39);
            string[,,] map = CreateGiantMap();

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(map, startLocation, endLocation);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Any());
            Assert.AreEqual(97, PathFindingResult.Path.Count);
            CreateDebugPictureOfMapAndRoute(map, 70, 1, 40, PathFindingResult.Path);
        }

        [TestMethod]
        public void Test_Contained_RangeOf1_NoPath()
        {
            // 4 · · · · F 
            // 3 · ■ ■ ■ · 
            // 2 · ■ S ■ · 
            // 1 · ■ ■ ■ · 
            // 0 · · · · · 
            //   0 1 2 3 4 

            //Arrange
            int height = 5;
            int width = 5;
            string[,,] map = MapCore.InitializeMap(width, 1, height);
            map[1, 0, 1] = MapObjectType.FullCover;
            map[1, 0, 2] = MapObjectType.FullCover;
            map[1, 0, 3] = MapObjectType.FullCover;
            map[2, 0, 1] = MapObjectType.FullCover;
            map[2, 0, 3] = MapObjectType.FullCover;
            map[3, 0, 1] = MapObjectType.FullCover;
            map[3, 0, 2] = MapObjectType.FullCover;
            map[3, 0, 3] = MapObjectType.FullCover;
            Vector3 startLocation = new(2, 0, 2);
            Vector3 endLocation = new(2, 0, 4);

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(map, startLocation, endLocation);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Count == 0);
            Assert.IsTrue(PathFindingResult.Tiles.Count == 0);
            Assert.IsTrue(PathFindingResult.GetLastTile() == null);
        }

        [TestMethod]
        public void Test_Contained_RangeOf1_StartIsSameAsFinish()
        {
            // 4 · · · · · 
            // 3 · ■ ■ ■ · 
            // 2 · ■ S ■ · 
            // 1 · ■ ■ ■ · 
            // 0 · · · · · 
            //   0 1 2 3 4 

            //Arrange
            Vector3 startLocation = new(2, 0, 2);
            Vector3 endLocation = new(2, 0, 2);
            int height = 5;
            int width = 5;
            string[,,] map = MapCore.InitializeMap(width, 1, height);
            map[1, 0, 1] = MapObjectType.FullCover;
            map[1, 0, 2] = MapObjectType.FullCover;
            map[1, 0, 3] = MapObjectType.FullCover;
            map[2, 0, 1] = MapObjectType.FullCover;
            map[2, 0, 3] = MapObjectType.FullCover;
            map[3, 0, 1] = MapObjectType.FullCover;
            map[3, 0, 2] = MapObjectType.FullCover;
            map[3, 0, 3] = MapObjectType.FullCover;

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(map, startLocation, endLocation);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Count == 0);
            Assert.IsTrue(PathFindingResult.Tiles.Count == 0);
            Assert.IsTrue(PathFindingResult.GetLastTile() == null);
        }

        #region "private helper functions"

        private static void CreateDebugPictureOfMapAndRoute(string[,,] map, int xMax, int yMax, int zMax, List<Vector3> path)
        {
            string[,,] mapDebug = new string[xMax, yMax, zMax];
            int y = 0;
            for (int z = 0; z < zMax; z++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    if (map[x, y, z] == "")
                    {
                        mapDebug[x, y, z] = " .";
                    }
                    else if (map[x, y, z] != "")
                    {
                        mapDebug[x, y, z] = " ■";
                    }
                }
            }

            int i = 0;
            foreach (Vector3 item in path)
            {
                if (i == 0)
                {
                    mapDebug[0, 0, 0] = " S";
                }
                if (i == path.Count - 1)
                {
                    mapDebug[(int)item.X, (int)item.Y, (int)item.Z] = " F";
                }
                else
                {
                    mapDebug[(int)item.X, (int)item.Y, (int)item.Z] = " *";
                }
                i++;
            }

            y = 0;
            for (int z = zMax - 1; z >= 0; z--)
            {
                for (int x = 0; x < xMax; x++)
                {
                    System.Diagnostics.Debug.Write(mapDebug[x, y, z]);
                }
                System.Diagnostics.Debug.WriteLine("");
            }

        }

        [TestMethod]
        public void TileTest()
        {
            //Arrange
            MapTile tile = new(3, 0, 3, "", new(6, 0, 6));

            //Act
            string result = tile.ToString();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("(3, 0, 3): Untested", result);
        }

        [TestMethod]
        public void Test_WithoutWalls_CanFindPathNextDoor()
        {
            //Arrange
            Vector3 startLocation = new(25, 0, 30);
            Vector3 endLocation = new(25, 0, 29);
            string[,,] map = MapCore.InitializeMap(50, 1, 50);

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(map, startLocation, endLocation);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.IsTrue(PathFindingResult.Path.Any());
            Assert.AreEqual(1, PathFindingResult.Path.Count);
            Assert.AreEqual("<25, 0, 29>", PathFindingResult.Path[0].ToString());
        }

        [TestMethod]
        public void Test_WithoutWalls_NoMovement()
        {
            //Arrange
            Vector3 startLocation = new(1, 0, 2);
            Vector3 endLocation = new(1, 0, 2);
            string[,,] map = MapCore.InitializeMap(7, 1, 5);

            //Act
            PathFindingResult PathFindingResult = PathFinding.FindPath(map, startLocation, endLocation);

            //Assert
            Assert.IsNotNull(PathFindingResult);
            Assert.IsNotNull(PathFindingResult.Path);
            Assert.AreEqual(0, PathFindingResult.Path.Count);
        }


        #region "Create huge map"

        private static string[,,] CreateGiantMap()
        {
            //· ■ · · · ■ ■ · ■ ■ ■ ■ · · · ■ · ■ · · · ■ ■ · · ■ · · · · ■ · · · ■ · ■ · ■ · ■ ■ ■ ■ ■ · ■ ■ ■ · · ■ ■ ■ · · · · ■ · · · · · ■ ■ · · · F
            //■ ■ · ■ · ■ ■ · · · ■ ■ · ■ · · · · · ■ · · · · · · · ■ · · · ■ · ■ · · · ■ · · · · · · ■ · · ■ ■ · · · ■ ■ ■ · · · ■ · · ■ ** · ■ · · * ■
            //■ · ■ · ■ ■ ■ · ■ · · · ■ ■ · · · · · ■ · ■ · · ■ · ■ · · · · · · · · · · · · · ■ · ■ · · · · · · · ■ · ■ ■ ■ · · · · · ** ■ ■ * ■ ■ * ■ .
            //■ · ■ · ■ ■ ■ · · · · · ■ ■ ■ · · ■ · · · · ■ · · · · · · ■ · · · · · ■ · · · · · ■ ■ ■ · ■ ■ ■ · ■ · · · ■ · · · ■ · * ■ · ■ · · * * · ■ .
            //· ■ · · · ■ · · · · · ■ ■ ■ · · · · ■ ■ ■ ■ ■ · · ■ · · ■ · · ■ ■ ■ ■ · · ■ ■ · ■ ■ ■ · ■ · · · · ■ · · ■ ■ · · · · * ■ · ■ ■ · ■ ■ · ■ · ■
            //■ ■ · ■ · ■ · · · ■ ■ · ■ · · · · ■ ■ · · ■ · · · · ■ · · ■ ■ · ■ · ■ · · · · · ■ · ■ ■ ■ ■ · · ■ ■ · ■ · · · ■ · * ■ · ■ · ■ · ■ ■ · ■ · .
            //· · ■ · ■ ■ ■ ■ · ■ ■ ■ · · ■ · · · ■ ■ · · · ■ · · ■ · ■ · · ■ · ■ ■ · ■ ■ · · · · · ■ ■ · · · ■ · ■ · · ■ ■ · · * ■ ■ · · · · · ■ · · ■ .
            //· ■ · ■ ■ · ■ · ■ · · · · ■ · · ■ · · ■ · ■ · · ■ · ■ ■ ■ · · ■ · · · · ■ · · ■ ■ · ■ · · ■ ■ ■ ■ · · ■ · ■ ■ · * ■ · · · ■ ■ · ■ · · ■ · ■
            //· ■ · · · · · · ■ · · · ■ ■ · ■ · ■ · · · ■ ■ · · · · ■ · · ■ · · · · · · · ■ · · ■ · · · · ■ ■ · · · ■ · ■ · · * ■ ■ · · · ■ · · · ■ · · ■
            //· ■ · · ■ ■ · · · ■ ■ ■ · ■ · ■ ■ · · · ■ · ■ ■ · ■ · ■ ■ ■ · · · ■ ■ ■ · · · ■ ■ · · ■ · ■ ■ · ■ · ■ ■ ■ · ■ * ■ ■ · ■ · ■ · · · · ■ · · .
            //■ · · · · · · · ■ ■ · ■ · ■ ■ · · · · ■ · · ■ · · · · · · ■ ■ ■ ■ · · · · · ■ ■ ■ · ■ ■ ■ ■ · · · ■ * · ■ · ■ · * ■ ■ ■ ■ ■ ■ · ■ ■ · · · .
            //■ ■ · · · ■ ■ · ■ · ■ · · · ■ ■ ■ · · ■ ■ · · · ■ · · · ■ ■ · ■ ■ · · ■ · * * * ■ ■ ■ ■ · ■ ■ ■ * * ■ * ■ ■ ■ * ■ ■ · · · · · · · ■ · · · ■
            //· · · · ■ ■ · · ■ · ■ · · · ■ ■ ■ ■ ■ · · ■ · · · ■ · · · ■ ■ · · · * * * ■ ■ ■ * * * ■ * * * * ■ ■ ■ · * * ■ * · ■ ■ · · · ■ ■ ■ · · · · ■
            //· ■ · · ■ · ■ · ■ ■ · · ■ · · ■ · · ■ ■ · · · ■ · ■ · · ■ · · · · * ■ ■ ■ · · · ■ · ■ * · · · · · · · · · ■ * · ■ ■ · ■ · · ■ ■ ■ · ■ · · .
            //· ■ · · · ■ ■ ■ · · · · · ■ · · ■ ■ ■ ■ ■ ■ · · · ■ · ■ ■ ■ · ■ · ■ * ■ · · · ■ ■ · · ■ ■ · · ■ · · ■ · · · ■ · ■ · ■ · ■ ■ · · · ■ · · · ■
            //· · ■ · · · · · · · · ■ ■ ■ ■ · · ■ · ■ · ■ · · · ■ ■ ■ ■ ■ ■ · · · * ■ · ■ ■ ■ · ■ · · ■ · · ■ · ■ · ■ · ■ · ■ · · · · · · ■ · ■ ■ · · · ■
            //· · · · ■ · · · · ■ ■ · ■ ■ ■ · · ■ ■ · · · · · ■ · ■ · ■ · · ■ · · * ■ · ■ · · ■ · ■ ■ ■ · ■ ■ · ■ · ■ · · · ■ · · ■ ■ · · · · · · · · · .
            //■ · · · · ■ ■ · ■ · ■ · · · · ■ · · · ■ ■ · · · · ■ ■ · · · ■ ■ · * ■ ■ · ■ ■ · ■ ■ ■ · ■ · ■ · · · · · · · · · · · · ■ ■ ■ ■ · · ■ ■ · · ■
            //· · · · · · ■ · · ■ · · · ■ ■ ■ · · · · ■ · · ■ · · · ■ ■ ■ ■ ■ · * ■ ■ ■ ■ ■ ■ ■ · ■ ■ ■ · ■ · · ■ · ■ · · · ■ · · · ■ · · · · · ■ · ■ · .
            //· · · · · · · ■ ■ · ■ · · · ■ ■ ■ · · · · ■ ■ · ■ ■ ■ · ■ ■ ■ · * ■ ■ ■ · ■ · ■ · · ■ · · ■ · · · ■ ■ · · · ■ ■ ■ ■ · · ■ ■ ■ · ■ · · · · .
            //■ · · ■ ■ ■ · ■ · · ■ · ■ · · ■ · ■ · · · · · ■ ■ ■ · · · ■ · * · ■ ■ · · ■ · · · · · · ■ · · ■ ■ · · ■ · · · · · ■ · ■ · · · · ■ ■ · ■ · .
            //■ ■ ■ ■ · ■ · · ■ · · · · · · · ■ ■ · ■ ■ · · · · · · · · · * ■ ■ ■ ■ ■ · ■ · · ■ · ■ · · ■ ■ ■ · · · ■ ■ · · ■ · · ■ ■ ■ ■ ■ · ■ · · ■ · .
            //■ ■ · ■ ■ · ■ · · · ■ · · · · · · · · ■ ■ · · · · ■ · ■ ■ ■ · * * * * * · ■ · · · ■ · · · ■ ■ ■ · · · · ■ · · ■ · · · · · ■ ■ ■ ■ · · ■ ■ ■
            //· · · · · · · ■ ■ · ■ ■ ■ · · · ■ ■ · ■ · · ■ ■ · · ■ ■ · · · ■ · ■ · ■ * ■ · · ■ · · · · ■ · · · · · ■ · · ■ · · · · · · · · ■ ■ ■ · ■ · .
            //■ ■ · ■ · ■ · ■ ■ · ■ · · · ■ · ■ · · ■ · ■ ■ ■ · · ■ · ■ · · ■ ■ · ■ * · ■ · ■ · · · · · · · ■ · ■ · ■ · ■ · · ■ · ■ · ■ · ■ · ■ ■ ■ · · .
            //■ ■ ■ · · ■ · ■ ■ ■ ■ · · ■ ■ ■ · · ■ ■ · · · · · ■ · ■ ■ · * * ■ ■ * ■ · · ■ ■ · ■ ■ ■ · ■ · · ■ · · ■ · · · · · · · ■ ■ · · ■ ■ · · · ■ .
            //■ ■ ■ · ■ · ■ · ■ ■ ■ · · ■ · ■ ■ ■ ■ · · · ■ ■ ■ · ■ ■ ■ * · · * * · · · · · · · · · · ■ · ■ · ■ · · ■ ■ · · ■ ■ ■ · · · · · · · ■ ■ · · .
            //· · ■ · · · · · · · · ■ ■ * * ■ · ■ · · · ■ · * ■ ■ ■ * * · · · · · · ■ · ■ · · ■ ■ ■ ■ · ■ ■ ■ ■ · ■ ■ · · · ■ ■ ■ ■ · · · · ■ ■ · ■ ■ ■ .
            //· · ■ ■ · ■ · ■ ■ · · * * · ■ * · ■ · ■ · · * ■ * ■ * · · · ■ · · ■ ■ · ■ · ■ · · ■ · · · · · · · · ■ · ■ · · · · · · ■ ■ ■ ■ · · · ■ · · .
            //■ · · ■ · · ■ ■ · ■ * ■ · ■ · ■ * ■ ■ ■ ■ · * ■ · * · · · ■ ■ ■ · ■ · ■ · ■ · ■ · ■ ■ · · ■ ■ ■ · · ■ · · · · ■ ■ · ■ · · · ■ · · · ■ · · .
            //· ■ · · · ■ ■ · * * ■ ■ · · · * ■ ■ · ■ · * ■ · · ■ ■ ■ ■ · · ■ · · · ■ · · · ■ · · ■ ■ ■ · · ■ · · ■ ■ · · · ■ ■ · · · ■ ■ · · ■ · · · ■ ■
            //· · ■ · ■ ■ ■ * ■ ■ · ■ · ■ · * ■ * * ■ * · · · ■ ■ ■ · ■ ■ ■ ■ · · ■ · ■ · ■ · · · ■ · ■ ■ ■ ■ · · · · · ■ · ■ · · · ■ · ■ · ■ · · · · · .
            //■ · · · ■ * * ■ · ■ · ■ ■ · · · * · · * · ■ · · · · · · · · · · · · ■ ■ · ■ · · · · · · · · · ■ ■ · ■ ■ · ■ · · · ■ · · · · ■ ■ ■ · ■ · · ■
            //· · ■ · * ■ ■ · · ■ · ■ · ■ · ■ ■ · ■ · · · · ■ ■ · ■ ■ ■ ■ · · · ■ ■ · ■ ■ · · · · · · ■ ■ · ■ · · ■ · · · · ■ · · · ■ · ■ ■ · · ■ ■ · ■ ■
            //· ■ ■ · * ■ · · · ■ · · · · · ■ · · ■ · · · · · · ■ · · ■ · · ■ ■ · · ■ ■ ■ · · · · · ■ · · · · ■ ■ ■ ■ ■ · · · · · · · ■ · ■ · · · · · · ■
            //■ ■ · * ■ · · · · ■ · · ■ ■ · · · · ■ ■ ■ · · · ■ ■ ■ ■ ■ · · · · · · · ■ ■ · · ■ ■ · · ■ · · · · · ■ · · · · · ■ ■ · ■ · · ■ · ■ ■ ■ ■ · ■
            //· · · * ■ · · · · · · ■ · ■ · · · · · · · ■ ■ · · · ■ · · · ■ ■ · · ■ · · · · · · ■ ■ · ■ · · · · · · ■ · · · ■ · ■ · · · · · ■ · · · · · .
            //· · * ■ · ■ · ■ ■ · ■ · · · ■ ■ · · ■ · · · ■ · ■ · · · · ■ ■ · ■ ■ ■ · · ■ · ■ · · · · · · · ■ ■ · · ■ ■ · ■ · · ■ · ■ · ■ · ■ · ■ · · · .
            //■ * ■ · · · · · ■ ■ · · ■ ■ ■ ■ ■ ■ · · · · · ■ · · · · · · · · · · · · ■ ■ · · · ■ · ■ · ■ · ■ · ■ · ■ ■ · · · ■ · ■ · · · · ■ · · ■ · · .
            //S · · · · · ■ · ■ · ■ · · · · · · ■ ■ · ■ · · ■ ■ · · · · · ■ · · ■ ■ · ■ · · · ■ · ■ · ■ · ■ · · · · · ■ ■ · · · · ■ · · ■ ■ ■ · · · ■ · .



            string[,,] map = MapCore.InitializeMap(70, 1, 40);
            map[6, 0, 0] = MapObjectType.FullCover;
            map[8, 0, 0] = MapObjectType.FullCover;
            map[10, 0, 0] = MapObjectType.FullCover;
            map[17, 0, 0] = MapObjectType.FullCover;
            map[18, 0, 0] = MapObjectType.FullCover;
            map[20, 0, 0] = MapObjectType.FullCover;
            map[23, 0, 0] = MapObjectType.FullCover;
            map[24, 0, 0] = MapObjectType.FullCover;
            map[30, 0, 0] = MapObjectType.FullCover;
            map[33, 0, 0] = MapObjectType.FullCover;
            map[34, 0, 0] = MapObjectType.FullCover;
            map[36, 0, 0] = MapObjectType.FullCover;
            map[40, 0, 0] = MapObjectType.FullCover;
            map[42, 0, 0] = MapObjectType.FullCover;
            map[44, 0, 0] = MapObjectType.FullCover;
            map[46, 0, 0] = MapObjectType.FullCover;
            map[52, 0, 0] = MapObjectType.FullCover;
            map[53, 0, 0] = MapObjectType.FullCover;
            map[58, 0, 0] = MapObjectType.FullCover;
            map[61, 0, 0] = MapObjectType.FullCover;
            map[62, 0, 0] = MapObjectType.FullCover;
            map[63, 0, 0] = MapObjectType.FullCover;
            map[67, 0, 0] = MapObjectType.FullCover;
            map[0, 0, 1] = MapObjectType.FullCover;
            map[2, 0, 1] = MapObjectType.FullCover;
            map[8, 0, 1] = MapObjectType.FullCover;
            map[9, 0, 1] = MapObjectType.FullCover;
            map[12, 0, 1] = MapObjectType.FullCover;
            map[13, 0, 1] = MapObjectType.FullCover;
            map[14, 0, 1] = MapObjectType.FullCover;
            map[15, 0, 1] = MapObjectType.FullCover;
            map[16, 0, 1] = MapObjectType.FullCover;
            map[17, 0, 1] = MapObjectType.FullCover;
            map[23, 0, 1] = MapObjectType.FullCover;
            map[36, 0, 1] = MapObjectType.FullCover;
            map[37, 0, 1] = MapObjectType.FullCover;
            map[41, 0, 1] = MapObjectType.FullCover;
            map[43, 0, 1] = MapObjectType.FullCover;
            map[45, 0, 1] = MapObjectType.FullCover;
            map[47, 0, 1] = MapObjectType.FullCover;
            map[49, 0, 1] = MapObjectType.FullCover;
            map[51, 0, 1] = MapObjectType.FullCover;
            map[52, 0, 1] = MapObjectType.FullCover;
            map[56, 0, 1] = MapObjectType.FullCover;
            map[58, 0, 1] = MapObjectType.FullCover;
            map[63, 0, 1] = MapObjectType.FullCover;
            map[66, 0, 1] = MapObjectType.FullCover;
            map[3, 0, 2] = MapObjectType.FullCover;
            map[5, 0, 2] = MapObjectType.FullCover;
            map[7, 0, 2] = MapObjectType.FullCover;
            map[8, 0, 2] = MapObjectType.FullCover;
            map[10, 0, 2] = MapObjectType.FullCover;
            map[14, 0, 2] = MapObjectType.FullCover;
            map[15, 0, 2] = MapObjectType.FullCover;
            map[18, 0, 2] = MapObjectType.FullCover;
            map[22, 0, 2] = MapObjectType.FullCover;
            map[24, 0, 2] = MapObjectType.FullCover;
            map[29, 0, 2] = MapObjectType.FullCover;
            map[30, 0, 2] = MapObjectType.FullCover;
            map[32, 0, 2] = MapObjectType.FullCover;
            map[33, 0, 2] = MapObjectType.FullCover;
            map[34, 0, 2] = MapObjectType.FullCover;
            map[37, 0, 2] = MapObjectType.FullCover;
            map[39, 0, 2] = MapObjectType.FullCover;
            map[47, 0, 2] = MapObjectType.FullCover;
            map[48, 0, 2] = MapObjectType.FullCover;
            map[51, 0, 2] = MapObjectType.FullCover;
            map[52, 0, 2] = MapObjectType.FullCover;
            map[54, 0, 2] = MapObjectType.FullCover;
            map[57, 0, 2] = MapObjectType.FullCover;
            map[59, 0, 2] = MapObjectType.FullCover;
            map[61, 0, 2] = MapObjectType.FullCover;
            map[63, 0, 2] = MapObjectType.FullCover;
            map[65, 0, 2] = MapObjectType.FullCover;
            map[4, 0, 3] = MapObjectType.FullCover;
            map[11, 0, 3] = MapObjectType.FullCover;
            map[13, 0, 3] = MapObjectType.FullCover;
            map[21, 0, 3] = MapObjectType.FullCover;
            map[22, 0, 3] = MapObjectType.FullCover;
            map[26, 0, 3] = MapObjectType.FullCover;
            map[30, 0, 3] = MapObjectType.FullCover;
            map[31, 0, 3] = MapObjectType.FullCover;
            map[34, 0, 3] = MapObjectType.FullCover;
            map[41, 0, 3] = MapObjectType.FullCover;
            map[42, 0, 3] = MapObjectType.FullCover;
            map[44, 0, 3] = MapObjectType.FullCover;
            map[51, 0, 3] = MapObjectType.FullCover;
            map[55, 0, 3] = MapObjectType.FullCover;
            map[57, 0, 3] = MapObjectType.FullCover;
            map[63, 0, 3] = MapObjectType.FullCover;
            map[0, 0, 4] = MapObjectType.FullCover;
            map[1, 0, 4] = MapObjectType.FullCover;
            map[4, 0, 4] = MapObjectType.FullCover;
            map[9, 0, 4] = MapObjectType.FullCover;
            map[12, 0, 4] = MapObjectType.FullCover;
            map[13, 0, 4] = MapObjectType.FullCover;
            map[18, 0, 4] = MapObjectType.FullCover;
            map[19, 0, 4] = MapObjectType.FullCover;
            map[20, 0, 4] = MapObjectType.FullCover;
            map[24, 0, 4] = MapObjectType.FullCover;
            map[25, 0, 4] = MapObjectType.FullCover;
            map[26, 0, 4] = MapObjectType.FullCover;
            map[27, 0, 4] = MapObjectType.FullCover;
            map[28, 0, 4] = MapObjectType.FullCover;
            map[36, 0, 4] = MapObjectType.FullCover;
            map[37, 0, 4] = MapObjectType.FullCover;
            map[40, 0, 4] = MapObjectType.FullCover;
            map[41, 0, 4] = MapObjectType.FullCover;
            map[44, 0, 4] = MapObjectType.FullCover;
            map[50, 0, 4] = MapObjectType.FullCover;
            map[56, 0, 4] = MapObjectType.FullCover;
            map[57, 0, 4] = MapObjectType.FullCover;
            map[59, 0, 4] = MapObjectType.FullCover;
            map[62, 0, 4] = MapObjectType.FullCover;
            map[64, 0, 4] = MapObjectType.FullCover;
            map[65, 0, 4] = MapObjectType.FullCover;
            map[66, 0, 4] = MapObjectType.FullCover;
            map[67, 0, 4] = MapObjectType.FullCover;
            map[69, 0, 4] = MapObjectType.FullCover;
            map[1, 0, 5] = MapObjectType.FullCover;
            map[2, 0, 5] = MapObjectType.FullCover;
            map[5, 0, 5] = MapObjectType.FullCover;
            map[9, 0, 5] = MapObjectType.FullCover;
            map[15, 0, 5] = MapObjectType.FullCover;
            map[18, 0, 5] = MapObjectType.FullCover;
            map[25, 0, 5] = MapObjectType.FullCover;
            map[28, 0, 5] = MapObjectType.FullCover;
            map[31, 0, 5] = MapObjectType.FullCover;
            map[32, 0, 5] = MapObjectType.FullCover;
            map[35, 0, 5] = MapObjectType.FullCover;
            map[36, 0, 5] = MapObjectType.FullCover;
            map[37, 0, 5] = MapObjectType.FullCover;
            map[43, 0, 5] = MapObjectType.FullCover;
            map[48, 0, 5] = MapObjectType.FullCover;
            map[49, 0, 5] = MapObjectType.FullCover;
            map[50, 0, 5] = MapObjectType.FullCover;
            map[51, 0, 5] = MapObjectType.FullCover;
            map[52, 0, 5] = MapObjectType.FullCover;
            map[60, 0, 5] = MapObjectType.FullCover;
            map[62, 0, 5] = MapObjectType.FullCover;
            map[69, 0, 5] = MapObjectType.FullCover;
            map[2, 0, 6] = MapObjectType.FullCover;
            map[5, 0, 6] = MapObjectType.FullCover;
            map[6, 0, 6] = MapObjectType.FullCover;
            map[9, 0, 6] = MapObjectType.FullCover;
            map[11, 0, 6] = MapObjectType.FullCover;
            map[13, 0, 6] = MapObjectType.FullCover;
            map[15, 0, 6] = MapObjectType.FullCover;
            map[16, 0, 6] = MapObjectType.FullCover;
            map[18, 0, 6] = MapObjectType.FullCover;
            map[23, 0, 6] = MapObjectType.FullCover;
            map[24, 0, 6] = MapObjectType.FullCover;
            map[26, 0, 6] = MapObjectType.FullCover;
            map[27, 0, 6] = MapObjectType.FullCover;
            map[28, 0, 6] = MapObjectType.FullCover;
            map[29, 0, 6] = MapObjectType.FullCover;
            map[33, 0, 6] = MapObjectType.FullCover;
            map[34, 0, 6] = MapObjectType.FullCover;
            map[36, 0, 6] = MapObjectType.FullCover;
            map[37, 0, 6] = MapObjectType.FullCover;
            map[44, 0, 6] = MapObjectType.FullCover;
            map[45, 0, 6] = MapObjectType.FullCover;
            map[47, 0, 6] = MapObjectType.FullCover;
            map[50, 0, 6] = MapObjectType.FullCover;
            map[55, 0, 6] = MapObjectType.FullCover;
            map[59, 0, 6] = MapObjectType.FullCover;
            map[61, 0, 6] = MapObjectType.FullCover;
            map[62, 0, 6] = MapObjectType.FullCover;
            map[65, 0, 6] = MapObjectType.FullCover;
            map[66, 0, 6] = MapObjectType.FullCover;
            map[68, 0, 6] = MapObjectType.FullCover;
            map[69, 0, 6] = MapObjectType.FullCover;
            map[0, 0, 7] = MapObjectType.FullCover;
            map[4, 0, 7] = MapObjectType.FullCover;
            map[7, 0, 7] = MapObjectType.FullCover;
            map[9, 0, 7] = MapObjectType.FullCover;
            map[11, 0, 7] = MapObjectType.FullCover;
            map[12, 0, 7] = MapObjectType.FullCover;
            map[21, 0, 7] = MapObjectType.FullCover;
            map[34, 0, 7] = MapObjectType.FullCover;
            map[35, 0, 7] = MapObjectType.FullCover;
            map[37, 0, 7] = MapObjectType.FullCover;
            map[47, 0, 7] = MapObjectType.FullCover;
            map[48, 0, 7] = MapObjectType.FullCover;
            map[50, 0, 7] = MapObjectType.FullCover;
            map[51, 0, 7] = MapObjectType.FullCover;
            map[53, 0, 7] = MapObjectType.FullCover;
            map[57, 0, 7] = MapObjectType.FullCover;
            map[62, 0, 7] = MapObjectType.FullCover;
            map[63, 0, 7] = MapObjectType.FullCover;
            map[64, 0, 7] = MapObjectType.FullCover;
            map[66, 0, 7] = MapObjectType.FullCover;
            map[69, 0, 7] = MapObjectType.FullCover;
            map[2, 0, 8] = MapObjectType.FullCover;
            map[4, 0, 8] = MapObjectType.FullCover;
            map[5, 0, 8] = MapObjectType.FullCover;
            map[6, 0, 8] = MapObjectType.FullCover;
            map[8, 0, 8] = MapObjectType.FullCover;
            map[9, 0, 8] = MapObjectType.FullCover;
            map[11, 0, 8] = MapObjectType.FullCover;
            map[13, 0, 8] = MapObjectType.FullCover;
            map[16, 0, 8] = MapObjectType.FullCover;
            map[19, 0, 8] = MapObjectType.FullCover;
            map[24, 0, 8] = MapObjectType.FullCover;
            map[25, 0, 8] = MapObjectType.FullCover;
            map[26, 0, 8] = MapObjectType.FullCover;
            map[28, 0, 8] = MapObjectType.FullCover;
            map[29, 0, 8] = MapObjectType.FullCover;
            map[30, 0, 8] = MapObjectType.FullCover;
            map[31, 0, 8] = MapObjectType.FullCover;
            map[34, 0, 8] = MapObjectType.FullCover;
            map[36, 0, 8] = MapObjectType.FullCover;
            map[38, 0, 8] = MapObjectType.FullCover;
            map[42, 0, 8] = MapObjectType.FullCover;
            map[44, 0, 8] = MapObjectType.FullCover;
            map[45, 0, 8] = MapObjectType.FullCover;
            map[46, 0, 8] = MapObjectType.FullCover;
            map[47, 0, 8] = MapObjectType.FullCover;
            map[53, 0, 8] = MapObjectType.FullCover;
            map[55, 0, 8] = MapObjectType.FullCover;
            map[59, 0, 8] = MapObjectType.FullCover;
            map[61, 0, 8] = MapObjectType.FullCover;
            map[63, 0, 8] = MapObjectType.FullCover;
            map[1, 0, 9] = MapObjectType.FullCover;
            map[5, 0, 9] = MapObjectType.FullCover;
            map[6, 0, 9] = MapObjectType.FullCover;
            map[10, 0, 9] = MapObjectType.FullCover;
            map[11, 0, 9] = MapObjectType.FullCover;
            map[16, 0, 9] = MapObjectType.FullCover;
            map[17, 0, 9] = MapObjectType.FullCover;
            map[19, 0, 9] = MapObjectType.FullCover;
            map[22, 0, 9] = MapObjectType.FullCover;
            map[25, 0, 9] = MapObjectType.FullCover;
            map[26, 0, 9] = MapObjectType.FullCover;
            map[27, 0, 9] = MapObjectType.FullCover;
            map[28, 0, 9] = MapObjectType.FullCover;
            map[31, 0, 9] = MapObjectType.FullCover;
            map[35, 0, 9] = MapObjectType.FullCover;
            map[39, 0, 9] = MapObjectType.FullCover;
            map[42, 0, 9] = MapObjectType.FullCover;
            map[43, 0, 9] = MapObjectType.FullCover;
            map[44, 0, 9] = MapObjectType.FullCover;
            map[47, 0, 9] = MapObjectType.FullCover;
            map[50, 0, 9] = MapObjectType.FullCover;
            map[51, 0, 9] = MapObjectType.FullCover;
            map[55, 0, 9] = MapObjectType.FullCover;
            map[56, 0, 9] = MapObjectType.FullCover;
            map[60, 0, 9] = MapObjectType.FullCover;
            map[61, 0, 9] = MapObjectType.FullCover;
            map[64, 0, 9] = MapObjectType.FullCover;
            map[68, 0, 9] = MapObjectType.FullCover;
            map[69, 0, 9] = MapObjectType.FullCover;
            map[0, 0, 10] = MapObjectType.FullCover;
            map[3, 0, 10] = MapObjectType.FullCover;
            map[6, 0, 10] = MapObjectType.FullCover;
            map[7, 0, 10] = MapObjectType.FullCover;
            map[9, 0, 10] = MapObjectType.FullCover;
            map[11, 0, 10] = MapObjectType.FullCover;
            map[13, 0, 10] = MapObjectType.FullCover;
            map[15, 0, 10] = MapObjectType.FullCover;
            map[17, 0, 10] = MapObjectType.FullCover;
            map[18, 0, 10] = MapObjectType.FullCover;
            map[19, 0, 10] = MapObjectType.FullCover;
            map[20, 0, 10] = MapObjectType.FullCover;
            map[23, 0, 10] = MapObjectType.FullCover;
            map[29, 0, 10] = MapObjectType.FullCover;
            map[30, 0, 10] = MapObjectType.FullCover;
            map[31, 0, 10] = MapObjectType.FullCover;
            map[33, 0, 10] = MapObjectType.FullCover;
            map[35, 0, 10] = MapObjectType.FullCover;
            map[37, 0, 10] = MapObjectType.FullCover;
            map[39, 0, 10] = MapObjectType.FullCover;
            map[41, 0, 10] = MapObjectType.FullCover;
            map[42, 0, 10] = MapObjectType.FullCover;
            map[45, 0, 10] = MapObjectType.FullCover;
            map[46, 0, 10] = MapObjectType.FullCover;
            map[47, 0, 10] = MapObjectType.FullCover;
            map[50, 0, 10] = MapObjectType.FullCover;
            map[55, 0, 10] = MapObjectType.FullCover;
            map[56, 0, 10] = MapObjectType.FullCover;
            map[58, 0, 10] = MapObjectType.FullCover;
            map[62, 0, 10] = MapObjectType.FullCover;
            map[66, 0, 10] = MapObjectType.FullCover;
            map[2, 0, 11] = MapObjectType.FullCover;
            map[3, 0, 11] = MapObjectType.FullCover;
            map[5, 0, 11] = MapObjectType.FullCover;
            map[7, 0, 11] = MapObjectType.FullCover;
            map[8, 0, 11] = MapObjectType.FullCover;
            map[14, 0, 11] = MapObjectType.FullCover;
            map[17, 0, 11] = MapObjectType.FullCover;
            map[19, 0, 11] = MapObjectType.FullCover;
            map[23, 0, 11] = MapObjectType.FullCover;
            map[25, 0, 11] = MapObjectType.FullCover;
            map[30, 0, 11] = MapObjectType.FullCover;
            map[33, 0, 11] = MapObjectType.FullCover;
            map[34, 0, 11] = MapObjectType.FullCover;
            map[36, 0, 11] = MapObjectType.FullCover;
            map[38, 0, 11] = MapObjectType.FullCover;
            map[41, 0, 11] = MapObjectType.FullCover;
            map[50, 0, 11] = MapObjectType.FullCover;
            map[52, 0, 11] = MapObjectType.FullCover;
            map[59, 0, 11] = MapObjectType.FullCover;
            map[60, 0, 11] = MapObjectType.FullCover;
            map[61, 0, 11] = MapObjectType.FullCover;
            map[62, 0, 11] = MapObjectType.FullCover;
            map[66, 0, 11] = MapObjectType.FullCover;
            map[2, 0, 12] = MapObjectType.FullCover;
            map[11, 0, 12] = MapObjectType.FullCover;
            map[12, 0, 12] = MapObjectType.FullCover;
            map[15, 0, 12] = MapObjectType.FullCover;
            map[17, 0, 12] = MapObjectType.FullCover;
            map[21, 0, 12] = MapObjectType.FullCover;
            map[24, 0, 12] = MapObjectType.FullCover;
            map[25, 0, 12] = MapObjectType.FullCover;
            map[26, 0, 12] = MapObjectType.FullCover;
            map[35, 0, 12] = MapObjectType.FullCover;
            map[37, 0, 12] = MapObjectType.FullCover;
            map[40, 0, 12] = MapObjectType.FullCover;
            map[41, 0, 12] = MapObjectType.FullCover;
            map[42, 0, 12] = MapObjectType.FullCover;
            map[43, 0, 12] = MapObjectType.FullCover;
            map[45, 0, 12] = MapObjectType.FullCover;
            map[46, 0, 12] = MapObjectType.FullCover;
            map[47, 0, 12] = MapObjectType.FullCover;
            map[48, 0, 12] = MapObjectType.FullCover;
            map[50, 0, 12] = MapObjectType.FullCover;
            map[51, 0, 12] = MapObjectType.FullCover;
            map[55, 0, 12] = MapObjectType.FullCover;
            map[56, 0, 12] = MapObjectType.FullCover;
            map[57, 0, 12] = MapObjectType.FullCover;
            map[58, 0, 12] = MapObjectType.FullCover;
            map[63, 0, 12] = MapObjectType.FullCover;
            map[64, 0, 12] = MapObjectType.FullCover;
            map[66, 0, 12] = MapObjectType.FullCover;
            map[67, 0, 12] = MapObjectType.FullCover;
            map[68, 0, 12] = MapObjectType.FullCover;
            map[0, 0, 13] = MapObjectType.FullCover;
            map[1, 0, 13] = MapObjectType.FullCover;
            map[2, 0, 13] = MapObjectType.FullCover;
            map[4, 0, 13] = MapObjectType.FullCover;
            map[6, 0, 13] = MapObjectType.FullCover;
            map[8, 0, 13] = MapObjectType.FullCover;
            map[9, 0, 13] = MapObjectType.FullCover;
            map[10, 0, 13] = MapObjectType.FullCover;
            map[13, 0, 13] = MapObjectType.FullCover;
            map[15, 0, 13] = MapObjectType.FullCover;
            map[16, 0, 13] = MapObjectType.FullCover;
            map[17, 0, 13] = MapObjectType.FullCover;
            map[18, 0, 13] = MapObjectType.FullCover;
            map[22, 0, 13] = MapObjectType.FullCover;
            map[23, 0, 13] = MapObjectType.FullCover;
            map[24, 0, 13] = MapObjectType.FullCover;
            map[26, 0, 13] = MapObjectType.FullCover;
            map[27, 0, 13] = MapObjectType.FullCover;
            map[28, 0, 13] = MapObjectType.FullCover;
            map[44, 0, 13] = MapObjectType.FullCover;
            map[46, 0, 13] = MapObjectType.FullCover;
            map[48, 0, 13] = MapObjectType.FullCover;
            map[51, 0, 13] = MapObjectType.FullCover;
            map[52, 0, 13] = MapObjectType.FullCover;
            map[55, 0, 13] = MapObjectType.FullCover;
            map[56, 0, 13] = MapObjectType.FullCover;
            map[57, 0, 13] = MapObjectType.FullCover;
            map[65, 0, 13] = MapObjectType.FullCover;
            map[66, 0, 13] = MapObjectType.FullCover;
            map[0, 0, 14] = MapObjectType.FullCover;
            map[1, 0, 14] = MapObjectType.FullCover;
            map[2, 0, 14] = MapObjectType.FullCover;
            map[5, 0, 14] = MapObjectType.FullCover;
            map[7, 0, 14] = MapObjectType.FullCover;
            map[8, 0, 14] = MapObjectType.FullCover;
            map[9, 0, 14] = MapObjectType.FullCover;
            map[10, 0, 14] = MapObjectType.FullCover;
            map[13, 0, 14] = MapObjectType.FullCover;
            map[14, 0, 14] = MapObjectType.FullCover;
            map[15, 0, 14] = MapObjectType.FullCover;
            map[18, 0, 14] = MapObjectType.FullCover;
            map[19, 0, 14] = MapObjectType.FullCover;
            map[25, 0, 14] = MapObjectType.FullCover;
            map[27, 0, 14] = MapObjectType.FullCover;
            map[28, 0, 14] = MapObjectType.FullCover;
            map[32, 0, 14] = MapObjectType.FullCover;
            map[33, 0, 14] = MapObjectType.FullCover;
            map[35, 0, 14] = MapObjectType.FullCover;
            map[38, 0, 14] = MapObjectType.FullCover;
            map[39, 0, 14] = MapObjectType.FullCover;
            map[41, 0, 14] = MapObjectType.FullCover;
            map[42, 0, 14] = MapObjectType.FullCover;
            map[43, 0, 14] = MapObjectType.FullCover;
            map[45, 0, 14] = MapObjectType.FullCover;
            map[48, 0, 14] = MapObjectType.FullCover;
            map[51, 0, 14] = MapObjectType.FullCover;
            map[59, 0, 14] = MapObjectType.FullCover;
            map[60, 0, 14] = MapObjectType.FullCover;
            map[63, 0, 14] = MapObjectType.FullCover;
            map[64, 0, 14] = MapObjectType.FullCover;
            map[68, 0, 14] = MapObjectType.FullCover;
            map[0, 0, 15] = MapObjectType.FullCover;
            map[1, 0, 15] = MapObjectType.FullCover;
            map[3, 0, 15] = MapObjectType.FullCover;
            map[5, 0, 15] = MapObjectType.FullCover;
            map[7, 0, 15] = MapObjectType.FullCover;
            map[8, 0, 15] = MapObjectType.FullCover;
            map[10, 0, 15] = MapObjectType.FullCover;
            map[14, 0, 15] = MapObjectType.FullCover;
            map[16, 0, 15] = MapObjectType.FullCover;
            map[19, 0, 15] = MapObjectType.FullCover;
            map[21, 0, 15] = MapObjectType.FullCover;
            map[22, 0, 15] = MapObjectType.FullCover;
            map[23, 0, 15] = MapObjectType.FullCover;
            map[26, 0, 15] = MapObjectType.FullCover;
            map[28, 0, 15] = MapObjectType.FullCover;
            map[31, 0, 15] = MapObjectType.FullCover;
            map[32, 0, 15] = MapObjectType.FullCover;
            map[34, 0, 15] = MapObjectType.FullCover;
            map[37, 0, 15] = MapObjectType.FullCover;
            map[39, 0, 15] = MapObjectType.FullCover;
            map[47, 0, 15] = MapObjectType.FullCover;
            map[49, 0, 15] = MapObjectType.FullCover;
            map[51, 0, 15] = MapObjectType.FullCover;
            map[53, 0, 15] = MapObjectType.FullCover;
            map[56, 0, 15] = MapObjectType.FullCover;
            map[58, 0, 15] = MapObjectType.FullCover;
            map[60, 0, 15] = MapObjectType.FullCover;
            map[62, 0, 15] = MapObjectType.FullCover;
            map[64, 0, 15] = MapObjectType.FullCover;
            map[65, 0, 15] = MapObjectType.FullCover;
            map[66, 0, 15] = MapObjectType.FullCover;
            map[7, 0, 16] = MapObjectType.FullCover;
            map[8, 0, 16] = MapObjectType.FullCover;
            map[10, 0, 16] = MapObjectType.FullCover;
            map[11, 0, 16] = MapObjectType.FullCover;
            map[12, 0, 16] = MapObjectType.FullCover;
            map[16, 0, 16] = MapObjectType.FullCover;
            map[17, 0, 16] = MapObjectType.FullCover;
            map[19, 0, 16] = MapObjectType.FullCover;
            map[22, 0, 16] = MapObjectType.FullCover;
            map[23, 0, 16] = MapObjectType.FullCover;
            map[26, 0, 16] = MapObjectType.FullCover;
            map[27, 0, 16] = MapObjectType.FullCover;
            map[31, 0, 16] = MapObjectType.FullCover;
            map[33, 0, 16] = MapObjectType.FullCover;
            map[35, 0, 16] = MapObjectType.FullCover;
            map[37, 0, 16] = MapObjectType.FullCover;
            map[40, 0, 16] = MapObjectType.FullCover;
            map[45, 0, 16] = MapObjectType.FullCover;
            map[51, 0, 16] = MapObjectType.FullCover;
            map[54, 0, 16] = MapObjectType.FullCover;
            map[63, 0, 16] = MapObjectType.FullCover;
            map[64, 0, 16] = MapObjectType.FullCover;
            map[65, 0, 16] = MapObjectType.FullCover;
            map[67, 0, 16] = MapObjectType.FullCover;
            map[0, 0, 17] = MapObjectType.FullCover;
            map[1, 0, 17] = MapObjectType.FullCover;
            map[3, 0, 17] = MapObjectType.FullCover;
            map[4, 0, 17] = MapObjectType.FullCover;
            map[6, 0, 17] = MapObjectType.FullCover;
            map[10, 0, 17] = MapObjectType.FullCover;
            map[19, 0, 17] = MapObjectType.FullCover;
            map[20, 0, 17] = MapObjectType.FullCover;
            map[25, 0, 17] = MapObjectType.FullCover;
            map[27, 0, 17] = MapObjectType.FullCover;
            map[28, 0, 17] = MapObjectType.FullCover;
            map[29, 0, 17] = MapObjectType.FullCover;
            map[37, 0, 17] = MapObjectType.FullCover;
            map[41, 0, 17] = MapObjectType.FullCover;
            map[45, 0, 17] = MapObjectType.FullCover;
            map[46, 0, 17] = MapObjectType.FullCover;
            map[47, 0, 17] = MapObjectType.FullCover;
            map[52, 0, 17] = MapObjectType.FullCover;
            map[55, 0, 17] = MapObjectType.FullCover;
            map[61, 0, 17] = MapObjectType.FullCover;
            map[62, 0, 17] = MapObjectType.FullCover;
            map[63, 0, 17] = MapObjectType.FullCover;
            map[64, 0, 17] = MapObjectType.FullCover;
            map[67, 0, 17] = MapObjectType.FullCover;
            map[68, 0, 17] = MapObjectType.FullCover;
            map[69, 0, 17] = MapObjectType.FullCover;
            map[0, 0, 18] = MapObjectType.FullCover;
            map[1, 0, 18] = MapObjectType.FullCover;
            map[2, 0, 18] = MapObjectType.FullCover;
            map[3, 0, 18] = MapObjectType.FullCover;
            map[5, 0, 18] = MapObjectType.FullCover;
            map[8, 0, 18] = MapObjectType.FullCover;
            map[16, 0, 18] = MapObjectType.FullCover;
            map[17, 0, 18] = MapObjectType.FullCover;
            map[19, 0, 18] = MapObjectType.FullCover;
            map[20, 0, 18] = MapObjectType.FullCover;
            map[31, 0, 18] = MapObjectType.FullCover;
            map[32, 0, 18] = MapObjectType.FullCover;
            map[33, 0, 18] = MapObjectType.FullCover;
            map[34, 0, 18] = MapObjectType.FullCover;
            map[35, 0, 18] = MapObjectType.FullCover;
            map[37, 0, 18] = MapObjectType.FullCover;
            map[40, 0, 18] = MapObjectType.FullCover;
            map[42, 0, 18] = MapObjectType.FullCover;
            map[45, 0, 18] = MapObjectType.FullCover;
            map[46, 0, 18] = MapObjectType.FullCover;
            map[47, 0, 18] = MapObjectType.FullCover;
            map[51, 0, 18] = MapObjectType.FullCover;
            map[52, 0, 18] = MapObjectType.FullCover;
            map[55, 0, 18] = MapObjectType.FullCover;
            map[58, 0, 18] = MapObjectType.FullCover;
            map[59, 0, 18] = MapObjectType.FullCover;
            map[60, 0, 18] = MapObjectType.FullCover;
            map[61, 0, 18] = MapObjectType.FullCover;
            map[62, 0, 18] = MapObjectType.FullCover;
            map[64, 0, 18] = MapObjectType.FullCover;
            map[67, 0, 18] = MapObjectType.FullCover;
            map[0, 0, 19] = MapObjectType.FullCover;
            map[3, 0, 19] = MapObjectType.FullCover;
            map[4, 0, 19] = MapObjectType.FullCover;
            map[5, 0, 19] = MapObjectType.FullCover;
            map[7, 0, 19] = MapObjectType.FullCover;
            map[10, 0, 19] = MapObjectType.FullCover;
            map[12, 0, 19] = MapObjectType.FullCover;
            map[15, 0, 19] = MapObjectType.FullCover;
            map[17, 0, 19] = MapObjectType.FullCover;
            map[23, 0, 19] = MapObjectType.FullCover;
            map[24, 0, 19] = MapObjectType.FullCover;
            map[25, 0, 19] = MapObjectType.FullCover;
            map[29, 0, 19] = MapObjectType.FullCover;
            map[33, 0, 19] = MapObjectType.FullCover;
            map[34, 0, 19] = MapObjectType.FullCover;
            map[37, 0, 19] = MapObjectType.FullCover;
            map[44, 0, 19] = MapObjectType.FullCover;
            map[47, 0, 19] = MapObjectType.FullCover;
            map[48, 0, 19] = MapObjectType.FullCover;
            map[51, 0, 19] = MapObjectType.FullCover;
            map[57, 0, 19] = MapObjectType.FullCover;
            map[59, 0, 19] = MapObjectType.FullCover;
            map[64, 0, 19] = MapObjectType.FullCover;
            map[65, 0, 19] = MapObjectType.FullCover;
            map[67, 0, 19] = MapObjectType.FullCover;
            map[7, 0, 20] = MapObjectType.FullCover;
            map[8, 0, 20] = MapObjectType.FullCover;
            map[10, 0, 20] = MapObjectType.FullCover;
            map[14, 0, 20] = MapObjectType.FullCover;
            map[15, 0, 20] = MapObjectType.FullCover;
            map[16, 0, 20] = MapObjectType.FullCover;
            map[21, 0, 20] = MapObjectType.FullCover;
            map[22, 0, 20] = MapObjectType.FullCover;
            map[24, 0, 20] = MapObjectType.FullCover;
            map[25, 0, 20] = MapObjectType.FullCover;
            map[26, 0, 20] = MapObjectType.FullCover;
            map[28, 0, 20] = MapObjectType.FullCover;
            map[29, 0, 20] = MapObjectType.FullCover;
            map[30, 0, 20] = MapObjectType.FullCover;
            map[33, 0, 20] = MapObjectType.FullCover;
            map[34, 0, 20] = MapObjectType.FullCover;
            map[35, 0, 20] = MapObjectType.FullCover;
            map[37, 0, 20] = MapObjectType.FullCover;
            map[39, 0, 20] = MapObjectType.FullCover;
            map[42, 0, 20] = MapObjectType.FullCover;
            map[45, 0, 20] = MapObjectType.FullCover;
            map[49, 0, 20] = MapObjectType.FullCover;
            map[50, 0, 20] = MapObjectType.FullCover;
            map[54, 0, 20] = MapObjectType.FullCover;
            map[55, 0, 20] = MapObjectType.FullCover;
            map[56, 0, 20] = MapObjectType.FullCover;
            map[57, 0, 20] = MapObjectType.FullCover;
            map[60, 0, 20] = MapObjectType.FullCover;
            map[61, 0, 20] = MapObjectType.FullCover;
            map[62, 0, 20] = MapObjectType.FullCover;
            map[64, 0, 20] = MapObjectType.FullCover;
            map[6, 0, 21] = MapObjectType.FullCover;
            map[9, 0, 21] = MapObjectType.FullCover;
            map[13, 0, 21] = MapObjectType.FullCover;
            map[14, 0, 21] = MapObjectType.FullCover;
            map[15, 0, 21] = MapObjectType.FullCover;
            map[20, 0, 21] = MapObjectType.FullCover;
            map[23, 0, 21] = MapObjectType.FullCover;
            map[27, 0, 21] = MapObjectType.FullCover;
            map[28, 0, 21] = MapObjectType.FullCover;
            map[29, 0, 21] = MapObjectType.FullCover;
            map[30, 0, 21] = MapObjectType.FullCover;
            map[31, 0, 21] = MapObjectType.FullCover;
            map[34, 0, 21] = MapObjectType.FullCover;
            map[35, 0, 21] = MapObjectType.FullCover;
            map[36, 0, 21] = MapObjectType.FullCover;
            map[37, 0, 21] = MapObjectType.FullCover;
            map[38, 0, 21] = MapObjectType.FullCover;
            map[39, 0, 21] = MapObjectType.FullCover;
            map[40, 0, 21] = MapObjectType.FullCover;
            map[42, 0, 21] = MapObjectType.FullCover;
            map[43, 0, 21] = MapObjectType.FullCover;
            map[44, 0, 21] = MapObjectType.FullCover;
            map[46, 0, 21] = MapObjectType.FullCover;
            map[49, 0, 21] = MapObjectType.FullCover;
            map[51, 0, 21] = MapObjectType.FullCover;
            map[55, 0, 21] = MapObjectType.FullCover;
            map[59, 0, 21] = MapObjectType.FullCover;
            map[65, 0, 21] = MapObjectType.FullCover;
            map[67, 0, 21] = MapObjectType.FullCover;
            map[0, 0, 22] = MapObjectType.FullCover;
            map[5, 0, 22] = MapObjectType.FullCover;
            map[6, 0, 22] = MapObjectType.FullCover;
            map[8, 0, 22] = MapObjectType.FullCover;
            map[10, 0, 22] = MapObjectType.FullCover;
            map[15, 0, 22] = MapObjectType.FullCover;
            map[19, 0, 22] = MapObjectType.FullCover;
            map[20, 0, 22] = MapObjectType.FullCover;
            map[25, 0, 22] = MapObjectType.FullCover;
            map[26, 0, 22] = MapObjectType.FullCover;
            map[30, 0, 22] = MapObjectType.FullCover;
            map[31, 0, 22] = MapObjectType.FullCover;
            map[34, 0, 22] = MapObjectType.FullCover;
            map[35, 0, 22] = MapObjectType.FullCover;
            map[37, 0, 22] = MapObjectType.FullCover;
            map[38, 0, 22] = MapObjectType.FullCover;
            map[40, 0, 22] = MapObjectType.FullCover;
            map[41, 0, 22] = MapObjectType.FullCover;
            map[42, 0, 22] = MapObjectType.FullCover;
            map[44, 0, 22] = MapObjectType.FullCover;
            map[46, 0, 22] = MapObjectType.FullCover;
            map[59, 0, 22] = MapObjectType.FullCover;
            map[60, 0, 22] = MapObjectType.FullCover;
            map[61, 0, 22] = MapObjectType.FullCover;
            map[62, 0, 22] = MapObjectType.FullCover;
            map[65, 0, 22] = MapObjectType.FullCover;
            map[66, 0, 22] = MapObjectType.FullCover;
            map[69, 0, 22] = MapObjectType.FullCover;
            map[4, 0, 23] = MapObjectType.FullCover;
            map[9, 0, 23] = MapObjectType.FullCover;
            map[10, 0, 23] = MapObjectType.FullCover;
            map[12, 0, 23] = MapObjectType.FullCover;
            map[13, 0, 23] = MapObjectType.FullCover;
            map[14, 0, 23] = MapObjectType.FullCover;
            map[17, 0, 23] = MapObjectType.FullCover;
            map[18, 0, 23] = MapObjectType.FullCover;
            map[24, 0, 23] = MapObjectType.FullCover;
            map[26, 0, 23] = MapObjectType.FullCover;
            map[28, 0, 23] = MapObjectType.FullCover;
            map[31, 0, 23] = MapObjectType.FullCover;
            map[35, 0, 23] = MapObjectType.FullCover;
            map[37, 0, 23] = MapObjectType.FullCover;
            map[40, 0, 23] = MapObjectType.FullCover;
            map[42, 0, 23] = MapObjectType.FullCover;
            map[43, 0, 23] = MapObjectType.FullCover;
            map[44, 0, 23] = MapObjectType.FullCover;
            map[46, 0, 23] = MapObjectType.FullCover;
            map[47, 0, 23] = MapObjectType.FullCover;
            map[49, 0, 23] = MapObjectType.FullCover;
            map[51, 0, 23] = MapObjectType.FullCover;
            map[55, 0, 23] = MapObjectType.FullCover;
            map[58, 0, 23] = MapObjectType.FullCover;
            map[59, 0, 23] = MapObjectType.FullCover;
            map[2, 0, 24] = MapObjectType.FullCover;
            map[11, 0, 24] = MapObjectType.FullCover;
            map[12, 0, 24] = MapObjectType.FullCover;
            map[13, 0, 24] = MapObjectType.FullCover;
            map[14, 0, 24] = MapObjectType.FullCover;
            map[17, 0, 24] = MapObjectType.FullCover;
            map[19, 0, 24] = MapObjectType.FullCover;
            map[21, 0, 24] = MapObjectType.FullCover;
            map[25, 0, 24] = MapObjectType.FullCover;
            map[26, 0, 24] = MapObjectType.FullCover;
            map[27, 0, 24] = MapObjectType.FullCover;
            map[28, 0, 24] = MapObjectType.FullCover;
            map[29, 0, 24] = MapObjectType.FullCover;
            map[30, 0, 24] = MapObjectType.FullCover;
            map[35, 0, 24] = MapObjectType.FullCover;
            map[37, 0, 24] = MapObjectType.FullCover;
            map[38, 0, 24] = MapObjectType.FullCover;
            map[39, 0, 24] = MapObjectType.FullCover;
            map[41, 0, 24] = MapObjectType.FullCover;
            map[44, 0, 24] = MapObjectType.FullCover;
            map[47, 0, 24] = MapObjectType.FullCover;
            map[49, 0, 24] = MapObjectType.FullCover;
            map[51, 0, 24] = MapObjectType.FullCover;
            map[53, 0, 24] = MapObjectType.FullCover;
            map[55, 0, 24] = MapObjectType.FullCover;
            map[62, 0, 24] = MapObjectType.FullCover;
            map[64, 0, 24] = MapObjectType.FullCover;
            map[65, 0, 24] = MapObjectType.FullCover;
            map[69, 0, 24] = MapObjectType.FullCover;
            map[1, 0, 25] = MapObjectType.FullCover;
            map[5, 0, 25] = MapObjectType.FullCover;
            map[6, 0, 25] = MapObjectType.FullCover;
            map[7, 0, 25] = MapObjectType.FullCover;
            map[13, 0, 25] = MapObjectType.FullCover;
            map[16, 0, 25] = MapObjectType.FullCover;
            map[17, 0, 25] = MapObjectType.FullCover;
            map[18, 0, 25] = MapObjectType.FullCover;
            map[19, 0, 25] = MapObjectType.FullCover;
            map[20, 0, 25] = MapObjectType.FullCover;
            map[21, 0, 25] = MapObjectType.FullCover;
            map[25, 0, 25] = MapObjectType.FullCover;
            map[27, 0, 25] = MapObjectType.FullCover;
            map[28, 0, 25] = MapObjectType.FullCover;
            map[29, 0, 25] = MapObjectType.FullCover;
            map[31, 0, 25] = MapObjectType.FullCover;
            map[33, 0, 25] = MapObjectType.FullCover;
            map[35, 0, 25] = MapObjectType.FullCover;
            map[39, 0, 25] = MapObjectType.FullCover;
            map[40, 0, 25] = MapObjectType.FullCover;
            map[43, 0, 25] = MapObjectType.FullCover;
            map[44, 0, 25] = MapObjectType.FullCover;
            map[47, 0, 25] = MapObjectType.FullCover;
            map[50, 0, 25] = MapObjectType.FullCover;
            map[54, 0, 25] = MapObjectType.FullCover;
            map[56, 0, 25] = MapObjectType.FullCover;
            map[58, 0, 25] = MapObjectType.FullCover;
            map[60, 0, 25] = MapObjectType.FullCover;
            map[61, 0, 25] = MapObjectType.FullCover;
            map[65, 0, 25] = MapObjectType.FullCover;
            map[69, 0, 25] = MapObjectType.FullCover;
            map[1, 0, 26] = MapObjectType.FullCover;
            map[4, 0, 26] = MapObjectType.FullCover;
            map[6, 0, 26] = MapObjectType.FullCover;
            map[8, 0, 26] = MapObjectType.FullCover;
            map[9, 0, 26] = MapObjectType.FullCover;
            map[12, 0, 26] = MapObjectType.FullCover;
            map[15, 0, 26] = MapObjectType.FullCover;
            map[18, 0, 26] = MapObjectType.FullCover;
            map[19, 0, 26] = MapObjectType.FullCover;
            map[23, 0, 26] = MapObjectType.FullCover;
            map[25, 0, 26] = MapObjectType.FullCover;
            map[28, 0, 26] = MapObjectType.FullCover;
            map[34, 0, 26] = MapObjectType.FullCover;
            map[35, 0, 26] = MapObjectType.FullCover;
            map[36, 0, 26] = MapObjectType.FullCover;
            map[40, 0, 26] = MapObjectType.FullCover;
            map[42, 0, 26] = MapObjectType.FullCover;
            map[53, 0, 26] = MapObjectType.FullCover;
            map[56, 0, 26] = MapObjectType.FullCover;
            map[57, 0, 26] = MapObjectType.FullCover;
            map[59, 0, 26] = MapObjectType.FullCover;
            map[62, 0, 26] = MapObjectType.FullCover;
            map[63, 0, 26] = MapObjectType.FullCover;
            map[64, 0, 26] = MapObjectType.FullCover;
            map[66, 0, 26] = MapObjectType.FullCover;
            map[4, 0, 27] = MapObjectType.FullCover;
            map[5, 0, 27] = MapObjectType.FullCover;
            map[8, 0, 27] = MapObjectType.FullCover;
            map[10, 0, 27] = MapObjectType.FullCover;
            map[14, 0, 27] = MapObjectType.FullCover;
            map[15, 0, 27] = MapObjectType.FullCover;
            map[16, 0, 27] = MapObjectType.FullCover;
            map[17, 0, 27] = MapObjectType.FullCover;
            map[18, 0, 27] = MapObjectType.FullCover;
            map[21, 0, 27] = MapObjectType.FullCover;
            map[25, 0, 27] = MapObjectType.FullCover;
            map[29, 0, 27] = MapObjectType.FullCover;
            map[30, 0, 27] = MapObjectType.FullCover;
            map[37, 0, 27] = MapObjectType.FullCover;
            map[38, 0, 27] = MapObjectType.FullCover;
            map[39, 0, 27] = MapObjectType.FullCover;
            map[43, 0, 27] = MapObjectType.FullCover;
            map[48, 0, 27] = MapObjectType.FullCover;
            map[49, 0, 27] = MapObjectType.FullCover;
            map[50, 0, 27] = MapObjectType.FullCover;
            map[54, 0, 27] = MapObjectType.FullCover;
            map[57, 0, 27] = MapObjectType.FullCover;
            map[58, 0, 27] = MapObjectType.FullCover;
            map[62, 0, 27] = MapObjectType.FullCover;
            map[63, 0, 27] = MapObjectType.FullCover;
            map[64, 0, 27] = MapObjectType.FullCover;
            map[69, 0, 27] = MapObjectType.FullCover;
            map[0, 0, 28] = MapObjectType.FullCover;
            map[1, 0, 28] = MapObjectType.FullCover;
            map[5, 0, 28] = MapObjectType.FullCover;
            map[6, 0, 28] = MapObjectType.FullCover;
            map[8, 0, 28] = MapObjectType.FullCover;
            map[10, 0, 28] = MapObjectType.FullCover;
            map[14, 0, 28] = MapObjectType.FullCover;
            map[15, 0, 28] = MapObjectType.FullCover;
            map[16, 0, 28] = MapObjectType.FullCover;
            map[19, 0, 28] = MapObjectType.FullCover;
            map[20, 0, 28] = MapObjectType.FullCover;
            map[24, 0, 28] = MapObjectType.FullCover;
            map[28, 0, 28] = MapObjectType.FullCover;
            map[29, 0, 28] = MapObjectType.FullCover;
            map[31, 0, 28] = MapObjectType.FullCover;
            map[32, 0, 28] = MapObjectType.FullCover;
            map[35, 0, 28] = MapObjectType.FullCover;
            map[40, 0, 28] = MapObjectType.FullCover;
            map[41, 0, 28] = MapObjectType.FullCover;
            map[42, 0, 28] = MapObjectType.FullCover;
            map[43, 0, 28] = MapObjectType.FullCover;
            map[45, 0, 28] = MapObjectType.FullCover;
            map[46, 0, 28] = MapObjectType.FullCover;
            map[47, 0, 28] = MapObjectType.FullCover;
            map[50, 0, 28] = MapObjectType.FullCover;
            map[52, 0, 28] = MapObjectType.FullCover;
            map[53, 0, 28] = MapObjectType.FullCover;
            map[54, 0, 28] = MapObjectType.FullCover;
            map[56, 0, 28] = MapObjectType.FullCover;
            map[57, 0, 28] = MapObjectType.FullCover;
            map[65, 0, 28] = MapObjectType.FullCover;
            map[69, 0, 28] = MapObjectType.FullCover;
            map[0, 0, 29] = MapObjectType.FullCover;
            map[8, 0, 29] = MapObjectType.FullCover;
            map[9, 0, 29] = MapObjectType.FullCover;
            map[11, 0, 29] = MapObjectType.FullCover;
            map[13, 0, 29] = MapObjectType.FullCover;
            map[14, 0, 29] = MapObjectType.FullCover;
            map[19, 0, 29] = MapObjectType.FullCover;
            map[22, 0, 29] = MapObjectType.FullCover;
            map[29, 0, 29] = MapObjectType.FullCover;
            map[30, 0, 29] = MapObjectType.FullCover;
            map[31, 0, 29] = MapObjectType.FullCover;
            map[32, 0, 29] = MapObjectType.FullCover;
            map[38, 0, 29] = MapObjectType.FullCover;
            map[39, 0, 29] = MapObjectType.FullCover;
            map[40, 0, 29] = MapObjectType.FullCover;
            map[42, 0, 29] = MapObjectType.FullCover;
            map[43, 0, 29] = MapObjectType.FullCover;
            map[44, 0, 29] = MapObjectType.FullCover;
            map[45, 0, 29] = MapObjectType.FullCover;
            map[49, 0, 29] = MapObjectType.FullCover;
            map[52, 0, 29] = MapObjectType.FullCover;
            map[54, 0, 29] = MapObjectType.FullCover;
            map[57, 0, 29] = MapObjectType.FullCover;
            map[58, 0, 29] = MapObjectType.FullCover;
            map[59, 0, 29] = MapObjectType.FullCover;
            map[60, 0, 29] = MapObjectType.FullCover;
            map[61, 0, 29] = MapObjectType.FullCover;
            map[62, 0, 29] = MapObjectType.FullCover;
            map[64, 0, 29] = MapObjectType.FullCover;
            map[65, 0, 29] = MapObjectType.FullCover;
            map[1, 0, 30] = MapObjectType.FullCover;
            map[4, 0, 30] = MapObjectType.FullCover;
            map[5, 0, 30] = MapObjectType.FullCover;
            map[9, 0, 30] = MapObjectType.FullCover;
            map[10, 0, 30] = MapObjectType.FullCover;
            map[11, 0, 30] = MapObjectType.FullCover;
            map[13, 0, 30] = MapObjectType.FullCover;
            map[15, 0, 30] = MapObjectType.FullCover;
            map[16, 0, 30] = MapObjectType.FullCover;
            map[20, 0, 30] = MapObjectType.FullCover;
            map[22, 0, 30] = MapObjectType.FullCover;
            map[23, 0, 30] = MapObjectType.FullCover;
            map[25, 0, 30] = MapObjectType.FullCover;
            map[27, 0, 30] = MapObjectType.FullCover;
            map[28, 0, 30] = MapObjectType.FullCover;
            map[29, 0, 30] = MapObjectType.FullCover;
            map[33, 0, 30] = MapObjectType.FullCover;
            map[34, 0, 30] = MapObjectType.FullCover;
            map[35, 0, 30] = MapObjectType.FullCover;
            map[39, 0, 30] = MapObjectType.FullCover;
            map[40, 0, 30] = MapObjectType.FullCover;
            map[43, 0, 30] = MapObjectType.FullCover;
            map[45, 0, 30] = MapObjectType.FullCover;
            map[46, 0, 30] = MapObjectType.FullCover;
            map[48, 0, 30] = MapObjectType.FullCover;
            map[50, 0, 30] = MapObjectType.FullCover;
            map[51, 0, 30] = MapObjectType.FullCover;
            map[52, 0, 30] = MapObjectType.FullCover;
            map[54, 0, 30] = MapObjectType.FullCover;
            map[56, 0, 30] = MapObjectType.FullCover;
            map[57, 0, 30] = MapObjectType.FullCover;
            map[59, 0, 30] = MapObjectType.FullCover;
            map[61, 0, 30] = MapObjectType.FullCover;
            map[66, 0, 30] = MapObjectType.FullCover;
            map[1, 0, 31] = MapObjectType.FullCover;
            map[8, 0, 31] = MapObjectType.FullCover;
            map[12, 0, 31] = MapObjectType.FullCover;
            map[13, 0, 31] = MapObjectType.FullCover;
            map[15, 0, 31] = MapObjectType.FullCover;
            map[17, 0, 31] = MapObjectType.FullCover;
            map[21, 0, 31] = MapObjectType.FullCover;
            map[22, 0, 31] = MapObjectType.FullCover;
            map[27, 0, 31] = MapObjectType.FullCover;
            map[30, 0, 31] = MapObjectType.FullCover;
            map[38, 0, 31] = MapObjectType.FullCover;
            map[41, 0, 31] = MapObjectType.FullCover;
            map[46, 0, 31] = MapObjectType.FullCover;
            map[47, 0, 31] = MapObjectType.FullCover;
            map[51, 0, 31] = MapObjectType.FullCover;
            map[53, 0, 31] = MapObjectType.FullCover;
            map[57, 0, 31] = MapObjectType.FullCover;
            map[58, 0, 31] = MapObjectType.FullCover;
            map[62, 0, 31] = MapObjectType.FullCover;
            map[66, 0, 31] = MapObjectType.FullCover;
            map[69, 0, 31] = MapObjectType.FullCover;
            map[1, 0, 32] = MapObjectType.FullCover;
            map[3, 0, 32] = MapObjectType.FullCover;
            map[4, 0, 32] = MapObjectType.FullCover;
            map[6, 0, 32] = MapObjectType.FullCover;
            map[8, 0, 32] = MapObjectType.FullCover;
            map[13, 0, 32] = MapObjectType.FullCover;
            map[16, 0, 32] = MapObjectType.FullCover;
            map[19, 0, 32] = MapObjectType.FullCover;
            map[21, 0, 32] = MapObjectType.FullCover;
            map[24, 0, 32] = MapObjectType.FullCover;
            map[26, 0, 32] = MapObjectType.FullCover;
            map[27, 0, 32] = MapObjectType.FullCover;
            map[28, 0, 32] = MapObjectType.FullCover;
            map[31, 0, 32] = MapObjectType.FullCover;
            map[36, 0, 32] = MapObjectType.FullCover;
            map[39, 0, 32] = MapObjectType.FullCover;
            map[40, 0, 32] = MapObjectType.FullCover;
            map[42, 0, 32] = MapObjectType.FullCover;
            map[45, 0, 32] = MapObjectType.FullCover;
            map[46, 0, 32] = MapObjectType.FullCover;
            map[47, 0, 32] = MapObjectType.FullCover;
            map[48, 0, 32] = MapObjectType.FullCover;
            map[51, 0, 32] = MapObjectType.FullCover;
            map[53, 0, 32] = MapObjectType.FullCover;
            map[54, 0, 32] = MapObjectType.FullCover;
            map[57, 0, 32] = MapObjectType.FullCover;
            map[61, 0, 32] = MapObjectType.FullCover;
            map[62, 0, 32] = MapObjectType.FullCover;
            map[64, 0, 32] = MapObjectType.FullCover;
            map[67, 0, 32] = MapObjectType.FullCover;
            map[69, 0, 32] = MapObjectType.FullCover;
            map[2, 0, 33] = MapObjectType.FullCover;
            map[4, 0, 33] = MapObjectType.FullCover;
            map[5, 0, 33] = MapObjectType.FullCover;
            map[6, 0, 33] = MapObjectType.FullCover;
            map[7, 0, 33] = MapObjectType.FullCover;
            map[9, 0, 33] = MapObjectType.FullCover;
            map[10, 0, 33] = MapObjectType.FullCover;
            map[11, 0, 33] = MapObjectType.FullCover;
            map[14, 0, 33] = MapObjectType.FullCover;
            map[18, 0, 33] = MapObjectType.FullCover;
            map[19, 0, 33] = MapObjectType.FullCover;
            map[23, 0, 33] = MapObjectType.FullCover;
            map[26, 0, 33] = MapObjectType.FullCover;
            map[28, 0, 33] = MapObjectType.FullCover;
            map[31, 0, 33] = MapObjectType.FullCover;
            map[33, 0, 33] = MapObjectType.FullCover;
            map[34, 0, 33] = MapObjectType.FullCover;
            map[36, 0, 33] = MapObjectType.FullCover;
            map[37, 0, 33] = MapObjectType.FullCover;
            map[43, 0, 33] = MapObjectType.FullCover;
            map[44, 0, 33] = MapObjectType.FullCover;
            map[48, 0, 33] = MapObjectType.FullCover;
            map[50, 0, 33] = MapObjectType.FullCover;
            map[53, 0, 33] = MapObjectType.FullCover;
            map[54, 0, 33] = MapObjectType.FullCover;
            map[58, 0, 33] = MapObjectType.FullCover;
            map[59, 0, 33] = MapObjectType.FullCover;
            map[65, 0, 33] = MapObjectType.FullCover;
            map[68, 0, 33] = MapObjectType.FullCover;
            map[0, 0, 34] = MapObjectType.FullCover;
            map[1, 0, 34] = MapObjectType.FullCover;
            map[3, 0, 34] = MapObjectType.FullCover;
            map[5, 0, 34] = MapObjectType.FullCover;
            map[9, 0, 34] = MapObjectType.FullCover;
            map[10, 0, 34] = MapObjectType.FullCover;
            map[12, 0, 34] = MapObjectType.FullCover;
            map[17, 0, 34] = MapObjectType.FullCover;
            map[18, 0, 34] = MapObjectType.FullCover;
            map[21, 0, 34] = MapObjectType.FullCover;
            map[26, 0, 34] = MapObjectType.FullCover;
            map[29, 0, 34] = MapObjectType.FullCover;
            map[30, 0, 34] = MapObjectType.FullCover;
            map[32, 0, 34] = MapObjectType.FullCover;
            map[34, 0, 34] = MapObjectType.FullCover;
            map[40, 0, 34] = MapObjectType.FullCover;
            map[42, 0, 34] = MapObjectType.FullCover;
            map[43, 0, 34] = MapObjectType.FullCover;
            map[44, 0, 34] = MapObjectType.FullCover;
            map[45, 0, 34] = MapObjectType.FullCover;
            map[48, 0, 34] = MapObjectType.FullCover;
            map[49, 0, 34] = MapObjectType.FullCover;
            map[51, 0, 34] = MapObjectType.FullCover;
            map[55, 0, 34] = MapObjectType.FullCover;
            map[58, 0, 34] = MapObjectType.FullCover;
            map[60, 0, 34] = MapObjectType.FullCover;
            map[62, 0, 34] = MapObjectType.FullCover;
            map[64, 0, 34] = MapObjectType.FullCover;
            map[65, 0, 34] = MapObjectType.FullCover;
            map[67, 0, 34] = MapObjectType.FullCover;
            map[1, 0, 35] = MapObjectType.FullCover;
            map[5, 0, 35] = MapObjectType.FullCover;
            map[11, 0, 35] = MapObjectType.FullCover;
            map[12, 0, 35] = MapObjectType.FullCover;
            map[13, 0, 35] = MapObjectType.FullCover;
            map[18, 0, 35] = MapObjectType.FullCover;
            map[19, 0, 35] = MapObjectType.FullCover;
            map[20, 0, 35] = MapObjectType.FullCover;
            map[21, 0, 35] = MapObjectType.FullCover;
            map[22, 0, 35] = MapObjectType.FullCover;
            map[25, 0, 35] = MapObjectType.FullCover;
            map[28, 0, 35] = MapObjectType.FullCover;
            map[31, 0, 35] = MapObjectType.FullCover;
            map[32, 0, 35] = MapObjectType.FullCover;
            map[33, 0, 35] = MapObjectType.FullCover;
            map[34, 0, 35] = MapObjectType.FullCover;
            map[37, 0, 35] = MapObjectType.FullCover;
            map[38, 0, 35] = MapObjectType.FullCover;
            map[40, 0, 35] = MapObjectType.FullCover;
            map[41, 0, 35] = MapObjectType.FullCover;
            map[42, 0, 35] = MapObjectType.FullCover;
            map[44, 0, 35] = MapObjectType.FullCover;
            map[49, 0, 35] = MapObjectType.FullCover;
            map[52, 0, 35] = MapObjectType.FullCover;
            map[53, 0, 35] = MapObjectType.FullCover;
            map[59, 0, 35] = MapObjectType.FullCover;
            map[61, 0, 35] = MapObjectType.FullCover;
            map[62, 0, 35] = MapObjectType.FullCover;
            map[64, 0, 35] = MapObjectType.FullCover;
            map[65, 0, 35] = MapObjectType.FullCover;
            map[67, 0, 35] = MapObjectType.FullCover;
            map[69, 0, 35] = MapObjectType.FullCover;
            map[0, 0, 36] = MapObjectType.FullCover;
            map[2, 0, 36] = MapObjectType.FullCover;
            map[4, 0, 36] = MapObjectType.FullCover;
            map[5, 0, 36] = MapObjectType.FullCover;
            map[6, 0, 36] = MapObjectType.FullCover;
            map[12, 0, 36] = MapObjectType.FullCover;
            map[13, 0, 36] = MapObjectType.FullCover;
            map[14, 0, 36] = MapObjectType.FullCover;
            map[17, 0, 36] = MapObjectType.FullCover;
            map[22, 0, 36] = MapObjectType.FullCover;
            map[29, 0, 36] = MapObjectType.FullCover;
            map[35, 0, 36] = MapObjectType.FullCover;
            map[41, 0, 36] = MapObjectType.FullCover;
            map[42, 0, 36] = MapObjectType.FullCover;
            map[43, 0, 36] = MapObjectType.FullCover;
            map[45, 0, 36] = MapObjectType.FullCover;
            map[46, 0, 36] = MapObjectType.FullCover;
            map[47, 0, 36] = MapObjectType.FullCover;
            map[49, 0, 36] = MapObjectType.FullCover;
            map[53, 0, 36] = MapObjectType.FullCover;
            map[57, 0, 36] = MapObjectType.FullCover;
            map[60, 0, 36] = MapObjectType.FullCover;
            map[62, 0, 36] = MapObjectType.FullCover;
            map[68, 0, 36] = MapObjectType.FullCover;
            map[0, 0, 37] = MapObjectType.FullCover;
            map[2, 0, 37] = MapObjectType.FullCover;
            map[4, 0, 37] = MapObjectType.FullCover;
            map[5, 0, 37] = MapObjectType.FullCover;
            map[6, 0, 37] = MapObjectType.FullCover;
            map[8, 0, 37] = MapObjectType.FullCover;
            map[12, 0, 37] = MapObjectType.FullCover;
            map[13, 0, 37] = MapObjectType.FullCover;
            map[19, 0, 37] = MapObjectType.FullCover;
            map[21, 0, 37] = MapObjectType.FullCover;
            map[24, 0, 37] = MapObjectType.FullCover;
            map[26, 0, 37] = MapObjectType.FullCover;
            map[40, 0, 37] = MapObjectType.FullCover;
            map[42, 0, 37] = MapObjectType.FullCover;
            map[50, 0, 37] = MapObjectType.FullCover;
            map[52, 0, 37] = MapObjectType.FullCover;
            map[53, 0, 37] = MapObjectType.FullCover;
            map[54, 0, 37] = MapObjectType.FullCover;
            map[62, 0, 37] = MapObjectType.FullCover;
            map[63, 0, 37] = MapObjectType.FullCover;
            map[65, 0, 37] = MapObjectType.FullCover;
            map[66, 0, 37] = MapObjectType.FullCover;
            map[68, 0, 37] = MapObjectType.FullCover;
            map[0, 0, 38] = MapObjectType.FullCover;
            map[1, 0, 38] = MapObjectType.FullCover;
            map[3, 0, 38] = MapObjectType.FullCover;
            map[5, 0, 38] = MapObjectType.FullCover;
            map[6, 0, 38] = MapObjectType.FullCover;
            map[10, 0, 38] = MapObjectType.FullCover;
            map[11, 0, 38] = MapObjectType.FullCover;
            map[13, 0, 38] = MapObjectType.FullCover;
            map[19, 0, 38] = MapObjectType.FullCover;
            map[27, 0, 38] = MapObjectType.FullCover;
            map[31, 0, 38] = MapObjectType.FullCover;
            map[33, 0, 38] = MapObjectType.FullCover;
            map[37, 0, 38] = MapObjectType.FullCover;
            map[44, 0, 38] = MapObjectType.FullCover;
            map[47, 0, 38] = MapObjectType.FullCover;
            map[48, 0, 38] = MapObjectType.FullCover;
            map[52, 0, 38] = MapObjectType.FullCover;
            map[53, 0, 38] = MapObjectType.FullCover;
            map[54, 0, 38] = MapObjectType.FullCover;
            map[58, 0, 38] = MapObjectType.FullCover;
            map[61, 0, 38] = MapObjectType.FullCover;
            map[65, 0, 38] = MapObjectType.FullCover;
            map[69, 0, 38] = MapObjectType.FullCover;
            map[1, 0, 39] = MapObjectType.FullCover;
            map[5, 0, 39] = MapObjectType.FullCover;
            map[6, 0, 39] = MapObjectType.FullCover;
            map[8, 0, 39] = MapObjectType.FullCover;
            map[9, 0, 39] = MapObjectType.FullCover;
            map[10, 0, 39] = MapObjectType.FullCover;
            map[11, 0, 39] = MapObjectType.FullCover;
            map[15, 0, 39] = MapObjectType.FullCover;
            map[17, 0, 39] = MapObjectType.FullCover;
            map[21, 0, 39] = MapObjectType.FullCover;
            map[22, 0, 39] = MapObjectType.FullCover;
            map[25, 0, 39] = MapObjectType.FullCover;
            map[30, 0, 39] = MapObjectType.FullCover;
            map[34, 0, 39] = MapObjectType.FullCover;
            map[36, 0, 39] = MapObjectType.FullCover;
            map[38, 0, 39] = MapObjectType.FullCover;
            map[40, 0, 39] = MapObjectType.FullCover;
            map[41, 0, 39] = MapObjectType.FullCover;
            map[42, 0, 39] = MapObjectType.FullCover;
            map[43, 0, 39] = MapObjectType.FullCover;
            map[44, 0, 39] = MapObjectType.FullCover;
            map[46, 0, 39] = MapObjectType.FullCover;
            map[47, 0, 39] = MapObjectType.FullCover;
            map[48, 0, 39] = MapObjectType.FullCover;
            map[51, 0, 39] = MapObjectType.FullCover;
            map[52, 0, 39] = MapObjectType.FullCover;
            map[53, 0, 39] = MapObjectType.FullCover;
            map[58, 0, 39] = MapObjectType.FullCover;
            map[64, 0, 39] = MapObjectType.FullCover;
            map[65, 0, 39] = MapObjectType.FullCover;

            return map;
        }
        #endregion

        #endregion

    }
}
