using Core.Jwt.Interfaces;

namespace Core.Jwt;

internal class JwtTokenAccessor : IJwtTokenAccessor
{
    public string Name => "Authorization";

    private string _value = null!;
    
    public string ReadValue()
    {
        return _value;
    }

    public void WriteValue(string value)
    {
        _value = value;
    }
}