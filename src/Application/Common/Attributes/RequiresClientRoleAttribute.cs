namespace Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequiresClientRoleAttribute(params string[] clientRoles) : Attribute
{
    public string[] ClientRoles { get; init; } = clientRoles;
}
