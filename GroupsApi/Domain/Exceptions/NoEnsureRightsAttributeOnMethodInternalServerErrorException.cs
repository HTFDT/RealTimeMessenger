using Core.Exceptions;

namespace Domain.Exceptions;

public class NoEnsureRightsAttributeOnMethodInternalServerErrorException : InternalServerErrorException
{
    public NoEnsureRightsAttributeOnMethodInternalServerErrorException(string methodName) 
        : base($"Can't ensure roles on the action using '{methodName}'")
    {
    }
}