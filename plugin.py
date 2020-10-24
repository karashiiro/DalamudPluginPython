import clr
clr.AddReference("System.Collections")
clr.AddReference("Dalamud")

from System.Collections.Generic import List
from Dalamud.Game.Command import CommandInfo
from Dalamud.Plugin import PluginLog

class InteropCommand:
	def __init__(self, name=None, aliases=[], help_message="", show_in_help=True, command=None):
		self.Name = name
		self.Aliases = aliases
		self.HelpMessage = help_message
		self.ShowInHelp = show_in_help
		self.Command = command

def example_command_1(command, args):
	chat = PluginInterface.Framework.Gui.Chat
	world = PluginInterface.ClientState.LocalPlayer.CurrentWorld.GameData
	chat.Print("Hello " + world.Name + "!")
command1 = InteropCommand(
	name="/example1",
	aliases=["/ex1"],
	help_message="Example help message.",
	command=example_command_1)

commands = List[object]([command1])