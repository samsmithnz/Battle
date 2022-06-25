using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class FieldOfViewTests
    {
        [TestMethod]
        public void BasicShallowLineWithNoCoverTest()
        {
            //Arrange
            Vector3 source = new(1, 0, 3);
            Vector3 target = new(4, 0, 2);

            //Act
            List<Vector3> results = FieldOfView.GetPointsOnLine(source, target);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(4, results.Count);
            Assert.AreEqual(new(1, 0, 3), results[0]);
            Assert.AreEqual(new(2, 0, 3), results[1]);
            Assert.AreEqual(new(3, 0, 2), results[2]);
            Assert.AreEqual(new(4, 0, 2), results[3]);
        }

        [TestMethod]
        public void BasicSteepLineWithNoCoverTest()
        {
            //Arrange
            Vector3 source = new(1, 0, 3);
            Vector3 target = new(3, 0, 1);

            //Act
            List<Vector3> results = FieldOfView.GetPointsOnLine(source, target);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual(new(1, 0, 3), results[0]);
            Assert.AreEqual(new(2, 0, 2), results[1]);
            Assert.AreEqual(new(3, 0, 1), results[2]);
        }

        [TestMethod]
        public void BasicShallowLineWithCoverTest()
        {
            //Arrange
            Vector3 source = new(1, 0, 3);
            Vector3 target = new(4, 0, 2);
            string[,,] map = MapCore.InitializeMap(5, 1, 5);
            map[3, 0, 2] = MapObjectType.FullCover;

            //Act
            List<Vector3> results = FieldOfView.GetPointsOnLine(source, target);
            List<Vector3> newResults = new();
            foreach (Vector3 item in results)
            {
                if (map[(int)item.X, (int)item.Y, (int)item.Z] != "")
                {
                    break;
                }
                else
                {
                    newResults.Add(item);
                }
            }

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(2, newResults.Count);
            Assert.AreEqual(new(1, 0, 3), newResults[0]);
            Assert.AreEqual(new(2, 0, 3), newResults[1]);
        }

        [TestMethod]
        public void FieldOfViewRange1NNoCoverTest()
        {
            //Arrange            
            //  "P" = player/fred
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · P · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            int range = 1;
            Vector3 startingLocation = new(4, 0, 4);

            //Act
            List<Vector3> results = FieldOfView.GetFieldOfView(map, startingLocation, range);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(8, results.Count);
        }



        [TestMethod]
        public void FieldOfViewRange1WithCoverTest()
        {
            //Arrange            
            //  "P" = player/fred
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · ■ P ■ · · .
            //  · · · · ■ ■ ■ · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            int range = 1;
            Vector3 startingLocation = new(4, 0, 4);
            map[3, 0, 4] = MapObjectType.FullCover;
            map[3, 0, 3] = MapObjectType.FullCover;
            map[4, 0, 3] = MapObjectType.FullCover;
            map[5, 0, 3] = MapObjectType.FullCover;
            map[5, 0, 4] = MapObjectType.FullCover;

            //Act
            List<Vector3> results = FieldOfView.GetFieldOfView(map, startingLocation, range);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(8, results.Count);
        }

        [TestMethod]
        public void FieldOfViewRange12NoCoverTest()
        {
            //Arrange            
            //  "P" = player/fred
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · P · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            //  · · · · · · · · · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            int range = 12;
            Vector3 startingLocation = new(4, 0, 4);

            //Act
            List<Vector3> results = FieldOfView.GetFieldOfView(map, startingLocation, range);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(99, results.Count);
        }

        [TestMethod]
        public void FieldOfViewRange5WithCoverTest()
        {
            //Arrange            
            //  "P" = player/fred
            //  CoverType.FullCover = cover
            //  "." = open ground

            //  · · · · · 
            //  · · · · · 
            //  · ■ P ■ · 
            //  · ■ ■ ■ · 
            //  · · · · · 
            string[,,] map = MapCore.InitializeMap(5, 1, 5);
            int range = 10;
            Vector3 startingLocation = new(2, 0, 2);
            map[1, 0, 2] = MapObjectType.FullCover;
            map[1, 0, 1] = MapObjectType.FullCover;
            map[2, 0, 1] = MapObjectType.FullCover;
            map[3, 0, 1] = MapObjectType.FullCover;
            map[3, 0, 2] = MapObjectType.FullCover;

            //Act
            List<Vector3> results = FieldOfView.GetFieldOfView(map, startingLocation, range);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(14, results.Count);
            //foreach (Vector3 item in results)
            //{
            //    Assert.AreNotEqual(0, item.Z);
            //    Assert.AreNotEqual(1, item.Z);
            //    Assert.AreNotEqual(2, item.Z);
            //}
        }

        [TestMethod]
        public void LineLengthTest()
        {
            //Arrange            
            Vector3 start = new(4, 0, 4);
            Vector3 end1 = new(7, 0, 4);
            Vector3 end2 = new(7, 0, 5);
            Vector3 end3 = new(7, 0, 6);
            Vector3 end4 = new(7, 0, 7);
            int decimals = 1;

            //Act
            double result1 = MapCore.GetLengthOfLine(start, end1, decimals);
            double result2 = MapCore.GetLengthOfLine(start, end2, decimals);
            double result3 = MapCore.GetLengthOfLine(start, end3, decimals);
            double result4 = MapCore.GetLengthOfLine(start, end4, decimals);

            //Assert
            Assert.AreEqual(3, result1);
            Assert.AreEqual(3.2, result2);
            Assert.AreEqual(3.6, result3);
            Assert.AreEqual(4.2, result4);
        }

    }
}
