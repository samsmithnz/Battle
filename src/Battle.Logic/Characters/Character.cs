using Battle.Logic.AbilitiesAndEffects;
using Battle.Logic.Items;
using Battle.Logic.Map;
using System.Collections.Generic;
using System.Numerics;

namespace Battle.Logic.Characters
{
    public class Character
    {
        public Character()
        {
            Abilities = new List<Ability>();
            Effects = new List<Effect>();
            FOVHistory = new HashSet<Vector3>();
            CoverState = new CoverState();
            TargetCharacters = new List<string>();
            TargetCharacterIndex = -1;
        }

        public string Name { get; set; }
        public string Background { get; set; }
        public int HitpointsMax { get; set; }
        public int HitpointsCurrent { get; set; }
        public int ArmorPointsMax { get; set; }
        public int ArmorPointsCurrent { get; set; }
        public int ActionPointsMax { get; set; }
        public int ActionPointsCurrent { get; set; }
        public int ChanceToHit { get; set; }
        private int _xp;
        public int XP
        {
            get
            {
                return _xp;
            }
            set
            {
                _xp = value;
                //Check if the character has enough experience to level up
                LevelUpIsReady = Experience.CheckIfReadyToLevelUp(Level, _xp);
            }
        }
        public int Level { get; set; }
        public bool LevelUpIsReady { get; set; }
        public int Speed { get; set; }
        public int Intelligence { get; set; } //measured by percentage
        public List<Ability> Abilities { get; set; }
        public List<Effect> Effects { get; set; }
        private Vector3 _location { get; set; }
        //TODO: Location should never be set here- but I need this for serialization. Need to figure this out later
        public Vector3 Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        //This needs to be a separate function, as we want to include the current map to build the fov/field of view calcualtions
        public void SetLocationAndRange(string[,,] map, Vector3 characterLocation, int fovRange, List<Character> opponentCharacters)
        {
            FOVRange = fovRange;
            Vector3 previousLocation = _location;
            _location = characterLocation; //set the location before we start the fov recalculation
            //Only update the map if the character is still alive
            if (map != null && HitpointsCurrent > 0)
            {
                //Set the previous location on the map to blank (the character is no longer there)
                map[(int)previousLocation.X, (int)previousLocation.Y, (int)previousLocation.Z] = "";
                //Place the player in the new location on the map
                map[(int)characterLocation.X, (int)characterLocation.Y, (int)characterLocation.Z] = "P";
                UpdateCharacterFOV(map);
                //Get targets 
                List<Character> targetCharacters = FieldOfView.GetCharactersInView(map, Location, ShootingRange, opponentCharacters);
                TargetCharacters = new List<string>();
                foreach (Character item in targetCharacters)
                {
                    TargetCharacters.Add(item.Name);
                }
                if (targetCharacters.Count > 0)
                {
                    TargetCharacterIndex = 0;
                }
                //Update cover
                CoverState = CharacterCover.CalculateCover(map, Location, opponentCharacters);
            }
        }
        public int MobilityRange { get; set; }
        public int ShootingRange { get; set; }
        public int FOVRange { get; set; }
        /// <summary>
        /// Shows the character Field of View map, where "░" is not visible, and " " is visible
        /// </summary>
        public string[,,] FOVMap { get; set; }
        public HashSet<Vector3> FOVHistory { get; set; }
        public Weapon WeaponEquipped { get; set; }
        public Weapon UtilityWeaponEquipped { get; set; }
        public Item UtilityItemEquipped { get; set; }
        public CoverState CoverState { get; set; }
        public bool InOverwatch { get; set; }
        public bool HunkeredDown { get; set; }

        //Records & statistics
        public int MissionsCompleted { get; set; }
        //public int DaysWounded { get; set; }
        public int TotalKills { get; set; }
        public int TotalShots { get; set; }
        public int TotalHits { get; set; }
        public int TotalMisses { get { return TotalShots - TotalHits; } }
        public int TotalDamage { get; set; }
        public bool ExtractedFromMission { get; set; }
        public int TargetCharacterIndex { get; set; }
        public List<string> TargetCharacters { get; set; }
        public string GetTargetCharacter()
        {
            if (TargetCharacterIndex >= 0)
            {
                return TargetCharacters[TargetCharacterIndex];
            }
            else
            {
                return null;
            }
        }

        public void NextTarget()
        {
            TargetCharacterIndex++;
            if (TargetCharacterIndex > TargetCharacters.Count - 1)
            {
                TargetCharacterIndex = 0;
            }

            //int targetTeamIndex = 0;
            //if (Mission.CurrentTeamIndex == 0)
            //{
            //    targetTeamIndex = 1;
            //}
            //CurrentTargetCharacter = null;
            //if (CurrentCharacter.CharacterLogic.TargetCharacters.Count == 0)
            //{
            //    CurrentCharacter.CharacterLogic.TargetCharacterIndex = -1;
            //}
            ////Move to the next target character if the first one is not valid
            //else if (Mission.Teams[targetTeamIndex].GetCharacter(CurrentCharacter.CharacterLogic.TargetCharacterIndex).HitpointsCurrent <= 0)
            //{
            //    CurrentCharacter.CharacterLogic.NextTarget();
            //}
        }

        //public void PreviousTarget()
        //{
        //    TargetCharacterIndex--;
        //    if (TargetCharacterIndex < 0)
        //    {
        //        TargetCharacterIndex = TargetCharacters.Count - 1;
        //    }
        //}

        public void ProcessEffects(int currentTurn)
        {
            List<int> itemIndexesToRemove = new List<int>();
            for (int i = 0; i < Effects.Count; i++)
            {
                Effect effect = Effects[i];
                //If the effect is expiring this turn ,remove it
                if (effect.TurnExpiration <= currentTurn)
                {
                    itemIndexesToRemove.Add(i);
                }
                else //the effect is still active, process it
                {
                    switch (effect.Type)
                    {
                        case AbilityType.FireDamage:
                            HitpointsCurrent -= effect.Adjustment;
                            break;
                    }
                }
            }
            for (int i = itemIndexesToRemove.Count - 1; i >= 0; i--)
            {
                Effects.RemoveAt(i);
            }
        }

        public List<CharacterAction> GetCurrentActions(string[,,] map = null)
        {
            List<CharacterAction> options = new List<CharacterAction>();
            if (ActionPointsCurrent > 0)
            {
                if (WeaponEquipped.AmmoCurrent > 0)
                {
                    if (TargetCharacters.Count > 0)
                    {
                        options.Add(new CharacterAction() { Name = "_shoot", Caption = "Shoot", KeyBinding = "1" });
                    }
                    options.Add(new CharacterAction() { Name = "_overwatch", Caption = "Overwatch", KeyBinding = "2" });
                }
                else
                {
                    options.Add(new CharacterAction() { Name = "_reload", Caption = "Reload", KeyBinding = "1" });
                }
                if (UtilityWeaponEquipped != null)
                {
                    options.Add(new CharacterAction() { Name = "_throw_grenade", Caption = "Throw grenade", KeyBinding = "3" });
                }
                if (UtilityItemEquipped != null && UtilityItemEquipped.Type == ItemType.MedKit && UtilityItemEquipped.ClipRemaining > 0)
                {
                    options.Add(new CharacterAction() { Name = "_use_medkit", Caption = "Use medkit", KeyBinding = "4" });
                }
                if (CoverState.InFullCover || CoverState.InHalfCover)
                {
                    options.Add(new CharacterAction() { Name = "_hunker_down", Caption = "Hunker down", KeyBinding = "5" });
                }
                if (map != null)
                {
                    //Add the found tiles action
                    List<Vector3> foundTiles = MapCore.FindAdjacentTile(map, Location, MapObjectType.ToggleSwitchOn);
                    if (foundTiles != null && foundTiles.Count > 0)
                    {
                        options.Add(new CharacterAction() { Name = "_toggle", Caption = "Toggle switch", KeyBinding = "0" });
                    }
                }
            }

            return options;
        }

        public void UseItem(Item item)
        {
            if (item != null && item.Type == ItemType.MedKit && item.ClipRemaining > 0)
            {
                HitpointsCurrent += item.Adjustment;
                item.ClipRemaining -= 1;
            }
        }

        public string GetCharactersInViewMapString(string[,,] map, List<Character> teamCharacters)
        {
            List<Vector3> fov = FieldOfView.GetFieldOfView(map, Location, ShootingRange);
            string[,,] mapFov = MapCore.ApplyListToMap((string[,,])map.Clone(), fov, "o");
            mapFov[(int)Location.X, (int)Location.Y, (int)Location.Z] = "P";
            foreach (Character character in teamCharacters)
            {
                mapFov[(int)character.Location.X, (int)character.Location.Y, (int)character.Location.Z] = "P";
            }
            string mapString = MapCore.GetMapString(mapFov);

            return mapString;
        }

        private void UpdateCharacterFOV(string[,,] map)
        {
            int xMax = map.GetLength(0);
            int yMax = map.GetLength(1);
            int zMax = map.GetLength(2);

            if (FOVMap == null)
            {
                FOVMap = MapCore.InitializeMap(xMax, yMax, zMax, FieldOfView.FOV_Unknown);
            }
            List<Vector3> fov = FieldOfView.GetFieldOfView(map, Location, FOVRange);
            foreach (Vector3 item in fov)
            {
                FOVHistory.Add(item);
                FOVMap[(int)item.X, (int)item.Y, (int)item.Z] = FieldOfView.FOV_CanSee;
            }
            FOVMap[(int)Location.X, (int)Location.Y, (int)Location.Z] = "P";

            //foreach (Vector3 item in fov)
            //{
            //    FOVHistory.Add(item);
            //}
            //string[,,] inverseMap = MapCore.InitializeMap(xMax, yMax, zMax);
            ////Set the player position to visible
            //inverseMap[(int)Location.X, (int)Location.Y, (int)Location.Z] = "P";
            //////Set the map to all of the visible positions
            ////foreach (Vector3 item in fov)
            ////{
            ////    inverseMap[(int)item.X, (int)item.Y, (int)item.Z] = FieldOfView.FOV_CanNotSee;
            ////}
            ////Now that we have the inverse map, reverse it to show areas that are not visible
            //for (int y = 0; y < 1; y++)
            //{
            //    for (int x = 0; x < xMax; x++)
            //    {
            //        for (int z = 0; z < zMax; z++)
            //        {
            //            if (inverseMap[x, y, z] != "")
            //            {
            //                FOVMap[x, y, z] = FieldOfView.FOV_CanSee;
            //            }
            //            //else if (FOVHistory.Contains(new Vector3(x, y, z)))
            //            //{
            //            //    FOVMap[x, y, z] = FieldOfView.FOV_CanNotSee;
            //            //}
            //        }

            //    }
            //}
        }

        public bool LevelUpCharacter()
        {
            if (LevelUpIsReady)
            {
                LevelUpIsReady = false;
                HitpointsCurrent++;
                HitpointsMax++;
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
