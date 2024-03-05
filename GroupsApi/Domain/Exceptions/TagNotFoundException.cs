using Core.Base.Exceptions;

namespace Domain.Exceptions;

public class TagNotFoundException : NotFoundException
{
    public TagNotFoundException(Guid id) : base($"The tag with the identifier {id} was not found.")
    {
    }
}