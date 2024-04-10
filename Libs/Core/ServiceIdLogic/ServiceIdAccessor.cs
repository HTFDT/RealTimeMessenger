using Core.ServiceIdLogic.Interfaces;

namespace Core.ServiceIdLogic;

/// <summary>
/// синглтон для хранения уникального id микросервиса
/// </summary>
internal class ServiceIdAccessor : IServiceIdAccessor
{
    private readonly Lazy<Guid> _storedId = new(Guid.NewGuid);
    public Guid ServiceId => _storedId.Value;
}