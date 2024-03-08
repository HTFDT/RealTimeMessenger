using Shared.Enums;

namespace Shared.Dto.Messages;

public record GetMessagesRequest
{
    public required FilterMessagesBy FilterBy { get; init; }
    public DateTime? DateFrom { get; init; }
    public DateTime? DateTo { get; init; }
    public long? GroupNumFrom { get; init; }
    public long? GroupNumTo { get; init; }
}