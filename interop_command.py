class InteropCommand:
	def __init__(self, name=None, aliases=[], help_message="", show_in_help=True, command=None):
		self.Name = name
		self.Aliases = aliases
		self.HelpMessage = help_message
		self.ShowInHelp = show_in_help
		self.Command = command