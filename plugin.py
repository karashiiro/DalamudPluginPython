def example_command_1(command, args):
	chat = PluginInterface.Framework.Gui.Chat
	world = PluginInterface.ClientState.LocalPlayer.CurrentWorld.GameData
	chat.Print("Hello " + world.Name + "!")

command_1 = example_command_1