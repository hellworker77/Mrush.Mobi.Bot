namespace Domain.Abstraction.Interfaces;

public interface IDetourAntiBotSystem
{
    public Task<string> FindDynamicKeyAsync(string response);

    public Task<string> FindStaticKeyAsync(string dynamicKey);
}