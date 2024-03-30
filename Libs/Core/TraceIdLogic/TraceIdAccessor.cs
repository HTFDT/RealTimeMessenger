using Core.TraceIdLogic.Interfaces;
using Core.TraceLogic.Interfaces;

namespace Core.TraceIdLogic;

internal class TraceIdAccessor : ITraceIdAccessor
{
    public string Name => "TraceId";

    private string? _value;
    
    public string? ReadValue()
    {
        return _value;
    }

    public void WriteValue(string value)
    {
        _value = value;
    }
}