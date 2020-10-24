using System.Collections.Generic;
using Dalamud.Configuration;
using Dalamud.Plugin;
using Newtonsoft.Json;

namespace DalamudPluginProjectTemplatePython
{
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; }

        [JsonProperty]
        private IDictionary<string, string> Properties { get; set; }

        [JsonIgnore] private DalamudPluginInterface pluginInterface;

        // ReSharper disable once UnusedMember.Global
        public void SetProperty(string key, string value)
        {
            Properties[key] = value;
            Save();
        }

        // ReSharper disable once UnusedMember.Global
        public void DeleteProperty(string key)
        {
            Properties.Remove(key);
            Save();
        }

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;

            Properties ??= new Dictionary<string, string>();
        }

        public void Save()
        {
            this.pluginInterface.SavePluginConfig(this);
        }
    }
}