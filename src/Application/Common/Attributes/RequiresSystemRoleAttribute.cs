namespace Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequiresSystemRoleAttribute(params string[] systemRoles) : Attribute
{
    public string[] SystemRoles { get; init; } = systemRoles;
}
