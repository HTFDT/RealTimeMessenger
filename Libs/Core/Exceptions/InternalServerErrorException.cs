namespace Core.Exceptions;

public class InternalServerErrorException : Exception
{
    protected InternalServerErrorException(string message) : base(message)
    {
    }
}