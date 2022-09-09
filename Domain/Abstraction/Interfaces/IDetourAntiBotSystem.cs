namespace Domain.Abstraction.Interfaces;

public interface IDetourAntiBotSystem
{
    public Task<string> FindExternalKeyAsync(string response);

    public Task<string> FindInternalKeyAsync(string externalKey);
}