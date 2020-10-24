using System;
using System.Collections.Generic;
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

        private ScriptEngine engine;

        private Configuration config;
        private DalamudPluginInterface pluginInterface;
        private InteropCommandManager commandManager;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;

            this.config = (Configuration)this.pluginInterface.GetPluginConfig() ?? new Configuration();
            this.config.Initialize(pluginInterface);

            this.engine = Python.CreateEngine(AppDomain.CurrentDomain);
            ConfigureSearchPaths();

            this.commandManager = new InteropCommandManager(pluginInterface);

            var scriptScope = ConfigureScope();
            Execute("plugin.py", scriptScope);
        }

        private ScriptScope ConfigureScope()
        {
            var scope = this.engine.CreateScope();

            scope.SetVariable("Configuration", this.config);
            scope.SetVariable("PluginInterface", this.pluginInterface);

            return scope;
        }

        private void ConfigureSearchPaths()
        {
            var dir = Path.Combine(Assembly.GetExecutingAssembly().Location, "..");
            var paths = this.engine.GetSearchPaths();
            paths.Add(dir);
            this.engine.SetSearchPaths(paths);
        }

        private void Execute(string scriptFile, ScriptScope scope)
        {
            var filePath = GetRelativeFile(scriptFile);
            this.engine.ExecuteFile(filePath, scope);

            IList<dynamic> commands = scope.GetVariable("commands");
            this.commandManager.Install(commands);
        }

        private static string GetRelativeFile(string fileName)
        {
            return Path.Combine(Assembly.GetExecutingAssembly().Location, "..", fileName);
        }
        
        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            this.config.Save();
            this.commandManager.Uninstall();
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
