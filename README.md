# Battle
A POC to build the battle logic for a turn based game idea, similar to [XCOM](https://en.wikipedia.org/wiki/XCOM) and [Jagged Alliance](https://en.wikipedia.org/wiki/Jagged_Alliance_(series)). 

[![.NET Build](https://github.com/samsmithnz/Battle/actions/workflows/dotnet.yml/badge.svg)](https://github.com/samsmithnz/Battle/actions/workflows/dotnet.yml)
[![Coverage Status](https://coveralls.io/repos/github/samsmithnz/Battle/badge.svg?branch=main)](https://coveralls.io/github/samsmithnz/Battle?branch=main)
![Current Release](https://img.shields.io/github/release/samsmithnz/Battle/all.svg)

### Current features include:
- Basic map generation, in 2d world, with x, y, and z axis (note that y axis/height is not multi-level, just different heights - e.g. a hill, not a multi-level building)
- Basic pathfinding, movement and identification of possible movement tiles
- Basic attacks between multiple characters, with weapons, special abilities, and range taken into account
- Basic high and low cover, with cover destruction
- Grenades/area effects 
- Basic field of view
- Reactions/ overwatch
- A very basic AI, that attempts to move, flank and attack a character
- Basic mission objectives, that can be combined, including: eliminate all, extract troops, toggle switch

### Current workflow
1. Start team turn
2. Player with action points and positive hit points selected
3. Player calculates field of view
4. Player calculates possible movement tiles
5. Player calculates opponenents in view
6. Player selects first opponent (if in view) as their current target
7. Camera centers over player
8. Player decides to move, switch target (if in view), or shoot (if in view) 
    1. If moving, moves to selected square. As moving, if player passes a character in view with overwatch, they are attacked
    2. If attacking, the player first goes into "aim" mode, where they can select which target in view to attack
    3. Once selected, the player attacks, either missing, or hitting and dealing damage to opponents and/or the environment
9. If the player still has action points left, go to 2.
10. If player does not have action points, move to next player on team
11. If no players remain with action points, move to the next team
12. If the team has no players with hit points remaining, the mission is over

## How to install/use
- Requires .NET 6 (for tests)
- Battle.Logic dll is .NET Standard 2.0 for Unity compatibility
- Codespaces is configured, or you can continue to use Visual Studio
- See tests & scenarios for examples of how to utilize the battle.logic

## Contributions
- Contributions welcome! Fork/Issue/Pull Request. Whatever you are comfortable suggesting.
