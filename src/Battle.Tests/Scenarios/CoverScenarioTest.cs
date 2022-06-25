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
    public class CoverScenarioTest
    {
        //Enemy is behind cover and not visible.
        //Throw grenade to destory cover and make him visible.
        [TestMethod]
        public void JethroIsHidingBehindCoverScenarioTest()
        {
            //Arrange
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(50, 1, 50)
            };
            mission.Map[5, 0, 6] = MapObjectType.FullCover;
            mission.Map[14, 0, 5] = MapObjectType.FullCover;
            mission.Map[14, 0, 6] = MapObjectType.FullCover;
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
            mission.RandomNumbers = new(new List<int> { 100, 100, 100, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll
            mission.StartMission();

            //Assert - Setup
            Assert.AreEqual(1, mission.TurnNumber);
            Assert.AreEqual(2, mission.Teams.Count);
            Assert.AreEqual(50 * 50, mission.Map.Length);
            Assert.AreEqual("Good guys", mission.Teams[0].Name);
            Assert.AreEqual(1, mission.Teams[0].Characters.Count);
            Assert.AreEqual("Bad guys", mission.Teams[1].Name);
            Assert.AreEqual(1, mission.Teams[1].Characters.Count);

            //Act

            //Turn 1 - Team 1 starts
            //Fred cannot see Jethro, who is hiding behind cover
            string mapString1 = fred.GetCharactersInViewMapString(mission.Map, team2.Characters);
            Assert.AreEqual(0, fred.TargetCharacters.Count);
            string mapStringExpected1 = @"
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · o o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · o o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · o o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
o · · · · · · · · · o o o o ■ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
o o · · · · · · · o o o o o ■ · · · · · · · · · · · · · · · · · · · o · · · · · · · · · · · · · · · 
o o · · · · · · · o o o o o ■ · · · · · · · · · · · · · · · o o o o o o · · · · · · · · · · · · · · 
o o o · · · · · o o o o o o ■ · · · · · · · · · · · o o o o o o o o o o · · · · · · · · · · · · · · 
o o o · · · · · o o o o o o ■ P · · · · · · o o o o o o o o o o o · · · · · · · · · · · · · · · · · 
o o o o · · · o o o o o o o ■ · · · · o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · 
o o o o · · · o o o o o o o ■ o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · 
o o o o o · o o o o o o o o □ o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
o o o o o ■ o o o o o o o o ■ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
o o o o o P o o o o o o o o ■ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · 
";
            Assert.AreEqual(mapStringExpected1, mapString1);

            //Throw grenade in front of wall
            Vector3 targetThrowingLocation = new(13, 0, 10);
            EncounterResult encounter1 = mission.AttackCharacterWithAreaOfEffect(fred,
                fred.UtilityWeaponEquipped,
                team1,
                team2,
                targetThrowingLocation);
            string log1 = @"
Fred is attacking with area effect Grenade aimed at <13, 0, 10>
Characters in affected area: Jethro
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 100)
Critical damage range: 3-4, (dice roll: 100)
4 damage dealt to character Jethro, HP is now 1
10 XP added to character Fred, for a total of 10 XP
High cover downgraded to low cover at <14, 0, 8>
Low cover downgraded to no cover at <14, 0, 7>
High cover downgraded to low cover at <14, 0, 12>
High cover downgraded to low cover at <14, 0, 13>
High cover downgraded to low cover at <14, 0, 9>
High cover downgraded to low cover at <14, 0, 11>
High cover downgraded to low cover at <14, 0, 10>
";
            Assert.AreEqual(log1, encounter1.LogString);
            Assert.AreEqual(7, encounter1.AffectedMap.Count);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new(14, 0, 8), 1), encounter1.AffectedMap[0]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new(14, 0, 7), 1), encounter1.AffectedMap[1]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new(14, 0, 12), 1), encounter1.AffectedMap[2]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new(14, 0, 13), 1), encounter1.AffectedMap[3]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new(14, 0, 9), 1), encounter1.AffectedMap[4]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new(14, 0, 11), 1), encounter1.AffectedMap[5]);
            Assert.AreEqual(new KeyValuePair<Vector3, int>(new(14, 0, 10), 1), encounter1.AffectedMap[6]);

            string mapString2 = fred.GetCharactersInViewMapString(mission.Map, team2.Characters);
            Assert.AreEqual(1, fred.TargetCharacters.Count);
            string mapStringExpected2 = @"
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · · o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · · o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · o o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · · o o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · o o o o o o o o o o · o o · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · · o o o o o o o o o · o o o · · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · o o o o o o o o o · o o o o o · · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · · o o o o o o o o · o o o o o o o · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · o o o o o o o o · o o o o o o o o · · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · · o o o o o o o · o o o o o o o o o o · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · · o o o o o o o · o o o o o o o o o o o · · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · o o o o o o o · o o o o o o o o o o o o o · · · · · · · · · · · · · · · · · 
· · · · · · · · · · · · o o o o o o · o o o o o o o o o o o o o o o · · · · · · · · · · · · · · · · 
· · · · · · · · · · · o o o o o o · o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · · · 
· · · · · · · · · · · o o o o o · o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · · 
· · · · · · · · · · o o o o o · o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · · 
o · · · · · · · · · o o o o ■ o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · · 
o o · · · · · · · o o o o o □ o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · · 
o o · · · · · · · o o o o o □ o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · 
o o o · · · · · o o o o o o □ o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · 
o o o · · · · · o o o o o o □ P o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · · · · 
o o o o · · · o o o o o o o □ o o o o o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · 
o o o o · · · o o o o o o o □ o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · 
o o o o o · o o o o o o o o . o o o · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
o o o o o ■ o o o o o o o o ■ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
o o o o o P o o o o o o o o ■ · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · · 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · · · · · · · · · 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · 
o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o o · · · · · · · · · · · · · · 
";
            Assert.AreEqual(mapStringExpected2, mapString2);

            //End of of battle
            mission.EndMission();

            //Assert
            Assert.AreEqual(1, fred.MissionsCompleted);
            Assert.AreEqual(1, jethro.MissionsCompleted);
            Assert.AreEqual(0, fred.TotalShots);
            Assert.AreEqual(1, fred.TotalHits);
            Assert.AreEqual(0, fred.TotalKills);
            Assert.AreEqual(4, fred.TotalDamage);
            Assert.AreEqual(0, jethro.TotalShots);
            Assert.AreEqual(0, jethro.TotalHits);
            Assert.AreEqual(0, jethro.TotalKills);
            Assert.AreEqual(0, jethro.TotalDamage);
        }
    }
}
