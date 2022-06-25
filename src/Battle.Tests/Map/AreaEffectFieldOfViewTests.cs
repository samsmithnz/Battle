using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class AreaEffectFieldOfViewTests
    {
        [TestMethod]
        public void Radius1AreaOfEffectAt4x4zTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(5, 1, 5);
            Vector3 location = new(4, 0, 4);
            int radius = 1;

            //Act
            List<Vector3> results = MapCore.GetMapArea(map, location, radius, false, true);
            string mapString = MapCore.GetMapStringWithItems(map, results);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(4, results.Count);
            Assert.AreEqual(new(3, 0, 3), results[0]);
            Assert.AreEqual(new(3, 0, 4), results[1]);
            Assert.AreEqual(new(4, 0, 3), results[2]);
            Assert.AreEqual(location, results[3]);
            string mapStringExpected = @"
· · · o o 
· · · o o 
· · · · · 
· · · · · 
· · · · · 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void Radius1AreaOfEffectAt1x1zTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(5, 1, 5);
            Vector3 location = new(1, 0, 1);
            int radius = 1;

            //Act
            List<Vector3> results = MapCore.GetMapArea(map, location, radius, false, true);
            string mapString = MapCore.GetMapStringWithItems(map, results);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(9, results.Count);
            Assert.AreEqual(new(0, 0, 0), results[0]);
            Assert.AreEqual(new(0, 0, 2), results[1]);
            Assert.AreEqual(new(1, 0, 0), results[2]);
            Assert.AreEqual(new(1, 0, 2), results[3]);
            Assert.AreEqual(new(2, 0, 0), results[4]);
            Assert.AreEqual(location, results[8]);
            string mapStringExpected = @"
· · · · · 
· · · · · 
o o o · · 
o o o · · 
o o o · · 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void Radius1AreaOfEffectAt0x0zTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(5, 1, 5);
            Vector3 location = new(0, 0, 0);
            int radius = 1;

            //Act
            List<Vector3> results = MapCore.GetMapArea(map, location, radius, false, true);
            string mapString = MapCore.GetMapStringWithItems(map, results);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(4, results.Count);
            Assert.AreEqual(new(0, 0, 1), results[0]);
            Assert.AreEqual(new(1, 0, 0), results[1]);
            Assert.AreEqual(new(1, 0, 1), results[2]);
            Assert.AreEqual(location, results[3]);
            string mapStringExpected = @"
· · · · · 
· · · · · 
· · · · · 
o o · · · 
o o · · · 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }


        [TestMethod]
        public void Radius3AreaOfEffectAt4x4zTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 location = new(4, 0, 4);
            int radius = 3;

            //Act
            List<Vector3> results = MapCore.GetMapArea(map, location, radius, false, true);
            string mapString = MapCore.GetMapStringWithItems(map, results);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(45, results.Count);
            Assert.AreEqual(new(3, 0, 3), results[0]);
            Assert.AreEqual(new(7, 0, 6), results[43]);
            Assert.AreEqual(location, results[44]);
            string mapStringExpected = @"
· · · · · · · · · · 
· · · · · · · · · · 
· · o o o o o · · · 
· o o o o o o o · · 
· o o o o o o o · · 
· o o o o o o o · · 
· o o o o o o o · · 
· o o o o o o o · · 
· · o o o o o · · · 
· · · · · · · · · · 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void Radius3AreaOfEffectAt0x0zTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Vector3 location = new(0, 0, 0);
            int radius = 3;

            //Act
            List<Vector3> results = MapCore.GetMapArea(map, location, radius, false, true);
            string mapString = MapCore.GetMapStringWithItems(map, results);

            //Assert
            Assert.IsTrue(results != null);
            Assert.AreEqual(15, results.Count);
            Assert.AreEqual(new(0, 0, 1), results[0]);
            Assert.AreEqual(new(3, 0, 2), results[13]);
            Assert.AreEqual(location, results[14]);
            string mapStringExpected = @"
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
o o o · · · · · · · 
o o o o · · · · · · 
o o o o · · · · · · 
o o o o · · · · · · 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }



    }
}
