This is a console application for updating default priority sets for the Hilda plugin.

How to use this:

- Update the json file(s) requiring updates
- run `dotnet build`
- run `dotnet run +version` (this updates the patch version)
  - if this is a minor or (GULP) major version update, provide `minor` or `major` after `+version`

Do NOT push directly to the `alpha` branch!