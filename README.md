This is a console application for updating default priority sets for the Hilda plugin.

How to use this:

- Update the json file(s) requiring updates
  - This includes updating the VERSION and APPVERSION (If applicable) on a per set basis
- run `dotnet build`
- run `dotnet run +version` (this updates the patch version)
  - if this is a minor or (GULP) major version update, provide `minor` or `major` after `+version`

Do NOT push directly to the `gh-pages` branch!

## Curated Priority Sets Philosophy

### General

- The target audience is _not_ high end raiders with our curated sets. While hardcore / high end raiders can use the tool, the expectation
  of our curated sets is to help the average player understand how to properly play their job. In a game like XIV where anyone can play any
  job at any time, the average player doesn't retain the information of what their rotation looks like.
- Priority Names
  - Names should call out what they're doing in special circumstances. As many of the same actions can be in a single Priority Set, proper
    naming will help with understanding what it's doing as well as help with debugging.
  - Ninja Example:
    - Suiton (requires Trick Attack Recast to be 0) -> _Suiton (Trick Attack)_
    - Ten (requires Trick Attack Recast to be 0) -> _Ten (Suiton)_
    - Chi (requires Trick Attack Recast to be 0) -> _Chi (Suiton)_
    - Jin (requires Trick Attack Recast to be 0) -> _Jin (Suiton)_
- Don't forget about the lower levels!
  - While it's important to focus the sets on level cap / full job gauge unlocked rotations, this plugin works great on down-leveling
    for Level Capped content
  - Provide "replacements actions" for high level actions
    - Red Mage Example:
      - Veraero III (gained at level 82)
      - Verthunder III (gained at level 82)
      - Veraero
      - Verthunder

### Single Target Sets

- Try to emulate the basic "rotation" as found on [The Balance](https://www.thebalanceffxiv.com/) / [The Balance (Discord)](https://discord.gg/thebalanceffxiv)
- Party buffs are not expected to be in the rotation, as they're situational
- Self-buffs (Damage up, etc.) should be implemented in the following ways
  - Use during "Burst Phase"
    - Utilize Requirements which provide information on these phases (Action Cooldowns, Status Gains, etc.)

### Multi Target Sets

- "Simplify" the Single Target Set
  - Rather than 100% emulating the rotation, focus more on multi-target actions
- Very rarely should a single target action be in a multi target set
- ALL actions in a multi target set should have an "Enemy Count" requirement of at least 2+ enemies
  - If an action doesn't have this, then the action & set window will display at all times essentially making for a bad user experience
