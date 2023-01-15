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

## Notes + Tips

- Weave Window > Active has a 0.5s buffer on it for to not show a weave when you can queue your next ability
- Almost all NIN priorities should check if Mudra is active, to not recommend using it in the middle of a Ninjutsu combo
- BLM
  - Ice phase may have additional recommendations flash based on mana tick timing
  - Weave Window > 0 is used, instead of Weave Window Active, due to the many actions that provide short weave windows between casts

## Rotations Tested

- Tanks
  - WAR (1-60)
  - DRK (1-60)
  - PLD (1-60)
  - GNB (1-60)

- Healers
  - All (1-90) (was hard)

- Magic DPS
  - SMN (1-60, 90)
  - BLM (1-60)
    - very non optimal
  - RDM (1-60, 80-90)

- Ranged DPS
  - DNC (1-60, 80)
  - BRD (1-60)
  - MCH (1-60)

- Melee DPS
  - SAM (1-60)
  - DRG (1-75)
  - NIN (1-60)
  - RPR (1-70)
  - MNK (1-60)

## BLU Support

By default, Hilda does not have full or well-tested predictive support of BLU actions, so we encourage the community to fill in the gaps here.

A default BLU rotation is also difficult to provide given the abilities players have access and what is currently selected for the encounter, so we recommend hitting our [Discord server](https://discord.gg/JzrMWYT7ay) for sharing of BLU specific rotations.

### Adding BLU actions

Hilda requires more information on certain actions for predictive support on upcoming abilities and other minor features. This mostly impacts abilities that place status effects on the player or target.

If you find a BLU spell that doesn't have good predictions, here are the steps to add support:

1. Open the `BLU Action Definitions` JSON file in the BLU job folder (`priorities/36/actions.json`)
2. Add an entry for the action that needs support
3. Fill in the schema as needed. Everything other than the `id`s are optional and can be ommitted.

```json
{
  "id": integer, // action id -- REQUIRED!
  "detrimentalStatusEffects": Status, // the debuff put on the target
  "detrimentalStatusEffects": [ Status ], // a list of debuffs; do not use both this and detrimentalStatusEffects,
  "grantsStatusEffect": Status, // the buff put on the player
  "grantsStatusEffects": [ Status ], // a list of buffs; do not use both this and grantsStatusEffect
  "requiresStatusEffect": Status, // use if this action requires a specific buff to be usable
  "removesTriggeringStatusId": boolean, // set to true if the requiresStatusEffect status gets consumed on this action's use,
  "sharesRecastActionIds": [ integer ], // a list of other action IDs that this action shares a recast with
  "baseCharges": integer, // the number of charges this action has when first acquired
  "isWeavable": boolena, // set to true if Hilda does not recognize the proper weave status by default
  "grantsManaPoints": integer // mana points this action grants when used
}
```

A Status schema is defined as:

```json
{
    "id": integer, // status id -- REQUIRED
    "remainingTime": decimal, // the length of time, in seconds, the status initially lasts
    "isPermanent": boolean, // true if this status does not have a set duration
    "stackCount": integer, // only needed if multiple stacks of the status are granted by the action
}
```

If an ability has no special properties, you can remove the in-game exclamation mark for the ability as being "unsupported" by adding an empty definition such as:

```json
{
  "name": "Sonic Boom",
  "id": 18308
},
```

You can test these changes in-game locally by editing the definitions in your `%AppData%\XIVLauncher\installedPlugins\Hilda\<current Hilda version>\Priorities\36\actions.json` (or appropriate matching install location for XIVLauncher). Then disable and re-enable Hilda in-game to load the changes.

Once validated, push a new branch with your changes in this repo (and follow the steps for versioning at the top of this file).

> Warning! This file updates automatically as this repo is updated. If you're actively testing action definitions in this folder, keep a backup.
