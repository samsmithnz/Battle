﻿using Battle.Logic.Encounters;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Characters
{
    public static class CharacterAI
    {
        public static ActionResult CalculateAction(string[,,] map, Character character, RandomNumberQueue diceRolls)
        {
            List<string> log = new List<string>
            {
                character.Name + " is processing AI, with intelligence " + character.Intelligence
            };
            Vector3 startLocation = character.Location;
            Vector3 endLocation = new Vector3(20, 0, 19);

            //1. Get a list of all possible moves
            List<Vector3> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(map, character.Location, character.MobilityRange);
            List<KeyValuePair<Vector3, int>> movementAIValues = new List<KeyValuePair<Vector3, int>>();
            foreach (Vector3 item in movementPossibileTiles)
            {
                movementAIValues.Add(new KeyValuePair<Vector3, int>(item, 0));
            }

            //2. Assign a value to each possible tile
            //TODO
            //3. Sort the values, highest first
            //TODO
            //4. Assign a move based on the intelligence check
            //TODO

            //If the number rolled is higher than the chance to hit, the attack was successful!
            int randomInt = diceRolls.Dequeue();
            if ((100 - character.Intelligence) <= randomInt)
            {
                log.Add("Successful intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                //roll successful
                //TODO
            }
            else
            {
                log.Add("Failed intelligence check: " + character.Intelligence.ToString() + ", (dice roll: " + randomInt.ToString() + ")");
                //roll failed
                //TODO            }

                character.InFullCover = true;
                return new ActionResult()
                {
                    Log = log,
                    StartLocation = startLocation,
                    EndLocation = endLocation
                };
            }
        }
    }