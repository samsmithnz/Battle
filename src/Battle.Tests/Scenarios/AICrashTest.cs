using Battle.Logic.Characters;
using Battle.Logic.Encounters;
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
    public class AICrashTest
    {
        private string _rootPath;

        [TestInitialize]
        public void GameSerializationStartUp()
        {
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/SaveGames/Saves/";
        }

        [TestMethod]
        public void AICrashesOnFirstTurnTest()
        {
            //Arrange
            string path = _rootPath + "Save019.json";

            //Act
            Mission mission = GameSerialization.LoadGameFile(path);
            mission.StartMission();
            //Character fred = mission.Teams[0].GetCharacter("Fred");
            Character enemy1 = mission.Teams[1].Characters[0];
            Character enemy2 = mission.Teams[1].Characters[1];
            Team team1 = mission.Teams[0];
            Team team2 = mission.Teams[1];
            Assert.IsTrue(enemy1.HitpointsCurrent <= 0);

            //Move to enemy turn
            mission.MoveToNextTurn();

            AIAction aIAction = mission.CalculateAIAction(enemy2, team2, team1);
            string mapString = aIAction.MapString;
            string mapStringExpected = @"
· · · · · · · · · · · □ · · ■ · · □ · · · · · · · · □ · · · · · ■ · · · · · · · · · ■ · · · ■ · · · 
· · · · · · · · · · · · · · · · · · · · ■ · · · ■ 10 0 0 0 · · · · · · · · · · · · · · ■ · · · · □ · 
· · · · · · · · · · · · · · · · ■ · · · · · 0 0 0 0 0 0 0 ■ · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · ■ · · · · · · · · ■ 10 6 0 0 0 0 0 0 0 □ ■ 10 0 0 · · · · · · · · · · · · · · · · 
· · · ■ · · ■ · · · □ · · · · · · 0 0 0 0 □ 6 0 0 0 0 0 0 0 0 0 0 0 6 0 · · · · · · · · · · · · · · 
· · · · □ · · · · □ · · · · · 0 0 0 0 0 0 0 0 0 0 6 0 0 0 0 0 0 0 0 □ 6 0 · · · · · · ■ · · · · · · 
· · · · · □ · · · · · · · · · 0 0 0 0 0 0 0 0 0 0 □ 6 0 0 0 0 0 0 0 0 0 0 0 · · · □ · · · · · · □ · 
· · · · ■ · · · · · · · · · □ 6 0 0 0 0 0 0 0 0 0 0 0 0 0 0 10 0 0 0 0 10 0 0 6 · ■ · · · · · □ · · · 
· · · · · · · · · · · · · · 0 0 0 0 0 0 0 0 6 0 0 0 0 6 0 0 ■ 10 0 0 0 ■ 10 0 □ · · · · · · · · · · · 
· · · · · · · · · · · · · 0 0 0 6 0 0 0 0 0 □ 6 0 0 0 □ 2 0 □ 6 0 0 0 0 0 6 0 0 ■ · · · · · · · · · 
· · · · · · · · · · · □ · 0 0 0 □ 6 0 0 0 0 0 0 0 0 0 0 0 0 0 10 0 0 0 0 0 □ 10 0 · · · · ■ · · · · · 
· · · · · · · □ · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 ■ 10 0 0 0 0 0 ■ 10 · · · · · · · · · · 
· · · · · ■ · · · · · · 0 0 0 0 0 0 0 0 6 0 0 0 6 0 0 0 0 0 0 6 0 0 0 10 0 0 0 0 ■ · · · · · · · · · 
· · · · · · · · · · · · 0 10 0 0 0 0 0 0 ■ 6 0 0 ■ 6 0 2 0 0 0 ■ 6 0 0 ■ 10 0 0 0 0 · · · · · · · · · 
· · · · · · ■ · · · · 0 0 ■ 10 0 0 0 0 0 0 0 0 0 0 0 0 □ 2 0 6 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · 
· □ · · · · · · · · · 0 0 0 0 0 0 0 0 0 3 0 0 0 0 6 0 2 0 0 ■ 6 0 0 0 0 0 0 0 0 0 · ■ · · ■ · · · · 
· · · · · · · · · · · · 0 0 0 0 0 0 0 0 □ 3 0 0 6 ■ P □ 2 0 6 0 0 0 0 0 0 0 0 10 0 6 · · · · · · · · 
· · · · · · · · ■ · · · 0 0 0 0 0 0 0 0 □ 3 0 0 □ 6 0 0 0 0 □ 2 0 0 0 0 0 0 0 ■ 10 □ · · · · · · · · 
· · · · □ · □ · · · · ■ 15 0 0 0 0 0 4 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 ■ · · · · · · · · 
· · · · · · · · · · · · 2 0 0 0 0 0 □ 4 0 0 0 0 1 1 0 0 0 0 2 0 0 0 0 0 0 0 0 0 0 · · · · · · · □ · 
· · · · · · · · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 □ □ 3 0 9 0 □ 2 0 10 0 0 0 10 0 0 0 · · · · · · ■ · · 
□ · · · · · · · · · ■ · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 ■ 6 0 □ 10 ■ 10 0 0 ■ 10 0 · ■ · · · · · · ■ · 
· · · · · · · · · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 10 0 0 10 0 0 0 6 0 □ · · □ · · · · · · 
· · · · · · · · · · · · · 11 0 0 0 0 . 0 0 0 0 0 0 0 3 0 0 0 0 ■ 10 0 ■ 10 0 0 □ 6 □ · · · · · · · · · 
· · · · · · · · · · · · · ■ 11 0 0 0 0 0 0 0 11 0 0 0 □ 2 0 0 10 10 0 0 0 0 0 6 0 · · · · · · · · · · · 
· · · · · · · □ · ■ · · □ · 11 2 13 0 0 0 0 11 ■ 11 0 0 0 2 0 0 ■ ■ 10 0 0 0 0 □ 6 · · · · · · · · · ■ · 
· · · · · · · · · ■ · · · · ■ 13 ■ 9 0 0 0 ■ 9 0 0 0 0 □ 4 0 0 0 ■ 10 0 0 0 0 · □ · · · □ · · · · · · 
□ · · · □ · · · · · · · · · · · 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · ■ · · · · · 
· · · · · · · · · · · · · · · · · 0 0 0 5 0 0 0 0 0 0 0 0 0 0 0 0 0 0 0 · · · · · · · · · · · · · · 
■ · · · · · · · · · · · · · · · · · · 0 □ 7 0 0 0 0 0 0 0 0 0 0 · 0 · · · · · · · · · · · · · · · · 
· ■ · · · · · · · · · · · · · □ · · · · · · 0 0 0 0 2 0 0 0 0 · □ · · · · · · · · · · · · · · · · · 
■ · · · · · · · · · · · · · · · · · · · · · · · 11 2 2 · 0 · · · · · · · · · · · · □ · · □ · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · □ · · · · · · · · □ · · · · · · · · · · · □ · · · · 
· · · · · · · · · · · · □ · · · · · · · · · · · · · ■ · · · · · · ■ · · · ■ · · · · □ · · · · · · · 
· · · · · · · · · · · · · · · · ■ · · · · · · · · · · □ · · · · · · · · · · · · · · · · □ ■ · · · · 
· · · · · □ · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · · · · · · · · · □ ■ · · 
· · · ■ · · · · · □ · · □ · · □ · · · · · · · · · · · · · ■ · □ · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · · · · · · · □ · · · · · □ · · · · · · · 
· · · · · · · ■ · · · · · · · · · · · · · · · · · · · · · · ■ · □ · · · · · · · ■ · □ · · · · · · · 
· · · P □ □ · · ■ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · · □ · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · · · · · · 
· · · □ · □ · · · □ · ■ · · · · · · · · · · · · ■ □ · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · □ · · · □ · · · · · · · · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · · 
· · · □ · □ · ■ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · 
· · · · · · · P · · · · · · · · · · · · · · · · · · · · · · · · · ■ · □ · · · ■ · · · · · · · · · · 
· · · · · · · · · ■ ■ · · · ■ · · · · · · · · · · · ■ · · · · · □ · · · · · · · · ■ · · · · · ■ · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · · · · · · · · · · · · · · · · · · · 
· P · · · · · · · · · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · □ · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · □ · · · · · · · · · · · · · · · · ■ · 
";

            //Assert
            Assert.AreEqual(ActionTypeEnum.DoubleMove, aIAction.ActionType);
            Assert.AreEqual(13, aIAction.Score);
            Assert.AreEqual(new(26, 0, 32), aIAction.StartLocation);
            Assert.AreEqual(new(16, 0, 23), aIAction.EndLocation);
            Assert.AreEqual(mapStringExpected, mapString);

            //            //Act
            //            mission.MoveCharacter(mission.Teams[1].Characters[0],
            //                mission.Teams[1],
            //                mission.Teams[0],
            //                aIAction.EndLocation);
            //            EncounterResult encounterResult = mission.AttackCharacter(mission.Teams[1].Characters[0],
            //                mission.Teams[1].Characters[0].WeaponEquipped,
            //                mission.Teams[1].GetCharacter(mission.Teams[1].Characters[0].TargetCharacters[0]),
            //                mission.Teams[1],
            //                mission.Teams[0]);

            //            //Assert
            //            string log = @"
            //Jethro is attacking with Shotgun, targeted on Fred
            //Hit: Chance to hit: 75, (dice roll: 81)
            //Damage range: 3-5, (dice roll: 76)
            //Critical chance: 70, (dice roll: 55)
            //Critical damage range: 9-13, (dice roll: 76)
            //Armor prevented 1 damage to character Fred
            //11 damage dealt to character Fred, HP is now -7
            //Fred is killed
            //100 XP added to character Jethro, for a total of 100 XP
            //Jethro is ready to level up
            //";
            //            Assert.AreEqual(log, encounterResult.LogString);

            //            List<Character> charactersInView = FieldOfView.GetCharactersInView(mission.Map,
            //                   mission.Teams[1].Characters[1].Location,
            //                   mission.Teams[1].Characters[1].ShootingRange,
            //                   mission.Teams[0].Characters);
            //            Assert.AreEqual(0, charactersInView.Count);
            //            List<Character> charactersInView2 = FieldOfView.GetCharactersInView(mission.Map,
            //                   mission.Teams[1].Characters[0].Location,
            //                   mission.Teams[1].Characters[1].ShootingRange,
            //                   mission.Teams[0].Characters);
            //            Assert.AreEqual(2, charactersInView2.Count);
            //            CharacterAI ai2 = new CharacterAI();
            //            AIAction aIAction2 = ai2.CalculateAIAction(mission.Map,
            //                mission.Teams[1].Characters[1],
            //                mission.Teams,
            //                mission.RandomNumbers);
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
            //· · · · · · · · ■ · · · 3 3 3 3 3 0 0 1 □ 3 2 2 □ 0 1 1 0 0 □ 0 0 1 1 1 1 1 1 ■ 4 □ · · · · · · · · 
            //· · · · □ · □ · · · · ■ 3 3 3 3 1 1 3 1 3 4 2 3 0 0 2 3 1 0 0 0 0 0 1 1 1 1 1 1 1 ■ · · · · · · · · 
            //· · · · · · · · · · · · 3 3 3 1 1 1 □ 3 5 5 3 1 1 5 3 0 2 0 3 0 0 1 1 1 1 1 1 1 1 · · · · · · · □ · 
            //· · · · · · · · · · · · 3 3 1 1 1 1 1 1 5 5 3 1 □ □ 3 2 2 0 □ 3 1 4 1 1 1 4 1 1 1 · · · · · · ■ · · 
            //□ · · · · · · · · · ■ · · 1 1 1 1 1 1 1 3 1 3 0 1 5 4 2 ■ 2 0 □ 4 ■ 4 1 1 ■ 4 1 · ■ · · · · · · ■ · 
            //· · · · · · · · · · · · · 1 1 3 1 1 4 3 1 3 5 1 1 5 5 3 2 0 0 2 1 1 4 1 1 1 3 1 □ · · □ · · · · · · 
            //· · · · · · · · · · · · · 4 3 1 1 1 ■ 4 3 1 1 1 5 5 5 2 2 0 0 ■ 4 1 ■ 4 1 1 □ 3 □ · · · · · · · · · 
            //· · · · · · · · · · · · · ■ 1 1 3 3 1 3 1 1 4 3 3 3 □ 1 0 0 2 4 1 0 1 0 1 3 1 · · · · · · · · · · · 
            //· · · · · · · □ · ■ · · □ · 4 3 4 1 1 1 1 4 ■ 6 3 1 0 1 0 0 ■ ■ 2 1 0 0 0 □ 3 · · · · · · · · · ■ · 
            //· · · · · · · · · ■ · · · · ■ 1 ■ 4 1 1 1 ■ 6 3 3 0 0 □ 1 0 0 0 ■ 2 0 0 0 0 · □ · · · □ · · · · · · 
            //□ · · · □ · · · · · · · · · · · 1 1 1 1 3 3 3 3 0 0 0 0 0 0 0 1 0 0 0 0 0 · · · · · · · ■ · · · · · 
            //· · · · · · · · · · · · · · · · · 1 3 3 5 3 3 3 0 0 0 0 0 0 0 0 1 1 1 0 · · · · · · · · · · · · · · 
            //■ · · · · · · · · · · · · · · · · · · 3 □ 5 3 0 0 0 0 0 0 0 0 1 · 0 · · · · · · · · · · · · · · · · 
            //· ■ · · · · · · · · · · · · · □ · · · · · · 1 0 0 0 0 0 0 0 1 · □ · · · · · · · · · · · · · · · · · 
            //■ · · · · · · · · · · · · · · · · · · · · · · · 1 0 0 · 0 · · · · · · · · · · · · □ · · □ · · · · · 
            //· · · · · · · · · · · · · · · · · · · · · · · · □ · · · · · · · · □ · · · · · · · · · · · □ · · · · 
            //· · · · · · · · · · · · □ · · · P · · · · · · · · · ■ · · · · · · ■ · · · ■ · · · · □ · · · · · · · 
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
            //· · · · · · · · · P · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · □ · 
            //· · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · □ · · · · · · · · · · · · · · · · ■ · 
            //";

            //            //Assert
            //            Assert.AreEqual(ActionTypeEnum.DoubleMove, aIAction2.ActionType);
            //            //Assert.AreEqual(5, aIAction2.Score);
            //            Assert.AreEqual(new(26, 0, 32), aIAction2.StartLocation);
            //            Assert.AreEqual(new(20, 0, 20), aIAction2.EndLocation);
            //            //Assert.AreEqual(mapStringExpected2, mapString2);

            //            mission.MoveCharacter(mission.Teams[1].Characters[1],
            //                mission.Teams[1],
            //                mission.Teams[0],
            //                aIAction2.EndLocation);
        }
    }
}
