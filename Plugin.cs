using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Dalamud.Game.ClientState;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace DalamudPluginProjectTemplatePython
{
    public class Plugin : IDalamudPlugin
    {
        [PluginService]
        [RequiredVersion("1.0")]
        private DalamudPluginInterface PluginInterface { get; init; }

        [PluginService]
        [RequiredVersion("1.0")]
        private CommandManager Commands { get; init; }

        [PluginService]
        [RequiredVersion("1.0")]
        private ChatGui Chat { get; init; }

        [PluginService]
        [RequiredVersion("1.0")]
        private ClientState ClientState { get; init; }

        public string Name => "Your Plugin's Display Name";

        private readonly ScriptEngine engine;

        private readonly Configuration config;
        private readonly InteropCommandManager commandManager;

        public Plugin()
        {
            this.config = (Configuration)PluginInterface.GetPluginConfig() ?? new Configuration();
            this.config.Initialize(PluginInterface);

            this.engine = Python.CreateEngine();
            ConfigureSearchPaths();

            this.commandManager = new InteropCommandManager(Commands);

            var scriptScope = ConfigureScope();
            Execute("plugin.py", scriptScope);
        }

        private ScriptScope ConfigureScope()
        {
            var scope = this.engine.CreateScope();

            scope.SetVariable("Configuration", this.config);
            scope.SetVariable("PluginInterface", PluginInterface);
            scope.SetVariable("Chat", Chat);
            scope.SetVariable("ClientState", ClientState);

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
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
