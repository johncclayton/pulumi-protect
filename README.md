# pulumi-protect

Protect against accidental stack destroy commands when multiple referenced stacks are in use.

# Running

This tool acts as a wrapper around your pulumi CLI command.  The only interception it makes 
is when the "destroy" command is issued, at which point it will check to see if the stack
being destroyed is referenced by any other stacks.  If it is, it will prompt you to confirm
that you really want to destroy the stack.

pulumi-protect can also delete dependant stacks, making it much easier to clean up chains
of stacks that are no longer needed.  This is done by passing the `--delete-dependants` flag
to the command.

# Installation

Install this command as a dotnet global tool, then run it instead of the pulumi command using exactly
the same paramters you would provide to the pulumi CLI command. 



