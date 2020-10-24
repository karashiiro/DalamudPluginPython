using System;
using System.IO;
using System.Reflection;
using Dalamud.Game.Command;
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

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;

            this.config = (Configuration)this.pluginInterface.GetPluginConfig() ?? new Configuration();
            this.config.Initialize(pluginInterface);

            this.engine = Python.CreateEngine(AppDomain.CurrentDomain);

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

        private void Execute(string scriptFile, ScriptScope scope)
        {
            var filePath = GetRelativeFile(scriptFile);
            var newScope = this.engine.ExecuteFile(filePath, scope);

            var command = newScope.GetVariable<CommandInfo.HandlerDelegate>("command_1");
            this.pluginInterface.CommandManager.AddHandler("/example1", new CommandInfo(command));
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
            this.pluginInterface.CommandManager.RemoveHandler("/example1");
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
