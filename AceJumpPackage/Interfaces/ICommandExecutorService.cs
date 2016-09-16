namespace AceJumpPackage.Interfaces
{
    public interface ICommandExecutorService
    {
        bool IsCommandAvailable(string commandName);
    }
}