namespace ServiceOrder.Application.Interfaces;

public interface IServiceBase
{
    public bool HasError { get; }
    public Dictionary<string, string[]> Errors { get; }

}
