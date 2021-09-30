import clr
clr.AddReference("System.Collections")
clr.AddReference("Dalamud")

from System.Collections.Generic import List
from Dalamud.Game.Command import CommandInfo
from Dalamud.Plugin import PluginLog

from interop_command import InteropCommand

# Configuration can be set with Configuration.SetProperty(key: str, value: str)
# and accessed with Configuration.GetProperty(key: str): str.

def example_command_1(command, args):
	world = ClientState.LocalPlayer.CurrentWorld.GameData
	Chat.Print("Hello, " + world.Name + "!")
command1 = InteropCommand(
	name="/example1",
	aliases=["/ex1"],
	help_message="Example help message.",
	command=example_command_1)

commands = List[object]([command1])