using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battle.Tests.Characters
{
    [TestClass]
    [TestCategory("L1")]
    public class CharacterAITests
    {

        [TestMethod]
        public void CharacterJethroAIMovesIntoFullCoverNoOpponentsTest()
        {
            //Arrange
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            mission.Map[20, 0, 20] = MapObjectType.FullCover;
            mission.Map[18, 0, 18] = MapObjectType.HalfCover;
            Team team1 = new(1)
            {
                Name = "Good guys",
                Characters = new() { },
                Color = "Blue"
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new(15, 0, 15));
            jethro.ActionPointsCurrent = 2;
            Team team2 = new(0)
            {
                Name = "Bad guys",
                Characters = new() { jethro },
                Color = "Red"
            };
            mission.Teams.Add(team2);

            //Act            
            //Scenario 1: Failure
            AIAction actionResult1 = mission.CalculateAIAction(jethro, team2, team1);
            //Scenario 2: Success
            AIAction actionResult2 = mission.CalculateAIAction(jethro, team2, team1);
            string mapString = actionResult2.MapString;

            //Assert
            string mapStringExpected = @"
· · · · · · · · · · · · · · · 0 · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · 
· · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · 
· · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · 
· · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 10 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 11 ■ 10 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 7 0 11 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · 
· 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 7 □ 7 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · 
0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 7 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · 
0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · 
0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 P 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · 
0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · 
0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · 
· 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · 
· 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · 
· · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · 
· · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · 
· · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · 
· · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
";
            Assert.AreEqual(mapStringExpected, mapString);
            string log1 = @"
Jethro is processing AI, with intelligence 25
Failed intelligence check: 25, (dice roll: 8)
";
            Assert.AreEqual(log1, actionResult1.LogString);
            Assert.AreEqual(10, actionResult1.Score);
            Assert.AreEqual(new(15, 0, 15), actionResult1.StartLocation);
            Assert.AreEqual(new(20, 0, 21), actionResult1.EndLocation);
            Assert.AreEqual(ActionTypeEnum.DoubleMove, actionResult1.ActionType);

            string log2 = @"
Jethro is processing AI, with intelligence 25
Successful intelligence check: 25, (dice roll: 81)
";
            Assert.AreEqual(log2, actionResult2.LogString);
            Assert.AreEqual(new(15, 0, 15), actionResult2.StartLocation);
            Assert.AreEqual(new(19, 0, 20), actionResult2.EndLocation);
            Assert.AreEqual(ActionTypeEnum.DoubleMove, actionResult2.ActionType);
        }


        [TestMethod]
        public void CharacterJethroAIMovesIntoFullCoverWithOpponentTest()
        {
            //Arrange
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            mission.Map[5, 0, 6] = MapObjectType.FullCover;
            mission.Map[14, 0, 5] = MapObjectType.HalfCover; //half cover here!
            mission.Map[14, 0, 6] = MapObjectType.HalfCover; //half cover here!
            mission.Map[14, 0, 7] = MapObjectType.HalfCover; //half cover here!
            mission.Map[14, 0, 8] = MapObjectType.FullCover;
            mission.Map[14, 0, 9] = MapObjectType.FullCover;
            mission.Map[14, 0, 10] = MapObjectType.FullCover;
            mission.Map[14, 0, 11] = MapObjectType.FullCover;
            mission.Map[14, 0, 12] = MapObjectType.FullCover;
            mission.Map[14, 0, 13] = MapObjectType.FullCover;
            mission.Map[14, 0, 14] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(mission.Map, new(5, 0, 5));
            Team team1 = new(1)
            {
                Name = "Good guys",
                Characters = new() { fred }
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new(15, 0, 10));
            jethro.HitpointsCurrent = 5;
            Team team2 = new(0)
            {
                Name = "Bad guys",
                Characters = new() { jethro }
            };
            mission.Teams.Add(team2);
            //Remove the 8, so that the 81 is primed and ready
            mission.RandomNumbers.Dequeue();

            //Act            
            AIAction actionResult = mission.CalculateAIAction(jethro, team2, team1);
            string mapString = actionResult.MapString;

            //Assert         
            string mapStringExpected = @"
· · · · · · · · · · · · · · · 0 · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · 
· · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · 
· · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· · · · 0 0 0 0 0 0 0 0 0 0 6 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· · · · 0 0 0 0 0 0 0 0 0 0 ■ 6 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · 
· · · · · 0 0 0 0 0 0 0 0 0 ■ 6 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · 
· · · · · 0 0 0 0 0 0 0 0 0 ■ 6 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · 
· · · · · · 0 0 0 0 0 0 0 · ■ 6 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · 
· · · · · · 0 0 0 0 0 0 0 · ■ P 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · 
· · · · · · · 0 0 0 0 0 0 · ■ 6 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · 
· · · · · · · 0 0 0 0 0 0 · ■ 4 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · 
· · · · · · 0 0 0 0 0 0 0 0 □ 2 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · 
· · · · · ■ 0 0 0 0 0 0 0 0 □ 7 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · 
· · · · · P 0 0 0 0 0 0 0 0 □ 7 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · 
· · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · · · · · · · · · · 
";
            Assert.AreEqual(mapStringExpected, mapString);
            string log1 = @"
Jethro is processing AI, with intelligence 25
Successful intelligence check: 25, (dice roll: 81)
";
            Assert.AreEqual(log1, actionResult.LogString);
            Assert.AreEqual(new(15, 0, 10), actionResult.StartLocation);
            Assert.AreEqual(new(15, 0, 6), actionResult.EndLocation);
            Assert.AreEqual(ActionTypeEnum.MoveThenAttack, actionResult.ActionType);
        }

    }
}
