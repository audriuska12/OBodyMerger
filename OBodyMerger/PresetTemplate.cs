using Newtonsoft.Json;
using System.Xml;

namespace OBodyMerger
{
    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class PresetTemplate
    {
        [JsonProperty("presetData")]
        public Dictionary<string, PresetData> Data { get; set; } = new Dictionary<string, PresetData>();
        public static PresetTemplate FromXml(XmlDocument xmlDocument)
        {
            PresetTemplate template = new PresetTemplate();
            XmlElement? presets = xmlDocument["SliderPresets"];
            if (presets != null)
            {
                foreach (XmlNode preset in presets.ChildNodes)
                {
                    template.Data[preset.Attributes?["name"]?.Value ?? throw new Exception("Preset has no name!")] = new PresetData();
                }
            }
            return template;
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptOut)]
    public class PresetData
    {
        [JsonProperty("npcFormIDs")]
        public Dictionary<string, HashSet<string>> NPCFormIDs { get; set; } = new();
        [JsonProperty("npcs")]
        public HashSet<string> NPCs { get; set; } = new();
        [JsonProperty("factionFemale")]
        public HashSet<string> FemaleFactions { get; set; } = new();
        [JsonProperty("factionMale")]
        public HashSet<string> MaleFactions { get; set; } = new();
        [JsonProperty("pluginFemale")]
        public HashSet<string> FemalePlugins { get; set; } = new();
        [JsonProperty("pluginMale")]
        public HashSet<string> MalePlugins { get; set; } = new();
        [JsonProperty("raceFemale")]
        public HashSet<string> FemaleRaces { get; set; } = new();
        [JsonProperty("raceMale")]
        public HashSet<string> MaleRaces { get; set; } = new();
        [JsonProperty("blacklistFromRandomDistribution")]
        public bool BlacklistFromRandomDistribution = false;
    }
}