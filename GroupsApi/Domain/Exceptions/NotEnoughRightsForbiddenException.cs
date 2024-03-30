using Core.Exceptions;

namespace Domain.Exceptions;

public class NotEnoughRightsForbiddenException : ForbiddenException
{
    public NotEnoughRightsForbiddenException(Guid memberId, IEnumerable<string> missingRights)
        : base($"Action is not permitted as member with the identifier '{memberId}' has no rights: " +
               $"{string.Join(", ", missingRights.Select(mr => '\'' + mr + '\''))}")
    {
    }
}