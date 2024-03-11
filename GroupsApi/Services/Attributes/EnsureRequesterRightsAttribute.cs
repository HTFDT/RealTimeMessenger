namespace Services.Attributes;

internal class EnsureRequesterRightsAttribute(params string[] rightNames) : Attribute
{
    public string[] RightsNames { get; } = rightNames;
}