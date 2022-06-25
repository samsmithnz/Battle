using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Characters;
using Battle.Logic.Items;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Battle.Logic.Encounters
{
    public static class Encounter
    {
        /// <summary>
        /// An area attack (e.g. grenade) aqainst a character
        /// </summary>
        /// <param name="sourceCharacter">Source of the attack</param>
        /// <param name="weapon">The weapon used</param>
        /// <param name="allCharacters">A list of all characters on the map</param>
        /// <param name="map">Current map</param>
        /// <param name="diceRolls">Random number dice rolls (passed for repeatability)</param>
        /// <param name="throwingTargetLocation">Location of area effect/grenade</param>
        /// <returns>EncounterResult: details of how the event resolved</returns>
        public static EncounterResult AttackCharacterWithAreaOfEffect(string[,,] map, Character sourceCharacter, Weapon weapon, List<Character> allCharacters, RandomNumberQueue diceRolls, Vector3 throwingTargetLocation)
        {
            int damageDealt;
            bool isHit = false;
            bool isCriticalHit = false;
            List<KeyValuePair<Vector3, int>> affectedMap = new List<KeyValuePair<Vector3, int>>();
            List<string> log = new List<string>();

            if (diceRolls == null || diceRolls.Count == 0 || weapon == null || weapon.AmmoCurrent <= 0)
            {
                return null;
            }
            log.Add(sourceCharacter.Name + " is attacking with area effect " + weapon.Name + " aimed at " + throwingTargetLocation.ToString());

            //Get the targets in the area affected
            List<Character> areaEffectTargets = FieldOfView.GetCharactersInArea(map, allCharacters, throwingTargetLocation, weapon.AreaEffectRadius);
            StringBuilder names = new StringBuilder();
            foreach (Character item in areaEffectTargets)
            {
                names.Append(' ');
                names.Append(item.Name);
                names.Append(", ");
            }
            if (names.Length > 1)
            {
                log.Add("Characters in affected area: " + names.ToString().Substring(1, names.ToString().Length - 3));//remove the first " " and last two characters: ", "
            }
            else
            {
                log.Add("No characters in affected area");
            }

            //Deal damage to each target
            int totalDamageDealt = 0;
            int totalArmorAbsorbed = 0;
            int totalArmorShredded = 0;
            foreach (Character targetCharacter in areaEffectTargets)
            {
                EncounterResult tempResult = ProcessCharacterDamageAndExperience(map, sourceCharacter, weapon, targetCharacter, diceRolls, log, true);
                sourceCharacter = tempResult.SourceCharacter;
                damageDealt = tempResult.DamageDealt;
                totalDamageDealt += damageDealt;
                totalArmorAbsorbed += tempResult.ArmorAbsorbed;
                totalArmorShredded += tempResult.ArmorShredded;
                isHit = true;
                if (!isCriticalHit)
                {
                    isCriticalHit = tempResult.IsCriticalHit;
                }
                log = tempResult.Log;
            }

            //Remove cover
            List<Vector3> area = MapCore.GetMapArea(map, throwingTargetLocation, weapon.AreaEffectRadius, false, true);
            foreach (Vector3 item in area)
            {
                switch (map[(int)item.X, (int)item.Y, (int)item.Z])
                {
                    //Full cover becomes low cover
                    case MapObjectType.FullCover:
                        map[(int)item.X, (int)item.Y, (int)item.Z] = MapObjectType.HalfCover;
                        affectedMap.Add(new KeyValuePair<Vector3, int>(item, 1));
                        log.Add("High cover downgraded to low cover at " + item.ToString());
                        break;
                    //Low cover becomes no cover
                    case MapObjectType.HalfCover:
                        map[(int)item.X, (int)item.Y, (int)item.Z] = MapObjectType.NoCover;
                        affectedMap.Add(new KeyValuePair<Vector3, int>(item, 1));
                        log.Add("Low cover downgraded to no cover at " + item.ToString());
                        break;
                }
            }

            //Consume source characters action points
            sourceCharacter.ActionPointsCurrent = 0;

            if (sourceCharacter.LevelUpIsReady)
            {
                log.Add(sourceCharacter.Name + " is ready to level up");
            }
            EncounterResult result = new EncounterResult()
            {
                SourceCharacter = sourceCharacter,
                AllCharacters = allCharacters,
                ArmorAbsorbed = totalArmorAbsorbed,
                ArmorShredded = totalArmorShredded,
                DamageDealt = totalDamageDealt,
                IsCriticalHit = isCriticalHit,
                AffectedMap = affectedMap,
                IsHit = isHit,
                Log = log
            };
            return result;
        }

        /// <summary>
        /// A regular attack (e.g. shooting or hitting) against a character
        /// </summary>
        /// <param name="sourceCharacter">Source of the attack</param>
        /// <param name="weapon">The weapon used</param>
        /// <param name="targetCharacter">Target of the attack</param>
        /// <param name="map">Current map</param>
        /// <param name="diceRolls">Random number dice rolls (passed for repeatability)</param>
        /// <returns>EncounterResult: details of how the event resolved</returns>
        public static EncounterResult AttackCharacter(string[,,] map, Character sourceCharacter, Weapon weapon, Character targetCharacter, RandomNumberQueue diceRolls)
        {
            if (diceRolls == null || diceRolls.Count == 0)
            {
                return null;
            }

            int armorAbsorbed = 0;
            int armorShredded = 0;
            int damageDealt = 0;
            bool isHit = false;
            bool isCriticalHit = false;
            Vector3 missedLocation = Vector3.Zero;
            List<KeyValuePair<Vector3, int>> affectedMap = new List<KeyValuePair<Vector3, int>>();
            List<string> log = new List<string>
            {
                sourceCharacter.Name + " is attacking with " + weapon.Name + ", targeted on " + targetCharacter.Name.ToString()
            };

            //Don't attack if the clip is empty
            if (weapon.AmmoCurrent > 0)
            {
                sourceCharacter.TotalShots++;
                int toHitPercent = EncounterCore.GetChanceToHit(sourceCharacter, weapon, targetCharacter);

                //If the number rolled is higher than the chance to hit, the attack was successful!
                int randomToHit = diceRolls.Dequeue();
                if ((100 - toHitPercent) <= randomToHit)
                {
                    log.Add("Hit: Chance to hit: " + toHitPercent.ToString() + ", (dice roll: " + randomToHit.ToString() + ")");

                    EncounterResult tempResult = ProcessCharacterDamageAndExperience(map, sourceCharacter, weapon, targetCharacter, diceRolls, log, false);
                    sourceCharacter = tempResult.SourceCharacter;
                    targetCharacter = tempResult.TargetCharacter;
                    armorAbsorbed = tempResult.ArmorAbsorbed;
                    armorShredded = tempResult.ArmorShredded;
                    damageDealt = tempResult.DamageDealt;
                    isCriticalHit = tempResult.IsCriticalHit;
                    isHit = true;
                    log = tempResult.Log;
                    if (targetCharacter.HitpointsCurrent <= 0)
                    {
                        affectedMap.Add(new KeyValuePair<Vector3, int>(targetCharacter.Location, 1));
                    }
                }
                else
                {
                    log.Add("Missed: Chance to hit: " + toHitPercent.ToString() + ", (dice roll: " + randomToHit.ToString() + ")");

                    //How badly did we miss?
                    int missedByPercent = (100 - toHitPercent) - randomToHit;

                    //Work out where the shot goes
                    //get the percentage miss
                    //Randomize x,y,z coordinates.
                    //Aim and shoot at that new target and see if we hit anything
                    //do this by doubling the lines. 
                    missedLocation = FieldOfView.MissedShot(map, sourceCharacter.Location, targetCharacter.Location, missedByPercent);

                    //Remove cover at this location
                    if (map[(int)missedLocation.X, (int)missedLocation.Y, (int)missedLocation.Z] != "")
                    {
                        switch (map[(int)missedLocation.X, (int)missedLocation.Y, (int)missedLocation.Z])
                        {
                            //Full cover becomes low cover
                            case MapObjectType.FullCover:
                                map[(int)missedLocation.X, (int)missedLocation.Y, (int)missedLocation.Z] = MapObjectType.HalfCover;
                                affectedMap.Add(new KeyValuePair<Vector3, int>(missedLocation, 1));
                                log.Add("High cover downgraded to low cover at " + missedLocation.ToString());
                                break;
                            //Low cover becomes no cover
                            case MapObjectType.HalfCover:
                                map[(int)missedLocation.X, (int)missedLocation.Y, (int)missedLocation.Z] = MapObjectType.NoCover;
                                affectedMap.Add(new KeyValuePair<Vector3, int>(missedLocation, 1));
                                log.Add("Low cover downgraded to no cover at " + missedLocation.ToString());
                                break;
                        }
                        //map[(int)missedLocation.X, (int)missedLocation.Y, (int)missedLocation.Z] = "";
                    }

                    int xp = Experience.GetExperience(false);
                    sourceCharacter.XP += xp;
                    log.Add(xp.ToString() + " XP added to character " + sourceCharacter.Name + ", for a total of " + sourceCharacter.XP + " XP");
                }

                //Consume source characters action points
                sourceCharacter.ActionPointsCurrent = 0;
                //Consume weapon ammo
                weapon.AmmoCurrent--;

                //Check if the character has enough experience to level up
                if (sourceCharacter.LevelUpIsReady)
                {
                    log.Add(sourceCharacter.Name + " is ready to level up");
                }
            }
            else
            {
                log.Add(weapon.Name + " has no ammo remaining and the attack cannot be completed");
            }

            EncounterResult result = new EncounterResult()
            {
                SourceCharacter = sourceCharacter,
                TargetCharacter = targetCharacter,
                ArmorAbsorbed = armorAbsorbed,
                ArmorShredded = armorShredded,
                DamageDealt = damageDealt,
                IsCriticalHit = isCriticalHit,
                IsHit = isHit,
                MissedLocation = missedLocation,
                AffectedMap = affectedMap,
                Log = log
            };
            return result;
        }

        private static EncounterResult ProcessCharacterDamageAndExperience(string[,,] map, Character sourceCharacter, Weapon weapon, Character targetCharacter, RandomNumberQueue diceRolls, List<string> log, bool isAreaEffectAttack)
        {
            //Get damage 
            int damageRollPercent = diceRolls.Dequeue();
            DamageRange damageOptions = EncounterCore.GetDamageRange(sourceCharacter, weapon);
            int lowDamage = damageOptions.DamageLow;
            int highDamage = damageOptions.DamageHigh;
            log.Add("Damage range: " + lowDamage.ToString() + "-" + highDamage.ToString() + ", (dice roll: " + damageRollPercent + ")");

            //Check if it was a critical hit
            bool isCriticalHit = false;
            // player can't be critically hit if hunkered down
            if (targetCharacter.HunkeredDown)
            {
                log.Add("Critical chance: 0, hunkered down");
            }
            else
            {
                int randomToCrit = diceRolls.Dequeue();
                int chanceToCrit = EncounterCore.GetChanceToCrit(map, sourceCharacter, weapon, targetCharacter, isAreaEffectAttack);
                if ((100 - chanceToCrit) <= randomToCrit)
                {
                    isCriticalHit = true;
                    lowDamage = damageOptions.CriticalDamageLow;
                    highDamage = damageOptions.CriticalDamageHigh;
                }
                log.Add("Critical chance: " + chanceToCrit.ToString() + ", (dice roll: " + randomToCrit.ToString() + ")");
                if (isCriticalHit)
                {
                    log.Add("Critical damage range: " + lowDamage.ToString() + "-" + highDamage.ToString() + ", (dice roll: " + damageRollPercent + ")");
                }
            }

            //process damage
            int damageDealt = RandomNumber.ScaleRandomNumber(
                        lowDamage, highDamage,
                        damageRollPercent);

            //Process armor shredding. This happens in addition to piercing, and therefore happens first
            int armorShredded = EncounterCore.AggregateAbilitiesByType(sourceCharacter.Abilities, AbilityType.ArmorShredding);
            targetCharacter.ArmorPointsCurrent -= armorShredded;

            //Now process any other damage with armor
            int armorAbsorbed = 0;
            int armorPiercing = EncounterCore.AggregateAbilitiesByType(sourceCharacter.Abilities, AbilityType.ArmorPiercing);
            if (armorPiercing > 0)
            {
                log.Add("Armor was ignored due to 'armor piercing' ability");
            }
            else
            {
                int damageAfterArmor = damageDealt - targetCharacter.ArmorPointsCurrent;
                //If the armor points are higher than the damage, we have -ve damage, we don't want to heal characters, set to 
                if (damageAfterArmor < 0)
                {
                    damageAfterArmor = 0;
                }
                armorAbsorbed = damageDealt - damageAfterArmor;
                damageDealt = damageAfterArmor;
            }

            //Apply any damage
            targetCharacter.HitpointsCurrent -= damageDealt;
            sourceCharacter.TotalDamage += damageDealt;

            //Update log
            if (armorShredded > 0)
            {
                log.Add(armorShredded.ToString() + " armor points shredded");
            }
            if (armorAbsorbed > 0)
            {
                log.Add("Armor prevented " + armorAbsorbed.ToString() + " damage to character " + targetCharacter.Name);
            }
            log.Add(damageDealt.ToString() + " damage dealt to character " + targetCharacter.Name + ", HP is now " + targetCharacter.HitpointsCurrent.ToString());

            //process experience
            int xp;
            if (targetCharacter.HitpointsCurrent <= 0)
            {
                log.Add(targetCharacter.Name + " is killed");
                xp = Experience.GetExperience(true, true);
                sourceCharacter.TotalKills++;
                //Update map to remove character - this square is now clear and can be tranversed/used
                map[(int)targetCharacter.Location.X, (int)targetCharacter.Location.Y, (int)targetCharacter.Location.Z] = "";
            }
            else
            {
                xp = Experience.GetExperience(true);
            }
            sourceCharacter.XP += xp;
            log.Add(xp.ToString() + " XP added to character " + sourceCharacter.Name + ", for a total of " + sourceCharacter.XP + " XP");
            sourceCharacter.TotalHits++;

            EncounterResult result = new EncounterResult()
            {
                SourceCharacter = sourceCharacter,
                TargetCharacter = targetCharacter,
                ArmorAbsorbed = armorAbsorbed,
                ArmorShredded = armorShredded,
                DamageDealt = damageDealt,
                IsCriticalHit = isCriticalHit,
                Log = log
            };
            return result;
        }
    }
}
