using System.Runtime.InteropServices;

namespace AceJumpPackage.Interfaces
{
    public interface ICommandExecutorService
    {
        /// <summary>
        /// See, if <paramref name="commandName"/> is available. 
        /// </summary>
        /// <param name="commandName">name of the command</param>
        /// <returns><c>true</c> if command exists, else <c>false</c></returns>
        bool IsCommandAvailable(string commandName);

        /// <summary>
        /// Executes <paramref name="commandName"/>
        /// </summary>
        /// <param name="commandName">name of the command that shall be executed</param>
        /// <exception cref="COMException">thrown if the command name is not valid</exception>
        void Execute(string commandName);
    }
}