﻿using Battle.Logic.Characters;
using Battle.Logic.FieldOfView;
using Battle.Logic.Utility;
using Battle.Logic.Weapons;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Encounters
{
    public class Encounter
    {
        public static EncounterResult AttackCharacterWithAreaOfEffect(Character sourceCharacter, Weapon weapon, List<Character> allCharacters, string[,] map, Queue<int> diceRolls, Vector3 throwingTargetLocation)
        {
            int damageDealt;
            bool isCriticalHit = false;
            List<string> log = new();
            log.Add(sourceCharacter.Name + " is attacking with area effect " + weapon.Name + " aimed at " + throwingTargetLocation.ToString());

            if (diceRolls == null || diceRolls.Count == 0)
            { 
                return null;
            }
            //throws always hit, process this before tohit        
            //int toHitPercent = GetChanceToHit(sourceCharacter, weapon, targetCharacter);

            //If the number rolled is higher than the chance to hit, the attack was successful!
            //int randomToHit = diceRolls.Dequeue();

            //Get the targets in the area affected
            List<Character> areaEffectTargets = AreaEffectFieldOfView.GetCharactersInArea(allCharacters, map, throwingTargetLocation, weapon.AreaEffectRadius);
            string names = "";
            foreach (Character item in areaEffectTargets)
            {
                names += " " + item.Name + ", ";
            }
            names = names[1..^2]; //remove the first " " and last two characters: ", "
            log.Add("Characters in affected area: " + names);

            //Get damage 
            int damageRollPercent = diceRolls.Dequeue();
            DamageOptions damageOptions = EncounterCore.GetDamageRange(sourceCharacter, weapon);
            int lowDamage = damageOptions.DamageLow;
            int highDamage = damageOptions.DamageHigh;
            log.Add("Damage range: " + lowDamage.ToString() + "-" + highDamage.ToString() + ", (dice roll: " + damageRollPercent + ")");

            //Check if it was a critical hit
            int randomToCrit = diceRolls.Dequeue();
            int chanceToCrit = EncounterCore.GetChanceToCrit(sourceCharacter, weapon, null, map);
            if ((100 - chanceToCrit) <= randomToCrit)
            {
                isCriticalHit = true;
                lowDamage = damageOptions.CriticalDamageLow;
                highDamage = damageOptions.CriticalDamageHigh;
            }
            log.Add("Critical chance: " + chanceToCrit.ToString() + ", (dice roll: " + randomToCrit.ToString() + ")");
            if (isCriticalHit == true)
            {
                log.Add("Critical damage range: " + lowDamage.ToString() + "-" + highDamage.ToString() + ", (dice roll: " + damageRollPercent + ")");
            }

            //process damage
            damageDealt = RandomNumber.ScaleRandomNumber(
                    lowDamage, highDamage,
                    damageRollPercent);

            //Deal damage to each target
            foreach (Character character in areaEffectTargets)
            {
                //Deal the damage
                character.HP -= damageDealt;
                log.Add(damageDealt.ToString() + " damage dealt to character " + character.Name + ", HP is now: " + character.HP.ToString());

                //process experience
                int xp;
                if (character.HP <= 0)
                {
                    //targetCharacter.HP = 0;
                    log.Add(character.Name + " is killed");
                    xp = Experience.GetExperience(true, true);
                }
                else
                {
                    xp = Experience.GetExperience(true);
                }
                sourceCharacter.Experience += xp;
                log.Add(xp.ToString() + " XP added to character " + sourceCharacter.Name + ", for a total of " + sourceCharacter.Experience + " XP");
            }

            //Remove cover
            List<Vector3> area = AreaEffectFieldOfView.GetAreaOfEffect(map, throwingTargetLocation, weapon.AreaEffectRadius);
            foreach (Vector3 item in area)
            {
                if (map[(int)item.X, (int)item.Z] == "W")
                {
                    map[(int)item.X, (int)item.Z] = "";
                    log.Add("Cover removed from " + item.ToString());
                }
            }

            //Consume source characters action points
            sourceCharacter.ActionPoints = 0;

            //Check if the character has enough experience to level up
            sourceCharacter.LevelUpIsReady = Experience.CheckIfReadyToLevelUp(sourceCharacter.Level, sourceCharacter.Experience);

            EncounterResult result = new()
            {
                SourceCharacter = sourceCharacter,
                AllCharacters = allCharacters,
                DamageDealt = damageDealt,
                IsCriticalHit = isCriticalHit,
                Log = log
            };
            return result;
        }

        public static EncounterResult AttackCharacter(Character sourceCharacter, Weapon weapon, Character targetCharacter, string[,] map, Queue<int> diceRolls)
        {
            int damageDealt = 0;
            bool isCriticalHit = false;
            List<string> log = new();
            log.Add(sourceCharacter.Name + " is attacking with " + weapon.Name + ", targeted on " + targetCharacter.Name.ToString());

            if (diceRolls == null || diceRolls.Count == 0)
            {
                return null;
            }
            int toHitPercent = EncounterCore.GetChanceToHit(sourceCharacter, weapon, targetCharacter);

            //If the number rolled is higher than the chance to hit, the attack was successful!
            int randomToHit = diceRolls.Dequeue();

            if ((100 - toHitPercent) <= randomToHit)
            {
                log.Add("Hit: Chance to hit: " + (100 - toHitPercent).ToString() + ", (dice roll: " + randomToHit.ToString() + ")");
               
                //Get damage 
                int damageRollPercent = diceRolls.Dequeue();
                DamageOptions damageOptions = EncounterCore.GetDamageRange(sourceCharacter, weapon);
                int lowDamage = damageOptions.DamageLow;
                int highDamage = damageOptions.DamageHigh;
                log.Add("Damage range: " + lowDamage.ToString() + "-" + highDamage.ToString() + ", (dice roll: " + damageRollPercent + ")");

                //Check if it was a critical hit
                int randomToCrit = diceRolls.Dequeue();
                int chanceToCrit = EncounterCore.GetChanceToCrit(sourceCharacter, weapon, targetCharacter, map);
                if ((100 - chanceToCrit) <= randomToCrit)
                {
                    isCriticalHit = true;
                    lowDamage = damageOptions.CriticalDamageLow;
                    highDamage = damageOptions.CriticalDamageHigh;
                }
                log.Add("Critical chance: " + chanceToCrit.ToString() + ", (dice roll: " + randomToCrit.ToString() + ")");
                if (isCriticalHit == true)
                {
                    log.Add("Critical damage range: " + lowDamage.ToString() + "-" + highDamage.ToString() + ", (dice roll: " + damageRollPercent + ")");
                }

                //process damage
                damageDealt = RandomNumber.ScaleRandomNumber(
                        lowDamage, highDamage,
                        damageRollPercent);

                //If the damage dealt is more than the health, set damage to be equal to health
                //if (targetCharacter.HP <= damageDealt)
                //{
                //    damageDealt = targetCharacter.HP;
                //}
                targetCharacter.HP -= damageDealt;
                log.Add(damageDealt.ToString() + " damage dealt to character " + targetCharacter.Name + ", character HP is now " + targetCharacter.HP.ToString());

                //process experience
                int xp;
                if (targetCharacter.HP <= 0)
                {
                    //targetCharacter.HP = 0;
                    log.Add(targetCharacter.Name + " is killed");
                    xp = Experience.GetExperience(true, true);
                }
                else
                {
                    xp = Experience.GetExperience(true);
                }
                sourceCharacter.Experience += xp;
                log.Add(xp.ToString() + " XP added to character " + sourceCharacter.Name + ", for a total of " + sourceCharacter.Experience + " XP");

                //Consume source characters action points
                sourceCharacter.ActionPoints = 0;
            }
            else
            {
                log.Add("Missed: Chance to hit: " + (100 - toHitPercent).ToString() + ", (dice roll: " + randomToHit.ToString() + ")");
  
                int xp = Experience.GetExperience(false);
                sourceCharacter.Experience += xp;
                log.Add(xp.ToString() + " XP added to character " + sourceCharacter.Name + ", for a total of " + sourceCharacter.Experience + " XP");
            }

            //Check if the character has enough experience to level up
            sourceCharacter.LevelUpIsReady = Experience.CheckIfReadyToLevelUp(sourceCharacter.Level, sourceCharacter.Experience);

            EncounterResult result = new()
            {
                SourceCharacter = sourceCharacter,
                TargetCharacter = targetCharacter,
                DamageDealt = damageDealt,
                IsCriticalHit = isCriticalHit,
                Log = log
            };
            return result;
        }

    }
}
