using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace Battle.Tests.Map
{
    [TestClass]
    [TestCategory("L0")]
    public class MissedShotsTests
    {
        [TestMethod]
        public void NorthEastTargetTest()
        {
            //Arrange
            Vector3 source = new(1, 0, 1);
            Vector3 target = new(5, 0, 5);
            string[,,] map = MapCore.InitializeMap(10, 1, 10);

            //Act
            Vector3 result = FieldOfView.GetMissedLocation(map, source, target, 0);

            //Assert
            Assert.AreEqual(new(13, 1, 15), result);
        }

        [TestMethod]
        public void SouthEastTargetTest()
        {
            //Arrange
            Vector3 source = new(1, 0, 9);
            Vector3 target = new(5, 0, 5);
            string[,,] map = MapCore.InitializeMap(10, 1, 10);

            //Act
            Vector3 result = FieldOfView.GetMissedLocation(map, source, target, 0);

            //Assert
            Assert.AreEqual(new(13, 1, -1), result);
        }

        [TestMethod]
        public void SouthWestTargetTest()
        {
            //Arrange
            Vector3 source = new(9, 0, 1);
            Vector3 target = new(5, 0, 5);
            string[,,] map = MapCore.InitializeMap(10, 1, 10);

            //Act
            Vector3 result = FieldOfView.GetMissedLocation(map, source, target, 0);

            //Assert
            Assert.AreEqual(new(-3, 1, 15), result);
        }

        [TestMethod]
        public void NorthWestTargetTest()
        {
            //Arrange
            Vector3 source = new(9, 0, 1);
            Vector3 target = new(5, 0, 5);
            string[,,] map = MapCore.InitializeMap(10, 1, 10);

            //Act
            Vector3 result = FieldOfView.GetMissedLocation(map, source, target, 0);

            //Assert
            Assert.AreEqual(new(-3, 1, 15), result);
        }

        [TestMethod]
        public void NorthEastSmallTargetTest()
        {
            //Arrange
            Vector3 source = new(0, 0, 0);
            Vector3 target = new(2, 0, 2);
            string[,,] map = MapCore.InitializeMap(10, 1, 10);

            //Act
            Vector3 result = FieldOfView.GetMissedLocation(map, source, target, 0);

            //Assert
            Assert.AreEqual(new(10, 1, 12), result);
        }

        [TestMethod]
        public void NorthEastSmallTargetWithWallTest()
        {
            //Arrange
            Vector3 source = new(0, 0, 0);
            Vector3 target = new(2, 0, 2);
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            for (int i = 0; i < 10; i++)
            {
                map[8, 0, i] = MapObjectType.FullCover;
            }

            //Act
            Vector3 impactLocation = FieldOfView.MissedShot(map, source, target, 0);

            //Assert
            Assert.AreEqual(new(7, 0, 9), impactLocation);
        }

        [TestMethod]
        public void NorthEastSmallTargetWithNoWallTest()
        {
            //Arrange
            Vector3 source = new(0, 0, 0);
            Vector3 target = new(2, 0, 2);
            string[,,] map = MapCore.InitializeMap(10, 1, 10);

            //Act
            Vector3 impactLocation = FieldOfView.MissedShot(map, source, target, 0);

            //Assert
            Assert.AreEqual(new(7, 0, 9), impactLocation);
        }

        [TestMethod]
        public void NorthEastSmallTargetBiggerMapWithNoWallTest()
        {
            //Arrange
            Vector3 source = new(0, 0, 0);
            Vector3 target = new(2, 0, 2);
            string[,,] map = MapCore.InitializeMap(20, 1, 20);

            //Act
            Vector3 impactLocation = FieldOfView.MissedShot(map, source, target, 0);

            //Assert
            Assert.AreEqual(new(17, 0, 19), impactLocation);
        }


    }
}
