﻿using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.SaveGames;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Numerics;
using System.Reflection;

namespace Battle.Tests.Scenarios
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L2")]
    public class AICoverCrashTest
    {
        private string _rootPath;

        [TestInitialize]
        public void GameSerializationStartUp()
        {
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        [TestMethod]
        public void AICoverStateCrashTest()
        {
            //Arrange
            string path = _rootPath + @"\SaveGames\Saves\Save005.json";

            //Act
            string fileContents;
            using (var streamReader = new StreamReader(path))
            {
                fileContents = streamReader.ReadToEnd();
            }
            Mission mission = GameSerialization.LoadGame(fileContents);
            mission.StartMission();

            mission.MoveToNextTurn();
            CharacterAI ai = new CharacterAI();
            AIAction aIAction = ai.CalculateAIAction(mission.Map, 
                mission.Teams, 
                mission.Teams[1].Characters[0], 
                mission.RandomNumbers);
            string mapString = ai.CreateAIMap(mission.Map);
            string mapStringExpected = @"
. . . . . . . . . . . □ . . ■ . . □ . . . . . . . . □ . . . . . ■ . . . . . . . . . ■ . . . ■ . . . 
. . . . . . . . . . . . . . . . . . . . ■ . . . ■ . . . . . . . . . . . . . . . . . . ■ . . . . □ . 
. . . . . . . . . . . . . . . . ■ . . . . . . . . . . . . ■ . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . . . ■ . . . . . . . . ■ . . . . . . . . . □ ■ . . . . . . . . . . . . . . . . . . . 
. . . ■ . . ■ . . . □ . . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
. . . . □ . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . □ . . . . . . . . ■ . . . . . . 
. . . . . □ . . . . . . . . . . . . . . . . . . . □ . . . . . . . . . . . . . . . □ . . . . . . □ . 
. . . . ■ . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . □ . . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . ■ . . □ . . . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . . . . □ . . . . □ . . □ . . . . . . . . . ■ . . . . . . . . . 
. . . . . . . . . . . □ . . . . □ . . . . . . . . . . . . . . . . . . . . □ . . . . . . ■ . . . . . 
. . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . ■ . . . . . . . . . . . 
. . . . . ■ . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . . . . 
. . . . . . . . . . . . . . . . . . . 1 ■ . . . ■ . . . . . . ■ . . . ■ . . . . . . . . . . . . . . 
. . . . . . ■ . . . . . . ■ . . . 1 1 1 1 1 . . . . . □ . . . . . . . . . . . . . . . . . . . . . . 
. □ . . . . . . . . . . . . . 1 1 1 1 1 3 1 1 1 . . . . . . ■ . . . . . . . . . . . ■ . . ■ . . . . 
. . . . . . . . . . . . 1 1 1 1 1 1 1 1 □ 3 1 1 4 ■ . □ . . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . ■ . 1 4 1 1 1 1 1 1 1 1 □ 3 1 1 □ 4 1 1 1 . □ . . . . . . . . ■ . □ . . . . . . . . 
. . . . □ . □ . . 1 1 ■ 6 1 1 1 1 1 3 1 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . ■ . . . . . . . . 
. . . . . . . . 1 1 3 3 3 1 1 1 1 1 □ 3 1 1 1 1 3 3 1 1 1 1 3 . . . . . . . . . . . . . . . . . □ . 
. . . . . . . 1 1 1 6 3 3 1 1 3 3 1 1 1 1 1 1 1 □ □ 3 1 4 1 □ 3 . . . . . . . . . . . . . . . ■ . . 
□ . . . . . . 1 1 1 ■ 6 1 1 1 3 1 3 1 0 1 1 1 1 1 1 1 1 ■ 4 1 □ . ■ . . . ■ . . . ■ . . . . . . ■ . 
. . . . . . 3 1 1 1 3 3 1 1 3 1 3 0 6 0 0 0 1 1 1 1 1 1 1 1 1 4 1 . . . . . . . □ . . □ . . . . . . 
. . . . . . 3 1 1 1 3 1 1 4 3 1 0 3 ■ 4 1 1 1 0 1 1 3 1 1 1 1 ■ 4 . ■ . . . □ . □ . . . . . . . . . 
. . . . . . . 3 1 4 3 1 4 ■ 4 0 3 0 3 1 3 3 6 0 0 1 □ 3 1 1 4 4 1 . . . . . . . . . . . . . . . . . 
. . . . . 3 1 □ 4 ■ 6 1 □ 4 6 0 6 0 1 1 3 6 ■ 6 1 0 1 3 1 1 ■ ■ 4 1 . . . □ . . . . . . . . . . ■ . 
. . . . . 3 1 1 1 ■ 4 1 1 1 ■ 6 ■ 4 1 3 3 ■ 4 1 3 0 1 □ 3 1 1 1 ■ 4 . . . . . □ . . . □ . . . . . . 
□ . . . □ 5 1 1 1 1 1 1 1 1 3 3 1 1 3 3 1 1 1 3 0 0 0 1 1 1 1 1 1 1 1 . . . . . . . . . ■ . . . . . 
. . . . 1 1 1 1 1 1 1 1 1 3 0 1 1 3 3 1 3 1 3 0 1 1 1 1 1 1 1 1 1 1 1 . . . . . . . . . . . . . . . 
■ . . 1 3 1 1 1 1 1 1 1 1 3 1 3 3 3 1 P □ 5 0 1 1 1 3 1 1 1 1 1 3 1 1 . . . . . . . . . . . . . . . 
. ■ . . 3 1 1 1 1 1 1 1 3 1 1 □ 0 1 1 1 3 0 1 1 3 3 3 1 1 1 1 1 □ 3 . . . . . . . . . . . . . . . . 
■ . . . 3 1 1 1 1 1 1 1 0 1 1 3 1 1 1 0 0 1 1 3 5 0 0 3 1 1 1 1 1 3 1 . . . . . . □ . . □ . . . . . 
. . . . . 1 1 1 1 1 1 3 3 1 3 1 1 1 0 1 1 1 3 3 □ 0 6 3 3 1 1 1 1 □ . . . . . . . . . . . □ . . . . 
. . . . . 1 1 1 1 1 3 1 □ 5 3 1 4 0 1 1 3 3 0 0 0 3 ■ 4 1 1 1 1 1 ■ . . . ■ . . . . □ . . . . . . . 
. . . . . . 1 1 1 1 3 1 3 3 1 1 ■ 4 1 3 3 0 0 3 3 1 1 □ 3 1 1 1 4 . . . . . . . . . . . □ ■ . . . . 
. . . . . □ 3 1 1 5 1 1 5 1 1 3 1 1 3 0 0 3 3 1 1 1 1 1 1 4 1 4 ■ . . . . . . . . . . . . . □ ■ . . 
. . . ■ . . 1 1 1 □ 3 3 □ 3 1 □ 3 3 0 0 3 1 1 1 1 1 1 1 4 ■ 4 □ 4 . . . . . . . . . . . . . . . . . 
. . . . . . . 4 1 1 3 1 1 1 1 3 3 3 3 1 1 1 1 1 1 1 1 1 □ 4 4 1 . . . . □ . . . . . □ . . . . . . . 
. . . . . . . ■ 4 3 1 1 1 1 3 3 3 1 1 1 1 1 1 1 1 1 1 1 1 1 ■ 4 □ . . . . . . . ■ . □ . . . . . . . 
. . . . □ □ . . ■ 4 1 1 1 3 3 3 1 1 1 1 1 1 1 1 1 1 1 3 3 3 3 . . . . . . . . ■ . . . . . . □ . . . 
. . . . . . . . . 3 1 6 3 3 1 1 1 1 1 1 1 1 1 1 6 5 3 3 3 3 3 . . . . . . . . . . . . □ . . . . . . 
. . . □ . □ . . . □ 6 ■ 4 1 1 1 1 1 1 1 1 3 3 3 ■ □ 5 3 3 . . . . . . . . . . . . . . . . . . . . . 
. . . . . . . . □ . . . □ 3 1 1 1 1 3 3 3 3 3 3 3 . □ . . . . . . . . . . . . . . . . . . . . . . . 
. . . □ . □ . ■ . . . . . . . 3 3 3 3 3 3 3 3 3 . . . . . . . . . . . . . . . . . . . . . . . . □ . 
. . . . . . . . . . . . . . . . . 3 3 3 3 3 . . . . . . . . . . . ■ . □ . . . ■ . . . . . . . . . . 
. . . . . . . . . ■ ■ . . . ■ . . . . 1 . . . . . . ■ . . . . . □ . . . . . . . . ■ . . . . . ■ . . 
. . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . . . . . . . . . . . . . . . . . . . . . 
. P . . . . . . . . . . . . . . □ . . . . . . . . . . . . . . . . . . . . . . . . . . . ■ . . . □ . 
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . □ . □ . . . . . . . . . . . . . . . . ■ . 
";

            //Assert
            Assert.AreEqual(ActionTypeEnum.MoveThenAttack, aIAction.ActionType);
            Assert.AreEqual(5, aIAction.Score);
            Assert.AreEqual(new Vector3(19,0,19), aIAction.StartLocation);
            Assert.AreEqual(new Vector3(13,0,15), aIAction.EndLocation);
            Assert.AreEqual(mapStringExpected, mapString);
        }
    }
}
