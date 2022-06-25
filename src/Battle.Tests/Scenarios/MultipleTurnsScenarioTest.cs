using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Scenarios
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L2")]
    public class MultipleTurnsScenarioTest
    {
        [TestMethod]
        public void MultipleTurnsTest()
        {
            //Arrange
            bool teamIsDone;
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            Character fred = CharacterPool.CreateFredHero(mission.Map, new(4, 0, 4));
            Team team1 = new(1)
            {
                Name = "Good guys",
                Characters = new() { fred },
                Color = "Blue"
            };
            mission.Teams.Add(team1);
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new(12, 0, 12));
            Team team2 = new(0)
            {
                Name = "Bad guys",
                Characters = new() { jethro },
                Color = "Red"
            };
            mission.Teams.Add(team2);
            mission.RandomNumbers = new(new List<int> { 100, 100, 0, 0, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll
            mission.StartMission();

            //Assert - Setup
            Assert.AreEqual(1, mission.Objectives.Count);
            Assert.AreEqual(MissionObjectiveType.EliminateAllOpponents, mission.Objectives[0].Type);
            Assert.AreEqual(1, mission.TurnNumber);
            Assert.AreEqual(2, mission.Teams.Count);
            Assert.AreEqual(50 * 50, mission.Map.Length);
            Assert.AreEqual("Good guys", mission.Teams[0].Name);
            Assert.AreEqual("Blue", mission.Teams[0].Color);
            Assert.AreEqual(1, mission.Teams[0].Characters.Count);
            Assert.AreEqual("Bad guys", mission.Teams[1].Name);
            Assert.AreEqual("Red", mission.Teams[1].Color);
            Assert.AreEqual(1, mission.Teams[1].Characters.Count);
            Assert.AreEqual(1, mission.TurnNumber);
            Assert.AreEqual(0, mission.CurrentTeamIndex);

            //Act

            //Turn 1 - Team 1 starts
            //Fred moves 
            Vector3 fredDestination = new(14, 0, 14);
            mission.MoveCharacter(fred,
                team1,
                team2,
                fredDestination);
            Assert.AreEqual(0, fred.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(true, teamIsDone);
            bool missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 1 - Team 2 starts
            //Jethro moves
            Assert.AreEqual(1, mission.TurnNumber);
            Assert.AreEqual(1, mission.CurrentTeamIndex);
            Vector3 jethroDestination = new(2, 0, 2);
            mission.MoveCharacter(jethro,
                team2,
                team1,
                jethroDestination);
            Assert.AreEqual(0, jethro.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(true, teamIsDone);
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 2 - Team 1 starts
            Assert.AreEqual(2, mission.TurnNumber);
            Assert.AreEqual(0, mission.CurrentTeamIndex);
            fredDestination = new(4, 0, 4);
            mission.MoveCharacter(fred,
                team1,
                team2,
                fredDestination);
            Assert.AreEqual(0, fred.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(true, teamIsDone);
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 2 - Team 2 starts
            Assert.AreEqual(2, mission.TurnNumber);
            Assert.AreEqual(1, mission.CurrentTeamIndex);
            jethroDestination = new(12, 0, 12);
            mission.MoveCharacter(jethro,
                team2,
                team1,
                jethroDestination);
            Assert.AreEqual(0, jethro.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(true, teamIsDone);
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 3 - Team 1 starts - force an end turn
            Assert.AreEqual(3, mission.TurnNumber);
            Assert.AreEqual(0, mission.CurrentTeamIndex);
            fredDestination = new(5, 0, 5);
            mission.MoveCharacter(fred,
               team1,
               team2,
               fredDestination);
            Assert.AreEqual(1, fred.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(false, teamIsDone);
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 3 - Team 2 starts - force an end turn
            Assert.AreEqual(3, mission.TurnNumber);
            Assert.AreEqual(1, mission.CurrentTeamIndex);
            jethroDestination = new(11, 0, 11);
            mission.MoveCharacter(jethro,
                team2,
                team1,
                jethroDestination);
            Assert.AreEqual(1, jethro.ActionPointsCurrent);
            teamIsDone = mission.CheckIfCurrentTeamIsDoneTurn();
            Assert.AreEqual(false, teamIsDone);
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(false, missionIsComplete);
            mission.MoveToNextTurn();

            //Turn 4 - Team 1 starts, and aborts mission
            //Fred shoots at Jethro and kills him· 
            EncounterResult encounter1 = mission.AttackCharacter(fred,
                fred.WeaponEquipped,
                jethro,
                team1,
                team2);
            string log1 = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 86, (dice roll: 100)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jethro, HP is now -1
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log1, encounter1.LogString);

            //End of of battle
            missionIsComplete = mission.CheckIfMissionIsCompleted();
            Assert.AreEqual(true, missionIsComplete);
            mission.EndMission();

            //Assert
            Assert.AreEqual(-1, jethro.HitpointsCurrent);
            Assert.AreEqual(1, fred.MissionsCompleted);
            Assert.AreEqual(0, jethro.MissionsCompleted);
            Assert.AreEqual(1, fred.TotalShots);
            Assert.AreEqual(1, fred.TotalHits);
            Assert.AreEqual(0, fred.TotalMisses);
            Assert.AreEqual(1, fred.TotalKills);
            Assert.AreEqual(5, fred.TotalDamage);
            Assert.AreEqual(0, jethro.TotalShots);
            Assert.AreEqual(0, jethro.TotalHits);
            Assert.AreEqual(0, jethro.TotalMisses);
            Assert.AreEqual(0, jethro.TotalKills);
            Assert.AreEqual(0, jethro.TotalDamage);
        }
    }
}
