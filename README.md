# pulumi-protect

Protect against accidental stack destroy commands when multiple referenced stacks are in use.

# What

This tool acts as a wrapper around your pulumi CLI command.  The only interception it makes 
is when the "destroy" command is issued, at which point it will check to see if the stack
being destroyed is referenced by any other stacks.  

pulumi-protect can also delete dependant stacks, making it much easier to clean up chains
of stacks that are no longer needed.  This is done by passing the `--delete-dependants` flag
to the command.

Given that all ops are destructive, the only this pulumi-protect does is to print out
the commands that it *would* execute.  If you are happy with the commands, you can then
respond with "y/n" to execute exactly these commands.

# Installation

Install this command as a dotnet global tool, then run it instead of the pulumi command using exactly
the same parameters you would provide to the pulumi CLI command, and it will validate 

# Authentication

You should export an environment variable with your Pulumi Access Token, called: ``PULUMI_ACCESS_TOKEN``



