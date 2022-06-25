using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Game;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Tests.Encounters
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    [TestCategory("L0")]
    public class AreaAttackTests
    {

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJethroTest()
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
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 4;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(true, result.IsHit);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jethro.HitpointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jethro
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
High cover downgraded to low cover at <2, 0, 3>
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJethroWearingArmorTest()
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
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 2;
            jethro.ArmorPointsCurrent = 2;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(2, result.DamageDealt);
            Assert.AreEqual(true, result.IsHit);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jethro.HitpointsCurrent);
            Assert.AreEqual(2, jethro.ArmorPointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jethro
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
Armor prevented 2 damage to character Jethro
2 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
High cover downgraded to low cover at <2, 0, 3>
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJethroWearingArmorThisShreddedTest()
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
            fred.Abilities.Add(AbilityPool.ShredderAbility());
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 4;
            jethro.ArmorPointsCurrent = 2;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(true, result.IsHit);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jethro.HitpointsCurrent);
            Assert.AreEqual(0, jethro.ArmorPointsCurrent);
            Assert.AreEqual(100, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jethro
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
2 armor points shredded
4 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
High cover downgraded to low cover at <2, 0, 3>
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndInjuriesJethroWearingArmorWithShreddedTest()
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
            fred.Abilities.Add(AbilityPool.ShredderAbility());
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 4;
            jethro.ArmorPointsCurrent = 3;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(3, result.DamageDealt);
            Assert.AreEqual(true, result.IsHit);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(1, jethro.HitpointsCurrent);
            Assert.AreEqual(1, jethro.ArmorPointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jethro
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
2 armor points shredded
Armor prevented 1 damage to character Jethro
3 damage dealt to character Jethro, HP is now 1
10 XP added to character Fred, for a total of 10 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndInjuriesJethroTest()
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
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 5;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(true, result.IsHit);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(1, jethro.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jethro
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jethro, HP is now 1
10 XP added to character Fred, for a total of 10 XP
High cover downgraded to low cover at <2, 0, 3>
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJethroAndHarryTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jethro
            //  "H" = enemy/harry
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  · · · · .
            //  · E ■ H · 
            //  · · · · · 
            //  · · · · .
            //  · · P · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = MapObjectType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 0));
            Character harry = CharacterPool.CreateHarryHero(map, new(3, 0, 3));
            harry.HitpointsCurrent = 4;
            harry.ArmorPointsCurrent = 0;
            Team team1 = new(1);
            team1.Characters.Add(fred);
            team1.Characters.Add(harry);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 4;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 0, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro, harry };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(8, result.DamageDealt);
            Assert.AreEqual(true, result.IsHit);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jethro.HitpointsCurrent);
            Assert.AreEqual(0, harry.HitpointsCurrent);
            Assert.AreEqual(200, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jethro,  Harry
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Harry, HP is now 0
Harry is killed
100 XP added to character Fred, for a total of 200 XP
High cover downgraded to low cover at <2, 0, 3>
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndKillsJethroAndHarryIsSavedByArmorTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jethro
            //  "H" = enemy/harry
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  · · · · .
            //  · E ■ H · 
            //  · · · · · 
            //  · · · · .
            //  · · P · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = MapObjectType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 0));
            Character harry = CharacterPool.CreateHarryHero(map, new(3, 0, 3));
            harry.HitpointsCurrent = 4;
            harry.ArmorPointsCurrent = 1;
            Team team1 = new(1);
            team1.Characters.Add(fred);
            team1.Characters.Add(harry);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 4;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 0, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro, harry };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(7, result.DamageDealt);
            Assert.AreEqual(true, result.IsHit);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(0, jethro.HitpointsCurrent);
            Assert.AreEqual(1, harry.HitpointsCurrent);
            Assert.AreEqual(110, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jethro,  Harry
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
Armor prevented 1 damage to character Harry
3 damage dealt to character Harry, HP is now 1
10 XP added to character Fred, for a total of 110 XP
High cover downgraded to low cover at <2, 0, 3>
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndInjuriesJethroAndKillsHarryTest()
        {
            //Arrange
            //  "P" = player/fred
            //  "E" = enemy/jethro
            //  "H" = enemy/harry
            //  CoverType.FullCover = cover
            //  "." = open ground
            //  · · · · .
            //  · E ■ H · 
            //  · · · · · 
            //  · · · · .
            //  · · P · .
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = MapObjectType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 0));
            Character harry = CharacterPool.CreateHarryHero(map, new(3, 0, 3));
            harry.HitpointsCurrent = 4;
            harry.ArmorPointsCurrent = 0;
            Team team1 = new(1);
            team1.Characters.Add(fred);
            team1.Characters.Add(harry);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 15;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 0, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro, harry };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(8, result.DamageDealt);
            Assert.AreEqual(true, result.IsHit);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(4, fred.HitpointsCurrent);
            Assert.AreEqual(0, harry.HitpointsCurrent);
            Assert.AreEqual(110, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jethro,  Harry
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jethro, HP is now 11
10 XP added to character Fred, for a total of 10 XP
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Harry, HP is now 0
Harry is killed
100 XP added to character Fred, for a total of 110 XP
High cover downgraded to low cover at <2, 0, 3>
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FredThrowsGrenadeWithCriticalAbilityAndKillsJethroTest()
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
            fred.Abilities.Add(AbilityPool.BiggestBoomsAbility1());
            fred.Abilities.Add(AbilityPool.BiggestBoomsAbility2());
            Character harry = CharacterPool.CreateHarryHero(map, new(3, 0, 3));
            harry.HitpointsCurrent = 4;
            harry.ArmorPointsCurrent = 0;
            Team team1 = new(1);
            team1.Characters.Add(fred);
            team1.Characters.Add(harry);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 6;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 80, 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro, harry };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result != null);
            Assert.IsTrue(result.AllCharacters != null);
            Assert.AreEqual(10, result.DamageDealt);
            Assert.AreEqual(true, result.IsHit);
            Assert.AreEqual(true, result.IsCriticalHit);
            Assert.AreEqual(0, jethro.HitpointsCurrent);
            Assert.AreEqual(0, harry.HitpointsCurrent);
            Assert.IsTrue(result.TargetCharacter != null);
            Assert.IsTrue(result.AllCharacters.Count > 0);
            Assert.AreEqual(200, result.SourceCharacter.XP);
            Assert.AreEqual(true, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jethro,  Harry
Damage range: 3-4, (dice roll: 100)
Critical chance: 20, (dice roll: 80)
Critical damage range: 5-6, (dice roll: 100)
6 damage dealt to character Jethro, HP is now 0
Jethro is killed
100 XP added to character Fred, for a total of 100 XP
Damage range: 3-4, (dice roll: 100)
Critical chance: 20, (dice roll: 0)
4 damage dealt to character Harry, HP is now 0
Harry is killed
100 XP added to character Fred, for a total of 200 XP
High cover downgraded to low cover at <2, 0, 3>
Fred is ready to level up
";
            Assert.AreEqual(log, result.LogString);
        }

        [TestMethod]
        public void FOVFredThrowsGrenadeAndDestoriesCoverUpdatingFOVTest()
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
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 0), 5);
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(2, 0, 4));
            jethro.HitpointsCurrent = 5;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act 1: get the FOV
            //List<Vector3> fov = FieldOfView.GetFieldOfView(map, fred.Location, fred.FOVRange);
            string fredFOVString = MapCore.GetMapStringWithMapMask(map, fred.FOVMap);
            string expectedString = @"
▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ 
▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ 
▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ 
▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ ▓ 
· · ▓ · · · ▓ ▓ ▓ ▓ 
· · ▓ · · · · ▓ ▓ ▓ 
· · ■ · · · · · ▓ ▓ 
· · · · · · · · ▓ ▓ 
· · · · · · · · ▓ ▓ 
· · P · · · · · ▓ ▓ 
";
            Assert.AreEqual(expectedString, fredFOVString);

            //Act 2: Now destroy the cover
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);
            List<Vector3> fov = FieldOfView.GetFieldOfView(map, fred.Location, 10);

            //Assert 2: Check the FOV now
            Assert.AreEqual(98, fov.Count);
            bool foundItem = false;
            foreach (Vector3 item in fov)
            {
                if (item.X == 2 && item.Z == 3)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem == true);
            foundItem = false;
            foreach (Vector3 item in fov)
            {
                if (item.X == 2 && item.Z == 4)
                {
                    foundItem = true;
                }
            }
            Assert.IsTrue(foundItem == true);

            //Assert 3: Check the area attack result 
            Assert.IsTrue(result != null);
            Assert.AreEqual(4, result.DamageDealt);
            Assert.AreEqual(true, result.IsHit);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(1, jethro.HitpointsCurrent);
            Assert.AreEqual(10, result.SourceCharacter.XP);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <2, 0, 4>
Characters in affected area: Jethro
Damage range: 3-4, (dice roll: 100)
Critical chance: 0, (dice roll: 0)
4 damage dealt to character Jethro, HP is now 1
10 XP added to character Fred, for a total of 10 XP
High cover downgraded to low cover at <2, 0, 3>
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
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(8, 0, 8));
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = null;
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result == null);
        }

        //The encounter has no random numbers and returns null
        [TestMethod]
        public void NoItemAvailableToThrowTest()
        {
            //Arrange
            string[,,] map = MapCore.InitializeMap(10, 1, 10);
            map[2, 0, 3] = MapObjectType.FullCover; //Add cover 
            Character fred = CharacterPool.CreateFredHero(map, new(2, 0, 0));
            fred.UtilityWeaponEquipped = null;
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 4;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(2, 0, 4);
            List<Character> characterList = new() { fred, jethro };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        public void FredThrowsGrenadeAndMissesJethroTest()
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
            Team team1 = new(1);
            team1.Characters.Add(fred);
            Character jethro = CharacterPool.CreateJethroBaddie(map, new(1, 0, 3));
            jethro.HitpointsCurrent = 4;
            Team team2 = new(0);
            team2.Characters.Add(jethro);
            RandomNumberQueue diceRolls = new(new List<int> { 100, 0 }); //Chance to hit roll, damage roll, critical chance roll
            Vector3 targetThrowingLocation = new(9, 0, 9);
            List<Character> characterList = new() { fred, jethro };
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Act
            EncounterResult result = Encounter.AttackCharacterWithAreaOfEffect(map, fred, fred.UtilityWeaponEquipped, characterList, diceRolls, targetThrowingLocation);
            team1.UpdateTargets(map, team2.Characters);
            team2.UpdateTargets(map, team1.Characters);

            //Assert
            Assert.IsTrue(result != null);
            Assert.AreEqual(0, result.DamageDealt);
            Assert.AreEqual(false, result.IsHit);
            Assert.AreEqual(false, result.IsCriticalHit);
            Assert.AreEqual(4, jethro.HitpointsCurrent);
            Assert.AreEqual(00, result.SourceCharacter.XP);
            Assert.AreEqual(false, result.SourceCharacter.LevelUpIsReady);
            string log = @"
Fred is attacking with area effect Grenade aimed at <9, 0, 9>
No characters in affected area
";
            Assert.AreEqual(log, result.LogString);
        }

    }
}