using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Battle.Tests.Scenarios
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L1")]
    public class TargetingScenarioTest
    {
        [TestMethod]
        public void MultipleTurnsTest()
        {
            //Arrange
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            Character fred = CharacterPool.CreateFredHero(mission.Map, new(4, 0, 4));
            Character harry = CharacterPool.CreateHarryHero(mission.Map, new(4, 0, 6));
            Character jeff = CharacterPool.CreateJeffHero(mission.Map, new(4, 0, 8));
            Team team1 = new(1)
            {
                Name = "Good guys",
                Characters = new() { fred, harry, jeff },
                Color = "Blue"
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new(14, 0, 4));
            Character bart = CharacterPool.CreateBartBaddie(mission.Map, new(14, 0, 6));
            Character derek = CharacterPool.CreateDerekBaddie(mission.Map, new(14, 0, 8));
            Team team2 = new(0)
            {
                Name = "Bad guys",
                Characters = new() { jethro, bart, derek },
                Color = "Red"
            };
            mission.Teams.Add(team2);
            mission.RandomNumbers = new(new List<int> { 100, 100, 0, 0, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll
            mission.StartMission();

            //Act: Turn 1 - Team 1 starts
            Assert.AreEqual(0, fred.TargetCharacterIndex);
            fred.NextTarget();
            Assert.AreEqual("Bart", fred.GetTargetCharacter());
            Assert.AreEqual(1, fred.TargetCharacterIndex);
            fred.NextTarget();
            Assert.AreEqual("Derek", fred.GetTargetCharacter());
            Assert.AreEqual(2, fred.TargetCharacterIndex);
            fred.NextTarget();
            Assert.AreEqual("Jethro", fred.GetTargetCharacter());
            Assert.AreEqual(0, fred.TargetCharacterIndex);
            ;
            Assert.AreEqual(3, fred.TargetCharacters.Count);
            Assert.AreEqual(3, harry.TargetCharacters.Count);
            Assert.AreEqual(3, jeff.TargetCharacters.Count);
            Assert.AreEqual(3, jethro.TargetCharacters.Count);
            Assert.AreEqual(3, bart.TargetCharacters.Count);
            Assert.AreEqual(3, derek.TargetCharacters.Count);

            EncounterResult encounterResult1 = mission.AttackCharacter(fred, fred.WeaponEquipped, jethro, team1, team2);
            string log1 = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jethro, HP is now -1
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log1, encounterResult1.LogString);

            Assert.AreEqual(2, fred.TargetCharacters.Count);
            Assert.AreEqual(2, harry.TargetCharacters.Count);
            Assert.AreEqual(2, jeff.TargetCharacters.Count);
            Assert.AreEqual(3, jethro.TargetCharacters.Count);
            Assert.AreEqual(3, bart.TargetCharacters.Count);
            Assert.AreEqual(3, derek.TargetCharacters.Count);

            Assert.AreEqual(0, fred.TargetCharacterIndex);
            fred.NextTarget();
            Assert.AreEqual("Derek", fred.GetTargetCharacter());
            Assert.AreEqual(1, fred.TargetCharacterIndex);
            fred.NextTarget();
            Assert.AreEqual("Bart", fred.GetTargetCharacter());
            Assert.AreEqual(0, fred.TargetCharacterIndex);

            //Test that we get the right next character
            mission.MoveToNextTurn();
            Character firstCharacter = mission.Teams[mission.CurrentTeamIndex].GetFirstCharacter();
            Assert.AreEqual(firstCharacter.Name, mission.Teams[mission.CurrentTeamIndex].Characters[1].Name);

            //            //Turn 1 - Team 2 starts
            //            //Jethro moves
            //            Assert.AreEqual(1, mission.TurnNumber);
            //            Assert.AreEqual(1, mission.CurrentTeamIndex);
            //            Vector3 jethroDestination = new(2, 0, 2);
            //            mission.MoveCharacter(jethro,
            //                team2,
            //                team1,
            //                jethroDestination);
            //            Assert.AreEqual(0, jethro.ActionPointsCurrent);
            //            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            //            Assert.AreEqual(true, teamIsDone);
            //            missionIsComplete = mission.CheckIfMissionIsCompleted();
            //            Assert.AreEqual(false, missionIsComplete);
            //            mission.MoveToNextTurn();

            //            //Turn 2 - Team 1 starts
            //            Assert.AreEqual(2, mission.TurnNumber);
            //            Assert.AreEqual(0, mission.CurrentTeamIndex);
            //            fredDestination = new(4, 0, 4);
            //            mission.MoveCharacter(fred,
            //                team1,
            //                team2,
            //                fredDestination);
            //            Assert.AreEqual(0, fred.ActionPointsCurrent);
            //            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            //            Assert.AreEqual(true, teamIsDone);
            //            missionIsComplete = mission.CheckIfMissionIsCompleted();
            //            Assert.AreEqual(false, missionIsComplete);
            //            mission.MoveToNextTurn();

            //            //Turn 2 - Team 2 starts
            //            Assert.AreEqual(2, mission.TurnNumber);
            //            Assert.AreEqual(1, mission.CurrentTeamIndex);
            //            jethroDestination = new(12, 0, 12);
            //            mission.MoveCharacter(jethro,
            //                team2,
            //                team1,
            //                jethroDestination);
            //            Assert.AreEqual(0, jethro.ActionPointsCurrent);
            //            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            //            Assert.AreEqual(true, teamIsDone);
            //            missionIsComplete = mission.CheckIfMissionIsCompleted();
            //            Assert.AreEqual(false, missionIsComplete);
            //            mission.MoveToNextTurn();

            //            //Turn 3 - Team 1 starts - force an end turn
            //            Assert.AreEqual(3, mission.TurnNumber);
            //            Assert.AreEqual(0, mission.CurrentTeamIndex);
            //            fredDestination = new(5, 0, 5);
            //            mission.MoveCharacter(fred,
            //               team1,
            //               team2,
            //               fredDestination);
            //            Assert.AreEqual(1, fred.ActionPointsCurrent);
            //            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            //            Assert.AreEqual(false, teamIsDone);
            //            missionIsComplete = mission.CheckIfMissionIsCompleted();
            //            Assert.AreEqual(false, missionIsComplete);
            //            mission.MoveToNextTurn();

            //            //Turn 3 - Team 2 starts - force an end turn
            //            Assert.AreEqual(3, mission.TurnNumber);
            //            Assert.AreEqual(1, mission.CurrentTeamIndex);
            //            jethroDestination = new(11, 0, 11);
            //            mission.MoveCharacter(jethro,
            //                team2,
            //                team1,
            //                jethroDestination);
            //            Assert.AreEqual(1, jethro.ActionPointsCurrent);
            //            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            //            Assert.AreEqual(false, teamIsDone);
            //            missionIsComplete = mission.CheckIfMissionIsCompleted();
            //            Assert.AreEqual(false, missionIsComplete);
            //            mission.MoveToNextTurn();

            //            //Turn 4 - Team 1 starts, and aborts mission
            //            //Fred shoots at Jethro and kills him· 
            //            EncounterResult encounter1 = mission.AttackCharacter(fred,
            //                fred.WeaponEquipped,
            //                jethro,
            //                team1,
            //                team2);
            //            string log1 = @"
            //Fred is attacking with Rifle, targeted on Jethro
            //Hit: Chance to hit: 86, (dice roll: 100)
            //Damage range: 3-5, (dice roll: 100)
            //Critical chance: 70, (dice roll: 0)
            //5 damage dealt to character Jethro, HP is now -1
            //Jethro is killed
            //100 XP added to character Fred, for a total of 100 XP
            //Fred is ready to level up
            //";
            //            Assert.AreEqual(log1, encounter1.LogString);

            //            //End of of battle
            //            missionIsComplete = mission.CheckIfMissionIsCompleted();
            //            Assert.AreEqual(true, missionIsComplete);
            //            mission.EndMission();

            //            //Assert
            //            Assert.AreEqual(-1, jethro.HitpointsCurrent);
            //            Assert.AreEqual(1, fred.MissionsCompleted);
            //            Assert.AreEqual(0, jethro.MissionsCompleted);
            //            Assert.AreEqual(1, fred.TotalShots);
            //            Assert.AreEqual(1, fred.TotalHits);
            //            Assert.AreEqual(0, fred.TotalMisses);
            //            Assert.AreEqual(1, fred.TotalKills);
            //            Assert.AreEqual(5, fred.TotalDamage);
            //            Assert.AreEqual(0, jethro.TotalShots);
            //            Assert.AreEqual(0, jethro.TotalHits);
            //            Assert.AreEqual(0, jethro.TotalMisses);
            //            Assert.AreEqual(0, jethro.TotalKills);
            //            Assert.AreEqual(0, jethro.TotalDamage);
        }
    }
}
