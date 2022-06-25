using Battle.Logic.Characters;
using Battle.Logic.Encounters;
using Battle.Logic.Items;
using Battle.Logic.Map;
using Battle.Logic.Utility;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Game
{
    public class Mission
    {
        public Mission()
        {
            Teams = new List<Team>();
            Objectives = new List<MissionObjective>() { new MissionObjective(MissionObjectiveType.EliminateAllOpponents, false) };
            TurnNumber = 1;
            CurrentTeamIndex = 0;
            RandomNumbers = new RandomNumberQueue(RandomNumber.GenerateRandomNumberList(0, 100, 0, 1000));
            RandomNumbers.Queue[0] = 8; //First shot always misses in this scenario.
        }

        public int TurnNumber { get; set; }
        public int CurrentTeamIndex { get; set; }
        public List<Team> Teams { get; set; }
        public string[,,] Map { get; set; }
        public RandomNumberQueue RandomNumbers { get; set; }

        public List<MissionObjective> Objectives { get; set; }

        public bool CheckIfCurrentTeamIsDoneTurn()
        {
            int totalActionPoints = 0;
            foreach (Character character in Teams[CurrentTeamIndex].Characters)
            {
                if (character.InOverwatch == false)
                {
                    totalActionPoints += character.ActionPointsCurrent;
                }
            }
            if (totalActionPoints == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MoveToNextTurn()
        {
            //Move to the next team
            if (CurrentTeamIndex == Teams.Count - 1)
            {
                CurrentTeamIndex = 0;
                //Move to the next turn
                TurnNumber++;
            }
            else
            {
                CurrentTeamIndex++;
            }

            //Reset the characters action points for the new turn
            foreach (Character character in Teams[CurrentTeamIndex].Characters)
            {
                if (character.HitpointsCurrent > 0)
                {
                    character.ActionPointsCurrent = character.ActionPointsMax;
                }
            }
            //Refresh targets
            Teams[0].UpdateTargets(Map, Teams[1].Characters);
            Teams[1].UpdateTargets(Map, Teams[0].Characters);
        }

        public bool CheckIfMissionIsCompleted()
        {
            bool result = false;
            int objectiveCount = Objectives.Count;

            for (int i = 0; i < Objectives.Count; i++)
            {
                MissionObjective item = Objectives[i];
                if (item.Type == MissionObjectiveType.EliminateAllOpponents)
                {
                    foreach (Team team in Teams)
                    {
                        int totalHPs = 0;
                        foreach (Character character in team.Characters)
                        {
                            if (character.HitpointsCurrent > 0)
                            {
                                totalHPs += character.HitpointsCurrent;
                            }
                        }
                        if (totalHPs <= 0)
                        {
                            Objectives[i].ObjectiveIsComplete = true;
                            objectiveCount--;
                        }
                    }
                }
                if (item.Type == MissionObjectiveType.ToggleSwitch)
                {
                    if (Objectives[i].ObjectiveIsComplete == true)
                    {
                        objectiveCount--;
                    }
                }
                if (item.Type == MissionObjectiveType.ExtractTroops)
                {
                    foreach (Team team in Teams)
                    {
                        int characterCount = team.Characters.Count;
                        foreach (Character character in team.Characters)
                        {
                            if (character.HitpointsCurrent <= 0 || character.ExtractedFromMission)
                            {
                                characterCount--;
                            }
                        }
                        if (characterCount <= 0)
                        {
                            Objectives[i].ObjectiveIsComplete = true;
                            objectiveCount--;
                        }
                    }
                }
            }
            if (objectiveCount == 0)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Start the mission, ensuring all data objects are configured correctly
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void StartMission()
        {
            if (Teams.Count != 2)
            {
                throw new Exception("Unexpected number of teams: " + Teams.Count.ToString());
            }
            else
            {
                Teams[0].UpdateTargets(Map, Teams[1].Characters);
                Teams[1].UpdateTargets(Map, Teams[0].Characters);
            }
            //Setup any objectives on the map
            foreach (MissionObjective objective in Objectives)
            {
                if (objective.Type == MissionObjectiveType.ToggleSwitch)
                {
                    Map[(int)objective.Location.X, (int)objective.Location.Y, (int)objective.Location.Z] = MapObjectType.ToggleSwitchOn;
                }
            }
        }

        /// <summary>
        /// End mission, add records
        /// </summary>
        public void EndMission()
        {
            foreach (Team team in Teams)
            {
                foreach (Character character in team.Characters)
                {
                    //If the character is still alive
                    if (character.HitpointsCurrent > 0)
                    {
                        //incrememt the missions completed metric
                        character.MissionsCompleted++;
                        //Add some experience for surviving
                        character.XP += 100;
                    }
                }
            }
        }

        public List<KeyValuePair<Vector3, int>> GetMovementPossibleTiles(Character sourceCharacter)
        {
            List<KeyValuePair<Vector3, int>> movementPossibileTiles = MovementPossibileTiles.GetMovementPossibileTiles(Map,
                sourceCharacter.Location,
                sourceCharacter.MobilityRange,
                sourceCharacter.ActionPointsCurrent);
            return movementPossibileTiles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceCharacter"></param>
        /// <param name="sourceTeam"></param>
        /// <param name="opponentTeam"></param>
        /// <param name="startLocation"></param>
        /// <param name="endLocation"></param>
        /// <returns></returns>
        public List<MovementAction> MoveCharacter(Character sourceCharacter,
            Team sourceTeam,
            Team opponentTeam,
            Vector3 endLocation)
        {
            PathFindingResult pathFindingResult = PathFinding.FindPath(Map,
                sourceCharacter.Location,
                endLocation);
            List<MovementAction> movementResults = CharacterMovement.MoveCharacter(Map,
                 sourceCharacter,
                 pathFindingResult,
                 sourceTeam,
                 opponentTeam,
                 RandomNumbers);
            sourceTeam.UpdateTargets(Map, opponentTeam.Characters);
            opponentTeam.UpdateTargets(Map, sourceTeam.Characters);

            return movementResults;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceCharacter"></param>
        /// <param name="equippedWeapon"></param>
        /// <param name="targetedCharacter"></param>
        /// <param name="sourceTeam"></param>
        /// <param name="opponentTeam"></param>
        /// <returns></returns>
        public EncounterResult AttackCharacter(Character sourceCharacter,
            Weapon equippedWeapon,
            Character targetedCharacter,
            Team sourceTeam,
            Team opponentTeam)
        {
            EncounterResult encounterResult = Encounter.AttackCharacter(Map,
                sourceCharacter,
                equippedWeapon,
                targetedCharacter,
                RandomNumbers);

            sourceTeam.UpdateTargets(Map, opponentTeam.Characters);
            opponentTeam.UpdateTargets(Map, sourceTeam.Characters);

            return encounterResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceCharacter"></param>
        /// <param name="equippedWeapon"></param>
        /// <param name="sourceTeam"></param>
        /// <param name="opponentTeam"></param>
        /// <param name="targetThrowingLocation"></param>
        /// <returns></returns>
        public EncounterResult AttackCharacterWithAreaOfEffect(Character sourceCharacter,
            Weapon equippedWeapon,
            Team sourceTeam,
            Team opponentTeam,
            Vector3 targetThrowingLocation)
        {
            //Build a list of all characters first
            List<Character> allCharacters = new List<Character>();
            foreach (Team team in Teams)
            {
                allCharacters.AddRange(team.Characters);
            }

            EncounterResult encounterResult = Encounter.AttackCharacterWithAreaOfEffect(Map,
                sourceCharacter, equippedWeapon,
                allCharacters,
                RandomNumbers,
                targetThrowingLocation);

            sourceTeam.UpdateTargets(Map, opponentTeam.Characters);
            opponentTeam.UpdateTargets(Map, sourceTeam.Characters);

            return encounterResult;
        }

        public AIAction CalculateAIAction(Character sourceCharacter, Team sourceTeam, Team opponentTeam)
        {
            CharacterAI ai = new CharacterAI();
            AIAction action = ai.CalculateAIAction(Map,
                sourceCharacter,
                sourceTeam,
                opponentTeam,
                RandomNumbers);
            return action;
        }

        public bool ToggleSwitch(Character sourceCharacter)
        {
            //Check that the source character is next to a tile that toggles a switch
            List<Vector3> foundTiles = MapCore.FindAdjacentTile(Map, sourceCharacter.Location, MapObjectType.ToggleSwitchOn);
            if (foundTiles != null && foundTiles.Count > 0)
            {
                //Update the map with an off toggle switch
                Map[(int)foundTiles[0].X, (int)foundTiles[0].Y, (int)foundTiles[0].Z] = MapObjectType.ToggleSwitchOff;

                //Mark the objective as complete
                for (int i = 0; i < Objectives.Count; i++)
                {
                    MissionObjective objective = Objectives[i];
                    if (objective.Type == MissionObjectiveType.ToggleSwitch)
                    {
                        sourceCharacter.ActionPointsCurrent -= 1;
                        Objectives[i].ObjectiveIsComplete = true;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
