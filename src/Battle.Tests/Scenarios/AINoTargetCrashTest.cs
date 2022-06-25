using Battle.Logic.Characters;
using Battle.Logic.Game;
using Battle.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection;

namespace Battle.Tests.Scenarios
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L2")]
    public class AINoTargetCrashTest
    {
        private string _rootPath;

        [TestInitialize]
        public void GameSerializationStartUp()
        {
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/SaveGames/Saves/";
        }

        [TestMethod]
        public void PlayerAimingCrashTest()
        {
            //Arrange
            string path = _rootPath + @"Save017.json";

            //Act
            Mission mission = GameSerialization.LoadGameFile(path);
            mission.StartMission();
            Character fred = mission.Teams[0].GetCharacter("Fred");
            Character enemy1 = mission.Teams[1].Characters[0];
            //Character enemy2 = mission.Teams[1].Characters[1];
            //Team team1 = mission.Teams[0];
            //Team team2 = mission.Teams[1];

            //process targeting for fred
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            Assert.AreEqual(0, fred.TargetCharacterIndex);
            Assert.AreEqual(-17, enemy1.HitpointsCurrent);


            //            //Assert
            //            Assert.IsTrue(aIAction.IntelligenceCheckSuccessful == false);
            //            Assert.AreEqual(ActionTypeEnum.MoveThenAttack, aIAction.ActionType);
            //            Assert.AreEqual(6, aIAction.Score);
            //            Assert.AreEqual(new(19, 0, 19), aIAction.StartLocation);
            //            Assert.AreEqual(new(16, 0, 23), aIAction.EndLocation);
            //            Assert.AreEqual(mapStringExpected, mapString);

            //            //Now run the action
            //            mission.MoveCharacter(enemy1,
            //                team2,
            //                team1,
            //                aIAction.EndLocation);
            //            EncounterResult encounterResult = mission.AttackCharacter(enemy1,
            //                  enemy1.WeaponEquipped,
            //                  team1.GetCharacter(aIAction.TargetName),
            //                  team2,
            //                  team1);
            //            Assert.AreEqual(true, encounterResult.IsHit);

            //            //process AI for character 2
            //            CharacterAI ai2 = new CharacterAI();
            //            mission.RandomNumbers.Dequeue(); //Remove the 20 roll
            //            AIAction aIAction2 = ai2.CalculateAIAction(mission.Map,
            //                 enemy2,
            //                 mission.Teams,
            //                 mission.RandomNumbers);
            //            string mapString2 = aIAction2.MapString;
            //            string mapStringExpected2 = @"
            //· · · · · · · · · · · □ · · ■ · · □ · · · · · · · · □ · · · · · ■ · · · · · · · · · ■ · · · ■ · · · 
            //· · · · · · · · · · · · · · · · · · · · ■ · · · ■ 4 1 1 1 · · · · · · · · · · · · · · ■ · · · · □ · 
            //· · · · · · · · · · · · · · · · ■ · · · · · 1 1 1 1 1 1 1 ■ · · · · · · · · · · · · · · · · · · · · 
            //· · · · · · · · · · ■ · · · · · · · · ■ 4 3 1 1 1 1 1 1 1 □ ■ 4 1 1 · · · · · · · · · · · · · · · · 
            //· · · ■ · · ■ · · · □ · · · · · · 1 1 1 1 □ 3 1 1 1 1 1 1 1 1 1 1 1 3 1 · · · · · · · · · · · · · · 
            //· · · · □ · · · · □ · · · · · 1 1 1 1 1 1 1 1 1 1 3 1 1 1 1 1 1 1 1 □ 3 1 · · · · · · ■ · · · · · · 
            //· · · · · □ · · · · · · · · · 1 1 1 1 1 1 1 1 1 1 □ 3 1 1 1 1 1 1 1 1 1 1 1 · · · □ · · · · · · □ · 
            //· · · · ■ · · · · · · · · · □ 1 1 1 1 1 1 1 1 1 1 1 1 1 1 1 4 1 1 1 1 4 1 1 3 · ■ · · · · · □ · · · 
            //· · · · · · · · · · · · · · 0 0 1 1 1 1 1 1 3 1 1 1 0 3 1 1 ■ 4 1 1 1 ■ 4 1 □ · · · · · · · · · · · 
            //· · · · · · · · · · · · · 0 0 0 3 1 1 1 1 1 □ 3 0 0 0 □ 0 1 □ 3 1 1 1 1 1 3 1 1 ■ · · · · · · · · · 
            //· · · · · · · · · · · □ · 0 0 1 □ 3 1 1 1 1 0 0 0 0 0 0 0 0 0 4 1 1 1 1 1 □ 4 1 · · · · ■ · · · · · 
            //· · · · · · · □ · · · · · 0 0 1 1 1 1 1 1 0 0 0 0 0 0 0 0 0 0 ■ 4 1 1 1 1 1 ■ 4 · · · · · · · · · · 
            //· · · · · ■ · · · · · · 0 0 1 1 1 1 1 1 4 0 1 0 0 0 0 0 0 0 0 0 0 1 1 4 1 1 1 1 ■ · · · · · · · · · 
            //· · · · · · · · · · · · 0 2 1 1 1 1 1 1 ■ 2 0 1 ■ 0 0 0 0 0 0 ■ 0 1 1 ■ 4 1 1 1 1 · · · · · · · · · 
            //· · · · · · ■ · · · · 0 0 ■ 1 1 1 1 1 0 0 0 1 0 1 0 0 □ 0 0 0 0 0 0 1 1 1 1 1 1 1 1 · · · · · · · · 
            //· □ · · · · · · · · · 0 0 1 1 1 1 1 0 0 1 1 0 0 0 4 0 0 0 0 ■ 0 0 1 1 1 1 1 1 1 1 · ■ · · ■ · · · · 
            //· · · · · · · · · · · · 1 1 1 1 1 0 0 0 □ 1 2 0 2 ■ P □ 0 0 0 0 0 0 1 1 1 1 1 4 1 3 · · · · · · · · 
            //· · · · · · · · ■ · · · 1 1 1 1 1 0 0 1 □ 3 2 2 □ 0 1 1 0 0 □ 0 0 1 1 1 1 1 1 ■ 4 □ · · · · · · · · 
            //· · · · □ · □ · · · · ■ 3 1 1 1 0 0 1 0 2 2 2 3 0 0 2 3 1 0 0 0 0 0 1 1 1 1 1 1 1 ■ · · · · · · · · 
            //· · · · · · · · · · · · 3 1 1 0 0 0 □ 1 2 2 1 1 1 5 3 0 2 0 3 0 0 1 1 1 1 1 1 1 1 · · · · · · · □ · 
            //· · · · · · · · · · · · 3 1 0 1 1 0 0 0 2 1 1 0 □ □ 3 2 2 0 □ 3 1 4 1 1 1 4 1 1 1 · · · · · · ■ · · 
            //□ · · · · · · · · · ■ · · 0 0 1 0 1 0 0 1 0 2 0 0 2 2 2 ■ 2 0 □ 4 ■ 4 1 1 ■ 4 1 · ■ · · · · · · ■ · 
            //· · · · · · · · · · · · · 0 1 1 1 1 4 3 0 1 3 0 0 2 2 0 0 0 0 2 1 1 4 1 1 1 3 1 □ · · □ · · · · · · 
            //· · · · · · · · · · · · · 2 3 0 1 1 ■ 2 1 0 0 0 2 1 1 0 0 0 0 ■ 4 1 ■ 4 1 1 □ 3 □ · · · · · · · · · 
            //· · · · · · · · · · · · · ■ 0 1 3 3 1 1 1 1 4 1 1 1 □ 1 0 0 2 4 1 0 1 0 1 3 1 · · · · · · · · · · · 
            //· · · · · · · □ · ■ · · □ · 4 3 P 1 0 0 1 4 ■ 6 1 0 0 1 0 0 ■ ■ 2 1 0 0 0 □ 3 · · · · · · · · · ■ · 
            //· · · · · · · · · ■ · · · · ■ · ■ 2 0 1 1 ■ 4 1 3 0 0 □ 1 0 0 0 ■ 2 0 0 0 0 · □ · · · □ · · · · · · 
            //□ · · · □ · · · · · · · · · · · 0 0 1 1 1 1 1 3 1 0 0 0 0 0 0 1 0 0 0 0 0 · · · · · · · ■ · · · · · 
            //· · · · · · · · · · · · · · · · · 1 3 1 3 1 3 3 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · 
            //■ · · · · · · · · · · · · · · · · · · 1 □ 5 3 0 0 0 1 0 0 0 0 0 · 0 · · · · · · · · · · · · · · · · 
            //· ■ · · · · · · · · · · · · · □ · · · · · · 1 0 1 1 1 0 0 0 0 · □ · · · · · · · · · · · · · · · · · 
            //■ · · · · · · · · · · · · · · · · · · · · · · · 3 1 1 · 0 · · · · · · · · · · · · □ · · □ · · · · · 
            //· · · · · · · · · · · · · · · · · · · · · · · · □ · · · · · · · · □ · · · · · · · · · · · □ · · · · 
            //· · · · · · · · · · · · □ · · · · · · · · · · · · · ■ · · · · · · ■ · · · ■ · · · · □ · · · · · · · 
            //· · · · · · · · · · · · · · · · ■ · · · · · · · · · · □ · · · · · · · · · · · · · · · · □ ■ · · · · 
            //· · · · · □ · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · · · · · · · · · □ ■ · · 
            //· · P ■ · · · · · □ · · □ · · □ · · · · · · · · · · · · · ■ · □ · · · · · · · · · · · · · · · · · · 
            //· · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · · · · · · · □ · · · · · □ · · · · · · · 
            //· · · · · · · ■ · · · · · · · · · · · · · · · · · · · · · · ■ · □ · · · · · · · ■ · □ · · · · · · · 
            //· · · · □ □ · · ■ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · · □ · · · 
            //· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · · · · · · 
            //· · · □ · □ · · · □ · ■ · · · · · · · · · · · · ■ □ · · · · · · · · · · · · · · · · · · · · · · · · 
            //· · · · · · · · □ · · · □ · · · · · · · · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · · 
            //· · · □ · □ · ■ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · 
            //· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · □ · · · ■ · · · · · · · · · · 
            //· · · · · · · · · ■ ■ · · · ■ · · · · · · · · · · · ■ · · · · · □ · · · · · · · · ■ · · · · · ■ · · 
            //· · · · · · · · · · · · · · P · · · · · · · · · · · ■ · · · · · · · · · · · · · · · · · · · · · · · 
            //· P · · · · · · · · · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · □ · 
            //· · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · □ · · · · · · · · · · · · · · · · ■ · 
            //";

            //            //Assert
            //            Assert.IsTrue(aIAction2.IntelligenceCheckSuccessful == true);
            //            Assert.AreEqual(ActionTypeEnum.DoubleMove, aIAction2.ActionType);
            //            Assert.AreEqual(6, aIAction2.Score);
            //            Assert.AreEqual(new(26, 0, 32), aIAction2.StartLocation);
            //            Assert.AreEqual(new(23, 0, 23), aIAction2.EndLocation);
            //            Assert.AreEqual(mapStringExpected2, mapString2);
        }
    }
}
