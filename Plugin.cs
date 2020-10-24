using System;
using System.IO;
using System.Reflection;
using Dalamud.Plugin;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace DalamudPluginProjectTemplatePython
{
    public class Plugin : IDalamudPlugin
    {
        public string Name => "Your Plugin's Display Name";

        private ScriptRuntime runtime;

        private Configuration config;
        private DalamudPluginInterface pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;
            this.config = (Configuration)this.pluginInterface.GetPluginConfig() ?? new Configuration();
            this.config.Initialize(pluginInterface);

            this.runtime = Python.CreateRuntime(AppDomain.CurrentDomain);
            
            Execute("plugin.py");
        }

        private void Execute(string scriptFile)
        {
            var filePath = GetRelativeFile(scriptFile);
            this.runtime.ExecuteFile(filePath);
        }

        private static string GetRelativeFile(string fileName)
        {
            return Path.Combine(Assembly.GetExecutingAssembly().Location, "..", fileName);
        }
        
        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            this.runtime.Shutdown();
            this.config.Save();
            this.pluginInterface.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
