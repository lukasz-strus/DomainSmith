using DomainSmith.Abstraction.Core.Primitives;

namespace DomainSmith.Entity.Examples.Entities;

public record OwnerId(Guid Value) : EntityIdRecord<Guid>(Value);

public record Address(string Street, string City, string State, string ZipCode);

[Entity(typeof(OwnerId))]
public partial class Owner
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public Address? Address { get; private set; }

    static partial void OnCreating(ref string firstname, ref string lastname, ref string email, ref Address? address)
    {
        firstname += " (Created)";
        lastname += " (Created)";
        email += " (Created)";
        address ??= new Address("Default Street", "Default City", "Default State", "00000");
    }

    partial void OnUpdating(ref string firstname, ref string lastname, ref string email, ref Address? address)
    {
        firstname += " (Updated)";
        lastname += " (Updated)";
        email += " (Updated)";
        address ??= new Address("Updated Street", "Updated City", "Updated State", "11111");
    }
}