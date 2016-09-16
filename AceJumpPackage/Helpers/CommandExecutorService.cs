using AceJumpPackage.Interfaces;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace AceJumpPackage.Helpers
{
    public class CommandExecutorService : ICommandExecutorService
    {
        readonly DTE _dte;

        public CommandExecutorService()
        {
            _dte = Package.GetGlobalService(typeof(SDTE)) as DTE;
        }
        public bool IsCommandAvailable(string commandName)
        {
            return findCommand(_dte.Commands, commandName) != null;
        }

        private dynamic findCommand(Commands commands, string commandName)
        {
            foreach (var command in commands)
            {
                if (((dynamic)command).Name == commandName)
                    return command;
            }

            return null;
        }
    }
}