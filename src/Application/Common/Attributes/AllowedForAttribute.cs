namespace Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class AllowedForAttribute(params string[] roles) : Attribute
{
    public string[] Roles { get; init; } = roles;
}