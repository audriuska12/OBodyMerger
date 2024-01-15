**OBodyMerger**

Patcher to merge multiple [OBody NG](https://www.nexusmods.com/skyrimspecialedition/mods/77016) distribution presets into a single list, and add support for per-preset distribution rules.

Features:
* Merge multiple OBody presets into a single file. Useful for, say, keeping your personal changes safe from accidental overwrites between versions, or merging mods like [ORefit Master List](https://www.nexusmods.com/skyrimspecialedition/mods/105052) into your setup without having to do nearly as much manual work. The preset JSONs must be in Data/SKSE/Plugins/OBodyTemplate, following the same formatting rules as the actual config file.
* Create per-preset distribution rules. Files must be located in SKSE/Plugins/OBodyPresets. Sample at the bottom of the readme.
* Create templates for per-preset distribution rules. This will scan your BodySlide presets folder and generate a .json in the OBodyPresets folder for each file and preset. Existing files will *not* be overwritten.

Preset distribution sample (don't *actually* distribute the same preset to males and females):
```
{
	"presetName": {
		"npcFormIDs": {
			"mod1.esp": [
				"FormID1",
				"FormID2"
			]
		},
		"npcs": [
			"Serana",
			"Valerica"
		],
		"factionFemale": [
			"SolitudeBardsCollegeFaction"
		],
		"factionMale": [
			"CompanionsCircle"
		],
		"pluginFemale": [
			"Mod2.esp",
			"Mod3.esp"
		],
		"pluginMale": [
			"Mod2.esp",
			"Mod3.esp"
		],
		"raceFemale": [
			"HighElfRace",
			"HighElfRaceVampire"
		],
		"raceMale": [
			"DarkElfRace",
			"DarkElfRaceVampire"
		],
		"blacklistFromRandomDistribution": false
	}
}
```

**Compatibility**
Should remain compatible with OBody NG until they change their file structure. If they do and I'm not around to fix it, see next section...

**Permissions**
The code's open. Use and modify as you see fit, but I'd appreciate the credit if you do base other work on it.
