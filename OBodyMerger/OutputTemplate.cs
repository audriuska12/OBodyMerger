using Mutagen.Bethesda.Skyrim;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Noggog;

namespace OBodyMerger
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class OutputTemplate
    {
        [JsonProperty("npcFormID")]
        public Dictionary<string, Dictionary<string, HashSet<string>>> NpcFormID { get; set; } = new();
        [JsonProperty("npc")]
        public Dictionary<string, HashSet<string>> Npcs { get; set; } = new();
        [JsonProperty("factionFemale")]
        public Dictionary<string, HashSet<string>> FactionFemale { get; set; } = new();
        [JsonProperty("factionMale")]
        public Dictionary<string, HashSet<string>> FactionMale { get; set; } = new();
        [JsonProperty("npcPluginFemale")]
        public Dictionary<string, HashSet<string>> PluginFemale { get; set; } = new();
        [JsonProperty("npcPluginMale")]
        public Dictionary<string, HashSet<string>> PluginMale { get; set; } = new();
        [JsonProperty("raceFemale")]
        public Dictionary<string, HashSet<string>> RaceFemale { get; set; } = new();
        [JsonProperty("raceMale")]
        public Dictionary<string, HashSet<string>> RaceMale { get; set; } = new();
        [JsonProperty("blacklistedNpcsFormID")]
        public Dictionary<string, HashSet<string>> BlacklistFormID { get; set; } = new();
        [JsonProperty("blacklistedNpcs")]
        public HashSet<string> BlacklistNPCs { get; set; } = new();
        [JsonProperty("blacklistedNpcsPluginFemale")]
        public HashSet<string> BlacklistPluginFemale { get; set; } = new();
        [JsonProperty("blacklistedNpcsPluginMale")]
        public HashSet<string> BlacklistPluginMale { get; set; } = new();
        [JsonProperty("blacklistedRacesFemale")]
        public HashSet<string> BlacklistRaceFemale { get; set; } = new();
        [JsonProperty("blacklistedRacesMale")]
        public HashSet<string> BlacklistRaceMale { get; set; } = new();
        [JsonProperty("blacklistedPresetsFromRandomDistribution")]
        public HashSet<string> BlacklistRandom { get; set; } = new();
        [JsonProperty("blacklistedPresetsShowInOBodyMenu")]
        public bool ShowBlacklistedInMenu = true;
        [JsonProperty("blacklistedOutfitsFromORefitFormID")]
        public Dictionary<string, HashSet<string>> ORefitBlacklistFormID { get; set; } = new();
        [JsonProperty("blacklistedOutfitsFromORefit")]
        public HashSet<string> ORefitBlacklistOutfit { get; set; } = new();
        [JsonProperty("blacklistedOutfitsFromORefitPlugin")]
        public HashSet<string> ORefitBlacklistPlugin { get; set; } = new();
        [JsonProperty("outfitsForceRefitFormID")]
        public Dictionary<string, HashSet<string>> ORefitForceFormID { get; set; } = new();
        [JsonProperty("outfitsForceRefit")]
        public HashSet<string> ORefitForceOutfit { get; set; } = new();

        public static OutputTemplate FromFile(string json)
        {
            OutputTemplate outputTemplate = new();
            JObject? data = JsonConvert.DeserializeObject<JObject>(json);
            if (data == null)
            {
                Console.WriteLine("Error parsing JSON");
                return outputTemplate;
            }
            foreach ((string key, Dictionary<string, Dictionary<string, HashSet<string>>> target) in new Dictionary<string, Dictionary<string, Dictionary<string, HashSet<string>>>>{
                { "npcFormID", outputTemplate.NpcFormID },
            })
            {
                JObject? source = (JObject?)data[key];
                if (source != null)
                {
                    ImportThreeLevelList(target, source);
                }
            }
            foreach ((string key, Dictionary<string, HashSet<string>> target) in new Dictionary<string, Dictionary<string, HashSet<string>>>
            {
                { "npc", outputTemplate.Npcs },
                { "factionFemale", outputTemplate.FactionFemale },
                { "factionMale", outputTemplate.FactionMale},
                { "npcPluginFemale", outputTemplate.PluginFemale },
                { "npcPluginMale", outputTemplate.PluginMale },
                { "raceFemale", outputTemplate.RaceFemale },
                { "raceMale", outputTemplate.RaceMale },
                { "blacklistedNpcsFormID", outputTemplate.BlacklistFormID },
                { "blacklistedOutfitsFromORefitFormID", outputTemplate.ORefitBlacklistFormID },
                { "outfitsForceRefitFormID", outputTemplate.ORefitForceFormID },
            })
            {
                JObject? src = (JObject?)data[key];
                if (src != null)
                {
                    ImportTwoLevelList(target, src);
                }
            }
            foreach ((string key, HashSet<string> target) in new Dictionary<string, HashSet<string>> {
                { "blacklistedNpcs", outputTemplate.BlacklistNPCs },
                { "blacklistedNpcsPluginFemale", outputTemplate.BlacklistPluginFemale },
                { "blacklistedNpcsPluginMale", outputTemplate.BlacklistPluginMale },
                { "blacklistedRacesFemale", outputTemplate.BlacklistRaceFemale },
                { "blacklistedRacesMale", outputTemplate.BlacklistRaceMale },
                { "blacklistedPresetsFromRandomDistribution", outputTemplate.BlacklistRandom },
                { "blacklistedOutfitsFromORefit", outputTemplate.ORefitBlacklistOutfit },
                { "blacklistedOutfitsFromORefitPlugin", outputTemplate.ORefitBlacklistPlugin },
                { "outfitsForceRefit", outputTemplate.ORefitForceOutfit },
            })
            {
                JToken? src = data[key];
                if (src != null)
                {
                    ImportOneLevelList(target, src);
                }
            }
            return outputTemplate;
        }
        private static void ImportThreeLevelList(Dictionary<string, Dictionary<string, HashSet<string>>> targetList, JObject source)
        {
            foreach (JProperty? level1 in source.Properties())
            {
                if (!targetList.ContainsKey(level1.Name))
                {
                    targetList[level1.Name] = new();
                }
                JObject? level2 = (JObject?)level1.Value;
                if (level2 == null) continue;
                foreach (JProperty? l2value in level2.Properties())
                {
                    if (!targetList[level1.Name].ContainsKey(l2value.Name))
                    {
                        targetList[level1.Name][l2value.Name] = new();
                    }
                    if (l2value.Value is JArray arr)
                    {
                        foreach (JToken value in arr)
                        {
                            targetList[level1.Name][l2value.Name].Add(value.ToString());
                        }
                    }
                    else
                    {
                        targetList[level1.Name][l2value.Name].Add(l2value.Value.ToString());
                    }
                }
            }
        }

        private static void ImportTwoLevelList(Dictionary<string, HashSet<string>> targetList, JObject source)
        {
            foreach (JProperty? level1 in source.Properties())
            {
                if (!targetList.ContainsKey(level1.Name))
                {
                    targetList[level1.Name] = new();
                }
                if (level1.Value is JArray arr)
                {
                    foreach (JToken value in arr)
                    {
                        targetList[level1.Name].Add(value.ToString());
                    }
                }
                else
                {
                    targetList[level1.Name].Add(level1.Value.ToString());
                }
            }
        }
        private static void ImportOneLevelList(HashSet<string> targetList, JToken source)
        {
            if (source is JArray arr)
            {
                foreach (JToken value in arr)
                {
                    targetList.Add(value.ToString());
                }
            }
            else
            {
                targetList.Add(source.ToString());
            }
        }

        public void AddAll(OutputTemplate other)
        {
            MergeThreeLevelList(NpcFormID, other.NpcFormID);
            MergeTwoLevelList(Npcs, other.Npcs);
            MergeTwoLevelList(FactionFemale, other.FactionFemale);
            MergeTwoLevelList(FactionMale, other.FactionMale);
            MergeTwoLevelList(PluginFemale, other.PluginFemale);
            MergeTwoLevelList(PluginMale, other.PluginMale);
            MergeTwoLevelList(RaceFemale, other.RaceFemale);
            MergeTwoLevelList(RaceMale, other.RaceMale);
            MergeTwoLevelList(BlacklistFormID, other.BlacklistFormID);
            MergeOneLevelList(BlacklistNPCs, other.BlacklistNPCs);
            MergeOneLevelList(BlacklistPluginFemale, other.BlacklistPluginFemale);
            MergeOneLevelList(BlacklistPluginMale, other.BlacklistPluginMale);
            MergeOneLevelList(BlacklistRaceFemale, other.BlacklistRaceFemale);
            MergeOneLevelList(BlacklistRaceMale, other.BlacklistRaceMale);
            MergeOneLevelList(BlacklistRandom, other.BlacklistRandom);
            MergeTwoLevelList(ORefitBlacklistFormID, other.ORefitBlacklistFormID);
            MergeOneLevelList(ORefitBlacklistOutfit, other.ORefitBlacklistOutfit);
            MergeOneLevelList(ORefitBlacklistPlugin, other.ORefitBlacklistPlugin);
            MergeTwoLevelList(ORefitForceFormID, other.ORefitForceFormID);
            MergeOneLevelList(ORefitForceOutfit, other.ORefitForceOutfit);
        }
        private static void MergeThreeLevelList(Dictionary<string, Dictionary<string, HashSet<string>>> target, Dictionary<string, Dictionary<string, HashSet<string>>> source)
        {
            foreach (KeyValuePair<string, Dictionary<string, HashSet<string>>> level1 in source)
            {
                if (!target.ContainsKey(level1.Key))
                {
                    target[level1.Key] = new();
                }
                foreach (KeyValuePair<string, HashSet<string>> level2 in level1.Value)
                {
                    if (!target[level1.Key].ContainsKey(level2.Key))
                    {
                        target[level1.Key][level2.Key] = new();
                    }
                    foreach (string preset in level2.Value)
                    {
                        target[level1.Key][level2.Key].Add(preset);
                    }
                }
            }
        }
        private static void MergeTwoLevelList(Dictionary<string, HashSet<string>> target, Dictionary<string, HashSet<string>> source)
        {
            foreach (var name in source)
            {
                if (!target.ContainsKey(name.Key))
                {
                    target[name.Key] = new();
                }
                foreach (var value in name.Value)
                {
                    target[name.Key].Add(value);
                }
            }
        }

        private static void MergeOneLevelList(HashSet<string> target, HashSet<string> source)
        {
            foreach (string s in source)
            {
                target.Add(s);
            }
        }

        public void AddAll(PresetTemplate preset, Settings settings)
        {
            foreach ((string presetName, PresetData data) in preset.Data)
            {
                AddToThreeLevelList(NpcFormID, data.NPCFormIDs, presetName, settings);
                AddToTwoLevelList(Npcs, data.NPCs, presetName, settings);
                AddToTwoLevelList(FactionFemale, data.FemaleFactions, presetName, settings);
                AddToTwoLevelList(FactionMale, data.MaleFactions, presetName, settings);
                AddToTwoLevelList(PluginFemale, data.FemalePlugins, presetName, settings);
                AddToTwoLevelList(PluginMale, data.MalePlugins, presetName, settings);
                AddToTwoLevelList(RaceFemale, data.FemaleRaces, presetName, settings);
                AddToTwoLevelList(RaceMale, data.MaleRaces, presetName, settings);
                if (data.BlacklistFromRandomDistribution)
                {
                    BlacklistRandom.Add(presetName);
                }
            }
        }

        private static void AddToThreeLevelList(Dictionary<string, Dictionary<string, HashSet<string>>> target, Dictionary<string, HashSet<string>> source, string value, Settings settings)
        {
            foreach ((string l1key, HashSet<string> l1value) in source)
            {
                if (!target.ContainsKey(l1key))
                {
                    target[l1key] = new();
                }
                foreach (string l2key in l1value)
                {
                    foreach (string val in GetAliases(settings, l2key))
                    {
                        if (!target[l1key].ContainsKey(val))
                        {
                            target[l1key][val] = new();
                        }
                        target[l1key][val].Add(value);
                    }
                }
            }
        }

        private static void AddToTwoLevelList(Dictionary<string, HashSet<string>> target, HashSet<string> source, string value, Settings settings)
        {
            foreach (string key in source)
            {
                foreach (string val in GetAliases(settings, key))
                {
                    if (!target.ContainsKey(val))
                    {
                        target[val] = new();
                    }
                    target[val].Add(value);
                }
            }
        }

        private static List<string> GetAliases(Settings settings, string value)
        {
            return settings.Aliases.TryGetValue(value, out List<string>? vals) ? (vals ?? []) : [value];
        }
    }

}