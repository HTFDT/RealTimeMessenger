namespace Core.TraceLogic.Interfaces;

public interface ITraceReader
{
    string Name { get; }
    string? ReadValue();
}