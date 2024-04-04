using Core.Exceptions;

namespace IdentityConnectionLib.Exceptions;

public class UserInfoNotFoundException : NotFoundException
{
    public UserInfoNotFoundException(Guid id) : 
        base($"the info for the user with the identifier '{id}' was not found")
    {
    }
}