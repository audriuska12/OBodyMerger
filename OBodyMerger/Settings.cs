namespace OBodyMerger
{
    public class Settings
    {
        public Dictionary<string, List<string>> Aliases = new();
        public bool PreserveExistingConfig { get; set; } = true;
        public bool GenerateJSONForTemplates { get; set; } = false;
        public bool ShowBlacklistedPresetsInRefitMenu { get; set; } = true;
    }
}