using Battle.Logic.Characters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Map
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class CharacterFieldOfViewTests
    {
        [TestMethod]
        public void FredCanSeeJethroTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, team2.Characters);
            //List<Character> characters = fred.GetCharactersInRangeWithCurrentWeapon(map, team2.Characters);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            Assert.AreEqual("Jethro", fred.TargetCharacters[0]);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o P o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
P o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void FredCanSeeJethroInAngleTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = MapObjectType.FullCover;
            map[8, 0, 7] = MapObjectType.FullCover;
            map[9, 0, 7] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, team2.Characters);
            string fovMapString = MapCore.GetMapStringWithMapMask(map, fred.FOVMap);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o · 
o o o o o o o o P · 
o o o o o o o ■ ■ ■ 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
P o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);

            string expectedFOV = @"
· · · · · · · · · ▓ 
· · · · · · · · ▓ ▓ 
· · · · · · · ■ ■ ■ 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
P · · · · · · · · · 
";
            Assert.AreEqual(expectedFOV, fovMapString);
        }

        [TestMethod]
        public void FredCannotSeeJethroTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = MapObjectType.FullCover;
            map[8, 0, 7] = MapObjectType.FullCover;
            map[9, 0, 7] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new(8, 0, 0));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, team2.Characters);
            string fovMapString = MapCore.GetMapStringWithMapMask(map, fred.FOVMap);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(0, fred.TargetCharacters.Count);
            string expected = @"
o o o o o o o · · · 
o o o o o o o · P · 
o o o o o o o ■ ■ ■ 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o P o 
";
            Assert.AreEqual(expected, mapString);

            string expectedFOV = @"
· · · · · · · ▓ ▓ ▓ 
· · · · · · · ▓ ▓ ▓ 
· · · · · · · ■ ■ ■ 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · P · 
";
            Assert.AreEqual(expectedFOV, fovMapString);
        }

        [TestMethod]
        public void FredCanSeeJethroInNorthCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[8, 0, 7] = MapObjectType.FullCover;
            map[9, 0, 7] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new(8, 0, 0));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, team2.Characters);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o · · 
o o o o o o o o P · 
o o o o o o o o ■ ■ 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o P o 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }



        [TestMethod]
        public void FredCanSeeJethroInSouthCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[8, 0, 2] = MapObjectType.FullCover;
            map[9, 0, 2] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new(8, 0, 8));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 1));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, team2.Characters);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o P o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o ■ ■ 
o o o o o o o o P · 
o o o o o o o o · · 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }



        [TestMethod]
        public void FredCanSeeJethroInEastCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[8, 0, 7] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new(1, 0, 7));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(9, 0, 7));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, team2.Characters);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o o o 
o P o o o o o o ■ P 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }


        [TestMethod]
        public void FredCanSeeJethroInWestCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[1, 0, 7] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new(9, 0, 7));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(0, 0, 7));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, team2.Characters);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o o o 
P ■ o o o o o o o P 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void JethroCannotSeeFredTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = MapObjectType.FullCover;
            map[8, 0, 7] = MapObjectType.FullCover;
            map[9, 0, 7] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            string mapString = jethro.GetCharactersInViewMapString(map, team1.Characters);

            //Assert
            Assert.IsTrue(jethro.TargetCharacters != null);
            Assert.AreEqual(0, jethro.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o P o 
o o o o o o o ■ ■ ■ 
o o o o o · · · · · 
o o · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
P · · · · · · · · · 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void JethroCanSeeFredTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[7, 0, 7] = MapObjectType.HalfCover;
            map[8, 0, 7] = MapObjectType.FullCover;
            map[9, 0, 7] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            string mapString = jethro.GetCharactersInViewMapString(map, team1.Characters);

            //Assert
            Assert.IsTrue(jethro.TargetCharacters != null);
            Assert.AreEqual(1, jethro.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o P o 
o o o o o o o □ ■ ■ 
o o o o o o o o · · 
o o o o o o o · · · 
o o o o o o o · · · 
o o o o o o · · · · 
o o o o o o · · · · 
o o o o o · · · · · 
P o o o o · · · · · 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void FredShouldSeeJethroInCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 5] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 2));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(2, 0, 6));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, team2.Characters);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o · · · o o o o o o 
o · · · o o o o o o 
o o · o o o o o o o 
o o P o o o o o o o 
o o ■ o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o P o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void FredSimpleRange3Test()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(11, 1, 11);
            Character fred = CharacterPool.CreateFredHero(map, new(5, 0, 5));
            fred.ShootingRange = 3;
            Team team1 = new(1);
            team1.Characters.Add(fred);
            team1.UpdateTargets(map, null);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, new());

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(0, fred.TargetCharacters.Count);
            string mapStringExpected = @"
· · · · · · · · · · · 
· · · · · · · · · · · 
· · · o o o o o · · · 
· · o o o o o o o · · 
· · o o o o o o o · · 
· · o o o P o o o · · 
· · o o o o o o o · · 
· · o o o o o o o · · 
· · · o o o o o · · · 
· · · · · · · · · · · 
· · · · · · · · · · · 
";
            Assert.AreEqual(mapStringExpected, mapString);
        }

        [TestMethod]
        public void FredCanFlankJethroInNorthCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 2] = MapObjectType.FullCover;
            map[4, 0, 2] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new(5, 0, 2));
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(2, 0, 1));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            string mapString = fred.GetCharactersInViewMapString(map, team2.Characters);

            //Assert
            Assert.IsTrue(fred.TargetCharacters != null);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected = @"
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
o o o o o o o o o o 
· o o o o o o o o o 
· · · o o o o o o o 
· · ■ · ■ P o o o o 
· · P o o o o o o o 
· o o o o o o o o o 
";
            Assert.AreEqual(mapStringExpected, mapString);

            //Now test cover for each character

            //Act
            CoverState coverStateResultPlayer = CharacterCover.CalculateCover(map, fred.Location, new List<Vector3>() { jethro.Location });
            CoverState coverStateResultEnemy = CharacterCover.CalculateCover(map, jethro.Location, new List<Vector3>() { fred.Location });

            //Assert
            Assert.IsTrue(coverStateResultPlayer.InFullCover);
            Assert.IsTrue(coverStateResultPlayer.InWestFullCover);
            Assert.IsTrue(!coverStateResultEnemy.InFullCover);
            Assert.IsTrue(!coverStateResultEnemy.InSouthFullCover);
        }
    }
}