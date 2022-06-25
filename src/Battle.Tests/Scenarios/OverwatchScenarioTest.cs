using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Battle.Logic.SaveGames;
using Battle.Logic.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;

namespace Battle.Tests.Scenarios
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L2")]
    public class OverwatchScenarioTest
    {
        private string _rootPath;

        [TestInitialize]
        public void GameSerializationStartUp()
        {
            _rootPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/SaveGames/Saves/";
        }

        [TestMethod]
        public void FredInOverwatchShootsAtJethroTest()
        {
            //Arrange
            int xMax = 50;
            int zMax = 50;
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            List<int> MapRandomNumbers = RandomNumber.GenerateRandomNumberList(0, xMax - 1, 0, xMax * zMax * 5);
            Queue<int> MapNumberQueue = new(MapRandomNumbers);
            //Add 100 full cover items randomly
            for (int i = 0; i < 100; i++)
            {
                int x = MapNumberQueue.Dequeue();
                int z = MapNumberQueue.Dequeue();
                mission.Map[x, 0, z] = MapObjectType.FullCover;
            }
            //Add 100 half cover items randomly
            for (int i = 0; i < 100; i++)
            {
                int x = MapNumberQueue.Dequeue();
                int z = MapNumberQueue.Dequeue();
                mission.Map[x, 0, z] = MapObjectType.HalfCover;
            }
            Character fred = CharacterPool.CreateFredHero(mission.Map, new(1, 0, 1));
            fred.MobilityRange = 8;
            Team team1 = new(1)
            {
                Name = "Good guys",
                Characters = new() { fred }
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new(19, 0, 19));
            jethro.ActionPointsCurrent = 2;
            Team team2 = new(0)
            {
                Name = "Bad guys",
                Characters = new() { jethro }
            };
            mission.Teams.Add(team2);
            mission.StartMission();

            //Assert - Setup
            Assert.AreEqual(1, mission.Objectives.Count);
            Assert.AreEqual(MissionObjectiveType.EliminateAllOpponents, mission.Objectives[0].Type);
            Assert.AreEqual(1, mission.TurnNumber);
            Assert.AreEqual(2, mission.Teams.Count);
            Assert.AreEqual(50 * 50, mission.Map.Length);
            Assert.AreEqual("Good guys", mission.Teams[0].Name);
            Assert.AreEqual(1, mission.Teams[0].Characters.Count);
            Assert.AreEqual("Bad guys", mission.Teams[1].Name);
            Assert.AreEqual(1, mission.Teams[1].Characters.Count);
            Assert.AreEqual(8, mission.RandomNumbers.Queue[0]);

            //Act

            //Turn 1 - Team 1 starts
            //Fred runs to cover
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = mission.GetMovementPossibleTiles(fred);
            string mapMovementString = MapCore.GetMapStringWithItems(mission.Map, MovementPossibileTiles.ExtractVectorListFromKeyValuePair(movementPossibileTiles));
            string mapMovementResult = @"
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · □ · · ■ · · □ · · · · · · · · □ · · · · · ■ · · · · · · · · · ■ · · · ■ · · · 
· · · · · · · · · · · · · · · · · · · · ■ · · · ■ · · · · · · · · · · · · · · · · · · ■ · · · · □ · 
· · · · · · · · · · · · · · · · ■ · · · · · · · · · · · · ■ · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · ■ · · · · · · · · ■ · · · · · · · · · □ ■ · · · · · · · · · · · · · · · · · · · 
· · · ■ · · ■ · · · □ · · · · · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · □ · · · · □ · · · · · · · · · · · · · · · · · · · · · · · · □ · · · · · · · · ■ · · · · · · 
· · · · · □ · · · · · · · · · · · · · · · · · · · □ · · · · · · · · · · · · · · · □ · · · · · · □ · 
· · · · ■ · · · · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · □ · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · ■ · · □ · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · □ · · · · □ · · □ · · · · · · · · · ■ · · · · · · · · · 
· · · · · · · · · · · □ · · · · □ · · · · · · · · · · · · · · · · · · · · □ · · · · · · ■ · · · · · 
· · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · · ■ · · · · · · · · · · · 
· · · · · ■ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · ■ · · · ■ · · · · · · ■ · · · ■ · · · · · · · · · · · · · · 
· · · · · · ■ · · · · · · ■ · · · · · · · · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · 
· □ · · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · · · · · · · ■ · · ■ · · · · 
· · · · · · · · · · · · · · · · · · · · □ · · · · ■ · □ · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · ■ · · · · · · · · · · · □ · · · □ · · · · · □ · · · · · · · · ■ · □ · · · · · · · · 
· · · · □ · □ · · · · ■ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · · · · 
· · · · · · · · · · · · · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · 
· · · · · · · · · · · · · · · · · · · · · · · · □ □ · · · · □ · · · · · · · · · · · · · · · · ■ · · 
□ · · · · · · · · · ■ · · · · · · · · · · · · · · · · · ■ · · □ · ■ · · · ■ · · · ■ · · · · · · ■ · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · · □ · · · · · · 
· · · · · · · · · · · · · · · · · · ■ · · · · · · · · · · · · ■ · · ■ · · · □ · □ · · · · · · · · · 
· · · · · · · · · · · · · ■ · · · · · · · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · □ · ■ · · □ · · · · · · · · · ■ · · · · · · · ■ ■ · · · · · □ · · · · · · · · · · ■ · 
· · · · · · · · · ■ · · · · ■ · ■ · · · · ■ · · · · · □ · · · · ■ · · · · · · □ · · · □ · · · · · · 
□ · · · □ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
■ · · · · · · · · · · · · · · · · · · P □ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· ■ · · · · · · · · · · · · · □ · · · · · · · · · · · · · · · · □ · · · · · · · · · · · · · · · · · 
■ o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · · □ · · · · · 
o o o o · · · · · · · · · · · · · · · · · · · · □ · · · · · · · · □ · · · · · · · · · · · □ · · · · 
o o o o o o · · · · · · □ · · · · · · · · · · · · · ■ · · · · · · ■ · · · ■ · · · · □ · · · · · · · 
o o o o o o o · o · · · · · · · ■ · · · · · · · · · · □ · · · · · · · · · · · · · · · · □ ■ · · · · 
o o o o o □ o o o o o · · · · · · · · · · · · · · · · · · · · · ■ · · · · · · · · · · · · · □ ■ · · 
o o o ■ o o o o o □ o o □ · · □ · · · · · · · · · · · · · ■ · □ · · · · · · · · · · · · · · · · · · 
o o o o o o o o o o o o o · · · · · · · · · · · · · · · □ · · · · · · · □ · · · · · □ · · · · · · · 
o o o o o o o ■ o o o o o o · · · · · · · · · · · · · · · · ■ · □ · · · · · · · ■ · □ · · · · · · · 
o o o o □ □ o o ■ o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · · · · □ · · · 
o o o o o o o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · · · · · · 
o o o □ o □ o o o □ o ■ o o o · · · · · · · · · ■ □ · · · · · · · · · · · · · · · · · · · · · · · · 
o o o o o o o o □ o o o □ o o · · · · · · · · · · · □ · · · · · · · · · · · · · · · · · · · · · · · 
o o o □ o □ o ■ o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · □ · 
o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · · · · ■ · □ · · · ■ · · · · · · · · · · 
o o o o o o o o o ■ ■ o o o ■ o o · · · · · · · · · ■ · · · · · □ · · · · · · · · ■ · · · · · ■ · · 
o o o o o o o o o o o o o o o o o · · · · · · · · · ■ · · · · · · · · · · · · · · · · · · · · · · · 
o P o o o o o o o o o o o o o o □ · · · · · · · · · · · · · · · · · · · · · · · · · · · ■ · · · □ · 
o o o o o o o o o o o o o o o o o · · · · · · · · · · · · □ · □ · · · · · · · · · · · · · · · · ■ · 
";
            Assert.AreEqual(mapMovementResult, mapMovementString);

            //Fred aims at Jethro, who is behind high cover· 
            Assert.AreEqual(fred.TargetCharacters[0], jethro.Name);

            int chanceToHit = EncounterCore.GetChanceToHit(fred, fred.WeaponEquipped, jethro);
            int chanceToCrit = EncounterCore.GetChanceToCrit(mission.Map, fred, fred.WeaponEquipped, jethro, false);
            DamageRange damageOptions = EncounterCore.GetDamageRange(fred, fred.WeaponEquipped);
            Assert.AreEqual(80, chanceToHit);
            Assert.AreEqual(70, chanceToCrit);
            Assert.AreEqual(3, damageOptions.DamageLow);
            Assert.AreEqual(5, damageOptions.DamageHigh);

            //Fred shoots at Jethro, and misses 
            EncounterResult encounter1 = mission.AttackCharacter(fred,
                   fred.WeaponEquipped,
                   jethro,
                   team1,
                   team2);
            string log1 = @"
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 80, (dice roll: 8)
Low cover downgraded to no cover at <5, 0, 5>
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log1, encounter1.LogString);
            Assert.AreEqual(new(5, 0, 5), encounter1.MissedLocation);

            //Fred shoots at Jethro, and hits him· 
            EncounterResult encounter2 = mission.AttackCharacter(fred,
               fred.WeaponEquipped,
               jethro,
               team1,
               team2);
            mission.EndMission();
            string log2 = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 81)
Damage range: 3-5, (dice roll: 76)
Critical chance: 70, (dice roll: 55)
Critical damage range: 8-12, (dice roll: 76)
11 damage dealt to character Jethro, HP is now -7
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log2, encounter2.LogString);

        }

        [TestMethod]
        public void OverwatchTest()
        {
            //Arrange
            string path = _rootPath + "Save022.json";

            //Act
            string fileContents;
            using (var streamReader = new StreamReader(path))
            {
                fileContents = streamReader.ReadToEnd();
            }
            Mission mission = GameSerialization.LoadGameFile(path);
            mission.StartMission();
            Character fred = mission.Teams[0].Characters[0];
            Character henry = mission.Teams[0].Characters[1];
            Character jeff = mission.Teams[0].Characters[2];
            fred.InOverwatch = true;
            Character jethro = mission.Teams[1].Characters[0];
            Character bart = mission.Teams[1].Characters[1];
            Team team1 = mission.Teams[0];
            Team team2 = mission.Teams[1];

            //Assert
            Assert.AreEqual(true, fred.InOverwatch);
            Assert.AreEqual(true, henry.InOverwatch);
            Assert.AreEqual(true, jeff.InOverwatch);
            Assert.AreEqual("Jethro", jethro.Name);

            mission.MoveToNextTurn();

            AIAction aIAction = mission.CalculateAIAction(jethro, team2, team1);
            Assert.AreEqual(new(19, 0, 19), aIAction.StartLocation);
            Assert.AreEqual(new(13, 0, 25), aIAction.EndLocation);
            List<MovementAction> movementActions = mission.MoveCharacter(jethro,
                       team2,
                       team1,
                       aIAction.EndLocation);


            Assert.AreEqual(1, movementActions.Count);
            Assert.AreEqual(1, movementActions[0].OverwatchEncounterResults.Count);
            string expectedLog = @"
Harry is attacking with Sniper Rifle, targeted on Jethro
Hit: Chance to hit: 56, (dice roll: 81)
Damage range: 3-5, (dice roll: 76)
Critical chance: 0, (dice roll: 55)
4 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Harry, for a total of 100 XP
Harry is ready to level up
";
            Assert.AreEqual(expectedLog, movementActions[0].OverwatchEncounterResults[0].LogString);


            AIAction aIAction2 = mission.CalculateAIAction(bart, team2, team1);
            Assert.AreEqual(new(26, 0, 32), aIAction2.StartLocation);
            Assert.AreEqual(new(28, 0, 28), aIAction2.EndLocation);
            List<MovementAction> movementActions2 = mission.MoveCharacter(bart,
                       team2,
                       team1,
                       aIAction2.EndLocation);

            Assert.AreEqual(1, movementActions2.Count);
            Assert.AreEqual(1, movementActions2[0].OverwatchEncounterResults.Count);
            string expectedLog2 = @"
Jeff is attacking with Rifle, targeted on Bart
Hit: Chance to hit: 56, (dice roll: 55)
Damage range: 3-5, (dice roll: 90)
Critical chance: 0, (dice roll: 44)
4 damage dealt to character Bart, HP is now 0
Bart is killed
100 XP added to character Jeff, for a total of 100 XP
Jeff is ready to level up
";
            Assert.AreEqual(expectedLog2, movementActions2[0].OverwatchEncounterResults[0].LogString);

        }
    }
}
