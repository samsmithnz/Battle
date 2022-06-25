using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.Items;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Battle.Tests.Encounters
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class RegularAttackTests
    {
        [TestMethod]
        public void FredAttacksJethroWithRifleAndHitsTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 12;
            RandomNumberQueue diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            int chanceToHit = EncounterCore.GetChanceToHit(fred, rifle, jethro);
            int chanceToCrit = EncounterCore.GetChanceToCrit(map, fred, rifle, jethro, false);
            DamageRange damageOptions = EncounterCore.GetDamageRange(fred, rifle);
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(80, chanceToHit);
            Assert.AreEqual(70, chanceToCrit);
            Assert.IsNotNull(damageOptions);
            Assert.AreEqual(3, damageOptions.DamageLow);
            Assert.AreEqual(5, damageOptions.DamageHigh);
            Assert.AreEqual(8, damageOptions.CriticalDamageLow);
            Assert.AreEqual(12, damageOptions.CriticalDamageHigh);
            Assert.AreEqual(7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jethro, HP is now 7
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleAndMissesHittingHighCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[8, 0, 9] = MapObjectType.FullCover;
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.ChanceToHit = 45;
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.CoverState.InFullCover = false;
            RandomNumberQueue diceRolls = new(new List<int> { 44 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.SourceCharacter.XP);
            Assert.AreEqual(false, result.IsHit);
            Assert.AreEqual(new(8, 0, 9), result.MissedLocation);
            Assert.AreEqual(MapObjectType.HalfCover, map[8, 0, 9]);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 55, (dice roll: 44)
High cover downgraded to low cover at <8, 0, 9>
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleAndMissesHittingLowCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[8, 0, 9] = MapObjectType.HalfCover;
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.ChanceToHit = 45;
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.CoverState.InFullCover = false;
            RandomNumberQueue diceRolls = new(new List<int> { 44 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.SourceCharacter.XP);
            Assert.AreEqual(false, result.IsHit);
            Assert.AreEqual(new(8, 0, 9), result.MissedLocation);
            Assert.AreEqual(MapObjectType.NoCover, map[8, 0, 9]);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 35, (dice roll: 44)
Low cover downgraded to no cover at <8, 0, 9>
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleAndMissesHittingNoCoverTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.ChanceToHit = 45;
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.CoverState.InFullCover = false;
            RandomNumberQueue diceRolls = new(new List<int> { 44 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.SourceCharacter.XP);
            Assert.AreEqual(false, result.IsHit);
            Assert.AreEqual(new(8, 0, 9), result.MissedLocation);
            Assert.AreEqual("", map[8, 0, 9]);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 55, (dice roll: 44)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleModifiersAndHitsTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.ChanceToHit = 45;
            Weapon rifle = fred.WeaponEquipped;
            rifle.ChanceToHitAdjustment = 20;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.CoverState.InFullCover = false;
            jethro.HitpointsCurrent = 12;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 65, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jethro, HP is now 7
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        //The encounter has no random numbers and returns null
        [TestMethod]
        public void NoDiceRollsEncounterTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            RandomNumberQueue diceRolls = null;

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void FredAttacksAndKillsJethroWithRifleTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 5;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 20 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 20)
5 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksAndKillsJethroWithRifleAndCriticalHitTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 12;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 30 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);
            MapCore.GetMapString(map);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(12, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 30)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksAndKillsJethroWithRifleAndCritsTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 5;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        //Fred hits Jethro with a rifle, causing 10 points of damage, killing him, and leveling up
        [TestMethod]
        public void FredAttacksAndKillsJethroWithRifleCausingFredToLevelUpTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.XP = 0;
            fred.Level = 1;
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 5;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 30 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(1, result.SourceCharacter.Level);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 30)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jethro, HP is now -7
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleWhoIsInHalfCoverAndHitsTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.ChanceToHit = 85;
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.CoverState.InHalfCover = true;
            jethro.HitpointsCurrent = 12;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 75, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jethro, HP is now 7
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleWhoIsInFullCoverAndMissesTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.CoverState.InFullCover = true;
            RandomNumberQueue diceRolls = new(new List<int> { 55 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.SourceCharacter.XP);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 40, (dice roll: 55)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleWhoIsInFullCoverDiagAndMissesWithNegativeChanceToHitTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.ChanceToHit = 30;
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.CoverState.InFullCover = true;
            RandomNumberQueue diceRolls = new(new List<int> { 65 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.SourceCharacter.XP);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 0, (dice roll: 65)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }



        [TestMethod]
        public void FredAttacksJethroWithRifleWhoIsInFullCoverStraightOnAndMissesWithNegativeChanceToHitTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.ChanceToHit = 30;
            fred.XP = 50;
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(5, 0, 0));
            jethro.CoverState.InFullCover = true;
            RandomNumberQueue diceRolls = new(new List<int> { 65 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(50, result.SourceCharacter.XP);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 19, (dice roll: 65)
0 XP added to character Fred, for a total of 50 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleAndHitsWithPlus10DamageAbilityTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.Abilities.Add(AbilityPool.SharpShooterAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 15;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-15, (dice roll: 100)
Critical chance: 70, (dice roll: 100)
Critical damage range: 8-22, (dice roll: 100)
22 damage dealt to character Jethro, HP is now -7
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleAndHitsWithPlus5DamageTwiceAbilityTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.Abilities.Add(AbilityPool.SharpShooterAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 15;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 100 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(-7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-15, (dice roll: 100)
Critical chance: 70, (dice roll: 100)
Critical damage range: 8-22, (dice roll: 100)
22 damage dealt to character Jethro, HP is now -7
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksAndKillsJethroWithRifleAndCriticalChanceAbilityBonusTest()
        {
            //Arrange
            Mission mission = new()
            {
                Map = MapCore.InitializeMap(10, 1, 10)
            };
            Character fred = CharacterPool.CreateFredHero(mission.Map, new(1, 0, 1));
            fred.Abilities.Add(AbilityPool.PlatformStabilityAbility());
            Weapon rifle = fred.WeaponEquipped;
            mission.Teams.Add(new(1)
            {
                Name = "good",
                Characters = new() { fred }
            });
            Character jethro = CharacterPool.CreateJethroBaddie(mission.Map, new(8, 0, 8));
            jethro.HitpointsCurrent = 12;
            mission.Teams.Add(new(0)
            {
                Name = "bad",
                Characters = new() { jethro }
            });
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 30 }); //Chance to hit roll, damage roll, critical chance roll         
            CharacterCover.RefreshCoverStates(mission);

            //Act
            string mapStringBefore = MapCore.GetMapString(mission.Map);
            EncounterResult result = Encounter.AttackCharacter(mission.Map, fred, rifle, jethro, diceRolls);
            string mapStringAfter = MapCore.GetMapString(mission.Map);

            //Assert
            string mapStringBeforeExpected = @"
· · · · · · · · · · 
· · · · · · · · P · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· P · · · · · · · · 
· · · · · · · · · · 
";
            Assert.AreEqual(mapStringBeforeExpected, mapStringBefore);
            string mapStringAfterExpected = @"
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· · · · · · · · · · 
· P · · · · · · · · 
· · · · · · · · · · 
";
            Assert.AreEqual(mapStringAfterExpected, mapStringAfter);
            Assert.IsTrue(result != null);
            Assert.AreEqual(12, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 81, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 80, (dice roll: 30)
Critical damage range: 8-12, (dice roll: 100)
12 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }



        [TestMethod]
        public void FredAttacksAndKillsJethroWithRifleAndCriticalDamageAbilityBonusTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            fred.Abilities.Add(AbilityPool.BringEmOnAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 15;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 30 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(15, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 30)
Critical damage range: 11-15, (dice roll: 100)
15 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksWithRifleJethroBehindCoverAndInjuriesHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jethro
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  · · E · .
            //  · · ■ · · 
            //  · · · · · 
            //  · · · · .
            //  · · P · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = MapObjectType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 0));
            fred.Abilities.Add(AbilityPool.BringEmOnAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(2, 0, 4));
            jethro.HitpointsCurrent = 15;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(10, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 64, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 20, (dice roll: 0)
5 damage dealt to character Jethro, HP is now 10
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksWithRifleJethroWhoIsFlankedAndKillsHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jethro
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  · · · · .
            //  · E ■ · · 
            //  · · · · · 
            //  · · · · .
            //  · · P · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = MapObjectType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 0));
            fred.Abilities.Add(AbilityPool.BringEmOnAbility());
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 15;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 70 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(15, result.DamageDealt);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 68, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 70)
Critical damage range: 11-15, (dice roll: 100)
15 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksPlayerWhoIsOffMapTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jethro
            //  CoverType.FullCover = cover
            //  "." = open ground
            //            E
            //  · · · · .
            //  · · ■ · · 
            //  · · · · · 
            //  · · · · .
            //  · · P · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = MapObjectType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(5, 0, 5));
            RandomNumberQueue diceRolls = new(new List<int> { 65, 65, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            try
            {
                EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);
            }
            catch (Exception ex)
            {
                //Assert
                Assert.AreEqual("The character is off the map", ex.Message);
            }
        }

        [TestMethod]
        public void FredAttacksWithRifleJethroBehindFullCoverAndHunkeredDownMissingHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jethro
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  · · E · .
            //  · · ■ · · 
            //  · · · · · 
            //  · · · · .
            //  · · P · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = MapObjectType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(2, 0, 4));
            jethro.HitpointsCurrent = 15;
            jethro.CoverState.InFullCover = true;
            jethro.HunkeredDown = true;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(15, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(0, result.SourceCharacter.XP);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 24, (dice roll: 65)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksWithRifleJethroBehindHalfCoverAndHunkeredDownMissingHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jethro
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  · · E · .
            //  · · □ · · 
            //  · · · · · 
            //  · · · · .
            //  · · P · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = MapObjectType.HalfCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(2, 0, 4));
            jethro.HitpointsCurrent = 15;
            jethro.CoverState.InFullCover = true;
            jethro.HunkeredDown = true;
            RandomNumberQueue diceRolls = new(new List<int> { 35, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            //Assert.AreEqual(5, result.DamageDealt);
            //Assert.AreEqual(false, result.IsCriticalHit);
            //Assert.AreEqual(15, result.TargetCharacter.HitpointsCurrent);
            //Assert.AreEqual(0, result.SourceCharacter.XP);
            //Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Missed: Chance to hit: 64, (dice roll: 35)
0 XP added to character Fred, for a total of 0 XP
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksWithRifleJethroBehindFullCoverAndHunkeredDownInjuriesHimTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jethro
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  · · E · .
            //  · · ■ · · 
            //  · · · · · 
            //  · · · · .
            //  · · P · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = MapObjectType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(2, 0, 4));
            jethro.HitpointsCurrent = 15;
            jethro.CoverState.InHalfCover = true;
            jethro.HunkeredDown = true;
            RandomNumberQueue diceRolls = new(new List<int> { 65, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(5, result.DamageDealt);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(10, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 64, (dice roll: 65)
Damage range: 3-5, (dice roll: 100)
Critical chance: 0, hunkered down
5 damage dealt to character Jethro, HP is now 10
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }


        [TestMethod]
        public void FredAttacksJethroWithRifleWithNoAmmoTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            rifle.AmmoCurrent = 0;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            RandomNumberQueue diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            //Assert.AreEqual(7, result.TargetCharacter.Hitpoints);
            //Assert.AreEqual(10, result.SourceCharacter.Experience);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Rifle has no ammo remaining and the attack cannot be completed
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredAttacksJethroWithRifleWithNoAmmoAndReloadsFirstTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            Character fred = CharacterPool.CreateFredHero(map, new(0, 0, 0));
            Weapon rifle = fred.WeaponEquipped;
            rifle.AmmoCurrent = 0;
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            jethro.HitpointsCurrent = 12;
            RandomNumberQueue diceRolls = new(new List<int> { 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll

            //Act
            fred.WeaponEquipped.Reload();
            EncounterResult result = Encounter.AttackCharacter(map, fred, rifle, jethro, diceRolls);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.TargetCharacter.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            Assert.AreEqual(3, result.SourceCharacter.WeaponEquipped.AmmoCurrent);
            string log = @"
Fred is attacking with Rifle, targeted on Jethro
Hit: Chance to hit: 80, (dice roll: 80)
Damage range: 3-5, (dice roll: 100)
Critical chance: 70, (dice roll: 0)
5 damage dealt to character Jethro, HP is now 7
10 XP added to character Fred, for a total of 10 XP
";
            Assert.AreEqual(log, result.LogString);
        }

    }
}