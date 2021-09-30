using Dalamud.Game.Command;
using System.Collections.Generic;

namespace DalamudPluginProjectTemplatePython
{
    public class InteropCommandManager
    {
        private IList<dynamic> commands;

        private readonly CommandManager commandManager;

        public InteropCommandManager(CommandManager commandManager)
        {
            this.commandManager = commandManager;
        }

        public void Install(IList<dynamic> commands)
        {
            this.commands = commands;
            foreach (var command in this.commands)
            {
                command.Command = (CommandInfo.HandlerDelegate)command.Command;

                var commandInfo = new CommandInfo(command.Command)
                {
                    HelpMessage = command.HelpMessage,
                    ShowInHelp = command.ShowInHelp,
                };

                this.commandManager.AddHandler(command.Name, commandInfo);

                foreach (var alias in command.Aliases)
                {
                    this.commandManager.AddHandler(alias, commandInfo);
                }
            }
        }

        public void Uninstall()
        {
            foreach (var command in this.commands)
            {
                this.commandManager.RemoveHandler(command.Name);

                foreach (var alias in command.Aliases)
                {
                    this.commandManager.RemoveHandler(alias);
                }
            }
        }
    }
}